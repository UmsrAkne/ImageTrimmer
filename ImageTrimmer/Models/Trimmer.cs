using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageTrimmer.Models
{
    public class Trimmer
    {
        public void Trim(FileInfo targetFile, Rect rect)
        {
            if (!ValidateParameter(targetFile, rect) || !targetFile.Exists)
            {
                Console.WriteLine("パラメーターが不正です");
                Console.WriteLine("ファイルが .png かどうか、パスが正しいか、切り抜く範囲が画像より大きくないを確認してください。");
                return;
            }

            // 切り抜く範囲を指定 (幅、高さ、X座標、Y座標)
            var width = rect.Width;
            var height = rect.Height;
            var x = rect.X;
            var y = rect.Y;

            var inputImagePath = targetFile.FullName;
            var outputImagePath = $@"{targetFile.Directory}\{Path.GetFileNameWithoutExtension(targetFile.Name)}_cropped.png";

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

        public async Task TrimAsync(FileInfo targetFile, Rect rect)
        {
            if (!ValidateParameter(targetFile, rect) || !targetFile.Exists)
            {
                Console.WriteLine("パラメーターが不正です");
                Console.WriteLine("ファイルが .png かどうか、パスが正しいか、切り抜く範囲が画像より大きくないを確認してください。");
                return;
            }

            // 切り抜く範囲を指定 (幅、高さ、X座標、Y座標)
            var width = rect.Width;
            var height = rect.Height;
            var x = rect.X;
            var y = rect.Y;

            var inputImagePath = targetFile.FullName;
            var outputImagePath = $@"{targetFile.Directory}\{Path.GetFileNameWithoutExtension(targetFile.Name)}_cropped.png";

            // ImageMagick のコマンド引数を作成
            var arguments = $"\"{inputImagePath}\" -crop {width}x{height}+{x}+{y} \"{outputImagePath}\"";

            // ImageMagick を呼び出し
            var startInfo = new ProcessStartInfo
            {
                FileName = "magick",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            await process.WaitForExitAsync();

            // エラー出力を取得して表示
            var errorMessage = await process.StandardError.ReadToEndAsync();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine("Error: " + errorMessage);
            }
            else
            {
                Console.WriteLine("Image cropped successfully.");
            }
        }

        /// <summary>
        /// Trim() の先頭で実行され、入力されたパラメーターが適切な値であるかを返します。
        /// </summary>
        /// <returns>２つのパラメーターが適切な値であれば true　を返し、それ以外は false を返します</returns>
        /// <param name="fileInfo">画像ファイルを入力します。サポートされる拡張子は png のみです。</param>
        /// <param name="rect">切り抜く範囲を指定します。画像のサイズ以上の範囲を指定することはできません。</param>
        public bool ValidateParameter(FileInfo fileInfo, Rect rect)
        {
            if (fileInfo.Extension != ".png")
            {
                return false;
            }

            var image = new BitmapImage(new Uri(fileInfo.FullName));
            var range = new Rect(0, 0, image.PixelWidth, image.PixelHeight);

            return range.Contains(rect);
        }
    }
}