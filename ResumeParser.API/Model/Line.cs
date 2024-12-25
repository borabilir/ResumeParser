using System;
using System.Drawing;

namespace ResumeParser.API.Model
{
    public class Line
    {
        public string Text { get; set; }
        public double HorizontalPosition { get; set; }
        public double VerticalPosition { get; set; }
        public double FontSize { get; set; }
        public string FontName { get; set; }
        public Color FontColor { get; set; }
        public string Tag { get; set; }
        public int PageNumber { get; set; }
    }
}
