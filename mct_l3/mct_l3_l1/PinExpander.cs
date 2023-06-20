using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Support
{
    public class PinExpander
    {
        #region Definitionen 

        private const byte BASE_PORT_EXPANDER_I2C_ADDRESS = 0x20; // 7-bit I2C address of the first port expander

        private const byte PORT_EXPANDER_IODIRA_REGISTER_ADDRESS = 0x00; // IODIR register controls the direction of the GPIO on the port expander
        private const byte PORT_EXPANDER_IODIRB_REGISTER_ADDRESS = 0x01; // IODIR register controls the direction of the GPIO on the port expander

        private const byte PORT_EXPANDER_IPOLA_REGISTER_ADDRESS = 0x02; // Input Polarity Register
        private const byte PORT_EXPANDER_IPOLB_REGISTER_ADDRESS = 0x03; // Input Polarity Register

        private const byte PORT_EXPANDER_GPINTA_ADDRESS = 0x04; // Interrput on Change pin 
        private const byte PORT_EXPANDER_GPINTB_ADDRESS = 0x05; // Interrput on Change pin 

        private const byte PORT_EXPANDER_DEFVALA_ADDRESS = 0x06; // Default Compare Register for Interrupt-on-change
        private const byte PORT_EXPANDER_DEFVALB_ADDRESS = 0x07; // Default Compare Register for Interrupt-on-change

        private const byte PORT_EXPANDER_INTCONA_ADDRESS = 0x08; // Interrupt Control Register
        private const byte PORT_EXPANDER_INTCONB_ADDRESS = 0x09; // Interrupt Control Register

        private const byte PORT_EXPANDER_IOCON_ADDRESS = 0x0A;  // I/O Expander Configruation Register

        private const byte PORT_EXPANDER_GPPUA_ADDRESS = 0x0C; // GPIO Pull-up Resistor Register
        private const byte PORT_EXPANDER_GPPUB_ADDRESS = 0x0D; // GPIO Pull-up Resistor Register

        private const byte PORT_EXPANDER_INTFA_ADDRESS = 0x0E; //Interrupt Flag Register
        private const byte PORT_EXPANDER_INTFB_ADDRESS = 0x0F; //Interrupt Flag Register

        private const byte PORT_EXPANDER_INTCAPA_ADDRESS = 0x10; //Interrupt Capture Register
        private const byte PORT_EXPANDER_INTCAPB_ADDRESS = 0x11; //Interrupt Capture Register

        private const byte PORT_EXPANDER_GPIOA_REGISTER_ADDRESS = 0x12; // GPIO register is used to read the pins input
        private const byte PORT_EXPANDER_GPIOB_REGISTER_ADDRESS = 0x13; // GPIO register is used to read the pins input

        private const byte PORT_EXPANDER_OLATA_REGISTER_ADDRESS = 0x14; // Output Latch register is used to set the pins output high/low
        private const byte PORT_EXPANDER_OLATB_REGISTER_ADDRESS = 0x15; // Output Latch register is used to set the pins output high/low

        #endregion


        private I2cDevice i2cPortExpander;


        public async Task<int> Init(int address)
        {
            string aqs = I2cDevice.GetDeviceSelector("I2C0");
            var dis = await DeviceInformation.FindAllAsync(aqs);
            /*
            if (dis.Count > 0)
                Debug.WriteLine(dis[0].Id);
*/
            if (dis.Count == 0)
            {
                Debug.WriteLine("Kein I2C-Teilnehmer gefunden");
                return 0;    
            }  
            
            var settings = new I2cConnectionSettings(address);
            settings.BusSpeed = I2cBusSpeed.StandardMode;

            i2cPortExpander = await I2cDevice.FromIdAsync(dis[0].Id, settings);
          

            //Input polarity auf 0x00
            i2cPortExpander.Write(new byte[] { PORT_EXPANDER_IPOLA_REGISTER_ADDRESS, 0x00 });
            i2cPortExpander.Write(new byte[] { PORT_EXPANDER_IPOLB_REGISTER_ADDRESS, 0x00 });

            //Interrupt on change of pin 0x00 disable
            i2cPortExpander.Write(new byte[] { PORT_EXPANDER_GPINTA_ADDRESS, 0x00 });
            i2cPortExpander.Write(new byte[] { PORT_EXPANDER_GPINTB_ADDRESS, 0x00 });
            i2cPortExpander.Write(new byte[] { PORT_EXPANDER_IOCON_ADDRESS, 0x00 });
            //Interupt on change aus

            return 0;
        }

        
        private bool portAisInput = false;
        private bool portBisInput = false;

        /// <summary>
        /// Definert alle Pins von PortA als Input und aktiviert die Pullup-Widerstände
        /// </summary>
        public bool PortAisInput
        {
            get
            {
                return portAisInput;
            }
            set

            {
                portAisInput = value;
                if (value)
                {
                    i2cPortExpander.Write(new byte[] { PORT_EXPANDER_IODIRA_REGISTER_ADDRESS, 0xFF });
                    i2cPortExpander.Write(new byte[] { PORT_EXPANDER_GPPUA_ADDRESS, 0xFF });
                }
                else
                {
                    i2cPortExpander.Write(new byte[] { PORT_EXPANDER_IODIRA_REGISTER_ADDRESS, 0x00 });
                    i2cPortExpander.Write(new byte[] { PORT_EXPANDER_GPPUA_ADDRESS, 0x00 });
                }
            }
        }
        public bool PortBisInput
        {
            get
            {
                return portBisInput;
            }
            set

            {
                portAisInput = value;
                if (value)
                {
                    i2cPortExpander.Write(new byte[] { PORT_EXPANDER_IODIRB_REGISTER_ADDRESS, 0xFF });
                    i2cPortExpander.Write(new byte[] { PORT_EXPANDER_GPPUB_ADDRESS, 0xFF });
                }
                else
                {
                    i2cPortExpander.Write(new byte[] { PORT_EXPANDER_IODIRB_REGISTER_ADDRESS, 0x00 });
                    i2cPortExpander.Write(new byte[] { PORT_EXPANDER_GPPUB_ADDRESS, 0x00 });
                }
            }
        }
        /// <summary>
        /// PortA des Pultgehäuses
        /// </summary>
        public byte PortA
        {
            get
            {
                byte[] readBuffer = new byte[1];
                i2cPortExpander.WriteRead(new byte[] { PORT_EXPANDER_GPIOA_REGISTER_ADDRESS }, readBuffer);
                return readBuffer[0];
            }
            set
            {
                i2cPortExpander.Write(new byte[] { PORT_EXPANDER_OLATA_REGISTER_ADDRESS, value });
            }
        }
        /// <summary>
        /// PortB des Pultgehäuses
        /// </summary>
        public byte PortB
        {
            get
            {
                byte[] readBuffer = new byte[1];
                 i2cPortExpander.WriteRead(new byte[] { PORT_EXPANDER_GPIOB_REGISTER_ADDRESS }, readBuffer);
                return readBuffer[0];
            }
            set
            {
                i2cPortExpander.Write(new byte[] { PORT_EXPANDER_OLATB_REGISTER_ADDRESS, value });
            }
        }

    }
}
