using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace 自定义控件库
{
    public class Converter
    {
        // html to markdown
        private readonly IList<IReplacer> _replacers1 = new List<IReplacer>
			{
                new Element
				{
					Pattern = @"\r|\n",     // 删掉所有换行符
					Replacement = Environment.NewLine
				},
				new Element
				{
					Pattern = @"<a.+?href\s*=\s*['""]([^'""]+)['""]>([^<]+)</a>",
					Replacement = @"[$2]($1)"
				},
				new Element
				{
					Pattern = @"</?(strong|b)>",
					Replacement = @"**"
				},
				new Element
				{
					Pattern = @"</?(em|i)>",
					Replacement = @"*"
				},
				new Element
				{
					Pattern = @"<br\s*/>",
					Replacement = @"  " + Environment.NewLine
				},
				new Element
				{
					Pattern = @"</?code>",
					Replacement = @"`"
				},
				new Element
				{
					Pattern = @"</h[1-6]>",
					Replacement = Environment.NewLine + Environment.NewLine
				},
				new Element
				{
					Pattern = @"<h1>",
					Replacement = Environment.NewLine + Environment.NewLine + "# "
				},
				new Element
				{
					Pattern = @"<h2>",
					Replacement = Environment.NewLine + Environment.NewLine + "## "
				},
				new Element
				{
					Pattern = @"<h3>",
					Replacement = Environment.NewLine + Environment.NewLine + "### "
				},
				new Element
				{
					Pattern = @"<h4>",
					Replacement = Environment.NewLine + Environment.NewLine + "#### "
				},
				new Element
				{
					Pattern = @"<h5>",
					Replacement = Environment.NewLine + Environment.NewLine + "##### "
				},
				new Element
				{
					Pattern = @"<h6>",
					Replacement = Environment.NewLine + Environment.NewLine + "###### "
				},
                new Element
				{
                    Pattern = @"<blockquote>",
                    Replacement = Environment.NewLine + Environment.NewLine + @"> "
                },
                new Element
				{
                    Pattern = @"</blockquote>",
                    Replacement = Environment.NewLine + Environment.NewLine
                },
				new Element
				{
					Pattern = @"<p>",
					Replacement = Environment.NewLine + Environment.NewLine
				},
				new Element
				{
					Pattern = @"</p>",
					Replacement = Environment.NewLine
				},
				new Element
				{
					Pattern = @"(<hr\s*/>)|(<hr\s*>)",
					Replacement = Environment.NewLine + Environment.NewLine + "-----" + Environment.NewLine
				},
				new CustomReplacer
				{
					CustomAction = HtmlParser.ReplaceImg
				},
				new CustomReplacer
				{
					CustomAction = HtmlParser.ReplacePre
				},
				new CustomReplacer
				{
					CustomAction = HtmlParser.ReplaceLists
				},
				new Element
				{
					Pattern = @"(" + Environment.NewLine + @"){3,}",
					Replacement = Environment.NewLine + Environment.NewLine
				},
				new Element
				{
					Pattern = @"^(" + Environment.NewLine + @"){1,}",
					Replacement = string.Empty
				}
			};

        // markdown to html
        private readonly IList<IReplacer> _replacers2 = new List<IReplacer>
			{
                new Element
				{
					Pattern = @"!\[([^\]]*)\]\((\S*)(?: ""([\s\S]*)"")?\)",
					Replacement = @"<img alt='$1' src='$2' title='$3'>"
				},
				new Element
				{
					Pattern = @"\[([^\]]+)\]\(([^\)]+)\)",
					Replacement = @"<a href='$2'>$1</a>"
				},
				new Element
				{
                    Pattern = @"\*\*(.+?)\*\*",
					Replacement = @"<b>$1</b>"
				},
				new Element
				{
					Pattern = @"\*(.+?)\*",
					Replacement = @"<i>$1</i>"
				},
				new Element
				{
					Pattern = @"  " + Environment.NewLine,
					Replacement = @"<br />" + Environment.NewLine
				},
				new Element
				{
					Pattern = @"`(.+?)`",
					Replacement = @"<code>$1</code>"
				},
				new Element
				{
					Pattern = string.Format(@"{0}{0}# ([^{0}]+)", Environment.NewLine),
					Replacement = Environment.NewLine + Environment.NewLine + @"<h1>$1</h1>"
				},
				new Element
				{
					Pattern = string.Format(@"{0}{0}## ([^{0}]+)", Environment.NewLine),
					Replacement = Environment.NewLine + Environment.NewLine + @"<h2>$1</h2>"
				},
				new Element
				{
					Pattern = string.Format(@"{0}{0}### ([^{0}]+)", Environment.NewLine),
					Replacement = Environment.NewLine + Environment.NewLine + @"<h3>$1</h3>"
				},
				new Element
				{
					Pattern = string.Format(@"{0}{0}#### ([^{0}]+)", Environment.NewLine),
					Replacement = Environment.NewLine + Environment.NewLine + @"<h4>$1</h4>"
				},
				new Element
				{
					Pattern = string.Format(@"{0}{0}##### ([^{0}]+)", Environment.NewLine),
					Replacement = Environment.NewLine + Environment.NewLine + @"<h5>$1</h5>"
				},
				new Element
				{
					Pattern = string.Format(@"{0}{0}###### ([^{0}]+)", Environment.NewLine),
					Replacement = Environment.NewLine + Environment.NewLine + @"<h6>$1</h6>"
				},
                new Element
				{
                    Pattern = Environment.NewLine + Environment.NewLine + @"> ([\s\S]+?)" + Environment.NewLine + Environment.NewLine,
                    Replacement = Environment.NewLine + Environment.NewLine + @"<blockquote>$1</blockquote>" + Environment.NewLine + Environment.NewLine
                },
				new Element
				{
					Pattern = Environment.NewLine + Environment.NewLine + @"-{3,}|\*{3,}" + Environment.NewLine,
					Replacement = Environment.NewLine + Environment.NewLine + @"<hr/>" + Environment.NewLine
				},
				new CustomReplacer
				{
					CustomAction = MarkdownParser.ReplaceLists
				},
				new CustomReplacer
				{
					CustomAction = MarkdownParser.ReplaceParagraph
				}
			};

        public string Html2Md(string html)
        {
            MatchCollection matchs = Regex.Matches(html, @"(?<tag><[^\s>]+\s)|(?<tag><[^\s>]+>)");
            foreach (Match match in matchs)
            {
                string tag = match.Value.ToLower();
                html = html.Replace(match.Value, tag);
            }
            html = System.Web.HttpUtility.HtmlDecode(html); // 解码网络特殊符号
            return _replacers1.Aggregate(html, (current, element) => element.Replace(current));
        }
        public string Md2Html(string markdown)
        {
            markdown = Environment.NewLine + Environment.NewLine + markdown + Environment.NewLine + Environment.NewLine;
            markdown = _replacers2.Aggregate(markdown, (current, element) => element.Replace(current));
            // 整理html的换行
            string html = Regex.Replace(markdown, @"[\r|\n]", string.Empty);
            html = Regex.Replace(html, @"(</[^>]+>|<[^>]+/>)", @"$1" + Environment.NewLine);
            return html;
        }
    }

    internal interface IReplacer
    {
        string Replace(string content);
    }

    internal class Element : IReplacer
    {
        public string Pattern { get; set; }

        public string Replacement { get; set; }
        public string Replace(string content)
        {
            var regex = new Regex(Pattern);

            return regex.Replace(content, Replacement);
        }
    }

    internal class CustomReplacer : IReplacer
    {
        public string Replace(string content)
        {
            return CustomAction.Invoke(content);
        }

        public Func<string, string> CustomAction { get; set; }
    }

    internal static class HtmlParser
    {
        private static readonly Regex NoChildren = new Regex(@"<(ul|ol)\b[^>]*>(?:(?!<ul|<ol)[\s\S])*?<\/\1>");

        internal static string ReplaceLists(string html)
        {
            while (HasNoChildLists(html))
            {
                var listToReplace = NoChildren.Match(html).Value;
                var formattedList = ReplaceList(listToReplace);
                html = html.Replace(listToReplace, formattedList);
            }

            return html;
        }

        private static string ReplaceList(string html)
        {
            var list = Regex.Match(html, @"<(ul|ol)\b[^>]*>([\s\S]*?)<\/\1>");
            var type = list.Groups[1].Value;
            var lis = list.Groups[2].Value.Split(new[] { "</li>", "\r\n", "\n" }, StringSplitOptions.None);

            var counter = 0;
            var markdownList = new List<string>();
            foreach (var li in lis)
            {
                var prefix = "";
                var final = "";
                if (Regex.IsMatch(li, @"^([ ]*)+(\*|\+|-|\d+\.) [\s\S]*"))
                {// 缩进嵌套列表
                    final = Regex.Replace(li, @"^([ ]*)+(\*|\d+\.)", @"$1    $2");
                }
                else
                {
                    final = Regex.Replace(li, @"\s*<li[^>]*>", string.Empty);
                    if (final.Trim().Length == 0) continue;

                    prefix = (type.Equals("ol")) ? string.Format("{0}.  ", ++counter) : "*   ";

                    final = Regex.Replace(final, @"^\s+", string.Empty);
                    final = Regex.Replace(final, @"\n{2}", string.Format("{0}{1}    ", Environment.NewLine, Environment.NewLine));
                }

                markdownList.Add(string.Format("{0}{1}", prefix, final));
            }

            return Environment.NewLine + Environment.NewLine + markdownList.Aggregate((current, item) => current + Environment.NewLine + item);
        }

        private static bool HasNoChildLists(string html)
        {
            return NoChildren.Match(html).Success;
        }

        internal static string ReplacePre(string html)
        {
            var preTags = new Regex(@"<pre\b[^>]*>([\s\S]*)<\/pre>").Matches(html);

            return preTags.Cast<Match>().Aggregate(html, ConvertPre);
        }

        private static string ConvertPre(string html, Match preTag)
        {
            var tag = preTag.Groups[1].Value;
            tag = TabsToSpaces(tag);
            tag = IndentNewLines(tag);
            html = html.Replace(preTag.Value, Environment.NewLine + Environment.NewLine + tag + Environment.NewLine);
            return html;
        }

        private static string IndentNewLines(string tag)
        {
            return tag.Replace(Environment.NewLine, Environment.NewLine + "    ");
        }

        private static string TabsToSpaces(string tag)
        {
            return tag.Replace("\t", "    ");
        }

        internal static string ReplaceImg(string html)
        {
            var originalImages = new Regex(@"<img([^>]+)>").Matches(html);

            foreach (Match image in originalImages)
            {
                var img = image.Value;
                var src = AttributeParser(img, "src");
                var alt = AttributeParser(img, "alt");
                var title = AttributeParser(img, "title");

                html = html.Replace(img, string.Format(@"![{0}]({1}{2})", alt, src, (title.Length > 0) ? string.Format(" \"{0}\"", (object)title) : ""));
            }

            return html;
        }

        private static string AttributeParser(string html, string attribute)
        {
            var match = Regex.Match(html, string.Format(@"{0}\s*=\s*[""\']?([^""\']*)[""\']?", attribute));
            var groups = match.Groups;
            return groups[1].Value;
        }
    }

    internal static class MarkdownParser
    {
        internal static string ReplaceLists(string markdown)
        {
            string pattern = string.Format(@"{0}{0}([ ]*(?:\*|\+|-|\d+\.) [\s\S]*?{0})+{0}", Environment.NewLine);

            foreach (Match match in Regex.Matches(markdown, pattern))
            {
                var listToReplace = match.Value;
                var formattedList = Environment.NewLine + ReplaceListTree(listToReplace, 0) + Environment.NewLine;
                markdown = markdown.Replace(listToReplace, formattedList);
            }

            return markdown;
        }

        private static string ReplaceListTree(string markdown, int depth)
        {
            // 先检查子层
            if (Regex.IsMatch(markdown, string.Format(@" {{{0},}}(\*|\+|-|\d+\.) ", 4 * depth + 1)))
            {// 存在更子层的列表
                markdown = ReplaceListTree(markdown, depth + 1);
            }

            // 处理本层
            string pattern = string.Format(@"(([ ]{{{0}}}(\*|\+|-|\d+\.) [\s\S]*?){1}|(<[ou]l>[\s\S]*</[ou]l>))+", 4 * depth, Environment.NewLine);
            foreach (Match match in Regex.Matches(markdown, pattern))
            {
                var listToReplace = match.Value;
                var formattedList = ReplaceList(listToReplace);
                markdown = markdown.Replace(listToReplace, formattedList);
            }

            return markdown;
        }

        private static string ReplaceList(string markdown)
        {
            string pattern = string.Format(@"[ ]*(\*|\+|-|\d+\.)[ ]+([\s\S]*?){0}", Environment.NewLine);
            var type = Regex.Match(markdown, pattern).Groups[1].Value;

            markdown = Regex.Replace(markdown, pattern, @"<li>$2</li>");

            if (Regex.IsMatch(type, @"\*|\+|-"))
            {
                return string.Format("<ul>{0}</ul>", markdown);
            }
            else
            {
                return string.Format("<ol>{0}</ol>", markdown);
            }
        }

        internal static string ReplaceParagraph(string markdown)
        {
            var regex = new Regex(string.Format(@"{0}{0}(?!<\S+>)(.+)(?!</\S+>){0}", Environment.NewLine));

            while (regex.Match(markdown).Success)
            {
                markdown = regex.Replace(markdown, string.Format(@"{0}<p>$1</p>{0}", Environment.NewLine));
            }

            return markdown;
        }
    }
}
