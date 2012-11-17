namespace PG4Map
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using Tao.OpenGl;
    using Tao.Platform.Windows;

    public class Preview_Obj : Form
    {
        private IContainer components = null;
        public SimpleOpenGlControl GlControl2;

        public event Action RenderScene;

        public Preview_Obj()
        {
              InitializeComponent();
              GlControl2.InitializeContexts();
              Render();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (  components != null))
            {
                  components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GlControl2_Paint(object sender, PaintEventArgs e)
        {
              Render();
        }

        private void InitializeComponent()
        {
              GlControl2 = new SimpleOpenGlControl();
            base.SuspendLayout();
              GlControl2.AccumBits = 0;
              GlControl2.AutoCheckErrors = false;
              GlControl2.AutoFinish = false;
              GlControl2.AutoMakeCurrent = true;
              GlControl2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
              GlControl2.AutoSwapBuffers = true;
              GlControl2.BackColor = Color.Black;
              GlControl2.ColorBits = 0x20;
              GlControl2.DepthBits = 0x10;
              GlControl2.Dock = DockStyle.Fill;
              GlControl2.Location = new Point(0, 0);
              GlControl2.Name = "GlControl2";
              GlControl2.Size = new Size(0x1a8, 0x24a);
              GlControl2.StencilBits = 0;
              GlControl2.TabIndex = 2;
              GlControl2.Paint += new PaintEventHandler(  GlControl2_Paint);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1a8, 0x24a);
            base.Controls.Add(  GlControl2);
            base.Name = "Preview_Obj";
              Text = "Form1";
            base.ResumeLayout(false);
        }

        private void Render()
        {
            Gl.glViewport(0, 0, base.Width, base.Height);
            float[] vp = new float[4];
            Gl.glGetFloatv(0xba2, vp);
            float num = (vp[2] - vp[0]) / (vp[3] - vp[1]);
            Gl.glMatrixMode(0x1701);
            Gl.glLoadIdentity();
            Glu.gluPerspective(90.0, (double) num, 0.019999999552965164, 32.0);
            if (  RenderScene != null)
            {
                  RenderScene();
            }
        }
    }
}

