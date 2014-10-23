using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace 自定义控件库
{
    [ToolboxBitmap(typeof(OpaqueLayer))]
    public partial class OpaqueLayer : System.Windows.Forms.Control
    {

        private int _alpha = 125;
        private System.ComponentModel.IContainer components = new System.ComponentModel.Container();

        public OpaqueLayer() : this(125)
        {
        }

        public OpaqueLayer(int Alpha)
        {
            SetStyle(System.Windows.Forms.ControlStyles.Opaque, true);
            base.CreateControl();
            this._alpha = Alpha;
        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 自定义绘制窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            float vlblControlWidth;
            float vlblControlHeight;

            Pen labelBorderPen;
            SolidBrush labelBackColorBrush;

            Color drawColor = Color.FromArgb(this._alpha, this.BackColor);
            labelBorderPen = new Pen(drawColor, 0);
            labelBackColorBrush = new SolidBrush(drawColor);
            base.OnPaint(e);
            vlblControlWidth = this.Size.Width;
            vlblControlHeight = this.Size.Height;
            e.Graphics.DrawRectangle(labelBorderPen, 0, 0, vlblControlWidth, vlblControlHeight);
            e.Graphics.FillRectangle(labelBackColorBrush, 0, 0, vlblControlWidth, vlblControlHeight);

        }
        /// <summary>
        /// 
        /// </summary>
        protected override CreateParams CreateParams//v1.10 
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;  // 开启 WS_EX_TRANSPARENT,使控件支持透明
                return cp;
            }
        }

        [Category("myOpaqueLayer"), Description("设置透明度")]
        public int Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value;
                this.Invalidate();
            }
        }

        #endregion
    }
}
