using FocusWarden.DataAccess.Domain.FilterSettings.Query;
using FocusWarden.DataAccess.Domain.FocusSessions.Command;
using FocusWarden.DataAccess.Domain.FocusSessions.Query;
using FocusWarden.DataAccess.Domain.SessionDuration.Command;
using FocusWarden.DataAccess.Domain.SessionDuration.Query;
using FocusWarden.DataAccess.Domain.TodoItems.Command;
using FocusWarden.DataAccess.Domain.TodoItems.Query;
using FocusWarden.DataAccess.Models;
using FocusWarden.Lib.Notifications;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using FocusWarden.Lib.Helpers;
using FocusWarden.Lib.Helpers.Interfaces;

namespace FocusWarden.Lib.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private static readonly Random Random = new Random();

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
            set
            {
                time = value;
                RaisePropertyChanged();
            }
        }

        private TimeSpan sessionDuration;
        public TimeSpan SessionDuration
        {
            get => sessionDuration;
            set
            {
                if (sessionDuration == value) return;
                sessionDuration = value;
                Time = value;
                RaisePropertyChanged();
            }
        }

        private bool sessionActive;
        public bool SessionActive
        {
            get => sessionActive;
            set
            {
                if (sessionActive == value) return;
                sessionActive = value;
                RaisePropertyChanged();
            }
        }

        private bool isCreateTodoItemItemPopupOpen;
        public bool IsTodoItemPopupOpen
        {
            get => isCreateTodoItemItemPopupOpen;
            set
            {
                if (isCreateTodoItemItemPopupOpen == value) return;
                isCreateTodoItemItemPopupOpen = value;
                RaisePropertyChanged();
            }
        }

        private string motivationSentence;
        public string MotivationSentence
        {
            get => motivationSentence;
            set
            {
                if (motivationSentence == value) return;
                motivationSentence = value;
                RaisePropertyChanged();
            }
        }

        public List<FocusSession> DailyFocusSessions
        {
            get
            {
                var focusSessions = mediator.Send(new GetFocusSessionsQuery() { Date = DateTime.Now }).Result;
                return focusSessions.ToList();
            }
        }

        public List<TodoItem> TodoItems
        {
            get
            {
                var settings = mediator.Send(new GetFilterSettingsQuery()).Result;
                var todoItems = mediator.Send(new GetTodoItemsQuery() { Settings = settings }).Result;
                return todoItems.ToList();
            }
        }

        public string TodoItemPopupText => EditedToDoItem?.Title ?? string.Empty;

        private TodoItem editedToDoItem;
        public TodoItem EditedToDoItem
        {
            get => editedToDoItem;
            set
            {
                if (editedToDoItem == value) return;
                editedToDoItem = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand StartTimerCommand { get; set; }

        public RelayCommand StopTimerCommand { get; set; }

        public RelayCommand IncreaseSessionTimeCommand { get; set; }

        public RelayCommand DecreaseSessionTimeCommand { get; set; }

        public RelayCommand<object> MarkTodoItemAsDoneCommand { get; set; }

        public RelayCommand<object> SaveTodoItemCommand { get; set; }

        public RelayCommand<object> RemoveTodoItemCommand { get; set; }

        public RelayCommand<object> RemoveFocusSessionCommand { get; set; }

        public RelayCommand OpenCreateTodoItemCommand { get; set; }

        public RelayCommand<object> OpenEditTodoItemCommand { get; set; }
        #endregion

        public SessionViewModel(IToastNotification notification, IMediator mediator)
        {
            this.mediator = mediator;
            this.notification = notification;

            maxSessionDuration = TimeSpan.FromMinutes(55);
            minSessionDuration = TimeSpan.FromMinutes(5);

            motivationSentenceProvider = new MotivationSentencesProvider();

            SessionDuration = mediator.Send(new GetSessionDurationQuery()).Result;

            var viewModelLocator = new ViewModelLocator();
            viewModelLocator.TasksFilter.Filter += (s, e) => RaisePropertyChanged(nameof(TodoItems));

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Tick;

            MarkTodoItemAsDoneCommand = new RelayCommand<object>(MarkTodoItemAsDone);
            SaveTodoItemCommand = new RelayCommand<object>(SaveTodoItem);
            RemoveTodoItemCommand = new RelayCommand<object>(RemoveTodoItem);
            OpenCreateTodoItemCommand = new RelayCommand(OpenCreateTodoItemPopup);
            OpenEditTodoItemCommand = new RelayCommand<object>(OpenEditTodoItemPopup);
            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
            IncreaseSessionTimeCommand = new RelayCommand(IncreaseSessionTime, CanIncreaseSessionTIme);
            DecreaseSessionTimeCommand = new RelayCommand(DecreaseSessionTime, CanDecreaseSessionTIme);
            RemoveFocusSessionCommand = new RelayCommand<object>(RemoveFocusSession, CanRemoveFocusSession);
        }

        private bool CanRemoveFocusSession(object obj)
        {
            if (!(obj is FocusSession session)) return false;
            return !(SessionActive && DailyFocusSessions.IndexOf(session) == DailyFocusSessions.Count - 1);
        }

        private async void RemoveFocusSession(object obj)
        {
            if (!(obj is FocusSession session)) return;

            await mediator.Send(new RemoveFocusSessionCommand() { Id = session.Id });

            RaisePropertyChanged(nameof(DailyFocusSessions));
        }

        private bool CanDecreaseSessionTIme() =>
            !SessionActive && SessionDuration > minSessionDuration;

        private bool CanIncreaseSessionTIme() =>
            !SessionActive && SessionDuration < maxSessionDuration;

        private async void IncreaseSessionTime()
        {
            SessionDuration = await mediator.Send(new ChangeSessionDurationCommand() { Type = Common.Enumerators.AtomicOperationType.Increase });
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => { IncreaseSessionTimeCommand.RaiseCanExecuteChanged(); DecreaseSessionTimeCommand.RaiseCanExecuteChanged(); }));
        }

        private async void DecreaseSessionTime()
        {
            SessionDuration = await mediator.Send(new ChangeSessionDurationCommand() { Type = Common.Enumerators.AtomicOperationType.Decrease });
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => { IncreaseSessionTimeCommand.RaiseCanExecuteChanged(); DecreaseSessionTimeCommand.RaiseCanExecuteChanged(); }));
        }

        private void OpenCreateTodoItemPopup()
        {
            EditedToDoItem = null;
            IsTodoItemPopupOpen = true;
            RaisePropertyChanged(nameof(TodoItemPopupText));
        }

        private void OpenEditTodoItemPopup(object obj)
        {
            EditedToDoItem = (TodoItem)obj;
            IsTodoItemPopupOpen = true;
            RaisePropertyChanged(nameof(TodoItemPopupText));
        }

        private void RemoveTodoItem(object obj)
        {
            var todoItem = obj as TodoItem;
            mediator.Send(new RemoveTodoItemCommand() { Id = todoItem?.Id });
            RaisePropertyChanged(nameof(TodoItems));
        }

        private async void MarkTodoItemAsDone(object obj)
        {
            var todoItem = obj as TodoItem;
            await mediator.Send(new UpdateTodoItemCommand() { Id = todoItem?.Id, IsDone = !todoItem.IsDone });

            RaisePropertyChanged(nameof(TodoItems));
        }

        private async void SaveTodoItem(object obj)
        {
            var title = obj as string;

            if (this.EditedToDoItem != null)
            {
                await mediator.Send(new UpdateTodoItemCommand()
                {
                    Id = EditedToDoItem.Id,
                    Title = title,
                    IsDone = EditedToDoItem.IsDone
                });

                this.EditedToDoItem = null;
            }
            else
            {
                await mediator.Send(new AddTodoItemCommand() { Title = title });
            }

            RaisePropertyChanged(nameof(TodoItems));
        }

        private void StopTimer()
        {
            SessionActive = false;
            timer.Stop();

            Time = sessionDuration;

            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => { IncreaseSessionTimeCommand.RaiseCanExecuteChanged(); DecreaseSessionTimeCommand.RaiseCanExecuteChanged(); }));
        }

        private async void StartTimer()
        {
            SessionActive = true;
            Time = await mediator.Send(new GetSessionDurationQuery());
            timer.Start();

            RefreshMotivationSentence();

            this.currentFocusSessionId = await mediator.Send(new AddFocusSessionCommand());
            RaisePropertyChanged(nameof(DailyFocusSessions));

            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => { IncreaseSessionTimeCommand.RaiseCanExecuteChanged(); DecreaseSessionTimeCommand.RaiseCanExecuteChanged(); }));
        }

        private void RefreshMotivationSentence()
        {
            var motivationSentences = motivationSentenceProvider.GetMotivationSentences();
            var sentenceIndex = Random.Next(motivationSentences.Count());
            MotivationSentence = motivationSentences.ElementAt(sentenceIndex);
        }

        private async void Tick(object sender, EventArgs e)
        {
            Time = time.Subtract(TimeSpan.FromSeconds(1));

            if (Time.Seconds == 0)
            {
                await mediator.Send(
                    new UpdateFocusSessionCommand()
                    {
                        Id = currentFocusSessionId,
                        FocusTime = sessionDuration.Subtract(Time)
                    });
            }

            if (!(Time.TotalSeconds <= 0)) return;

            StopTimer();

            Application.Current.Dispatcher.Invoke(() =>
                notification.ShowSuccess("Your session is over. Good job :)"));

            await mediator.Send(
                new UpdateFocusSessionCommand()
                {
                    Id = currentFocusSessionId,
                    FocusTime = sessionDuration,
                    IsCompleted = true
                });

            RaisePropertyChanged(nameof(DailyFocusSessions));

            if (Application.Current?.MainWindow?.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                Application.Current.MainWindow.Topmost = true;
            }

            var player = new System.Media.SoundPlayer("Sounds/TimesUp.wav"); //TODO: Move to distinct class

            player.Load();
            player.Play();
        }
    }
}
