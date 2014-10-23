namespace Diary
{
    partial class Main
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
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.calendarBox1 = new 自定义控件库.CalendarBox(file_path);
            this.htmlTextBox1 = new 自定义控件库.HtmlTextBox(resource_path, file_path);
            this.SuspendLayout();
            // 
            // calendarBox1
            // 
            this.calendarBox1.Location = new System.Drawing.Point(12, 12);
            this.calendarBox1.Name = "calendarBox1";
            this.calendarBox1.Size = new System.Drawing.Size(490, 460);
            this.calendarBox1.TabIndex = 1;
            this.calendarBox1.clickDateHandler = this.openFile;
            // 
            // htmlTextBox1
            // 
            this.htmlTextBox1.AutoSize = true;
            this.htmlTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.htmlTextBox1.BoxSize = new System.Drawing.Size(740, 680);
            this.htmlTextBox1.Editable = true;
            this.htmlTextBox1.Location = new System.Drawing.Point(549, 28);
            this.htmlTextBox1.MinimumSize = new System.Drawing.Size(625, 500);
            this.htmlTextBox1.Name = "htmlTextBox1";
            this.htmlTextBox1.Size = new System.Drawing.Size(740, 680);
            this.htmlTextBox1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1327, 584);
            this.Controls.Add(this.htmlTextBox1);
            this.Controls.Add(this.calendarBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1366, 728);
            this.MinimizeBox = false;
            this.Name = "Diary";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private 自定义控件库.CalendarBox calendarBox1;
        private 自定义控件库.HtmlTextBox htmlTextBox1;
    }
}

