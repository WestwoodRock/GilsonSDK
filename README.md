# Gilson GSIOC SDK for Microsoft .Net written in C#  

Also includes GSIOC Command Utility for sending commands to Gilson Devices

### Features

- Completed managed code
- Scanning for devices
- Sending Immediate and Buffered commands
- Clasess for specific device functions such as Pump and Liquid Handlers
- Uses System.IO.Ports nuget package and is .Net Standard 2.0 compatible 

## Connecting

To connect create a new instance of `GSIOCConnection` and provide the relecant Serail connection details, port name, baud rate, Parity, DataBits and stop bits.

    using GilsonSdk;

    var connection = new GSIOCConnection("COM6", 19200);

    connection.Open();

*Note: SerialPorts.Available ports can be used to find available ports*  

## Finding Devices

`GSIOCConnection` provides the `FindAllDevicesAsync` method for scanning for all devices.

    await _connection.DisconnectDevicesAsync(); //tells all devices to disconnect

    var deviceIds = await _connection.FindAllDevicesAsync(0, (currentPort) =>
                        {
                            var message = $"Scanning: {currentPort}";

                            UI.InvokeOnUIThread(() =>
                            {
                                ScanningStatus = message;
                            });
                        });
`FindAllDevicesAsync` returns a `List<GSIOCDeviceInfo>` object that contains the `Id` of the device and the module information.

`FindFirstDeviceAsync` will similarly scan for devices, but it will stop after finding the first device.


