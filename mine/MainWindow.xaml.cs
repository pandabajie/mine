﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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


        public MainWindow()
        {
            InitializeComponent();
            initGrid();
        }


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
                    MessageBox.Show("游戏结束");
                    this.start();
                    return;
                }
                else
                {
                    MessageBox.Show("这是地雷哦");
                    this.add_red_flag(x,y);
                    return;
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

        private void btn_right_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int y = Grid.GetRow(btn);
            int x = Grid.GetColumn(btn);
            Console.WriteLine(x + "," + y);
            this.add_red_flag(x, y);
        }

        private void img_right_click(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            int y = Grid.GetRow(img);
            int x = Grid.GetColumn(img);
            this.gameArea.Children.Remove(img);
            Console.WriteLine(x + "," + y);
        }

        private void initGrid()
        {
            BombArray = this.CreateRank2Array(size);
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

                    if (BombArray[i, j] == 1)
                    {
                        Image img = new Image();
                        img.Width = 500 / this.size;
                        img.Height = 500 / this.size;
                        img.Source = new BitmapImage(new Uri("pack://application:,,,/bomb.png"));
                        Grid.SetColumn(img, i);
                        Grid.SetRow(img, j);
                        this.gameArea.Children.Add(img);
                    }
                    else
                    {
                        //计算周围藏雷的数量
                        Label lab = new Label();
                        int cb = this.countBomb(i, j);
                        if (cb > 0)
                        {
                            lab.Content = cb;
                            lab.FontSize = 16;
                            lab.HorizontalAlignment = HorizontalAlignment.Center;
                            lab.VerticalAlignment = VerticalAlignment.Center;
                            Grid.SetColumn(lab, i);
                            Grid.SetRow(lab, j);
                            this.gameArea.Children.Add(lab);
                        }

                    }
                    Button btn = new Button();
                    Grid.SetColumn(btn, i);
                    Grid.SetRow(btn, j);
                    btn.Click += new RoutedEventHandler(btn_click);
                    btn.MouseRightButtonDown += new MouseButtonEventHandler(btn_right_click);
                    this.gameArea.Children.Add(btn);
                }
            }
            // labelInfo.Content = "地雷数：" + (this.size + this.BombSeed); ;
        }

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

        private void btnSimple_Click(object sender, RoutedEventArgs e)
        {
            this.BombSeed = this.BombSimpleSeed;
            this.size = this.SizeSimpleSeed;
            this.start();
        }

        private void start()
        {
            BombArray = new int[,] { };
            this.gameArea.RowDefinitions.Clear();
            this.gameArea.ColumnDefinitions.Clear();
            this.gameArea.Children.Clear();
            initGrid();
        }

        private void btnMiddle_Click(object sender, RoutedEventArgs e)
        {
            this.BombSeed = this.BombMiddleSeed;
            this.size = this.SizeMiddleSeed;
            this.start();
        }

        private void btnDiff_Click(object sender, RoutedEventArgs e)
        {
            this.BombSeed = this.BombDiffSeed;
            this.size = this.SizeDiffSeed;
            this.start();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            //this.gameArea.FindResource();
        }

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