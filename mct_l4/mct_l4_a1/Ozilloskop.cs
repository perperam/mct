using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace mct_l4_a1
{

    public delegate void neueDatenDelegatenTyp(double[] buffer);
    internal class Ozilloskop
    {

        SpiDevice ADC;
        GpioPin PinChipSelectLED;
        GpioPin PinChipSelectADC;
        GpioPin PinChipSelectDAC;

        Task task;

        // 2 Trigger * 2 Data * 960 Displayed Values
        byte[] writeBuffer = new byte[2 * 2 * 960];
        byte[] readBuffer = new byte[2 * 2 * 960];

        // 2 Trigger * 960 Displayed Values
        double[] values = new double[2 * 960];

        DispatcherTimer timer = new DispatcherTimer();

        public event neueDatenDelegatenTyp neueDaten;

        double triggLvl = 0;

        public double TriggLvl { get => triggLvl; set => triggLvl = value; }

        public Ozilloskop()
        {
            this.initGPIO();
            task = this.initSPI();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private async Task initSPI()
        {
            String spiDeviceSelector = SpiDevice.GetDeviceSelector();
            IReadOnlyList<DeviceInformation> devices = await
            DeviceInformation.FindAllAsync(spiDeviceSelector);
            var Settings = new SpiConnectionSettings(0);
            Settings.ClockFrequency = 4800000;
            Settings.Mode = SpiMode.Mode3;
            ADC = await SpiDevice.FromIdAsync(devices[0].Id, Settings);
        }

        private void initGPIO()
        {
            GpioController gpio = GpioController.GetDefault();
            PinChipSelectLED = gpio.OpenPin(28);
            PinChipSelectADC = gpio.OpenPin(35);
            PinChipSelectDAC = gpio.OpenPin(12);
            PinChipSelectLED.SetDriveMode(GpioPinDriveMode.Output);
            PinChipSelectADC.SetDriveMode(GpioPinDriveMode.Output);
            PinChipSelectDAC.SetDriveMode(GpioPinDriveMode.Output);

            PinChipSelectLED.Write(GpioPinValue.High);
            PinChipSelectADC.Write(GpioPinValue.Low);
            PinChipSelectDAC.Write(GpioPinValue.High);
        }

        private void Timer_Tick(object sender, object e)
        {
            int number;

            // value in bytes
            double value;

            byte firstMask = 0b_0000_1111;
            byte secondMask = 0b_1111_1100;

            // linear relation between ADC bits and Voltage
            double m = 0.006432;
            double b = -3.2699763;

            ADC.TransferFullDuplex(writeBuffer, readBuffer);

            // convert to Voltage Values
            // 2 Trigger * 960 Displayed Values
            for (int i = 0; i < 2 * 960; i++)
            {
                number = 0;
                number += (readBuffer[i*2] & firstMask) << 6;
                number += (readBuffer[i*2+1] & secondMask) >> 2;

                values[i] = m * number + b;
            }

            // Trigger to triggLvl and cut VoltageValues after it
            int trigI = 0;
            double[] cutValues = new double[960];

            for (int i = 0; i < 960; i++)
            {
                if (values[i] < values[i + 2] && values[i] >= triggLvl) {
                    trigI = i;
                    break;
                }
            }

            /*
            for (int i = 0; i < 960; i++) {
                cutValues[i] = values[i+trigI];
            }
            */

            cutValues = values.Skip(trigI).Take(960).ToArray();


            // data do delagate for main
            neueDaten(cutValues);
        }
    }
}
