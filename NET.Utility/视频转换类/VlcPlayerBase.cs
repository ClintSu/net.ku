using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace NET.Utilities
{
    public class VlcPlayerBase
    {
        private IntPtr libvlc_instance_;
        private IntPtr libvlc_media_player_;     
        private double duration_;

        /// <summary>
        /// VLC 播放器。
        /// </summary>
        /// <param name="pluginPath"></param>
        public VlcPlayerBase(string pluginPath)
        {
            //string pluginPath = Environment.CurrentDirectory + "\\vlc\\plugins\\";  //插件目录
            string plugin_arg = "--plugin-path=" + pluginPath;
            string[] arguments = { "-I", "dummy", "--ignore-config", "--no-video-title", "--no-osd", plugin_arg };
            libvlc_instance_ = LibVlcAPI.libvlc_new(arguments);

            libvlc_media_player_ = LibVlcAPI.libvlc_media_player_new(libvlc_instance_);  //创建 libvlc_media_player 播放核心
        }

        /// <summary>
        /// 设置播放容器
        /// </summary>
        /// <param name="wndHandle">播放容器句柄</param>
        public void SetRenderWindow(int wndHandle)
        {
            if (libvlc_instance_ != IntPtr.Zero && wndHandle != 0)
            {
                LibVlcAPI.libvlc_media_player_set_hwnd(libvlc_media_player_, wndHandle);  //设置播放容器
            }
        }

        /// <summary>
        /// 播放指定媒体文件
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadFile(string filePath)
        {
            //IntPtr libvlc_media = LibVlcAPI.libvlc_media_new_path(libvlc_instance_, filePath);  //创建 libvlc_media_player 播放核心
            IntPtr libvlc_media = IntPtr.Zero;
            if (filePath.StartsWith("http"))
            {
                //在线播放
                libvlc_media = LibVlcAPI.libvlc_media_new_location(libvlc_instance_, filePath);
            }
            else
            {
                //本地播放
                libvlc_media = LibVlcAPI.libvlc_media_new_path(libvlc_instance_, filePath);
            }

            if (libvlc_media != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_media_parse(libvlc_media);
                duration_ = LibVlcAPI.libvlc_media_get_duration(libvlc_media) / 1000.0;  //获取视频时长

                LibVlcAPI.libvlc_media_player_set_media(libvlc_media_player_, libvlc_media);  //将视频绑定到播放器去
                LibVlcAPI.libvlc_media_release(libvlc_media);

                LibVlcAPI.libvlc_media_player_play(libvlc_media_player_);  //播放
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            if (libvlc_media_player_ != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_media_player_play(libvlc_media_player_);
            }
        }

        /// <summary>
        /// 暂停播放
        /// </summary>
        public void Pause()
        {
            if (libvlc_media_player_ != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_media_player_pause(libvlc_media_player_);
            }
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        public void Stop()
        {
            if (libvlc_media_player_ != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_media_player_stop(libvlc_media_player_);
            }
        }

        /// <summary>
        /// 释放播放
        /// </summary>
        public void Release()
        {
            if (libvlc_media_player_ != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_media_release(libvlc_media_player_);
            }
            if (libvlc_instance_ != IntPtr.Zero)
            {
                LibVlcAPI.libvlc_release(libvlc_instance_);
            }
        }

        /// <summary>
        /// 获取播放状态
        /// </summary>
        /// <returns></returns>
        public long GetPlayState()
        {
            return LibVlcAPI.libvlc_media_player_get_state(libvlc_media_player_);

        }

        /// <summary>
        /// 获取播放时间进度
        /// </summary>
        /// <returns></returns>
        public double GetPlayTime()
        {
            return LibVlcAPI.libvlc_media_player_get_time(libvlc_media_player_) / 1000.0;
        }

        /// <summary>
        /// 设置播放时间
        /// </summary>
        /// <param name="seekTime"></param>
        public void SetPlayTime(double seekTime)
        {
            LibVlcAPI.libvlc_media_player_set_time(libvlc_media_player_, (Int64)(seekTime * 1000));
        }
        /// <summary>
        /// 设置播放速度
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public int SetPlaySpeed(float speed)
        {
            return LibVlcAPI.libvlc_media_player_set_rate(libvlc_media_player_, speed);
        }

        /// <summary>
        /// 抓图(支持多种格式)
        /// </summary>
        /// <param name="savePath">完整路径（bmp,png,jpg）</param>
        /// <param name="width">抓图宽度</param>
        /// <param name="height">抓图高度</param>
        /// <returns></returns>
        public int SnapShot(string savePath, uint width = 0, uint height = 0)
        {
            int result = -1;
            if (libvlc_media_player_ != IntPtr.Zero)
            {
                IntPtr pathPtr = StringToPtr(savePath);
                result = LibVlcAPI.libvlc_video_take_snapshot(libvlc_media_player_, 0, pathPtr, width, height);
                Marshal.FreeHGlobal(pathPtr);

            }
            return result;
        }

        /// <summary>
        /// 字符串转指针
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public IntPtr StringToPtr(string str)
        {
            byte[] bs = Encoding.UTF8.GetBytes(str);

            List<byte> list = new List<byte>(100);
            list.AddRange(bs);
            list.Add((byte)0);//必须末尾增加0，c语言字符串以0结束，否则文件名乱码

            IntPtr pathPtr = Marshal.AllocHGlobal(list.Count);
            Marshal.Copy(list.ToArray(), 0, pathPtr, list.Count);
            return pathPtr;
        }

        /// <summary>
        /// 获取音量
        /// </summary>
        /// <returns></returns>
        public int GetVolume()
        {
            return LibVlcAPI.libvlc_audio_get_volume(libvlc_media_player_);
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(int volume)
        {
            LibVlcAPI.libvlc_audio_set_volume(libvlc_media_player_, volume);
        }

        /// <summary>
        /// 设置是否全屏
        /// </summary>
        /// <param name="istrue"></param>
        public void SetFullScreen(bool istrue)
        {
            LibVlcAPI.libvlc_set_fullscreen(libvlc_media_player_, istrue ? 1 : 0);
        }

        /// <summary>
        /// 视频时长
        /// </summary>
        /// <returns></returns>
        public double Duration { get { return duration_; } }

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                if (Duration > 0 && (int)GetPlayTime() == (int)Duration) this.Stop();  //如果播放完，关闭视频

                return (int)GetPlayTime() < (int)Duration /* 播放时间进度小于视频时长 */
                    && Duration > 0 /* 播放时间进度大于0 */
                    && GetPlayTime() > 0; /* 视频时长大于0 */
            }
        }

        /// <summary>
        /// 获取版本（VS2015 调试模式程序会直接崩掉）
        /// </summary>
        /// <returns></returns>
        public string Version { get { return LibVlcAPI.libvlc_get_version(); } }

    }

    #region vlclib.dll
    internal static class LibVlcAPI
    {
        internal struct PointerToArrayOfPointerHelper
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public IntPtr[] pointers;
        }

        /// <summary>
        /// 传入播放参数
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static IntPtr libvlc_new(string[] arguments)
        {
            PointerToArrayOfPointerHelper argv = new PointerToArrayOfPointerHelper();
            argv.pointers = new IntPtr[11];

            for (int i = 0; i < arguments.Length; i++)
            {
                argv.pointers[i] = Marshal.StringToHGlobalAnsi(arguments[i]);  //将托管 System.String 中的内容复制到非托管内存，并在复制时转换为 ANSI 格式。
            }

            IntPtr argvPtr = IntPtr.Zero;
            try
            {
                int size = Marshal.SizeOf(typeof(PointerToArrayOfPointerHelper));  //返回非托管类型的大小（以字节为单位）。
                argvPtr = Marshal.AllocHGlobal(size);  //从进程的非托管内存中分配内存。
                Marshal.StructureToPtr(argv, argvPtr, false);  //将数据从托管对象封送到非托管内存块。

                return libvlc_new(arguments.Length, argvPtr);  //创建一个libvlc实例，它是引用计数的
            }
            finally
            {
                for (int i = 0; i < arguments.Length + 1; i++)
                {
                    if (argv.pointers[i] != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(argv.pointers[i]);  //释放以前使用 System.Runtime.InteropServices.Marshal.AllocHGlobal(System.IntPtr) 从进程的非托管内存中分配的内存。
                    }
                }

                if (argvPtr != IntPtr.Zero) { Marshal.FreeHGlobal(argvPtr);/* 释放以前使用 System.Runtime.InteropServices.Marshal.AllocHGlobal(System.IntPtr) 从进程的非托管内存中分配的内存。 */ }
            }
        }

        /// <summary>
        /// 从本地文件系统路径新建,其他参照上一条
        /// </summary>
        /// <param name="libvlc_instance"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IntPtr libvlc_media_new_path(IntPtr libvlc_instance, string path)
        {
            IntPtr pMrl = IntPtr.Zero;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(path);
                pMrl = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, pMrl, bytes.Length);
                Marshal.WriteByte(pMrl, bytes.Length, 0);
                return libvlc_media_new_path(libvlc_instance, pMrl);  // 从本地文件路径构建一个libvlc_media
            }
            finally
            {
                if (pMrl != IntPtr.Zero) { Marshal.FreeHGlobal(pMrl);/* 释放以前使用 System.Runtime.InteropServices.Marshal.AllocHGlobal(System.IntPtr) 从进程的非托管内存中分配的内存。 */                }
            }
        }

        /// <summary>
        /// 使用一个给定的媒体资源路径来建立一个libvlc_media对象.参数psz_mrl为要读取的MRL(Media Resource Location).此函数返回新建的对象或NULL.
        /// </summary>
        /// <param name="libvlc_instance"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IntPtr libvlc_media_new_location(IntPtr libvlc_instance, string path)
        {
            IntPtr pMrl = IntPtr.Zero;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(path);
                pMrl = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, pMrl, bytes.Length);
                Marshal.WriteByte(pMrl, bytes.Length, 0);
                return libvlc_media_new_path(libvlc_instance, pMrl);  // 从本地文件路径构建一个libvlc_media
            }
            finally
            {
                if (pMrl != IntPtr.Zero) { Marshal.FreeHGlobal(pMrl);/* 释放以前使用 System.Runtime.InteropServices.Marshal.AllocHGlobal(System.IntPtr) 从进程的非托管内存中分配的内存。 */                }
            }
        }

        // ----------------------------------------------------------------------------------------
        // 以下是libvlc.dll导出函数

        /// <summary>
        /// 创建一个libvlc实例，它是引用计数的 
        /// </summary>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_new(int argc, IntPtr argv);

        /// <summary>
        /// 释放libvlc实例 
        /// </summary>
        /// <param name="libvlc_instance"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_release(IntPtr libvlc_instance);

        /// <summary>
        /// 获取版本 
        /// </summary>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern String libvlc_get_version();

        /// <summary>
        /// 从视频来源(例如Url)构建一个libvlc_meida 
        /// </summary>
        /// <param name="libvlc_instance"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_new_location(IntPtr libvlc_instance, IntPtr path);

        /// <summary>
        /// 从本地文件路径构建一个libvlc_media 
        /// </summary>
        /// <param name="libvlc_instance"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_new_path(IntPtr libvlc_instance, IntPtr path);

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="libvlc_media_inst"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_release(IntPtr libvlc_media_inst);

        /// <summary>
        /// 创建libvlc_media_player(播放核心) 
        /// </summary>
        /// <param name="libvlc_instance"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_player_new(IntPtr libvlc_instance);

        /// <summary>
        /// 将视频(libvlc_media)绑定到播放器上 
        /// </summary>
        /// <param name="libvlc_media_player"></param>
        /// <param name="libvlc_media"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_media(IntPtr libvlc_media_player, IntPtr libvlc_media);

        /// <summary>
        /// 设置图像输出的窗口 
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        /// <param name="drawable"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_hwnd(IntPtr libvlc_mediaplayer, Int32 drawable);

        #region 播放控制
        /// <summary>
        /// 播放 
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_play(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// 暂停 
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_pause(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// 停止 
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_stop(IntPtr libvlc_mediaplayer);
        #endregion

        /// <summary>
        /// 释放播放文件 
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_release(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// 解析视频资源的媒体信息(如时长等) 
        /// </summary>
        /// <param name="libvlc_media"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_parse(IntPtr libvlc_media);

        /// <summary>
        /// 返回视频的时长(必须先调用libvlc_media_parse之后，该函数才会生效) 
        /// </summary>
        /// <param name="libvlc_media"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_get_duration(IntPtr libvlc_media);


        #region 播放时间进度 

        /// <summary>
        /// 当前播放的时间进度 
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_player_get_time(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// 设置播放位置(拖动) 
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        /// <param name="time"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_time(IntPtr libvlc_mediaplayer, Int64 time);

        #endregion

        #region 音量

        /// <summary>
        /// 获取音量 
        /// </summary>
        /// <param name="libvlc_media_player"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int libvlc_audio_get_volume(IntPtr libvlc_media_player);

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="libvlc_media_player"></param>
        /// <param name="volume"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_audio_set_volume(IntPtr libvlc_media_player, int volume);

        #endregion

        /// <summary>
        /// 设置全屏
        /// </summary>
        /// <param name="libvlc_media_player"></param>
        /// <param name="isFullScreen"></param>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_set_fullscreen(IntPtr libvlc_media_player, int isFullScreen);

        /// <summary>
        /// 获取播放状态。（Win10 不支持）
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_player_get_state(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// 设置播放速度
        /// </summary>
        /// <param name="libvlc_media"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int libvlc_media_player_set_rate(IntPtr libvlc_media, float rate);

        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        /// <param name="num">经典0</param>
        /// <param name="filePath">完整路径，文件名英文或下划线开头</param>
        /// <param name="i_width"></param>
        /// <param name="i_height"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_take_snapshot(IntPtr libvlc_mediaplayer, uint num, IntPtr filePath, uint i_width, uint i_height);
    }
    #endregion
}
