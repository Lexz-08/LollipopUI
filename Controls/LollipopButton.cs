using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class LollipopButton : Control
{

    #region  Variables

    Timer AnimationTimer = new Timer { Interval = 1 };

    FontManager font = new FontManager();
    StringFormat SF = new StringFormat();
    Rectangle R;

    bool Focus = false;

    int xx;
    int yy;

    float SizeAnimation = 0;
    float SizeIncNum;

    Color fontcolor = Color.White;
    Color Backcolor = Color.FromArgb(80, 142, 245);

    Color EnabledBGColor;
    Color StringColor;

    Color DisabledBGColor = ColorTranslator.FromHtml("#b0b2b5");

    int radius = 2;

    #endregion
    #region  Properties

    [Category("Appearance")]
    public int Radius
	{
		get { return radius; }
		set
		{
            if (value < 2)
			{
                value = 2;
                radius = value;
			}
            else if (value > Math.Min(Width, Height) / 2)
			{
                value = Math.Min(Width, Height) / 2;
                radius = value;
			}
			else
			{
                radius = value;
			}
            Invalidate();
		}
	}

    [Category("Appearance")]
    public Color BGColor
    {
        get { return Backcolor; }
        set
        {
            Backcolor = value;
            Invalidate();
        }
    }

    [Category("Appearance")]
    public Color FontColor
    {
        get { return fontcolor; }
        set
        {
            fontcolor = value;
            Invalidate();
        }
    }
    
    [Browsable(false)]
    public override Font Font
    {
        get { return base.Font; }
        set { base.Font = value; }
    }

    [Browsable(false)]
    public override Color ForeColor
    {
        get { return base.ForeColor; }
        set { base.ForeColor = value; }
    }
    
    #endregion
    #region  Events

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        xx = e.X;
        yy = e.Y;
        Focus = true;
        AnimationTimer.Start();
        Invalidate();
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        Focus = false;
        AnimationTimer.Start();
        Invalidate();
    }

    protected override void OnTextChanged(System.EventArgs e)
    {       
        base.OnTextChanged(e);
        Invalidate();
    }
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        R = new Rectangle(0, 1, Width, Height);
    }

    #endregion

    public LollipopButton()
    {
        SetStyle((ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint), true);
        DoubleBuffered = true;

        Size = new Size(143, 41);
        BackColor = Color.Transparent;

        SF.Alignment = StringAlignment.Center;
        SF.LineAlignment = StringAlignment.Center;
        
        AnimationTimer.Tick += new EventHandler(AnimationTick);
    }

    protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);
        SizeIncNum = 11;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var G = e.Graphics;
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.Clear(Parent.BackColor);

        StringColor = fontcolor;
        EnabledBGColor = Backcolor;

        var BG = DrawHelper.CreateRoundRect(1, 1, Width - 3, Height - 3, radius);
        Region region = new Region(BG);

        G.FillPath(new SolidBrush(Enabled? EnabledBGColor:DisabledBGColor), BG);
        G.DrawPath(new Pen(Enabled ? EnabledBGColor : DisabledBGColor), BG);

        G.SetClip(region, CombineMode.Replace);
        
        //The Ripple Effect
        G.FillEllipse(new SolidBrush(Color.FromArgb(25,Color.Black)), xx - (SizeAnimation / 2), yy - (SizeAnimation / 2), SizeAnimation, SizeAnimation);

        G.DrawString(Text, font.Roboto_Medium9, new SolidBrush(StringColor), R, SF);       
    }

    protected void AnimationTick(object sender, EventArgs e)
    {
        if (Focus)
        {
            if (SizeAnimation < Width + 250)
            {
                SizeAnimation += SizeIncNum;
                this.Invalidate();
            }          
        }
        else
        {
            if (SizeAnimation > 0)
            {
                SizeAnimation = 0;
                this.Invalidate();
            }
        }
    }
}
