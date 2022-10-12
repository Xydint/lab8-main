using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.Text.Json;
using System.Text.Json.Serialization;
using lab8.Models;
using lab8.ViewModels;

namespace lab8.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.FindControl<MenuItem>("New").Click += ClickEventNew;
            this.FindControl<MenuItem>("Save").Click += ClickEventSave;
            this.FindControl<MenuItem>("Load").Click += ClickEventLoad;
            this.FindControl<MenuItem>("Exit").Click += ClickEventExit;
            this.FindControl<MenuItem>("About").Click += ClickEventAbout;
            this.FindControl<Button>("AddPlanned").Click += ClickEventAddPlanned;
            this.FindControl<Button>("AddInWork").Click += ClickEventAddInWork;
            this.FindControl<Button>("AddCompleted").Click += ClickEventAddCompleted;
        }
        private async void ClickEventNew(object sender, RoutedEventArgs e)
        {
            this.DataContext = new MainWindowViewModel();
            (this.DataContext as MainWindowViewModel).view = this;
        }
        // Как сохранять именно изображения, а не пути к ним, - я вообще не нашёл способа
        // Вините Авалонию с её несериализуемыми битмапами
        private async void ClickEventSave(object sender, RoutedEventArgs e)
        {
            string? path;
            var taskPath = new SaveFileDialog()
            {
                Title = "Save file",
                Filters = new List<FileDialogFilter>()
            };
            taskPath.Filters.Add(new FileDialogFilter() { Name = "Таблица задач (*.tot)", Extensions = { "tot" } });

            path = await taskPath.ShowAsync((Window)this.VisualRoot);

            if (path is not null)
            {
                var itemsPlanned = (this.DataContext as MainWindowViewModel).ItemsPlanned;
                var itemsInWork = (this.DataContext as MainWindowViewModel).ItemsInWork;
                var itemsCompleted = (this.DataContext as MainWindowViewModel).ItemsCompleted;
                var str = File.Create(path);
                var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(str, itemsPlanned.Count);
                foreach (var item in itemsPlanned)
                {
                    bf.Serialize(str, item.Name);
                    bf.Serialize(str, item.Description);
                    if (item.ImagePath is not null)
                    {
                        bf.Serialize(str, true);
                        bf.Serialize(str, item.ImagePath);
                    }
                    else bf.Serialize(str, false);
                }
                bf.Serialize(str, itemsInWork.Count);
                foreach (var item in itemsInWork)
                {
                    bf.Serialize(str, item.Name);
                    bf.Serialize(str, item.Description);
                    if (item.ImagePath is not null)
                    {
                        bf.Serialize(str, true);
                        bf.Serialize(str, item.ImagePath);
                    }
                    else bf.Serialize(str, false);
                }
                bf.Serialize(str, itemsCompleted.Count);
                foreach (var item in itemsCompleted)
                {
                    bf.Serialize(str, item.Name);
                    bf.Serialize(str, item.Description);
                    if (item.ImagePath is not null)
                    {
                        bf.Serialize(str, true);
                        bf.Serialize(str, item.ImagePath);
                    }
                    else bf.Serialize(str, false);
                }
                str.Close();
            }
        }
        private async void ClickEventLoad(object sender, RoutedEventArgs e)
        {
            string? path;
            var taskPath = new OpenFileDialog()
            {
                Title = "Open file",
                Filters = new List<FileDialogFilter>()
            };
            taskPath.Filters.Add(new FileDialogFilter() { Name = "Таблица задач (*.tot)", Extensions = { "tot" } });

            string[]? pathArray = await taskPath.ShowAsync((Window)this.VisualRoot);
            path = pathArray is null ? null : string.Join(@"\", pathArray);

            if (path is not null)
            {
                int count;
                string name;
                string description;
                string? pathImage = null;
                var itemsPlanned = (this.DataContext as MainWindowViewModel).ItemsPlanned;
                var itemsInWork = (this.DataContext as MainWindowViewModel).ItemsInWork;
                var itemsCompleted = (this.DataContext as MainWindowViewModel).ItemsCompleted;
                itemsPlanned.Clear();
                itemsInWork.Clear();
                itemsCompleted.Clear();
                var str = File.OpenRead(path);
                var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                count = (int)bf.Deserialize(str);
                for (int i = 0; i < count; i++)
                {
                    name = (string)bf.Deserialize(str);
                    description = (string)bf.Deserialize(str);
                    if ((bool)bf.Deserialize(str))
                    {
                        pathImage = (string)bf.Deserialize(str);
                    }
                    itemsPlanned.Add(new Plan(name, description, pathImage));
                    pathImage = null;
                }
                count = (int)bf.Deserialize(str);
                for (int i = 0; i < count; i++)
                {
                    name = (string)bf.Deserialize(str);
                    description = (string)bf.Deserialize(str);
                    if ((bool)bf.Deserialize(str))
                    {
                        pathImage = (string)bf.Deserialize(str);
                    }
                    itemsInWork.Add(new Plan(name, description, pathImage));
                    pathImage = null;
                }
                count = (int)bf.Deserialize(str);
                for (int i = 0; i < count; i++)
                {
                    name = (string)bf.Deserialize(str);
                    description = (string)bf.Deserialize(str);
                    if ((bool)bf.Deserialize(str))
                    {
                        pathImage = (string)bf.Deserialize(str);
                    }
                    itemsCompleted.Add(new Plan(name, description, pathImage));
                    pathImage = null;
                }
                str.Close();
            }
        }
        private async void ClickEventExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void ClickEventAbout(object? sender, RoutedEventArgs e)
        {
            var window = new About();
            window.Icon = new WindowIcon
                (new Bitmap(AvaloniaLocator
                .Current.GetService<IAssetLoader>()
                .Open(
                    new Uri(
                    $"avares://" +
                    $"{Assembly.GetEntryAssembly().GetName().Name}" +
                    $"/Assets/avalonia-logo-but-better.ico"
                ))));
            await window.ShowDialog((Window)this.VisualRoot);
        }
        private async void ClickEventAddPlanned(object sender, RoutedEventArgs e)
        {
            AddPlanNewWindow("Planned");
        }
        private async void ClickEventAddInWork(object sender, RoutedEventArgs e)
        {
            AddPlanNewWindow("InWork");
        }
        private async void ClickEventAddCompleted(object sender, RoutedEventArgs e)
        {
            AddPlanNewWindow("Completed");
        }
        private async void AddPlanNewWindow(string type)
        {
            var window = new AddPlanView();
            window.Icon = new WindowIcon
                (new Bitmap(AvaloniaLocator
                .Current.GetService<IAssetLoader>()
                .Open(
                    new Uri(
                    $"avares://" +
                    $"{Assembly.GetEntryAssembly().GetName().Name}" +
                    $"/Assets/avalonia-logo-but-better.ico"
                ))));
            var item = await window.ShowDialog<Plan?>((Window)this.VisualRoot);
            if (item != null)
            {
                var context = this.DataContext as MainWindowViewModel;
                switch (type)
                {
                    case "Planned":
                        context.ItemsPlanned.Add(item);
                        break;
                    case "InWork":
                        context.ItemsInWork.Add(item);
                        break;
                    case "Completed":
                        context.ItemsCompleted.Add(item);
                        break;
                }
            }
        }
    }
}
