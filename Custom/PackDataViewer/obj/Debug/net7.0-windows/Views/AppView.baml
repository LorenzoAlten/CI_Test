   M S B A M L   `   `   `  ����I  EPackDataViewer, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PackDataViewer.ViewsPackDataViewer.Views  5   	   I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5   	   G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5   	   G4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5   	   "    PackDataViewer.Views.AppView   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   4local"clr-namespace:PackDataViewer.Views     5   	   IWPFUtils4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils     5   	   Bfbdll0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	   ^	 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 5
   	   ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 	 5   	   -   5           MaterialDesignBody#���   5   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#��[ 5   	    � 	Resources 5      ��%    5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5!   
   �� -   5        $��
ActiveItem��5!      $��0 5 0 0q�6+   $��0��6<   '  !MaterialDesignThemes.Wpf.Snackbar  5#   
   -   5          Name$ MainSnackbar��5#   O   $��1��6e   $��Stretch=�5$   "   $��Center=�6@      MessageQueue 5#   "   4  .MaterialDesignThemes.Wpf.MessageQueueExtension  5$   @   �� 5&   
   $��2��6   $	��0 0q�6!   � 5(   
   $��3��6    � ColumnDefinitions 5)      �� 5*      $
��Auto �6#   �� 5+      $��* �6#   �� 5,      $
��Auto �6#   .+�5/      �� $��2��50      $
��10 5q�6(   $��Center=�66   ��5/      ��  �� Path$	 Now� �� StringFormat$! {0:dd/MM/yyyy} {0:HH:mm:ss}��50   6   51      � 53   
   -   5        	 �Name$	 LoadingIsVisible��53      $��10��6)   $��10��6;   
�������
 J� Opacity$	
 0.7Q�54   &   .+�55      �  % !MaterialDesignCircularProgressBar#��[ 6    � Value$ 0Q�56      $��40��6$   $��40��6/    � IsIndeterminate$
 True��57      58      59      5:      