﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

[DefaultEvent("TextChanged")]
public class LollipopPasswordInput : Control
{

    #region  Variables

    TextBox LollipopTB = new TextBox();
    HorizontalAlignment ALNType;

    int maxchars = 32767;
    bool multiline;
    bool showPassword = false;
    bool Enable = true;

    Timer AnimationTimer = new Timer { Interval = 1 };
    FontManager font = new FontManager();

    bool Focus = false;

    float SizeAnimation = 0;
    float SizeInc_Dec;

    float PointAnimation;
    float PointInc_Dec;

    Color fontColor = ColorTranslator.FromHtml("#999999");
    Color focusColor = ColorTranslator.FromHtml("#508ef5");

    Color EnabledFocusedColor;
    Color EnabledStringColor;

    Color EnabledUnFocusedColor = ColorTranslator.FromHtml("#dbdbdb");

    Color DisabledUnFocusedColor = ColorTranslator.FromHtml("#e9ecee");
    Color DisabledStringColor = ColorTranslator.FromHtml("#babbbd");

    #endregion
    #region  Properties

    public HorizontalAlignment TextAlignment
    {
        get
        {
            return ALNType;
        }
        set
        {
            ALNType = value;
            Invalidate();
        }
    }

    [Category("Behavior")]
    public int MaxLength
    {
        get
        {
            return maxchars;
        }
        set
        {
            maxchars = value;
            LollipopTB.MaxLength = MaxLength;
            Invalidate();
        }
    }
    [Category("Behavior")]
    public bool ShowPassword
    {
        get
        {
            return showPassword;
        }
        set
        {
            LollipopTB.UseSystemPasswordChar = !ShowPassword;
            showPassword = value;
            Invalidate();
        }
    }

    [Category("Behavior")]
    public bool IsEnabled
    {
        get { return Enable; }
        set
        {
            Enable = value;

            if (IsEnabled)
            {
                LollipopTB.ForeColor = EnabledStringColor;
            }
            else
            {
                LollipopTB.ForeColor = DisabledStringColor;
            }

            Invalidate();
        }
    }

    [Category("Appearance")]
    public Color FocusedColor
    {
        get { return focusColor; }
        set
        {
            focusColor = value;
            Invalidate();
        }
    }

    [Category("Appearance")]
    public Color FontColor
    {
        get { return fontColor; }
        set
        {
            fontColor = value;
            Invalidate();
        }
    }

    [Browsable(false)]
    public bool Enabled
    {
        get { return base.Enabled; }
        set { base.Enabled = value; }
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

    protected void OnKeyDown(object Obj, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.A)
        {
            LollipopTB.SelectAll();
            e.SuppressKeyPress = true;
        }
        if (e.Control && e.KeyCode == Keys.C)
        {
            LollipopTB.Copy();
            e.SuppressKeyPress = true;
        }
        if (e.Control && e.KeyCode == Keys.X)
        {
            LollipopTB.Cut();
            e.SuppressKeyPress = true;
        }
    }
    protected override void OnTextChanged(System.EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    protected override void OnGotFocus(System.EventArgs e)
    {
        base.OnGotFocus(e);
        LollipopTB.Focus();
        LollipopTB.SelectionLength = 0;
    }
    protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);

        PointAnimation = Width / 2;
        SizeInc_Dec = Width / 18;
        PointInc_Dec = Width / 36;

        LollipopTB.Width = Width;


        if (multiline)
        {
            LollipopTB.Location = new Point(-3, 3);
            LollipopTB.Width = Width + 3;
            LollipopTB.Height = Height - 6;
        }
        else
        {
            LollipopTB.Location = new Point(0, 3);
            LollipopTB.Width = Width;
            Height = 24;
        }
    }

    #endregion

    public void AddTextBox()
    {
        LollipopTB.Location = new Point(0, 4);
        LollipopTB.Size = new Size(Width, 18);
        LollipopTB.Text = Text;

        LollipopTB.BorderStyle = BorderStyle.None;
        LollipopTB.TextAlign = HorizontalAlignment.Left;
        LollipopTB.Font = font.Roboto_Regular10;
        LollipopTB.UseSystemPasswordChar = !ShowPassword;
        LollipopTB.Multiline = false;
        LollipopTB.BackColor = BackColor;
        LollipopTB.ScrollBars = ScrollBars.None;
        LollipopTB.KeyDown += OnKeyDown;

        LollipopTB.GotFocus += (sender, args) => Focus = true; AnimationTimer.Start();
        LollipopTB.LostFocus += (sender, args) => Focus = false; AnimationTimer.Start();
    }

    public LollipopPasswordInput()
    {
        Width = 300;
        DoubleBuffered = true;

        BackColorChanged += (sender, e) =>
        {
            LollipopTB.BackColor = BackColor;
        };

        AddTextBox();
        Controls.Add(LollipopTB);

        LollipopTB.TextChanged += (sender, args) => Text = LollipopTB.Text;
        base.TextChanged += (sender, args) => LollipopTB.Text = Text;

        AnimationTimer.Tick += new EventHandler(AnimationTick);
    }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        base.OnPaint(e);
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        G.Clear(Color.Transparent);

        EnabledStringColor = fontColor;
        EnabledFocusedColor = focusColor;

        LollipopTB.TextAlign = TextAlignment;
        LollipopTB.ForeColor = IsEnabled ? EnabledStringColor : DisabledStringColor;
        LollipopTB.UseSystemPasswordChar = !ShowPassword;

        G.DrawLine(new Pen(new SolidBrush(IsEnabled ? EnabledUnFocusedColor : DisabledUnFocusedColor), 2), new Point(0, Height - 1), new Point(Width, Height - 2));
        if (IsEnabled)
        { G.FillRectangle(new SolidBrush(EnabledFocusedColor), PointAnimation, (float)Height - 3, SizeAnimation, 2); }

        e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
        G.Dispose();
        B.Dispose();
    }

    protected void AnimationTick(object sender, EventArgs e)
    {
        if (Focus)
        {
            if (SizeAnimation < Width)
            {
                SizeAnimation += SizeInc_Dec;
                this.Invalidate();
            }

            if (PointAnimation > 0)
            {
                PointAnimation -= PointInc_Dec;
                this.Invalidate();
            }
        }
        else
        {
            if (SizeAnimation > 0)
            {
                SizeAnimation -= SizeInc_Dec;
                this.Invalidate();
            }

            if (PointAnimation < Width / 2)
            {
                PointAnimation += PointInc_Dec;
                this.Invalidate();
            }
        }
    }

}
