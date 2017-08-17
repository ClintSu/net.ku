using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace NET.Utilities
{
    /// <summary>
    ///     感知哈希算法——找出相似的图片
    /// </summary>
    public class SimilarImage
    {
        private readonly Image _sourceImg;

        public SimilarImage(string filePath)
        {
            _sourceImg = Image.FromFile(filePath);
        }

        public SimilarImage(Stream stream)
        {
            _sourceImg = Image.FromStream(stream);
        }

        public string GetHash()
        {
            var image = ReduceSize();
            var grayValues = ReduceColor(image);
            var average = CalcAverage(grayValues);
            var reslut = ComputeBits(grayValues, average);
            return reslut;
        }

        /// <summary>
        ///     Step 1 : Reduce size to 8*8
        ///     第一步 缩小图片尺寸
        ///     将图片缩小到8x8的尺寸, 总共64个像素. 这一步的作用是去除各种图片尺寸和图片比例的差异, 只保留结构、明暗等基本信息.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Image ReduceSize(int width = 8, int height = 8)
        {
            var image = _sourceImg.GetThumbnailImage(width, height, () => { return false; }, IntPtr.Zero);
            return image;
        }

        // Step 2 : Reduce Color
        /// <summary>
        ///     第二步 转为灰度图片
        ///     将缩小后的图片, 转为64级灰度图片.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private byte[] ReduceColor(Image image)
        {
            var bitMap = new Bitmap(image);
            var grayValues = new byte[image.Width*image.Height];

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = bitMap.GetPixel(x, y);
                    var grayValue = (byte) ((color.R*30 + color.G*59 + color.B*11)/100);
                    grayValues[x*image.Width + y] = grayValue;
                }
            }

            return grayValues;
        }

        // Step 3 : Average the colors
        /// <summary>
        ///     第三步 计算灰度平均值
        ///     计算图片中所有像素的灰度平均值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private byte CalcAverage(byte[] values)
        {
            var sum = 0;
            for (var i = 0; i < values.Length; i++)
                sum += values[i];
            return Convert.ToByte(sum/values.Length);
        }

        // Step 4 : Compute the bits
        /// <summary>
        ///     第四步 比较像素的灰度
        ///     将每个像素的灰度与平均值进行比较, 如果大于或等于平均值记为1, 小于平均值记为0.
        ///     第五步 计算哈希值
        ///     将上一步的比较结果, 组合在一起, 就构成了一个64位的二进制整数, 这就是这张图片的指纹.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="averageValue"></param>
        /// <returns></returns>
        private string ComputeBits(byte[] values, byte averageValue)
        {
            var result = new char[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] < averageValue)
                    result[i] = '0';
                else
                    result[i] = '1';
            }
            return new string(result);
        }

        // Compare hash
        /// <summary>
        ///     第六步 对比图片指纹
        ///     得到图片的指纹后, 就可以对比不同的图片的指纹, 计算出64位中有多少位是不一样的. 如果不相同的数据位数不超过5,
        ///     就说明两张图片很相似, 如果大于10, 说明它们是两张不同的图片.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int CalcSimilarDegree(string a, string b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException();
            var count = 0;
            for (var i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    count++;
            }
            return count;
        }

        private static string ComputeHash(Image img)
        {
            var image = img.GetThumbnailImage(8, 8, () => false, IntPtr.Zero);
            var bitMap = new Bitmap(image);
            var grayValues = new byte[image.Width*image.Height];

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = bitMap.GetPixel(x, y);
                    var grayValue = (byte) ((color.R*30 + color.G*59 + color.B*11)/100);
                    grayValues[x*image.Width + y] = grayValue;
                }
            }

            var sum = grayValues.Aggregate(0, (current, t) => current + t);

            var averageValue = Convert.ToByte(sum/grayValues.Length);

            var result = new char[grayValues.Length];
            for (var i = 0; i < grayValues.Length; i++)
            {
                if (grayValues[i] < averageValue)
                {
                    result[i] = '0';
                }
                else
                {
                    result[i] = '1';
                }
            }
            return new string(result);
        }

        public static double Veritify(Image img1, Image img2)
        {
            var key1 = ComputeHash(img1);
            var key2 = ComputeHash(img2);

            if (key1.Length != key2.Length)
                throw new ArgumentException();
            var count = 0;
            for (var i = 0; i < key1.Length; i++)
            {
                if (key1[i] != key2[i])
                {
                    count++;
                }
            }
            var result = (10 - count)/10;
            if (result < 0)
            {
                return 0;
            }
            return result;
        }
    }
}