namespace FocusWarden.Lib.ViewModels
{
    using GongSolutions.Wpf.DragDrop;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using System.Collections.ObjectModel;

    public class BacklogViewModel : ObservableObject, IDropTarget
    {
        public BacklogViewModel()
        {
            Items1 = new ObservableCollection<ExampleItemViewModel>
            {
                new() {Title = "Title 1"}, new() {Title = "Title 2"}
            };
            Items2 = new ObservableCollection<ExampleItemViewModel>
            {
                new() {Title = "Title 3"}, new() {Title = "Title 4"}
            };
            Items3 = new ObservableCollection<ExampleItemViewModel>
            {
                new() {Title = "Title 5"}, new() {Title = "Title 6"}
            };
        }

        public ObservableCollection<ExampleItemViewModel> Items1 { get; set; }
        public ObservableCollection<ExampleItemViewModel> Items2 { get; set; }
        public ObservableCollection<ExampleItemViewModel> Items3 { get; set; }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            //TODO
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            //TODO
        }
    }

    public class ExampleItemViewModel
    {
        public string Title { get; set; }
    }
}