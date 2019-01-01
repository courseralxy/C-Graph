
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CG
{
    public partial class Form1 : Form
    {
        double Pi = 3.1415926;
        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(900,600);
            Graphics G = Graphics.FromImage(bm);
            G.Clear(BackColor1);
            //bm = new Bitmap("default.bmp");
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
        public int SaveNumber;//Bezier曲线，控制点编号
        public int FirstX;//DDALine，起始端点坐标
        public int FirstY;//DDALine
        public int OldX;//DDALine
        public int OldY;//DDALine
        public List<line> Line = new List<line>();//没有用到
        Point[] group = new Point[100];//图形填充时用以记录顶点
        Bitmap bm;//位图，画布，用以储存正式图像和保存文件
        //二维裁剪窗口
        public int XL, XR, YU, YD;//裁剪算法生成框图
        Point[] pointsgroup = new Point[4];
       
        private void initializeLine(int mid)//初始化
        {
            MenuID = mid;
            PressNum = 0;
            PointNum = 0;
            //CurrentGraph.Clear(BackColor1);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)//退出
        {
            this.Close();
        }

        private void DDALine_Click(object sender, EventArgs e)//选择DDA直线功能
        {
          
            initializeLine(1);
            //MenuID = 1;
            //PressNum = 0;
            Graphics g = CreateGraphics();//创建一张画纸
            g.Clear(BackColor1);//设置底色

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(bm);  //创建画纸
            Graphics G = CreateGraphics();
            Pen pen = new Pen(Color.Red, 1);
            if (MenuID == 1 || MenuID == 2)//DDA直线和中点直线
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

            else if (MenuID == 5)//画圆
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
                    //else BresenhamCircle1(FirstX, FirstY, e.X, e.Y);
                    else BresenhamCircle1((FirstX + e.X) / 2, (FirstY + e.Y) / 2, e.X, e.Y);
                }
                PressNum = (PressNum + 1) % 2;
            }
            
            else if(MenuID == 1111)//画窗口
            {
                if(PressNum == 0)
                {
                    FirstX = e.X;
                    FirstY = e.Y;
                    PressNum++;
                }
                else
                {
                    if ((FirstX == e.X) && (FirstY == e.Y)){

                    }
                    else
                    {
                        XL = FirstX;
                        XR = e.X;
                        YD = FirstY;
                        YU = e.Y;
                        if(XL > XR)
                        {
                            int tmp = XL;
                            XL = XR;
                            XR = tmp;
                        }
                        if(YD > YU)
                        {
                            int tmp = YD;
                            YD = YU;
                            YU = tmp;
                        }
                        g.DrawRectangle(Pens.Blue, XL, YD, XR - XL, YU - YD);
                    }
                    PressNum++;
                }
                //PressNum = (PressNum + 1) % 2;
            }

            else if(MenuID == 11)
            {
                if(PressNum == 0)
                {
                    FirstX = e.X;
                    FirstY = e.Y;

                }
                else
                {
                    for(int i = 0; i < 4; i++)
                    {
                        pointsgroup[i].X += e.X - FirstX;
                        pointsgroup[i].Y += e.Y - FirstY;
                    }
                    g.DrawPolygon(Pens.Blue, pointsgroup);
                }
                PressNum = (PressNum + 1) % 2;
            }
            else if(MenuID == 12)//图形旋转功能
            {
                if (PressNum == 0)
                {
                    FirstX = e.X;
                    FirstY = e.Y;
                }
                else
                {
                    double a;
                    if (e.X == FirstX && e.Y == FirstY)//两点重合，不旋转
                        return;
                    if (e.X == FirstX && e.Y > FirstY)//分母为零
                        a = Pi / 2.0;
                    else if (e.X == FirstX && e.Y < FirstY)
                    {
                        a = Pi / 2.0 * 3.0;
                    }
                    else
                        a = Math.Atan((double)(e.Y - FirstY) / (double)(e.X - FirstX));
                    a = a / Pi * 180.0;//转化为角度
                    int x0 = 150, y0 = 150;
                    Matrix DrawMatrix = new Matrix();//利用矩阵计算
                    DrawMatrix.Translate(-x0, -y0);
                    DrawMatrix.Rotate((float)a, MatrixOrder.Append);
                    DrawMatrix.Translate(x0, y0, MatrixOrder.Append);

                    //Graphics g = CreateGraphics();
                    g.Transform = DrawMatrix;
                    g.DrawPolygon(Pens.Blue, pointsgroup);
                }
                PressNum = (PressNum + 1) % 2;
            }
            else if (MenuID == 24)
            {
                if (PressNum == 0)
                {
                    FirstX = e.X;
                    FirstY = e.Y;
                    PressNum++;
                }
                else if (PressNum == 1)
                {
                    if ((FirstX == e.X) && (FirstY == e.Y))
                    {

                    }
                    else
                    {
                        XL = FirstX;
                        XR = e.X;
                        YD = FirstY;
                        YU = e.Y;
                        if (XL > XR)
                        {
                            int tmp = XL;
                            XL = XR;
                            XR = tmp;
                        }
                        if (YD > YU)
                        {
                            int tmp = YD;
                            YD = YU;
                            YU = tmp;
                        }
                        g.DrawRectangle(Pens.Blue, XL, YD, XR - XL, YU - YD);
                    }
                    PressNum++;
                }
                else
                {
 
                    if (PressNum == 2)//记录起点
                    {//起点
                        OldX = e.X;
                        OldY = e.Y;
                        //group[0].X = OldX;
                        //group[0].Y = OldY;
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
                        if (PressNum > 2)//画出这些点
                        { 
                            g.DrawLine(Pens.Red, group[PointNum - 1], group[PointNum]);
                            G.DrawImage(bm,new Point(0,0));
                        }
                        PointNum++;
                        PressNum++;
                    }
                    else if (e.Button == MouseButtons.Right)//右键结束
                    {
                        g.DrawLine(Pens.Red, group[PointNum - 1], group[0]);
                        //if (MenuID == 31) ScanLineFill1();//扫描线填充
                        //else if (MenuID == 24)
                            WindowCut1();//多边形裁剪
                        PointNum = 0;
                        PressNum = 2;//清零
                    }
                }
            }
            else if (MenuID == 31)//扫描线填充算法和多边形裁剪算法
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
                if (PressNum == 0)
                {
                    FirstX = e.X;
                    FirstY = e.Y;
                    PressNum++;
                }
                else if(PressNum == 1)
                {
                    if ((FirstX == e.X) && (FirstY == e.Y))
                    {

                    }
                    else
                    {
                        XL = FirstX;
                        XR = e.X;
                        YD = FirstY;
                        YU = e.Y;
                        if (XL > XR)
                        {
                            int tmp = XL;
                            XL = XR;
                            XR = tmp;
                        }
                        if (YD > YU)
                        {
                            int tmp = YD;
                            YD = YU;
                            YU = tmp;
                        }
                        g.DrawRectangle(Pens.Blue, XL, YD, XR - XL, YU - YD);
                        PressNum++;
                    }
                    
                }
                
                else if (PressNum == 2)
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
                    PressNum = 2;
                }
            }
            else if (MenuID == 7 || MenuID == 8)//Bizier曲线和B样条曲线
            {
                if (e.Button == MouseButtons.Left)
                {
                    group[PointNum].X = e.X;
                    group[PointNum].Y = e.Y;
                    PointNum++;
                    g.DrawLine(Pens.Black, e.X - 5, e.Y, e.X + 5, e.Y);//Bezier曲线选点并做十字标志
                    g.DrawLine(Pens.Black, e.X, e.Y - 5, e.X, e.Y + 5);

                    PressNum = 1;
                }
                if ((e.Button == MouseButtons.Right) && (PointNum > 3))
                {
                    if (MenuID == 7)
                    {
                        Bezier1(1);//绘制曲线
                        MenuID = 107;//讲后续操作改为修改控制点位置
                    }
                    else
                    {
                        BSample1(1);//绘制曲线
                        MenuID = 108;
                    }
                    PressNum = 0;
                }
            }
            else if(MenuID == 107 || MenuID == 108)
            {
                if((e.Button == MouseButtons.Left)&&PressNum == 0)
                {
                    for(int i = 0;i<PointNum;i++)
                    {
                        if((e.X>=(group[i].X - 5))&&(e.X<=group[i].X+ 5)&&(e.Y>=group[i].Y-5)&&(e.Y<=group[i].Y+5))
                        {
                            SaveNumber = i;
                            PressNum = 1;
                        }
                    }
                }
                else if((e.Button == MouseButtons.Right)&&(PointNum > 3))
                {
                    PressNum = 0;
                }
            }
        }
        private void DDALine1(int x0, int y0, int x1, int y1)
        {//程序仍需实现画点
            
            int flag;
            float k;
            Graphics g = Graphics.FromImage(bm);//创建画布
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
            Graphics output = CreateGraphics();
            output.DrawImage(bm, new Point(0, 0));
        }

        
        private void MidLine1(int x0, int y0, int x1, int y1)
        {
            Graphics g = Graphics.FromImage(bm);
            Graphics G = CreateGraphics();//创建画布

            int flag;
           
            if (x0 == x1 && y0 == y1)
            {
                g.DrawRectangle(Pens.Red, x0, y0, 1, 1);
               
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
               
                G.DrawImage(bm, new Point(0, 0));
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
                
                G.DrawImage(bm, new Point(0, 0));
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
           
            G.DrawImage(bm, new Point(0, 0));
        }
        private void BresenhamCircle1(int x0, int y0, int x1, int y1)
        {

            Graphics g = Graphics.FromImage(bm);//创建画布
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
            Graphics G = CreateGraphics();//创建画布
            G.DrawImage(bm, new Point(0, 0));
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
            Graphics g = Graphics.FromImage(bm);
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
            Graphics G = CreateGraphics();//创建画布
            G.DrawImage(bm, new Point(0, 0));
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
            Graphics g = Graphics.FromImage(bm);

            g.DrawLine(Pens.Red, x1, y1, x2, y2);
            
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
            Graphics G = CreateGraphics();//创建画布
            G.DrawImage(bm, new Point(0, 0));

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
            Graphics g = Graphics.FromImage(bm);
            g.DrawLine(Pens.Red, x1, y1, x2, y2);
            
            Point p1, p2;
            if (LineIsOutOfWindow(x1, y1, x2, y2))
                return;
            p1 = FindNearestPoint(x1, y1, x2, y2);
            if (PointIsOutOfWindow(p1.X, p1.Y))
                return;
            p2 = FindNearestPoint(x2, y2, x1, y1);
            Pen DrawPen = new Pen(Color.Yellow, 3);
            g.DrawLine(DrawPen, p1, p2);
            Graphics G = CreateGraphics();//创建画布
            G.DrawImage(bm, new Point(0, 0));
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();//创建画纸
            g.DrawImage(bm, new Point(0, 0));
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
                    g.DrawEllipse(BackgroundPen, (FirstX+OldX)/2-r1/2 , (FirstY+OldY)/2-r1/2 , r1, r1);//擦除旧圆
                    deltaX = FirstX - e.X;
                    deltaY = FirstY - e.Y;
                    r = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    r1 = (int)(r + 0.5);//取整
                    
                    g.DrawEllipse(DrawPen, (FirstX+e.X)/2-r1/2, (FirstY+e.Y)/2-r1/2 , r1, r1);//画新圆
                    OldX = e.X;
                    OldY = e.Y;
                }
            }
            else if ((MenuID == 1111||MenuID==21 || MenuID == 22 || MenuID == 23||MenuID==24) &&(PressNum == 1))
            {
                if (!((e.X == OldX) && (e.Y == OldY)))
                {
                    //g.DrawRectangle(Pens.Blue,FirstX, FirstY, e.X - FirstX, e.Y - FirstY);
                    g.DrawLine(Pens.White, FirstX, FirstY, OldX, FirstY);
                    g.DrawLine(Pens.White, FirstX, FirstY, FirstX, OldY);
                    g.DrawLine(Pens.White, OldX, FirstY, OldX, OldY);
                    g.DrawLine(Pens.White, FirstX, OldY, e.X, e.Y);

                    g.DrawLine(Pens.Blue, FirstX, FirstY, FirstX, e.Y);
                    g.DrawLine(Pens.Blue, FirstX, FirstY, e.X, FirstY);
                    g.DrawLine(Pens.Blue, e.X, FirstY, e.X, e.Y);
                    g.DrawLine(Pens.Blue, FirstX, e.Y, e.X, e.Y);
                    OldX = e.X;
                    OldY = e.Y;
                }
            }
            else if (MenuID == 24 && PressNum > 2)
            {
                if (!(e.X == OldX && e.Y == OldY))
                {
                    //g.DrawLine(BackgroundPen, group[PressNum - 1].X, group[PressNum - 1].Y, OldX, OldY);
                    g.DrawLine(DrawPen, group[PressNum-2 - 1].X, group[PressNum-2 - 1].Y, e.X, e.Y);
                    OldX = e.X;
                    OldY = e.Y;
                }
            }
            else if((MenuID == 31)&& PressNum > 0)
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
            else if(((MenuID == 107)||(MenuID == 108)) && (PressNum > 0))
            {
                if(!((group[SaveNumber].X==e.X)&&(group[SaveNumber].Y == e.Y)))
                {
                    g.DrawLine(BackgroundPen, group[SaveNumber].X - 5, group[SaveNumber].Y, group[SaveNumber].X + 5, group[SaveNumber].Y);
                    g.DrawLine(BackgroundPen, group[SaveNumber].X, group[SaveNumber].Y - 5, group[SaveNumber].X, group[SaveNumber].Y + 5);
                    if (MenuID == 107)
                        Bezier1(0);//擦除十字标志和旧线
                    else
                        BSample1(0);
                    g.DrawLine(DrawPen, e.X - 5, e.Y, e.X + 5, e.Y);
                    g.DrawLine(DrawPen, e.X, e.Y - 5, e.X, e.Y + 5);
                    group[SaveNumber].X = e.X;
                    group[SaveNumber].Y = e.Y;
                    if (MenuID == 107)
                        Bezier1(1);
                    else
                        BSample1(1);

               
                }
            }
            
        }
      

        private void MidLine_Click(object sender, EventArgs e)
        {
            initializeLine(2);
            //MenuID = 2;
            //PressNum = 0;//初始化
            
            Graphics g = CreateGraphics();
            g.Clear(BackColor1);
        }

        private void MidCut_Click(object sender, EventArgs e)
        {
            initializeLine(22);
            /*
            Graphics g = Graphics.FromImage(bm);
            XL = 100;
            XR = 400;
            YD = 100;
            YU = 400;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);
            Graphics G = CreateGraphics();
            G.DrawImage(bm, new Point(0, 0));
           */
        }

        private void 中点圆ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void LiangCut1(int x0, int y0, int x1, int y1)
        {
            Graphics g = Graphics.FromImage(bm);
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
            Graphics G = CreateGraphics();//创建画布
            G.DrawImage(bm, new Point(0, 0));
        }
        private void LiangCut_Click(object sender, EventArgs e)
        {
            initializeLine(23);
            /*
            Graphics g = Graphics.FromImage(bm);

            XL = 100;
            XR = 400;
            YD = 100;
            YU = 400;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);
            Graphics G = CreateGraphics();
            G.DrawImage(bm, new Point(0, 0));
            */
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
                    if (group[n].X < XL && group[n + 1].X < XL)//都在外，一个也不保留
                        ;
                    //q[number1++] = group[n];
                    else if (group[n].X >= XL && group[n + 1].X >= XL)//都在内，保留后点。因为如果上一个是外入内或者全内，则前点已被保留
                    {
                        q[number1] = group[n + 1];
                        number1++;
                    }
                    else if (group[n].X >= XL && group[n + 1].X < XL)//内入外，保留边界点，因为前点已经被保留了
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XL - group[n].X);
                        //q[number1] = group[n];//.X = XL;
                        //number1++;
                        q[number1].Y = (int)y;
                        q[number1].X = XL;
                        number1++;
                    }
                    else if (group[n].X < XL && group[n + 1].X >= XL)//外入内，输出与边框交点和后点
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XL - group[n].X);
                        q[number1].X = XL;
                        q[number1].Y = (int)y;
                        number1++;
                        q[number1] = group[n + 1];
                        number1++;
                    }
                }
                for (i = 0; i < number1; i++)//裁剪结果放入group数组
                    group[i] = q[i];
                group[number1] = q[0];
                PressNum = number1;
            }
            else if (linecode == 1)
            {
                for (n = 0; n < PressNum; n++)
                {
                    if (group[n].Y >= YU && group[n + 1].Y >= YU)//外，丢弃
                        //q[number1++] = group[n];
                        ;
                    else if (group[n].Y < YU && group[n + 1].Y < YU)//都在内，输出后点，如果上一次是外入内或者都内，那么前点已被保留
                    {
                        q[number1] = group[n + 1];
                        number1++;
                    }
                    else if (group[n].Y < YU && group[n + 1].Y >= YU)//从内到外，保留边界点，如果上一次是外入内或者都内，那么前点已被保留
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YU - group[n].Y);
                        //q[number1] = group[n];
                        //number1++;
                        q[number1].X = (int)x;
                        q[number1].Y = YU;
                        number1++;
                    }
                    else if (group[n].Y >= YU && group[n + 1].Y < YU)//外入内，保留边界点和后点
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YU - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1].Y = YU;
                        number1++;
                        q[number1] = group[n + 1];
                        number1++;
                    }

                }
                for (i = 0; i < number1; i++)//裁剪结果放入group数组
                    group[i] = q[i];
                group[number1] = q[0];
                PressNum = number1;
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
                    else if (group[n].X < XR && group[n + 1].X >= XR)//内入外，保留边界点
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XR - group[n].X);
                        q[number1].X = XR;
                        q[number1].Y = (int)y;
                        number1++;
                    }
                    else if (group[n].X >= XR && group[n + 1].X < XR)//外入内，输出与边框交点和后点
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XR - group[n].X);
                        q[number1].X = XR;
                        q[number1].Y = (int)y;
                        number1++;
                        q[number1] = group[n + 1];
                        number1++;
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
                    else if (group[n].Y >= YD && group[n + 1].Y < YD)//内入外，保留边界点
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YD - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1].Y = YD;
                        number1++;
                    }
                    else if (group[n].Y < YD && group[n + 1].Y >= YD)//外入内，输出与边框交点和后点
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YD - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1].Y = YD;
                        number1++;
                        q[number1] = group[n + 1];
                        number1++;
                    }

                }
                for (i = 0; i < number1; i++)//裁剪结果放入group数组
                    group[i] = q[i];
                group[number1] = q[0];
                PressNum = number1;
            }
        }
        private void WindowCut1()
        {
            group[PressNum] = group[0];
            Graphics G = CreateGraphics();
            Graphics tmp = Graphics.FromImage(bm);
            for (int i = 0; i < 4; i++)
            {
                EdgeClipping(i);
              
                
                    Pen pen;
                if (i == 0) pen = new Pen(Color.Blue, 3);
                else if (i == 1) pen = new Pen(Color.Orange, 3);
                else if (i == 2) pen = new Pen(Color.Green, 3);
                else pen = new Pen(Color.Yellow, 3);
                
                for (int j = 0; j < PressNum; j++)
                {
                    tmp.DrawLine(pen, group[j], group[j + 1]);
                    G.DrawImage(bm,new Point(0,0));
                }
                System.Threading.Thread.Sleep(1000);


            }
            
            G.DrawImage(bm, new Point(0, 0));
            /*
            Graphics g = CreateGraphics();

            Pen DrawPen = new Pen(Color.Yellow, 3);
            for(int i = 0;i < PressNum; i++)//绘制裁剪多边形
            {
                g.DrawLine(DrawPen, group[i], group[i + 1]);
            }
            */

        }
        private void WindowCut_Click(object sender, EventArgs e)
        {
            //initializeLine(1111);
            initializeLine(24);
            /*
            Graphics g = Graphics.FromImage(bm);
            //g.Clear(BackColor1);

            XL = 100;
            XR = 400;
            YD = 100;
            YU = 400;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            //g.DrawPolygon(Pens.Blue, pointsgroup);
            Graphics G = CreateGraphics();
            //G.DrawImage(bm, new Point(0, 0));
            */
        }

        private void PingMove_Click(object sender, EventArgs e)
        {
            initializeLine(11);
            Graphics g = CreateGraphics();
            pointsgroup[0] = new Point(100, 100);
            pointsgroup[1] = new Point(200, 100);
            pointsgroup[2] = new Point(200, 200);
            pointsgroup[3] = new Point(100, 200);
            g.DrawPolygon(Pens.Red, pointsgroup);
        }

        private void 图形旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initializeLine(12);
            Graphics g = CreateGraphics();
            pointsgroup[0] = new Point(100, 100);
            pointsgroup[1] = new Point(200, 100);
            pointsgroup[2] = new Point(200, 200);
            pointsgroup[3] = new Point(100, 200);
            g.DrawPolygon(Pens.Red, pointsgroup);
        }

        private void Bezier_4(int mode, Point p1,Point p2,Point p3,Point p4)
        {
            int i, n;
            Graphics g = Graphics.FromImage(bm);
            Point p = new Point();
            Point oldp = new Point();
            double t1, t2, t3, t4,dt;
            Pen DrawPen = new Pen(Color.Red, 1);
            n = 100;
            if(mode == 2)
            {
                DrawPen = new Pen(Color.Red, 1);
            }
            else if(mode == 1)
            {
                DrawPen = new Pen(Color.Black, 1);
            }
            else if(mode == 0)
            {
                DrawPen = new Pen(Color.White, 1);
            }
            oldp = p1;
            dt = 1.0 / n;//参数t的间隔，分为100段，用100段直线表示一条曲线
            for(i = 1;i <= n; i++)//Bezier参数方程计算
            {
                t1 = (1.0 - i * dt) * (1.0 - i * dt) * (1.0 - i * dt);
                t2 = i * dt * (1.0 - i * dt) * (1.0 - i * dt);
                t3 = i * i * dt * dt * (1.0 - i * dt);
                t4 = i * i * i * dt * dt * dt;
                p.X = (int)(t1 * p1.X + 3 * t2 * p2.X + 3 * t3 * p3.X + t4 * p4.X);
                p.Y = (int)(t1 * p1.Y + 3 * t2 * p2.Y + 3 * t3 * p3.Y + t4 * p4.Y);
                g.DrawLine(DrawPen, oldp, p);
                oldp = p;
            }
            Graphics G = CreateGraphics();//创建画布
            G.DrawImage(bm, new Point(0, 0));
        }
        private void Bezier_41(int mode, Point p1, Point p2, Point p3,Point p4)
        {
            Graphics g = CreateGraphics();
            Point p = new Point();
            Point oldp = new Point();
            double t, dt;
            Point[] g1 = new Point[4];
            g1[0] = p1;
            g1[1] = p2;
            g1[2] = p3;
            g1[3] = p4;
            Pen DrawPen = new Pen(Color.Red, 1);
            int n = 100;
            if (mode == 2)
            {
                DrawPen = new Pen(Color.Red, 1);
            }
            else if (mode == 1)
            {
                DrawPen = new Pen(Color.Black, 1);
            }
            else if (mode == 0)
            {
                DrawPen = new Pen(Color.White, 1);
            }
            oldp = p1;
            dt = 1.0 / n;
            for(int i = 1; i <= n; i++)
            {
                t = i * dt;
                for(int k = 3;k > 0; k--)
                {
                    for(int j = 0; j < k; j++)
                    {
                        g1[j].X = (int)((1.0 - t) * g1[j].X + t * g1[j + 1].X);
                        g1[j].Y = (int)((1.0 - t) * g1[j].Y + t * g1[j + 1].Y);
                    }
                }
                p = g1[0];
                g.DrawLine(DrawPen, oldp, p);
                oldp = p;
            }

        }
        private void Bezier1(int mode)
        {
            Point[] p = new Point[300];//储存完整的Bezier曲线控制点
            int i = 0, j = 0;
            p[i] = group[j];
            i++;
            j++;
            p[i] = group[j];
            j++;
            i++;
            while(j <= PointNum - 2)
            {
                p[i] = group[j];
                j++;
                i++;
                p[i].X = (group[j].X + group[j - 1].X) / 2;
                p[i].Y = (group[j].Y + group[j - 1].Y) / 2;
                i++;
                p[i] = group[j];
                i++;
                j++;

            };
            for(j = 0; j < i - 3; j += 3)
            {
                Bezier_4(mode, p[j], p[j + 1], p[j + 2], p[j + 3]);
            }
        }
        private void BezierCurve_Click(object sender, EventArgs e)
        {
            //initializeLine(7);
            MenuID = 7;
            PointNum = 0;
            PressNum = 0;
            Graphics g = CreateGraphics();
            g.Clear(BackColor1);
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();
            Pen DrawPen = new Pen(Color.White, 1);
            if (MenuID == 107 || MenuID == 108)
            {
                for (int i = 0; i < PointNum; i++)
                {
                    g.DrawLine(DrawPen, group[i].X - 5, group[i].Y, group[i].X + 5, group[i].Y);
                    g.DrawLine(DrawPen, group[i].X, group[i].Y - 5, group[i].X, group[i].Y + 5);
                }
                if (MenuID == 107)
                {
                    Bezier1(2);//绘制Bezier曲线
                    //initializeLine(7);//将操作改回Bezier
                    MenuID = 7;
                }
                else
                {
                    BSample1(2);
                    MenuID = 8;
                }
                PressNum = 0;
                PointNum = 0;
            }
        }

        private void BSample_4(int mode, Point p0, Point p1,Point p2,Point p3)
        {
            Graphics g = Graphics.FromImage(bm);

            Point p = new Point();
            Point oldp = new Point();
            Pen DrawPen = new Pen(Color.Red, 1);
            int n = 100;
            if(mode == 2)
            {
                DrawPen = new Pen(Color.Red, 1);
            }
            else if(mode == 1)
            {
                DrawPen = new Pen(Color.Black, 1);

            }
            else if(mode == 0)
            {
                DrawPen = new Pen(Color.White, 1);
            }
            oldp = p;
            double dt = 1.0 / n;
            for(double t = 0.0;t <= 1.0; t += dt)
            {
                double t1 = (1.0 - t) * (1.0 - t) * (1.0 - t);
                double t2 = 3.0 * t * t * t - 6.0 * t * t + 4.0;
                double t3 = -3.0 * t * t * t + 3.0 * t * t + 3.0 * t + 1.0;
                double t4 = t * t * t;
                p.X = (int)((t1 * p0.X + t2 * p1.X + t3 * p2.X + t4 * p3.X) / 6.0);
                p.Y = (int)((t1 * p0.Y + t2 * p1.Y + t3 * p2.Y + t4 * p3.Y) / 6.0);
                if (t > 0)
                    g.DrawLine(DrawPen, oldp, p);
                oldp = p;
            }
            Graphics G = CreateGraphics();//创建画布
            G.DrawImage(bm, new Point(0, 0));
        }
        private void BSample1(int mode)
        {
            for(int i = 0; i < PointNum - 3; i++)
            {
                BSample_4(mode, group[i], group[i + 1], group[i + 2], group[i + 3]);
            }
        }
        
        private void BSampleCurve_Click(object sender, EventArgs e)
        {
            initializeLine(8);
            Graphics g = CreateGraphics();
            g.Clear(BackColor1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "d:\\";
            saveFileDialog1.Filter = "Jpg 图片|*.jpg|Bmp 图片|*.bmp|Gif 图片|*.gif|Png 图片|*.png|Wmf  图片|*.wmf";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            DialogResult dr = saveFileDialog1.ShowDialog();
            saveFileDialog1.FileName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "-";
            if (dr == DialogResult.OK && saveFileDialog1.FileName.Length > 0)
            {
                bm.Save(saveFileDialog1.FileName);
                MessageBox.Show("存储文件成功！", "保存文件");
            }
        }
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bm = new Bitmap(900, 600);
            Graphics G = CreateGraphics();
            G.Clear(BackColor1);
            G.DrawImage(bm, new Point(0, 0));
        }

        private void BresenhamCircle_Click(object sender, EventArgs e)
        {
            initializeLine(5);//圆与直线初始化通用
            
            Graphics g = CreateGraphics();//生成画布
            g.Clear(BackColor1);//画布上色
        }

        private void ScanLineFill_Click(object sender, EventArgs e)
        {
            MenuID = 31;//扫描线填充算法
            initializeLine(31);
            
            Graphics g = CreateGraphics();//创建画布
            g.Clear(BackColor1);//背景填充
        }

        private void CohenCut_Click(object sender, EventArgs e)
        {
            initializeLine(21);
            /*
            Graphics g = Graphics.FromImage(bm);
            XL = 100;
            XR = 400;
            YD = 100;
            YU = 400;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);
            Graphics G = CreateGraphics();
            G.DrawImage(bm, new Point(0, 0));
            */
        }
    }
}
