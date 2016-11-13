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
using System.Net.Sockets;
namespace Pingkontrol
{
    public partial class Form1 : Form
    {
        Thread t;// yeni thread surekli olarak arduinodan veri kontrolü
        System.Net.Sockets.TcpClient arduinom = new System.Net.Sockets.TcpClient(); //arduinom adında yeni bir tcp client tanımladık
        String gelen_veri;
        String ping_mesaji = "pk";
        bool baglanti_durumu = false;
        string varsayilan_ip = "192.168.1.10";
        Int32 varsayilan_port = 1237;
        public Form1()
        {

            InitializeComponent();
            this.TransparencyKey = Color.Turquoise;
           this.BackColor = Color.Turquoise;
           this.Top = 200;
           this.Left = 0;
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@"c:/Robot/resimler/baglantiyok.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            string[] argument = Environment.GetCommandLineArgs();
            if (argument.Length > 1)
            {
                varsayilan_ip = argument[1];


            }

            try
            {
                arduinom.Connect(varsayilan_ip, varsayilan_port);// 23 portu üzerinde arduino ip si ile baglanmayı deniyoruz
                MessageBox.Show("Robot ile Baglantı Saglandı!"); // baglantı saglanırsa ekrana baglantı saglandı bilgisi basılıyor
                pictureBox1.Image = Image.FromFile(@"c:/Robot/resimler/ping.gif");
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                baglanti_durumu = true;
                t = new Thread(new ThreadStart(raspberry_oku));// threadi arduino oku fonksiyonuna baglıyoruz
                t.Start();// baglantı saglandıktan sonra thread basladı
                timer2.Start();
            }
            catch (Exception a)// baglantı kurulamadı ise
            {

                MessageBox.Show("baglantı yok tekrar baglanmayı deneyiniz");//baglantı yok tekrar baglan uyarısı ekrana bastırılıyor

                pictureBox1.Image = Image.FromFile(@"c:/Robot/resimler/baglantiyok.jpg");
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                baglanti_durumu = false;
            }

    

        }

     

        void raspberry_veri_gonder(string veri)
        {
            try
            {
                NetworkStream serverStream = arduinom.GetStream();//server stream adında iletişim ağı kurduk ve arduino adındaki client ile ilişkilendirdik
                byte[] gonderi = System.Text.Encoding.ASCII.GetBytes(veri);//byte olarak bir gönderi oluşturduk ve encoding tipini ascii belirledik verilerimizi bu stream üzerinden yollayacağız
                serverStream.Write(gonderi, 0, gonderi.Length);//outStream ile göndereceğimiz veriyi tcp üzerinden arduinoya gönderiyoruz.
                serverStream.Flush();//veriyi gönderdikten sonra tcp hattımızı temizliyoruz.
            }

            catch (Exception a)// baglantı kurulamadı ise
            {

                pictureBox1.Image = Image.FromFile(@"c:/Robot/resimler/baglantikoptu.jpg");
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                baglanti_durumu = false;
            }

        }



        void raspberry_oku()
        {
            NetworkStream st = arduinom.GetStream();
            byte[] myReadBuffer = new byte[10];
            while (true)
            {
                if (st.DataAvailable)
                {
                    st.BeginRead(myReadBuffer, 0, myReadBuffer.Length,
                                                     null,
                                                     null);
                }
                 gelen_veri = System.Text.Encoding.UTF8.GetString(myReadBuffer);
                 timer3.Start();
                 Thread.Sleep(100);
            }
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
       
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            raspberry_veri_gonder(ping_mesaji);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@"c:/Robot/resimler/baglantikoptu.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            baglanti_durumu = false;
            arduinom.Client.Disconnect(true);
            timer3.Stop();
            timer2.Stop();
            t.Abort();
           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            t.Abort();
            Environment.Exit(0);
            
        }

    }
}
