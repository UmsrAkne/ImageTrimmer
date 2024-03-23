using Prism.Mvvm;

namespace ImageTrimmer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string title = "Image Trimmer";

        public string Title { get => title; set => SetProperty(ref title, value); }
    }
}