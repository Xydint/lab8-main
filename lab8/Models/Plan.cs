using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.IO;
using System.Reflection;

namespace lab8.Models
{
    public class Plan : INotifyPropertyChanged
    {
        public Plan()
        {
        }
        public Plan(string n = "", string d = "", string? p = null)
        {
            Name = n;
            Description = d;
            if (p is not null)
            {
                ImagePath = p;
            }
        }
        string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }
        string _description = "";
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyPropertyChanged(nameof(Description));
            }
        }
        Bitmap? _image = null;
        public Bitmap? Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyPropertyChanged(nameof(Image));
            }
        }
        string? _imagePath = null;
        public string? ImagePath
        {
            get => _imagePath;
            set
            {
                try
                {
                    var bm = new Bitmap(value);
                }
                catch (Exception ex)
                {
                    return;
                }
                _imagePath = value;
                Image = new Bitmap(value);
                NotifyPropertyChanged(nameof(ImagePath));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
