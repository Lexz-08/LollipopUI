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

	[Category("Appearance")]
	public TabAlignment Alignment
	{
		get { return base.Alignment; }
		set
		{
			if (value == TabAlignment.Left)
			{
				value = TabAlignment.Top;
				base.Alignment = value;
			}
			else if (value == TabAlignment.Right)
			{
				value = TabAlignment.Bottom;
				base.Alignment = value;
			}
			else
			{
				base.Alignment = value;
			}
			Invalidate();
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
				switch (Alignment)
				{
					case TabAlignment.Top:
						gfx.FillRectangle(new SolidBrush(tabColor), GetTabRect(i).X + 3, ItemSize.Height - 3, ItemSize.Width - 6, 3);
						gfx.DrawString(TabPages[i].Text, fontManager.Roboto_Regular9, new SolidBrush(tabColor),
							new Rectangle(GetTabRect(i).X, GetTabRect(i).Y - 3, GetTabRect(i).Width, GetTabRect(i).Height),
							new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
						break;
					case TabAlignment.Bottom:
						gfx.FillRectangle(new SolidBrush(tabColor), GetTabRect(i).X + 3, GetTabRect(i).Y + 2, ItemSize.Width - 6, 3);
						gfx.DrawString(TabPages[i].Text, fontManager.Roboto_Regular9, new SolidBrush(tabColor),
							new Rectangle(GetTabRect(i).X, GetTabRect(i).Y + 4, GetTabRect(i).Width, GetTabRect(i).Height),
							new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
						break;
				}
			}
			else
			{
				switch (Alignment)
				{
					case TabAlignment.Top:
						gfx.DrawString(TabPages[i].Text, fontManager.Roboto_Regular9, new SolidBrush(Color.Black),
							new Rectangle(GetTabRect(i).X, GetTabRect(i).Y - 3, GetTabRect(i).Width, GetTabRect(i).Height),
							new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
						break;
					case TabAlignment.Bottom:
						gfx.DrawString(TabPages[i].Text, fontManager.Roboto_Regular9, new SolidBrush(Color.Black),
							new Rectangle(GetTabRect(i).X, GetTabRect(i).Y + 4, GetTabRect(i).Width, GetTabRect(i).Height),
							new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
						break;
				}
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