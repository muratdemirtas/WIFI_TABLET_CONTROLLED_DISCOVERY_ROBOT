using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.FFMPEG;
using AForge.Video;
using System.Diagnostics;

namespace ScreenRecord
{
    public partial class Form1 : Form
    {
        private bool kayittami;
        private bool duraklatmi;
        private VideoFileWriter yazici;
        private int ekran_yuksekligi = 0;
        private int ekran_genisligi = 0;
        private int bithizi = 5000000;
        private int fps_degeri = 25;
        private ScreenCaptureStream ekrankaydedici;
        private Stopwatch kronometre;
        private Rectangle ekranboyutu;
        private string kayityeri = "C:/Robot/kayit";
        public Form1()
        {
            InitializeComponent();
            kayittami = false;
            duraklatmi = false;
            ekran_genisligi = SystemInformation.VirtualScreen.Width;
            ekran_yuksekligi = SystemInformation.VirtualScreen.Height;
            kronometre = new Stopwatch();
            ekranboyutu = Rectangle.Empty;
            yazici = new VideoFileWriter();
            this.TransparencyKey = Color.Turquoise;
            this.BackColor = Color.Turquoise;
            this.Top = 130;
            this.Left = 0;
        }

        private void bt_start_Click( object sender, EventArgs e )
        {
            try {   
                        
            StartRec( kayityeri );                              
            }
            catch ( Exception exc )
            {
                MessageBox.Show( exc.Message );
            }
        }

        private void StartRec( string kayityolu )
        {
            if ( kayittami== false )
            {
                foreach (Screen screen in Screen.AllScreens)
                {
                    ekranboyutu= Rectangle.Union(ekranboyutu, screen.Bounds);
                }
                this.SetVisible( true );
                
                string fullName = string.Format( @"{0}\{1}_{2}.avi", kayityolu, Environment.UserName.ToUpper(), DateTime.Now.ToString( "d_MMM_yyyy_HH_mm_ssff" ) ); 
                yazici.Open(fullName,ekran_genisligi,ekran_yuksekligi,( int ) fps_degeri,( VideoCodec ) VideoCodec.MPEG4,
                ( int ) bithizi);
                StartRecord();
            }
        }

        private void StartRecord() //Object stateInfo
        {
           ekrankaydedici = new ScreenCaptureStream(ekranboyutu);
           ekrankaydedici.NewFrame += new NewFrameEventHandler(this.video_NewFrame);
           ekrankaydedici.Start();
           kronometre.Start();
        }

        private void video_NewFrame( object sender, NewFrameEventArgs eventArgs )
        {
            if ( kayittami )
            {
               yazici.WriteVideoFrame( eventArgs.Frame );
                this.lb_stopWatch.Invoke( new Action( () =>
                {
                    this.lb_stopWatch.Text = kronometre.Elapsed.ToString();
                } ) );
            }
            else if (duraklatmi==true)
            {
  
                kronometre.Stop();
              
            }

            else
            {
                kronometre.Reset();
                Thread.Sleep( 500 );
                ekrankaydedici.SignalToStop();
                Thread.Sleep( 500 );
                yazici.Close();
            }
        }

        private void bt_Save_Click( object sender, EventArgs e )
        {
            SetVisible( false );
            MessageBox.Show( @"Dosya kaydedildi!." );
        }

        private void SetVisible( bool visible )
        { 
            kayittami = visible;
        }

        private void Form1_FormClosing( object sender, FormClosingEventArgs e )
        {
            kayittami = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                DateTime date = DateTime.Now;
                String fileName = String.Format(@"C:\Robot\ekrangoruntuleri\goruntu-{0}-{1}-{2} {3}-{4}-{5}.jpg",
                    date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                bitmap.Save(fileName, ImageFormat.Jpeg);
                
            }
        }
    }
}
