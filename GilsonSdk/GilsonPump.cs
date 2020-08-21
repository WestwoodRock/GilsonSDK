using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace GilsonSdk
{
    public class GilsonPump : GilsonGSIOCDevice
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public GilsonPump(SerialPort port) : base(port)
        {
        }

        public GilsonPump(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) : base(portName, baudRate, parity, dataBits, stopBits)
        {

        }


        #endregion

        #region Methods

        #endregion

    }
}
