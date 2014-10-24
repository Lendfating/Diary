注意，为了让webbrowser运行在ie11版本（默认是7）下，需要修改注册表，如下：

让WebBrowser在工作在标准模式下的方法：
32位系统中HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION
64位系统中HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION

在其中增加一个Dword值，名字为咱们的程序的exe文件名，比如“Diary.exe”，值为十进制的11000即可。
当然这里有一个陷阱，在Visual Studio中调试的时候浏览器还是工作在兼容模式下，而直接双击bin/debug下的“Diary.exe”则不会。这是因为VS调试启动一个程序的时候默认是启动“Diary.vshost.exe”，所以把“海驾约车.vshost.exe”也加入注册表即可。

注意，这个选ie最新的版本，不然排版可能跟预期不一样。