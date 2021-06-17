namespace FocusWarden.Lib.ViewModels
{
    using Common.Enumerators;
    using DataAccess.Domain.FocusSessions.Command;
    using DataAccess.Domain.FocusSessions.Query;
    using DataAccess.Domain.SessionDuration.Command;
    using DataAccess.Domain.SessionDuration.Query;
    using DataAccess.Domain.TodoItems.Command;
    using DataAccess.Domain.TodoItems.Query;
    using DataAccess.Models;
    using Helpers;
    using Helpers.Interfaces;
    using MediatR;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using Notifications;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Media;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    public class SessionViewModel : ObservableObject
    {
        private static readonly Random Random = new();

        public SessionViewModel(IToastNotification notification, IMediator mediator)
        {
            this.mediator = mediator;
            this.notification = notification;

            maxSessionDuration = TimeSpan.FromMinutes(55);
            minSessionDuration = TimeSpan.FromMinutes(5);

            motivationSentenceProvider = new MotivationSentencesProvider();

            SessionDuration = mediator.Send(new GetSessionDurationQuery()).Result;

            timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            timer.Tick += Tick;

            MarkTodoItemAsDoneCommand = new AsyncRelayCommand<object>(MarkTodoItemAsDoneAsync);
            SaveTodoItemCommand = new AsyncRelayCommand<object>(SaveTodoItemAsync);
            RemoveTodoItemCommand = new AsyncRelayCommand<object>(RemoveTodoItemAsync);
            OpenCreateTodoItemCommand = new RelayCommand(OpenCreateTodoItemPopup);
            OpenEditTodoItemCommand = new RelayCommand<object>(OpenEditTodoItemPopup);
            StartTimerCommand = new AsyncRelayCommand(StartTimerAsync);
            StopTimerCommand = new RelayCommand(StopTimer);
            IncreaseSessionTimeCommand = new AsyncRelayCommand(IncreaseSessionTimeAsync, CanIncreaseSessionTIme);
            DecreaseSessionTimeCommand = new AsyncRelayCommand(DecreaseSessionTimeAsync, CanDecreaseSessionTIme);
            RemoveFocusSessionCommand = new AsyncRelayCommand<object>(RemoveFocusSessionAsync, CanRemoveFocusSession);

            GetFocusSessionsTask = mediator.Send(new GetFocusSessionsQuery {Date = DateTime.Now});
            GetTodoItemsTask = mediator.Send(new GetTodoItemsQuery());
        }

        private bool CanRemoveFocusSession(object obj)
        {
            if (obj is not FocusSession session)
            {
                return false;
            }

            return !(SessionActive && DailyFocusSessions.IndexOf(session) == DailyFocusSessions.Count - 1);
        }

        private async Task RemoveFocusSessionAsync(object obj)
        {
            if (obj is not FocusSession session)
            {
                return;
            }

            await mediator.Send(new RemoveFocusSessionCommand {Id = session.Id});
            GetFocusSessionsTask = mediator.Send(new GetFocusSessionsQuery {Date = DateTime.Now});
        }

        private bool CanDecreaseSessionTIme()
        {
            return !SessionActive && SessionDuration > minSessionDuration;
        }

        private bool CanIncreaseSessionTIme()
        {
            return !SessionActive && SessionDuration < maxSessionDuration;
        }

        private async Task IncreaseSessionTimeAsync()
        {
            SessionDuration =
                await mediator.Send(new ChangeSessionDurationCommand {Type = AtomicOperationType.Increase});
            RefreshIncreaseDecreaseSessionTimeButtonsState();
        }

        private async Task DecreaseSessionTimeAsync()
        {
            SessionDuration =
                await mediator.Send(new ChangeSessionDurationCommand {Type = AtomicOperationType.Decrease});
            RefreshIncreaseDecreaseSessionTimeButtonsState();
        }

        private void OpenCreateTodoItemPopup()
        {
            EditedToDoItem = null;
            IsTodoItemPopupOpen = true;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TodoItemPopupText)));
        }

        private void OpenEditTodoItemPopup(object obj)
        {
            EditedToDoItem = (TodoItem)obj;
            IsTodoItemPopupOpen = true;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TodoItemPopupText)));
        }

        private async Task RemoveTodoItemAsync(object obj)
        {
            var todoItem = obj as TodoItem;
            await mediator.Send(new RemoveTodoItemCommand {Id = todoItem?.Id});
            GetTodoItemsTask = mediator.Send(new GetTodoItemsQuery());
        }

        private async Task MarkTodoItemAsDoneAsync(object obj)
        {
            var todoItem = obj as TodoItem;
            await mediator.Send(new UpdateTodoItemCommand {Id = todoItem?.Id, IsDone = todoItem is {IsDone: false}});
            GetTodoItemsTask = mediator.Send(new GetTodoItemsQuery());
        }

        private async Task SaveTodoItemAsync(object obj)
        {
            var title = obj as string;

            if (EditedToDoItem != null)
            {
                await mediator.Send(new UpdateTodoItemCommand
                {
                    Id = EditedToDoItem.Id, Title = title, IsDone = EditedToDoItem.IsDone
                });

                EditedToDoItem = null;
            }
            else
            {
                await mediator.Send(new AddTodoItemCommand {Title = title});
            }

            GetTodoItemsTask = mediator.Send(new GetTodoItemsQuery());
        }

        private void StopTimer()
        {
            SessionActive = false;
            timer.Stop();

            Time = sessionDuration;
            RefreshIncreaseDecreaseSessionTimeButtonsState();
        }

        private async Task StartTimerAsync()
        {
            SessionActive = true;
            Time = await mediator.Send(new GetSessionDurationQuery());
            timer.Start();

            RefreshMotivationSentence();

            currentFocusSessionId = await mediator.Send(new AddFocusSessionCommand());
            GetFocusSessionsTask = mediator.Send(new GetFocusSessionsQuery {Date = DateTime.Now});

            RefreshIncreaseDecreaseSessionTimeButtonsState();
        }

        private void RefreshIncreaseDecreaseSessionTimeButtonsState()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() =>
                {
                    IncreaseSessionTimeCommand.NotifyCanExecuteChanged();
                    DecreaseSessionTimeCommand.NotifyCanExecuteChanged();
                }));
        }

        private void RefreshMotivationSentence()
        {
            var motivationSentences = motivationSentenceProvider.GetMotivationSentences().ToArray();
            var sentenceIndex = Random.Next(motivationSentences.Length);
            MotivationSentence = motivationSentences.ElementAt(sentenceIndex);
        }

        private async void Tick(object sender, EventArgs e)
        {
            Time = time.Subtract(TimeSpan.FromSeconds(1));

            if (Time.Seconds == 0)
            {
                await mediator.Send(
                    new UpdateFocusSessionCommand
                    {
                        Id = currentFocusSessionId, FocusTime = sessionDuration.Subtract(Time)
                    });
            }

            if (!(Time.TotalSeconds <= 0))
            {
                return;
            }

            StopTimer();

            Application.Current.Dispatcher.Invoke(() =>
                notification.ShowSuccess("Your session is over. Good job :)"));

            await mediator.Send(
                new UpdateFocusSessionCommand
                {
                    Id = currentFocusSessionId, FocusTime = sessionDuration, IsCompleted = true
                });

            GetFocusSessionsTask = mediator.Send(new GetFocusSessionsQuery {Date = DateTime.Now});

            if (Application.Current?.MainWindow?.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                Application.Current.MainWindow.Topmost = true;
            }

            var player = new SoundPlayer("Sounds/TimesUp.wav"); //TODO: Move to distinct class

            player.Load();
            player.Play();
        }

        #region Fields

        private readonly IMotivationSentenceProvider motivationSentenceProvider;

        private readonly IToastNotification notification;

        private readonly IMediator mediator;

        private readonly TimeSpan maxSessionDuration;

        private readonly TimeSpan minSessionDuration;

        private readonly DispatcherTimer timer;

        private string currentFocusSessionId;

        #endregion

        #region Properties

        private TimeSpan time;

        public TimeSpan Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        private TimeSpan sessionDuration;

        public TimeSpan SessionDuration
        {
            get => sessionDuration;
            set
            {
                SetProperty(ref sessionDuration, value);
                Time = value;
            }
        }

        private bool sessionActive;

        public bool SessionActive
        {
            get => sessionActive;
            set => SetProperty(ref sessionActive, value);
        }

        private bool isTodoItemPopupOpen;

        public bool IsTodoItemPopupOpen
        {
            get => isTodoItemPopupOpen;
            set => SetProperty(ref isTodoItemPopupOpen, value);
        }

        private string motivationSentence;

        public string MotivationSentence
        {
            get => motivationSentence;
            set => SetProperty(ref motivationSentence, value);
        }

        private TaskNotifier<IEnumerable<FocusSession>> getFocusSessionsTask;

        private Task<IEnumerable<FocusSession>> GetFocusSessionsTask
        {
            get => getFocusSessionsTask;
            set => SetPropertyAndNotifyOnCompletion(ref getFocusSessionsTask, value,
                _ => OnPropertyChanged(nameof(DailyFocusSessions)));
        }

        public List<FocusSession> DailyFocusSessions => GetFocusSessionsTask.Status == TaskStatus.RanToCompletion
            ? new List<FocusSession>(GetFocusSessionsTask.Result)
            : null;

        private TaskNotifier<IEnumerable<TodoItem>> getTodoItemsTask;

        private Task<IEnumerable<TodoItem>> GetTodoItemsTask
        {
            get => getTodoItemsTask;
            set => SetPropertyAndNotifyOnCompletion(ref getTodoItemsTask, value,
                _ => OnPropertyChanged(nameof(TodoItems)));
        }

        public List<TodoItem> TodoItems => GetTodoItemsTask.Status == TaskStatus.RanToCompletion
            ? new List<TodoItem>(GetTodoItemsTask.Result)
            : null;

        public string TodoItemPopupText => EditedToDoItem?.Title ?? string.Empty;

        private TodoItem editedToDoItem;

        public TodoItem EditedToDoItem
        {
            get => editedToDoItem;
            set => SetProperty(ref editedToDoItem, value);
        }

        #endregion

        #region Commands

        public IAsyncRelayCommand StartTimerCommand { get; }
        public IRelayCommand StopTimerCommand { get; }
        public IAsyncRelayCommand IncreaseSessionTimeCommand { get; }
        public IAsyncRelayCommand DecreaseSessionTimeCommand { get; }
        public IAsyncRelayCommand<object> MarkTodoItemAsDoneCommand { get; }
        public IAsyncRelayCommand<object> SaveTodoItemCommand { get; }
        public IAsyncRelayCommand<object> RemoveTodoItemCommand { get; }
        public IAsyncRelayCommand<object> RemoveFocusSessionCommand { get; }
        public IRelayCommand OpenCreateTodoItemCommand { get; }
        public IRelayCommand<object> OpenEditTodoItemCommand { get; }

        #endregion
    }
}