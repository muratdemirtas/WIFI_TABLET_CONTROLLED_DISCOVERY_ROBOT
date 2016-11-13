using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using AForge.Video;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
namespace RobotArayuz
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
      
        private const int statLength = 15;
        // current statistics index
        private int statIndex = 0;
        // ready statistics values
        private int statReady = 0;
        // statistics array
        private int sayac = 0;
        private int sayac2,sayac3 = 0;
        String gelen_veri;
        private int[] statCount = new int[statLength];
        private IVideoSource videoSource = null;
        Process[] prg, prg2, prg3, prg4, prg5,prg6;
        string varsayilan_ip="192.168.1.10";
        Int32 varsayilan_port=8080;
        Int32 varsayilan_port2 = 8081;
        public Form1()
        {
            string[] argument = Environment.GetCommandLineArgs();
            if (argument.Length > 1)
            {
                varsayilan_ip = argument[1];
                varsayilan_port = Int32.Parse(argument[2]);
            }

            InitializeComponent();
            // this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            IVideoSource videoSource = videoSourcePlayer1.VideoSource;

            pictureBox1.Image = Image.FromFile(@"C:\Robot\resimler\onizleme.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            textBox1.Text = varsayilan_ip;
            textBox2.Text = varsayilan_port.ToString();

            pictureBox2.Visible = false; pictureBox3.Visible = false; pictureBox4.Visible = false; pictureBox5.Visible = false;
         
        }

      
        private void OpenVideoSource(IVideoSource source)
        {


            CloseVideoSource();
            videoSourcePlayer1.VideoSource = new AsyncVideoSource(source);
            videoSourcePlayer1.Start();



          
            statIndex = statReady = 0;

            timer1.Start();


            videoSource = source;
         
        

        }






        // Close video source if it is running
        private void CloseVideoSource()
        {


            // stop current video source
            videoSourcePlayer1.SignalToStop();

            // wait 2 seconds until camera stops
            for (int i = 0; (i < 50) && (videoSourcePlayer1.IsRunning); i++)
            {
                Thread.Sleep(100);
            }
            if (videoSourcePlayer1.IsRunning)
                videoSourcePlayer1.Stop();

            // stop timers
            timer1.Stop();


        }

        private void Form1_Load(object sender, EventArgs e)

        {
            videoSourcePlayer1.SignalToStop();
            CloseVideoSource();
            string ip = textBox1.ToString();
            string port = "8080";
            string protokol = "http://";
            string slash = ":";
            string erisim = "/?action=stream";
            string baglanti_linki = string.Concat(protokol, textBox1.Text, slash, port, erisim);

            MJPEGStream mjpegSource = new MJPEGStream(baglanti_linki);

            OpenVideoSource(mjpegSource);
    
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {


            if (videoSource != null)
            {
                // get number of frames for the last second
                statCount[statIndex] = videoSource.FramesReceived;

                // increment indexes
                if (++statIndex >= statLength)
                    statIndex = 0;
                if (statReady < statLength)
                    statReady++;

                float fps = 0;

                // calculate average value
                for (int i = 0; i < statReady; i++)
                {
                    fps += statCount[i];
                }
                fps /= statReady;

                statCount[statIndex] = 0;
                if (fps > 15)
                {

                    label5.Text =  fps.ToString("F2");
                    label5.BackColor = Color.Green;

                    label4.BackColor = Color.Green;
                }
                else if ((fps > 10) && (fps < 15))
                {

                    label5.Text =  fps.ToString("F2");
                    label5.BackColor = Color.Blue;
                    label4.BackColor = Color.Blue;

                }
                else if (fps < 10)
                {

                    label5.Text =  fps.ToString("F2");
                    label5.BackColor = Color.Red;
                    label4.BackColor = Color.Red;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            

            prg = Process.GetProcessesByName("TankMotor");
            if (prg.Length > 0)
                prg[0].Kill();


            prg2 = Process.GetProcessesByName("ServoMotor");
            if (prg2.Length > 0)
                prg2[0].Kill();

            prg3 = Process.GetProcessesByName("Sensorler");
            if (prg3.Length > 0)
                prg3[0].Kill();
         

            prg5 = Process.GetProcessesByName("ScreenRecord");
            if (prg5.Length > 0)
                prg5[0].Kill();

            prg6 = Process.GetProcessesByName("Pingkontrol");
            if (prg6.Length > 0)
                prg6[0].Kill();

            CloseVideoSource();
            this.Close();

        }

        private void button6_Click(object sender, EventArgs e)
        {
          
            sayac = sayac + 1;
            int sayac_degeri= sayac % 2;
            if (sayac_degeri == 1)
            {
                prg = Process.GetProcessesByName("TankMotor");
                if (prg.Length > 0)
                    prg[0].Kill();


                prg2 = Process.GetProcessesByName("ServoMotor");
                if (prg2.Length > 0)
                    prg2[0].Kill();

                prg3 = Process.GetProcessesByName("Sensorler");
                if (prg3.Length > 0)
                    prg3[0].Kill();
             
                prg5 = Process.GetProcessesByName("ScreenRecord");
                if (prg5.Length > 0)
                    prg5[0].Kill();
                prg6 = Process.GetProcessesByName("Pingkontrol");
                if (prg6.Length > 0)
                    prg6[0].Kill();
                
            }
            else if (sayac_degeri == 0)
            {
       
             
                System.Diagnostics.Process.Start(@"C:\Robot\EkranKayit\ScreenRecord.exe");
                
                ProcessStartInfo motorkontrol = new ProcessStartInfo();
                motorkontrol.FileName = @"C:\Robot\TankMotor.exe";
                motorkontrol.Arguments = varsayilan_ip;
                Process.Start(motorkontrol);
                Thread.Sleep(500);
                ProcessStartInfo servokontrol = new ProcessStartInfo();
                servokontrol.FileName = @"C:\Robot\ServoMotor.exe";
                servokontrol.Arguments = varsayilan_ip;
                Process.Start(servokontrol);

                ProcessStartInfo pingkontrol = new ProcessStartInfo();
                pingkontrol.FileName = @"C:\Robot\Pingkontrol.exe";
                pingkontrol.Arguments = varsayilan_ip;
                Process.Start(pingkontrol);
                Thread.Sleep(500);
                ProcessStartInfo sensorler = new ProcessStartInfo();
                sensorler.FileName = @"C:\Robot\Sensorler.exe";
                sensorler.Arguments = varsayilan_ip;
                Process.Start(sensorler);




            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.BackColor = Color.Green;
            Bitmap bitmap = (Bitmap)videoSourcePlayer1.GetCurrentVideoFrame().Clone(); 
           
            DateTime date = DateTime.Now;
            String fileName = String.Format(@"C:\Robot\ekrangoruntuleri\goruntu-{0}-{1}-{2} {3}-{4}-{5}.jpg",
                date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            bitmap.Save(fileName, ImageFormat.Jpeg);
            

           
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = bitmap;
           
            pictureBox1.Visible = true;
            button7.Visible = true;
           
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            button7.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", @"C:\Robot\ekrangoruntuleri");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string ip = textBox1.ToString();
            string port = textBox2.ToString();
            string protokol = "http://";
            string slash = ":";
            string erisim="/?action=stream";
            string baglanti_linki = string.Concat(protokol,textBox1.Text,slash, textBox2.Text,erisim);
         
            MJPEGStream mjpegSource = new MJPEGStream(baglanti_linki);

            OpenVideoSource(mjpegSource);

        }
       

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label8.Text = DateTime.Now.ToString();
            button3.BackColor = Color.Red;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sayac3++;
                int sayac_degeri3 = sayac3 % 2;

                if (sayac_degeri3 == 1)
                {
                    label1.Text = "NOIR CAM";
                    videoSourcePlayer1.SignalToStop();
                    CloseVideoSource();
                    string ip = textBox1.ToString();
                    string port = "8081";
                    string protokol = "http://";
                    string slash = ":";
                    string erisim = "/?action=stream";
                    string baglanti_linki = string.Concat(protokol, textBox1.Text, slash, port, erisim);

                    MJPEGStream mjpegSource = new MJPEGStream(baglanti_linki);

                    OpenVideoSource(mjpegSource);
            }
                else
                {
                    videoSourcePlayer1.SignalToStop();
                    CloseVideoSource();
                    string ip = textBox1.ToString();
                    string port = "8080";
                    string protokol = "http://";
                    string slash = ":";
                    string erisim = "/?action=stream";
                    string baglanti_linki = string.Concat(protokol, textBox1.Text, slash, port, erisim);

                    MJPEGStream mjpegSource = new MJPEGStream(baglanti_linki);

                    OpenVideoSource(mjpegSource);
                }
            

            
                  
 
       }

        private void button9_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            
            sayac2++;
            int sayac_degeri2 = sayac2 % 2;

            if (sayac_degeri2 == 1)
            {
                button4.BackColor = Color.Green;
                pictureBox2.Visible = true; pictureBox3.Visible = true; pictureBox4.Visible = true; pictureBox5.Visible = true;
            }
            else 
            {
                button4.BackColor = Color.Red;
                pictureBox2.Visible = false; pictureBox3.Visible = false; pictureBox4.Visible = false; pictureBox5.Visible = false;
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSourcePlayer1.SignalToStop();
            CloseVideoSource();
            prg = Process.GetProcessesByName("TankMotor");
            if (prg.Length > 0)
                prg[0].Kill();


            prg2 = Process.GetProcessesByName("ServoMotor");
            if (prg2.Length > 0)
                prg2[0].Kill();

            prg3 = Process.GetProcessesByName("Sensorler");
            if (prg3.Length > 0)
                prg3[0].Kill();

            prg5 = Process.GetProcessesByName("ScreenRecord");
            if (prg5.Length > 0)
                prg5[0].Kill();
            prg6 = Process.GetProcessesByName("Pingkontrol");
            if (prg6.Length > 0)
                prg6[0].Kill();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        }


  
    }
