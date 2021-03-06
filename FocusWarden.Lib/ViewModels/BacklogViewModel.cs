using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;

namespace FocusWarden.Lib.ViewModels
{
    public class BacklogViewModel : ViewModelBase, IDropTarget
    {
        public ObservableCollection<ExampleItemViewModel> Items1 { get; set; }
        public ObservableCollection<ExampleItemViewModel> Items2 { get; set; }
        public ObservableCollection<ExampleItemViewModel> Items3 { get; set; }

        public BacklogViewModel()
        {
            Items1 = new ObservableCollection<ExampleItemViewModel>
            {
                new ExampleItemViewModel(){Title="Title 1"},
                new ExampleItemViewModel(){Title="Title 2"}
            };
            Items2 = new ObservableCollection<ExampleItemViewModel>
            {
                new ExampleItemViewModel(){Title="Title 3"},
                new ExampleItemViewModel(){Title="Title 4"}
            };
            Items3 = new ObservableCollection<ExampleItemViewModel>
            {
                new ExampleItemViewModel(){Title="Title 5"},
                new ExampleItemViewModel(){Title="Title 6"}
            };
        }

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
