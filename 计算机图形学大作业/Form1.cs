﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        public struct line
        {
            public int x0;
            public int y0;
            public int x1;
            public int y1;
            public line(int _x0, int _y0,int _x1,int _y1)//有参构造函数
            {
                this.x0 = _x0;
                this.x1 = _x1;
                this.y0 = _y0;
                this.y1 = _y1;
            }
        }
        Color BackColor1 = Color.White;//DDALine，背景色
        Color ForeColor1 = Color.Black;//DDALine，笔色
        public int MenuID;//DDALine功能选择
        public int PressNum;//DDAline，识别起始点还是结束点
        public int PointNum;//图线填充，记录点个数
        public int FirstX;//DDALine，起始端点坐标
        public int FirstY;//DDALine
        public int OldX;//DDALine
        public int OldY;//DDALine
        public List<line> Line = new List<line>();
        Point[] group = new Point[100];//图形填充时用以记录顶点
        Graphics CurrentGraph; 
        //二维裁剪窗口
        public int XL, XR, YU, YD;
        Point[] pointsgroup = new Point[4];
       
        private void initializeLine(int mid)
        {
            MenuID = mid;
            PressNum = 0;
            PointNum = 0;
            CurrentGraph.Clear(BackColor1);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DDALine_Click(object sender, EventArgs e)
        {
            CurrentGraph.Clear(BackColor1);
            initializeLine(1);
            //MenuID = 1;
            //PressNum = 0;
            Graphics g = CreateGraphics();//创建一张画纸
            g.Clear(BackColor1);//设置底色
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();  //创建画纸
            Pen pen = new Pen(Color.Red, 1);
            if (MenuID == 1 || MenuID == 2)
            {
                if (PressNum == 0)
                {//起点
                    FirstX = e.X;
                    FirstY = e.Y;
                    OldX = e.X;
                    OldY = e.Y;
                }
                else//终点
                {
                    Line.Add(new CG.Form1.line(FirstX, FirstY, e.X, e.Y));//记录所有直线的两端点，后续用与直线的删除、拖动、裁剪。并在鼠标移动画线时修正原先直线被擦除的bug
                    if (MenuID == 1)
                        DDALine1(FirstX, FirstY, e.X, e.Y);
                    //g.DrawLine(pen, FirstX, FirstY, e.X, e.Y);
                    else if (MenuID == 2)
                        MidLine1(FirstX, FirstY, e.X, e.Y);
                }
                PressNum = (PressNum + 1) % 2;//画线完毕则清零
            }
            else if (MenuID == 5)//当前模式为圆规画点，后续重构为直径画圆
            {
                if (PressNum == 0)//圆心，保留
                {
                    FirstX = e.X;
                    FirstY = e.Y;
                }
                else//圆上任一点
                {
                    if ((FirstX == e.X) && (FirstY == e.Y))//半径为0，画点
                    {
                        g.DrawRectangle(Pens.Red, FirstX, FirstY, 1, 1);
                        return;
                    }
                    else BresenhamCircle1(FirstX, FirstY, e.X, e.Y);
                }
                PressNum = (PressNum + 1) % 2;
            }
            else if (MenuID == 31||MenuID == 24)//扫描线填充算法和多边形裁剪算法
            {
               if (PressNum == 0)//记录起点
               {//起点
                   OldX = e.X;
                   OldY = e.Y;
               }
               // OldX = FirstX;
               // OldY = FirstY;
               // FirstX = e.X;
               // FirstY = e.Y;
                if (e.Button == MouseButtons.Left) //左键定点
                {
                    //group[PressNum].X = e.X;
                    //group[PressNum].Y = e.Y;
                    group[PointNum].X = e.X;
                    group[PointNum].Y = e.Y;
                    if (PressNum > 0)//画出这些点
                    {
                        g.DrawLine(Pens.Red, group[PointNum - 1], group[PointNum]);
                    }
                    PointNum++;
                    PressNum++;
                }
                else if (e.Button == MouseButtons.Right)//右键结束
                {
                    g.DrawLine(Pens.Red, group[PointNum - 1], group[0]);
                    if (MenuID == 31) ScanLineFill1();//扫描线填充
                    else if (MenuID == 24) WindowCut1();//多边形裁剪
                    PointNum = 0;
                    PressNum = 0;//清零
                }

            }
            else if(MenuID == 21 || MenuID == 22||MenuID == 23)//三种裁剪算法通用
            {
                if(PressNum == 0)
                {
                    FirstX = e.X;
                    FirstY = e.Y;
                    PressNum++;
                }
                else
                {
                    if (MenuID == 21)
                        CohenCut1(FirstX, FirstY, e.X, e.Y);
                    else if (MenuID == 22)
                        MidCut1(FirstX, FirstY, e.X, e.Y);
                    else if (MenuID == 23)
                        LiangCut1(FirstX, FirstY, e.X, e.Y);
                    PressNum = 0;
                }
            }
        }
        private void DDALine1(int x0, int y0, int x1, int y1)
        {//程序仍需实现画点
            //CurrentGraph = CreateGraphics();
            int flag;
            float k;
            Graphics g = CreateGraphics();//创建画布
            if (x0 == x1 && y0 == y1)
            {
                g.DrawRectangle(Pens.Red, x0,y0, 1, 1);
                return;//端点重叠啥都没画
            }
            if (x0 == x1)//斜率为正无穷
            {
                if (y0 < y1)
                {
                    int tmp = y0;
                    y0 = y1;
                    y1 = tmp;
                }
                for (int i = y0; i <= y1; i++)
                {
                    g.DrawRectangle(Pens.Red, x0, i, 1, 1);
                }
                return;
            }
            if (y0 == y1)//斜率为0，水平
            {

                if (x0 > x1)
                {
                    int tmp = x0;
                    x0 = x1;
                    x1 = tmp;
                }
                for (int i = x0; i <= x1; i++)
                {
                    g.DrawRectangle(Pens.Red, i, y0, 1, 1);
                }
                return;

            }
            if (x0 > x1)
            {
                int tmp = x0;
                x0 = x1;
                x1 = tmp;
                tmp = y0;
                y0 = y1;
                y1 = tmp;
            }
            flag = 0;//直线种类
            if (((x1 - x0) > (y1 - y0)) && ((y1 - y0) > 0)) flag = 1;//第一种，无需转化
            else if (((x1 - x0) > (y0 - y1)) && ((y0 - y1) > 0))//第二种，进行转化
            {
                flag = 2;
                y0 = -y0; y1 = -y1;
            }
            else if ((y1 - y0) > (x1 - x0))//第三种,斜率大于1
            {
                flag = 3;
                int tmp = x0;
                x0 = y0; y0 = tmp;
                tmp = x1;
                x1 = y1; y1 = tmp;
            }
            else if ((y0 - y1) > (x1 - x0))//第四种
            {
                flag = 4;
                int tmp = x0;
                x0 = -y0; y0 = tmp;
                tmp = x1;
                x1 = -y1; y1 = tmp;

            }
            k = (float)(y1 - y0) / (float)(x1 - x0);
            int x = x0;
            float y = (float)y0;
            //Console.WriteLine(flag);
            for (; x <= x1; x++, y += k)
            {
                switch (flag)
                {
                    case 1:
                        g.DrawRectangle(Pens.Red, x, (int)(y + 0.5), 1, 1);
                        break;
                    case 2:
                        g.DrawRectangle(Pens.Red, x, -(int)(y + 0.5), 1, 1);
                        break;
                    case 3:
                        g.DrawRectangle(Pens.Red, (int)(y + 0.5), x, 1, 1);
                        break;
                    case 4:
                        g.DrawRectangle(Pens.Red, (int)(y + 0.5), -x, 1, 1);
                        break;
                    default:
                        break;
                }
            }
            CurrentGraph = g;
        }
        private void MidLine1(int x0, int y0, int x1, int y1)
        {
            int flag;
            Graphics g = CreateGraphics();//创建画布
            if (x0 == x1 && y0 == y1)
            {
                g.DrawRectangle(Pens.Red, x0, y0, 1, 1);
                CurrentGraph = g;
                return;//画点
            }
            else if(x0 == x1)//竖直线
            {
                if(y0 > y1)
                {
                    int tmp = y0;
                    y0 = y1;
                    y1 = y0;
                }
                for(int i = y0;i <= y1; i++)
                {
                    g.DrawRectangle(Pens.Red, x1, i, 1, 1);//画点
                }
                CurrentGraph = g;
                return;
            }
            else if(y0 == y1)//水平线
            {
                if(x0 > x1)
                {
                    int tmp = x0;
                    x0 = x1;
                    x1 = tmp;
                }
                for(int i = x0;i <= x1; i++)
                {
                    g.DrawRectangle(Pens.Red, i, y1, 1, 1);//画点
                }
                CurrentGraph = g;
                return;
            }
            if(x0 > x1)//将左边的作为起点
            {
                int tmp = x0;
                x0 = x1;
                x1 = tmp;

                tmp = y0;
                y0 = y1;
                y1 = tmp;
            }
            flag = 0;
            int deltaX = x1 - x0;
            int deltaY = y1 - y0;
            if ((deltaX > deltaY) && deltaY > 0) flag = 1;
            else if((deltaX > (-deltaY))&&((-deltaY) > 0))
            {
                flag = 2;
                y0 = -y0;
                y1 = -y1;
            }
            else if(deltaY > deltaX)
            {
                flag = 3;
                int tmp = x0;
                x0 = y0;
                y0 = tmp;

                tmp = x1;
                x1 = y1;
                y1 = tmp;
            }
            else if((-deltaY) > deltaX)
            {
                flag = 4;
                int tmp = x0;
                x0 = -y0;
                y0 = tmp;
                tmp = x1;
                x1 = -y1;
                y1 = tmp;

            }
            int x = x0, y = y0, d = (x1 - x0) - 2 * (y1 - y0);
            deltaX = x1 - x0;
            deltaY = y1 - y0;
            while (x < x1 + 1)
            {
                switch(flag)
                {
                    case 1:
                        g.DrawRectangle(Pens.Red, x, y, 1, 1);
                        break;
                    case 2:
                        g.DrawRectangle(Pens.Red, x, -y, 1, 1);
                        break;
                    case 3:
                        g.DrawRectangle(Pens.Red, y, x, 1, 1);
                        break;
                    case 4:
                        g.DrawRectangle(Pens.Red, y, -x, 1, 1);
                        break;
                    default:
                        break;
                }
                x++;
                if (d > 0)
                    d = d - 2 * deltaY;
                else
                {
                    y++;
                    d = d - 2 * (deltaY - deltaX);
                }
            }
            CurrentGraph = g;
        }
        private void BresenhamCircle1(int x0, int y0, int x1, int y1)
        {

            Graphics g = CreateGraphics();//创建画布
            int deltaX = x1 - x0, deltaY = y1 - y0;
            int r = (int)(Math.Sqrt(deltaX * deltaX + deltaY * deltaY) + 0.5);
            int x = 0, y = r, d = 3 - 2 * r;
            while(x < y || x == y)
            {
                g.DrawRectangle(Pens.Blue, x + x0, y + y0, 1, 1);
                g.DrawRectangle(Pens.Red, -x + x0, y + y0, 1, 1);
                g.DrawRectangle(Pens.Green, x + x0, -y + y0, 1, 1);
                g.DrawRectangle(Pens.Yellow, -x + x0, -y + y0, 1, 1);

                g.DrawRectangle(Pens.Black, y + x0, x + y0, 1, 1);
                g.DrawRectangle(Pens.Pink, -y + x0, x + y0, 1, 1);
                g.DrawRectangle(Pens.Purple, y + x0, -x + y0, 1, 1);
                g.DrawRectangle(Pens.Gray, -y + x0, -x + y0, 1, 1);
                x++;
                if (d < 0 || d == 0)
                    d = d + 4 * x + 6;
                else
                {
                    y = y - 1;
                    d = d + 4 * (x - y) + 10;
                }
            }
            CurrentGraph = g;
        }
        public struct EdgeInfo
        {
            int ymax, ymin;//Y的上下端点
            float k, xmin;//斜率倒数和X的下端点

            //为四个内部变量设置公共变量，方便对数据进行存取
            public int YMax { get { return ymax; } set { ymax = value; } }
            public int YMin { get { return ymin; } set { ymin = value; } }

            public float XMin { get { return xmin; } set { xmin = value; } }

            public float K { get { return k; } set { k = value; } }

            public EdgeInfo(int x1, int y1, int x2, int y2)//(x1, y1)上端点(x2, y2)下端点
            {
                ymax = y2;
                ymin = y1;
                xmin = (float)x1;
                k = (float)(x1 - x2) / (float)(y1 - y2);
            }
        }
        private void ScanLineFill1()
        {
            int edgesize = 0;
            int yu = 0, yd = 600;//活化边扫描范围
            EdgeInfo[] edgelist = new EdgeInfo[100];//存边
            group[PressNum] = group[0];
            for(int i = 0; i < PressNum;i++)
            {
                if (group[i].Y > yu) yu = group[i].Y;//最高点
                if (group[i].Y < yd) yd = group[i].Y;//最低点
                if(group[i].Y!=group[i+1].Y)//只处理非水平边
                {
                    if(group[i].Y > group[i+1].Y)//下端点在前，上端点在后
                    {
                        edgelist[edgesize++] = new EdgeInfo(group[i + 1].X, group[i + 1].Y, group[i].X, group[i].Y);
                    }
                    else
                    {
                        edgelist[edgesize++] = new EdgeInfo(group[i].X, group[i].Y, group[i + 1].X, group[i + 1].Y);
                    }
                }
            }
            Graphics g = CreateGraphics();
            for (int y = yd; y < yu; y++)//AEL表操作
            {
                var sorted =
                    from item in edgelist
                    where y < item.YMax && y >= item.YMin
                    orderby item.XMin, item.K
                    select item;

                int flag = 0;//设置一个变量用于记录是线段的第几个点
                foreach (var item in sorted)
                {
                    if (flag == 0)//起点
                    {
                        FirstX = (int)(item.XMin + 0.5);
                        flag++;
                    }
                    else//终点
                    {
                        g.DrawLine(Pens.Blue, (int)(item.XMin + 0.5), y, FirstX - 1, y);
                        flag = 0;
                    }
                }
                for (int i = 0; i < edgesize; i++)//将dx加到x上
                {
                    if (y < edgelist[i].YMax - 1 && y > edgelist[i].YMin)//选出与扫描线相交的边
                    {
                        edgelist[i].XMin += edgelist[i].K;
                    }
                }
            }
            CurrentGraph = g;
        }
        private int encode(int x, int y)
        {
            int code = 0;
            if (x >= XL && x <= XR && y >= YD && y <= YU) code = 0;

            if (x < XL && y >= YD && y <= YU) code = 1;
            if (x > XR && y >= YD && y <= YU) code = 2;

            if (x >= XL && x <= XR && y > YU) code = 8;
            if (x >= XL && x <= XR && y < YD) code = 4;

            if (x <= XL && y > YU) code = 9;

            if (x >= XR && y > YU) code = 10;
            if (x <= XL && y < YD) code = 5;
            if (x >= XR && y < YD) code = 6;
            return code;
        }
        private void CohenCut1(int x1, int y1, int x2, int y2)
        {
            int code1 = 0, code2 = 0, code, x = 0, y = 0;
            Graphics g = CreateGraphics();

            g.DrawLine(Pens.Red, x1, y1, x2, y2);
            CurrentGraph = g;
            code1 = encode(x1, y1);
            code2 = encode(x2, y2);
            while(code1 != 0 || code2 !=0)
            {
                if ((code1 & code2) != 0)
                    return;
                code = code1;
                if (code1 == 0) code = code2;
                if((1&code)!=0)
                {
                    x = XL;
                    y = y1 + (y2 - y1) * (x - x1) / (x2 - x1);
                }
                else if((2&code)!=0)
                {
                    x = XR;
                    y = y1 + (y2 - y1) * (x - x1) / (x2 - x1);
                }
                else if((4&code)!=0)
                {
                    y = YD;
                    x = x1 + (x2 - x1) * (y - y1) / (y2 - y1);
                }
                else if((8&code)!=0)
                {
                    y = YU;
                    x = x1 + (x2 - x1) * (y - y1) / (y2 - y1);
                }
                if(code == code1)
                {
                    x1 = x;
                    y1 = y;
                    code1 = encode(x, y);
                }
                else
                {
                    x2 = x;
                    y2 = y;
                    code2 = encode(x, y);
                }
            }
            Pen DrawPen = new Pen(Color.Yellow, 3);
            g.DrawLine(DrawPen, x1, y1, x2, y2);
            CurrentGraph = g;
        }      

        private bool LineIsOutOfWindow(int x1, int y1, int x2, int y2)
        {
            if (x1 < XL && x2 < XL)
                return true;
            else if (x1 > XR && x2 > XR)
                return true;
            else if (y1 > YU && y2 > YU)
                return true;
            else if (y1 < YD && y2 < YD)
                return true;
            else
                return false;
        }
        private bool PointIsOutOfWindow(int x, int y)
        {
            if (x < XL)
                return true;
            else if (x > XR)
                return true;
            else if (y > YU)
                return true;
            else if (y < YD)
                return true;
            else
                return false;
        }
        private Point FindNearestPoint(int x1, int y1, int x2, int y2)
        {
            int x = 0, y = 0;
            Point p = new Point(0, 0);
            if(!PointIsOutOfWindow(x1, y1))
            {
                p.X = x1;
                p.Y = y1;
                return p;
            }
            while(!(Math.Abs(x1 - x2) <= 1 && Math.Abs(y1-y2)<=1))
            {
                x = (x1 + x2) / 2;
                y = (y1 + y2) / 2;
                if(LineIsOutOfWindow(x1, y1, x, y))
                {
                    x1 = x; y1 = y;
                }
                else
                {
                    x2 = x;y2 = y;
                }
            }
            if(PointIsOutOfWindow(x1, y1))
            {
                p.X = x2;
                p.Y = y2;
            }
            else
            {
                p.X = x1;
                p.Y = y1;
            }
            return p;
        }
        private void MidCut1(int x1, int y1, int x2, int y2)
        {
            Graphics g = CreateGraphics();
            g.DrawLine(Pens.Red, x1, y1, x2, y2);
            CurrentGraph = g;
            Point p1, p2;
            if (LineIsOutOfWindow(x1, y1, x2, y2))
                return;
            p1 = FindNearestPoint(x1, y1, x2, y2);
            if (PointIsOutOfWindow(p1.X, p1.Y))
                return;
            p2 = FindNearestPoint(x2, y2, x1, y1);
            Pen DrawPen = new Pen(Color.Yellow, 3);
            g.DrawLine(DrawPen, p1, p2);
            CurrentGraph = g;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();//创建画纸
            Pen BackgroundPen = new Pen(BackColor1,1);
            Pen DrawPen = new Pen(ForeColor1, 1);
            if((MenuID == 1 || MenuID == 2)&& PressNum == 1)
            {
                if(!(e.X == OldX && e.Y == OldY))
                {
                    g.DrawLine(BackgroundPen, FirstX, FirstY, OldX, OldY);//擦掉原来的。因此解决办法是，将确定的直线的画布记录下来，然后每次画新的move直线都是在确定直线画布的拷贝上画，就不需要擦除了
                    g.DrawLine(DrawPen, FirstX, FirstY, e.X, e.Y);
                    OldX = e.X;
                    OldY = e.Y;
                }
            }
            else if((MenuID == 5) && (PressNum == 1))
            {
                if(!((e.X == OldX)&&(e.Y == OldY)))
                {
                    int deltaX = FirstX - OldX, deltaY = FirstY - OldY;
                    double r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);//半径
                    int r1 = (int)(r + 0.5);//取整
                    g.DrawEllipse(BackgroundPen, FirstX - r1, FirstY - r1, 2 * r1, 2 * r1);//擦除旧圆
                    deltaX = FirstX - e.X;
                    deltaY = FirstY - e.Y;
                    r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    r1 = (int)(r + 0.5);//取整
                    g.DrawEllipse(DrawPen, FirstX - r1, FirstY - r1, 2 * r1, 2 * r1);//画新圆
                    OldX = e.X;
                    OldY = e.Y;
                }
            }
            else if((MenuID == 31 || MenuID == 24)&& PressNum > 0)
            {
                {
                    if(!(e.X == OldX && e.Y == OldY))
                    {
                        g.DrawLine(BackgroundPen, group[PressNum - 1].X, group[PressNum - 1].Y, OldX, OldY);
                        g.DrawLine(DrawPen, group[PressNum - 1].X, group[PressNum- 1].Y, e.X, e.Y);
                        OldX = e.X;
                        OldY = e.Y;
                    }
                }
            }
            CurrentGraph = g;
        }
      

        private void MidLine_Click(object sender, EventArgs e)
        {
            initializeLine(2);
            //MenuID = 2;
            //PressNum = 0;//初始化
            CurrentGraph.Clear(BackColor1);
            Graphics g = CreateGraphics();
            g.Clear(BackColor1);
        }

        private void MidCut_Click(object sender, EventArgs e)
        {
            initializeLine(22);
            CurrentGraph.Clear(BackColor1);
            Graphics g = CreateGraphics();
            XL = 100;
            XR = 400;
            YD = 100;
            YU = 400;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);
            CurrentGraph = g;
        }

        private void 中点圆ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void LiangCut1(int x0, int y0, int x1, int y1)
        {
            Graphics g = CreateGraphics();
            g.DrawLine(Pens.Red, x0, y0, x1, y1);
            float tsx, tsy, tex, tey;//设置两个始边、两个终边对应的t参数
            if(x0==x1)//竖直线
            {
                //？？特殊情况统一设置以便后续统一？？？
                tsx = 0;
                tex = 1;
            }
            else if(x0 < x1)
            {
                //满足条件，X方向起始边为XL，终点边为XR，可直接计算对应参数
                tsx = (float)(XL - x0) / (float)(x1 - x0);
                tex = (float)(XR - x0) / (float)(x1 - x0);
            }
            else
            {
                //如果条件不满足，X方向起始边为XR，终点边为XL，仍可直接计算
                tsx = (float)(XR - x0) / (float)(x1 - x0);
                tex = (float)(XL - x0) / (float)(x1 - x0);
            }
            if (y0 == y1)//水平线
            {
                //？？？又是特殊情况，这样设置可以使后续工作方式统一？？
                tsy = 0;
                tey = 1;
            }
            else if(y0 < y1){
                //满足条件，Y方向起始边为YD
                tsy = (float)(YD - y0) / (float)(y1 - y0);
                tey = (float)(YU - y0) / (float)(y1 - y0);
            }
            else
            {
                tsy = (float)(YU - y0) / (float)(y1 - y0);
                tey = (float)(YD - y0) / (float)(y1 - y0);
            }

            tsx = Math.Max(tsx, tsy);
            tsx = Math.Max(tsx, 0);

            tex = Math.Min(tex, tey);
            tex = Math.Min(tex, 1);
            if(tsx < tex)//该条件满足，裁剪结果才有可见部分
            {
                int xx0, yy0, xx1, yy1;
                xx0 = (int)(x0 + (x1 - x0) * tsx);
                yy0 = (int)(y0 + (y1 - y0) * tsx);

                xx1 = (int)(x0 + (x1 - x0) * tex);
                yy1 = (int)(y0 + (y1 - y0) * tex);

                Pen DrawPen = new Pen(Color.Yellow, 3);
                g.DrawLine(DrawPen, xx0, yy0, xx1, yy1);

            }

        }
        private void LiangCut_Click(object sender, EventArgs e)
        {
            initializeLine(23);
            Graphics g = CreateGraphics();

            XL = 100;
            XR = 400;
            YD = 100;
            YU = 400;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);

        }

        private void EdgeClipping(int linecode)
        {
            float x, y;
            int n, i, number1;
            Point[] q = new Point[200];//裁剪结果
            number1 = 0;
            if (linecode == 0)
            {
                for (n = 0; n < PressNum; n++)
                {
                    if (group[n].X < XL && group[n + 1].X < XL)
                        //q[number1++] = group[n];

                        ;
                    else if (group[n].X >= XL && group[n + 1].X >= XL)//都在内，输出后点
                        q[number1++] = group[n + 1];
                    else if (group[n].X >= XL && group[n + 1].X < XL)
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XL - group[n].X);
                        q[number1].X = XL;
                        q[number1++].Y = (int)y;
                    }
                    else if (group[n].X < XL && group[n + 1].X >= XL)//一内一外，输出与边框交点
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XL - group[n].X);
                        q[number1].X = XL;
                        q[number1++].Y = (int)y;
                        q[number1++] = group[n + 1];
                    }
                }
                for (i = 0; i < number1; i++)//裁剪结果放入group数组
                    group[i] = q[i];
                group[number1] = q[0];
                PressNum += number1;
            }
            else if (linecode == 1)
            {
                for (n = 0; n < PressNum; n++)
                {
                    if (group[n].Y >= YU && group[n + 1].Y >= YU)
                        //q[number1++] = group[n];
                        ;
                    else if (group[n].Y < YU && group[n + 1].Y < YU)//都在内，输出后点
                        q[number1++] = group[n + 1];
                    else if (group[n].Y < YU && group[n + 1].Y >= YU)
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YU - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1++].Y = YU;
                    }
                    else if (group[n].Y >= YU && group[n + 1].Y < YU)//一内一外，输出与边框交点
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YU - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1++].Y = YU;
                        q[number1++] = group[n + 1];
                    }
                    for (i = 0; i < number1; i++)//裁剪结果放入group数组
                        group[i] = q[i];
                    group[number1] = q[0];
                    PressNum = number1;
                }
            }
            else if (linecode == 2)
            {
                for (n = 0; n < PressNum; n++)
                {
                    if (group[n].X >= XR && group[n + 1].X >= XR)
                        // q[number1++]=group[n];
                        ;
                    else if (group[n].X < XR && group[n + 1].X < XR)//都在内，输出后点
                        q[number1++] = group[n + 1];
                    else if (group[n].X < XR && group[n + 1].X >= XR)
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XR - group[n].X);
                        q[number1].X = XR;
                        q[number1++].Y = (int)y;
                    }
                    else if (group[n].X >= XR && group[n + 1].X < XR)//一内一外，输出与边框交点
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XR - group[n].X);
                        q[number1].X = XR;
                        q[number1++].Y = (int)y;
                        q[number1++] = group[n + 1];
                    }
                }
                for (i = 0; i < number1; i++)//裁剪结果放入group数组
                    group[i] = q[i];
                group[number1] = q[0];
                PressNum = number1;
            }
            else
            {
                for (n = 0; n < PressNum; n++)
                {
                    if (group[n].Y < YD && group[n + 1].Y < YD)
                        //q[number1++]=group[n];
                        ;
                    else if (group[n].Y >= YD && group[n + 1].Y >= YD)//都在内，输出后点
                        q[number1++] = group[n + 1];
                    else if (group[n].Y >= YD && group[n + 1].Y < YD)
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YD - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1++].Y = YD;
                    }
                    else if (group[n].Y < YD && group[n + 1].Y >= YD)//一内一外，输出与边框交点
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YD - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1++].Y = YD;
                        q[number1++] = group[n + 1];
                    }
                    for (i = 0; i < number1; i++)//裁剪结果放入group数组
                        group[i] = q[i];
                    group[number1] = q[0];
                    PressNum = number1;
                }
            }
        }
        private void WindowCut1()
        {
            group[PressNum] = group[0];
            for(int i = 0; i < 4; i++)
            {
                EdgeClipping(i);
            }
            Graphics g = CreateGraphics();

            Pen DrawPen = new Pen(Color.Yellow, 3);
            for(int i = 0;i < PressNum; i++)//绘制裁剪多边形
            {
                g.DrawLine(DrawPen, group[i], group[i + 1]);
            }

        }
        private void WindowCut_Click(object sender, EventArgs e)
        {
            initializeLine(24);
            Graphics g = CreateGraphics();
            g.Clear(BackColor1);

            XL = 100;
            XR = 400;
            YD = 100;
            YU = 400;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);
        }

        private void BresenhamCircle_Click(object sender, EventArgs e)
        {
            initializeLine(5);//圆与直线初始化通用
            CurrentGraph.Clear(BackColor1);
            Graphics g = CreateGraphics();//生成画布
            g.Clear(BackColor1);//画布上色
        }

        private void ScanLineFill_Click(object sender, EventArgs e)
        {
            MenuID = 31;//扫描线填充算法
            initializeLine(31);
            CurrentGraph.Clear(BackColor1);
            Graphics g = CreateGraphics();//创建画布
            g.Clear(BackColor1);//背景填充
        }

        private void CohenCut_Click(object sender, EventArgs e)
        {
            initializeLine(21);
            CurrentGraph.Clear(BackColor1);
            Graphics g = CreateGraphics();
            XL = 100;
            XR = 400;
            YD = 100;
            YU = 400;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);
            CurrentGraph = g;

        }
    }
}
