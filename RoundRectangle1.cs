using System.Drawing.Drawing2D;

public sealed class RoundRectangle1
{
	public static GraphicsPath CreateRoundRect(int x, int y, int w, int h, int r)
	{
		int r2 = r * 2;

		GraphicsPath p = new GraphicsPath();
		p.AddArc(x, y, r2, r2, 180, 90);
		//p.AddLine(x + r, y, x + w - r, y);
		p.AddArc(x + w - r2, y, r2, r2, 270, 90);
		//p.AddLine(x + w, y + r, x + w, y + h - r);
		p.AddArc(x + w - r2, y + h - r2, r2, r2, 0, 90);
		//p.AddLine(x + r, y + h, x + w - r, y + h);
		p.AddArc(x, y + h - r2, r2, r2, 90, 90);
		p.CloseFigure();
		return p;
	}
}
