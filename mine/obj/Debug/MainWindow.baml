   M S B A M L   `   `   `  ����?  ;mine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:minemine  5   	       mine.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:mine     5   	     � Icon$  bomb.png��5   	    � 
ResizeMode$ CanMinimize=�6    � Title$) #扫雷游戏 developed by 熊跃辉��5	   	   $	��600��6(   $	��500��65    � 	Resources 5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ BureauBlack.xaml?�6%   %    5      .��5      � .+�5   
   �� -   5         ��Name$ topArea��5   .    �� Orientation$ 
Horizontal=�6   $��	0,0,0,460q�6?   
����  �.+�5      �� -   5         ��Name$ 	btnSimple��5      $��20,30,20,40q�6(   $��简易��6=   $��56��6b   �� 5      -   5        $ 	btnMiddle��5      $��中等��6*   $��56��6O   $��
0,30,20,40q�6Z   �� 5      -   5        $ btnDiff��5      $��困难��6(   $��56��6K   $��
0,30,20,40q�6V   �� 5      -   5        $ btnBig��5      $��开启作弊��6'   $��80��6K   $��
0,30,20,40q�6V   5      � 5   
   -   5        	 �Name$	 gameArea��5      $��	0,100,0,0q�6!   66   5      5      