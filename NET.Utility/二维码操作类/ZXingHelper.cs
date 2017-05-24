using com.google.zxing.qrcode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.google.zxing;
using System.Drawing.Imaging;
using com.google.zxing.common;
using com.google.zxing.multi.qrcode;

namespace NET.Utilities
{
    /// <summary>
    /// ZXing.NET生成读取二维码
    /// </summary>
    public class ZXingHelper
    {
        /// <summary>
        /// 二维码生成到本地
        /// </summary>
        /// <param name="contents">内容</param>
        /// <param name="fileName">路径</param>
        /// <param name="W">宽</param>
        /// <param name="H">高</param>
        public static void Write(string contents, string fileName, int W = 300, int H = 300)
        {
            Bitmap bitmap = Write(contents, W, H);
            if (bitmap != null)
            {
                bitmap.Save(fileName, ImageFormat.Png);
                bitmap.Dispose();
            }
        }
        /// <summary>
        /// 二维码生成图片
        /// </summary>
        /// <param name="contents">内容</param>
        /// <param name="W">宽</param>
        /// <param name="H">高</param>
        /// <returns></returns>
        public static Bitmap Write(string contents, int W = 300, int H = 300)
        {
            try
            {
                QRCodeWriter qrCodeWriter = new QRCodeWriter();
                ByteMatrix byteMatrix = qrCodeWriter.encode(contents, BarcodeFormat.QR_CODE, W, H);
                //return byteMatrix.ToBitmap();
                return ToBitmap(byteMatrix);
            }
            catch (Exception e)
            {
               return null;
            }
        }
        /// <summary>
        /// 定义生成图片颜色
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(ByteMatrix matrix)
        {
            int width = matrix.Width;
            int height = matrix.Height;
            Bitmap bmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bmap.SetPixel(x, y, matrix.get_Renamed(x, y) != -1 ? ColorTranslator.FromHtml("0xFF000000") : ColorTranslator.FromHtml("0xFFFFFFFF"));
                }
            }
            return bmap;
        }
        /// <summary>
        /// 读取本地二维码
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Read(string fileName)
        {
            Image image= Image.FromFile(fileName);
            try
            {
                var bitmap = new Bitmap(image);
                return Read(bitmap);
            }
            catch (System.IO.IOException ioe)
            {
                return string.Empty;
            } 
        }
        /// <summary>
        /// 读取二维码内容
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static string Read(Bitmap bitmap)
        {
            try
            {
                LuminanceSource luminanceSource = new RGBLuminanceSource(bitmap, bitmap.Width, bitmap.Height);
                //Binarizer binarizer= new GlobalHistogramBinarizer(luminanceSource);
                Binarizer binarizer = new HybridBinarizer(luminanceSource);
                BinaryBitmap binaryBitmap = new BinaryBitmap(binarizer);
                QRCodeReader qrCodeReader = new QRCodeReader();
                Result result = qrCodeReader.decode(binaryBitmap);
                return result.Text;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
            
        } 
    }
}
