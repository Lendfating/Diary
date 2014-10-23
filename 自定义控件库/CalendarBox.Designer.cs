namespace 自定义控件库
{
    partial class CalendarBox
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
            components = new System.ComponentModel.Container();
            resources = new System.ComponentModel.ComponentResourceManager(typeof(CalendarBox));
            this.calendarPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cal_weekPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cal_listPanel = new System.Windows.Forms.FlowLayoutPanel();
            for (int i = 0; i < 7; i++)
            {
                this.cal_weeks[i] = new System.Windows.Forms.Label();
            }
            for (int i = 0; i < 42; i++)
            {
                this.cal_dates[i] = new System.Windows.Forms.Panel();
                this.cal_solar_dates[i] = new System.Windows.Forms.Label();
                this.cal_lunar_dates[i] = new System.Windows.Forms.Label();
                this.cal_opaque_layers[i] = new OpaqueLayer();
                this.cal_dates[i].SuspendLayout();
            }
            this.prev_month = new System.Windows.Forms.Button();
            this.current_month = new System.Windows.Forms.Label();
            this.next_month = new System.Windows.Forms.Button();
            this.today_panel = new System.Windows.Forms.Panel();
            this.goto_today = new System.Windows.Forms.Button();
            this.today_date = new System.Windows.Forms.Label();
            this.calendarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // calendarPanel 日历基面板初始化
            // 
            this.calendarPanel.Controls.Add(this.prev_month);
            this.calendarPanel.Controls.Add(this.current_month);
            this.calendarPanel.Controls.Add(this.next_month);
            this.calendarPanel.Controls.Add(this.cal_weekPanel);
            this.calendarPanel.Controls.Add(this.cal_listPanel);
            this.calendarPanel.Controls.Add(this.today_panel);
            this.calendarPanel.Location = new System.Drawing.Point(12, 24);
            this.calendarPanel.Size = new System.Drawing.Size(468, 425);
            this.calendarPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // prev_month
            // 
            this.prev_month.Cursor = System.Windows.Forms.Cursors.Hand;
            this.prev_month.FlatAppearance.BorderSize = 0;
            this.prev_month.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prev_month.Font = new System.Drawing.Font("华文琥珀", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.prev_month.ForeColor = System.Drawing.Color.DarkGreen;
            this.prev_month.Size = new System.Drawing.Size(75, 45);
            this.prev_month.Text = "《";
            this.prev_month.Click += new System.EventHandler(this.goto_prev_month);
            // 
            // current_month
            // 
            this.current_month.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.current_month.ForeColor = System.Drawing.Color.BlueViolet;
            this.current_month.Size = new System.Drawing.Size(298, 50);
            //this.current_month.Text = "2014年12月";
            this.current_month.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // next_month
            // 
            this.next_month.Cursor = System.Windows.Forms.Cursors.Hand;
            this.next_month.FlatAppearance.BorderSize = 0;
            this.next_month.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.next_month.Font = new System.Drawing.Font("华文琥珀", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.next_month.ForeColor = System.Drawing.Color.DarkGreen;
            this.next_month.Size = new System.Drawing.Size(75, 45);
            this.next_month.Text = "》";
            this.next_month.Click += new System.EventHandler(this.goto_next_month);
            // 
            // cal_weekPanel 标题显示面板
            // 
            string[] xq  = {"星期日","星期一","星期二","星期三","星期四","星期五","星期六"};
            for (int i = 0; i < 7; i++)
            {
                this.cal_weekPanel.Controls.Add(this.cal_weeks[i]);
                // 
                // cal_weeks
                // 
                this.cal_weeks[i].BackColor = System.Drawing.Color.Transparent;
                this.cal_weeks[i].Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(134)));
                if ((i + 1) % 7 < 2)
                { // 周末
                    this.cal_weeks[i].ForeColor = System.Drawing.Color.Orange;
                }
                this.cal_weeks[i].Location = new System.Drawing.Point(3, 0);
                this.cal_weeks[i].Size = new System.Drawing.Size(60, 24);
                this.cal_weeks[i].Text = xq[i];
                this.cal_weeks[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            this.cal_weekPanel.Location = new System.Drawing.Point(3, 3);
            this.cal_weekPanel.Size = new System.Drawing.Size(462, 26);
            //this.cal_weekPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // cal_listPanel 日期显示面板
            // 
            for (int i = 0; i < 42; i++)
            {
                // date
                this.cal_listPanel.Controls.Add(this.cal_dates[i]);
                // date
                //this.cal_dates[i].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.cal_dates[i].Controls.Add(this.cal_solar_dates[i]);
                this.cal_dates[i].Controls.Add(this.cal_lunar_dates[i]);
                this.cal_dates[i].Controls.Add(this.cal_opaque_layers[i]);
                this.cal_dates[i].Cursor = System.Windows.Forms.Cursors.Hand;
                this.cal_dates[i].Size = new System.Drawing.Size(60, 41);
                this.cal_dates[i].Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
                // 
                // label1
                // 
                this.cal_solar_dates[i].BackColor = System.Drawing.Color.Transparent;
                this.cal_solar_dates[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(134)));
                this.cal_solar_dates[i].TextAlign = System.Drawing.ContentAlignment.TopRight;
                this.cal_solar_dates[i].Location = new System.Drawing.Point(19, -2); ;
                this.cal_solar_dates[i].Size = new System.Drawing.Size(43, 25);
                // 
                // label2
                // 
                this.cal_lunar_dates[i].BackColor = System.Drawing.Color.Transparent;
                this.cal_lunar_dates[i].Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(134)));
                this.cal_lunar_dates[i].TextAlign = System.Drawing.ContentAlignment.TopRight;
                this.cal_lunar_dates[i].Location = new System.Drawing.Point(3, 23);
                this.cal_lunar_dates[i].Size = new System.Drawing.Size(54, 15);
                //
                // OpaqueLayer
                //
                this.cal_opaque_layers[i].Dock = System.Windows.Forms.DockStyle.Fill;
                this.cal_opaque_layers[i].BringToFront();
                this.cal_opaque_layers[i].Enabled = true;
                this.cal_opaque_layers[i].Visible = true;
                this.cal_opaque_layers[i].Click += new System.EventHandler(this.goto_someday);
            }
            this.cal_listPanel.Location = new System.Drawing.Point(3, 3);
            this.cal_listPanel.Size = new System.Drawing.Size(462, 283);
            //this.cal_listPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // today_panel
            // 
            this.today_panel.Controls.Add(this.today_date);
            this.today_panel.Controls.Add(this.goto_today);
            this.today_panel.Location = new System.Drawing.Point(3, 386);
            this.today_panel.Name = "today_panel";
            this.today_panel.Size = new System.Drawing.Size(462, 45);
            this.today_panel.TabIndex = 8;
            // 
            // goto_today
            // 
            this.goto_today.Cursor = System.Windows.Forms.Cursors.Hand;
            this.goto_today.Location = new System.Drawing.Point(85, 8);
            this.goto_today.Name = "goto_today";
            this.goto_today.Size = new System.Drawing.Size(60, 30);
            this.goto_today.TabIndex = 0;
            this.goto_today.UseVisualStyleBackColor = true;
            this.goto_today.Click += new System.EventHandler(this.back_to_today);
            // 
            // today_date
            // 
            this.today_date.AutoSize = true;
            this.today_date.Cursor = System.Windows.Forms.Cursors.Hand;
            this.today_date.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.today_date.Location = new System.Drawing.Point(150, 7);
            this.today_date.Name = "today_date";
            this.today_date.Size = new System.Drawing.Size(231, 31);
            this.today_date.TabIndex = 1;
            this.today_date.Text = "今天：" + System.DateTime.Now.ToString("M/d/yyyy");
            this.today_date.Click += new System.EventHandler(this.back_to_today);
            // 
            // Form1
            // 
            this.Controls.Add(this.calendarPanel);
            this.calendarPanel.ResumeLayout(false);
            for (int i = 0; i < 42; i++)
            {
                this.cal_dates[i].ResumeLayout(false);
                this.cal_dates[i].PerformLayout();
            }
            this.Size = new System.Drawing.Size(490, 460);
            this.ResumeLayout(false);
        }

        #endregion

        private System.ComponentModel.ComponentResourceManager resources;
        private System.Windows.Forms.FlowLayoutPanel calendarPanel;
        private System.Windows.Forms.FlowLayoutPanel cal_weekPanel;
        private System.Windows.Forms.FlowLayoutPanel cal_listPanel;
        private System.Windows.Forms.Label[] cal_weeks = new System.Windows.Forms.Label[7];
        private System.Windows.Forms.Panel[] cal_dates = new System.Windows.Forms.Panel[42];
        private System.Windows.Forms.Label[] cal_solar_dates = new System.Windows.Forms.Label[42];
        private System.Windows.Forms.Label[] cal_lunar_dates = new System.Windows.Forms.Label[42];
        private OpaqueLayer[] cal_opaque_layers = new OpaqueLayer[42];
        private System.Windows.Forms.Button prev_month;
        private System.Windows.Forms.Label current_month;
        private System.Windows.Forms.Button next_month;
        private System.Windows.Forms.Panel today_panel;
        private System.Windows.Forms.Button goto_today;
        private System.Windows.Forms.Label today_date;
    }
}
