   M S B A M L   `   `   `  ����A  =WhsMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:WhsMgr.ViewsWhsMgr.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5          WhsMgr.Views.AppView   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      ,localclr-namespace:WhsMgr.Views     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5      ^	 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 5	      -   5           MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5      $��Roboto)�5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   ]� .+�5      �� $	��Top=�67   ��6   �� *	Screens+ �� SelectedItem 5       �� *
ActiveItem+��5!      �� 5"      .R�5#      ��  �� Orientation$ 
Horizontal=�6%   5$      ��5&      �� 5'      .R�5(      �� $
��16 8q�5)   $    $  MaterialDesignSubtitle1TextBlock#��[ 62    	 Caliburn.Micro.Message   Attach$@ :[Event MouseLeftButtonDown] = [ActivateItem($dataContext)]��5*   $   ��5(   $   �� *DisplayName+5*   $   5+      5-      �� 5/      -   5         ��Name$ 
ActiveItem��5/      50      �� 52   
   $��1��6   $��Bottom=�6!   $��	10 5 10 0q�6<   � 54   
   $��2��6    � ColumnDefinitions 55      �� 56      $��* �6#   �� 57      $
��Auto �6#   .+�5:      �� $��2��5;       $
��10 5q�60   ��5:      ��  �� Path$	 Now�	 �� StringFormat$	 {0:d} {0:T}��
 �� ConverterCulture$
 IT��5;   0   5<      5=      5>      