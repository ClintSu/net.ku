using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;

namespace NET.Utilities
{
    /// <summary>
    /// SharpZipLib帮助类
    /// </summary>
    public class ZipHelper
    {
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        public static void ZipFile(string fileToZip, string zipedFile,string password,int compressionLevel, int blockSize)
        {
            if (System.IO.File.Exists(fileToZip))
            {
                throw new System.IO.FileNotFoundException("Specifies that the compressed file:" + fileToZip + " does not exist!");
            }
            using (System.IO.FileStream zipedFileStream = System.IO.File.Create(zipedFile))
            {
                using (ZipOutputStream zipOutputStream = new ZipOutputStream(zipedFileStream))
                {
                    if (!string.IsNullOrEmpty(password))
                        zipOutputStream.Password = password;
                    using (System.IO.FileStream streamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        ZipEntry zipEntry = new ZipEntry(fileName);
                        zipOutputStream.PutNextEntry(zipEntry); //往ZipOutputStream里写入一个ZipEntry
                        zipOutputStream.SetLevel(compressionLevel); //设置压缩等级，等级越高文件越小,CPU使用率越高
                        zipOutputStream.SetComment("zip Comment"); //压缩包的注释
                        byte[] buffer = new byte[blockSize]; //定义写入缓冲区
                        int sizeRead = 0;
                        try
                        {
                            do
                            {
                                sizeRead = streamToZip.Read(buffer, 0, buffer.Length); //读取文件内容
                                zipOutputStream.Write(buffer, 0, sizeRead);//将文件内容写入压缩相应的文件
                            } while (sizeRead > 0);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        streamToZip.Close();
                    }
                    zipOutputStream.Finish(); //结束写入
                    zipOutputStream.Close();
                }
                zipedFileStream.Close();
            }
        }
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        public static void ZipFile(string fileToZip, string zipedFile)
        {
            ZipFile(fileToZip, zipedFile, "", 5, 4 * 1024);
        }
        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="password">设置压缩密码</param>
        public static void ZipFileDirectory(string strDirectory, string zipedFile,string password)
        {
            using (System.IO.FileStream zipedStream = System.IO.File.Create(zipedFile))
            {
                using (ZipOutputStream zopStream = new ZipOutputStream(zipedStream))
                {
                    if(!string.IsNullOrEmpty(password))
                        zopStream.Password = password;
                    ZipStep(strDirectory, zopStream, "");
                }
            }
        }
        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        public static void ZipFileDirectory(string strDirectory, string zipedFile)
        {
            ZipFileDirectory(strDirectory, zipedFile, "");
        }
        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">要压缩的文件</param>
        /// <param name="zopStream">压缩文件流</param>
        /// <param name="parentPath">文件所属父路径</param>
        private static void ZipStep(string strDirectory,ZipOutputStream zopStream,string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                strDirectory += System.IO.Path.DirectorySeparatorChar;
            }

            Crc32 crc32 = new Crc32();
            string[] fileNames = System.IO.Directory.GetFileSystemEntries(strDirectory);
            foreach(string fileName in fileNames)
            {
                if (System.IO.Directory.Exists(fileName))
                {

                    var path = parentPath;
                    path += fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    path += "\\";
                    ZipStep(fileName, zopStream, path);
                }
                else
                {
                    using (System.IO.FileStream fs = System.IO.File.OpenRead(fileName))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        var file = parentPath + fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(file);

                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;
                        fs.Close();

                        crc32.Reset();
                        crc32.Update(buffer);

                        entry.Crc = crc32.Value;
                        zopStream.PutNextEntry(entry);
                        zopStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }



        }   
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="zipedFile">要解压的文件</param>
        /// <param name="strDirectory">压缩后的文件路径</param>
        /// <param name="password">文件的密码</param>
        /// <param name="overWrite">是否覆盖已存在的文件</param>
        public void UnZip(string zipedFile,string strDirectory, string password,bool overWrite)
        {
            if (strDirectory == "")
                strDirectory = System.IO.Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream zipInputStream = new ZipInputStream(System.IO.File.OpenRead(zipedFile)))
            {
                zipInputStream.Password = password;
                ZipEntry theEntry;

                while ((theEntry = zipInputStream.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = System.IO.Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = System.IO.Path.GetFileName(pathToZip);

                    System.IO.Directory.CreateDirectory(strDirectory + directoryName);

                    if (fileName != "")
                    {
                        if ((System.IO.File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!System.IO.File.Exists(strDirectory + directoryName + fileName)))
                        {
                            try
                            {
                                using (System.IO.FileStream streamWriter = System.IO.File.Create(strDirectory + directoryName + fileName))
                                {
                                    int size = 2048;
                                    byte[] data = new byte[2048];
                                    while (true)
                                    {
                                        size = zipInputStream.Read(data, 0, data.Length);

                                        if (size > 0)
                                            streamWriter.Write(data, 0, size);
                                        else
                                            break;
                                    }
                                    streamWriter.Close();
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                zipInputStream.Close();
            }
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="zipedFile">要解压的文件</param>
        /// <param name="strDirectory">压缩后的文件路径</param>
        public void UnZip(string zipedFile, string strDirectory)
        {
            UnZip(zipedFile, strDirectory, "", true);
        }

    }
}
