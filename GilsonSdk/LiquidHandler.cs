using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace GilsonSdk
{
    public class LiquidHandler : GilsonGSIOCDevice
    {

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidHandler"/> class.
        /// </summary>
        /// <param name="deviceId">The device Id.</param>
        /// <param name="connection">The <see cref="GSIOCConnection" /> connection to use</param>
        public LiquidHandler(byte deviceId, GSIOCConnection connection) : base(deviceId, connection)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Wait until the specified status is matched or the status is E (Errored) or U (Unpowered)
        /// </summary>
        /// <param name="status">The statusto wait for</param>
        /// <param name="statusUpdater">Current status updater action for showing the current status untilt the specified status is matched</param>
        /// <returns></returns>
        /// <param name="queryDelay">The dela, between motor status queries. in millseconds</param>
        /// <returns></returns>
        public async Task<string> WaitForMotorStatusAsync(byte deviceId, char status, Action<string> statusUpdater = null, int queryDelay = 0)
        {
            var result = await ExecuteImmediateCommandAsync('M');

            var mStatus = result.StringValue;

            statusUpdater?.Invoke(mStatus);

            if (mStatus[0] == 'E' || mStatus[0] == 'U')
                return mStatus;

            while ((mStatus[0] != status) && (result.BinaryData[0] != 0xd5))
            {

                result = await ExecuteImmediateCommandAsync('M');
                mStatus = result.StringValue;

                statusUpdater?.Invoke(mStatus);

                if (mStatus[0] == 'E' || mStatus[0] == 'U')
                    return mStatus;

                if (queryDelay > 0)
                    await Task.Delay(queryDelay);
            }

            return mStatus;
        }

        /// <summary>
        /// Gets the error number asynchronous.
        /// </summary>
        /// <param name="deviceId">The device Id.</param>
        /// <returns></returns>
        public async Task<(int ErrorCode, string ErrorMessage)> GetErrorNumberAsync(byte deviceId)
        {
            var result = await ExecuteImmediateCommandAsync('e');

            var output = result.StringValue;

            var errorCode = 0;

            if (output.IndexOf("\r") == -1)
            {
                var erroCode = output.Substring(0, output.IndexOf(" "));

                errorCode = int.Parse(erroCode);
            }
            else
            {
                var erroCode = output.Substring(output.IndexOf("\r") + 1);

                erroCode = erroCode.Substring(0, erroCode.IndexOf(" "));

                errorCode = int.Parse(erroCode);
            }


            return (errorCode, output);
        }
        #endregion

    }
}
