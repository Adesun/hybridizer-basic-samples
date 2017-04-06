﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Sobel
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap baseImage = (Bitmap)Image.FromFile("lena512.bmp");
            
            int height = baseImage.Height, width = baseImage.Width;
            
            Bitmap resImage = new Bitmap(width, height);

            byte[] inputPixels = new byte[width * height];
            byte[] outputPixels = new byte[width * height];

            ReadImage(inputPixels, baseImage, width, height);

            ComputeSobel(outputPixels, inputPixels, width, height);

            SaveImage("lena-sobel.bmp", outputPixels, width, height);
        }

        public static void ReadImage(byte[] inputPixel, Bitmap image, int width, int height)
        {
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    inputPixel[i * height + j] = image.GetPixel(i, j).B;
                }
            }
        }
        
        public static void ComputeSobel(byte[] outputPixel, byte[] inputPixel, int width, int height)
        {
            Parallel.For(0, width * height, (pixelId) => {
                int i = pixelId / height;
                int j = pixelId - i * height;

                byte output = 0;
                if (i != 0 && j != 0 && i != height - 1 && j != width - 1)
                {
                    byte topl = inputPixel[pixelId - width - 1];
                    byte top = inputPixel[pixelId - width];
                    byte topr = inputPixel[pixelId - width + 1];
                    byte l = inputPixel[pixelId - 1];
                    byte r = inputPixel[pixelId + 1];
                    byte botl = inputPixel[pixelId + width - 1];
                    byte bot = inputPixel[pixelId + width];
                    byte botr = inputPixel[pixelId + width + 1];


                    // todo
                    output = (byte)(Math.Abs((int)(topl + 2 *l + botl - topr - 2*r - botr)) +

                                    Math.Abs((int)(topl + 2* top + topr - botl - 2*bot - botr)));
                    if (output > 255)
                    {
                        output = 255;
                    }
                    outputPixel[pixelId] = output;
                }
            });
        }
        
        public static void SaveImage(string nameImage, byte[] outputPixel, int width, int height)
        {
            Bitmap resImage = new Bitmap(width, height);
            int col = 0;
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    col = outputPixel[i * height + j];
                    resImage.SetPixel(i, j, Color.FromArgb(col, col, col));
                }
            }

            //store the result image.
            resImage.Save(nameImage, System.Drawing.Imaging.ImageFormat.Png);
        }
        
    }
}
