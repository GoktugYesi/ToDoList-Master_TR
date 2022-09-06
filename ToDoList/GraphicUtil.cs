using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    class GraphicUtil
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(
        IntPtr hdc, // handle to DC
        int nIndex // index of capability
        );
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        const int HORZRES = 8;
        const int VERTRES = 10;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;

        /// <summary>
        /// Ekran çözünürlüğünün mevcut fiziksel boyutunu alın
        /// </summary>
        public static Size WorkingArea
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                Size size = new Size();
                size.Width = GetDeviceCaps(hdc, HORZRES);
                size.Height = GetDeviceCaps(hdc, VERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }
        /// <summary>
        /// Mevcut sistem DPI_X boyutu genellikle 96'dır.
        /// </summary>
        public static int DpiX
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int DpiX = GetDeviceCaps(hdc, LOGPIXELSX);
                ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }
        /// <summary>
        /// Mevcut sistem DPI_Y boyutu genellikle 96'dır.
        /// </summary>
        public static int DpiY
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int DpiX = GetDeviceCaps(hdc, LOGPIXELSY);
                ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }
        /// <summary>
        /// Gerçek ayarın masaüstü çözünürlük boyutunu alın
        /// </summary>
        public static Size DESKTOP
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                Size size = new Size();
                size.Width = GetDeviceCaps(hdc, DESKTOPHORZRES);
                size.Height = GetDeviceCaps(hdc, DESKTOPVERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        /// <summary>
        /// Genişlik ölçekleme yüzdesini alın
        /// </summary>
        public static float ScaleX
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                int t = GetDeviceCaps(hdc, DESKTOPHORZRES);
                int d = GetDeviceCaps(hdc, HORZRES);
                float ScaleX = (float)GetDeviceCaps(hdc, DESKTOPHORZRES) / (float)GetDeviceCaps(hdc, HORZRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return ScaleX;
            }
        }
        /// <summary>
        /// Yükseklik yakınlaştırma yüzdesini alın
        /// </summary>
        public static float ScaleY
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                float ScaleY = (float)(float)GetDeviceCaps(hdc, DESKTOPVERTRES) / (float)GetDeviceCaps(hdc, VERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return ScaleY;
            }
        }

        public static void NewImage(string sourceImagePath, string attachImagePath, string newImagePath, int width, int height,string t1,string t2,string t3,string t4)
        {
            System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(sourceImagePath);
            System.Drawing.Image attachImage = System.Drawing.Image.FromFile(attachImagePath);

            //Yeni oluşturulan görüntünün genişliği ve yüksekliği, masaüstü çözünürlüğünün genişlik ve yüksekliğine ayarlanır
            int towidth = width;
            int toheight = height;

            //Orijinal masaüstü arka plan görüntüsünün genişliği ve yüksekliği
            int ow = sourceImage.Width;
            int oh = sourceImage.Height;

            //Orijinal masaüstü resmi yüksek çözünürlüğe sahiptir ve değiştirilmeden yeni bir resim oluşturulabilir.
            if ( towidth > ow || toheight> oh)
            {

            }

            //Yeni bir bmp görüntüsü oluşturun
            System.Drawing.Image bm = new System.Drawing.Bitmap(width, height);
            //Yeni bir çalışma yüzeyi oluşturun
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm);
            //Yüksek Kaliteli İnterpolasyon Ayarla
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //Yüksek kaliteli, düşük hızlı işleme düzgünlüğü ayarlayın
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //Tuvali boşaltın ve şeffaf bir arka plan rengiyle doldurun
            g.Clear(System.Drawing.Color.White);
            //Orijinal görüntünün belirtilen kısmını belirtilen konumda ve belirtilen boyutta çizin
            g.DrawImage(sourceImage, new System.Drawing.Rectangle((width - towidth) / 2, (height - toheight) / 2, towidth, toheight),
                0, 0, ow, oh,
                System.Drawing.GraphicsUnit.Pixel);

            //g.DrawImage(attachImage, new System.Drawing.Rectangle((width - ow) / 2, (height - oh) / 2, towidth, toheight),
            //    0, 0, attachImage.Width, attachImage.Height,
            //    System.Drawing.GraphicsUnit.Pixel);

            g.DrawImage(attachImage, new System.Drawing.Rectangle((width - attachImage.Width) , (height - attachImage.Height)/2, attachImage.Width, attachImage.Height),
            0, 0, attachImage.Width, attachImage.Height,
            System.Drawing.GraphicsUnit.Pixel);

            using (Font f = new Font("Microsoft Yahei", 30))
            {
                using (Brush b = new SolidBrush(Color.Green))
                {
                    int x = 720, y = 250, h = 1368, w = 768;
                    g.DrawString(t1, f, b, new Rectangle(x + 30, y + 10, w, h));
                    g.DrawString(t2, f, b, new Rectangle(x + 990, y + 10, w, h));
                    g.DrawString(t3, f, b, new Rectangle(x + 30, y + 550, w, h));
                    g.DrawString(t4, f, b, new Rectangle(x + 990, y + 550, w, h));
                }
            }


            try
            {
                //Küçük resimleri jpg formatında kaydedin
                bm.Save(newImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                sourceImage.Dispose();
                attachImage.Dispose();
                bm.Dispose();
                g.Dispose();
            }
        }
    }
}
