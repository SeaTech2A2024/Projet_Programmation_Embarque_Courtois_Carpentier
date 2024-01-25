using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExtendedSerialPort_NS;
using System.IO.Ports;
using System.Windows.Threading;
using System.Windows.Threading;


namespace Courtois_Carpentier_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ExtendedSerialPort serialPort1;
        DispatcherTimer timerAffichage;
        string receivedText;

        public MainWindow()
        {
            InitializeComponent();
            serialPort1 = new ExtendedSerialPort("COM21", 115200, Parity.None, 8, StopBits.One);
            serialPort1.DataReceived += SerialPort1_DataReceived;
            serialPort1.Open();

            timerAffichage = new DispatcherTimer();
            timerAffichage.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timerAffichage.Tick += TimerAffichage_Tick;
            timerAffichage.Start();

            
        }

        private void TimerAffichage_Tick(object? sender, EventArgs e)
        {
            textBoxReception.Text += receivedText;
            receivedText = "";
        }

        public void SerialPort1_DataReceived(object sender, DataReceivedArgs e)
        {
            receivedText += Encoding.UTF8.GetString(e.Data, 0, e.Data.Length);
            
        }

        private void buttonEnvoyer_Click(object sender, RoutedEventArgs e)
        {
            textBoxReception.Text += "Reçu : " + textBoxEmission.Text + "\n"; 
            serialPort1.WriteLine(textBoxEmission.Text);

            textBoxEmission.Text = "";

        }

        private void textBoxEmission_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                buttonEnvoyer_Click((object) sender, e);
            }
        }
    }
}
