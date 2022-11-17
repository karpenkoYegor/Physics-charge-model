using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace Diplom
{
    class PowerLines
    {

        public static BitmapImage image;
        public System.Drawing.Bitmap btm;
        List<double[,]> fi;
        private RaschetFI RaschetFI;
        int width, height;
        public PowerLines(int width, int height)
        {
            this.width = width;
            this.height = height;
            RaschetFI = new RaschetFI();
            fi = new List<double[,]>();
        }

        public PowerLines()
        {

        }

        public void Element(List<double[,]> fi_l)
        {
            
            fi = fi_l;
            CreateImage();
        }

        void CreateImage()
        {
            btm = new Bitmap(width, height);
            double max; double x; double y; double fi = 0; double x0, y0, amax = 0;
            for (int i = 0; i < RaschetFI.ind; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    x = RaschetFI.coords[i][0] + Math.Cos(0.314 * j);
                    y = RaschetFI.coords[i][1] + Math.Sin(j * 0.314);
                    for (int k = 0; k < 700; k++)
                    {
                        fi = 0;
                        for (int b = 0; b < RaschetFI.ind; b++)
                        {
                            var range = Math.Sqrt((x - RaschetFI.coords[b][0]) * (x - RaschetFI.coords[b][0]) + (y - RaschetFI.coords[b][1]) * (y - RaschetFI.coords[b][1]));
                            fi += RaschetFI.charge[b] / range;
                        }
                        x0 = x;
                        y0 = y;
                        max = 0;
                        for (int b = 0; b < 120; b++)
                        {
                            var a = 2 * Math.PI * b / 90;
                            x = x0 + Math.Cos(a);
                            y = y0 + Math.Sin(a);
                            var fi1 = 0d;
                            for (int c = 0; c < RaschetFI.ind; c++)
                            {
                                var range = Math.Sqrt((x - RaschetFI.coords[c][0]) * (x - RaschetFI.coords[c][0]) + (y - RaschetFI.coords[c][1]) * (y - RaschetFI.coords[c][1]));
                                fi1 += RaschetFI.charge[c] / range;
                            }
                            if (fi1 - fi > max)
                            {
                                max = fi1 - fi;
                                amax = a;
                            }
                        }
                        x = x0 + Math.Cos(amax);
                        y = y0 + Math.Sin(amax);
                        if ((int)Math.Ceiling(x) > 0 && (int)Math.Ceiling(x) < 1080 && (int)Math.Ceiling(y) > 0 && (int)Math.Ceiling(y) < 684)
                        {
                            btm.SetPixel((int)Math.Ceiling(x), (int)Math.Ceiling(y), System.Drawing.Color.Red);
                        }
                    }
                }
            }
                
            image = BitmapToImageSource(btm);
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
    }
}
