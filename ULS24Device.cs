using InTheHand.Bluetooth;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading.Tasks;

namespace ULS24_Host
{
    internal class ULS24Device
    {
        public enum ULS24_PixelGain
        {
            Low,
            High
        }
        public struct ULS24_PixelBinningPattern
        {
            public bool Subpixel1_Active;
            public bool Subpixel2_Active;
            public bool Subpixel4_Active;
            public bool Subpixel8_Active;
            public ULS24_PixelGain gain;

            public ULS24_PixelBinningPattern(byte singleByteRawData)
            {
                Subpixel8_Active = (singleByteRawData & (1 << 0)) != 0;
                Subpixel4_Active = (singleByteRawData & (1 << 1)) != 0;
                Subpixel2_Active = (singleByteRawData & (1 << 2)) != 0;
                Subpixel1_Active = (singleByteRawData & (1 << 3)) != 0;
                gain = (singleByteRawData & (1 << 4)) != 0 ? ULS24_PixelGain.Low : ULS24_PixelGain.High;
            }

            public ULS24_PixelBinningPattern(byte[] rawData)
            {
                Subpixel8_Active = rawData[0] != 0;
                Subpixel4_Active = rawData[1] != 0;
                Subpixel2_Active = rawData[2] != 0;
                Subpixel1_Active = rawData[3] != 0;
                gain = rawData[4] != 0 ? ULS24_PixelGain.Low : ULS24_PixelGain.High;
            }

            public const uint BinningPatternLength = 5;
        }

        public struct ULS24_TrimSettings
        {
            public const UInt32 TRIM_IMAGER_SIZE = 12;

            public UInt16[,] kbi { get; }       // New (auto) calib method with hump factor, all are int
            public UInt16[,] fpni { get; }		// 0 - lg, 1 - hg

            public ULS24_TrimSettings(byte[] rawData)
            {
                kbi = new UInt16[TRIM_IMAGER_SIZE, 6];
                fpni = new UInt16[2, TRIM_IMAGER_SIZE];

                int position = 2; // skip serial number
                for (int i = 0; i < TRIM_IMAGER_SIZE; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        kbi[i, j] = TrimBuff2Int(rawData, position);
                        position += 2;
                    }
                }

                for (int i = 0; i < TRIM_IMAGER_SIZE; i++)
                {
                    fpni[0, i] = TrimBuff2Int(rawData, position);
                    position += 2;
                    fpni[1, i] = TrimBuff2Int(rawData, position);
                    position += 2;
                }
            }

            private UInt16 TrimBuff2Int(byte[] rawData, int pos)
            {
                UInt16 result;
                result = (UInt16)(((UInt16)rawData[pos] << 8) | ((UInt16)rawData[pos + 1]));
                return result;
            }
        }

        public struct ULS24_CapturedFrameData
        {
            public UInt16 frameIndex { get; }
            public ULS24_PixelBinningPattern usedBinningPattern { get; }
            public UInt16[,] FrameBuffer { get; }

            public ULS24_CapturedFrameData(byte[] rawData, ULS24_TrimSettings trim)
            {
                frameIndex = ULS24_Utils.BytesToLittleEndianU16(rawData);
                byte[] binningPatternRaw = new byte[ULS24_PixelBinningPattern.BinningPatternLength];
                Array.Copy(rawData, 2, binningPatternRaw, 0, ULS24_PixelBinningPattern.BinningPatternLength);
                usedBinningPattern = new ULS24_PixelBinningPattern(binningPatternRaw);

                FrameBuffer = new UInt16[ULS24_RowsCount, ULS24_ColsCount];
                for (uint row = 0u; row < ULS24_RowsCount; row++)
                {
                    for (uint col = 0u; col < ULS24_ColsCount; col++)
                    {
                        uint pixelOffset = FrameDataOffset + (((ULS24_ColsCount * row) + col) * sizeof(short));
                        byte[] rawPixelData = new byte[sizeof(short)];
                        Array.Copy(rawData, pixelOffset, rawPixelData, 0, sizeof(short));
                        FrameBuffer[row, col] = ADCCorrection(trim, (int)col, rawPixelData[1], rawPixelData[0], 12, usedBinningPattern.gain);
                    }
                }
            }
            private UInt16 ADCCorrection(ULS24_TrimSettings trim, int ColNum, byte HighByte, byte LowByte, int pixelNum, ULS24_PixelGain gain_mode)
            {
                int hb, lb, lbc, hbi;
                int hbln, lbp, hbhn;
                bool oflow = false, uflow = false;

                int ioffset = 0;
                int result;

                int intmax = 32767;
                int intmax256 = 128;

                hb = (int)HighByte;
                hbln = hb % 16;     //
                hbhn = hb / 16;     //

                int nd = 0;
                if (pixelNum == 12) nd = ColNum;
                else nd = ColNum >> 1;

                int k, b, c, h;

                //	double shrink = 0.022;

                c = (int)trim.kbi[nd, 4];
                h = (int)trim.kbi[nd, 5];

                if (hb < 16)
                {
                    k = (int)trim.kbi[nd, 0];
                    b = (int)(trim.kbi[nd, 1]) + h / 2;         // 15 is just an empirical value, the first bump is raised higher. To do: what about reverse bump
                    c = c + h / 10;
                }
                else if (hb < 128)
                {
                    k = (int)(trim.kbi[nd, 0]);
                    b = (int)(trim.kbi[nd, 1]);                 // 
                }
                else
                {
                    k = (int)(trim.kbi[nd, 2]);
                    b = (int)(trim.kbi[nd, 3]);                 // 
                }

                ioffset = k * hb / intmax + b / intmax256;

                lb = (int)LowByte;
                lbc = lb + ioffset;

                if (hb > 128)
                {
                    hbi = 128 + (hb - 128) / 2;
                }
                else
                {
                    hbi = hb;
                }

                // Use lbc, not hbln to calculate sawtooth correction, as hbln tends to be a little jittery	
                ioffset += (lbc - 128) * c * (300 - hbi) / (12 * 300 * intmax256);      // 12/19/2016 modification, shrinking sawtooth.
                lbc = lb + ioffset;                                             // re-calc lbc, 2 pass algorithm

                if (lbc > 255) lbc = 255;
                else if (lbc < 0) lbc = 0;

                lbp = hbln * 16 + 7;

                int lbpc = lbp - ioffset;               // lpb - ioffset: low byte predicted from the high byte low nibble BEFORE correction
                int qerr = lbp - lbc;                   // if the lbc is correct, this would be the quantization error. If it is too large, maybe lb was the saturated "stuck" version

                if (lbpc > 255 + 20)
                {                   // We allow some correction error, because hbln may have randomly flipped.
                    oflow = true;
                }
                else if (lbpc > 255 && qerr > 28)
                {       // Again we allow some tolerance because hbln may have drifted, leading to fake error
                    oflow = true;
                }
                else if (lbpc > 191 && qerr > 52)
                {
                    oflow = true;
                }
                else if (qerr > 96)
                {
                    oflow = true;
                }
                else if (lbpc < -20)
                {
                    uflow = true;
                }
                else if (lbpc < 0 && qerr < -28)
                {
                    uflow = true;
                }
                else if (lbpc < 64 && qerr < -52)
                {
                    uflow = true;
                }
                else if (qerr < -96)
                {
                    uflow = true;
                }
                else
                {
                    // Nothing to do
                }

                if (oflow || uflow)
                {
                    result = hb * 16 + 7;
                }
                else
                {
                    result = hbhn * 256 + lbc;
                }

                //	if (calib2) return result;

                if (gain_mode == ULS24_PixelGain.High)
                    result += -(int)(trim.fpni[1, nd]) + DARK_LEVEL;        // high gain
                else
                    result += -(int)(trim.fpni[0, nd]) + DARK_LEVEL;        // low gain

                if (result < 0) result = 0;

                return (UInt16)result;
            }


            private const int DARK_LEVEL = 100;
            private const uint FrameDataOffset = 8;
            private const uint ULS24_RowsCount = 12;
            private const uint ULS24_ColsCount = 12;
        }

        public static class ULS24_Utils
        {
            public static byte[] LittleEndianToBytes(UInt32 value)
            {
                byte[] result = new byte[4];
                result[0] = (byte)(value & 0xFFu);
                result[1] = (byte)((value >> 8) & 0xFFu);
                result[2] = (byte)((value >> 16) & 0xFFu);
                result[3] = (byte)((value >> 24) & 0xFFu);

                return result;
            }
            public static UInt32 BytesToLittleEndianU32(byte[] bytes)
            {
                UInt32 value;

                value = (UInt32)bytes[0];
                value |= (UInt32)bytes[1] << 8;
                value |= (UInt32)bytes[2] << 16;
                value |= (UInt32)bytes[3] << 24;

                return value;
            }
            public static byte[] LittleEndianToBytes(UInt16 value)
            {
                byte[] result = new byte[2];
                result[0] = (byte)(value & 0xFFu);
                result[1] = (byte)((value >> 8) & 0xFFu);

                return result;
            }
            public static UInt16 BytesToLittleEndianU16(byte[] bytes)
            {
                UInt16 value;

                value = (UInt16)bytes[0];
                value |= (UInt16)(bytes[1] << 8);

                return value;
            }
        }

        private byte[] _UlsTrimData;
        private readonly uint UlsTrimDataPageSize = 51;
        private ULS24_TrimSettings trim;

        private List<ULS24_CapturedFrameData> capturedFrames;

        public static readonly BluetoothUuid ImageCaptureServiceUuid = BluetoothUuid.FromGuid(Guid.Parse("2028f1be-b22b-4884-b13c-a15951ffe725"));
        public static readonly BluetoothUuid ImageCaptureControlCharUuid = BluetoothUuid.FromGuid(Guid.Parse("c71e1e50-60b6-4710-aac8-af847a1f4d2f"));
        public static readonly BluetoothUuid ImageCaptureLengthCharUuid = BluetoothUuid.FromGuid(Guid.Parse("9f71e486-981a-4e3b-b9c2-ec1b4234cc88"));
        public static readonly BluetoothUuid BinningPatternCharUuid = BluetoothUuid.FromGuid(Guid.Parse("53a1612e-eeba-48f9-9c5e-48c011f15d1d"));
        public static readonly BluetoothUuid PipelineModeEnCharUuid = BluetoothUuid.FromGuid(Guid.Parse("869c5ac5-ec5f-4a1e-917b-e44322594cfc"));
        public static readonly BluetoothUuid PipelineDelayCharUuid = BluetoothUuid.FromGuid(Guid.Parse("cb6c3356-67cc-4654-bc78-73a8961be684"));
        public static readonly BluetoothUuid PixelIntegrationTimeCharUuid = BluetoothUuid.FromGuid(Guid.Parse("54925220-b53a-4303-b43d-4f4961cb0585"));
        public static readonly BluetoothUuid ImgBufferCountCharUuid = BluetoothUuid.FromGuid(Guid.Parse("9abbc54e-d399-4bf7-968d-2e3d42be9f78"));
        public static readonly BluetoothUuid GetNextImageCharUuid = BluetoothUuid.FromGuid(Guid.Parse("d037378b-e880-4842-a3ba-6e26c485dc49"));
        public static readonly BluetoothUuid ImgData1CharUuid = BluetoothUuid.FromGuid(Guid.Parse("fec13ca3-1ef7-429e-b975-d75c37c6d2a7"));
        public static readonly BluetoothUuid ImgData2CharUuid = BluetoothUuid.FromGuid(Guid.Parse("a0c7454a-b370-4188-ad9c-c8dca1e28de4"));

        public static readonly BluetoothUuid TrimSettigsServiceUuid = BluetoothUuid.FromGuid(Guid.Parse("949fb9a8-09ae-4fad-a0ae-6d0ccca25ace"));
        public static readonly BluetoothUuid TrimDataPage0CharUuid = BluetoothUuid.FromGuid(Guid.Parse("e4a8beba-2c4e-406d-a34d-6c8a1817b181"));
        public static readonly BluetoothUuid TrimDataPage1CharUuid = BluetoothUuid.FromGuid(Guid.Parse("3f2e95b8-3ea4-4d9e-9e7d-df43e008de06"));
        public static readonly BluetoothUuid TrimDataPage2CharUuid = BluetoothUuid.FromGuid(Guid.Parse("1b64148a-27c0-4bd6-9ce6-9031c267c41b"));
        public static readonly BluetoothUuid TrimDataPage3CharUuid = BluetoothUuid.FromGuid(Guid.Parse("7d25bc72-5ee2-4331-bf06-9767f155a7b1"));

        private static readonly uint pipelineDelayUsMin = 100;
        private static readonly uint pipelineDelayUsMax = 100000000;

        private static readonly uint integrationTimeUsMin = 100;
        private static readonly uint integrationTimeUsMax = 100000000;

        private BluetoothDevice _bleDevice = null;

        private GattService _imgCaptureService = null;
        private GattCharacteristic _captModeChar = null;
        private GattCharacteristic _binningPatternChar = null;
        private GattCharacteristic _pipelineDelayChar = null;
        private GattCharacteristic _imgCaptLengthChar = null;
        private GattCharacteristic _imgCaptCtrlChar = null;
        private GattCharacteristic _imgBufferCountChar = null;
        private GattCharacteristic _pixIntegrationTimeChar = null;
        private GattCharacteristic _getNextImgChar = null;
        private GattCharacteristic _imgData1Char = null;
        private GattCharacteristic _imgData2Char = null;

        private GattService _trimService = null;
        private GattCharacteristic _trimDataPage0Char = null;
        private GattCharacteristic _trimDataPage1Char = null;
        private GattCharacteristic _trimDataPage2Char = null;
        private GattCharacteristic _trimDataPage3Char = null;

        private TaskCompletionSource<UInt32> _FrameCapturePromise;

        private bool sensorConfigChanged = true;

        private uint captureLength = 0;

        private bool _usePipelineMode = false;
        public bool usePipelineMode { 
            get
            {
                return _usePipelineMode;
            }
            set
            {
                if (_usePipelineMode != value)
                {
                    _usePipelineMode = value;
                    sensorConfigChanged = true;
                }
            }
        }

        public uint _pipelineDelayUs = pipelineDelayUsMin;
        public uint pipelineDelayUs
        {
            get
            {
                return _pipelineDelayUs;
            }
            set
            {
                if (pipelineDelayUs != value)
                {
                    if (value < pipelineDelayUsMin)
                    {
                        _pipelineDelayUs = pipelineDelayUsMin;
                    }
                    else if (value > pipelineDelayUsMax)
                    {
                        _pipelineDelayUs = pipelineDelayUsMax;
                    }
                    else
                    {
                        _pipelineDelayUs = value;
                    }
                    sensorConfigChanged = true;
                }
            }
        }

        private uint _integrationTimeUs = integrationTimeUsMin;
        public uint integrationTimeUs
        {
            get
            {
                return _integrationTimeUs;
            }
            set
            {
                if (_integrationTimeUs!= value)
                {
                    if (value < integrationTimeUsMin)
                    {
                        _integrationTimeUs = integrationTimeUsMin;
                    }
                    else if (value > integrationTimeUsMax)
                    {
                        _integrationTimeUs = integrationTimeUsMax;
                    }
                    else
                    {
                        _integrationTimeUs = value;
                    }
                    sensorConfigChanged = true;
                }
            }
        }

        private bool _useLowGain = true;
        public bool useLowGain
        {
            get
            {
                return _useLowGain;
            }
            set
            {
                if (_useLowGain != value)
                {
                    _useLowGain = value;
                    sensorConfigChanged = true;
                }
            }
        }

        private bool _useLowResMode = true;
        public bool useLowResMode
        {
            get { return _useLowResMode;}
            set
            {
                if (_useLowResMode != value)
                {
                    _useLowResMode = value;
                    sensorConfigChanged = true;
                }
            }
        }

        public bool isValid
        {
            get
            {
                return _bleDevice != null;
            }
        }

        private ULSSensorImage _latestImage = null;
        public ULSSensorImage latestImage
        {
            get
            {
                return _latestImage;
            }
        }

        internal const int _inBytesPerPixel = 2;
        internal const int _outBytesPerPixel = 6;


        public async static Task<ULS24Device> buildFromBleDevice(BluetoothDevice bleDevice)
        {
            if (bleDevice == null)
            {
                return null;
            }
            else
            {
                ULS24Device newDevice = new ULS24Device(bleDevice);
                await newDevice._init();

                if (newDevice.isValid)
                {
                    return newDevice;
                }
                else
                {
                    return null;
                }
            }
        }
        private ULS24Device(BluetoothDevice bleDevice)
        {
            _bleDevice = bleDevice;
            _UlsTrimData = new byte[4 * UlsTrimDataPageSize];
        }

        private async Task _init()
        {
            try
            {
                _bleDevice.GattServerDisconnected += _onBleDeviceDisconnected;
                await _bleDevice.Gatt.ConnectAsync();
                while ((_bleDevice != null) && (_bleDevice.Gatt.IsConnected != true))
                {
                    await Task.Delay(200);
                }

                _imgCaptureService = await _bleDevice.Gatt.GetPrimaryServiceAsync(ImageCaptureServiceUuid);
                _trimService = await _bleDevice.Gatt.GetPrimaryServiceAsync(TrimSettigsServiceUuid);

                /* Readout EEPROM trim data - required only once during runtime */
                _trimDataPage0Char = await _trimService.GetCharacteristicAsync(TrimDataPage0CharUuid);
                (await _trimDataPage0Char.ReadValueAsync()).CopyTo(_UlsTrimData, 0 * UlsTrimDataPageSize);
                _trimDataPage1Char = await _trimService.GetCharacteristicAsync(TrimDataPage1CharUuid);
                (await _trimDataPage1Char.ReadValueAsync()).CopyTo(_UlsTrimData, 1 * UlsTrimDataPageSize);
                _trimDataPage2Char = await _trimService.GetCharacteristicAsync(TrimDataPage2CharUuid);
                (await _trimDataPage2Char.ReadValueAsync()).CopyTo(_UlsTrimData, 2 * UlsTrimDataPageSize);
                _trimDataPage3Char = await _trimService.GetCharacteristicAsync(TrimDataPage3CharUuid);
                (await _trimDataPage3Char.ReadValueAsync()).CopyTo(_UlsTrimData, 3 * UlsTrimDataPageSize);
                trim = new ULS24_TrimSettings(_UlsTrimData);

                _captModeChar = await _imgCaptureService.GetCharacteristicAsync(PipelineModeEnCharUuid);
                _binningPatternChar = await _imgCaptureService.GetCharacteristicAsync(BinningPatternCharUuid);
                _pipelineDelayChar = await _imgCaptureService.GetCharacteristicAsync(PipelineDelayCharUuid);
                _imgCaptLengthChar = await _imgCaptureService.GetCharacteristicAsync(ImageCaptureLengthCharUuid);
                _imgCaptCtrlChar = await _imgCaptureService.GetCharacteristicAsync(ImageCaptureControlCharUuid);
                _imgCaptCtrlChar.CharacteristicValueChanged += _onCaptureControlChanged;
                _imgBufferCountChar = await _imgCaptureService.GetCharacteristicAsync(ImgBufferCountCharUuid);
                _getNextImgChar = await _imgCaptureService.GetCharacteristicAsync(GetNextImageCharUuid);
                _imgData1Char = await _imgCaptureService.GetCharacteristicAsync(ImgData1CharUuid);
                _imgData2Char = await _imgCaptureService.GetCharacteristicAsync(ImgData2CharUuid);
                _pixIntegrationTimeChar = await _imgCaptureService.GetCharacteristicAsync(PixelIntegrationTimeCharUuid);
            }
            catch (Exception)
            {
                _bleDevice = null;
            }
        }

        private async Task _configureCapture()
        {
            byte[] rawData;

            if (sensorConfigChanged != false)
            {
                /* 1. Configure capture mode - pipeline or normal */
                rawData = new byte[1];
                rawData[0] = usePipelineMode ? (byte)0x01u : (byte)0x00u; /* Any non-zero single byte value enables Pipeline mode. Zero value forces Normal mode */
                await _captModeChar.WriteValueWithResponseAsync(rawData);

                /* 2. Set pipeline delay if pipeline mode is used */
                if (usePipelineMode != false)
                {
                    rawData = ULS24_Utils.LittleEndianToBytes(pipelineDelayUs); /* Convert data to Little Endian format */
                    await _pipelineDelayChar.WriteValueWithResponseAsync(rawData);
                }

                /* 3. Set pixel integration time */
                rawData = ULS24_Utils.LittleEndianToBytes(integrationTimeUs);
                await _pixIntegrationTimeChar.WriteValueWithResponseAsync(rawData);

                /* New config is applied now */
                sensorConfigChanged = false;
            }

            /* 4. Set pixel binning pattern */
            if (useLowResMode != false)
            {
                /* Normal 12x12 capture */
                rawData = new byte[1];
                rawData[0] = MakeBinningPatternByte(true, false, false, false, useLowGain); /* Enable subpixel 1 only */
            }
            else
            {
                /* 24x24 mode - we need to cycle through all subpixels sequentially */
                rawData = new byte[4];
                rawData[0] = MakeBinningPatternByte(true, false, false, false, useLowGain); /* Enable subpixel 1 only */
                rawData[1] = MakeBinningPatternByte(false, true, false, false, useLowGain); /* Enable subpixel 2 only */
                rawData[2] = MakeBinningPatternByte(false, false, true, false, useLowGain); /* Enable subpixel 4 only */
                rawData[3] = MakeBinningPatternByte(false, false, false, true, useLowGain); /* Enable subpixel 8 only */
            }
            await _binningPatternChar.WriteValueWithResponseAsync(rawData);

            /* 5. Configure capture length */
            rawData = ULS24_Utils.LittleEndianToBytes(captureLength);
            await _imgCaptLengthChar.WriteValueWithResponseAsync(rawData);
        }

        private void _onCaptureControlChanged(object sender, GattCharacteristicValueChangedEventArgs e)
        {
            UInt32 scanExitCode = ULS24Device.ULS24_Utils.BytesToLittleEndianU32(e.Value);
            _FrameCapturePromise.TrySetResult(scanExitCode);
        }
        private void _onBleDeviceDisconnected(object sender, EventArgs e)
        {
            _trimDataPage0Char = null;
            _trimDataPage1Char = null;
            _trimDataPage2Char = null;
            _trimDataPage3Char = null;
            _trimService = null;

            _captModeChar = null;
            _binningPatternChar = null;
            _imgCaptLengthChar = null;
            _imgCaptCtrlChar = null;
            _imgBufferCountChar = null;
            _getNextImgChar = null;
            _imgData1Char = null;
            _imgData2Char = null;
            _imgCaptureService = null;

            _bleDevice = null;
        }

        public static BluetoothLEScanFilter getBleScanFilter()
        {
            BluetoothLEScanFilter bleScanFilter = new BluetoothLEScanFilter();
            bleScanFilter.NamePrefix = "ULS24";
            bleScanFilter.Services.Add(ImageCaptureServiceUuid);

            return bleScanFilter;
        }

        public bool IsConnected 
        { 
            get
            {
                if ((_bleDevice != null) && (_bleDevice.Gatt != null))
                {
                    return _bleDevice.Gatt.IsConnected;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Disconnect()
        {
            try
            {
                if ((_bleDevice != null) && (_bleDevice.Gatt != null))
                {
                    _bleDevice.Gatt.Disconnect();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _bleDevice = null;
            }
        }

        public async Task<Bitmap> CaptureSingleFrame()
        {
            byte[] rawData;

            /* 1. Configure capture length depending on image resolution */
            if (useLowResMode)
            {
                captureLength = 1;
            }
            else
            {
                captureLength = 4;
            }
            await _configureCapture();

            /* 2. Start capture */
            _FrameCapturePromise = new TaskCompletionSource<UInt32>();
            await _imgCaptCtrlChar.StartNotificationsAsync(); /* Enable BLE Notifications to monitor Capture Completed event */
            rawData = new byte[1];
            rawData[0] = 0x01; /* Writing any value to this characteristic will trigger the capture */
            await _imgCaptCtrlChar.WriteValueWithResponseAsync(rawData);

            /* 3. Await scan completion */
            UInt32 exitCode = await _FrameCapturePromise.Task;
            _FrameCapturePromise = null;
            await _imgCaptCtrlChar.StopNotificationsAsync();
            if (exitCode != 0)
            {
                throw new Exception("Frame capture failed");
            }

            /* Get the number of the images in the buffer */
            GattCharacteristic imgBufferCountChar = await _imgCaptureService.GetCharacteristicAsync(ImgBufferCountCharUuid);
            UInt32 capturedImagesNum = ULS24_Utils.BytesToLittleEndianU32(await imgBufferCountChar.ReadValueAsync());

            /* Prepare readout */
            capturedFrames = new List<ULS24_CapturedFrameData>((int)capturedImagesNum);

            /* Read each image */
            for (UInt32 i = 0; i < capturedImagesNum; i++)
            {
                /* Request a frame from the queue */
                rawData = new byte[1];
                rawData[0] = 0x01; /* Writing any value to this characteristic will push captured frame into Data1 and Data2 chars */
                await _getNextImgChar.WriteValueWithResponseAsync(rawData);

                /* Read first half */
                byte[] imgData1 = await _imgData1Char.ReadValueAsync();

                /* Read second half */
                byte[] imgData2 = await _imgData2Char.ReadValueAsync();

                /* Combine raw data and put into buffer */
                byte[] fullData = new byte[imgData1.Length + imgData2.Length];
                Array.Copy(imgData1, fullData, imgData1.Length);
                Array.Copy(imgData2, 0, fullData, imgData1.Length, imgData2.Length);
                ULS24_CapturedFrameData frame = new ULS24_CapturedFrameData(fullData, trim);
                capturedFrames.Add(frame);
            }

            /* Build bitmap from readout */
            Bitmap bmp = null;
            if (useLowResMode)
            {
                /* This is normal 12x12 image */
                if ((capturedFrames != null) && (capturedFrames.Count >= 1))
                {
                    _latestImage = ULSSensorImage.Build12x12Image(capturedFrames[0]);
                    bmp = _latestImage.bmp;
                }
            }
            else
            {
                /* This should be a 24x24 image */
                if ((capturedFrames != null) && (capturedFrames.Count >= 4))
                {
                    _latestImage = ULSSensorImage.Build24x24Image(capturedFrames);
                    bmp = _latestImage.bmp;
                }
            }

            return bmp;
        }

        public void addDeviceDisconnectedHandler(EventHandler handler)
        {
            if (_bleDevice != null)
            {
                _bleDevice.GattServerDisconnected += handler;
            }
        }

        private byte MakeBinningPatternByte(bool Subpix1_Enabled, bool Subpix2_Enabled, bool Subpix4_Enabled, bool Subpix8_Enabled, bool LowGain = true)
        {
            byte pattern = 0x00;

            if (Subpix8_Enabled != false)
            {
                pattern |= (byte)(1 << 0);
            }

            if (Subpix4_Enabled != false)
            {
                pattern |= (byte)(1 << 1);
            }

            if (Subpix2_Enabled != false)
            {
                pattern |= (byte)(1 << 2);
            }

            if (Subpix1_Enabled != false)
            {
                pattern |= (byte)(1 << 3);
            }

            if (LowGain != false)
            {
                pattern |= (byte)(1 << 4);
            }

            return pattern;
        }

        
    }
}
