using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class PNGCleaner
{
    // [RuntimeInitializeOnLoadMethod]
    // public static void createPNGObject()
    // {

    //     Bitmap bmp = new Bitmap(Application.dataPath + "/src/URP/128-Lov-Lan.png");

    //     Bitmap newBMP = scrubBadPixels(bmp);
    //     // newBMP = scrubBadPixels(bmp);

    //     newBMP.Save(Application.dataPath + "/src/URP/Clean-128-Lov-Lan.png", ImageFormat.Png);
    //     // newBMP.Save(Application.dataPath + "/src/URP/6-Clean-128-Lov-Lan.bmp", ImageFormat.Bmp);
    //     // newBMP.Save(Application.dataPath + "/src/URP/7-Clean-128-Lov-Lan.jpeg", ImageFormat.Jpeg);

    //     // Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
    //     // BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

    //     // // Get the address of the first line.
    //     // IntPtr ptr = bmpData.Scan0;

    //     // // Declare an array to hold the bytes of the bitmap.
    //     // int bytes = bmpData.Stride * bmp.Height;
    //     // byte[] rgbValues = new byte[bytes];
    //     // byte[] r = new byte[bytes / 3];
    //     // byte[] g = new byte[bytes / 3];
    //     // byte[] b = new byte[bytes / 3];

    //     // // Copy the RGB values into the array.
    //     // Marshal.Copy(ptr, rgbValues, 0, bytes);

    //     // int count = 0;
    //     // int stride = bmpData.Stride;

    //     // for (int column = 0; column < bmpData.Height; column++)
    //     // {
    //     //     for (int row = 0; row < bmpData.Width; row++)
    //     //     {
    //     //         b[count] = (byte)(rgbValues[(column * stride) + (row * 3)]);
    //     //         g[count] = (byte)(rgbValues[(column * stride) + (row * 3) + 1]);
    //     //         r[count++] = (byte)(rgbValues[(column * stride) + (row * 3) + 2]);
    //     //     }
    //     // }
        
    //     // for(int index = 0; index < r.Length; index++)
    //     // {
    //     //     Debug.LogError("RGB value of pixel at index #" + index + " = ("+r[index]+","+g[index]+","+b[index]+")");
    //     // }

    // }

    // private static Bitmap scrubBadPixels(Bitmap bmp)
    // {
    //     Bitmap newBMP = new Bitmap(bmp.Width,bmp.Height);

    //     int alphaThreshold = 125;

    //     // System.Drawing.Color[,] pixelColors = new System.Drawing.Color[bmp.Width,bmp.Height];

    //     for(int row = 0; row < bmp.Width; row++)
    //     {
    //         for(int col = 0; col < bmp.Height; col++)
    //         {
    //             System.Drawing.Color currentPixel = bmp.GetPixel(row, col);

    //             if(currentPixel.A > alphaThreshold)
    //             {
    //                 newBMP.SetPixel(row, col, currentPixel);
    //             } else
    //             {
    //                 // Debug.LogError("RGBA value of pixel at ("+row+","+col + ") = ("+newPixel.R+","+newPixel.G+","+newPixel.B+ "," + newPixel.A +")");
    //                 newBMP.SetPixel(row, col, new System.Drawing.Color());
    //             }
    //         }
    //     }

    //     return newBMP;
    // }

}
