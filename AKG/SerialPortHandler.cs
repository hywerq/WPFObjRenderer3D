using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AKG
{
    internal static class SerialPortHandler
    {
        static SerialPort serialPort = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.One);

        public static List<float> RotationBuffer = new List<float>();
        public static List<int> LightBuffer = new List<int>();

        [STAThread]
        public static void Start()
        {
            serialPort.ReadTimeout = 500;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            //serialPort.Open();
        }

        public static void SendCommand(string command)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write(command);
            }
        }

        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Task.Run( () =>
            {
                try
                {
                    string recieved = serialPort.ReadLine();
                    string data = Encoding.UTF8.GetString(Encoding.Default.GetBytes(recieved))
                        .Trim(new char[] { '\r', '\n' }).Replace('.', ',');

                    string[] command = data.Split(' ');

                    switch (command[0])
                    {
                        case "R":
                            if(float.TryParse(command[1], out float rValue))
                            {
                                if (rValue <= 3.3f && rValue >= 0.0f)
                                {
                                    RotationBuffer.Add(rValue);
                                }
                            }                            
                            break;

                        case "L":
                            if (int.TryParse(command[1], out int lValue))
                            {
                                if (lValue <= 100 && lValue >= 0)
                                {
                                    LightBuffer.Add(lValue);
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}
