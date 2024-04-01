using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using ImageTrimmer.Models;
using MyLibrary.Commands;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageTrimmer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string title = "Image Trimmer";
        private ObservableCollection<FileInfoWrapper> fileInfos = new ObservableCollection<FileInfoWrapper>();
        private bool uiEnabled = true;
        private int completedCount;

        public bool UiEnabled { get => uiEnabled; set => SetProperty(ref uiEnabled, value); }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public ObservableCollection<FileInfoWrapper> FileInfos
        {
            get => fileInfos;
            private set => SetProperty(ref fileInfos, value);
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int CompletedCount { get => completedCount; set => SetProperty(ref completedCount, value); }

        public AsyncDelegateCommand CropImagesCommand => new (async () =>
        {
            if (fileInfos == null || fileInfos.Count == 0)
            {
                return;
            }

            CompletedCount = 0;
            UiEnabled = false;
            var trimmer = new Trimmer();

            foreach (var f in fileInfos)
            {
                await trimmer.TrimAsync(f, new Rect(X, Y, Width, Height));
                CompletedCount = FileInfos.Count(fw => fw.Converted);

                if (CancelRequest)
                {
                    CancelRequest = false;
                    break;
                }
            }

            UiEnabled = true;
        });

        public DelegateCommand CancelCommand => new DelegateCommand(() =>
        {
            CancelRequest = true;
        });

        private bool CancelRequest { get; set; }

        public void AddFiles(IEnumerable<FileInfo> imageFiles)
        {
            FileInfos = new ObservableCollection<FileInfoWrapper>(imageFiles
                .Where(f => f.Extension == ".png")
                .Select(f => new FileInfoWrapper() { FileInfo = f, }));
        }
    }
}