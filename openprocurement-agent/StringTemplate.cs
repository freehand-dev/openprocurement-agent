using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace openprocurement_agent
{
    public class StringTemplate
    {
        private static readonly Dictionary<char, string> EscapeChars
            = new Dictionary<char, string>
            {
                ['r'] = "\r",
                ['n'] = "\n",
                ['\\'] = "\\",
                ['%'] = "%",
            };

        private static readonly Regex RenderExpr = new Regex(@"\\.|%([a-z0-9_.\-]+)%",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public string TemplateString { get; }

        public object Object { get; }

        public StringTemplate(string TemplateString, object Object)
        {
            if (TemplateString == null)
            {
                throw new ArgumentNullException(nameof(TemplateString));
            }

            this.TemplateString = TemplateString;
            this.Object = Object;
        }

        public override string ToString() => ToString(this.TemplateString, this.Object);

        public static string ToString(string TemplateString, object Object)
        {
            if (TemplateString == null)
            {
                throw new ArgumentNullException(nameof(TemplateString));
            }

            return RenderExpr.Replace(TemplateString, Match => {
                switch (Match.Value[0])
                {
                    case '\\':
                        if (EscapeChars.ContainsKey(Match.Value[1]))
                        {
                            return EscapeChars[Match.Value[1]];
                        }
                        break;

                    case '%':
                        var propValue = StringTemplate.GetPropertyValue(Object, Match.Groups[1].Value);
                        return (propValue != null) ? propValue.ToString() : "%" + Match.Groups[1].Value + "%";
                }

                return string.Empty;
            });
        }


        public static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (string.IsNullOrWhiteSpace(propName)) throw new ArgumentException(nameof(propName));

            foreach (string currentPropertyName in propName.Split('.'))
            {
                if (string.IsNullOrWhiteSpace(currentPropertyName)) return default;

                PropertyInfo propertyInfo = src.GetType().GetProperty(currentPropertyName);
                if (propertyInfo == null) return default;

                src = propertyInfo.GetValue(src);
            }

            return src;
        }

        public static T GetPropertyValue<T>(object src, string propName)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (string.IsNullOrWhiteSpace(propName)) throw new ArgumentException(nameof(propName));

            foreach (string currentPropertyName in propName.Split('.'))
            {
                if (string.IsNullOrWhiteSpace(currentPropertyName)) return default;

                PropertyInfo propertyInfo = src.GetType().GetProperty(currentPropertyName);
                if (propertyInfo == null) return default;

                src = propertyInfo.GetValue(src);
            }

            return src is T result ? result : default;
        }
    }
}
