/**********************************************************************
 * 文件名：editor.js
 * 描  述：文章读写、编辑、保存功能。
 * *******************************************************************/
window.editor = { version: "1.0.0" };

// 参考文献：http://zhaoningbo.iteye.com/blog/1697313
// http://www.yeebing.com/webdesign/webstudy/800.html


/* *
 * 初始化操作，初始化基本视图框架和全局变量，方便之后的页面更新
 */
editor.init=function(id){
	// 进行环境初始化操作
	this._obj = (typeof id == "string")?document.getElementById(id):id;
	this._title_obj = this._obj.childNodes[1];
	this._toolbar_obj = this._obj.childNodes[3];
	this._textarea_obj = this._obj.childNodes[5];
	
	this.mode = "reader";	// "reader"模式 or "editor"模式
			
	this.title.init();
	this.toolbar.init();
	this.editorArea.init();
	
};

editor.title={
	init:function(){
	}
};

editor.toolbar={
	init:function(){
		//初始化toolbar内的所有按钮，对应适当的操作
		var tools=editor._toolbar_obj.getElementsByTagName("a");
		for (var i=0; i < tools.length; i++){
			tools[i].onclick=editor.toolbar.click;
			tools[i].onmouseover=editor.toolbar.hover;
		}
	},
	hover:function(e){
		function getPosition(element){
			var pos = {top:element.offsetTop ,left:element.offsetLeft};
			var current = element.offsetParent;
			while (current !== null){
				pos.top += current.offsetTop;
				pos.left += current.offsetLeft;
				current = current.offsetParent;
			}
			return pos;
			}
		var src = e.target||e.srcElement;
		var tooltip = src.getAttribute('tooltip');
		if (tooltip==null) return ;
		
		var pos = getPosition(src);
		var div = document.createElement("DIV");
		div.className = "tooltip";
		div.style.top = ""+(pos.top-44)+"px";
		div.style.left = ""+(pos.left+(src.clientWidth-16-13*tooltip.length)/2)+"px";
		div.innerHTML = '<div class="tooltip-arrow"></div><div class="tooltip-inner">'+tooltip+'</div>';
		src.parentNode.appendChild(div);
		src.onmouseout = function(e){
			var src = e.target||e.srcElement;
			src.parentNode.removeChild(div);
			src.onmouseout = null;
		}
	},
	click:function(e){
		var src = e.target||e.srcElement;
		var type = src.getAttribute('tooltype'),
			optData=null; //附加属性值
			
		// 详细操作命令列表参见：https://developer.mozilla.org/zh-CN/docs/Web/API/document.execCommand
		switch(type){
			case "bold":type="bold";break;
			case "italic":type="italic";break;
			case "strikethrough":type="strikeThrough";break;
			case "underline":type="underline";break;
			case "list":type="insertUnorderedList";break;
			case "list-ol":type="insertOrderedList";break;
			case "quote":optData="blockquote";
						type="formatBlock";break;
			case "heading1":optData="H1";
						type="formatBlock";break;
			case "heading2":optData="H2";
						type="formatBlock";break;
			case "heading3":optData="H3";
						type="formatBlock";break;
			case "heading4":optData="H4";
						type="formatBlock";break;
			case "link":optData=window.prompt("输入链接","http://");
						type="createlink";break;
			case "picture":editor.editorArea.focus();//插入前必须让编辑区域获得焦点
						optData=window.prompt("输入图片url","http://");
						type="insertImage";break;
			case "undo":type="undo";break;
			case "repeat":type="redo";break;
			case "publish"://另存为只有IE支持,在新窗口打开,不然保存的是整个页面而不是编辑区域里面的内容
						var content=editor.editorArea.iframeDocument.body.innerHTML;
						var md = editor.translator.html2md(content);
						alert(md);
						fileName="fewrfew",
						openWindow=window.open("","");
						openWindow.document.write(content);
						openWindow.document.execCommand("saveas",fileName+".html");
						return ;
		}
		event.preventDefault();//阻止事件冒泡和默认事件
		editor.toolbar.exeCommand(type,optData);
		event.stopPropagation();
	},
	exeCommand:function(type,optData){  //执行命令,type为命令类型,optData为附加值
		if(editor.editorArea.iframeDocument.queryCommandEnabled(type)){ //测试是否可以执行
			editor.editorArea.iframeDocument.execCommand(type,false,optData); //第二参数默认为false
		}
	}
};

editor.editorArea={
	iframeDocument:null,
	init:function(){
		// 设置编辑区大小
		editor._textarea_obj.style.height=""+(document.body.clientHeight-100)+"px";
		window.onresize = function(){
			editor._textarea_obj.style.height=""+(document.body.clientHeight-94)+"px";
		}
		editor._textarea_obj.frameBorder=0;
		editor.editorArea.iframeDocument = editor._textarea_obj.contentDocument || editor._textarea_obj.contentWindow.document;
		editor.editorArea.iframeDocument.designMode = "on";
		editor.editorArea.iframeDocument.open();
		editor.editorArea.iframeDocument.write('<html><head></head><body></body></html>');
		editor.editorArea.iframeDocument.close();
	},
	editable:function(){
		if (editor.mode=="editor")
			editor.editorArea.iframeDocument.designMode = "on";
		else
			editor.editorArea.iframeDocument.designMode = "off";
	},
	focus:function(){
		editor._textarea_obj.focus();
	}
};

editor.translator = {
	html2md:function(string){
	  var ELEMENTS = [
		{
		  patterns: ['p', 'div'],
		  replacement: function(str, attrs, innerHTML) {
			return innerHTML ? '\n\n' + innerHTML + '\n' : '';
		  }
		},
		{
		  patterns: 'br',
		  type: 'void',
		  replacement: '\n'
		},
		{
		  patterns: 'h([1-6])',
		  replacement: function(str, hLevel, attrs, innerHTML) {
			var hPrefix = '';
			for(var i = 0; i < hLevel; i++) {
			  hPrefix += '#';
			}
			return '\n\n' + hPrefix + ' ' + innerHTML + '\n';
		  }
		},
		{
		  patterns: 'hr',
		  type: 'void',
		  replacement: '\n\n* * *\n'
		},
		{
		  patterns: 'a',
		  replacement: function(str, attrs, innerHTML) {
			var href = attrs.match(attrRegExp('href')),
				title = attrs.match(attrRegExp('title'));
			return href ? '[' + innerHTML + ']' + '(' + href[1] + (title && title[1] ? ' "' + title[1] + '"' : '') + ')' : str;
		  }
		},
		{
		  patterns: ['i', 'em'],
		  replacement: function(str, attrs, innerHTML) {
			return innerHTML ? '*' + innerHTML + '*' : '';
		  }
		},
		{
		  patterns: ['b', 'strong'],
		  replacement: function(str, attrs, innerHTML) {
			return innerHTML ? '**' + innerHTML + '**' : '';
		  }
		},
		{
		  patterns: 'strike',
		  replacement: function(str, attrs, innerHTML) {
			return innerHTML ? '~~' + innerHTML + '~~' : '';
		  }
		},
		{
		  patterns: 'code',
		  replacement: function(str, attrs, innerHTML) {
			return innerHTML ? '`' + he.decode(innerHTML) + '`' : '';
		  }
		},
		{
		  patterns: 'img',
		  type: 'void',
		  replacement: function(str, attrs, innerHTML) {
			var src = attrs.match(attrRegExp('src')),
				alt = attrs.match(attrRegExp('alt')),
				title = attrs.match(attrRegExp('title'));
			return '![' + (alt && alt[1] ? alt[1] : '') + ']' + '(' + src[1] + (title && title[1] ? ' "' + title[1] + '"' : '') + ')';
		  }
		}
	  ];

	  for(var i = 0, len = ELEMENTS.length; i < len; i++) {
		if(typeof ELEMENTS[i].patterns === 'string') {
		  string = replaceEls(string, { tag: ELEMENTS[i].patterns, replacement: ELEMENTS[i].replacement, type:  ELEMENTS[i].type });
		}
		else {
		  for(var j = 0, pLen = ELEMENTS[i].patterns.length; j < pLen; j++) {
			string = replaceEls(string, { tag: ELEMENTS[i].patterns[j], replacement: ELEMENTS[i].replacement, type:  ELEMENTS[i].type });
		  }
		}
	  }

	  function replaceEls(html, elProperties) {
		var pattern = elProperties.type === 'void' ? '<' + elProperties.tag + '\\b([^>]*)\\/?>' : '<' + elProperties.tag + '\\b([^>]*)>([\\s\\S]*?)<\\/' + elProperties.tag + '>',
			regex = new RegExp(pattern, 'gi'),
			markdown = '';
		if(typeof elProperties.replacement === 'string') {
		  markdown = html.replace(regex, elProperties.replacement);
		}
		else {
		  markdown = html.replace(regex, function(str, p1, p2, p3) {
			return elProperties.replacement.call(this, str, p1, p2, p3);
		  });
		}
		return markdown;
	  }

	  function attrRegExp(attr) {
		return new RegExp(attr + '\\s*=\\s*["\']?([^"\']*)["\']?', 'i');
	  }

	  // Pre code blocks

	  string = string.replace(/<pre\b[^>]*>`([\s\S]*)`<\/pre>/gi, function(str, innerHTML) {
		var text = he.decode(innerHTML);
		text = text.replace(/^\t+/g, '  '); // convert tabs to spaces (you know it makes sense)
		text = text.replace(/\n/g, '\n    ');
		return '\n\n    ' + text + '\n';
	  });

	  // Lists

	  // Escape numbers that could trigger an ol
	  // If there are more than three spaces before the code, it would be in a pre tag
	  // Make sure we are escaping the period not matching any character
	  string = string.replace(/^(\s{0,3}\d+)\. /g, '$1\\. ');

	  // Converts lists that have no child lists (of same type) first, then works its way up
	  var noChildrenRegex = /<(ul|ol)\b[^>]*>(?:(?!<ul|<ol)[\s\S])*?<\/\1>/gi;
	  while(string.match(noChildrenRegex)) {
		string = string.replace(noChildrenRegex, function(str) {
		  return replaceLists(str);
		});
	  }

	  function replaceLists(html) {

		html = html.replace(/<(ul|ol)\b[^>]*>([\s\S]*?)<\/\1>/gi, function(str, listType, innerHTML) {
		  var lis = innerHTML.split('</li>');
		  lis.splice(lis.length - 1, 1);

		  for(i = 0, len = lis.length; i < len; i++) {
			if(lis[i]) {
			  var prefix = (listType === 'ol') ? (i + 1) + ".  " : "*   ";
			  lis[i] = lis[i].replace(/\s*<li[^>]*>([\s\S]*)/i, function(str, innerHTML) {

				innerHTML = innerHTML.replace(/^\s+/, '');
				innerHTML = innerHTML.replace(/\n\n/g, '\n\n    ');
				// indent nested lists
				innerHTML = innerHTML.replace(/\n([ ]*)+(\*|\d+\.) /g, '\n$1    $2 ');
				return prefix + innerHTML;
			  });
			}
		  }
		  return lis.join('\n');
		});
		return '\n\n' + html.replace(/[ \t]+\n|\s+$/g, '');
	  }

	  // Blockquotes
	  var deepest = /<blockquote\b[^>]*>((?:(?!<blockquote)[\s\S])*?)<\/blockquote>/gi;
	  while(string.match(deepest)) {
		string = string.replace(deepest, function(str) {
		  return replaceBlockquotes(str);
		});
	  }

	  function replaceBlockquotes(html) {
		html = html.replace(/<blockquote\b[^>]*>([\s\S]*?)<\/blockquote>/gi, function(str, inner) {
		  inner = inner.replace(/^\s+|\s+$/g, '');
		  inner = cleanUp(inner);
		  inner = inner.replace(/^/gm, '> ');
		  inner = inner.replace(/^(>([ \t]{2,}>)+)/gm, '> >');
		  return inner;
		});
		return html;
	  }

	  function cleanUp(string) {
		string = string.replace(/^[\t\r\n]+|[\t\r\n]+$/g, ''); // trim leading/trailing whitespace
		string = string.replace(/\n\s+\n/g, '\n\n');
		string = string.replace(/\n{3,}/g, '\n\n'); // limit consecutive linebreaks to 2
		return string;
	  }

	  return cleanUp(string);
	},
	md2html:function(string){
	}
};
