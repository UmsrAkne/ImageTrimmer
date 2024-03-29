using System.IO;
using Prism.Mvvm;

namespace ImageTrimmer.Models
{
    public class FileInfoWrapper : BindableBase
    {
        private bool converted;

        public FileInfo FileInfo { get; set; }

        public string FullName => FileInfo != null ? FileInfo.FullName : string.Empty;

        public string Name => FileInfo != null ? FileInfo.Name : string.Empty;

        public bool Exists => FileInfo?.Exists ?? false;

        public bool Converted { get => converted; set => SetProperty(ref converted, value); }
    }
}