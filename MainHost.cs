using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using InTheHand.Bluetooth;

namespace ULS24_Host
{ 
    public partial class MainHost : Form
    {
        //BluetoothClient bluetoothClient = new BluetoothClient();

        List<BluetoothDevice> _bluetoothDevices;
        private ULS24Device _activeBleDevice = null;
        bool _isSearchingForDevices = false;

        private BluetoothLEScan bleScan = null;
        private BluetoothLEScanOptions bleScanOptions;
        private BluetoothLEScanFilter bleScanFilter;

        private TaskCompletionSource<bool> _BleScanPromise;

        private CancellationToken captureCancellationToken;
        private CancellationTokenSource captureCancellationSource = new CancellationTokenSource();
        private bool captureInProgress = false;


        private string dt;
        private string tt;
        public MainHost()
        {
            InitializeComponent();
            label6.Text = @"C:\";
            _bluetoothDevices = new List<BluetoothDevice>();

            Bluetooth.AdvertisementReceived += onBleDeviceDiscovered;

            bleScanFilter = ULS24Device.getBleScanFilter();

            bleScanOptions = new BluetoothLEScanOptions();
            bleScanOptions.Filters.Add(bleScanFilter);
            bleScanOptions.AcceptAllAdvertisements = false;
            bleScanOptions.KeepRepeatedDevices = false;
        }
        public bool IsSearchingForDevices
        {
            get => _isSearchingForDevices;
            set
            {
                _isSearchingForDevices = value;
                Cmd_Start.Enabled = !_isSearchingForDevices;
                Cmd_Capture.Enabled = !_isSearchingForDevices;
            }
        }

        public async Task<bool> SearchDevicesAsync()
        {
            _BleScanPromise = new TaskCompletionSource<bool>();
            bleScan = await Task.Run(() => Bluetooth.RequestLEScanAsync(bleScanOptions));
            return await _BleScanPromise.Task;
        }

        private async void onBleDeviceDiscovered(object sender, BluetoothAdvertisingEvent e)
        {
            if ((bleScan != null) && (bleScan.Active != false) && (e.Device != null))
            {
                if ((e.Device.Name == "ULS24") && (e.Uuids.Contains(ULS24Device.ImageCaptureServiceUuid)))
                {
                    //FIXME: stop on the first device for demo purposes. Full implementation shall proceed with scanning for additional devices
                    bleScan.Stop();
                    _bluetoothDevices.Add(e.Device);
                    _BleScanPromise.TrySetResult(true);
                }
            }
        }

        delegate void SetEnabledCallback(Button targer, bool enabled);
        delegate void SetTextCallback(Button targer, string text);

        private void SetControlEnabled(Control target, bool enabled)
        {
            if (target.InvokeRequired)
            {
                SetEnabledCallback enClbck = new SetEnabledCallback(SetControlEnabled);
                this.Invoke(enClbck, new object[] { target, false });
            }
            else
            {
                target.Enabled = enabled;
            }
        }

        private void SetButtonText(Button target, string text)
        {
            if (target.InvokeRequired)
            {
                SetTextCallback textClbck = new SetTextCallback(SetButtonText);
                this.Invoke(textClbck, new object[] { target, text });
            }
            else
            {
                target.Text = text;
            }
        }
        private void onBleDeviceDisconnected(object sender, EventArgs e)
        {
            SetControlEnabled(Cmd_Start, false);
            SetControlEnabled(Cmd_Capture, false);
            SetControlEnabled(tabControl1, false);
            SetButtonText(Cmd_ConnectDisconnect, "Connect");
            _activeBleDevice = null;
        }

        public void Cmd_Exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }
        
        private void DisplayLbItems()
        {
            listBox1.DisplayMember = "Name";
            listBox1.DataSource = _bluetoothDevices;
        }

        /*
        public async Task SearchDevices()
        {
            IsSearchingDevices = true;
            Devices.Clear();
            var client = new BluetoothClient();
            var devices = new List<BluetoothDeviceInfo>();
            try
            {
                devices.AddRange((await Task.Run(() => client.DiscoverDevices())).ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Devices.AddRange(devices.Select(x => new DeviceItemViewModel(x)));
                client.Close();
                IsSearchingDevices = false;
            }
        }*/

        public void Cmd_Start_Click(object sender, EventArgs e)
        {
            /* Demo of the capturing process */
            if ((_activeBleDevice == null) || (_activeBleDevice.IsConnected == false))
            {
                /* No device is connected, cannot proceed */
                return;
            }

            dt = DateTime.Now.ToString("ddMMyyyy");
            tt = DateTime.Now.ToString("HHmmss");

            try
            {
                /*******************************************/
                if (!captureInProgress)
                {
                    Cmd_ConnectDisconnect.Enabled = false;
                    captureCancellationSource = new CancellationTokenSource();
                    captureCancellationToken = captureCancellationSource.Token;
                    
                    int period = (int)photoRateUpDown.Value;

                    Cmd_Start.Text = "Stop Sampling";
                    captureInProgress = true;

                    _ = Task.Run(async () => {
                        String fileNamePrefix = Guid.NewGuid().ToString() + "_";
                        int frameIndex = 0;
                        int durationLimit = (int)sessionDurationUpDown.Value;
                        int accumulatedDuration = 0;
                        while ((!captureCancellationToken.IsCancellationRequested) && (accumulatedDuration <= durationLimit))
                        {
                            await _activeBleDevice.CaptureSingleFrame();

                            try
                            {
                                if ((_activeBleDevice != null) && (_activeBleDevice.latestImage != null))
                                {
                                    ULSSensorImage img = _activeBleDevice.latestImage;
                                    
                                    img.StoreBMP(label6.Text, fileNamePrefix + frameIndex.ToString());
                                    img.StoreRAW(label6.Text, fileNamePrefix + frameIndex.ToString(), dt, tt, true);

                                }
                            }
                            catch (Exception)
                            {
                                //ignore
                            }
                            finally
                            {
                                frameIndex++;
                            }

                            await Task.Delay(TimeSpan.FromSeconds(period));
                            accumulatedDuration += period;
                            if (accumulatedDuration > durationLimit)
                            {
                                _StopContinuousCapture();
                            }
                        }
                    });
                }
                else
                {
                    _StopContinuousCapture();

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Cmd_ConnectDisconnect.Enabled = true;
            }
            finally
            {
                Cmd_Start.Enabled = true;
            }
        }

        private void _StopContinuousCapture()
        {
            SetButtonText(Cmd_Start, "Start Sampling");
            SetControlEnabled(Cmd_ConnectDisconnect, true);
            captureCancellationSource.Cancel();
            captureInProgress = false;
        }

        private void Cmd_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if ((_activeBleDevice != null) && (_activeBleDevice.latestImage != null))
                {
                    ULSSensorImage img = _activeBleDevice.latestImage;
                    string fileName = img.guid.ToString();
                    img.StoreBMP(label6.Text, fileName);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Cmd_File_Click(object sender, EventArgs e)
        {
            string folderPath = "";
            FolderBrowserDialog directchoosedlg = new FolderBrowserDialog();
            if (directchoosedlg.ShowDialog() == DialogResult.OK)
            {
                folderPath = directchoosedlg.SelectedPath;
                label6.Text = folderPath;
            }
        }

        private async void Cmd_ConnectDisconnect_Click(object sender, EventArgs e)
        {
            if ((null == _activeBleDevice) || (_activeBleDevice.IsConnected == false))
            {
                try
                {
                    IsSearchingForDevices = true;
                    Cmd_ConnectDisconnect.Enabled = false;
                    
                    listBox1.DataSource = null;
                    listBox1.Items.Clear();

                    _bluetoothDevices.Clear();
                    await SearchDevicesAsync();

                    if (_bluetoothDevices.Count > 0)
                    {
                        DisplayLbItems();
                        _activeBleDevice = await ULS24Device.buildFromBleDevice(_bluetoothDevices[0]); //FIXME: using the very first discovered device as a connection point
                        if (_activeBleDevice != null)
                        {
                            _activeBleDevice.addDeviceDisconnectedHandler(onBleDeviceDisconnected);
                            Cmd_Start.Enabled = true;
                            Cmd_ConnectDisconnect.Text = "Disconnect";
                        }
                        else
                        {
                            throw new Exception("BLE connection error. Try to re-connect to the deivce");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Couldn't search for bluetooth devices: {ex.Message}");
                }
                finally
                {
                    IsSearchingForDevices = false;
                    Cmd_ConnectDisconnect.Enabled = true;
                    tabControl1.Enabled = true;
                }
            }
            else
            {
                try
                {
                    Cmd_ConnectDisconnect.Enabled = false;
                    tabControl1.Enabled = false;
                    _activeBleDevice.Disconnect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    _activeBleDevice = null;
                    Cmd_ConnectDisconnect.Enabled = true;
                }
            }
        }

        private async void Cmd_Capture_Click(object sender, EventArgs e)
        {
            if (_activeBleDevice != null)
            {

                

                SetControlEnabled(Cmd_Capture, false);
                Bitmap frame = await _activeBleDevice.CaptureSingleFrame();
                singleFramePictureBox.Image = frame;
                singleFramePictureBox.Invalidate();
                SetControlEnabled(Cmd_Capture, true);

            }
        }

        private void Cmd_SaveRaw_Click(object sender, EventArgs e)
        {

            dt = DateTime.Now.ToString("ddMMyyyy");
            tt = DateTime.Now.ToString("HHmmss");
            try
            {
                if ((_activeBleDevice != null) && (_activeBleDevice.latestImage != null))
                {
                    ULSSensorImage img = _activeBleDevice.latestImage;
                    String fileName = img.guid.ToString();
                    img.StoreRAW(label6.Text, fileName, dt, tt, false);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Cmd_Rate_Click(object sender, EventArgs e)
        {

        }

        private void imgIntegrationTimeUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_activeBleDevice != null)
            {
                _activeBleDevice.integrationTimeUs = (uint)(((NumericUpDown)sender).Value);
            }
        }

        private void imgLowGainRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (_activeBleDevice != null)
            {
                if (((RadioButton)sender).Checked)  
                {
                    _activeBleDevice.useLowGain = true;
                }
                else
                {
                    _activeBleDevice.useLowGain = false;
                }
            }
        }

        private void imgResolutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_activeBleDevice != null)
            {
                if (((ComboBox)imgResolutionComboBox).SelectedIndex == 0)
                {
                    _activeBleDevice.useLowResMode = true;
                }
                else
                {
                    _activeBleDevice.useLowResMode = false;
                }
            }
        }

        private void Cmd_Int_Click(object sender, EventArgs e)
        {

        }

        private void Cmd_Sens_Click(object sender, EventArgs e)
        {

        }
    }
}
