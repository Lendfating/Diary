using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义控件库
{
    //声明函数指针的类型
    public delegate void MsgHandler(string date, Boolean is_today);

    public partial class CalendarBox : UserControl
    {
        private String file_path = "";// 文件路径
        private Calendar calendar;
        private MonthInfo month_info;
        //函数指针
        public MsgHandler clickDateHandler;

        public CalendarBox()
        {
            calendar = new Calendar();
            InitializeComponent();

            month_info = calendar.GetVisibleDates();
            fillMonthView(month_info);
            if (this.clickDateHandler != null)
            {
                this.clickDateHandler(DateTime.Now.Date.ToString("yyyy-MM-dd"), true);
            }
        }

        public CalendarBox(String file_path)
        {
            this.file_path = file_path;

            calendar = new Calendar();
            InitializeComponent();

            month_info = calendar.GetVisibleDates();
            fillMonthView(month_info);
            if (this.clickDateHandler != null)
            {
                this.clickDateHandler(DateTime.Now.Date.ToString("yyyy-MM-dd"), true);
            }
        }


        private void back_to_today(object sender, EventArgs e)
        {
            this.cal_dates[selected_index].BorderStyle = BorderStyle.None;

            month_info = calendar.GetVisibleDates();
            fillMonthView(month_info);
            for (selected_index = 0; selected_index < 42; selected_index++)
            {
                if (month_info.dates[selected_index].solar_date.Equals(DateTime.Now.Date))
                {
                    break;
                }
            }
            // 选中日期
            this.cal_dates[selected_index].BorderStyle = BorderStyle.FixedSingle;
            if (this.clickDateHandler != null)
            {
                this.clickDateHandler(DateTime.Now.Date.ToString("yyyy-MM-dd"), true);
            }
        }

        private int selected_index = 0;
        private void goto_someday(object sender, EventArgs e)
        {
            this.cal_dates[selected_index].BorderStyle = BorderStyle.None;

            selected_index = (int)(((OpaqueLayer)sender).Tag);
            DateInfo date_info = month_info.dates[selected_index];
            if (date_info.solar_date.CompareTo(DateTime.Now.Date) > 0)
            {// 比当前时间晚
                return;
            }

            if (date_info.is_overflow)
            { // 不在本月，直接切换视图。并更新selected_index
                month_info = calendar.GetVisibleDates(date_info.solar_date);
                fillMonthView(month_info);
                for (selected_index = 0; selected_index < 42; selected_index++)
                {
                    if (month_info.dates[selected_index].solar_date.Equals(date_info.solar_date))
                    {
                        break;
                    }
                }
            }
            // 选中日期
            this.cal_dates[selected_index].BorderStyle = BorderStyle.FixedSingle;
            if (this.clickDateHandler != null)
            {
                this.clickDateHandler(date_info.solar_date.ToString("yyyy-MM-dd"), date_info.is_today);
            }

        }
        
        private void goto_prev_month(object sender, EventArgs e)
        {
            this.cal_dates[selected_index].BorderStyle = BorderStyle.None;

            month_info = calendar.PrevMonthDates();
            fillMonthView(month_info);
        }

        private void goto_next_month(object sender, EventArgs e)
        {
            this.cal_dates[selected_index].BorderStyle = BorderStyle.None;

            if (month_info.month.CompareTo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)) >= 0)
            {// 不允许浏览此月之后的月份
                return;
            }
            month_info = calendar.NextMonthDates();
            fillMonthView(month_info);
        }

        private void fillMonthView(MonthInfo month_info)
        {
            this.current_month.Text = month_info.month.ToString("yyyy年M月");
            for (int i = 0; i < 42; i++)
            {
                cal_solar_dates[i].Text = "" + month_info.dates[i].solar_date.Day;
                cal_lunar_dates[i].Text = month_info.dates[i].info;
                if (month_info.dates[i].is_festival)
                { // 节日，改变信息颜色
                    cal_lunar_dates[i].ForeColor = System.Drawing.Color.Green;
                }
                else
                { // 非节日，采用默认颜色
                    cal_lunar_dates[i].ForeColor = System.Drawing.Color.Black;
                }
                if (month_info.dates[i].is_overflow)
                { // 非有效日期
                    this.cal_opaque_layers[i].Alpha = 180;
                }
                else
                {
                    this.cal_opaque_layers[i].Alpha = 0;
                }
                if ((i + 1) % 7 < 2)
                { // 周末
                    cal_solar_dates[i].ForeColor = System.Drawing.Color.Orange;
                    cal_lunar_dates[i].ForeColor = System.Drawing.Color.Orange;
                }

                this.cal_dates[i].BackgroundImage = null;
                if (System.IO.File.Exists(string.Format(@"{0}\{1}.md", file_path, month_info.dates[i].solar_date.ToString("yyyy-MM-dd"))))
                {
                    this.cal_dates[i].BackgroundImage = (Image)(resources.GetObject("Rest"));
                }
                if (month_info.dates[i].is_today)
                {// 今天，背景图片
                    System.ComponentModel.ComponentResourceManager temp = resources;
                    this.cal_dates[i].BackgroundImage = (Image)(resources.GetObject("Today"));
                }
                this.cal_opaque_layers[i].Tag = i;
            }
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            if (((Panel)sender).BorderStyle == BorderStyle.FixedSingle)
            {
                int thickness = 2;//it's up to you
                int halfThickness = thickness / 2;
                using (Pen p = new Pen(Color.ForestGreen, thickness))
                {
                    e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                                                              halfThickness,
                                                              ((Panel)sender).ClientSize.Width - thickness,
                                                              ((Panel)sender).ClientSize.Height - thickness));
                }
            }
        }

        [Category("CalendarBox"), Description("设置文件保存路径")]
        public String FilePath
        {
            get { return this.file_path; }
            set
            {
                this.file_path = value;
                this.Invalidate();
            }
        }
    }
}
