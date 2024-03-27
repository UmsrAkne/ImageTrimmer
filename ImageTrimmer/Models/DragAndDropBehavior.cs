using System.IO;
using System.Linq;
using System.Windows;
using ImageTrimmer.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace ImageTrimmer.Models
{
    public class DragAndDropBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.Drop += OnDrop;
            AssociatedObject.DragEnter += OnDragEnter;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Drop -= OnDrop;
            AssociatedObject.DragEnter -= OnDragEnter;
            base.OnDetaching();
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && e.AllowedEffects.HasFlag(DragDropEffects.Copy))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var d = ((Window)sender).DataContext as MainWindowViewModel;
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                d?.AddFiles(files?.Select(f => new FileInfo(f)));
            }

            e.Handled = true;
        }
    }
}