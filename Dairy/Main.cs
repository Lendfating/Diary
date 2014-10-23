using System;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Diary
{
    public partial class Main : Form
    {
        private String resource_path = "";// 资源路径
        private String file_path = "";// 文件路径
        public Main()
        {
            loadSetting();
            InitializeComponent();
        }

        private void loadSetting()
        {
            this.resource_path = System.Configuration.ConfigurationManager.AppSettings["resourcePath"];
            this.file_path = System.Configuration.ConfigurationManager.AppSettings["filePath"];  
        }

        private void openFile(string date, Boolean is_today)
        {
            if (this.htmlTextBox1.Editable)
            {
                this.htmlTextBox1.SaveEditor();
            }
            this.htmlTextBox1.OpenEditor(date, is_today);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.htmlTextBox1.Editable)
            {
                this.htmlTextBox1.SaveEditor();
            }
        }
    }
}
