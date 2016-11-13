using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Diagnostics;

using AForge;
using AForge.Video;
using AForge.Vision;
using AForge.Video.VFW;
using AForge.Imaging.Filters;
using AForge.Vision.Motion;
using AForge.Imaging;

namespace alphaversiyon
{
    public partial class Form1 : Form
    {
    
        Stopwatch kronometre = new Stopwatch();
        Thread t;// yeni thread surekli olarak arduinodan veri kontrolü
        // opened video source
        private IVideoSource videoSource = null;
        // motion detector
       int kirmizi, sari, mavi = 0; 
        bool kayit_durumu = false;
        // statistics length
        private const int statLength = 15;
        private const int statLength2 = 15;
        // current statistics index
        private int statIndex = 0;
        private int statIndex2= 0;
        // ready statistics values
        private int statReady = 0;
        private int statReady2= 0;
        private int sayac2,sayac3 = 0;
        private bool hdkameradurumu=true;
        private bool noirkameradurumu = true;
        // statistics array
        private int[] statCount = new int[statLength];
        private int[] statCount2 = new int[statLength2];
        string varsayilan_ip = "192.168.1.7";
        Int32 varsayilan_port = 8080;
        Int32 varsayilan_port2 = 8081;
        bool goruntu_isleme = false;
        Process[] prg, prg2,prg3,prg4;
        private AVIWriter hd_writer = null;
        private AVIWriter noir_writer = null;
        // Constructor
        public Form1()
        {
            string[] argument = Environment.GetCommandLineArgs();
            if (argument.Length > 1)
            {
                varsayilan_ip = argument[1];
                varsayilan_port = Int32.Parse(argument[2]);
            }

            InitializeComponent();

           
              //  pictureBox1.Image = Image.FromFile(@"C:\Robot\resimler\onizleme.jpg");
            
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            textBox1.Text = varsayilan_ip;
            textBox2.Text = varsayilan_port.ToString();
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Visible = false; pictureBox3.Visible = false; pictureBox4.Visible = false; pictureBox5.Visible = false;

            this.Left = 0;
            this.Top = 0;
        }

        void kontrollerigizle()
        {
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button6.Visible = true;
        }

        // Application's main form is closing
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            hdkamerakapa();
            noirkamerakapa();


            prg = Process.GetProcessesByName("TankMotor");
            if (prg.Length > 0)
                prg[0].Kill();


            prg2 = Process.GetProcessesByName("ServoMotor");
            if (prg2.Length > 0)
                prg2[0].Kill();

            prg3 = Process.GetProcessesByName("TankMotor2");
            if (prg.Length > 0)
                prg[3].Kill();
          
            prg4 = Process.GetProcessesByName("Sensorler");
            if (prg4.Length > 0)
                prg4[0].Kill();
         
        }

        // "Exit" menu item clicked


        // "About" menu item clicked


        // "Open" menu item clieck - open AVI file



        // Open MJPEG URL
        private void openMJPEGURLToolStripMenuItem_Click(object sender, EventArgs e)
        {


            // create video source




        }

        // Open local video capture device


        // Open video file using DirectShow


        // Open video source
        private void hdcamera(IVideoSource link)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // close previous video source
            hdkamerakapa();

            // start new video source
            videoSourcePlayer.VideoSource = new AsyncVideoSource(link);
            videoSourcePlayer.Start();

            // reset statistics
            statIndex = statReady = 0;

            // start timers
            timer.Start();


            videoSource = link;

            this.Cursor = Cursors.Default;
        }

        private void noircamera(IVideoSource link)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // close previous video source
            noirkamerakapa();

            // start new video source
            videoSourcePlayer1.VideoSource = new AsyncVideoSource(link);
            videoSourcePlayer1.Start();

            // reset statistics
            statIndex = statReady = 0;

            // start timers
            timer1.Start();


            videoSource = link;
       
            this.Cursor = Cursors.Default;
        }


        // Close current video source
        private void hdkamerakapa()
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            videoSourcePlayer.SignalToStop();

            // wait 2 seconds until camera stops
            for (int i = 0; (i < 50) && (videoSourcePlayer.IsRunning); i++)
            {
                Thread.Sleep(100);
            }
            if (videoSourcePlayer.IsRunning)
                videoSourcePlayer.Stop();

            // stop timers
            timer.Stop();


            if (hd_writer != null)
            {
                hd_writer.Dispose();
                hd_writer = null;
            }


            videoSourcePlayer.BorderColor = Color.Black;
            this.Cursor = Cursors.Default;
        }

        private void noirkamerakapa()
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

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


            if (noir_writer != null)
            {
                noir_writer.Dispose();
                noir_writer = null;
            }


            videoSourcePlayer1.BorderColor = Color.Black;
            this.Cursor = Cursors.Default;
        }

        


            
        


        // New frame received by the player
        private void hdkamera_gelen_frame(object sender, ref Bitmap image)
        {
           
                if (kayit_durumu == true && hdkameradurumu == true)
                {
                    if (hd_writer == null)
                    {
                        // create file name
                        DateTime date = DateTime.Now;
                        String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}-{6}.avi",
                            date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);

                        try
                        {
                            // create AVI writer
                            hd_writer = new AVIWriter("wmv3");
                            // open AVI file

                            hd_writer.FrameRate = 25;


                            hd_writer.Open(fileName, 1280, 720);


                        }
                        catch (ApplicationException ex)
                        {
                            if (hd_writer != null)
                            {
                                hd_writer.Dispose();
                                hd_writer = null;
                            }
                        }

                    }
                    // hd_writer.AddFrame(videoSourcePlayer.GetCurrentVideoFrame());
                    hd_writer.AddFrame(image);
                    this.textBox3.Invoke(new Action(() =>
                    {
                        this.textBox3.Text = kronometre.Elapsed.ToString();
                    }));
                }




            }
        


        private void noirkamera_gelen_frame(object sender, ref Bitmap image)
        {
            if (kayit_durumu == true && noirkameradurumu==true)
            {
                if (noir_writer == null)
                {
                    // create file name
                    DateTime date = DateTime.Now;
                    String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}.avi",
                        date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

                    try
                    {
                        // create AVI writer
                        noir_writer = new AVIWriter("wmv3");
                        // open AVI file

                        noir_writer.FrameRate = 25;


                        noir_writer.Open(fileName, 640, 480);
                    }
                    catch (ApplicationException ex)
                    {
                        if (noir_writer != null)
                        {
                            noir_writer.Dispose();
                            noir_writer = null;
                        }
                    }

                }
                noir_writer.AddFrame(videoSourcePlayer1.GetCurrentVideoFrame());
                this.textBox3.Invoke(new Action(() =>
                {
                    this.textBox3.Text = kronometre.Elapsed.ToString();
                }));
            }
           
        }





        /*
    {
        Bitmap image1 = (Bitmap)image.Clone();
        ColorFiltering filter = new ColorFiltering();
        filter.Red = new IntRange(0, 75);
        filter.Green = new IntRange(0, 75);
        filter.Blue = new IntRange(50, 255);
        filter.ApplyInPlace(image1);
      

        BlobCounter blobCounter = new BlobCounter();
    blobCounter.MinWidth = 20;
    blobCounter.MinHeight = 20;
    blobCounter.FilterBlobs = true;
    blobCounter.ObjectsOrder = ObjectsOrder.Size;
    Grayscale griFiltre = new Grayscale(0.2125, 0.7154, 0.0721);
    Bitmap griImage = griFiltre.Apply(image1);
    blobCounter.ProcessImage(griImage);
    Rectangle[] rects = blobCounter.GetObjectsRectangles();
    foreach (Rectangle recs in rects)
        {
            if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];
                    //Graphics g = Graphics.FromImage(image);
                    Graphics g = videoSourcePlayer.CreateGraphics();
                    using (Pen pen = new Pen(Color.FromArgb(252, 3, 26), 2))
                        {
                            g.DrawRectangle(pen, objectRect);
                        }
                    //Cizdirilen Dikdörtgenin Koordinatlari aliniyor.
                    int objectX = objectRect.X + (objectRect.Width / 2);
                    int objectY = objectRect.Y + (objectRect.Height / 2);
                   string deneme1 = objectRect.Right.ToString();
                  //  g.DrawString(objectX.ToString() + "X" + objectY.ToString(), new Font("Arial", 12), Brushes.Red, new System.Drawing.Point(250, 1));
                    g.DrawString(objectRect.Size.ToString(), new Font("Arial", 12), Brushes.Red, new System.Drawing.Point(25, 2));
                        
                    g.Dispose();
                
                      
                }
        }
        

    }
/**/
        /*/   
      if (writer == null)
      {
          // create file name
          DateTime date = DateTime.Now;
          String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}.avi",
              date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

          try
          {
              // create AVI writer
              writer = new AVIWriter("wmv3");
              // open AVI file

              writer.FrameRate = 30;


              writer.Open(fileName, 1280, 720);
          }
          catch (ApplicationException ex)
          {
              if (writer != null)
              {
                  writer.Dispose();
                  writer = null;
              }
          }
               
      }

      if (checkBox1.Checked == true)
      {
       //   writer.AddFrame(image);
          writer.AddFrame(videoSourcePlayer.GetCurrentVideoFrame());
      }
             


  }

      */

        /*
            
        //*/


        // Update some UI elements

        // Draw motion history


        // On timer event - gather statistics
        private void timer_Tick(object sender, EventArgs e)
        {
            IVideoSource videoSource = videoSourcePlayer.VideoSource;

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

                fpslabel.Text = fps.ToString("F2") + " FPS";
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            string ip = textBox1.Text.ToString();
            string port = textBox2.Text.ToString(); ;
            string protokol = "http://";
            string slash = ":";
            string erisim = "/?action=stream";
            string baglanti_linki = string.Concat(protokol, ip, slash, port, erisim);

            MJPEGStream kameralinki = new MJPEGStream(baglanti_linki);

            hdcamera(kameralinki);

            string ip2 = textBox1.Text.ToString();
            string port2 = "8081";
            string protokol2 = "http://";
            string slash2 = ":";
            string erisim2 = "/?action=stream";
            string baglanti_linki2 = string.Concat(protokol2, ip2, slash2, port2, erisim2);
            
            MJPEGStream noircamlinki= new MJPEGStream(baglanti_linki2);

            noircamera(noircamlinki);
               
        }

        private void videoSourcePlayer_Click(object sender, EventArgs e)
        {
            hdkameradurumu = true;
            noirkameradurumu = false;
     
            videoSourcePlayer.Top = 0;
            videoSourcePlayer.Left = 0;
            videoSourcePlayer.Width = 1300;
            videoSourcePlayer.Height = 800;
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.Visible = false;
            videoSourcePlayer1.Top = 0;
            videoSourcePlayer1.Left = 0;
            videoSourcePlayer1.Width = 0;
            videoSourcePlayer1.Height = 0;
            timer1.Stop();
            fpslabel.Top = 9;
            fpslabel.Left = 1093;
            label1.Visible = false;
            fpslabel2.Visible = false;



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IVideoSource videoSource = videoSourcePlayer1.VideoSource;

            if (videoSource != null)
            {
                // get number of frames for the last second
                statCount2[statIndex2] = videoSource.FramesReceived;

                // increment indexes
                if (++statIndex2 >= statLength2)
                    statIndex2 = 0;
                if (statReady2 < statLength2)
                    statReady2++;

                float fps = 0;

                // calculate average value
                for (int i = 0; i < statReady2; i++)
                {
                    fps += statCount2[i];
                }
                fps /= statReady2;

                statCount2[statIndex2] = 0;

                fpslabel2.Text = fps.ToString("F2") + "FPS";
            }
        }

        private void videoSourcePlayer1_Click(object sender, EventArgs e)
        {
            noirkameradurumu = true;
            hdkameradurumu = false;
            videoSourcePlayer1.Top = 0;
            videoSourcePlayer1.Left = 0;
            videoSourcePlayer1.Width = 1300;
            videoSourcePlayer1.Height = 800;
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.Visible = false;
            videoSourcePlayer.Top = 0;
            videoSourcePlayer.Left = 0;
            videoSourcePlayer.Width = 0;
            videoSourcePlayer.Height = 0;
            fpslabel2.Top = 9;
            fpslabel2.Left = 1093;
            timer.Stop();
            label1.Visible = false;
            fpslabel.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            button7.Visible = true;
            if (hdkameradurumu == true)
            {
                button3.BackColor = Color.Green;
                Bitmap bitmap = (Bitmap)videoSourcePlayer.GetCurrentVideoFrame().Clone();

                DateTime date = DateTime.Now;
                String fileName = String.Format(@"C:\Robot\ekrangoruntuleri\goruntu-{0}-{1}-{2} {3}-{4}-{5}-{6}.jpg",
                date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second,date.Millisecond);
                bitmap.Save(fileName, ImageFormat.Jpeg);



                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = bitmap;

                pictureBox1.Visible = true;
           
            }
            else if (noirkameradurumu ==  true )
            {
                button3.BackColor = Color.Green;
                Bitmap bitmap = (Bitmap)videoSourcePlayer.GetCurrentVideoFrame().Clone();

                DateTime date = DateTime.Now;
                String fileName = String.Format(@"C:\Robot\ekrangoruntuleri\goruntu-{0}-{1}-{2} {3}-{4}-{5}.jpg",
                date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                bitmap.Save(fileName, ImageFormat.Jpeg);



                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = bitmap;

                pictureBox1.Visible = true;
            }
            else
            {
                button3.BackColor = Color.Green;
                Bitmap bitmap = (Bitmap)videoSourcePlayer.GetCurrentVideoFrame().Clone();
                Bitmap bitmap2 = (Bitmap)videoSourcePlayer1.GetCurrentVideoFrame().Clone();
                DateTime date = DateTime.Now;
                String fileName = String.Format(@"C:\Robot\ekrangoruntuleri\goruntu-{0}-{1}-{2} {3}-{4}-{5}.jpg",
                date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                bitmap.Save(fileName, ImageFormat.Jpeg);
           	
                DateTime date2 = DateTime.Now;
                String fileName2 = String.Format(@"C:\Robot\ekrangoruntuleri\goruntu-{0}-{1}-{2} {3}-{4}-{5}-{6}.jpg",
                date2.Year, date2.Month, date2.Day, date2.Hour, date2.Minute, date2.Second,date.Millisecond);
                bitmap2.Save(fileName2, ImageFormat.Jpeg);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = bitmap;

                pictureBox1.Visible = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", @"C:\Robot\ekrangoruntuleri");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            prg = Process.GetProcessesByName("TankMotor");
            if (prg.Length > 0)
                prg[0].Kill();


            prg2 = Process.GetProcessesByName("ServoMotor");
            if (prg2.Length > 0)
                prg2[0].Kill();


            prg3 = Process.GetProcessesByName("TankMotor2");
            if (prg3.Length > 0)
                prg3[0].Kill();
            prg4 = Process.GetProcessesByName("Sensorler");
            if (prg4.Length > 0)
                prg4[0].Kill();


            hdkamerakapa();
            noirkamerakapa();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            button7.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (hdkameradurumu == true)
            {
              

                noirkameradurumu = true;
                hdkameradurumu = false;
                videoSourcePlayer1.Top = 0;
                videoSourcePlayer1.Left = 0;
                videoSourcePlayer1.Width = 1300;
                videoSourcePlayer1.Height = 800;
                videoSourcePlayer1.Visible = true;
                videoSourcePlayer.SignalToStop();
                hdkamerakapa();
               
                videoSourcePlayer.Visible = false;
                videoSourcePlayer.Top = 0;
                videoSourcePlayer.Left = 0;
                videoSourcePlayer.Width = 0;
                videoSourcePlayer.Height = 0;

                string ip = textBox1.Text.ToString();
                string port = "8081";
                string protokol = "http://";
                string slash = ":";
                string erisim = "/?action=stream";
                string baglanti_linki = string.Concat(protokol, textBox1.Text, slash, port, erisim);

                MJPEGStream noircamlinki = new MJPEGStream(baglanti_linki);

                noircamera(noircamlinki);

                timer1.Start();
                fpslabel2.Top = 9;
                fpslabel2.Left = 1093;
                timer.Stop();
                fpslabel2.Visible = true;
            
                label1.Visible = false;
                fpslabel.Visible = false;

            }

            else if (noirkameradurumu == true)
            {

                hdkameradurumu = true;
                noirkameradurumu = false;
                videoSourcePlayer.Visible = true;
                videoSourcePlayer.Top = 0;
                videoSourcePlayer.Left = 0;
                videoSourcePlayer.Width = 1300;
                videoSourcePlayer.Height = 800;
                videoSourcePlayer1.Visible = false;
                videoSourcePlayer1.SignalToStop();
                noirkamerakapa();
                
                videoSourcePlayer1.Top = 0;
                videoSourcePlayer1.Left = 0;
                videoSourcePlayer1.Width = 0;
                videoSourcePlayer1.Height = 0;

                string ip = textBox1.Text.ToString();
                string port = textBox2.Text.ToString();
                string protokol = "http://";
                string slash = ":";
                string erisim = "/?action=stream";
                string baglanti_linki = string.Concat(protokol, textBox1.Text, slash, port, erisim);

                MJPEGStream hdcamlinki = new MJPEGStream(baglanti_linki);

                hdcamera(hdcamlinki);
                timer.Start();
                timer1.Stop();
                fpslabel.Top = 9;
                fpslabel.Left = 1093;
                label1.Visible = false;
                fpslabel2.Visible = false;
                fpslabel.Visible = true;
           
            }





        }

        private void button5_Click(object sender, EventArgs e)
        {
            kronometre.Start();
            kayit_durumu = true;
            button8.Enabled = true;
            button5.BackColor = Color.Green;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            kayit_durumu = false;
            button8.BackColor = Color.Green;
            if (noir_writer != null)
            {
               noir_writer.Close();
               noir_writer=null;
            }

            if (hd_writer != null)
            {
               hd_writer.Close();
                hd_writer = null;
            }
            button8.Enabled = false;
            kronometre.Reset();
            button5.BackColor = Color.Red;
            textBox3.Text = kronometre.Elapsed.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
                sayac3++;
                int sayac_degeri3 = sayac3 % 2;

                if (sayac_degeri3 == 0)
                {
                  Thread.Sleep(500);
            ProcessStartInfo motorkontrol = new ProcessStartInfo();
            motorkontrol.FileName = @"C:\Robot\TankMotor.exe";
            motorkontrol.Arguments = varsayilan_ip;
            Process.Start(motorkontrol);

            Thread.Sleep(500);
            ProcessStartInfo servokontrol = new ProcessStartInfo();
            servokontrol.FileName = @"C:\Robot\ServoMotor.exe";
            servokontrol.Arguments = varsayilan_ip;
            Process.Start(servokontrol);

            prg3 = Process.GetProcessesByName("TankMotor2");

            ProcessStartInfo goruntuisleme = new ProcessStartInfo();
            goruntuisleme.FileName = @"C:\Robot\goruntuisleme\tankKayitProgrami.exe";
            goruntuisleme.Arguments = varsayilan_ip;
            Process.Start(goruntuisleme);

            if (prg3.Length > 0)
                prg3[0].Kill();
               
 ProcessStartInfo sensor = new ProcessStartInfo();
            sensor.FileName = @"C:\Robot\Sensorler.exe";
            sensor.Arguments = varsayilan_ip;
            Process.Start(sensor);
                }
                else
                {
                  prg = Process.GetProcessesByName("TankMotor");
                  if (prg.Length > 0)
                  prg[0].Kill();


                  prg2 = Process.GetProcessesByName("ServoMotor");
                  if (prg2.Length > 0)
                  prg2[0].Kill();

                  prg3 = Process.GetProcessesByName("TankMotor2");
                  if (prg3.Length > 0)
                      prg3[0].Kill();
                   button9.Enabled = true;


                   prg4 = Process.GetProcessesByName("tankKayitProgrami");
                   if (prg4.Length > 0)
                       prg4[0].Kill();
                }
            }
           


          
        

        private void button9_Click(object sender, EventArgs e)
        {
            button9.BackColor = Color.Green;
            prg = Process.GetProcessesByName("TankMotor");
            if (prg.Length > 0)
                prg[0].Kill();

            ProcessStartInfo motorkontrol = new ProcessStartInfo();
            motorkontrol.FileName = @"C:\Robot\TankMotor2.exe";
            motorkontrol.Arguments = varsayilan_ip;
            Process.Start(motorkontrol);
            button9.Enabled = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ProcessStartInfo goruntuisleme= new ProcessStartInfo();
            goruntuisleme.FileName = @"C:\Robot\goruntuisleme\tankKayitProgrami.exe";
            goruntuisleme.Arguments = varsayilan_ip;
            Process.Start(goruntuisleme);
        }



    }
}