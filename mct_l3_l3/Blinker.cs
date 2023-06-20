﻿using Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

using Support;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace mct_l3_l1
{
    delegate void blinkerTyp(bool anAus);

    internal class Blinker
    {
        IOPult ioPult = IOPult.GetCurrentPult();

        private DispatcherTimer timer = new DispatcherTimer();
        private bool status;

        public event blinkerTyp rechts;
        public event blinkerTyp links;

        int counter;

        public Blinker()
        {
            this.status = false;

            timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timer.Tick += Timer_Tick;
            // timer_Tick += links;
            // timer.Tick += rechts;
            this.Start();
        }

        private void Timer_Tick(object sender, object e)
        {   
            // links
            if ((ioPult.PortB & 1 << 7) != 0)
            {
                blinkLinks();
            }
            else {
                // turn of when of and not blink
                ioPult.PortA = (byte)(ioPult.PortA & 0x0F);
                if (links != null) links(false);
            }

            // rechts
            if ((ioPult.PortB & 1 << 6) != 0) {
                blinkRechts();
            } else
            {
                // turn of when of and not blink
                ioPult.PortA = (byte)(ioPult.PortA & 0xF0);
                if (rechts != null) rechts(false);
            }


            status = !status;
            if ( (ioPult.PortB & 1 << 7) != 0 || (ioPult.PortB & 1 << 6) != 0)
            {
                if (counter <= 4)
                {
                    counter++;
                }
                else
                {
                    counter = 0;
                }
            }
                
        }

        public void blinkLinks()
        {
            byte maske = 0;

            maske = (byte) (0x0F << (counter));
            maske = (byte) (maske & 0xF0);

            ioPult.PortA = (byte)(ioPult.PortA | maske);

            if (links != null)
            {
                if (counter != 0) links(true);
                else links(false);
            }
        }

        public void blinkRechts() {
            if (status)
            {
                ioPult.PortA = (byte)(ioPult.PortA | 0x0F);
                if(rechts!=null) rechts(true);
            }
            else
            {
                ioPult.PortA = (byte)(ioPult.PortA & 0xF0);
                if (rechts != null) rechts(false);
            }
        }

        public void Start() {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}