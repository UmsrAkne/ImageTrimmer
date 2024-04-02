using System;
using Prism.Mvvm;

namespace ImageTrimmer.Models
{
    public class Logger : BindableBase
    {
        private string text;

        public string Text { get => text; set => SetProperty(ref text, value); }

        public void Add(string message)
        {
            var msg = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss ") + message;
            Text = $"{msg}\n{Text}";
        }
    }
}