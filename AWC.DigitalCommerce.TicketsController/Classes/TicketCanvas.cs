using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace AWC.DigitalCommerce.TicketsController.Classes
{
    public class TicketCanvas
    {
        private readonly Graphics _g;
        private readonly TicketStyle _style;

        public int CursorY { get; private set; }

        public int PaperWidth => _style.Width;
        public int MarginLeft => _style.MarginLeft;
        public int MarginRight => _style.Width - _style.MarginRight;
        public int PrintableWidth => MarginRight - MarginLeft;

        private float QtyX => MarginLeft;
        private float DescX => MarginLeft + PrintableWidth * 0.10f;
        private float PriceRightX => MarginLeft + PrintableWidth;

        public TicketCanvas(Graphics graphics, TicketStyle style)
        {
            _g = graphics ?? throw new ArgumentNullException(nameof(graphics));
            _style = style ?? throw new ArgumentNullException(nameof(style));

            CursorY = style.TopMargin;

            _g.SmoothingMode = SmoothingMode.HighQuality;
            _g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        public void Space(int pixels = 6) => CursorY += pixels;

        public void MoveTo(int y) => CursorY = y;

        private void Advance(Font font)
        {
            CursorY += (int)Math.Ceiling(font.GetHeight(_g)) + _style.LineSpacing;
        }

        public void TextLeft(string text, Font font)
        {
            _g.DrawString(text ?? "", font, Brushes.Black, MarginLeft, CursorY);
            Advance(font);
        }

        public void TextCenter(string text, Font font)
        {
            var s = _g.MeasureString(text ?? "", font);
            float x = (PaperWidth - s.Width) / 2f;
            _g.DrawString(text ?? "", font, Brushes.Black, x, CursorY);
            Advance(font);
        }

        public void TextRight(string text, Font font)
        {
            var s = _g.MeasureString(text ?? "", font);
            float x = MarginRight - s.Width;
            _g.DrawString(text ?? "", font, Brushes.Black, x, CursorY);
            Advance(font);
        }

        public void LabelValue(string label, string value, Font font = null)
        {
            font = font ?? TicketFonts.Normal;

            _g.DrawString(label ?? "", font, Brushes.Black, MarginLeft, CursorY);

            var size = _g.MeasureString(value ?? "", font);

            _g.DrawString(value ?? "", font, Brushes.Black, MarginRight - size.Width, CursorY);

            Advance(font);
        }

        public void Title(string text)
        {
            TextCenter(text, TicketFonts.Title);
            Space(2);
        }

        public void Subtitle(string text)
        {
            TextCenter(text, TicketFonts.Header);
        }

        public void Separator()
        {
            int y = CursorY + 4;
            _g.DrawLine(Pens.Black, MarginLeft, y, MarginRight, y);
            CursorY += 10;
        }

        public void Box(int height)
        {
            _g.DrawRectangle(Pens.Black, MarginLeft, CursorY, PrintableWidth, height);
            CursorY += height + 4;
        }

        public void Logo(Image image, int maxWidth = 220)
        {
            if (image == null) return;

            float scale = Math.Min((float)maxWidth / image.Width, 1f);
            int w = (int)(image.Width * scale);
            int h = (int)(image.Height * scale);

            int x = (PaperWidth - w) / 2;

            _g.DrawImage(image, x, CursorY, w, h);

            CursorY += h + 8;
        }

        public void Qr(Image image, int size = 140)
        {
            if (image == null) return;

            int x = (PaperWidth - size) / 2;

            _g.DrawImage(image, x, CursorY, size, size);

            CursorY += size + 8;
        }

        public void TableHeader()
        {
            DrawAt("Cant", TicketFonts.Bold, QtyX);
            DrawAt("Descripción", TicketFonts.Bold, DescX);
            DrawRight("Total", TicketFonts.Bold, PriceRightX);

            Advance(TicketFonts.Bold);
            Separator();
        }

        public void TableRow(int qty, string description, decimal total)
        {
            DrawAt(qty.ToString(), TicketFonts.Normal, QtyX);

            DrawClipped(description ?? "", TicketFonts.Normal, DescX, PriceRightX - DescX - 70);

            DrawRight(total.ToString("N0"), TicketFonts.Normal, PriceRightX);

            Advance(TicketFonts.Normal);
        }

        public void Amount(string label, decimal amount)
        {
            LabelValue(label, amount.ToString("N0"), TicketFonts.Normal);
        }

        public void GrandTotal(decimal total)
        {
            Separator();
            TextCenter("TOTAL", TicketFonts.Header);
            TextCenter("₡ " + total.ToString("N0"), TicketFonts.Big);
            Separator();
        }

        private void DrawAt(string text, Font font, float x)
        {
            _g.DrawString(text, font, Brushes.Black, x, CursorY);
        }

        private void DrawRight(string text, Font font, float rightX)
        {
            var size = _g.MeasureString(text, font);
            _g.DrawString(text, font, Brushes.Black, rightX - size.Width, CursorY);
        }

        private void DrawClipped(string text, Font font, float x, float maxWidth)
        {
            string value = text;

            while (value.Length > 0 &&
                   _g.MeasureString(value, font).Width > maxWidth)
            {
                value = value.Substring(0, value.Length - 1);
            }

            _g.DrawString(value, font, Brushes.Black, x, CursorY);
        }
    }
}
