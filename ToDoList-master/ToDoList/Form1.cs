using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ToDoList
{
	public partial class Form1 : System.Windows.Forms.Form
    {
        private StringBuilder wallPaperPath = new StringBuilder(200);

        /// <summary>
        /// İşletim sistemi kapandığında uygulamayı kapatınca
        /// </summary>
        /// <param name="m">Sistem mesajlarını durdur</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0011://WM_QUERYENDSESSION
                    m.Result = (IntPtr)1;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        public Form1()
		{
			InitializeComponent();
            //textBox1.Text = File.ReadAllText(@"D:\\dev_vs\\ToDoList\\1.txt", Encoding.UTF8);
            textBox1.Text = File.ReadAllText(@"1.txt", Encoding.UTF8);
            textBox2.Text = File.ReadAllText(@"2.txt", Encoding.UTF8);
			textBox3.Text = File.ReadAllText(@"3.txt", Encoding.UTF8);
			textBox4.Text = File.ReadAllText(@"4.txt", Encoding.UTF8);



            const int SPI_GETDESKWALLPAPER = 0x0073;
            //StringBuilder wallPaperPath = new StringBuilder(200);
            if (!SystemParametersInfo(SPI_GETDESKWALLPAPER, 200, wallPaperPath, 0))
            {
                System.Windows.Forms.MessageBox.Show("Masaüstü arka plan resmi alınamıyor, lütfen tekrar deneyin!");
            }

            //Program başladığında, masaüstü arka planını yükleyin
            string currentImg = System.Environment.CurrentDirectory + "\\new.png";
            SystemParametersInfo(20, 0, currentImg, 0x2);
        }

		public void WriteToFile()
		{
            //Dosya yoksa oluşturun, varsa üzerine yazın.
            //System.IO.File.WriteAllText(@"D:\\dev_vs\\ToDoList\\1.txt", textBox1.Text, Encoding.UTF8);
            System.IO.File.WriteAllText(@"1.txt", textBox1.Text, Encoding.UTF8);
            System.IO.File.WriteAllText(@"2.txt", textBox2.Text, Encoding.UTF8);
			System.IO.File.WriteAllText(@"3.txt", textBox3.Text, Encoding.UTF8);
			System.IO.File.WriteAllText(@"4.txt", textBox4.Text, Encoding.UTF8);
		}

		[DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
		public static extern int SystemParametersInfo(
		int uAction,
		int uParam,
		string lpvParam,
		int fuWinIni
		);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SystemParametersInfo(uint uAction, uint uParam, StringBuilder lpvParam, uint init);

        private void BtnSave_Click(object sender, EventArgs e)
        {
            WriteToFile();

            //Orijinal arka plan görüntüsünün yolunu alın
            //System.Drawing.Image imgSrc = System.Drawing.Image.FromFile("D:\\dev_vs\\ToDoList\\template1.jpg");
            //SystemParametersInfo(20, 0, "D:\\dev_vs\\ToDoList\\new.jpg", 0x2);

            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile("template1.jpg");
            //System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(wallPaperPath.ToString());

            using (Graphics g = Graphics.FromImage(imgSrc))
			{
				g.DrawImage(imgSrc, 0, 0, imgSrc.Width, imgSrc.Height);
				using (Font f = new Font("Microsoft Yahei", 30))
				{
					using (Brush b = new SolidBrush(Color.Black))
					{
						int x = 720, y = 250, h = 1368, w = 768;
						g.DrawString(textBox1.Text, f, b, new Rectangle(x + -25, y + 20, w, h));
						g.DrawString(textBox2.Text, f, b, new Rectangle(x + 930, y + 20, w, h));
						g.DrawString(textBox3.Text, f, b, new Rectangle(x + -25, y + 560, w, h));
						g.DrawString(textBox4.Text, f, b, new Rectangle(x + 930, y + 560, w, h));
					}
				}
			}

            //imgSrc.Save("D:\\dev_vs\\ToDoList\\new.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            imgSrc.Save("new.png", System.Drawing.Imaging.ImageFormat.Png);

            string currentImg = System.Environment.CurrentDirectory + "\\new.png";
            SystemParametersInfo(20, 0, currentImg, 0x2);

        }

        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            ////Form görüntüsünü geri yükle    
            //WindowState = FormWindowState.Normal;
            ////formu etkinleştir ve ona odaklan
            //this.Activate();
            ////Görev çubuğu alanındaki simgeleri göster
            //this.ShowInTaskbar = true;
            ////Tepsi alanı simgeleri gizlenir
            //notifyIcon1.Visible = false;

            if (e.Button == MouseButtons.Left)//Fare düğmelerini belirleyin
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.Hide();
                }
                else if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {

                if (MessageBox.Show("Programı kapatmak mı istiyorsun", "İpucu:", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    DialogResult = DialogResult.No;
                    Dispose();
                    Close();
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;//Formun kapanış etkinliğini iptal edin
            this.WindowState = FormWindowState.Minimized;//mevcut formu küçült
            notifyIcon1.Visible = true;//En kayan simgeyi görünür yap
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //Küçült düğmesinin seçili olup olmadığını belirleyin
            if (WindowState == FormWindowState.Minimized)
            {
                //Görev çubuğu alanı simgesini gizle
                this.ShowInTaskbar = false;
                //Simgeler tepsi alanında görüntülenir
                notifyIcon1.Visible = true;
            }
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //Program çıktığında, masaüstü arka planını geri yükleyin
            SystemParametersInfo(20, 0, wallPaperPath.ToString(), 0x2);
            Dispose();
            Close();
        }

        private void BtnSuper_Click(object sender, EventArgs e)
        {
            WriteToFile();
            if (wallPaperPath==null || wallPaperPath.ToString().Length <= 0)
            {
                MessageBox.Show("Arka plan düz bir renk, lütfen önce bir arka plan resmi seçin！");
                return;
            }
            //Orijinal masaüstü arka planının resmini alın, resmin boyutunu ve çözünürlüğün uzunluk ve genişliğini karşılaştırın
            //Aynıysa, doğrudan görüntüye dayalı olarak yeni bir masaüstü arka planı oluşturulacaktır; Aksi takdirde, önce çözünürlük
            //boyutuna sahip bir arka plan görüntüsü oluşturulacak ve ardından metin yazılacaktır.

            //Ekran çözünürlüğünü tam ekranda alın


            //int width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            //int height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            Size size = GraphicUtil.DESKTOP;
            //MessageBox.Show("Çözünürlük："+size.Width +"*"+ size.Height);

            string newImg = System.Environment.CurrentDirectory + "\\newPic.jpg";
            string attachImg = System.Environment.CurrentDirectory + "\\attachPic.jpg";
            GraphicUtil.NewImage(wallPaperPath.ToString(), attachImg, newImg,size.Width,size.Height,textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);

            SystemParametersInfo(20, 0, newImg, 0x2);
            //Görev çubuğu (çalışma alanı) olmadan ekranın genişliği ve yüksekliği:
            //SystemParameters.WorkArea.Width
            //SystemParameters.WorkArea.Height
        }
    }
}
