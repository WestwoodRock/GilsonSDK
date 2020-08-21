using GilsonSdk;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Windows.Input;

namespace GilsonGSIOCTester
{
    public class MainViewModel : ViewModel
    {
        #region Fields
        private string _baudRate;
        private bool _isConnected;
        private GSIOCConnection _connection;
        private string _scanningStatus;
        private List<GSIOCDeviceInfo> _devices;
        private GSIOCDeviceInfo _selectedDevice;
        #endregion

        #region Properties



        public string BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; NotifyPropertyChanged(nameof(BaudRate)); }
        }

        private string _selectedComsPort;

        public string SelectedComsPort
        {
            get { return _selectedComsPort; }
            set { _selectedComsPort = value; NotifyPropertyChanged(nameof(SelectedComsPort)); }
        }

        private List<string> _availablePort;

        public List<string> AvailablePorts
        {
            get { return _availablePort; }
            set { _availablePort = value; NotifyPropertyChanged(nameof(AvailablePorts)); }
        }

        public string ScanningStatus
        {
            get { return _scanningStatus; }
            set { _scanningStatus = value; NotifyPropertyChanged(nameof(ScanningStatus)); }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; NotifyPropertiesChanged(nameof(IsConnected), nameof(ConnectCommand), nameof(DisconnectCommand)); }
        }


        public List<GSIOCDeviceInfo> AvailableDevices
        {
            get { return _devices; }
            set { _devices = value; NotifyPropertyChanged(nameof(AvailableDevices)); }
        }



        public GSIOCDeviceInfo SelectedDevice
        {
            get { return _selectedDevice; }
            set { _selectedDevice = value; NotifyPropertyChanged(nameof(SelectedDevice)); }
        }

        #endregion
        #region Commands



        public ICommand ConnectCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        var connection = new GSIOCConnection(SelectedComsPort, 19200);

                        connection.Open();

                        _connection = connection;

                        IsConnected = true;
                    }
                    catch (Exception ex)
                    {

                        NotifyErrorOccured(ex);
                    }
                },()=>
                {
                    return !IsConnected;
                });
            }
        }

        public ICommand DisconnectCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                       if (_connection != null)
                        {
                            _connection.Close();

                            _connection = null;

                            IsConnected = false;
                        }
                    }
                    catch (Exception ex)
                    {

                        NotifyErrorOccured(ex);
                    }
                }, () =>
                {
                    return IsConnected;
                });
            }
        }



        public ICommand ScanCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    try
                    {
                        SelectedDevice = null;

                        ScanningStatus = string.Empty;

                        var deviceIds = await _connection.FindAllDevicesAsync(0, (currentPort) =>
                        {
                            var message = $"Scanning: {currentPort}";

                            UI.InvokeOnUIThread(() =>
                            {
                                ScanningStatus = message;
                            });
                        });

                        AvailableDevices = deviceIds;

                        ScanningStatus = string.Empty;

                        ScanningStatus = $"Found {AvailableDevices.Count} Devices";

                    }
                    catch (Exception ex)
                    {

                        NotifyErrorOccured(ex);
                    }
                }, (obj) =>
                {
                    return IsConnected;
                });
            }

        }

        #endregion

        #region Constructors

        public MainViewModel()
        {
            AvailablePorts = SerialPort.GetPortNames().OrderBy(x => x).ToList();

            SelectedComsPort = AvailablePorts.Last();

            BaudRate = "19200";

            AvailableDevices = new List<GSIOCDeviceInfo>();
        }
        #endregion
    }
}
