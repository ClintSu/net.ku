using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace NET.Utilities
{
    public class CommonMethod
    {
        private static DateTime EndClickTime = DateTime.MinValue;
        /// <summary>
        /// 判断是否重复点击
        /// </summary>        
        public static bool RepeatClick(double millisecond = 250)
        {
            if (DateTime.Now.Subtract(EndClickTime) < TimeSpan.FromMilliseconds(millisecond))
            {
                EndClickTime = DateTime.Now;
                return true;
            }
            EndClickTime = DateTime.Now;
            return false;

        }

        /// <summary>
        /// 本地路径转BitMap
        /// </summary>   
        public static BitmapImage GetBitmapImage(string fileName)
        {
            Console.WriteLine(fileName);
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // just in case you want to load the image in another thread
                return bitmapImage;
            }
        }

        /// <summary>
        /// 图片url下载转BitMap
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static BitmapImage GetHttpBitmapImage(string url)
        {
            Console.WriteLine(url);
            var image = new BitmapImage();
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    image.BeginInit();
                    image.UriSource = new Uri("pack://application:,,,/jg.CloudCube.WPF;component/Resources/NullImage.jpg");
                    image.EndInit();
                    image.DecodePixelWidth = 50;
                    image.Freeze();
                }
                int filenamelen = url.LastIndexOf("/");
                string LocalFilePath = System.IO.Path.GetTempPath() + url.Substring(filenamelen);
                if (File.Exists(LocalFilePath))
                {
                    image = GetBitmapImage(LocalFilePath);
                    return image;
                }
                int BytesToRead = 100;
                WebRequest request = WebRequest.Create(new Uri(url, UriKind.Absolute));
                request.Timeout = -1;
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                BinaryReader reader = new BinaryReader(responseStream);
                MemoryStream memoryStream = new MemoryStream();
                FileStream fs = new FileStream(LocalFilePath, FileMode.OpenOrCreate);
                byte[] bytebuffer = new byte[BytesToRead];
                int bytesRead = reader.Read(bytebuffer, 0, BytesToRead);

                while (bytesRead > 0)
                {
                    memoryStream.Write(bytebuffer, 0, bytesRead);
                    fs.Write(bytebuffer, 0, bytesRead);
                    bytesRead = reader.Read(bytebuffer, 0, BytesToRead);
                }
                image.BeginInit();
                memoryStream.Seek(0, SeekOrigin.Begin);
                image.StreamSource = memoryStream;
                image.EndInit();
                image.Freeze();
            }
            catch (Exception ex)
            {
                BitmapImage catchImage = new BitmapImage();
                catchImage.BeginInit();
                catchImage.UriSource = new Uri("pack://application:,,,/jg.CloudCube.WPF;component/Resources/NullImage.jpg");
                catchImage.EndInit();
                catchImage.DecodePixelWidth = 50;
                catchImage.Freeze();
                return catchImage;
            }
            return image;
        }



    }
}
