using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support
{/// <summary>
 /// Stellt eine Verbindung zum I/O-Pult des Fh-Kiel IoT-PC zur Verfügung
 /// </summary>
    public class IOPult
    {
        PinExpander Expander1 = new PinExpander();
        PinExpander Expander2 = new PinExpander();
        bool initdone = false;
        /// <summary>
        /// Erzeugt ein Objekt zum Zugriff auf das Paultgehäuse
        /// </summary>
        /// <returns>Eine Instanz der Klasse</returns>
        private static IOPult instance = null;
        public static IOPult GetCurrentPult()
        {
            if (instance == null)
                instance = new IOPult();

            return instance;
        }
        private IOPult()
        {
            Init();
        }
        /// <summary>
        /// Schreiben Sie auf diese Eigenschaft, um den Zustand des Ports A zu ändern
        /// </summary>
        public byte PortA
        {
            get => initdone ? Expander1.PortA : (byte)0;
            //LED dominant

            set
            {
                if (initdone)
                {
                    Expander1.PortA = value;
                    Expander1.PortB = value;
                }
            }

        }
        /// <summary>
        /// Schreiben Sie auf diese Eigenschaft, um den Zustand des Ports A nur die LED zu ändern
        /// </summary>
        public byte PortA_LED
        {
            get => initdone ? Expander1.PortA : (byte)0;

            set
            {
                if (initdone)
                {
                    Expander1.PortA = value;
                }
            }

        }
        /// <summary>
        /// Schreiben Sie auf diese Eigenschaft, um den Zustand des Ports A nur die Buchsen zu ändern
        /// </summary>
        public byte PortA_Buchsen
        {
            get => initdone ? Expander1.PortB : (byte)0;
            set
            {
                if (initdone)
                {
                    Expander1.PortB = value;
                }
            }

        }
        /// <summary>
        /// Lesen Sie diese Eigenschaft, um den Zustand der Schalter an Ports B zu erfahren
        /// </summary>
        public byte PortB
        {
            get => initdone ? Expander2.PortB : (byte)0;
            set

            {
                if (initdone)
                {
                    Expander2.PortB = value;
                }
            }

        }
        /// <summary>
        /// Lesen Sie diese Eigenschaft, um den Zustand der Buchsen an Ports C zu erfahren
        /// </summary>
        public byte PortC
        {
            get => initdone ? Expander2.PortA : (byte)0;
            set
            {
                if (initdone)
                {
                    Expander2.PortA = value;
                }
            }

        }
        private async void Init()
        {
            await Expander1.Init(0x20);
            await Expander2.Init(0x21);
            Expander1.PortAisInput = false;
            Expander1.PortBisInput = false;
            Expander2.PortAisInput = true;
            Expander2.PortBisInput = true;
            initdone = true;

        }
    }
}
