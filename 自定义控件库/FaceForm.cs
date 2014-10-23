using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace 自定义控件库
{
    public class FaceForm : Form
    {
        #region Protected Member Variables

        protected Bitmap _Bitmap;   // 背景画板，用于绘表情和间隔线

        protected int _imageWidth;
        protected int _imageHeight;

        protected int _nBitmapWidth;
        protected int _nBitmapHeight;
        protected int _nItemWidth;
        protected int _nItemHeight;
        protected int _nRows;
        protected int _nColumns;
        protected int _nHSpace;
        protected int _nVSpace;
        protected int _nCoordX = -1;
        protected int _nCoordY = -1;
        
        private int PageIndex;
        private int PageCount;
        private int PageImageCount;
        private int imageIndex;

        #endregion

        #region Public Properties

        public Color BackgroundColor = Color.FromArgb(255, 255, 255);
        public Color BackgroundOverColor = Color.FromArgb(241, 238, 231);
        public Color HLinesColor = Color.FromArgb(222, 222, 222);
        public Color VLinesColor = Color.FromArgb(165, 182, 222);
        public Color BorderColor = Color.FromArgb(0, 16, 123);

        private PictureBox Demo;
        private VScrollBar Scroll;

        // 保存所有图像
        private List<ImageEntity> _images = new List<ImageEntity>();
        private Label Hint;

        public List<ImageEntity> Images
        {
            get { return this._images; }
            set { this._images = value; }
        }

        private string _imagePath = "";

        public string ImagePath
        {
            get { return this._imagePath; }
            set { this._imagePath = value; }
        }

        private RectangleF FaceRect { get; set; }
        private Rectangle ClientRect { get; set; }
        private RectangleF Rect { get; set; }

        public bool ShowDemo { get; set; }

        #endregion

        public FaceForm()
        {// 构造函数
            this.InitializeComponent();

            // Window Style
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.Hide();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;

        }


        private void InitializeComponent()
        {
            this.Demo = new System.Windows.Forms.PictureBox();
            this.Scroll = new System.Windows.Forms.VScrollBar();
            this.Hint = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Demo)).BeginInit();
            this.SuspendLayout();
            // 
            // Demo
            // 
            this.Demo.BackColor = System.Drawing.Color.White;
            this.Demo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Demo.Location = new System.Drawing.Point(0, 0);
            this.Demo.Name = "Demo";
            this.Demo.Size = new System.Drawing.Size(100, 50);
            this.Demo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Demo.TabIndex = 0;
            this.Demo.TabStop = false;
            this.Demo.Visible = false;
            //this.Demo.VisibleChanged += new System.EventHandler(this.Demo_VisibleChanged);
            this.Demo.Click += new System.EventHandler(this.Demo_Click);
            // 
            // Scroll
            // 
            this.Scroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.Scroll.LargeChange = 1;
            this.Scroll.Location = new System.Drawing.Point(285, 0);
            this.Scroll.Name = "Scroll";
            this.Scroll.Size = new System.Drawing.Size(15, 240);
            this.Scroll.TabIndex = 0;
            this.Scroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnVScroll);
            // 
            // Hint
            // 
            this.Hint.AutoSize = true;
            this.Hint.BackColor = System.Drawing.Color.White;
            this.Hint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Hint.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Hint.Location = new System.Drawing.Point(0, 0);
            this.Hint.Name = "Hint";
            this.Hint.Size = new System.Drawing.Size(2, 22);
            this.Hint.TabIndex = 1;
            // 
            // ImagePopup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(300, 240);
            this.Controls.Add(this.Hint);
            this.Controls.Add(this.Demo);
            this.Controls.Add(this.Scroll);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImagePopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)(this.Demo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region Events

        public event SelectFaceHnadler Selected = null;

        protected virtual void OnSelected(SelectFaceArgs e)
        {
            if (this.Selected != null)
            {
                this.Selected(this, e);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="nHSpace">水平间隔</param>
        /// <param name="nVSpace">垂直间隔</param>
        /// <param name="nColumns">列数</param>
        /// <param name="nRows">行数</param>
        /// <returns></returns>
        public bool Init(int imageWidth, int imageHeight, int nHSpace, int nVSpace, int nColumns, int nRows)
        {
            if (this.DesignMode)
            {
                return false;
            }

            try
            {
                ImageEntity img;
                this.Images.Clear();

                try
                {
                    // 如果缓存记录，读取指定文件夹下的所有图片
                    if (!File.Exists(Path.Combine(Application.StartupPath, this.ImagePath, "Images.db")))
                    {
                        foreach (string imgPath in Directory.GetFiles(Path.Combine(Application.StartupPath, this.ImagePath)))
                        {
                            img = new ImageEntity(imgPath);

                            if (img.Image != null)
                            {
                                this.Images.Add(img);
                            }
                        }
                        try
                        {
                            Tools.BinarySerializer(this.Images, Path.Combine(Application.StartupPath, this.ImagePath, "Images.db"));
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                        }
                    }
                    else
                    { // 如果有缓存信息，则直接读取这些信息

                        this.Images = Tools.BinaryDeserialize<List<ImageEntity>>(Path.Combine(Application.StartupPath, this.ImagePath, "Images.db"));
                        try
                        {
                            foreach (var item in this.Images)
                            {
                                if (!File.Exists(item.FullName))
                                {
                                    item.IsDelete = true;
                                }
                            }

                        }
                        catch { }

                        foreach (var imgItem in this.Images.FindAll(item => item.IsDelete))
                        {
                            try
                            {
                                File.Delete(imgItem.FullName);
                            }
                            catch
                            {
                            }
                        }
                    }
                   
                }
                catch
                {
                    this.Images = new List<ImageEntity>();
                }

                this.Images = this.Images.FindAll(item => !item.IsDelete);
                this.Images.Sort();

                this.Demo.Width = imageWidth + nHSpace - 1;
                this.Demo.Height = imageHeight + nVSpace -1;

                this._nColumns = nColumns;
                this._nRows = nRows;
                this._nHSpace = nHSpace;
                this._nVSpace = nVSpace;

                this._imageWidth = imageWidth;
                this._imageHeight = imageHeight;
                this._nItemWidth = imageWidth + nHSpace;
                this._nItemHeight = imageHeight + nVSpace;

                this._nBitmapWidth = this._nColumns * this._nItemWidth + 1;
                this._nBitmapHeight = this._nRows * this._nItemHeight + 1;
                this.Width = this._nBitmapWidth + 14;
                this.Height = this._nBitmapHeight;

                this.PageIndex = 0;
                this.PageImageCount = this._nColumns * this._nRows;
                this.PageCount = (int)Math.Ceiling(this.Images.Count / (double)this.PageImageCount);
                this.Scroll.Maximum = this.PageCount-1;
                
                this.ClientRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

                this.FaceRect = new RectangleF(0f, 0f, this._nBitmapWidth, this._nBitmapHeight);

                this.Rect = new RectangleF(1f, 1f, this._nBitmapWidth - 2, this._nBitmapHeight - 2);
                
                this.DrawBackImage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// 设置背景图片，包括边缘线和表情。以后的绘制都是根据这个进行的
        /// </summary>
        private void DrawBackImage()
        {
            this._Bitmap = new Bitmap(this._nBitmapWidth, this._nBitmapHeight);

            Graphics g = Graphics.FromImage(this._Bitmap);

            g.FillRectangle(new SolidBrush(this.BackgroundColor), 0, 0, this._nBitmapWidth, this._nBitmapHeight);

            for(int i = 0; i < this._nColumns; i++)
            {
                g.DrawLine(new Pen(this.VLinesColor), i * this._nItemWidth, 0, i * this._nItemWidth,
                           this._nBitmapHeight - 1);
            }
            for(int i = 0; i < this._nRows; i++)
            {
                g.DrawLine(new Pen(this.HLinesColor), 0, i * this._nItemHeight, this._nBitmapWidth - 1,
                           i * this._nItemHeight);
            }

            g.DrawRectangle(new Pen(this.BorderColor), 0, 0, this._nBitmapWidth - 1, this._nBitmapHeight - 1);

            for(int i = 0; i < this._nColumns; i++)
            {
                for(int j = 0; j < this._nRows; j++)
                {
                    if((j * this._nColumns + i) < this.Images.Count - this.PageIndex * this.PageImageCount)
                    {
                        g.DrawImage(this.Images[this.PageIndex * this.PageImageCount + j * this._nColumns + i].Image,
                                    i * this._nItemWidth + this._nHSpace / 2,
                                    j * this._nItemHeight + this._nVSpace / 2, this._imageWidth, this._imageHeight);
                    }
                }
            }
        }
        
        /// <summary>
        /// 根据指定位置显示控件
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Show(int x, int y)
        {
            this.Show(x, y, 0);
        }

        public void Show(int x, int y, int offsetHeight)
        {
            this.Show(x, y, null, offsetHeight);
        }

        public void Show(int x, int y, Control ctl)
        {
            this.Show(x, y, ctl, 0);
        }

        public void Show(int x, int y, Control ctl, int offsetHeight)
        {
            Point pt = new Point(x, y);
            int tmpHeight = 0;
            if(ctl != null)
            {
                tmpHeight = ctl.Top + ctl.Height;
            }

            if(pt.X < 0)
            {
                pt = new Point(0, pt.Y);
            }
            if(pt.Y < 0)
            {
                pt = new Point(pt.X, 0);
            }
            if(pt.Y + this.Height > Screen.PrimaryScreen.WorkingArea.Height)
            {
                pt = new Point(pt.X, pt.Y - this.Height - tmpHeight - offsetHeight);
            }
            if(pt.X + this.Width > Screen.PrimaryScreen.WorkingArea.Width)
            {
                pt = new Point(pt.X - (pt.X + this.Width - Screen.PrimaryScreen.WorkingArea.Width), pt.Y);
            }

            this.Left = pt.X;
            this.Top = pt.Y;
            this.Demo.Visible = false;
            this.Show();
            this.Refresh();
        }
        
        #endregion

        #region Overrides
        protected override void OnDeactivate(EventArgs ea)
        {
            this.Hide();
        }

        /// <summary>
        /// 鼠标滑动。主要定义画过图像时使其可以动态显示
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {

            if(!this.Rect.Contains(e.Location))
            {
                this._nCoordX = -1;
                this._nCoordY = -1;
                this.Invalidate();

                base.OnMouseMove(e);
                return;
            }

            if (this.Images == null || this.Images.Count == 0)
            {
                return;
            }

            if(((e.X / this._nItemWidth) != this._nCoordX) || ((e.Y / this._nItemHeight) != this._nCoordY))
            {
                this._nCoordX = e.X / this._nItemWidth;
                this._nCoordY = e.Y / this._nItemHeight;
                this.imageIndex = this.PageIndex * this.PageImageCount + this._nCoordY * this._nColumns + this._nCoordX;

                if (this.imageIndex >= 0 && this.imageIndex < this.Images.Count)
                {
                    this.Demo.Visible = true && ShowDemo;

                    this.Demo.Left = this._nItemWidth * this._nCoordX + 1;
                    this.Demo.Top = this._nItemHeight * this._nCoordY + 1;

                    Image tmpImg = this.Images[this.imageIndex].Image;

                    if (tmpImg != null)
                    {
                        if (tmpImg.Width > this.Demo.Width || tmpImg.Height > this.Demo.Height)
                        {
                            this.Demo.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        else
                        {
                            this.Demo.SizeMode = PictureBoxSizeMode.CenterImage;
                        }

                        this.Demo.Image = tmpImg;
                    }

                    // hint提示设置
                    this.Hint.Visible = true && ShowDemo;
                    this.Hint.Left = (e.X + 70 < this.Width) ? e.X + 10 : this.Width - 60;
                    this.Hint.Top = (e.Y + 40 < this.Height) ? e.Y + 20 : this.Height - 20;
                    this.Hint.Text = this.Images[this.imageIndex].Hint;
                }
                else
                {
                    this.Demo.Visible = false;
                    this.Hint.Visible = false;
                }
                this.Invalidate();
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// 更新选中图片信息
        /// </summary>
        private void UpdateImageIndex()
        {
        }
        
        /// <summary>
        /// 翻页功能
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {// 翻页
            // e.Delta中已经包含正负号，据此判断上翻页还是下翻页
            this.PageIndex -= e.Delta / 120;
            if (this.PageIndex < 0)
                this.PageIndex = 0;
            else if (this.PageIndex > this.PageCount - 1)
                this.PageIndex = this.PageCount - 1;
            this.DrawBackImage();
            this.Scroll.Value = this.PageIndex;
            this.Invalidate();

            base.OnMouseWheel(e);
        }

        /// <summary>
        /// 滚轮事件，与上面结合着用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnVScroll(object sender, ScrollEventArgs e)
        {
            this.Demo.Visible = false;
            this.PageIndex = this.Scroll.Value;
            this.DrawBackImage();
            Invalidate();
        } 

        /// <summary>
        /// 定义绘制背景的规则
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Pixel;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.Clear(Color.White);

            g.DrawRectangle(new Pen(this.BorderColor), this.ClientRect);

            using(Bitmap offscreenBitmap = new Bitmap(this._nBitmapWidth, this._nBitmapHeight))
            {
                using(Graphics offscreenGrfx = Graphics.FromImage(offscreenBitmap))
                {
                    offscreenGrfx.DrawImage(this._Bitmap, this.FaceRect);

                    if (this.Images != null && this.Images.Count > 0)
                    {
                        if (this._nCoordX != -1 && this._nCoordY != -1 &&
                            (this._nCoordY * this._nColumns + this._nCoordX) <
                            this.Images.Count - this.PageIndex * this.PageImageCount)
                        {
                            offscreenGrfx.FillRectangle(new SolidBrush(this.BackgroundOverColor),
                                                        this._nCoordX * this._nItemWidth + 1,
                                                        this._nCoordY * this._nItemHeight + 1,
                                                        this._nItemWidth - 1, this._nItemHeight - 1);
                            offscreenGrfx.DrawRectangle(new Pen(this.BorderColor), this._nCoordX * this._nItemWidth,
                                                        this._nCoordY * this._nItemHeight, this._nItemWidth,
                                                        this._nItemHeight);
                        }
                    }
                }

                g.DrawImage(offscreenBitmap, this.FaceRect);
            }
        }

        #endregion
                /*
        private void Demo_VisibleChanged(object sender, EventArgs e)
        {
            if(!this.Demo.Visible)
            {
                this.Demo.Image = null;
                this.Demo.Refresh();
                this.Demo.Hide();
            }
        }*/

        /// <summary>
        /// 选中表情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Demo_Click(object sender, EventArgs e)
        {
            if (this.Selected != null && this.imageIndex >= 0 && this.imageIndex < this.Images.Count)
            {
                this.OnSelected(new SelectFaceArgs(this.Images[this.imageIndex]));

                this.Hide();
            }
        }
    }

    public delegate void SelectFaceHnadler(object sender, SelectFaceArgs e);

    public class SelectFaceArgs : EventArgs
    {
        public SelectFaceArgs()
        {
        }

        public SelectFaceArgs(ImageEntity img)
            : this()
        {
            this.Img = img;
        }

        public SelectFaceArgs(ImageEntity img, object tag)
            : this(img)
        {
            this.Tag = tag;
        }

        public ImageEntity Img { get; set; }

        public object Tag { get; set; }
    }

    [Serializable]
    public class ImageEntity : IComparable
    {
        private static Dictionary<string, int> Ids = new Dictionary<string, int>()
        {
            { "微笑",   1 },  { "得意",   2 },  { "窃喜",   3 },  { "开心",   4 },
            { "畅快",   5 },  { "飘飘然", 6 },  { "无聊",   7 },  { "发呆",   8 },
            { "难过",   9 },  { "委屈",  10 },  { "快哭了",11 },  { "大哭",  12 },
            { "流泪",  13 },  { "敲打",  14 },  { "发怒",  15 },  { "奋斗",  16 },
            { "疑问",  17 },  { "惊讶",  18 },  { "惊呆了",19 },  { "惊恐",  20 },
            { "抓狂",  21 },  { "折磨",  22 },  { "衰",    23 },  { "骷髅",  24 },
            { "强",    25 },  { "弱",    26 },  { "拳头",  27 },  { "拜托",  28 },
            { "胜利",  29 },  { "OK",    30 },  { "NO",    31 },  { "差劲",  32 },
            { "爱你",  33 },  { "受伤",  34 },  { "左哼哼",35 },  { "右哼哼",36 },
            { "哈欠",  37 },  { "困",    38 },  { "睡",    39 },  { "傲慢",  40 },
            { "冷汗",  41 },  { "晕",    42 },  { "流汗",  43 },  { "擦汗",  44 },
            { "糗大了",45 },  { "尴尬",  46 },  { "酷",    47 },  { "鼓掌",  48 },
            { "饥饿",  49 },  { "害羞",  50 },  { "色",    51 },  { "可怜",  52 },
            { "可爱",  53 },  { "吐",    54 },  { "吓",    55 },  { "咒骂",  56 },
            { "嘘",    57 },  { "坏笑",  58 },  { "撇嘴",  59 },  { "白眼",  60 },
            { "调皮",  61 },  { "鄙视",  62 },  { "闭嘴",  63 },  { "阴险",  64 },
        };

        public ImageEntity()
        {
        }

        public ImageEntity(string fullName)
            : this()
        {
            this.FullName = fullName.Replace(Application.StartupPath + "\\", ""); ;

            this.FilePath = Path.GetDirectoryName(fullName).Replace(Application.StartupPath + "\\", ""); 

            this.FileName = Path.GetFileName(fullName);

            this.Hint = FileName.Split(".".ToCharArray())[0];
            Console.WriteLine(this.Hint.ToCharArray());
            Console.WriteLine(Ids.Keys);
            if (Ids.ContainsKey(this.Hint))
            {
                int temp;
                Ids.TryGetValue(this.Hint, out temp);
                this.Id = temp;
                Console.WriteLine("" + this.Hint + ": " + this.Id);
            }

            try
            {
                this.Image = Image.FromFile(fullName);
                this.MD5 = SecurityHelper.GetMD5(fullName);
            }
            catch(Exception ex)
            {
                this.Image = null;
                try
                {
                    File.Delete(fullName);
                }
                catch
                {
                }
                Debug.WriteLine(fullName);
                Debug.WriteLine(ex);
            }
        }

        public ImageEntity(string filePath, object tag)
            : this(filePath)
        {
            this.Tag = tag;
        }

        public int Id { get; set; }
        public string Hint { get; set; }
        public string FileName { get; set; }

        private string _fullName;
        public string FullName
        {
            get
            {
                return Path.Combine(Application.StartupPath, this._fullName);
            }
            set { this._fullName = value; }
        }

        private string _filePath;
        public string FilePath
        {
            get
            { 
                return Path.Combine(Application.StartupPath,this._filePath);
            }
            set { this._filePath = value; }
        }

        public bool IsCustom { get; set; }

        public Image Image { get; set; }

        public string MD5 { get; set; }

        public object Tag { get; private set; }

        public bool IsDelete { get; set; }

        public int CompareTo(object obj) 
        {
            int result;
            try{
                ImageEntity img = obj as ImageEntity;
                if (this.Id > img.Id)
                    result = 1;
                else
                    result = -1;
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

    }
    
    /// <summary>
    /// 工具类定义
    /// </summary>
    public class Tools
    {

        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <typeparam name="T">序列化的类型</typeparam>
        /// <param name="obj">序列化的对象</param>
        /// <param name="filename">序列化的XML文件名</param>
        public static void BinarySerializer<T>(T obj, string filename)
        {


            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, obj);
                }
                catch (SerializationException e)
                {
                    Trace.WriteLine(e);

                }
            }
        }

        /// <summary>
        /// 二进制反列化
        /// </summary>
        /// <typeparam name="T">反序列化的类型</typeparam>
        /// <param name="filename">反序列化的XML文件名</param>
        /// <returns>T类型对象</returns>
        public static T BinaryDeserialize<T>(string filename)
        {

            // 检查文件是否存在
            if (!File.Exists(filename))
            {
                return default(T);
            }
            T obj = default(T);
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    obj = (T)formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Trace.WriteLine(e);

                }
            }
            return obj;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">目录名</param>
        public static void CreateDir(string path)
        {
            string[] dirs = path.Split(char.Parse("\\"));
            string tmp = "";
            if (!Directory.Exists(path))
            {
                foreach (string item in dirs)
                {
                    tmp += item + "\\";
                    if (!Directory.Exists(tmp))
                    {
                        Directory.CreateDirectory(tmp);
                    }
                }
            }
        }


    }
    /// <summary>
    /// 安全助手类定义
    /// </summary>
    public class SecurityHelper
    {
        /// <summary>
        /// 获取字符串的 MD5 值
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>MD5 值</returns>
        public static string GetStringMD5(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] encryptedBytes = md5.ComputeHash(Encoding.Default.GetBytes(text));

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 实现对一个文件md5的读取，path为文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMD5(string path)
        {
            try
            {
                using (Stream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return GetMD5(file);
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// 获取流的 MD5 值
        /// </summary>
        /// <param name="s">流</param>
        /// <returns>MD5 值</returns>
        public static string GetMD5(Stream s)
        {
            byte[] hash_byte;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                hash_byte = md5.ComputeHash(s);
            }
            return GetMD5String(hash_byte);
        }

        /// <summary>
        /// 获取数组的 MD5 值
        /// </summary>
        /// <param name="buffer">数组</param>
        /// <returns>MD5 值</returns>
        public static string GetMD5(byte[] buffer)
        {
            byte[] hash_byte;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                hash_byte = md5.ComputeHash(buffer);
            }
            return GetMD5String(hash_byte);
        }

        /// <summary>
        /// 获取数组的 MD5 值
        /// </summary>
        /// <param name="buffer">数组</param>
        /// <param name="offset">偏移</param>
        /// <param name="count">长度</param>
        /// <returns>MD5 值</returns>
        public static string GetMD5(byte[] buffer, int offset, int count)
        {
            byte[] hash_byte;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                hash_byte = md5.ComputeHash(buffer, offset, count);
            }
            return GetMD5String(hash_byte);
        }

        /// <summary>
        /// 获取数组的 MD5 值
        /// </summary>
        /// <param name="hash_byte">数组</param>
        /// <returns>MD5 值</returns>
        private static string GetMD5String(byte[] hash_byte)
        {
            string resule = BitConverter.ToString(hash_byte);
            resule = resule.Replace("-", "");
            return resule;
        }
    }
}