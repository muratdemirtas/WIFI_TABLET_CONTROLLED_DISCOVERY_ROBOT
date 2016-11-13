using System;
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
using System.IO;

 using System.Globalization;
using System.Net.Sockets;
using System.Threading;//Eklediğimiz .net için socket kütüphanemiz
namespace Sensorler
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        string str, str2; string aydinlik,gaz_degeri, batarya;  float batarya_voltaj,sicaklik_degeri;
        string varsayilan_ip = "192.168.1.7";
        
        private string sicaklik = "";
        private int on_mesafe = 0;
        private int arka_mesafe = 0;
        private int sayac, sayac2 = 0; Int32 x;
        Int32 varsayilan_port = 1236;
        Thread t;// yeni thread surekli olarak arduinodan veri kontrolü
        System.Net.Sockets.TcpClient arduino = new System.Net.Sockets.TcpClient(); //arduinom adında yeni bir tcp client tanımladık
        String gelen_veri;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();


            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(300); ;
            dispatcherTimer.Start();

            string[] argument = Environment.GetCommandLineArgs();
            if (argument.Length > 1)
            {
                varsayilan_ip = argument[1];


            }

            try
            {
                arduino.Connect(varsayilan_ip, varsayilan_port);

            }
            catch (Exception a)
            {
                MessageBox.Show("Raspberry Bağlantısı Kurulamadı.");

            }
            t = new Thread(new ThreadStart(arduino_oku));// threadi arduino oku fonksiyonuna baglıyoruz
            t.Start();// baglantı saglandıktan sonra thread basladı

        }

        void arduino_veri_gonder(string veri)
        {
            try
            {
                NetworkStream serverStream = arduino.GetStream();//server stream adında iletişim ağı kurduk ve arduino adındaki client ile ilişkilendirdik
                byte[] gonderi = System.Text.Encoding.ASCII.GetBytes(veri);//byte olarak bir gönderi oluşturduk ve encoding tipini ascii belirledik verilerimizi bu stream üzerinden yollayacağız
                serverStream.Write(gonderi, 0, gonderi.Length);//outStream ile göndereceğimiz veriyi tcp üzerinden arduinoya gönderiyoruz.
                serverStream.Flush();//veriyi gönderdikten sonra tcp hattımızı temizliyoruz.
            }
            catch
            {
                MessageBox.Show("Mesaj Gonderilirken/Alinirken Hata Olustu");
            }
        }

        void arduino_oku()
        {

            NetworkStream st = arduino.GetStream();
            while (true)
            {

                if (st.DataAvailable)
                {

                    byte[] data = new byte[100];

                    using (MemoryStream ms = new MemoryStream())
                    {

                        int numBytesRead;
                        while ((numBytesRead = st.Read(data, 0, data.Length)) > 0)
                        {
                            ms.Write(data, 0, numBytesRead);
                            break;

                        }

                        str = Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);

                        string[] degerler = str.Split('-');

                        try
                        {
                        

                        sicaklik_label.Dispatcher.BeginInvoke((Action)(() => sicaklik_label.Content = degerler[5].ToString()));
                        bataryalabel.Dispatcher.BeginInvoke((Action)(() => bataryalabel.Content = degerler[4].ToString()));
                        label1.Dispatcher.BeginInvoke((Action)(() => label1.Content = degerler[2].ToString()));
                        label2.Dispatcher.BeginInvoke((Action)(() => label2.Content = degerler[3].ToString()));
                        gaz_label1.Dispatcher.BeginInvoke((Action)(() => gaz_label1.Content = degerler[1].ToString()));

                        
                            on_mesafe = int.Parse(degerler[2]);
                            arka_mesafe = int.Parse(degerler[3]);
                            if (on_mesafe > 100)
                            {
                                label1.Dispatcher.BeginInvoke((Action)(() => label1.Content = "+100"));
                            }

                            if (arka_mesafe > 100)
                            {
                                label2.Dispatcher.BeginInvoke((Action)(() => label2.Content = "+100"));
                            }
                        }

                        catch
                        {

                        }

                        try
                        {
                            batarya_voltaj = System.Convert.ToSingle(degerler[4]);
                            if (batarya_voltaj > 900.0 && batarya_voltaj < 1200.0)

                                progress.Dispatcher.BeginInvoke((Action)(() => gaz_label1.Content = progress.Value = 100));


                            if (batarya_voltaj > 700.0 && batarya_voltaj < 900.0)
                                progress.Dispatcher.BeginInvoke((Action)(() => gaz_label1.Content = progress.Value = 80));
                            if (batarya_voltaj > 500.0 && batarya_voltaj < 700.0)
                                progress.Dispatcher.BeginInvoke((Action)(() => gaz_label1.Content = progress.Value = 60));
                            if (batarya_voltaj > 300.0 && batarya_voltaj < 500.0)
                                progress.Dispatcher.BeginInvoke((Action)(() => gaz_label1.Content = progress.Value = 30));
                        }
                        catch
                        {

                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
            
           

                    


                
         
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {


            arduino_veri_gonder("hello");
            Thread.Sleep(3000);
             
        }

     
        private void far_TouchDown(object sender, TouchEventArgs e)
        {
            sayac++;
            int sayac_degeri3 = sayac % 2;

            if (sayac_degeri3 == 1)
                arduino_veri_gonder("BAh");
            else
                arduino_veri_gonder("BAo");
        }

        private void buzzer_TouchDown(object sender, TouchEventArgs e)
        {
            sayac2++;
            int sayac_degeri3 = sayac2 % 2;

            if (sayac_degeri3 == 1)
                arduino_veri_gonder("BAb");
            else
                arduino_veri_gonder("BAk");
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            sayac++;
            int sayac_degeri3 = sayac % 2;

            if (sayac_degeri3 == 1)
                arduino_veri_gonder("BAh");
            else
                arduino_veri_gonder("BAo");
        }
    }
}
        


    

