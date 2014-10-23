using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 自定义控件库
{
    /// <summary>
    /// 用于界面层与Calendar处理函数之间传递数据的结构体
    /// </summary>
    struct DateInfo
    {
        public DateTime solar_date;     // 阳历信息
        public LunarDate lunar_date;    // 农历信息
        public String info;     // 农历日期显示区的显示信息
        public bool is_festival;// 当前日期是否是特殊节日，用以帮助界面显示层据此特殊强调显示
        public bool is_overflow;// 当前日期是否是当前活跃月之外的日期，显示界面可以据此控制显示格式
        public bool is_today;   // 是否为今天
    }

    struct MonthInfo
    {
        public DateTime month;
        public DateInfo[] dates;
        public MonthInfo(int length)
            : this(System.DateTime.Now.StartOfMonth(), length)
        {
        }
        public MonthInfo(DateTime month, int length)
        {
            this.month = month;
            dates = new DateInfo[length];
        }
    }

    #region 阳历日历类定义(对系统DateTime结构进行扩展)
    /// <summary>
    /// 通过扩展方法, 对系统自带的DateTime类的方法进行扩展，添加一些新功能
    /// </summary>
    static class SolarDate
    {
        // 获取本周的开始日期
        public static DateTime StartOfWeek(this DateTime date)
        {
            return date.AddDays(-1 * Convert.ToInt16(date.DayOfWeek));
        }

        // 获取本月的开始日期
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        // 判断该日是否是节日，如果是的话，在参数festival_name中包含节日内容
        public static bool Festival(this DateTime date, ref String festival_name)
        {
            // 一般格式的阳历节日
            String format_date = "" + date.Month + "-" + date.Day;
            switch (format_date)
            {
                case "1-1": festival_name = "元旦"; return true;
                case "3-12": festival_name = "植树节"; return true;
                case "3-15": festival_name = "消费日"; return true;
                case "4-1": festival_name = "愚人节"; return true;
                case "9-10": festival_name = "教师节"; return true;
                case "10-1": festival_name = "国庆节"; return true;
                case "12-24": festival_name = "平安夜"; return true;
            }

            // 某月第几个星期几格式的阳历节日      格式：月份-星期几-第几个
            format_date = "" + date.Month + "-" + Convert.ToInt16(date.DayOfWeek) + "-" + Math.Ceiling(date.Day / 7.0);
            switch (format_date)
            {
                case "5-0-2": festival_name = "母亲节"; return true;    // 5月第2个星期日
                case "6-0-3": festival_name = "父亲节"; return true;    // 6月第3个星期日
                case "11-4-4": festival_name = "感恩节"; return true;    // 11月第4个星期四
            }

            // 全部检查完没有匹配项，则表示此日不是节日
            return false;
        }
    }
    #endregion

    #region 农历日历类
    class LunarDate
    {
        // 这个是采用记录每月数据的方式来进行阳历、农历转换的，更妙的转换方法参考：http://blog.csdn.net/orbit/article/details/9210413
        #region 阴历数据
        // 阳历，每月的起始天数
        private static int[] days_array = new int[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
        // 农历每年的大小月及闰月数据，数据结构定义如下，
        // 从后往前，前0~12位依次表示第1~13个月，0表示29天（小月）   1表示30天（大月）
        // 第16-19位(共四位)表示闰月是哪个月，如果没有闰月则该位为0
        private static int[] data_array = new int[] { 0x4152B, 0x52B, 0xA5B, 0x2155A, 0x56A, 0x71B55, 0xBA4, 0xB49, 0x51A93, 0xA95, 0x52D, 0x40AAD, 0xAB5, 0x915AA, 0x5D2, 0xDA5, 0x61D4A, 0xD4A, 0xC95, 0x4152E, 0x556, 0xAB5, 0x215B2, 0x6D2, 0x60EA5, 0x725, 0x64B, 0x50C97, 0xCAB, 0x55A, 0x30AD6, 0xB69, 0x71752, 0xB52, 0xB25, 0x61A4B, 0xA4B, 0x4AB, 0x5055B, 0x5AD, 0xB6A, 0x21B52, 0xD92, 0x71D25, 0xD25, 0xA55, 0x5148D, 0x4B6, 0x5B5, 0x30FA2 };
        private static String TG_array = "甲乙丙丁戊己庚辛壬癸"; // 天干
        private static String DZ_array = "子丑寅卯辰巳午未申酉戌亥"; // 地支
        private static String SX_array = "鼠牛虎兔龙蛇马羊猴鸡狗猪"; // 生肖
        private static String RQ_array = "一二三四五六七八九十"; // 日期
        private static String YF_array = "正二三四五六七八九十冬腊"; // 月份
        #endregion

        private int year;
        private int month;
        private int day;

        public String TianGan;
        public String DiZhi;
        public String ShengXiao;
        public String YueFen;
        public String RiQi;

        // month是大月还是小月 1表示大月，0表示小月
        private int GetBit(int index, int month)
        {
            return (data_array[index] >> month) & 1;
        }

        public LunarDate(DateTime date)
        {
            int total, y, m, k;
            // 2001年1月23是除夕;该句计算到起始年正月初一的天数
            total = (date.Year - 2001) * 365 + (date.Year - 2001) / 4 + days_array[date.Month - 1] + date.Day - 23;
            if (date.Year % 4 == 0 && date.Month > 2)
            {
                total++;// 当年是闰年且已过2月再加一天！
            }

            // 从2001年1月23日开始，按农历的方式寻找当前日期对应的农历日期
            for (y = 0; ; y++)// 年
            {
                k = (data_array[y] < 0xfff) ? 12 : 13;    //(2001+m)年是闰月吗？
                for (m = 0; m < k; m++)// 月
                {
                    if (total <= 29 + GetBit(y, m))
                    { //找到该农历日！
                        #region 找到对应农历日子，据此设置变量
                        year = 2001 + y;    // 农历年
                        month = m + 1;      // 农历月
                        day = total;        // 农历日
                        if (k == 13 && (month >= (data_array[y] >> 16) + 1))
                        { // 闰年闰月之后的月！
                            month--; //闰月及以后月号要减一！
                        }
                        TianGan = "" + TG_array.ElementAt((year - 4) % 10); //天干
                        DiZhi = "" + DZ_array.ElementAt((year - 4) % 12); //地支
                        ShengXiao = "" + SX_array.ElementAt((year - 4) % 12); //生肖
                        YueFen = (k == 13 && (m == (data_array[y] >> 16)) ? "闰" : "") + YF_array.ElementAt(month - 1);//月份
                        RiQi = (day < 11) ? "初" : ((day < 20) ? "十" : ((day == 20) ? "二十" : ((day < 30) ? "廿" : "三十")));
                        if (day % 10 != 0 || day == 10)
                        {
                            RiQi += RQ_array.ElementAt((day - 1) % 10);
                        }
                        #endregion
                        return;
                    }
                    total = total - 29 - GetBit(y, m); //寻找农历年！
                }
            }
        }

        public String ToString()
        {
            if (day == 1)
            {
                return YueFen + "月";
            }
            else
            {
                return RiQi;
            }
        }

        // 判断该日是否是节日，如果是的话，在参数festival_name中包含节日内容
        public bool Festival(ref String festival_name)
        {
            // 一般格式的阳历节日
            String format_date = "" + month + "-" + day;
            switch (format_date)
            {
                case "1-1": festival_name = "春节"; return true;
                case "1-15": festival_name = "元宵节"; return true;
                case "4-9": festival_name = "生日"; return true;
                case "5-5": festival_name = "端午节"; return true;
                case "8-15": festival_name = "中秋节"; return true;
                case "9-9": festival_name = "重阳节"; return true;
                case "10-18": festival_name = "妈生日"; return true;
                case "11-15": festival_name = "姐生日"; return true;
                case "12-8": festival_name = "腊八节"; return true;
                case "12-17": festival_name = "哥生日"; return true;
                case "12-23": festival_name = "小年"; return true;
            }
            // 农历月底的节日
            format_date = "" + month + "-" + (day + GetBit(year - 2001, month));
            switch (format_date)
            {
                case "9-30": festival_name = "爸生日"; return true;
                case "12-30": festival_name = "除夕"; return true;
            }

            // 全部检查完没有匹配项，则表示此日不是节日
            return false;
        }
    }
    #endregion

    class Calendar
    {
        // 日历基本属性
        private DateTime active_month;  // 活跃月份，即当前显示的月份（月份采用‘X月1号’进行记录）

        // 获取指定月份的所有可见日期信息
        public MonthInfo GetVisibleDates()
        {// 默认当前的数据
            return GetVisibleDates(System.DateTime.Now);
        }

        public MonthInfo GetVisibleDates(DateTime new_date)
        {
            active_month = new_date.StartOfMonth();   // 强制使用该月1号作为活跃月份记录
            DateTime today = System.DateTime.Now.Date;
            DateTime date = active_month.StartOfWeek();
            MonthInfo result = new MonthInfo(active_month, 42);
            for (int i=0;i<42;i++)
            {
                result.dates[i].solar_date = date;
                result.dates[i].lunar_date = new LunarDate(date);
                //result[i].str_date = date.ToString("yyyy-MM-dd HH:mm:ss");
                if (date.Month != active_month.Month)
                {
                    result.dates[i].is_overflow = true;
                }
                result.dates[i].info = result.dates[i].lunar_date.ToString(); // 读取有用信息
                if (result.dates[i].lunar_date.Festival(ref result.dates[i].info) || date.Festival(ref result.dates[i].info))
                { // 如果是节日，则会自动覆盖
                    result.dates[i].is_festival = true;
                }
                if (date.Equals(today))
                {
                    result.dates[i].is_today = true;
                }

                date = date.AddDays(1); // 循环进行，下一天
            }
            return result;
        }

        public MonthInfo NextMonthDates()
        {
            return GetVisibleDates(this.active_month.AddMonths(1));
        }

        public MonthInfo PrevMonthDates()
        {
            return GetVisibleDates(this.active_month.AddMonths(-1));
        }

    }
}
