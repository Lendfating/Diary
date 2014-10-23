namespace 自定义控件库
{
    partial class HtmlTextBox
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                resources.ReleaseAllResources();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HtmlTextBox));
            this.dateLabel = new System.Windows.Forms.Label();
            this.webBrowserBody = new System.Windows.Forms.WebBrowser();
            this.panel = new System.Windows.Forms.Panel();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.ButtonPublish = new System.Windows.Forms.ToolStripButton();
            this.ButtonBold = new System.Windows.Forms.ToolStripButton();
            this.ButtonItalic = new System.Windows.Forms.ToolStripButton();
            this.ButtonUnderline = new System.Windows.Forms.ToolStripButton();
            this.ButtonStrikethrough = new System.Windows.Forms.ToolStripButton();
            this.ButtonList = new System.Windows.Forms.ToolStripButton();
            this.ButtonNumbers = new System.Windows.Forms.ToolStripButton();
            this.ButtonQuote = new System.Windows.Forms.ToolStripButton();
            this.ButtonHeading1 = new System.Windows.Forms.ToolStripButton();
            this.ButtonHeading2 = new System.Windows.Forms.ToolStripButton();
            this.ButtonHeading3 = new System.Windows.Forms.ToolStripButton();
            this.ButtonHeading4 = new System.Windows.Forms.ToolStripButton();
            this.ButtonLine = new System.Windows.Forms.ToolStripButton();
            this.ButtonLink = new System.Windows.Forms.ToolStripButton();
            this.ButtonPicture = new System.Windows.Forms.ToolStripButton();
            this.ButtonUndo = new System.Windows.Forms.ToolStripButton();
            this.ButtonRepeat = new System.Windows.Forms.ToolStripButton();
            this.moodPicture = new System.Windows.Forms.PictureBox();
            this.panel.SuspendLayout();
            this.toolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moodPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Font = new System.Drawing.Font("华文楷体", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateLabel.Location = new System.Drawing.Point(0, 0);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(277, 43);
            this.dateLabel.TabIndex = 0;
            this.dateLabel.Text = "2014年12月23日";
            // 
            // webBrowserBody
            // 
            this.webBrowserBody.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.webBrowserBody.Location = new System.Drawing.Point(0, 35);
            this.webBrowserBody.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserBody.Name = "webBrowserBody";
            this.webBrowserBody.Size = new System.Drawing.Size(625, 418);
            this.webBrowserBody.TabIndex = 2;
            // 
            // panel
            // 
            this.panel.AutoSize = true;
            this.panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel.Controls.Add(this.toolBar);
            this.panel.Controls.Add(this.webBrowserBody);
            this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel.Location = new System.Drawing.Point(0, 47);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(625, 453);
            this.panel.TabIndex = 1;
            // 
            // toolBar
            // 
            this.toolBar.AllowMerge = false;
            this.toolBar.AutoSize = false;
            this.toolBar.BackColor = System.Drawing.SystemColors.Control;
            this.toolBar.CanOverflow = false;
            this.toolBar.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.toolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolBar.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonPublish,
            this.ButtonBold,
            this.ButtonItalic,
            this.ButtonUnderline,
            this.ButtonStrikethrough,
            this.ButtonList,
            this.ButtonNumbers,
            this.ButtonQuote,
            this.ButtonHeading1,
            this.ButtonHeading2,
            this.ButtonHeading3,
            this.ButtonHeading4,
            this.ButtonLine,
            this.ButtonLink,
            this.ButtonPicture,
            this.ButtonUndo,
            this.ButtonRepeat});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolBar.Size = new System.Drawing.Size(625, 35);
            this.toolBar.TabIndex = 3;
            // 
            // ButtonPublish
            // 
            this.ButtonPublish.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ButtonPublish.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPublish.Image")));
            this.ButtonPublish.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonPublish.Name = "ButtonPublish";
            this.ButtonPublish.Size = new System.Drawing.Size(128, 32);
            this.ButtonPublish.Text = "发布文章";
            this.ButtonPublish.Click += new System.EventHandler(this.ButtonPublish_Click);
            // 
            // ButtonBold
            // 
            this.ButtonBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonBold.Image = ((System.Drawing.Image)(resources.GetObject("ButtonBold.Image")));
            this.ButtonBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonBold.Name = "ButtonBold";
            this.ButtonBold.Size = new System.Drawing.Size(36, 32);
            this.ButtonBold.Text = "粗体";
            this.ButtonBold.Click += new System.EventHandler(this.ButtonBold_Click);
            // 
            // ButtonItalic
            // 
            this.ButtonItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonItalic.Image = ((System.Drawing.Image)(resources.GetObject("ButtonItalic.Image")));
            this.ButtonItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonItalic.Name = "ButtonItalic";
            this.ButtonItalic.Size = new System.Drawing.Size(36, 32);
            this.ButtonItalic.Text = "斜体";
            this.ButtonItalic.Click += new System.EventHandler(this.ButtonItalic_Click);
            // 
            // ButtonUnderline
            // 
            this.ButtonUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonUnderline.Image = ((System.Drawing.Image)(resources.GetObject("ButtonUnderline.Image")));
            this.ButtonUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonUnderline.Name = "ButtonUnderline";
            this.ButtonUnderline.Size = new System.Drawing.Size(36, 32);
            this.ButtonUnderline.Text = "下划线";
            this.ButtonUnderline.Click += new System.EventHandler(this.ButtonUnderline_Click);
            // 
            // ButtonStrikethrough
            // 
            this.ButtonStrikethrough.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonStrikethrough.Image = ((System.Drawing.Image)(resources.GetObject("ButtonStrikethrough.Image")));
            this.ButtonStrikethrough.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonStrikethrough.Name = "ButtonStrikethrough";
            this.ButtonStrikethrough.Size = new System.Drawing.Size(36, 32);
            this.ButtonStrikethrough.Text = "删除线";
            this.ButtonStrikethrough.Click += new System.EventHandler(this.ButtonStrikethrough_Click);
            // 
            // ButtonList
            // 
            this.ButtonList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonList.Image = ((System.Drawing.Image)(resources.GetObject("ButtonList.Image")));
            this.ButtonList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonList.Name = "ButtonList";
            this.ButtonList.Size = new System.Drawing.Size(36, 32);
            this.ButtonList.Text = "列表";
            this.ButtonList.Click += new System.EventHandler(this.ButtonList_Click);
            // 
            // ButtonNumbers
            // 
            this.ButtonNumbers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonNumbers.Image = ((System.Drawing.Image)(resources.GetObject("ButtonNumbers.Image")));
            this.ButtonNumbers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonNumbers.Name = "ButtonNumbers";
            this.ButtonNumbers.Size = new System.Drawing.Size(36, 32);
            this.ButtonNumbers.Text = "编号";
            this.ButtonNumbers.Click += new System.EventHandler(this.ButtonNumbers_Click);
            // 
            // ButtonQuote
            // 
            this.ButtonQuote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonQuote.Image = ((System.Drawing.Image)(resources.GetObject("ButtonQuote.Image")));
            this.ButtonQuote.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonQuote.Name = "ButtonQuote";
            this.ButtonQuote.Size = new System.Drawing.Size(36, 32);
            this.ButtonQuote.Text = "引用";
            this.ButtonQuote.Click += new System.EventHandler(this.ButtonQuote_Click);
            // 
            // ButtonHeading1
            // 
            this.ButtonHeading1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonHeading1.Font = new System.Drawing.Font("Impact", 14F);
            this.ButtonHeading1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.ButtonHeading1.Image = ((System.Drawing.Image)(resources.GetObject("ButtonHeading1.Image")));
            this.ButtonHeading1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonHeading1.Name = "ButtonHeading1";
            this.ButtonHeading1.Size = new System.Drawing.Size(32, 32);
            this.ButtonHeading1.Text = "H1";
            this.ButtonHeading1.ToolTipText = "标题一";
            this.ButtonHeading1.Click += new System.EventHandler(this.ButtonHeading1_Click);
            // 
            // ButtonHeading2
            // 
            this.ButtonHeading2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonHeading2.Font = new System.Drawing.Font("Impact", 14F);
            this.ButtonHeading2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.ButtonHeading2.Image = ((System.Drawing.Image)(resources.GetObject("ButtonHeading2.Image")));
            this.ButtonHeading2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonHeading2.Name = "ButtonHeading2";
            this.ButtonHeading2.Size = new System.Drawing.Size(35, 32);
            this.ButtonHeading2.Text = "H2";
            this.ButtonHeading2.ToolTipText = "标题二";
            this.ButtonHeading2.Click += new System.EventHandler(this.ButtonHeading2_Click);
            // 
            // ButtonHeading3
            // 
            this.ButtonHeading3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonHeading3.Font = new System.Drawing.Font("Impact", 14F);
            this.ButtonHeading3.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.ButtonHeading3.Image = ((System.Drawing.Image)(resources.GetObject("ButtonHeading3.Image")));
            this.ButtonHeading3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonHeading3.Name = "ButtonHeading3";
            this.ButtonHeading3.Size = new System.Drawing.Size(35, 32);
            this.ButtonHeading3.Text = "H3";
            this.ButtonHeading3.ToolTipText = "标题三";
            this.ButtonHeading3.Click += new System.EventHandler(this.ButtonHeading3_Click);
            // 
            // ButtonHeading4
            // 
            this.ButtonHeading4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonHeading4.Font = new System.Drawing.Font("Impact", 14F);
            this.ButtonHeading4.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.ButtonHeading4.Image = ((System.Drawing.Image)(resources.GetObject("ButtonHeading4.Image")));
            this.ButtonHeading4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonHeading4.Name = "ButtonHeading4";
            this.ButtonHeading4.Size = new System.Drawing.Size(34, 32);
            this.ButtonHeading4.Text = "H4";
            this.ButtonHeading4.ToolTipText = "标题四";
            this.ButtonHeading4.Click += new System.EventHandler(this.ButtonHeading4_Click);
            // 
            // ButtonLine
            // 
            this.ButtonLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonLine.Font = new System.Drawing.Font("Impact", 14F);
            this.ButtonLine.Image = ((System.Drawing.Image)(resources.GetObject("ButtonLine.Image")));
            this.ButtonLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonLine.Name = "ButtonLine";
            this.ButtonLine.Size = new System.Drawing.Size(32, 32);
            this.ButtonLine.Text = "---";
            this.ButtonLine.ToolTipText = "分割线";
            this.ButtonLine.Click += new System.EventHandler(this.ButtonLine_Click);
            // 
            // ButtonLink
            // 
            this.ButtonLink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonLink.Image = ((System.Drawing.Image)(resources.GetObject("ButtonLink.Image")));
            this.ButtonLink.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonLink.Name = "ButtonLink";
            this.ButtonLink.Size = new System.Drawing.Size(36, 32);
            this.ButtonLink.Text = "插入链接";
            this.ButtonLink.Click += new System.EventHandler(this.ButtonLink_Click);
            // 
            // ButtonPicture
            // 
            this.ButtonPicture.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonPicture.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPicture.Image")));
            this.ButtonPicture.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonPicture.Name = "ButtonPicture";
            this.ButtonPicture.Size = new System.Drawing.Size(36, 32);
            this.ButtonPicture.Text = "插入图片";
            this.ButtonPicture.Click += new System.EventHandler(this.ButtonPicture_Click);
            // 
            // ButtonUndo
            // 
            this.ButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonUndo.Image = ((System.Drawing.Image)(resources.GetObject("ButtonUndo.Image")));
            this.ButtonUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonUndo.Name = "ButtonUndo";
            this.ButtonUndo.Size = new System.Drawing.Size(36, 32);
            this.ButtonUndo.Text = "撤销";
            this.ButtonUndo.Click += new System.EventHandler(this.ButtonUndo_Click);
            // 
            // ButtonRepeat
            // 
            this.ButtonRepeat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonRepeat.Image = ((System.Drawing.Image)(resources.GetObject("ButtonRepeat.Image")));
            this.ButtonRepeat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonRepeat.Name = "ButtonRepeat";
            this.ButtonRepeat.Size = new System.Drawing.Size(36, 32);
            this.ButtonRepeat.Text = "重做";
            this.ButtonRepeat.Click += new System.EventHandler(this.ButtonRepeat_Click);
            // 
            // moodPicture
            // 
            this.moodPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moodPicture.Image = ((System.Drawing.Image)(resources.GetObject("moodPicture.Image")));
            this.moodPicture.Location = new System.Drawing.Point(580, 1);
            this.moodPicture.Name = "moodPicture";
            this.moodPicture.Size = new System.Drawing.Size(45, 45);
            this.moodPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.moodPicture.TabIndex = 3;
            this.moodPicture.TabStop = false;
            this.moodPicture.Click += new System.EventHandler(this.moodPicture_Click);
            // 
            // HtmlTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.moodPicture);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.dateLabel);
            this.MinimumSize = new System.Drawing.Size(625, 500);
            this.Name = "HtmlTextBox";
            this.Size = new System.Drawing.Size(625, 500);
            this.panel.ResumeLayout(false);
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moodPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.ComponentResourceManager resources;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.WebBrowser webBrowserBody;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton ButtonPublish;
        private System.Windows.Forms.ToolStripButton ButtonBold;
        private System.Windows.Forms.ToolStripButton ButtonItalic;
        private System.Windows.Forms.ToolStripButton ButtonUnderline;
        private System.Windows.Forms.ToolStripButton ButtonStrikethrough;
        private System.Windows.Forms.ToolStripButton ButtonQuote;
        private System.Windows.Forms.ToolStripButton ButtonHeading1;
        private System.Windows.Forms.ToolStripButton ButtonHeading2;
        private System.Windows.Forms.ToolStripButton ButtonHeading3;
        private System.Windows.Forms.ToolStripButton ButtonHeading4;
        private System.Windows.Forms.ToolStripButton ButtonLink;
        private System.Windows.Forms.ToolStripButton ButtonPicture;
        private System.Windows.Forms.ToolStripButton ButtonUndo;
        private System.Windows.Forms.ToolStripButton ButtonRepeat;
        private System.Windows.Forms.PictureBox moodPicture;
        private System.Windows.Forms.ToolStripButton ButtonList;
        private System.Windows.Forms.ToolStripButton ButtonNumbers;
        private System.Windows.Forms.ToolStripButton ButtonLine;


    }
}
