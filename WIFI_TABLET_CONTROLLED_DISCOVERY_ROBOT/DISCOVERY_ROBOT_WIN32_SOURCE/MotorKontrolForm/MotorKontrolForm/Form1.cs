using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
namespace MotorKontrolForm
{
    
    public partial class Form1 : Form
    {
        
        bool suruklenme,suruklenme2 = false; 
        Point ilkkonum,ilkkonum2;
        string varsayilan_ip = "127.0.0.1";
        string hiz_header = "hg";
        string hiz_header2 = "hl";
        string hiz_komut = "";
        Int32 varsayilan_port = 1234;
        int motor_hiz = 150;
        System.Net.Sockets.TcpClient raspberry = new System.Net.Sockets.TcpClient();
        public Form1()
        {
           
            InitializeComponent();
            this.TransparencyKey = Color.Turquoise;
            this.BackColor = Color.Turquoise;
            this.TopMost = true;
            this.Top = 500;
            this.Left = 0;
           
         
            gizle();
        }
        void gizle()
        {

            pictureBox1.Visible = false; pictureBox2.Visible = false;
            pictureBox3.Visible = false; pictureBox4.Visible = false;
            pictureBox5.Visible = false; pictureBox6.Visible = false;
            pictureBox7.Visible = false; pictureBox8.Visible = false;
            pictureBox9.Visible = false;
        }

     

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            raspberry_veri_gonder("mi");
           
            
            suruklenme = true; //işlemi burada true diyerek başlatıyoruz.
            button3.Cursor = Cursors.SizeAll; //SizeAll yapmamımızın amacı taşırken hoş görüntü vermek için
            ilkkonum = e.Location; //İlk konuma gördüğünüz gibi değerimizi atıyoruz.
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            raspberry_veri_gonder("ms");
            timer2.Stop();
            suruklenme = false; //Sol tuştan elimizi çektik artık yani sürükle işlemi bitti.
            button3.Cursor = Cursors.Default; //İmlecimiz(Cursor) default değerini alıyor.
            button3.Top = 65;
            gizle();
        }

        private void button3_MouseMove(object sender, MouseEventArgs e)
        {
            if (suruklenme) // suruklenmedurumu==true dememiz ile aynı işlemdir.
            {

                //  button1.Left = e.X + button1.Left - (ilkkonum.X); //
                button3.Top = e.Y + button3.Top - (ilkkonum.Y); // burada button.height/2 dediğimizde tam y eksenine ortalı şekilde butonu tutar.
           
                if ( button3.Top <=0) {
                     suruklenme = false;
                button3.Top = 0;
                }
                else if (button3.Top >0 && button3.Top <77)
                {
                    timer2.Start();
                    if (button3.Top > 60 && button3.Top < 77)
                    {
                        motor_hiz = 50;
                        hiz_komut = String.Concat(hiz_header2, motor_hiz);
                        raspberry_veri_gonder(hiz_komut);
                    }
                        pictureBox1.Visible = true;
                        if (button3.Top > 50 && button3.Top < 60)
                        {
                            motor_hiz = 90;
                            hiz_komut = String.Concat(hiz_header2, motor_hiz);
                            raspberry_veri_gonder(hiz_komut);
                            pictureBox2.Visible = true;
                        }
                        if (button3.Top > 40 && button3.Top < 50)
                        {
                            motor_hiz = 130;
                            hiz_komut = String.Concat(hiz_header, motor_hiz);
                            raspberry_veri_gonder(hiz_komut);
                            pictureBox3.Visible = true;
                        }
                        if (button3.Top > 30 && button3.Top < 40)
                        {
                            motor_hiz = 160;
                            hiz_komut = String.Concat(hiz_header, motor_hiz);
                            raspberry_veri_gonder(hiz_komut);
                            pictureBox4.Visible = true;
                        }

                        if (button3.Top > 20 && button3.Top < 30)
                        {
                            motor_hiz = 200;
                            hiz_komut = String.Concat(hiz_header, motor_hiz);
                            raspberry_veri_gonder(hiz_komut);
                            pictureBox5.Visible = true;
                        }

                        if (button3.Top > 10 && button3.Top < 20)
                        {
                            motor_hiz = 230;
                            hiz_komut = String.Concat(hiz_header, motor_hiz);
                            raspberry_veri_gonder(hiz_komut);
                            pictureBox6.Visible = true;
                        }
                        
                    if (button3.Top > 0 && button3.Top < 10)
                    {
                        motor_hiz = 250;
                        hiz_komut = String.Concat(hiz_header, motor_hiz);
                        raspberry_veri_gonder(hiz_komut);
                        pictureBox7.Visible = true;
                        pictureBox8.Visible = true;
                        pictureBox9.Visible = true;
                    }

                 
                }
                else if (button1.Top >= 77)
                {
                    timer3.Start(); 
                    suruklenme = false; 
                    button3.Top = 77;
                  //  button3.Cursor = Cursors.Default; //İmlecimiz(Cursor) default değerini alıyor.
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] argument = Environment.GetCommandLineArgs();
            if (argument.Length > 1)
            {
                varsayilan_ip = argument[1];


            }

            try
            {
                raspberry.Connect(varsayilan_ip, varsayilan_port);

            }
            catch (Exception a)
            {
                MessageBox.Show("Raspberry Bağlantısı Kurulamadı.");

            }
            timer1.Start();
        }
        void raspberry_veri_gonder(string veri)
        {
            try
            {
                NetworkStream serverStream = raspberry.GetStream();//server stream adında iletişim ağı kurduk ve arduino adındaki client ile ilişkilendirdik
                byte[] gonderi = System.Text.Encoding.ASCII.GetBytes(veri);//byte olarak bir gönderi oluşturduk ve encoding tipini ascii belirledik verilerimizi bu stream üzerinden yollayacağız
                serverStream.Write(gonderi, 0, gonderi.Length);//outStream ile göndereceğimiz veriyi tcp üzerinden arduinoya gönderiyoruz.
                serverStream.Flush();//veriyi gönderdikten sonra tcp hattımızı temizliyoruz.
            }
            catch
            {
                MessageBox.Show("Mesaj Gonderilirken/Alinirken Hata Olustu.kapatiliyor!");
                Environment.Exit(0);
            }
        }


        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            suruklenme2 = true; //işlemi burada true diyerek başlatıyoruz.
            button2.Cursor = Cursors.SizeAll; //SizeAll yapmamımızın amacı taşırken hoş görüntü vermek için
            ilkkonum2 = e.Location; //İlk konuma gördüğünüz gibi değerimizi atıyoruz.
         
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            timer4.Stop();
            raspberry_veri_gonder("ms");
            suruklenme2 = false; //Sol tuştan elimizi çektik artık yani sürükle işlemi bitti.
            button2.Cursor = Cursors.Default; //İmlecimiz(Cursor) default değerini alıyor.
            button2.Top = 195;
            gizle();
        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            if (suruklenme2) // suruklenmedurumu==true dememiz ile aynı işlemdir.
            {

                //  button1.Left = e.X + button1.Left - (ilkkonum.X); //
                button2.Top = e.Y + button2.Top - (ilkkonum.Y); // burada button.height/2 dediğimizde tam y eksenine ortalı şekilde butonu tutar.
           
                if (button2.Top <= 190)
                {
                    suruklenme2 = false;
                    button2.Top = 190;
                }
                else if (button2.Top > 200 && button2.Top <277)
                {
                    timer4.Start();
                    if (button2.Top > 205 && button2.Top < 210)
                    {
                        motor_hiz = 50;
                        hiz_komut = String.Concat(hiz_header2, motor_hiz);
                        raspberry_veri_gonder(hiz_komut);
                    }
                    if (button2.Top > 210 && button2.Top < 220)
                    {
                        motor_hiz = 90;
                        hiz_komut = String.Concat(hiz_header2, motor_hiz);
                        raspberry_veri_gonder(hiz_komut);
                    }
                    if (button2.Top > 220 && button2.Top < 230)
                    {
                        motor_hiz = 130;
                        hiz_komut = String.Concat(hiz_header, motor_hiz);
                        raspberry_veri_gonder(hiz_komut);
                    }
                    if (button2.Top > 230 && button2.Top < 240)
                    {
                        motor_hiz = 160;
                        hiz_komut = String.Concat(hiz_header, motor_hiz);
                        raspberry_veri_gonder(hiz_komut);
                    }

                    if (button2.Top > 250 && button2.Top < 260)
                    {
                        motor_hiz = 200;
                        hiz_komut = String.Concat(hiz_header, motor_hiz);
                        raspberry_veri_gonder(hiz_komut);
                    }
                    if (button2.Top > 260 && button2.Top < 270)
                    {
                        motor_hiz = 220;
                        hiz_komut = String.Concat(hiz_header, motor_hiz);
                        raspberry_veri_gonder(hiz_komut);
                    }
                    if (button2.Top > 270 && button2.Top < 277)
                    {

                        motor_hiz = 250;
                        hiz_komut = String.Concat(hiz_header, motor_hiz);
                        raspberry_veri_gonder(hiz_komut);
                        
                    }

                    
                }
                else if (button2.Top >= 277)
                {
                    timer5.Start();
                    suruklenme2 = false; 
                    button2.Top = 277;
                    //  button3.Cursor = Cursors.Default; //İmlecimiz(Cursor) default değerini alıyor.
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            raspberry_veri_gonder("pk");

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            raspberry_veri_gonder("mi");
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
          //  suruklenme = true;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            raspberry_veri_gonder("mg");
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
           //suruklenme2 = true;
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            raspberry_veri_gonder("ml");
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            raspberry_veri_gonder("ms");
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            raspberry_veri_gonder("mr");
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            raspberry_veri_gonder("ms");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            raspberry_veri_gonder("ms");
            timer4.Stop();
            timer2.Stop();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int x=250;
            raspberry_veri_gonder(x.ToString());
        }



    }
}
