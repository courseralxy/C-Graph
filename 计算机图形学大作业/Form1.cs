using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 计算机图形学大作业
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Color BackColor1 = Color.White;//DDALine，背景色
        Color ForeColor1 = Color.Black;//DDALine，笔色
        public int MenuID;//DDALine功能选择
        public int PressNum;//DDAline，识别起始点还是结束点
        public int FirstX;//DDALine，起始端点坐标
        public int FirstY;//DDALine
        public int OldX;//DDALine
        public int OldY;//DDALine

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DDALine_Click(object sender, EventArgs e)
        {
            MenuID = 1;
            PressNum = 0;
            Graphics g = CreateGraphics();//创建一张画纸
            g.Clear(BackColor1);//设置底色
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
