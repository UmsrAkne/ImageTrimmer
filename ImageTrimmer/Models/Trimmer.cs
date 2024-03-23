using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ImageTrimmer.Models
{
    public class Trimmer
    {
        public void Trim(FileInfo targetPath, Rect rect)
        {
            // 切り抜く範囲を指定 (幅、高さ、X座標、Y座標)
            var width = rect.Width;
            var height = rect.Height;
            var x = rect.X;
            var y = rect.Y;

            var inputImagePath = targetPath.FullName;
            var outputImagePath = $@"{targetPath.Directory}\{Path.GetFileNameWithoutExtension(targetPath.Name)}_cropped.png";

            // ImageMagick のコマンド引数を作成
            var arguments = $"\"{inputImagePath}\" -crop {width}x{height}+{x}+{y} \"{outputImagePath}\"";

            // ImageMagick を呼び出し
            var startInfo = new ProcessStartInfo
            {
                FileName = "convert",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();

            // エラー出力を取得して表示
            var errorMessage = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine("Error: " + errorMessage);
            }
            else
            {
                Console.WriteLine("Image cropped successfully.");
            }
        }
    }
}