using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
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

                result = await ExecuteImmediateCommandAsync('M', false);
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

        /// <summary>
        /// Clears the error asynchronously
        /// </summary>
        public async Task ClearErrorAsync()
        {
            await ExecuteBufferedCommandAsync('e');
        }
        /// <summary>
        /// Move to the Home position
        /// </summary>
        public async Task GoHomeAsync()
        {
            await ExecuteBufferedCommandAsync('H');
        }

        /// <summary>
        /// Move to the specified X and Y position
        /// /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="xspeed">The x transition speed</param>
        /// <param name="yspeed">The y transition speed.</param>
        public async Task GoToXYPositionAsync(double x, double y, double xspeed = 100, double yspeed = 100)
        {
            var x1 = x.ToString("F3");
            var y1 = y.ToString("F3");
            var xSpect = xspeed.ToString("F3");
            var ySpect = yspeed.ToString("F3");

            var locs = $"{x1}:{xSpect}/{y1}:{ySpect}";

            await ExecuteBufferedCommandAsync('X', locs);
        }

        /// <summary>
        /// Gets the X and Y position asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<(double x, double y)> GetXYPositionAsync()
        {
            var result = await ExecuteImmediateCommandAsync('P');

            var output = result.StringValue;

            var x = 0d;
            var y = 0d;

            if (output.Contains(@"/"))
            {
                var oSplit = output.Split('/');

                double.TryParse(oSplit[0], out x);
                double.TryParse(oSplit[1], out y);

            }

            return (x, y);
        }

        /// <summary>
        /// Gets the minimum and maximum ranges of travel for the liquid hander
        /// </summary>
        /// <returns></returns>
        public async Task<(double minX, double maxX, double minY, double maxY)> GetTravelRangeAsync()
        {
            var result = await ExecuteImmediateCommandAsync('Q');
            var output = result.StringValue;


            var minX = 0d;
            var maxX = 0d;
            var minY = 0d;
            var maxY = 0d;

            var mSplit = output.Split(' ');

            foreach (var coords in mSplit)
            {
                if (coords.Contains('='))
                {
                    var tempMin = 0d;
                    var tempMax = 0d;

                    var substring = coords.Substring(coords.IndexOf("=") + 1);

                    if (substring.Contains(@"/"))
                    {
                        var oSplit = substring.Split('/');

                        double.TryParse(oSplit[0], out tempMin);
                        double.TryParse(oSplit[1], out tempMax);

                    }

                    switch (coords[0])
                    {
                        case 'Y':
                        case 'y':
                            {
                                minY = tempMin;
                                maxY = tempMax;
                            }
                            break;
                        case 'X':
                        case 'x':
                            {
                                minX = tempMin;
                                maxX = tempMax;
                            }
                            break;
                    }
                }
            }

            return (minX, maxX, minY, maxY);
        }
        #endregion

    }
}
