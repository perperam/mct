using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Devices.Gpio;

using Windows.Devices.Enumeration;
using Windows.Devices.Spi;

namespace RGB_LED
{
    internal class LEDStreifen
    {
        GpioPin led_select;
        GpioPin adc;
        GpioPin dac;
        GpioPin reset_led;

        GpioController controller = GpioController.GetDefault();

        SpiDevice SPI_LED;
        bool initdone = false;
        Task ta;

        public LEDStreifen()
        {
            this.initGPIO();
            ta=this.initSPI();
        }

        public void reset()
        {
            reset_led.Write(GpioPinValue.Low);
            Task.Delay(10).Wait();
            reset_led.Write(GpioPinValue.High);
        }

        public void sende(byte[] daten)
        {
            if (ta.IsCompleted)
            {
                this.led_select.Write(GpioPinValue.Low);
                SPI_LED.Write(daten);
                this.led_select.Write(GpioPinValue.High);

            }
        }
    
        void initGPIO()
        {
            this.led_select = controller.OpenPin(28); // Chip Select LED-Streifen
            this.adc = controller.OpenPin(35); // Chip Select ADC
            this.dac = controller.OpenPin(12);
            this.reset_led = controller.OpenPin(25);

            this.led_select.SetDriveMode(GpioPinDriveMode.Output);
            this.adc.SetDriveMode(GpioPinDriveMode.Output);
            this.dac.SetDriveMode(GpioPinDriveMode.Output);
            this.reset_led.SetDriveMode(GpioPinDriveMode.Output);

            this.led_select.Write(GpioPinValue.High);
            this.adc.Write(GpioPinValue.High);
            this.dac.Write(GpioPinValue.High);
            this.reset_led.Write(GpioPinValue.High);
        }

        private async Task initSPI()
        {
            String spiDeviceSelector = SpiDevice.GetDeviceSelector();
            IReadOnlyList<DeviceInformation> devices = await
            DeviceInformation.FindAllAsync(spiDeviceSelector);
            SpiConnectionSettings SPI_Settings = new SpiConnectionSettings(0);
            SPI_Settings.ClockFrequency = 4800000;
            SPI_Settings.Mode = SpiMode.Mode0;
            SPI_LED = await SpiDevice.FromIdAsync(devices[0].Id, SPI_Settings);
            initdone = true;
        }
    }
}
