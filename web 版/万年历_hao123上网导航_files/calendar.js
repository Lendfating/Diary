/**********************************************************************
 * 文件名：calendar.js
 * 描  述：日历控制
 * *******************************************************************/

window.calendar = { version: "1.0.0" };

/* *
 * 初始化操作，初始化基本视图框架和全局变量，方便之后的页面更新
 */
calendar.init=function(id){
	// 进行环境初始化操作
	this._obj=(typeof id == "string")?document.getElementById(id):id;
	
	this.active_date = null;	// 活跃(选中)日期
	this.active_month = null;	// 活跃月份，即当前正在显示的月份
		
	this.get_elements();
	this.set_actions();
	
	// 显示当前月份的视图
	this.setMonthView(this._currentDate());
	
	// 定时更新时间
	setInterval(function(){
		var now = new Date();
		var hour = (now.getHours()<10?'0':'')+now.getHours();
		var minute = (now.getMinutes()<10?'0':'')+now.getMinutes();
		var second = (now.getSeconds()<10?'0':'')+now.getSeconds();
		calendar._els['time'][0].innerHTML = ""+hour+":"+minute+":"+second;
	},1000);
};

calendar.get_elements=function(){
	this._els=[];
	//get all child elements as named hash
	var els=this._obj.getElementsByTagName("DIV");//将该区域的所有元素全部添入
	for (var i=0; i < els.length; i++){
		var name=els[i].className;
		if (name) name = name.split(" ")[0];
		if (!this._els[name]) this._els[name]=[];
		this._els[name].push(els[i]);
	}
};

/***
 * 各种鼠标触发适当的事件
 */
calendar.set_actions=function(){
	// 添加点击事件
	for (var a in this._els)
		if (this._click[a])
			for (var i=0; i < this._els[a].length; i++)
				this._els[a][i].onclick=calendar._click[a];

	// 完善下拉菜单响应功能
	lis = this._els['h_calendar_select'][0].getElementsByTagName("li");//获取所有的下拉项
	for (var i in lis) {
		lis[i].onclick=function(e){
			var src = e.target||e.srcElement;
			var update_date = calendar.date.copy(calendar.active_month);
			if (src.getAttribute("val")>100) // 年
				update_date.setYear(src.getAttribute("val"));
			else							// 月
				update_date.setMonth(src.getAttribute("val"));
			calendar.setMonthView(update_date);
		}
	}
	
	// 日历主区域的响应事件
	this._els['h_calendar_list'][0].onclick=function(e){
		var src = e.target||e.srcElement;
		while (src.tagName!='LI'&&src.parentNode)
			src = src.parentNode;
 
		// 替换上一个标志
		src.className += "on ";
		if (calendar.active_date && calendar.active_date.className){
			calendar.active_date.classList.remove('on');
		}
		calendar.active_date = src;
		var date = src.getAttribute('date');
		if (src.classList.contains('nextbefor')){
			// 非当前有效期的日子
			calendar.setMonthView(new Date(date));
		}
		alert(src.getAttribute('date'));
	};
};

selectBox = {
	obj: null,
	tag: false,
	init: function(src){
		if (this.obj!=null) this.close();
		this.obj = src;
		this.open();
	},
	open: function(){
		this.obj.style.display = "block";
		document.onclick = this.close;
		this.tag = true;
	},
	close: function(){
		if (!selectBox.tag){
			selectBox.obj.style.display = "none";
			document.onclick = null;
			selectBox.obj = null;
		}
		selectBox.tag = false;
	}
};

calendar._click={
	prev_year_btn:function(){
		calendar.setMonthView(calendar.date.add(calendar.active_month,-1,"year"));
	},
	select_year_box:function(e){
		var src = e.target||e.srcElement;
		selectBox.init(src.nextSibling.nextSibling);
	},
	next_year_btn:function(){
		calendar.setMonthView(calendar.date.add(calendar.active_month, 1,"year"));
	},
	prev_month_btn:function(){
		calendar.setMonthView(calendar.date.add(calendar.active_month,-1,"month"));
	},
	select_month_box:function(e){
		var src = e.target||e.srcElement;
		selectBox.init(src.nextSibling.nextSibling);
	},
	next_month_btn:function(dummy,step){
		calendar.setMonthView(calendar.date.add(calendar.active_month, 1,"month"));
	},
	return_today_btn:function(){
		calendar.setMonthView(calendar._currentDate());
	}
};

/***
 * 更新显示视图操作
 */
calendar.setMonthView = function(month) {
	month = this.date.month_start(month);	// 月份设置成某月一号的格式
	this.active_month = month;
	// 更新月份显示信息
	var year_view_box = this._els['select_year_box'][0].getElementsByTagName("h4")[0];
	var month_view_box = this._els['select_month_box'][0].getElementsByTagName("h4")[0];
	year_view_box.setAttribute("val",this.active_month.getFullYear());
	year_view_box.innerHTML = ''+this.active_month.getFullYear()+'年';
	month_view_box.setAttribute("val",this.active_month.getMonth()+1);
	month_view_box.innerHTML = ''+(this.active_month.getMonth()+1)+'月';
	
	// 更新该月详细日历信息
	var date = this.date.week_start(this.active_month);
	var current_date = this._currentDate();
	var start_date = this.active_month;
	var end_date = this.date.add(this.active_month, 1,"month")
	
	this._els['h_calendar_list'][0].innerHTML = "";
	var ul = document.createElement("UL");
	ul.className = "e_clear";
	for (var i=0; i<42; i++){
		var li = document.createElement("LI");
		li.setAttribute('date', calendar.date.date_to_str(date));
		if (date.valueOf() < start_date.valueOf() || date.valueOf() >= end_date.valueOf())
			li.className = 'nextbefor ';
		if (this.date.isWeekend(date))
			li.className += 'weekend ';
		if (date.valueOf() == current_date.valueOf())
			li.className += 'today ';
		if (calendar.active_date && date.valueOf() == (new Date(calendar.active_date.getAttribute('date'))).valueOf()){
			li.className += 'on ';
			calendar.active_date = li;
		}
			
		date_detail_info = calendar.date.detail_info(date);
		if (date_detail_info['festival']){
			li.className += 'isolar ';
			li.innerHTML = '<span class="border"></span><div class="solar_date">'+date.getDate()+'</div><div class="lunar_date">'+date_detail_info['festival']+'</div>';
		} else {
			li.innerHTML = '<span class="border"></span><div class="solar_date">'+date.getDate()+'</div><div class="lunar_date">'+date_detail_info['lunar_date']+'</div>';
		}
		ul.appendChild(li);
		date = calendar.date.add(date,1,"day");
	}
	this._els['h_calendar_list'][0].appendChild(ul);
};

calendar._currentDate = function(){
	// 返回当前时间的日期部分
	return calendar.date.date_part(new Date());
};

calendar.date={
	date_part:function(date){
		date.setHours(0);
		date.setMinutes(0);
		date.setSeconds(0);
		date.setMilliseconds(0);
		if (date.getHours() != 0)
			date.setTime(date.getTime() + 60 * 60 * 1000 * (24 - date.getHours()));
		return date;
	},
	day_start:function(date){
		return this.date_part(date);
	},
	week_start:function(date){
		var shift = date.getDay();
		return this.date_part(this.add(date,-1*shift,"day"));
	},
	month_start:function(date){
		var ndate=new Date(date);
		ndate.setDate(1);
		return this.date_part(ndate);
	},
	add:function(date,inc,mode){
		var ndate=new Date(date);
		switch(mode){
			case "week":
				inc *= 7;
			case "day":
				ndate.setDate(ndate.getDate() + inc);
				if (!date.getHours() && ndate.getHours()) //shift to yesterday
					ndate.setTime(ndate.getTime() + 60 * 60 * 1000 * (24 - ndate.getHours()));
				break;
			case "month": ndate.setMonth(ndate.getMonth()+inc); break;
			case "year": ndate.setYear(ndate.getFullYear()+inc); break;
		}
		return ndate;
	},
	isWeekend:function(date){
		return date.getDay()==0 || date.getDay()==6;
	},
	copy:function(date){
		return new Date(date);
	},
	festival:function(solar_date, lunar_date){
		// 阳历一般节日
		var format_date = ''+(solar_date.getMonth()+1)+'-'+solar_date.getDate();
		switch(format_date){
			case '1-1': return "元旦";
			case '3-12': return "植树节";
			case '3-15': return "消费日";
			case '4-1': return "愚人节";
			case '9-10': return "教师节";
			case '10-1': return "国庆节";
			case '12-24': return "平安夜";
		}
		// 阳历第几个星期日性质的节日
		format_date = ''+(solar_date.getMonth()+1)+'-'+solar_date.getDay()+'-'+Math.ceil(solar_date.getDate()/7);
		switch(format_date){
			case '5-0-2': return "母亲节"; // 5月第2个星期日
			case '6-0-3': return "父亲节"; // 6月第3个星期日
		}
		// 农历节日
		format_date = ''+lunar_date.cDate.Month+'-'+lunar_date.cDate.Day;
		switch(format_date){
			case '1-1': return "春节";
			case '1-15': return "元宵节";
			case '4-9': return "生日";
			case '5-5': return "端午节";
			case '8-15': return "中秋节";
			case '9-9': return "重阳节";
			case '9-30': return "爸生日";
			case '10-18': return "妈生日";
			case '11-15': return "姐生日";
			case '12-17': return "哥生日";
			case '12-8': return "腊八节";
			case '12-23': return "小年";
			case '12-29': 
				// 如果是小月的话，也要输出“除夕”
				if (!lunar_date.getBit(lunar_date.cDate.Year-2001, lunar_date.cDate.Month))
					break;	// 大月，退出
			case '12-30': return "除夕";
			default: return null;
		}
	},
	detail_info: function(date){
		// 阳历转农历
		var lunar_date = new LunarDate(date);
		var festival = calendar.date.festival(date, lunar_date);
		return {'lunar_date': lunar_date.cDate, 'festival':festival};
	},
	solar_to_lunar:function(date){
		// 阳历转农历
		var lunar_date = new LunarDate(date);
		var festival = calendar.date.festival(date, lunar_date);
		if (festival!=null)
			return festival;
		else
			return lunar_date.cDate;
	},
	date_to_str:function(date){
		return ""+date.getFullYear()+"-"+(date.getMonth()+1)+"-"+date.getDate();
	},
	str_to_date:function(str){
		return new Date(str);
	}
};


/*!
阳历转阴历
kael 2012-05-30
 */
var LunarDate = function(solar_date) {
	this.date = solar_date ? solar_date : new Date();
	this.cDate = {
		toString : function () {
			if (this.Day == 1)
				return this.yf + '月';
			else
				return this.rq;
		}
	};
	this.init();
	this.calc();
};

LunarDate.prototype = {
	constructor : LunarDate,
	cDays : [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334],
	cData : [0x41A95, 0xD4A, 0xDA5, 0x20B55, 0x56A, 0x7155B, 0x25D, 0x92D, 0x5192B, 0xA95, 0xB4A, 0x416AA, 0xAD5, 0x90AB5, 0x4BA, 0xA5B, 0x60A57, 0x52B, 0xA93, 0x40E95],
	CSTR : {
		TG : "甲乙丙丁戊己庚辛壬癸", // 天干
		DZ : "子丑寅卯辰巳午未申酉戌亥", // 地支
		SX : "鼠牛虎兔龙蛇马羊猴鸡狗猪", // 生肖
		RQ : "一二三四五六七八九十", // 日期
		YF : "正二三四五六七八九十冬腊", // 月份
	},
	// month是大月还是小月
	getBit : function (index, month) {
		return (this.cData[index] >> month) & 1;
	},
	// 初始化
	init : function () {
		var total, m, n, k;
		var isEnd = false;
		var tmp = this.date.getFullYear();
		total = (tmp - 2001) * 365
		 + Math.floor((tmp - 2001) / 4)
		 + this.cDays[this.date.getMonth()]
		 + this.date.getDate() - 23; // 2001年1月23是除夕;该句计算到起始年正月初一的天数
		if (this.date.getYear() % 4 == 0 && this.date.getMonth() > 1) {
			total++; // 当年是闰年且已过2月再加一天！
		}
		for (m = 0; ; m++) {
			k = (this.cData[m] < 0xfff) ? 11 : 12; //起始年+m闰月吗？
			for (n = k; n >= 0; n--) {
				if (total <= 29 + this.getBit(m, n)) { //已找到农历年!
					isEnd = true;
					break;
				}
				total = total - 29 - this.getBit(m, n); //寻找农历年！
			}
			if (isEnd) {
				break;
			}
		}
		this.cDate.Year = 2001 + m; //农历年
		this.cDate.Month = k - n + 1; //农历月
		this.cDate.Day = total; //农历日
		if (k == 12) { //闰年！
			if (this.cDate.Month == Math.floor(this.cData[m] / 0x10000) + 1) { //该月就是闰月！
				this.cDate.Month = 1 - this.cDate.Month;
			}
			if (this.cDate.Month > Math.floor(this.cData[m] / 0x10000) + 1) {
				this.cDate.Month--; //该月是闰月后某个月！
			}
		}
		this.cDate.Hour = Math.floor((this.date.getHours() + 1) / 2);
	},
	// 计算
	calc : function () {
		var year = this.cDate.Year - 4;
		this.cDate.tg = this.CSTR.TG.charAt(year % 10); //天干
		this.cDate.dz = this.CSTR.DZ.charAt(year % 12); //地支
		this.cDate.sx = this.CSTR.SX.charAt(year % 12); //生肖
		this.cDate.yf = this.CSTR.YF.charAt(this.cDate.Month - 1);
		if (this.cDate.Month < 1) {
			this.cDate.yf = "闰" + this.CSTR.YF.charAt(-this.cDate.Month - 1);
		}
		this.cDate.rq = (this.cDate.Day < 11) ? "初" : ((this.cDate.Day < 20) ? "十" : ((this.cDate.Day == 20) ? "二十" : ((this.cDate.Day < 30) ? "廿" : "三十")));
		if (this.cDate.Day % 10 != 0 || this.cDate.Day == 10) {
			this.cDate.rq += this.CSTR.RQ.charAt((this.cDate.Day - 1) % 10);
		}
		this.cDate.sj = this.CSTR.DZ.charAt((this.cDate.Hour) % 12) + "时";
		if (this.cDate.Hour == 12) {
			this.cDate.sj = "夜" + this.cDate.sj;
		}
	}
};
