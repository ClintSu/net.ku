using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utilities
{
    public class ffmpegHelper
    {
        /*
        //a) 通用选项
        -L license
        -h 帮助
        -fromats 显示可用的格式，编解码的，协议的...
        -f fmt 强迫采用格式fmt
        -I filename 输入文件
        -y 覆盖输出文件
        -t duration 设置纪录时间 hh:mm:ss[.xxx] 格式的记录时间也支持
        -ss position 搜索到指定的时间[-] hh:mm:ss[.xxx] 的格式也支持
        -title string 设置标题
        -author string 设置作者
        -copyright string 设置版权
        -comment string 设置评论
        -target type 设置目标文件类型(vcd, svcd, dvd) 所有的格式选项（比特率，编解码以及缓冲区大小）自动设置，只需要输入如下的就可以了：ffmpeg -i myfile.avi -target vcd /tmp/vcd.mpg
        -hq 激活高质量设置
        -itsoffset offset 设置以秒为基准的时间偏移，该选项影响所有后面的输入文件。该偏移被加到输入文件的时戳，定义一个正偏移意味着相应的流被延迟了 offset秒。 [-]hh:mm:ss[.xxx] 的格式也支持

        //b) 视频选项   
        -b bitrate 设置比特率，缺省200kb/s
        -r fps 设置帧频 缺省25
        -s size 设置帧大小 格式为WXH 缺省160X128.下面的简写也可以直接使用：
        Sqcif 128X96 qcif 176X144 cif 252X288 4cif 704X576
        -aspect aspect 设置横纵比 4:3 16:9 或 1.3333 1.7777
        -croptop size 设置顶部切除带大小 像素单位
        -cropbottom size –cropleft size –cropright size
        -padtop size 设置顶部补齐的大小 像素单位
        -padbottom size –padleft size –padright size –padcolor color 设置补齐条颜色(hex,6个16进制的数，红:绿:兰排列，比如 000000代表黑色)
        -vn 不做视频记录
        -bt tolerance 设置视频码率容忍度kbit/s
        -maxrate bitrate设置最大视频码率容忍度
        -minrate bitreate 设置最小视频码率容忍度
        -bufsize size 设置码率控制缓冲区大小
        -vcodec codec 强制使用codec编解码方式。如果用copy表示原始编解码数据必须被拷贝。
        -sameq 使用同样视频质量作为源（VBR）
        -pass n 选择处理遍数（1或者2）。两遍编码非常有用。第一遍生成统计信息，第二遍生成精确的请求的码率
        -passlogfile file 选择两遍的纪录文件名为file

        //c)高级视频选项
        -g gop_size 设置图像组大小
        -intra 仅适用帧内编码
        -qscale q 使用固定的视频量化标度(VBR)
        -qmin q 最小视频量化标度(VBR)
        -qmax q 最大视频量化标度(VBR)
        -qdiff q 量化标度间最大偏差(VBR)
        -qblur blur 视频量化标度柔化(VBR)
        -qcomp compression 视频量化标度压缩(VBR)
        -rc_init_cplx complexity 一遍编码的初始复杂度
        -b_qfactor factor 在p和b帧间的qp因子
        -i_qfactor factor 在p和i帧间的qp因子
        -b_qoffset offset 在p和b帧间的qp偏差
        -i_qoffset offset 在p和i帧间的qp偏差
        -rc_eq equation 设置码率控制方程 默认tex^qComp
        -rc_override override 特定间隔下的速率控制重载
        -me method 设置运动估计的方法 可用方法有 zero phods log x1 epzs(缺省) full
        -dct_algo algo 设置dct的算法 可用的有 0 FF_DCT_AUTO 缺省的DCT 1 FF_DCT_FASTINT 2 FF_DCT_INT 3 FF_DCT_MMX 4 FF_DCT_MLIB 5 FF_DCT_ALTIVEC
        -idct_algo algo 设置idct算法。可用的有 0 FF_IDCT_AUTO 缺省的IDCT 1 FF_IDCT_INT 2 FF_IDCT_SIMPLE 3 FF_IDCT_SIMPLEMMX 4 FF_IDCT_LIBMPEG2MMX 5 FF_IDCT_PS2 6 FF_IDCT_MLIB 7 FF_IDCT_ARM 8 FF_IDCT_ALTIVEC 9 FF_IDCT_SH4 10 FF_IDCT_SIMPLEARM
        -er n 设置错误残留为n 1 FF_ER_CAREFULL 缺省 2 FF_ER_COMPLIANT 3 FF_ER_AGGRESSIVE 4 FF_ER_VERY_AGGRESSIVE
        -ec bit_mask 设置错误掩蔽为bit_mask,该值为如下值的位掩码 1 FF_EC_GUESS_MVS(default=enabled) 2 FF_EC_DEBLOCK(default=enabled)
        -bf frames 使用frames B 帧，支持mpeg1,mpeg2,mpeg4
        -mbd mode 宏块决策 0 FF_MB_DECISION_SIMPLE 使用mb_cmp 1 FF_MB_DECISION_BITS 2 FF_MB_DECISION_RD
        -4mv 使用4个运动矢量 仅用于mpeg4
        -part 使用数据划分 仅用于mpeg4
        -bug param 绕过没有被自动监测到编码器的问题
        -strict strictness 跟标准的严格性
        -aic 使能高级帧内编码 h263+
        -umv 使能无限运动矢量 h263+
        -deinterlace 不采用交织方法
        -interlace 强迫交织法编码仅对mpeg2和mpeg4有效。当你的输入是交织的并且你想要保持交织以最小图像损失的时候采用该选项。可选的方法是不交织，但是损失更大
        -psnr 计算压缩帧的psnr
        -vstats 输出视频编码统计到vstats_hhmmss.log
        -vhook module 插入视频处理模块 module 包括了模块名和参数，用空格分开

        //d)音频选项
        -ab bitrate 设置音频码率
        -ar freq 设置音频采样率
        -ac channels 设置通道 缺省为1
        -an 不使能音频纪录
        -acodec codec 使用codec编解码

        //e)音频/视频捕获选项
        -vd device 设置视频捕获设备。比如/dev/video0
        -vc channel 设置视频捕获通道 DV1394专用
        -tvstd standard 设置电视标准 NTSC PAL(SECAM)
        -dv1394 设置DV1394捕获
        -av device 设置音频设备 比如/dev/dsp

        //f)高级选项
        -map file:stream 设置输入流映射
        -debug 打印特定调试信息
        -benchmark 为基准测试加入时间
        -hex 倾倒每一个输入包
        -bitexact 仅使用位精确算法 用于编解码测试
        -ps size 设置包大小，以bits为单位
        -re 以本地帧频读数据，主要用于模拟捕获设备
        -loop 循环输入流（只工作于图像流，用于ffserver测试）
         */

        /*
        // 去掉视频中的音频
        ffmpeg -i input.mp4 -vcodec copy -an output.mp4
        // -an: 去掉音频；-vcodec:视频选项，一般后面加copy表示拷贝

        // 提取视频中的音频
        ffmpeg -i input.mp4 -acodec copy -vn output.mp3
        // -vn: 去掉视频；-acodec: 音频选项， 一般后面加copy表示拷贝

        // 音视频合成
        ffmpeg -y –i input.mp4 –i input.mp3 –vcodec copy –acodec copy output.mp4
        // -y 覆盖输出文件

        // 剪切视频
        ffmpeg -ss 0:1:30 -t 0:0:20 -i input.mp4 -vcodec copy -acodec copy output.mp4
        // -ss 开始时间; -t 持续时间

        // 视频截图
        ffmpeg –i test.mp4 –f image2 -t 0.001 -s 320x240 image-%3d.jpg
        // -s 设置分辨率; -f 强迫采用格式fmt;

        // 视频分解为图片
        ffmpeg –i test.mp4 –r 1 –f image2 image-%3d.jpg
        // -r 指定截屏频率

        // 将图片合成视频
        ffmpeg -f image2 -i image%d.jpg output.mp4

        // 视频拼接
        ffmpeg -f concat -i filelist.txt -c copy output.mp4

        // 将视频转为gif
        ffmpeg -i input.mp4 -ss 0:0:30 -t 10 -s 320x240 -pix_fmt rgb24 output.gif
        // -pix_fmt 指定编码

        // 将视频前30帧转为gif
        ffmpeg -i input.mp4 -vframes 30 -f gif output.gif

        // 旋转视频
        ffmpeg -i input.mp4 -vf rotate = PI / 2 output.mp4

        // 缩放视频
        ffmpeg -i input.mp4 -vf scale = iw / 2:-1 output.mp4
        // iw 是输入的宽度， iw/2就是一半;-1 为保持宽高比

        // 视频变速
        ffmpeg -i input.mp4 -filter:v setpts = 0.5 * PTS output.mp4

        // 音频变速
        ffmpeg -i input.mp3 -filter:a atempo = 2.0 output.mp3

        // 音视频同时变速，但是音视频为互倒关系
        ffmpeg -i input.mp4 -filter_complex "[0:v]setpts=0.5*PTS[v];[0:a]atempo=2.0[a]" -map "[v]" -map "[a]" output.mp4

        // 视频添加水印
        ffmpeg -i input.mp4 -i logo.jpg -filter_complex[0:v][1:v] overlay = main_w - overlay_w - 10:main_h-overlay_h-10[out] -map[out] -map 0:a -codec:a copy output.mp4
        // main_w-overlay_w-10 视频的宽度-水印的宽度-水印边距；

        // 截取视频局部
        ffmpeg -i in.mp4 -filter:v "crop=out_w:out_h:x:y" out.mp4

        // 截取部分视频，从[80,60]的位置开始，截取宽200，高100的视频
        ffmpeg -i in.mp4 -filter:v "crop=80:60:200:100" -c:a copy out.mp4

        // 截取右下角的四分之一
        ffmpeg -i in.mp4 -filter:v "crop=in_w/2:in_h/2:in_w/2:in_h/2" -c:a copy out.mp4

        // 截去底部40像素高度
        ffmpeg -i in.mp4 -filter:v "crop=in_w:in_h-40" -c:a copy out.mp4

        //视频转码
        ffmpeg -i out.ogv -vcodec h264 out.mp4
        ffmpeg -i out.ogv -vcodec mpeg4 out.mp4
        ffmpeg -i out.ogv -vcodec libxvid out.mp4
        ffmpeg -i out.mp4 -vcodec wmv1 out.wmv
        ffmpeg -i out.mp4 -vcodec wmv2 out.wmv
        */

        private static string ffmpegDir = "\\ffmpeg\\ffmpeg.exe";

        /// <summary>
        /// 视频封面
        /// </summary>
        /// <param name="sourceDir">源文件</param>
        /// <param name="thumbnailDir">封面图</param>
        /// <param name="ratio">转码类型【h264\mpeg4\libxvid\wmv1\wmv2】</param>
        public void VideoConvert(string sourceDir, string targetDir, string encoding = "h264")
        {
            string strArg = $" -i {sourceDir} -vcodec {encoding} {targetDir}";
            StartFFmpeg(strArg);
            System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// 视频封面
        /// </summary>
        /// <param name="sourceDir">源文件</param>
        /// <param name="thumbnailDir">封面图</param>
        /// <param name="ratio">分辨率【3840*2160、2560*1440、1920*1080、1280*720】</param>
        public void VideoPictrue(string sourceDir, string thumbnailDir, string ratio = "1920*1080")
        {
            string strArg = $" -i -y {sourceDir} -f image2 -t 0.001 -s {ratio} {thumbnailDir}";
            StartFFmpeg(strArg);
            System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// 压缩视频
        /// </summary>
        /// <param name="sourceDir">源文件</param>
        /// <param name="targetDir">目标文件</param>
        /// <param name="ratio">分辨率【1920*1080、1280*720、960*540、640*360】</param>
        public void VideoCompress(string sourceDir, string targetDir, string ratio = "640*360")
        {
            string strArg = $" -i -y {sourceDir} -s  {ratio} -vcodec libx264 -b {targetDir}";
            StartFFmpeg(strArg);
            System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// 视频剪切
        /// </summary>
        /// <param name="sourceDir">原文件</param>
        /// <param name="targetDir">目标文件</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        public void VideoCut(string sourceDir, string targetDir, string beginTime, string endTime)
        {
            string strArg = $" -i -y {sourceDir} -ss {beginTime} -t {endTime} -acodec copy -vcodec copy {targetDir}";
            StartFFmpeg(strArg);
            System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// 视频合并
        /// </summary>
        /// <param name="filetxt">源文件txt列表</param>
        /// <param name="targetDir">目标文件</param>
        /// <remarks>file '1.mp4' \n file '2.mp4'</remarks>
        public void VideoMerge(string filetxt, string targetDir)
        {
            string strArg = $" -f -y concat -safe 0 -i {filetxt} -c copy {targetDir}";
            StartFFmpeg(strArg);
            System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="strArg"></param>
        public void StartFFmpeg(string strArg)
        {
            Process p = new Process();
            p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + ffmpegDir;
            p.StartInfo.Arguments = strArg; //参数
            p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
            p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用StandardOutput是捕获不到任何消息的...这是我耗费了2个多月得出来的经验...mencoder就是用standardOutput来捕获的)
            p.StartInfo.CreateNoWindow = true;//不创建进程窗口
            p.ErrorDataReceived += new DataReceivedEventHandler(Output);//外部程序(这里是FFMPEG)输出流时候产生的事件,这里是把流的处理过程转移到下面的方法中,详细请查阅MSDN
            p.Start();//启动线程
            p.BeginErrorReadLine();//开始异步读取
            //p.WaitForExit();//阻塞等待进程结束
            //p.Close();//关闭进程
            p.Dispose();//释放资源
        }
        /// <summary>
        /// 强制关闭
        /// </summary>
        /// <param name="procName"></param>
        public void KillFFmpeg(string procName = "ffmpeg")
        {
            try
            {
                foreach (Process p in Process.GetProcessesByName(procName))
                {
                    p.Kill();
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 输出命令行处理
        /// </summary>
        /// <param name="sendProcess"></param>
        /// <param name="output"></param>
        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (!String.IsNullOrEmpty(output.Data))
            {
                //处理方法...
            }
        }
    }
}


