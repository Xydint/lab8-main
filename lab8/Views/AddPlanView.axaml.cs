using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using lab8.Models;
using lab8.ViewModels;

namespace lab8.Views
{
    public partial class AddPlanView : Window
    {
        public AddPlanView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.FindControl<Button>("AddImage").Click += async delegate
            {
                OpenImage();
            };
            this.FindControl<Button>("OK").Click += async delegate
            {
                var context = this.DataContext as AddPlanViewModel;
                var plan = context.PlanToReturn;
                plan.PropertyChanged -= context.resetEnable;
                Close(plan);
            };
            this.FindControl<Button>("Cancel").Click += async delegate
            {
                Close(null);
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.DataContext = new AddPlanViewModel();
        }
        private async void OpenImage()
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

            pathArray = await taskPath.ShowAsync(this);
            path = pathArray is null ? null : string.Join(@"\", pathArray);
            if (path != null)
            {
                (this.DataContext as AddPlanViewModel).PlanToReturn.ImagePath = path;
            }
        }
    }
}
