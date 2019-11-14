﻿using System;
//using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VideoPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow:Window
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public override string ToString()
            {
                return ("X:" + X + ", Y:" + Y);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        #region 字段、属性

        private bool isMax = false;
        double menuLineY = 0;
        double footerLineY = 0;
        double videoListX = 0;
        double videoListY1 = 0;
        double videoListY2 = 0;
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            // Default installation path of VideoLAN.LibVLC.Windows
            //var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", System.Environment.Is64BitOperatingSystem == false ? "win-x86" : "win-x64"));
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            this.VlcControl.SourceProvider.CreatePlayer(libDirectory/* pass your player parameters here */);
            this.VlcControl.SourceProvider.MediaPlayer.Play(new Uri(@"D:\vue\vue\Vue高级实战-移动端音乐WebApp\第一章 课程内容介绍\1-1导学.mp4"));
            //this.VlcControl.SourceProvider.MediaPlayer.Play(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));

            this.BtnPlay.Visibility = Visibility.Collapsed;
            this.BtnSuspend.Visibility = Visibility.Visible;

            this.VlcControl.SourceProvider.MediaPlayer.AudioVolume += MediaPlayer_AudioVolume;
            this.VlcControl.SourceProvider.MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;


            this.Loaded += MainWindow_Loaded;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var t1=this.Height;
            //var t2= SystemParameters.PrimaryScreenHeight;
            //var t3 = SystemParameters.WorkArea.Height;
            //menu_height = this.TopMenu.Height;
            //footer_height = this.BottomFooter.Height;
            menuLineY = this.TopMenu.Height;
            footerLineY = SystemParameters.PrimaryScreenHeight - this.TopMenu.Height;
            videoListX = SystemParameters.PrimaryScreenWidth - this.VideoList.Width;
            videoListY1 = this.TopMenu.Height + 50;
            videoListY2 = SystemParameters.PrimaryScreenHeight - this.TopMenu.Height - 50;
        }

        #region 全屏鼠标移动显示

        private void MediaPlayer_TimeChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs e)
        {
            var t = e.NewTime;
        }

        private void MediaPlayer_AudioVolume(object sender, Vlc.DotNet.Core.VlcMediaPlayerAudioVolumeEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void TopMenu_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(!isMax) return;
            Point pt = e.GetPosition(this);
            if (pt.Y <= menuLineY)
            {
                if(this.Menu.Visibility == Visibility.Collapsed)
                {
                    this.Menu.Visibility = Visibility.Visible;
                    Task.Factory.StartNew(() => { MenuTimeMouse(); });
                }
            }
            else
            {
                this.Menu.Visibility = Visibility.Collapsed;
            }
            Console.WriteLine("TopMenu:"+ pt.Y+" "+ menuLineY);
        }
        protected void MenuTimeMouse()
        {
            POINT mouse = new POINT();
            while (isMax)
            {
                try
                {
                    GetCursorPos(out mouse);
                    if (mouse.Y <= menuLineY)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        this.Dispatcher.Invoke(new Action(() => { this.Menu.Visibility = Visibility.Collapsed; }));
                        break;
                    }
                    Console.WriteLine("TopMenu-k:" + mouse.Y + " " + menuLineY);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BottomFooter_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!isMax) return;
            Point pt = e.GetPosition(this);
            if (pt.Y >= footerLineY)
            {
                if (this.Footer.Visibility == Visibility.Collapsed)
                {
                    this.Footer.Visibility = Visibility.Visible;
                    Task.Factory.StartNew(() => { FooterTimeMouse(); });
                }
            }
            else
            {
                this.Footer.Visibility = Visibility.Collapsed;
            }

            Console.WriteLine("BottomFooter:" + pt.Y + " " + footerLineY);
        }
        protected void FooterTimeMouse()
        {
            POINT mouse = new POINT();
            while (isMax)
            {
                try
                {
                    GetCursorPos(out mouse);
                    if (mouse.Y >= footerLineY)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        this.Dispatcher.Invoke(new Action(() => { this.Footer.Visibility = Visibility.Collapsed; }));
                        break;
                    }

                    Console.WriteLine("BottomFooter-k:" + mouse.Y + " " + footerLineY);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void RightVideoList_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMax) return;
            Point pt = e.GetPosition(this);
            if (pt.X >= videoListX && pt.Y >= videoListY1 && pt.Y <= videoListY2)
            {
                if (this.VideoList.Visibility == Visibility.Collapsed)
                {
                    this.VideoList.Visibility = Visibility.Visible;
                    Task.Factory.StartNew(() => { VideoTimeMouse(); });
                }
            }
            else
            {
                this.VideoList.Visibility = Visibility.Collapsed;
            }
        }
        protected void VideoTimeMouse()
        {
            POINT mouse = new POINT();
            while (isMax)
            {
                try
                {
                    GetCursorPos(out mouse);
                    if (mouse.X >= videoListX && mouse.Y >= videoListY1 && mouse.Y <= videoListY2)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        this.Dispatcher.Invoke(new Action(() => { this.VideoList.Visibility = Visibility.Collapsed; }));
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        #endregion

        #region 菜单栏

        private void BtnTopmost_Click(object sender, RoutedEventArgs e)
        {
            this.isMax = false;
            this.Topmost = true;
            this.BtnTopmost.Visibility = Visibility.Collapsed;
            this.BtnNoTopmost.Visibility = Visibility.Visible;
            ShowTipMessage("最前端");
        }

        private void BtnNoTopmost_Click(object sender, RoutedEventArgs e)
        {
            this.isMax = false;
            this.Topmost = false;
            this.BtnTopmost.Visibility = Visibility.Visible;
            this.BtnNoTopmost.Visibility = Visibility.Collapsed;
            ShowTipMessage("最前端 关闭");
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.isMax = false;
            this.WindowState = WindowState.Minimized;
        }

        private void BtnFull_Click(object sender, RoutedEventArgs e)
        {
            this.isMax = true;
            if (this.Topmost == false)
            {
                this.Topmost = true;
                this.BtnTopmost.Visibility = Visibility.Collapsed;
                this.BtnNoTopmost.Visibility = Visibility.Visible;
            }
            this.BtnFull.Visibility = Visibility.Collapsed;
            this.BtnNoFull.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Maximized;

            this.VlcControl.Margin = new Thickness(0);
            this.RightVideoList.Margin = new Thickness(0);
            this.Menu.Visibility = Visibility.Collapsed;
            this.Footer.Visibility = Visibility.Collapsed;
            this.VideoList.Visibility = Visibility.Collapsed;

            ShowTipMessage("全屏 最前端");
        }

        private void BtnNoFull_Click(object sender, RoutedEventArgs e)
        {
            this.isMax = false;   
            this.BtnFull.Visibility = Visibility.Visible;
            this.BtnNoFull.Visibility = Visibility.Collapsed;
            this.WindowState = WindowState.Normal;

            this.VlcControl.Margin = new Thickness(0, 35, 300, 60);
            this.RightVideoList.Margin = new Thickness(0, 35, 0, 60);
            this.Menu.Visibility = Visibility.Visible;
            this.Footer.Visibility = Visibility.Visible;
            this.VideoList.Visibility = Visibility.Visible;

            ShowTipMessage("全屏 关闭");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {   
            this.Close();
        }
        #endregion

        #region 左下工具栏

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(!this.VlcControl.SourceProvider.MediaPlayer.IsPlaying())
            {
                this.VlcControl.SourceProvider.MediaPlayer.Play();
                this.BtnPlay.Visibility = Visibility.Collapsed;
                this.BtnSuspend.Visibility = Visibility.Visible;
            }
               
        }

        private void BtnSuspend_Click(object sender, RoutedEventArgs e)
        {
            if (this.VlcControl.SourceProvider.MediaPlayer.IsPlaying())
            {
                this.VlcControl.SourceProvider.MediaPlayer.Pause();
                this.BtnPlay.Visibility = Visibility.Visible;
                this.BtnSuspend.Visibility = Visibility.Collapsed;
            }
                
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            //this.VlcControl.SourceProvider.MediaPlayer
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region 右下工具栏

        private void BtnCutPicture_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCutVideo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnVideoList_Click(object sender, RoutedEventArgs e)
        {
            if (this.VideoList.Visibility == Visibility.Collapsed)
            {
                this.VideoList.Visibility = Visibility.Visible;
                this.VlcControl.Margin = new Thickness(0, 35, 300, 60);
            }
            else
            {
                this.VideoList.Visibility = Visibility.Collapsed;
                this.VlcControl.Margin = new Thickness(0);
            }
        }
        #endregion

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="tipMessage"></param>
        private void ShowTipMessage(string tipMessage)
        {
            this.TbMessage.Visibility = Visibility.Visible;
            this.TbMessage.Text = tipMessage;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    System.Threading.Thread.Sleep(2000);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.TbMessage.Visibility = Visibility.Collapsed;
                    }));
                }
                catch (Exception ex)
                {
                    //shengqin.Common.LogHelper.WriteLog(ex.Message, ex);
                }
            });
        }

        /// <summary>
        /// 双击全屏、退出全屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VlcControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (isMax)
            {
                BtnNoFull_Click(null, null);
            }
            else
            {
                BtnFull_Click(null, null);
            }
        }

        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
