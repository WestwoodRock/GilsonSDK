using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace GilsonSdk
{
    public class Pump : GilsonGSIOCDevice
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Pump"/> class.
        /// </summary>
        /// <param name="deviceId">The device Id.</param>
        /// <param name="connection">The <see cref="GSIOCConnection" /> connection to use</param>
        public Pump(byte deviceId, GSIOCConnection connection) : base(deviceId, connection)
        {

        }
        #endregion

        #region Methods

        #endregion

    }
}
