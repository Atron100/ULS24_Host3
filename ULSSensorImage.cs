using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using Windows.Data.Json;
using static ULS24_Host.ULS24Device;

namespace ULS24_Host
{
    internal class ULSSensorImage
    {
        private List<ULS24_CapturedFrameData> capturedFrames;
        private Guid _guid = Guid.NewGuid();
        public Guid guid { get { return _guid; } }

        public Bitmap bmp = null;

        int linecount = 0;

        StreamWriter filecsv;

        public static ULSSensorImage Build12x12Image(ULS24_CapturedFrameData inBuffer)
        {
            ULSSensorImage imgStorage = new ULSSensorImage();
            imgStorage.capturedFrames.Add(inBuffer);

            const int width = 12;
            const int height = 12;
            var rgbData = _Convert16BitGrayScaleToRgb48(inBuffer, width, height);
            imgStorage.bmp = _CreateBitmapFromBytes(rgbData, width, height);

            return imgStorage;
        }
        public static ULSSensorImage Build24x24Image(List<ULS24_CapturedFrameData> inFrames)
        {
            ULSSensorImage imgStorage = new ULSSensorImage();

            for (int i = 0; i < inFrames.Count; i++)
            {
                imgStorage.capturedFrames.Add(inFrames[i]);
            }

            const int singleWidth = 12;
            const int singleHeight = 12;
            const int fullWidth = 24;
            const int fullHeight = 24;

            var rgbDataSub1 = _Convert16BitGrayScaleToRgb48(imgStorage.capturedFrames[0], singleWidth, singleHeight);
            var rgbDataSub2 = _Convert16BitGrayScaleToRgb48(imgStorage.capturedFrames[1], singleWidth, singleHeight);
            var rgbDataSub4 = _Convert16BitGrayScaleToRgb48(imgStorage.capturedFrames[2], singleWidth, singleHeight);
            var rgbDataSub8 = _Convert16BitGrayScaleToRgb48(imgStorage.capturedFrames[3], singleWidth, singleHeight);

            /* Construct single 24x24 image */
            byte[] rgbData = new byte[fullWidth * fullHeight * _outBytesPerPixel];
            const int inStride = singleWidth * _outBytesPerPixel;
            const int outStride = fullWidth * _outBytesPerPixel;

            // Step through the image by row
            for (int y = 0; y < singleHeight; y++)
            {
                // Step through the image by column
                for (int x = 0; x < singleWidth; x++)
                {
                    // Get inbuffer index and outbuffer index
                    int inIndex = (y * inStride) + (x * _outBytesPerPixel);
                    int outIndexSub1 = (2 * y * outStride) + ((2 * x + 1) * _outBytesPerPixel);
                    int outIndexSub2 = (2 * y * outStride) + (2 * x * _outBytesPerPixel);
                    int outIndexSub4 = ((2 * y + 1) * outStride) + ((2 * x + 1) * _outBytesPerPixel);
                    int outIndexSub8 = ((2 * y + 1) * outStride) + (2 * x * _outBytesPerPixel);

                    Buffer.BlockCopy(rgbDataSub1, inIndex, rgbData, outIndexSub1, _outBytesPerPixel);
                    Buffer.BlockCopy(rgbDataSub2, inIndex, rgbData, outIndexSub2, _outBytesPerPixel);
                    Buffer.BlockCopy(rgbDataSub4, inIndex, rgbData, outIndexSub4, _outBytesPerPixel);
                    Buffer.BlockCopy(rgbDataSub8, inIndex, rgbData, outIndexSub8, _outBytesPerPixel);
                }
            }

            imgStorage.bmp = _CreateBitmapFromBytes(rgbData, fullWidth, fullHeight);

            return imgStorage;
        }

        private ULSSensorImage()
        {
            capturedFrames = new List<ULS24_CapturedFrameData>();
        }

        public void StoreBMP(String folderPath, String fileName)
        {
            if (this.bmp != null)
            {
                bmp.Save(folderPath + "\\" + fileName + ".bmp", ImageFormat.Bmp);
            }
        }

        public void StoreRAW(String folderPath, String fileName, string datet, string timet, bool modeflag)
        {
            if ((this.capturedFrames != null) && (this.capturedFrames.Count > 0))
            {
                string outputFileName = folderPath + "\\" + fileName + ".json";

                string csvfile = folderPath + "\\" + "sens_" + datet + "_" + timet + ".csv";

                var writerOptions = new JsonWriterOptions
                {
                    Indented = true
                };

                var documentOptions = new JsonDocumentOptions
                {
                    CommentHandling = JsonCommentHandling.Skip
                };

                FileStream fs = File.Create(outputFileName);
                var writer = new Utf8JsonWriter(fs, options: writerOptions);


                if (!modeflag)
                {
                    filecsv = new StreamWriter(csvfile, modeflag);
                    filecsv.Write("SENSOR ULS24 BUFFER DATA, DATE:" + datet + " TIME:" + timet + "\n");
                }
                else
                {
                    filecsv = new StreamWriter(csvfile, modeflag);
                    filecsv.Write("SENSOR ULS24 BUFFER DATA, DATE:" + DateTime.Now.ToString("ddMMyyyy") + " TIME:" + DateTime.Now.ToString("HHmmss") + "\n");

                }



                writer.WriteStartObject();

                writer.WriteStartArray("captured_frames");
                for (int i = 0; i < this.capturedFrames.Count; i++)
                {
                    writer.WriteStartObject();
                    ULS24_PixelBinningPattern binning = this.capturedFrames[i].usedBinningPattern;
                    writer.WriteNumber("frame_index", this.capturedFrames[i].frameIndex);
                    writer.WriteBoolean("low_gain", binning.gain == ULS24_PixelGain.Low);
                    writer.WriteBoolean("subpixel1_active", binning.Subpixel1_Active);
                    writer.WriteBoolean("subpixel2_active", binning.Subpixel2_Active);
                    writer.WriteBoolean("subpixel4_active", binning.Subpixel4_Active);
                    writer.WriteBoolean("subpixel8_active", binning.Subpixel8_Active);
                    
                    writer.WriteStartArray("raw_pixels");
                    
                    foreach (var rawPix in capturedFrames[i].FrameBuffer)
                    {
                        writer.WriteNumberValue(rawPix);

                        filecsv.Write(rawPix +",");
                        if ((linecount + 1) % 12 == 0)
                        {
                            filecsv.WriteLine();
                            linecount = -1;
                        }
                        linecount++;
                    }
                    filecsv.Close();
                    writer.WriteEndArray();
                    
                    writer.WriteEndObject();
                }

                writer.WriteEndArray();

                writer.WriteEndObject();

                writer.Flush();
                writer.Dispose();
                fs.Close();
            }
        }

        private static byte[] _Convert16BitGrayScaleToRgb48(ULS24_CapturedFrameData inBuffer, int width, int height)
        {
            byte[] outBuffer = new byte[width * height * ULS24Device._outBytesPerPixel];
            int outStride = width * ULS24Device._outBytesPerPixel;

            // Step through the image by row
            for (int y = 0; y < height; y++)
            {
                // Step through the image by column
                for (int x = 0; x < width; x++)
                {
                    // Get inbuffer index and outbuffer index
                    int outIndex = (y * outStride) + (x * ULS24Device._outBytesPerPixel);

                    UInt16 pixel = inBuffer.FrameBuffer[y, x];

                    pixel = (UInt16)((UInt32)pixel * 3 / 2); // Convert 12 bit to 16 bit

                    byte hibyte = (byte)(pixel >> 8);
                    byte lobyte = (byte)(pixel);

                    //R
                    outBuffer[outIndex] = lobyte;
                    outBuffer[outIndex + 1] = hibyte;

                    //G
                    outBuffer[outIndex + 2] = lobyte;
                    outBuffer[outIndex + 3] = hibyte;

                    //B
                    outBuffer[outIndex + 4] = lobyte;
                    outBuffer[outIndex + 5] = hibyte;
                }
            }
            return outBuffer;
        }

        private static Bitmap _CreateBitmapFromBytes(byte[] pixelValues, int width, int height)
        {
            //Create an image that will hold the image data
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format48bppRgb);

            //Get a reference to the images pixel data
            Rectangle dimension = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData picData = bmp.LockBits(dimension, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr pixelStartAddress = picData.Scan0;

            //Copy the pixel data into the bitmap structure
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, pixelStartAddress, pixelValues.Length);

            bmp.UnlockBits(picData);
            return bmp;
        }
    }
}
