using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;

namespace GilsonSdk
{
    public abstract class GilsonGSIOCDevice : IDisposable
    {
        #region Fields
        private GSIOCConnection _connection;
        #endregion

        #region Properties



        public byte DeviceId { get; private set; }


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GilsonGSIOCDevice"/> class.
        /// </summary>
        /// <param name="deviceId">The device Id.</param>
        /// <param name="connection">The <see cref="GSIOCConnection"/> connection to use</param>
        protected GilsonGSIOCDevice(byte deviceId, GSIOCConnection connection)
        {
            DeviceId = deviceId;
            _connection = connection;
        }
        #endregion

        #region Public Async Methods

        /// <summary>
        /// Connects to this device Id asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<byte> ConnectAsync() => _connection.ConnectAsync(DeviceId);

        /// <summary>
        /// Gets the module information for this device asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<string> GetModuleInfoAsync() => _connection.GetModuleInfoAsync(DeviceId);

        /// <summary>
        /// Sends the master reset to this device asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<string> ResetDeviceAsync() => _connection.ResetDeviceAsync(DeviceId);

        /// <summary>
        /// Executes an immediate instruction on this device asynchronously
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task<(byte[] BinaryData, string StringValue)> ExecuteImmediateCommandAsync(char command)
        {
            await ConnectAsync();

            return await _connection.ExecuteImmediateCommandAsync(command);
        }

        /// <summary>
        /// Executes a buffered command on this device asynchronously
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="parameters">The parameters.</param>
        public async Task ExecuteBufferedCommandAsync(char command, string parameters = null)
        {
            await ConnectAsync();

            await _connection.ExecuteBufferedCommandAsync(command, parameters);
        }

        public void Dispose()
        {
            _connection = null;
        }




        #endregion

    }
}
