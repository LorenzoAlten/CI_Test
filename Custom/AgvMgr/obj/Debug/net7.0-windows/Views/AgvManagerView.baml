   M S B A M L   `   `   `  ����A  =AgvMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:AgvMgr.ViewsAgvMgr.Views  5   	   5clr-namespace:AgvMgr.ViewModelsAgvMgr.ViewModels  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5   	   G4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5   	   G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5   	   !    AgvMgr.Views.AgvManagerView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   ,localclr-namespace:AgvMgr.Views     5   	   3localvmclr-namespace:AgvMgr.ViewModels     5      IWPFUtils4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils     5   	   b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5	   	   ^
 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 
 5
   	   ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 
 5   	   =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 
 5   	      MaterialDesignBody#���   5   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#��[ 5   	    � 	Resources 5         BooleanToVisibilityConverter �� 
TargetType �� BasedOn �� Property �� Value' 	 !MaterialDesignThemes.Wpf.PackIcon��%d  &	       '��      7��'  �     �� 5   
   6'   �� 5   
   " ��6   8   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 5q�6'   �� 5       � ��$ 16(�6)   5      �� 5   
   "  6   .�5      ��  � ��$ 5q�6'   �� 5!       � ��$ Center=�64   �� 5"       � ��$ Center=�62   5#      5$      .��5&      �  � RowDefinitions 5'   
   �� 5(      $
��0.6* �6   $	��200��6*   �� 5)      $
��Auto �6   �� 5*      $��* �6   .+�5,   
   �� $��0��6   $��0q�6    $��0q�6+   .��5-      �� -   5        $��MissionGrid��5-      5.      �� 5/   
   $��1��6   $��Stretch=�6$   $��Stretch=�6@   ��50      �� 51       �� 
TargetType" ��6"   .R�52        	   Kind$	 DragHorizontal=�6.   53      55      �� 56   
   $��0q�6   $
��12 4q�6#   $
��Auto=�61   $��2��6T   .��57      ]� .+�58      �� $��Stretch=�59      $��Stretch=�6;   ��58      �� *	AgvsModel+��5:      �� 5;      .R�5<      �� 
 �� Orientation$
 Vertical=�6%   5=      ��5?      �� 5@      .R�5A      �� $	��4 2q�6D    
 Caliburn.Micro.View   Model 6)   �� 6D   5B      5D      5E      5F      5G      5I      