using GilsonSdk.Enums;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Gets the pump status asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetStatusAsync()
        {
            var stats = await ExecuteImmediateCommandAsync('?');

            return stats.StringValue;
        }

        /// <summary>
        /// Gets the display read out asyncnorously
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDisplayTextAsync()
        {
            var readOut = await ExecuteImmediateCommandAsync('R');

            return readOut.StringValue;
        }

        /// <summary>
        /// Sets the pump speed asyncnorously
        /// </summary>
        /// <param name="speedRpm">The speed in hundredths of a revolution per minute</param>
        /// <exception cref="Exception">The specified speed is outside the acceptable range of 0 - 4800 hundredths of a revolution per minute</exception>
        public async Task SetPumpSpeedAsync(double speedRpm)
        {
            if (speedRpm < 0 || speedRpm > 4800)
                throw new Exception("The specified speed is outside the acceptable range of 0 - 4800 hundredths of a revolution per minute");

            var speedString = speedRpm.ToString();

            await ExecuteBufferedCommandAsync('R', speedString);
        }

        /// <summary>
        /// Sets the mode asynchronously
        /// </summary>
        /// <param name="mode">The mode.</param>
        public async Task SetModeAsync(PumpMode mode)
        {
            var modeKey = (mode == PumpMode.Keyboard) ? "K" : "R";

            await ExecuteBufferedCommandAsync('S', modeKey);

        }


        /// <summary>
        /// Sends an instruction to the pump motor asynchronously
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="allowReverse">if set to <c>true</c> [allow reverse].</param>
        /// <exception cref="Exception">You must specify allow reverse if instructing the pump to go backwards</exception>
        public async Task SendMotorInstructionAsync(PumpInstruction instruction, bool allowReverse = false)
        {
            if (!allowReverse && instruction == PumpInstruction.Backwards)
                throw new Exception("You must specify allow reverse if instructing the pump to go backwards");

            var intraChar = Convert.ToChar((byte)instruction).ToString();

            await ExecuteBufferedCommandAsync('K', intraChar);
        }

        /// <summary>
        /// Gets the previous key press and its status asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPreviousKeyPressAsync()
        {
            var readOut = await ExecuteImmediateCommandAsync('K');

            return readOut.StringValue;
        }

        /// <summary>
        /// Reads the analogue input status.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReadAnalogueInputStatusAsync()
        {
            var readOut = await ExecuteImmediateCommandAsync('V');

            return readOut.StringValue;
        }

        public async Task<string> ReadContactInputStatusAsync()
        {
            var readOut = await ExecuteImmediateCommandAsync('I');

            return readOut.StringValue;
        }

        #endregion

    }
}
