using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
namespace Giris_Ekrani
{
    public partial class Form1 : Form
     
    {
        string varsayilan_ip = "192.168.1.11";
        int varsayilan_port = 8080;
        public Form1()
        {
            InitializeComponent();
           
        }

     

        private void button1_Click(object sender, EventArgs e)
        {
         
            if (textBox1.TextLength > 1 && textBox2.TextLength > 1)
            {
         
                varsayilan_ip = textBox1.Text;
         
                varsayilan_port = Int32.Parse(textBox2.Text);
             
            }
            ProcessStartInfo arayuzubaslat = new ProcessStartInfo();
            arayuzubaslat.FileName = @"C:\Robot\AlphaEkran\alphaversiyon.exe";
            string baglanti = string.Concat(varsayilan_ip, " ", varsayilan_port);
            arayuzubaslat.Arguments = baglanti;
            Process.Start(arayuzubaslat);
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

            ProcessStartInfo sensor = new ProcessStartInfo();
            sensor.FileName = @"C:\Robot\Sensorler.exe";
            sensor.Arguments = varsayilan_ip;
            Process.Start(sensor);

            this.Close();
        
        }
    }

}