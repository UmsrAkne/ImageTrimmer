using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using ImageTrimmer.Models;
using MyLibrary.Commands;
using Prism.Mvvm;

namespace ImageTrimmer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string title = "Image Trimmer";
        private ObservableCollection<FileInfo> fileInfos = new ObservableCollection<FileInfo>();
        private bool uiEnabled = true;

        public bool UiEnabled { get => uiEnabled; set => SetProperty(ref uiEnabled, value); }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public ObservableCollection<FileInfo> FileInfos
        {
            get => fileInfos;
            set => SetProperty(ref fileInfos, value);
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public AsyncDelegateCommand CropImagesCommand => new (async () =>
        {
            if (fileInfos == null || fileInfos.Count == 0)
            {
                return;
            }

            UiEnabled = false;
            var trimmer = new Trimmer();

            foreach (var f in fileInfos)
            {
                await trimmer.TrimAsync(f, new Rect(X, Y, Width, Height));
            }

            UiEnabled = true;
        });

        public void AddFiles(IEnumerable<FileInfo> imageFiles)
        {
            FileInfos = new ObservableCollection<FileInfo>(imageFiles.Where(f => f.Extension == ".png"));
        }
    }
}