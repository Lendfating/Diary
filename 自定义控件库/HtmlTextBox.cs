using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace 自定义控件库
{
    /// <summary>
    /// Provides a user control that allows the user to edit HTML page.
    /// </summary>
    [Description("Provides a user control that allows the user to edit HTML page."), ClassInterface(ClassInterfaceType.AutoDispatch)]
    public partial class HtmlTextBox : UserControl
    {
        private String resource_path = "";// 资源路径
        private String file_path = "";// 文件路径
        private Boolean _editable = true;// 是否可编辑。

        public HtmlTextBox()
        {
            dataUpdate = 0;

            InitializeComponent();

            InitializeControls();
        }

        public HtmlTextBox(String resource_path, String file_path)
        {
            this.resource_path = resource_path;
            this.file_path = file_path;

            dataUpdate = 0;

            InitializeComponent();

            InitializeControls();
        }


        #region Properties

        [Category("HtmlTextBox"), Description("设置控件大小")]
        public Size BoxSize
        {
            get { return this.Size; }
            set
            {
                this.Size = value;
                this.webBrowserBody.Size = new System.Drawing.Size(625, value.Height - 82);
                this.Invalidate();
            }
        }

        [Category("HtmlTextBox"), Description("控件是否可编辑")]
        public Boolean Editable
        {
            get { return this._editable; }
            set
            {
                try
                {
                    BeginUpdate();
                    if (!this._editable && value)
                    {// 切换到编辑模式
                        this.webBrowserBody.Document.Click += new HtmlElementEventHandler(webBrowserBody_DocumentClick);
                        this.webBrowserBody.Document.Focusing += new HtmlElementEventHandler(webBrowserBody_DocumentFocusing);
                        webBrowserBody.Document.ExecCommand("EditMode", false, null);//设置为编辑状态
                        webBrowserBody.Document.ExecCommand("LiveResize", false, null);// 迫使 MSHTML 编辑器在缩放或移动过程中持续更新元素外观，而不是只在移动或缩放完成后更新。
                        this.toolBar.Visible = true;
                        this.webBrowserBody.Location = new System.Drawing.Point(0, 35);
                        this.webBrowserBody.Size = new Size(webBrowserBody.Size.Width, webBrowserBody.Size.Height - 35);
                        this.moodPicture.Click += new System.EventHandler(this.moodPicture_Click);
                    }
                    else if (this._editable && !value)
                    {// 浏览模式
                        this.webBrowserBody.Document.ExecCommand("BrowseMode", false, null);
                        this.toolBar.Visible = false;
                        this.webBrowserBody.Location = new System.Drawing.Point(0, 0);
                        this.webBrowserBody.Size = new Size(webBrowserBody.Size.Width, webBrowserBody.Size.Height + 35);
                        this.moodPicture.Click -= new System.EventHandler(this.moodPicture_Click);
                    }
                    this._editable = value;
                    EndUpdate();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current text in the HTMLTextBox
        /// </summary>
        public override string Text
        {
            get
            {
                String body = webBrowserBody.Document.Body.InnerHtml;
                return (body == null) ? string.Empty : body;
            }
            set
            {
                try
                {
                    if (value == null) value = string.Empty;
                    // set the body property
                    String format = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">{2}<HTML>{2}<HEAD>{2}<META content=""text/html; charset=unicode"" http-equiv=Content-Type>{2}<META name=GENERATOR content=""MSHTML 11.00.9600.17344"">{2}<link rel=""stylesheet"" type=""text/css"" href=""{0}"" />{2}</HEAD>{2}<BODY>{1}</BODY>{2}</HTML>{2}";
                    webBrowserBody.Document.OpenNew(false).Write(String.Format(format, Path.Combine(resource_path, "basic.css"), value, Environment.NewLine));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Gets the collection of the image path in the HTMLTextBox
        /// </summary>
        public string[] Images
        {
            get
            {
                List<string> images = new List<string>();

                foreach (HtmlElement element in webBrowserBody.Document.Images)
                {
                    string image = element.GetAttribute("src");
                    if (!images.Contains(image))
                    {
                        images.Add(image);
                    }
                }

                return images.ToArray();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize controls
        /// </summary>
        private void InitializeControls()
        {
            BeginUpdate();

            // 一开始初始化一个页面，之后就一直用这一个页面了
            webBrowserBody.Navigate("about:blank");
            this.Editable = false; //先关闭，以使第一次编辑时会出发响应操作
            OpenEditor(DateTime.Now.Date.ToString("yyyy-MM-dd"), true);
            EndUpdate();
        }
        
        /// <summary>
        /// Refresh tool bar buttons
        /// </summary>
        private void RefreshToolBar()
        {
            BeginUpdate();

            try
            {
                mshtml.IHTMLDocument2 document = (mshtml.IHTMLDocument2)webBrowserBody.Document.DomDocument;

                ButtonBold.Checked = document.queryCommandState("Bold");
                ButtonItalic.Checked = document.queryCommandState("Italic");
                ButtonUnderline.Checked = document.queryCommandState("Underline");
                ButtonStrikethrough.Checked = document.queryCommandState("strikeThrough");
                
                ButtonNumbers.Checked = document.queryCommandState("InsertOrderedList");
                ButtonList.Checked = document.queryCommandState("InsertUnorderedList");

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            finally
            {
                EndUpdate();
            }
        }

        #endregion

        #region Updating

        private int dataUpdate;
        private bool Updating
        {
            get
            {
                return dataUpdate != 0;
            }
        }

        private void BeginUpdate()
        {
            ++dataUpdate;
        }
        private void EndUpdate()
        {
            --dataUpdate;
        }

        #endregion

        #region Tool Bar

        // 详细操作命令列表参见：https://developer.mozilla.org/zh-CN/docs/Web/API/document.execCommand

        private void ButtonBold_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("Bold", false, null);
            RefreshToolBar();
        }

        private void ButtonItalic_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("Italic", false, null);
            RefreshToolBar();
        }

        private void ButtonUnderline_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("Underline", false, null);
            RefreshToolBar();
        }

        private void ButtonStrikethrough_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("StrikeThrough", false, null);
            RefreshToolBar();
        }

        private void ButtonList_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("InsertUnorderedList", false, null);
            RefreshToolBar();
        }

        private void ButtonNumbers_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("InsertOrderedList", false, null);
            RefreshToolBar();
        }

        private void ButtonQuote_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("FormatBlock", false, "<pre>");
            //webBrowserBody.Document.ExecCommand("FormatBlock", false, "<blockquote>");
            RefreshToolBar();
        }

        private void ButtonHeading1_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("FormatBlock", false, "<h1>");
            RefreshToolBar();
        }

        private void ButtonHeading2_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("FormatBlock", false, "<h2>");
            RefreshToolBar();
        }

        private void ButtonHeading3_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("FormatBlock", false, "<h3>");
            RefreshToolBar();
        }

        private void ButtonHeading4_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("FormatBlock", false, "<h4>");
            RefreshToolBar();
        }

        private void ButtonLine_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("InsertHorizontalRule", false, null);
            RefreshToolBar();
        }

        private void ButtonLink_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("CreateLink", true, null);
            RefreshToolBar();
        }

        private void ButtonPicture_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("InsertImage", true, null);
            RefreshToolBar();
        }

        private void ButtonUndo_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("Undo", false, null);
            RefreshToolBar();
        }

        private void ButtonRepeat_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            webBrowserBody.Document.ExecCommand("Redo", false, null);
            RefreshToolBar();
        }


        #endregion

        #region Web Browser
        
        private void webBrowserBody_DocumentClick(object sender, HtmlElementEventArgs e)
        {
            RefreshToolBar();
        }

        private void webBrowserBody_DocumentFocusing(object sender, HtmlElementEventArgs e)
        {
            RefreshToolBar();
        }

        #endregion


        public FaceForm _faceForm = null;
        public FaceForm FaceForm
        {
            get
            {
                if (this._faceForm == null)
                {
                    this._faceForm = new FaceForm
                    {
                        ImagePath = Path.Combine(resource_path, @"Face"),
                        ShowDemo = true,
                    };

                    this._faceForm.Init(56, 56, 8, 8, 8, 4);
                    this._faceForm.Selected += this._changeMood;

                }

                return this._faceForm;
            }
        }

        private void _changeMood(object sender, SelectFaceArgs e)
        {
            this.moodName = e.Img.Hint;
            this.moodPicture.Image = e.Img.Image;
        }

        private void moodPicture_Click(object sender, EventArgs e)
        {
            Point pt = this.PointToScreen(new Point(((PictureBox)sender).Left - 500, ((PictureBox)sender).Height + 5));
            this.FaceForm.Show(pt.X, pt.Y, ((PictureBox)sender).Height);
        }

        private String moodName = "微笑";
        public void OpenEditor(String fileName, Boolean editable)
        {
            try
            {
                moodName = "微笑";
                String path = Path.Combine(file_path, fileName + ".md");
                String content = string.Empty;
                if (File.Exists(Path.Combine(file_path, fileName + ".md")))
                {
                    // Open the file to read from. 
                    using (StreamReader sr = File.OpenText(Path.Combine(file_path, fileName + ".md")))
                    {
                        String seg = sr.ReadLine();
                        String name = sr.ReadLine();
                        moodName = sr.ReadLine().Split(new[] { ": " }, StringSplitOptions.None)[1];
                        seg = sr.ReadLine();
                        content = sr.ReadToEnd();
                        sr.Close();
                    }
                }

                // 初始化显示内容
                this.dateLabel.Text = String.Format("{0}年{1}月{2}日", fileName.Split(new[] { "-" }, StringSplitOptions.None));
                this.moodPicture.Image = Image.FromFile(Path.Combine(resource_path, "Face", moodName + ".gif"));
                Converter converter = new Converter();
                this.Text = converter.Md2Html(content);
                // 视情况打开编辑
                this.Editable = editable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveEditor()
        {
            String path = Path.Combine(file_path, System.DateTime.Now.ToString("yyyy-MM-dd") + ".md");
            // Create a file to write to. 
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("----------");
                sw.WriteLine(String.Format("Name: {0}", this.dateLabel.Text));
                sw.WriteLine(String.Format("Mood: {0}", this.moodName));
                sw.WriteLine("----------");
                Converter converter =  new Converter();
                sw.Write(converter.Html2Md(this.Text));
                sw.Flush();
                sw.Close();
            }
        }

        private void ButtonPublish_Click(object sender, EventArgs e)
        {
            this.SaveEditor();
        }

    }
}
