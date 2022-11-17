using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Diplom
{
    class Potentional
    {
        public System.Drawing.Bitmap btm;
        List<double[,]> fi;
        public static BitmapImage image;

        public Potentional(int width, int height)
        {
            btm = new Bitmap(width, height);
            fi = new List<double[,]>();
        }
        
        public void Element(List<double[,]> fi_l)
        {
            fi = fi_l;
            CreateImage();
        }

        public void CreateImage()
        {
            for (int i = 0; i < btm.Width; i++)
            {
                for (int j = 0; j < btm.Height; j++)
                {
                    double charge = Sum(i, j);
                    if ((Math.Ceiling(charge) - charge) < 0.5)
                    {
                        btm.SetPixel(i, j, Color.Green);
                    }
                    else
                    {
                        btm.SetPixel(i, j, Color.White);
                    }
                }
            }
            image = BitmapToImageSource(btm);
        }

        double Sum(int x, int y)
        {
            double f = 0;
            for (int i = 0; i < RaschetFI.ind; i++)
            {
                f += fi[i][x, y];
            }
            return f;
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
