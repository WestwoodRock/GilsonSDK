using GilsonSdk;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Mvvm;
using System.Text;

namespace GilsonGSIOCTester
{
    public class MainViewModel : ViewModel
    {

        #region Properties

        private string _baudRate;
        private bool _isConnected;
        private GSIOCConnection _connection;

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



        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; NotifyPropertiesChanged(nameof(IsConnected), nameof(ConnectCommand), nameof(DisconnectCommand)); }
        }


        #endregion
        #region Commands



        public System.Windows.Input.ICommand ConnectCommand
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

        public System.Windows.Input.ICommand DisconnectCommand
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
        #endregion

        #region Constructors

        public MainViewModel()
        {
            AvailablePorts = SerialPort.GetPortNames().OrderBy(x => x).ToList();

            SelectedComsPort = AvailablePorts.Last();

            BaudRate = "19200";
        }
        #endregion
    }
}
