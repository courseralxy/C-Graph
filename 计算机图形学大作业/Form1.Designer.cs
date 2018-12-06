

namespace CG
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.基本图形生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DDALine = new System.Windows.Forms.ToolStripMenuItem();
            this.MidLine = new System.Windows.Forms.ToolStripMenuItem();
            this.bresenham直线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MidCircle = new System.Windows.Forms.ToolStripMenuItem();
            this.BresenhamCircle = new System.Windows.Forms.ToolStripMenuItem();
            this.正负圆ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BezierCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.BSampleCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.hermite曲线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二维图形变换ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PingMove = new System.Windows.Forms.ToolStripMenuItem();
            this.图形旋转ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二维图形裁剪ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CohenCut = new System.Windows.Forms.ToolStripMenuItem();
            this.MidCut = new System.Windows.Forms.ToolStripMenuItem();
            this.LiangCut = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowCut = new System.Windows.Forms.ToolStripMenuItem();
            this.图形填充ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScanLineFill = new System.Windows.Forms.ToolStripMenuItem();
            this.投影ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.消隐ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.基本图形生成ToolStripMenuItem,
            this.二维图形变换ToolStripMenuItem,
            this.二维图形裁剪ToolStripMenuItem,
            this.图形填充ToolStripMenuItem,
            this.投影ToolStripMenuItem,
            this.消隐ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(882, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 基本图形生成ToolStripMenuItem
            // 
            this.基本图形生成ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DDALine,
            this.MidLine,
            this.bresenham直线ToolStripMenuItem,
            this.MidCircle,
            this.BresenhamCircle,
            this.正负圆ToolStripMenuItem,
            this.BezierCurve,
            this.BSampleCurve,
            this.hermite曲线ToolStripMenuItem});
            this.基本图形生成ToolStripMenuItem.Name = "基本图形生成ToolStripMenuItem";
            this.基本图形生成ToolStripMenuItem.Size = new System.Drawing.Size(111, 24);
            this.基本图形生成ToolStripMenuItem.Text = "基本图形生成";
            // 
            // DDALine
            // 
            this.DDALine.Name = "DDALine";
            this.DDALine.Size = new System.Drawing.Size(194, 26);
            this.DDALine.Text = "DDA直线";
            this.DDALine.Click += new System.EventHandler(this.DDALine_Click);
            // 
            // MidLine
            // 
            this.MidLine.Name = "MidLine";
            this.MidLine.Size = new System.Drawing.Size(194, 26);
            this.MidLine.Text = "中点直线";
            this.MidLine.Click += new System.EventHandler(this.MidLine_Click);
            // 
            // bresenham直线ToolStripMenuItem
            // 
            this.bresenham直线ToolStripMenuItem.Name = "bresenham直线ToolStripMenuItem";
            this.bresenham直线ToolStripMenuItem.Size = new System.Drawing.Size(194, 26);
            this.bresenham直线ToolStripMenuItem.Text = "Bresenham直线";
            // 
            // MidCircle
            // 
            this.MidCircle.Name = "MidCircle";
            this.MidCircle.Size = new System.Drawing.Size(194, 26);
            this.MidCircle.Text = "中点圆";
            this.MidCircle.Click += new System.EventHandler(this.中点圆ToolStripMenuItem_Click);
            // 
            // BresenhamCircle
            // 
            this.BresenhamCircle.Name = "BresenhamCircle";
            this.BresenhamCircle.Size = new System.Drawing.Size(194, 26);
            this.BresenhamCircle.Text = "Bresenham圆";
            this.BresenhamCircle.Click += new System.EventHandler(this.BresenhamCircle_Click);
            // 
            // 正负圆ToolStripMenuItem
            // 
            this.正负圆ToolStripMenuItem.Name = "正负圆ToolStripMenuItem";
            this.正负圆ToolStripMenuItem.Size = new System.Drawing.Size(194, 26);
            this.正负圆ToolStripMenuItem.Text = "正负圆";
            // 
            // BezierCurve
            // 
            this.BezierCurve.Name = "BezierCurve";
            this.BezierCurve.Size = new System.Drawing.Size(194, 26);
            this.BezierCurve.Text = "Bezier曲线";
            this.BezierCurve.Click += new System.EventHandler(this.BezierCurve_Click);
            // 
            // BSampleCurve
            // 
            this.BSampleCurve.Name = "BSampleCurve";
            this.BSampleCurve.Size = new System.Drawing.Size(194, 26);
            this.BSampleCurve.Text = "B样条曲线";
            this.BSampleCurve.Click += new System.EventHandler(this.BSampleCurve_Click);
            // 
            // hermite曲线ToolStripMenuItem
            // 
            this.hermite曲线ToolStripMenuItem.Name = "hermite曲线ToolStripMenuItem";
            this.hermite曲线ToolStripMenuItem.Size = new System.Drawing.Size(194, 26);
            this.hermite曲线ToolStripMenuItem.Text = "Hermite曲线";
            // 
            // 二维图形变换ToolStripMenuItem
            // 
            this.二维图形变换ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PingMove,
            this.图形旋转ToolStripMenuItem});
            this.二维图形变换ToolStripMenuItem.Name = "二维图形变换ToolStripMenuItem";
            this.二维图形变换ToolStripMenuItem.Size = new System.Drawing.Size(111, 24);
            this.二维图形变换ToolStripMenuItem.Text = "二维图形变换";
            // 
            // PingMove
            // 
            this.PingMove.Name = "PingMove";
            this.PingMove.Size = new System.Drawing.Size(144, 26);
            this.PingMove.Text = "图形平移";
            this.PingMove.Click += new System.EventHandler(this.PingMove_Click);
            // 
            // 图形旋转ToolStripMenuItem
            // 
            this.图形旋转ToolStripMenuItem.Name = "图形旋转ToolStripMenuItem";
            this.图形旋转ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.图形旋转ToolStripMenuItem.Text = "图形旋转";
            this.图形旋转ToolStripMenuItem.Click += new System.EventHandler(this.图形旋转ToolStripMenuItem_Click);
            // 
            // 二维图形裁剪ToolStripMenuItem
            // 
            this.二维图形裁剪ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CohenCut,
            this.MidCut,
            this.LiangCut,
            this.WindowCut});
            this.二维图形裁剪ToolStripMenuItem.Name = "二维图形裁剪ToolStripMenuItem";
            this.二维图形裁剪ToolStripMenuItem.Size = new System.Drawing.Size(111, 24);
            this.二维图形裁剪ToolStripMenuItem.Text = "二维图形裁剪";
            // 
            // CohenCut
            // 
            this.CohenCut.Name = "CohenCut";
            this.CohenCut.Size = new System.Drawing.Size(204, 26);
            this.CohenCut.Text = "Cohen算法";
            this.CohenCut.Click += new System.EventHandler(this.CohenCut_Click);
            // 
            // MidCut
            // 
            this.MidCut.Name = "MidCut";
            this.MidCut.Size = new System.Drawing.Size(204, 26);
            this.MidCut.Text = "中点分割算法";
            this.MidCut.Click += new System.EventHandler(this.MidCut_Click);
            // 
            // LiangCut
            // 
            this.LiangCut.Name = "LiangCut";
            this.LiangCut.Size = new System.Drawing.Size(204, 26);
            this.LiangCut.Text = "梁友栋算法";
            this.LiangCut.Click += new System.EventHandler(this.LiangCut_Click);
            // 
            // WindowCut
            // 
            this.WindowCut.Name = "WindowCut";
            this.WindowCut.Size = new System.Drawing.Size(204, 26);
            this.WindowCut.Text = "窗口对多边形裁剪";
            this.WindowCut.Click += new System.EventHandler(this.WindowCut_Click);
            // 
            // 图形填充ToolStripMenuItem
            // 
            this.图形填充ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScanLineFill});
            this.图形填充ToolStripMenuItem.Name = "图形填充ToolStripMenuItem";
            this.图形填充ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.图形填充ToolStripMenuItem.Text = "图形填充";
            // 
            // ScanLineFill
            // 
            this.ScanLineFill.Name = "ScanLineFill";
            this.ScanLineFill.Size = new System.Drawing.Size(189, 26);
            this.ScanLineFill.Text = "扫描线填充算法";
            this.ScanLineFill.Click += new System.EventHandler(this.ScanLineFill_Click);
            // 
            // 投影ToolStripMenuItem
            // 
            this.投影ToolStripMenuItem.Name = "投影ToolStripMenuItem";
            this.投影ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.投影ToolStripMenuItem.Text = "投影";
            // 
            // 消隐ToolStripMenuItem
            // 
            this.消隐ToolStripMenuItem.Name = "消隐ToolStripMenuItem";
            this.消隐ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.消隐ToolStripMenuItem.Text = "消隐";
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 553);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDoubleClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 基本图形生成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DDALine;
        private System.Windows.Forms.ToolStripMenuItem MidLine;
        private System.Windows.Forms.ToolStripMenuItem bresenham直线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MidCircle;
        private System.Windows.Forms.ToolStripMenuItem BresenhamCircle;
        private System.Windows.Forms.ToolStripMenuItem 正负圆ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BezierCurve;
        private System.Windows.Forms.ToolStripMenuItem BSampleCurve;
        private System.Windows.Forms.ToolStripMenuItem hermite曲线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二维图形变换ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二维图形裁剪ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图形填充ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 投影ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 消隐ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ScanLineFill;
        private System.Windows.Forms.ToolStripMenuItem CohenCut;
        private System.Windows.Forms.ToolStripMenuItem MidCut;
        private System.Windows.Forms.ToolStripMenuItem LiangCut;
        private System.Windows.Forms.ToolStripMenuItem WindowCut;
        private System.Windows.Forms.ToolStripMenuItem PingMove;
        private System.Windows.Forms.ToolStripMenuItem 图形旋转ToolStripMenuItem;
    }
}

