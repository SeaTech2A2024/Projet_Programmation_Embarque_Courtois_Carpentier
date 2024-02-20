﻿using System;
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
using System.Diagnostics;

namespace Courtois_Carpentier_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ExtendedSerialPort serialPort1;
        DispatcherTimer timerAffichage;
        Robot robot = new Robot();

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
            while (robot.byteListReceived.Count > 0)
            {
                byte byteValue = robot.byteListReceived.Dequeue();

                DecodeMessage(byteValue);
                //textBoxReception.Text += "0x" + byteValue.ToString("X2") + " ";

            }
        }


        public void SerialPort1_DataReceived(object sender, DataReceivedArgs e)
        {
            for (int i = 0; i < e.Data.Length; i++)
            {
                robot.byteListReceived.Enqueue(e.Data[i]);
            }

            //robot.receivedText += Encoding.UTF8.GetString(e.Data, 0, e.Data.Length);

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
                buttonEnvoyer_Click((object)sender, e);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxReception.Text = "";
        }

        public enum Supervision : int
        {
            TXT = 0x0080,
            LED = 0x0020,
            IR = 0x0030,
            CV = 0x0040
        }
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            foreach (Supervision command in Enum.GetValues(typeof(Supervision)))
            {

                switch (command)
                {
                    case Supervision.TXT:
                        int commandValue = (int)command;
                        byte[] payload = Encoding.ASCII.GetBytes("Y a pas de snickers dans la machine");
                        UartEncodeAndSendMessage(commandValue, payload.Length, payload);
                        break;

                    case Supervision.LED:
                        commandValue = (int)command;
                        payload = new byte[3];
                        payload[0] = 10;
                        payload[1] = 21;
                        payload[2] = 31;

                        UartEncodeAndSendMessage(commandValue, payload.Length, payload);

                        break;

                    case Supervision.IR:
                        commandValue = (int)command;
                        payload = new byte[3];
                        payload[0] = 10;
                        payload[1] = 20;
                        payload[2] = 30;
                        UartEncodeAndSendMessage(commandValue, payload.Length, payload);
                        break;

                    case Supervision.CV:
                        commandValue = (int)command;
                        payload = new byte[2];
                        payload[0] = 80;
                        payload[1] = 20;
                        UartEncodeAndSendMessage(commandValue, payload.Length, payload);
                        break;
                }
            }

            //byte[] array = Encoding.ASCII.GetBytes(textBoxEmission.Text);
            //serialPort1.Write(array, 0, array.Length);
        }

        void ProcessDecodedMessage(int msgFunction, int msgPayloadLength, byte[] msgPayload)
        {
            switch (msgFunction)
            {
                case 0x0020:
                    Led1
                    break;
            }
        }
        byte CalculateChecksum(int msgFunction, int msgPayloadLength, byte[] msgPayload)
        {

            byte checksum = 0;
            checksum ^= (byte)(0xFE);
            checksum ^= (byte)(msgFunction);
            checksum ^= (byte)(msgPayloadLength);

            for (int i = 0; i < msgPayloadLength; i++)
            {
                checksum ^= msgPayload[i];
            }

            return checksum;
        }


        void UartEncodeAndSendMessage(int msgFunction, int msgPayloadLength, byte[] msgPayload)
        {

            byte checksumSend = CalculateChecksum(msgFunction, msgPayloadLength, msgPayload);

            byte[] encodedMessage = new byte[msgPayloadLength + 6];
            int pos = 0;
            encodedMessage[pos++] = 0xFE;
            encodedMessage[pos++] = (byte)(msgFunction >> 8);
            encodedMessage[pos++] = (byte)(msgFunction >> 0);
            encodedMessage[pos++] = (byte)(msgPayloadLength >> 8);
            encodedMessage[pos++] = (byte)(msgPayloadLength >> 0);
            for (int i = 0; i < msgPayloadLength; i++)
            {
                encodedMessage[pos++] += msgPayload[i];
            }
            encodedMessage[pos++] = (byte)(checksumSend);
            serialPort1.Write(encodedMessage, 0, encodedMessage.Length);
        }


        public enum StateReception
        {
            Waiting,
            FunctionMSB,
            FunctionLSB,
            PayloadLengthMSB,
            PayloadLengthLSB,
            Payload,
            CheckSum
        }
        StateReception rcvState = StateReception.Waiting;
        int msgDecodedFunction = 0;
        int msgDecodedPayloadLength = 0;
        byte[] msgDecodedPayload;
        int msgDecodedPayloadIndex = 0;
        int calculatedChecksum = 0;

        private void DecodeMessage(byte c)
        {
            switch (rcvState)
            {
                case StateReception.Waiting:
                    if (c == 0xFE)
                    {
                        rcvState = StateReception.FunctionMSB;
                    }
                    break;

                case StateReception.FunctionMSB:
                    msgDecodedFunction = c << 8;
                    rcvState = StateReception.FunctionLSB;
                    break;
                case StateReception.FunctionLSB:
                    msgDecodedFunction += c;
                    rcvState = StateReception.PayloadLengthMSB;
                    break;
                case StateReception.PayloadLengthMSB:
                    msgDecodedPayloadLength = c << 8;
                    rcvState = StateReception.PayloadLengthLSB;
                    break;
                case StateReception.PayloadLengthLSB:
                    msgDecodedPayloadLength += c;
                    if (msgDecodedPayloadLength == 0)
                        rcvState = StateReception.CheckSum;
                    else if (msgDecodedPayloadLength >= 256)
                        rcvState = StateReception.Waiting;
                    else
                    {
                        msgDecodedPayload = new byte[msgDecodedPayloadLength];
                        msgDecodedPayloadIndex = 0;
                        rcvState = StateReception.Payload;
                    }
                    break;
                case StateReception.Payload:
                    msgDecodedPayload[msgDecodedPayloadIndex++] = c;
                    if (msgDecodedPayloadIndex >= msgDecodedPayloadLength)
                        rcvState = StateReception.CheckSum;
                    break;
                case StateReception.CheckSum:
                    calculatedChecksum = CalculateChecksum(msgDecodedFunction, msgDecodedPayloadLength, msgDecodedPayload);

                    if (calculatedChecksum == c)
                    {
                        Debug.WriteLine("Youpi");
                    }
                    else
                    {
                        Debug.WriteLine("Bouh");
                    }
                    rcvState = StateReception.Waiting;

                    break;
                default:
                    rcvState = StateReception.Waiting;
                    break;
            }
        }
    }
}
