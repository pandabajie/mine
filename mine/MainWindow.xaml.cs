/**
* 命名空间: mine
*
* 功 能： 扫雷游戏主页
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V1.0 2017-01-10 熊跃辉 初版
*
* Copyright (c) 2017 熊跃辉. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本人机密信息，未经本人书面同意禁止向第三方披露．　│
*│　版权所有：熊跃辉 　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mine
{
    public partial class MainWindow : Window
    {
        private int size = 10;
        private int[,] BombArray;
        private int BombSeed = 5;
        private int BombSimpleSeed = 5;
        private int BombMiddleSeed = 50;
        private int BombDiffSeed = 100;
        private int SizeSimpleSeed = 5;
        private int SizeMiddleSeed = 10;
        private int SizeDiffSeed = 15;
        //是否开启作弊模式
        private bool isBig = false;

        /// <summary>
        /// 主窗口
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            initGrid();
        }

        /// <summary>
        /// 单击方块事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int y = Grid.GetRow(btn);
            int x = Grid.GetColumn(btn);
            
            Console.WriteLine(x + "," + y);
            if (BombArray[x, y] == 1)
            {
                if (isBig == false)
                {
                    this.gameArea.Children.Remove(btn);
                    Image img = new Image();
                    img.Width = 500 / this.size;
                    img.Height = 500 / this.size;
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/bomb.png"));
                    Grid.SetColumn(img, x);
                    Grid.SetRow(img, y);
                    this.gameArea.Children.Add(img);
                    MessageBox.Show("游戏结束");
                    this.start();
                    return;
                }
                else
                {
                    MessageBox.Show("这是地雷哦");
                    this.add_red_flag(x, y);
                    return;
                }
            }
            else
            {
                //计算周围藏雷的数量
                int cb = this.countBomb(x, y);
                if (cb > 0)
                {
                    Label lab = new Label();
                    lab.Content = cb;
                    lab.FontSize = 32;
                    lab.HorizontalAlignment = HorizontalAlignment.Center;
                    lab.VerticalAlignment = VerticalAlignment.Center;
                    Grid.SetColumn(lab, x);
                    Grid.SetRow(lab, y);
                    lab.MouseLeftButtonDown += new MouseButtonEventHandler(lab_double_click);
                    this.gameArea.Children.Add(lab);
                }
            }
            this.gameArea.Children.Remove(btn);
            if (BombArray[x, y] == 0)
            {
                //标记已翻开过
                BombArray[x, y] = 2;
            }
            //检查是否胜利
            bool isRight = true;
            for (int i = 0; i < this.gameArea.ColumnDefinitions.Count; i++)
            {
                for (int j = 0; j < this.gameArea.RowDefinitions.Count; j++)
                {
                    if (BombArray[i, j] == 0) {
                        isRight = false;
                    } ;
                }
            }
            if (isRight == true)
            {
                MessageBox.Show("恭喜您获胜");
            }
        }

        /// <summary>
        /// 添加红旗
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void add_red_flag(int x,int y)
        {
            Image img = new Image();
            img.Width = 500 / this.size;
            img.Height = 500 / this.size;
            img.Source = new BitmapImage(new Uri("pack://application:,,,/timg.png"));
            Grid.SetColumn(img, x);
            Grid.SetRow(img, y);
            img.MouseRightButtonDown += new MouseButtonEventHandler(img_right_click);
            this.gameArea.Children.Add(img);
        }

        /// <summary>
        /// 鼠标右击事件，主要是添加红旗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_right_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int y = Grid.GetRow(btn);
            int x = Grid.GetColumn(btn);
            Console.WriteLine(x + "," + y);
            this.add_red_flag(x, y);
        }

        /// <summary>
        /// 点击红旗，即取消地雷选定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void img_right_click(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            int y = Grid.GetRow(img);
            int x = Grid.GetColumn(img);
            this.gameArea.Children.Remove(img);
            Console.WriteLine(x + "," + y);
        }

        /// <summary>
        /// 初始化布局
        /// </summary>
        private void initGrid()
        {
            this.gameArea.Background = Brushes.White;
            //布雷随机位置
            BombArray = this.CreateRank2Array(size);
            //开始布雷
            for (int i = 0; i < this.size; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                this.gameArea.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < this.size; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                this.gameArea.ColumnDefinitions.Add(columnDefinition);
            }
            for (int i = 0; i < this.gameArea.ColumnDefinitions.Count; i++)
            {
                for (int j = 0; j < this.gameArea.RowDefinitions.Count; j++)
                {

                    
                    Button btn = new Button();
                    Grid.SetColumn(btn, i);
                    Grid.SetRow(btn, j);
                    btn.Click += new RoutedEventHandler(btn_click);
                    btn.MouseRightButtonDown += new MouseButtonEventHandler(btn_right_click);
                    this.gameArea.Children.Add(btn);
                }
            }
        }


        /// <summary>
        /// 点击数字出现双击事件
        /// </summary>
        int iC = 0;
        private void lab_double_click(object sender, RoutedEventArgs e)
        {
            Label lab = sender as Label;
            int y = Grid.GetRow(lab);
            int x = Grid.GetColumn(lab);

            iC += 1;
            System.Timers.Timer t = new System.Timers.Timer(600);
            t.Interval = 600;

            t.Elapsed += (s, ee) => { t.Enabled = false; iC = 0; };
            t.Enabled = true;
            if (iC % 2 == 0)
            {
                t.Enabled = false;
                iC = 0;
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.gameArea); i++)
                {
                    var child = VisualTreeHelper.GetChild(this.gameArea, i);
                    if (child is Button)
                    {
                        Button btn = child as Button;
                        int a = Grid.GetColumn(btn);
                        int b = Grid.GetRow(btn);
                        if (a == x - 1 && b == y - 1)
                        {
                            this.btnAnimate(btn);
                        }
                        if (a == x - 1 && b == y)
                        {
                            this.btnAnimate(btn);
                        }
                        if (a == x - 1 && b == y+1)
                        {
                            this.btnAnimate(btn);
                        }
                        if (a == x  && b == y - 1)
                        {
                            this.btnAnimate(btn);
                        }
                        if (a == x && b == y + 1)
                        {
                            this.btnAnimate(btn);
                        }
                        if (a == x + 1 && b == y - 1)
                        {
                            this.btnAnimate(btn);
                        }
                        if (a == x + 1 && b == y)
                        {
                            this.btnAnimate(btn);
                        }
                        if (a == x + 1 && b == y + 1)
                        {
                            this.btnAnimate(btn);
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// 按钮动画
        /// </summary>
        /// <param name="btn"></param>
        public void btnAnimate(Button btn)
        {
            int a = Grid.GetColumn(btn);
            int b = Grid.GetRow(btn);
            Console.WriteLine(a+"---"+b);
            //动画定义
            Storyboard myStoryboard = new Storyboard();
            //透明度由0-1的变化动画 
            DoubleAnimation OpacityDoubleAnimation = new DoubleAnimation();
            OpacityDoubleAnimation.From = 0.5;
            OpacityDoubleAnimation.To = 1;
            OpacityDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1.4));
            //设置动画对象名称
            Storyboard.SetTarget(OpacityDoubleAnimation, btn);
            //设置动画属性
            Storyboard.SetTargetProperty(OpacityDoubleAnimation, new PropertyPath(Grid.OpacityProperty));
            myStoryboard.Children.Add(OpacityDoubleAnimation);
            //开始动画
            myStoryboard.Begin(this.gameArea);
        }



        /// <summary>
        /// 获取地雷位置
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        private int[,] CreateRank2Array(int Length)
        {
            Random rand = new Random();
            int[,] returnArray = new int[Length, Length];
            int int_From = 0;
            int int_intTo = Length - 1;
            for (int i = 0; i < Length + this.BombSeed; i++)
            {
                int x = rand.Next(int_From, int_intTo);
                int y = rand.Next(int_From, int_intTo);
                Console.WriteLine(x + "," + y);
                returnArray[x, y] = 1;
            }
            return returnArray;
        }

        /// <summary>
        /// 根据指定位置处理地雷数量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int countBomb(int x, int y)
        {
            int tmpCount = 0;
            if (x - 1 >= 0 && y - 1 >= 0 && BombArray[x - 1, y - 1] == 1)
            {
                tmpCount++;
            }
            if (x - 1 >= 0 && BombArray[x - 1, y] == 1)
            {
                tmpCount++;
            }
            if (x - 1 >= 0 && y + 1 < this.size && BombArray[x - 1, y + 1] == 1)
            {
                tmpCount++;
            }
            if (y - 1 >= 0 && BombArray[x, y - 1] == 1)
            {
                tmpCount++;
            }
            if (y + 1 < this.size && BombArray[x, y + 1] == 1)
            {
                tmpCount++;
            }
            if (y - 1 >= 0 && x + 1 < this.size && BombArray[x + 1, y - 1] == 1)
            {
                tmpCount++;
            }
            if (x + 1 < this.size && BombArray[x + 1, y] == 1)
            {
                tmpCount++;
            }
            if (x + 1 < this.size && y + 1 < this.size && BombArray[x + 1, y + 1] == 1)
            {
                tmpCount++;
            }
            Console.WriteLine(tmpCount);
            return tmpCount;
        }

        /// <summary>
        /// 简单模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSimple_Click(object sender, RoutedEventArgs e)
        {
            this.BombSeed = this.BombSimpleSeed;
            this.size = this.SizeSimpleSeed;
            this.start();
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        private void start()
        {
            BombArray = new int[,] { };
            this.gameArea.RowDefinitions.Clear();
            this.gameArea.ColumnDefinitions.Clear();
            this.gameArea.Children.Clear();
            initGrid();
        }

        /// <summary>
        /// 中等模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMiddle_Click(object sender, RoutedEventArgs e)
        {
            this.BombSeed = this.BombMiddleSeed;
            this.size = this.SizeMiddleSeed;
            this.start();
        }

        /// <summary>
        /// 困难模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDiff_Click(object sender, RoutedEventArgs e)
        {
            this.BombSeed = this.BombDiffSeed;
            this.size = this.SizeDiffSeed;
            this.start();
        }
  
        /// <summary>
        /// 开启作弊模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBig_Click(object sender, RoutedEventArgs e)
        {
            if (this.isBig == false)
            {
                this.isBig = true;
                this.btnBig.Content = "取消作弊";
            }
            else
            {
                this.isBig = false;
                this.btnBig.Content = "开启作弊";
            }
        }
    }
}
