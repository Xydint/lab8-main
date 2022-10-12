using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Interactivity;
using System.ComponentModel;
using ReactiveUI;
using System.Reactive;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using lab8.Models;

namespace lab8.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(bool makeItems = false)
        {
            if (makeItems)
            {
                ItemsPlanned = makePlansTODO();
                ItemsInWork = makePlansInWork();
                ItemsCompleted = makePlansCompleted();
            }
            else
            {
                ItemsPlanned = new ObservableCollection<Plan>();
                ItemsInWork = new ObservableCollection<Plan>();
                ItemsCompleted = new ObservableCollection<Plan>();
            }
            ButtonAddImage = ReactiveCommand.Create<Plan, Unit>((item) =>
            {
                OpenImage(item);
                return Unit.Default;
            });
            ButtonDeletePlanned = ReactiveCommand.Create<Plan, Unit>((item) =>
            {
                ItemsPlanned.Remove(item);
                return Unit.Default;
            });
            ButtonDeleteInWork = ReactiveCommand.Create<Plan, Unit>((item) =>
            {
                ItemsInWork.Remove(item);
                return Unit.Default;
            });
            ButtonDeleteCompleted = ReactiveCommand.Create<Plan, Unit>((item) =>
            {
                ItemsCompleted.Remove(item);
                return Unit.Default;
            });
        }
        ObservableCollection<Plan> itemsPlanned;
        public ObservableCollection<Plan> ItemsPlanned
        {
            get { return itemsPlanned; }
            set { this.RaiseAndSetIfChanged(ref itemsPlanned, value); }
        }
        ObservableCollection<Plan> itemsInWork;
        public ObservableCollection<Plan> ItemsInWork
        {
            get { return itemsInWork; }
            set { this.RaiseAndSetIfChanged(ref itemsInWork, value); }
        }
        ObservableCollection<Plan> itemsCompleted;
        public ObservableCollection<Plan> ItemsCompleted
        {
            get { return itemsCompleted; }
            set { this.RaiseAndSetIfChanged(ref itemsCompleted, value); }
        }
        private ObservableCollection<Plan> makePlansTODO()
        {
            return new ObservableCollection<Plan>
            {
                new Plan("Planned Task 1"),
                new Plan("Planned Task 2", "put something here"),
                new Plan("Planned Task 3", "aeiou",
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
                + "/Assets/avalonia-logo.ico")
            };
        }
        private ObservableCollection<Plan> makePlansInWork()
        {
            return new ObservableCollection<Plan>
            {
                new Plan("InWork Task 1"),
                new Plan("InWork Task 2", "something"),
                new Plan("InWork Task 3", "yee",
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
                + "/Assets/add.ico")
            };
        }
        private ObservableCollection<Plan> makePlansCompleted()
        {
            return new ObservableCollection<Plan>
            {

                new Plan("Completed Task 1"),
                new Plan("Completed Task 2", "| || || |_"),
                new Plan("Completed Task 3", "sus",
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
                + "/Assets/avalonia-logo-but-better.ico")
            };
        }
        public ReactiveCommand<Plan, Unit> ButtonAddImage { get; }
        public ReactiveCommand<Plan, Unit> ButtonDeletePlanned { get; }
        public ReactiveCommand<Plan, Unit> ButtonDeleteInWork { get; }
        public ReactiveCommand<Plan, Unit> ButtonDeleteCompleted { get; }
        private async void OpenImage(Plan item)
        {
            string? path;
            string[]? pathArray = null;
            var taskPath = new OpenFileDialog()
            {
                Title = "Open file",
                Filters = new List<FileDialogFilter>()
            };
            taskPath.Filters.Add
                (new FileDialogFilter() { Name = "image", Extensions = { "png", "ico", "jpg", "jpeg", "jpe" } });

            pathArray = await taskPath.ShowAsync(view);
            path = pathArray is null ? null : string.Join(@"\", pathArray);
            if (path != null)
            {
                item.Image = new Bitmap(path);
            }
        }
        public Window? view = null;
    }
}
