using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

public class LollipopTabControl : TabControl
{
	FontManager fontManager = new FontManager();

	public event EventHandler TabColorChanged;
	protected virtual void OnTabColorChanged(EventArgs e)
	{
		TabColorChanged?.Invoke(this, e);
		Invalidate();
	}

	private Color tabColor = Color.FromArgb(51, 182, 121);
	
	[Category("Appearance")]
	public Color TabColor
	{
		get { return tabColor; }
		set { tabColor = value; OnTabColorChanged(null); }
	}

	public override Rectangle DisplayRectangle
	{
		get
		{
			Rectangle rect = base.DisplayRectangle;
			return new Rectangle(rect.Left - 4, rect.Top - 4, rect.Width + 8, rect.Height + 8);
		}
	}
	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams cp = base.CreateParams;
			cp.ExStyle |= 0x02000000;
			return cp;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		var gfx = e.Graphics;
		gfx.SmoothingMode = SmoothingMode.HighQuality;
		gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
		gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
		gfx.Clear(Parent.BackColor);

		for (int i = 0; i < TabCount; i++)
		{
			if (i == SelectedIndex)
			{
				gfx.FillRectangle(new SolidBrush(tabColor), GetTabRect(i).X + 3, ItemSize.Height - 3, ItemSize.Width - 6, 3);
				gfx.DrawString(TabPages[i].Text, fontManager.Roboto_Medium10, new SolidBrush(tabColor), GetTabRect(i),
					new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			}
			else
			{
				gfx.DrawString(TabPages[i].Text, fontManager.Roboto_Medium10, new SolidBrush(Color.Black), GetTabRect(i),
					new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			}
		}
	}

	public LollipopTabControl()
	{
		SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
		DoubleBuffered = true;
		SizeMode = TabSizeMode.Fixed;
		ItemSize = new Size(120, 40);
	}
}