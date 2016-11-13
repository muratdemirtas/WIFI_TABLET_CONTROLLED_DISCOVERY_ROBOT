﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Net.Sockets.TcpClient raspberry = new System.Net.Sockets.TcpClient();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        string varsayilan_ip = "192.168.173.124";
        Int32 varsayilan_port = 1234;
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Start();
   

            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"C:\Robot\resimler\yukari.jpg", UriKind.RelativeOrAbsolute);
            bitimg.EndInit();

            BitmapImage bitimg2 = new BitmapImage();
            bitimg2.BeginInit();
            bitimg2.UriSource = new Uri(@"C:\Robot\resimler\asagi.png", UriKind.RelativeOrAbsolute);
            bitimg2.EndInit();

            BitmapImage bitimg3 = new BitmapImage();
            bitimg3.BeginInit();
            bitimg3.UriSource = new Uri(@"C:\Robot\resimler\sol.png", UriKind.RelativeOrAbsolute);
            bitimg3.EndInit();

            BitmapImage bitimg4 = new BitmapImage();
            bitimg4.BeginInit();
            bitimg4.UriSource = new Uri(@"C:\Robot\resimler\sag.png", UriKind.RelativeOrAbsolute);
            bitimg4.EndInit();

            BitmapImage bitimg5 = new BitmapImage();
            bitimg5.BeginInit();
            bitimg5.UriSource = new Uri(@"C:\Robot\resimler\dur.png", UriKind.RelativeOrAbsolute);
            bitimg5.EndInit();

            Image img = new Image();
            img.Stretch = Stretch.Fill;
            img.Source = bitimg;
            Image img2 = new Image();
            img2.Stretch = Stretch.Fill;
            img2.Source = bitimg2;
            Image img3 = new Image();
            img3.Stretch = Stretch.Fill;
            img3.Source = bitimg3;
            Image img4 = new Image();
            img4.Stretch = Stretch.Fill;
            img4.Source = bitimg4;
            Image img5 = new Image();
            img5.Stretch = Stretch.Fill;
            img5.Source = bitimg5;


            Button.Content = img;
            // Set Button.Content
            Button2.Content = img2;
            Button3.Content = img3;
            Button4.Content = img4;
            Button5.Content = img5;

            // Set Button.Background
            Button.Background = new ImageBrush(bitimg);
            Button2.Background = new ImageBrush(bitimg2);
            Button3.Background = new ImageBrush(bitimg3);
            Button4.Background = new ImageBrush(bitimg4);
            Button5.Background = new ImageBrush(bitimg5);

     

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
                MessageBox.Show("Mesaj Gonderilirken/Alinirken Hata Olustu");
            }
        }



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            raspberry_veri_gonder("pk");
        }
        private void dispatcherTimer2_Tick(object sender, EventArgs e)
        {
            raspberry_veri_gonder("mi");
        } 
       

        private void Button_TouchDown(object sender, TouchEventArgs e)
        {
            raspberry_veri_gonder("mi");
        }

        private void Button_TouchUp(object sender, TouchEventArgs e)
        {
            raspberry_veri_gonder("ms");
        }

        private void Button3_TouchDown(object sender, TouchEventArgs e)
        {
            raspberry_veri_gonder("ml");
        }

        private void Button3_TouchUp(object sender, TouchEventArgs e)
        {
            raspberry_veri_gonder("ms");
        }

        private void Button2_TouchDown(object sender, TouchEventArgs e)
        {
            raspberry_veri_gonder("mg");
        }

        private void Button2_TouchUp(object sender, TouchEventArgs e)
        {
            raspberry_veri_gonder("ms");
        }

        private void Button4_TouchDown(object sender, TouchEventArgs e)
        {
            raspberry_veri_gonder("mr");
        }

        private void Button5_TouchDown(object sender, TouchEventArgs e)
        {
            raspberry_veri_gonder("ms");
        }

        private void Button6_TouchDown(object sender, TouchEventArgs e)
        {
            this.Close();
        }

        private void Button4_TouchUp(object sender, TouchEventArgs e)
        {
            raspberry_veri_gonder("ms");
        }


    }
}
