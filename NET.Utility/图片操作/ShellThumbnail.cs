using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace NET.Utilities
{
    /// <summary>
    ///     毫秒级生成缩略图
    /// </summary>
    public class ShellThumbnail : IDisposable
    {
        private IMalloc _alloc;
        private bool _disposed;

        public Bitmap ThumbNail { get; private set; }

        public Size DesiredSize { get; set; } = new Size(100, 100);

        private IMalloc Allocator
        {
            get
            {
                if (!_disposed)
                {
                    if (_alloc == null)
                    {
                        UnmanagedMethods.SHGetMalloc(ref _alloc);
                    }
                }
                else
                {
                    Debug.Assert(false, "Object has been disposed.");
                }
                return _alloc;
            }
        }

        private IShellFolder GetDesktopFolder
        {
            get
            {
                IShellFolder ppshf = null;
                UnmanagedMethods.SHGetDesktopFolder(ref ppshf);
                return ppshf;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_alloc != null)
                {
                    Marshal.ReleaseComObject(_alloc);
                }
                _alloc = null;
                if (ThumbNail != null)
                {
                    ThumbNail.Dispose();
                }
                _disposed = true;
            }
        }

        ~ShellThumbnail()
        {
            Dispose();
        }

        public Bitmap GetThumbnail(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            if (!File.Exists(fileName) && !Directory.Exists(fileName))
            {
                throw new FileNotFoundException($"The file '{fileName}' does not exist", fileName);
            }
            if (ThumbNail != null)
            {
                ThumbNail.Dispose();
                ThumbNail = null;
            }
            var folder = GetDesktopFolder;
            if (folder != null)
            {
                var pidlMain = IntPtr.Zero;
                try
                {
                    var cParsed = 0;
                    var pdwAttrib = 0;
                    var filePath = Path.GetDirectoryName(fileName);
                    folder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, filePath, ref cParsed, ref pidlMain, ref pdwAttrib);
                }
                catch (Exception)
                {
                    Marshal.ReleaseComObject(folder);
                    throw;
                }
                if (pidlMain != IntPtr.Zero)
                {
                    var iidShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");
                    IShellFolder item = null;
                    try
                    {
                        folder.BindToObject(pidlMain, IntPtr.Zero, ref iidShellFolder, ref item);
                    }
                    catch (Exception)
                    {
                        Marshal.ReleaseComObject(folder);
                        Allocator.Free(pidlMain);
                        throw;
                    }
                    if (item != null)
                    {
                        IEnumIDList idEnum = null;
                        try
                        {
                            item.EnumObjects(IntPtr.Zero, Eshcontf.ShcontfFolders | Eshcontf.ShcontfNonfolders,
                                ref idEnum);
                        }
                        catch (Exception)
                        {
                            Marshal.ReleaseComObject(folder);
                            Allocator.Free(pidlMain);
                            throw;
                        }
                        if (idEnum != null)
                        {
                            var pidl = IntPtr.Zero;
                            var fetched = 0;
                            var complete = false;
                            while (!complete)
                            {
                                var hRes = idEnum.Next(1, ref pidl, ref fetched);
                                if (hRes != 0)
                                {
                                    pidl = IntPtr.Zero;
                                    complete = true;
                                }
                                else
                                {
                                    if (_getThumbNail(fileName, pidl, item))
                                    {
                                        complete = true;
                                    }
                                }
                                if (pidl != IntPtr.Zero)
                                {
                                    Allocator.Free(pidl);
                                }
                            }
                            Marshal.ReleaseComObject(idEnum);
                        }
                        Marshal.ReleaseComObject(item);
                    }
                    Allocator.Free(pidlMain);
                }
                Marshal.ReleaseComObject(folder);
            }
            return ThumbNail;
        }

        private bool _getThumbNail(string file, IntPtr pidl, IShellFolder item)
        {
            var hBmp = IntPtr.Zero;
            IExtractImage extractImage = null;
            try
            {
                var pidlPath = PathFromPidl(pidl);
                if (Path.GetFileName(pidlPath).ToUpper().Equals(Path.GetFileName(file).ToUpper()))
                {
                    IUnknown iunk = null;
                    var prgf = 0;
                    var iidExtractImage = new Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1");
                    item.GetUIObjectOf(IntPtr.Zero, 1, ref pidl, ref iidExtractImage, ref prgf, ref iunk);
                    extractImage = (IExtractImage) iunk;
                    if (extractImage != null)
                    {
                        Console.WriteLine("Got an IExtractImage object!");
                        var sz = new SIZE
                        {
                            cx = DesiredSize.Width,
                            cy = DesiredSize.Height
                        };
                        var location = new StringBuilder(260, 260);
                        var priority = 0;
                        var requestedColourDepth = 32;
                        var flags = Eieiflag.IeiflagAspect | Eieiflag.IeiflagScreen;
                        var uFlags = (int) flags;
                        try
                        {
                            extractImage.GetLocation(location, location.Capacity, ref priority, ref sz,
                                requestedColourDepth, ref uFlags);
                            extractImage.Extract(ref hBmp);
                        }
                        catch (COMException)
                        {
                        }
                        if (hBmp != IntPtr.Zero)
                        {
                            ThumbNail = Image.FromHbitmap(hBmp);
                        }
                        Marshal.ReleaseComObject(extractImage);
                        extractImage = null;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                if (hBmp != IntPtr.Zero)
                {
                    UnmanagedMethods.DeleteObject(hBmp);
                }
                if (extractImage != null)
                {
                    Marshal.ReleaseComObject(extractImage);
                }
                throw;
            }
        }

        private string PathFromPidl(IntPtr pidl)
        {
            var path = new StringBuilder(260, 260);
            var result = UnmanagedMethods.SHGetPathFromIDList(pidl, path);
            if (result == 0)
            {
                return string.Empty;
            }
            return path.ToString();
        }

        [Flags]
        private enum Estrret
        {
        }

        [Flags]
        private enum Eshcontf
        {
            ShcontfFolders = 32,
            ShcontfNonfolders = 64,
            ShcontfIncludehidden = 128
        }

        [Flags]
        private enum Eshgdn
        {
            ShgdnNormal = 0,
            ShgdnInfolder = 1,
            ShgdnForaddressbar = 16384,
            ShgdnForparsing = 32768
        }

        [Flags]
        private enum Esfgao
        {
            SfgaoCancopy = 1,
            SfgaoCanmove = 2,
            SfgaoCanlink = 4,
            SfgaoCanrename = 16,
            SfgaoCandelete = 32,
            SfgaoHaspropsheet = 64,
            SfgaoDroptarget = 256,
            SfgaoCapabilitymask = 375,
            SfgaoLink = 65536,
            SfgaoShare = 131072,
            SfgaoReadonly = 262144,
            SfgaoGhosted = 524288,
            SfgaoDisplayattrmask = 983040,
            SfgaoFilesysancestor = 268435456,
            SfgaoFolder = 536870912,
            SfgaoFilesystem = 1073741824,
            SfgaoHassubfolder = -2147483648,
            SfgaoContentsmask = -2147483648,
            SfgaoValidate = 16777216,
            SfgaoRemovable = 33554432,
            SfgaoCompressed = 67108864
        }

        private enum Eieiflag
        {
            IeiflagAsync = 1,
            IeiflagCache = 2,
            IeiflagAspect = 4,
            IeiflagOffline = 8,
            IeiflagGleam = 16,
            IeiflagScreen = 32,
            IeiflagOrigsize = 64,
            IeiflagNostamp = 128,
            IeiflagNoborder = 256,
            IeiflagQuality = 512
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0, CharSet = CharSet.Auto)]
        private struct StrretCstr
        {
            private readonly Estrret uType;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 520)] private readonly byte[] cStr;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SIZE
        {
            public int cx;
            public int cy;
        }

        [ComImport, Guid("00000000-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IUnknown
        {
            [PreserveSig]
            IntPtr QueryInterface(ref Guid riid, ref IntPtr pVoid);

            [PreserveSig]
            IntPtr AddRef();

            [PreserveSig]
            IntPtr Release();
        }

        [ComImport]
        [Guid("00000002-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMalloc
        {
            [PreserveSig]
            IntPtr Alloc(int cb);

            [PreserveSig]
            IntPtr Realloc(IntPtr pv, int cb);

            [PreserveSig]
            void Free(IntPtr pv);

            [PreserveSig]
            int GetSize(IntPtr pv);

            [PreserveSig]
            int DidAlloc(IntPtr pv);

            [PreserveSig]
            void HeapMinimize();
        }

        [ComImport]
        [Guid("000214F2-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IEnumIDList
        {
            [PreserveSig]
            int Next(int celt, ref IntPtr rgelt, ref int pceltFetched);

            void Skip(int celt);

            void Reset();

            void Clone(ref IEnumIDList ppenum);
        }

        [ComImport]
        [Guid("000214E6-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellFolder
        {
            void ParseDisplayName(IntPtr hwndOwner, IntPtr pbcReserved,
                [MarshalAs(UnmanagedType.LPWStr)] string lpszDisplayName,
                ref int pchEaten, ref IntPtr ppidl, ref int pdwAttributes);

            void EnumObjects(IntPtr hwndOwner,
                [MarshalAs(UnmanagedType.U4)] Eshcontf grfFlags,
                ref IEnumIDList ppenumIdList);

            void BindToObject(IntPtr pidl, IntPtr pbcReserved, ref Guid riid,
                ref IShellFolder ppvOut);

            void BindToStorage(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, IntPtr ppvObj);

            [PreserveSig]
            int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);

            void CreateViewObject(IntPtr hwndOwner, ref Guid riid,
                IntPtr ppvOut);

            void GetAttributesOf(int cidl, IntPtr apidl,
                [MarshalAs(UnmanagedType.U4)] ref Esfgao rgfInOut);

            void GetUIObjectOf(IntPtr hwndOwner, int cidl, ref IntPtr apidl, ref Guid riid, ref int prgfInOut,
                ref IUnknown ppvOut);

            void GetDisplayNameOf(IntPtr pidl,
                [MarshalAs(UnmanagedType.U4)] Eshgdn uFlags,
                ref StrretCstr lpName);

            void SetNameOf(IntPtr hwndOwner, IntPtr pidl,
                [MarshalAs(UnmanagedType.LPWStr)] string lpszName,
                [MarshalAs(UnmanagedType.U4)] Eshcontf uFlags,
                ref IntPtr ppidlOut);
        }

        [ComImport, Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IExtractImage
        {
            void GetLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPathBuffer, int cch,
                ref int pdwPriority, ref SIZE prgSize, int dwRecClrDepth, ref int pdwFlags);

            void Extract(ref IntPtr phBmpThumbnail);
        }

        private class UnmanagedMethods
        {
            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal static extern int SHGetMalloc(ref IMalloc ppMalloc);

            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal static extern int SHGetDesktopFolder(ref IShellFolder ppshf);

            [DllImport("shell32", CharSet = CharSet.Auto)]
            internal static extern int SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);

            [DllImport("gdi32", CharSet = CharSet.Auto)]
            internal static extern int DeleteObject(IntPtr hObject);
        }
    }
}