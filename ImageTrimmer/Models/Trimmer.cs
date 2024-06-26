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
        public Logger Logger { get; set; }

        public async Task TrimAsync(FileInfoWrapper targetFile, Rect rect)
        {
            if (!ValidateParameter(targetFile.FileInfo, rect) || !targetFile.Exists)
            {
                Logger.Add("パラメーターが不正です。以下の項目を確認してください");
                Logger.Add("ファイルの拡張子が .png であるか");
                Logger.Add("ファイルのパスが正しいか");
                Logger.Add("切り抜く範囲が画像より小さいく、サイズは 0 より大きい値か");
                return;
            }

            // 切り抜く範囲を指定 (幅、高さ、X座標、Y座標)
            var width = rect.Width;
            var height = rect.Height;
            var x = rect.X;
            var y = rect.Y;

            var inputImagePath = targetFile.FullName;
            var outputImagePath = $@"{targetFile.FileInfo.Directory}\{Path.GetFileNameWithoutExtension(targetFile.Name)}_cropped.png";

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
                CreateNoWindow = true,
            };

            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            await process.WaitForExitAsync();

            // エラー出力を取得して表示
            var errorMessage = await process.StandardError.ReadToEndAsync();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Logger.Add("Error: " + errorMessage);
            }
            else
            {
                Logger.Add("Image cropped successfully.");
                targetFile.Converted = true;
            }
        }

        /// <summary>
        /// Trim() の先頭で実行され、入力されたパラメーターが適切な値であるかを返します。
        /// </summary>
        /// <returns>２つのパラメーターが適切な値であれば true　を返し、それ以外は false を返します</returns>
        /// <param name="fileInfo">画像ファイルを入力します。サポートされる拡張子は png のみです。</param>
        /// <param name="rect">切り抜く範囲を指定します。画像のサイズ以上の範囲や、サイズ 0 の矩形を指定することはできません。</param>
        private bool ValidateParameter(FileInfo fileInfo, Rect rect)
        {
            if (fileInfo.Extension != ".png")
            {
                return false;
            }

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return false;
            }

            var image = new BitmapImage(new Uri(fileInfo.FullName));
            var range = new Rect(0, 0, image.PixelWidth, image.PixelHeight);

            return range.Contains(rect);
        }
    }
}