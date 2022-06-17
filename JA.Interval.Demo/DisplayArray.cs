using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace JA
{
    public enum TextAlign
    {
        Left,
        Center,
        Right
    }

    public class DisplayArray<T> : FormattableString
    {
        public DisplayArray(IEnumerable<T> array, string format,
            int minWidth = 0, TextAlign align = TextAlign.Right, string separator = ", ",
            string openBrace = "(", string closeBrace = ")")
            : this(array.ToArray(), format, minWidth, align, separator,
                openBrace, closeBrace)
        { }
        public DisplayArray(T[] array, string format,
            int minWidth = 0, TextAlign align = TextAlign.Right, string separator = ", ",
            string openBrace = "(", string closeBrace = ")")
        {
            Array = array;
            Format = format;
            MinWidth = minWidth;
            Align = align;
            Separator = separator;
            OpenBrace = openBrace;
            CloseBrace = closeBrace;
        }        
        public static implicit operator string(DisplayArray<T> display) => display.ToString();
        public static string DefaultFormat { get; set; } = "g";
        public static implicit operator DisplayArray<T>(T[] array)
            => new DisplayArray<T>(array, DefaultFormat);

        public T[] Array { get; set; }
        public override string Format { get; }
        public int MinWidth { get; set; }
        public string OpenBrace { get; set; }
        public string CloseBrace { get; set; }
        public string Separator { get; set; }
        public TextAlign Align { get; set; }
        public override int ArgumentCount => Array.Length;
        public override object[] GetArguments() => Array.Cast<object>().ToArray();
        public override object GetArgument(int index)
        {
            return Array[index];
        }
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture.NumberFormat);
        }
        public override string ToString(IFormatProvider formatProvider)
        {
            var parts = GetArguments().Select(obj =>
               {
                   string text = obj is IFormattable f ? f.ToString(Format, formatProvider) : obj.ToString();
                   if (Align == TextAlign.Left)
                   {
                       return text.PadRight(MinWidth);
                   }
                   else if (Align == TextAlign.Right)
                   {
                       return text.PadLeft(MinWidth);
                   }
                   else
                   {
                       int x = MinWidth / 2 - text.Length / 2;
                       return text.PadLeft(x+text.Length).PadRight(MinWidth);
                   }
               });
            return $"{OpenBrace}{string.Join(Separator, parts)}{CloseBrace}";
        }

    }
}
