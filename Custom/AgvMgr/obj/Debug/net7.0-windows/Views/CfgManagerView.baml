   M S B A M L   `   `   `  ����A  =AgvMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:AgvMgr.ViewsAgvMgr.Views  5   	   5clr-namespace:AgvMgr.ViewModelsAgvMgr.ViewModels  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5   	   G4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5   	   G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5   	   !    AgvMgr.Views.CfgManagerView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   ,localclr-namespace:AgvMgr.Views     5   	   3localvmclr-namespace:AgvMgr.ViewModels     5      IWPFUtils4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils     5   	   b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5	   	   ^
 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 
 5
   	   ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 
 5   	   =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 
 5   	      MaterialDesignBody#���   5   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#��[ 5   	    � 	Resources 5         BooleanToVisibilityConverter �� 
TargetType �� BasedOn �� Property �� Value' 	 !MaterialDesignThemes.Wpf.PackIcon��%�  &	       '��      7��'  �     �� 5   
   6'   �� 5   
   " ��6   8   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 5q�6'   �� 5       � ��$ 16(�6)   5      �� 5   
   "  6   .�5      ��  � ��$ 5q�6'   �� 5        � ��$ 24��6'   �� 5!       � ��$ 24��6&   �� 5"       � ��$ Center=�64   �� 5#       � ��$ Center=�62   5$      5%      .��5'      �  � RowDefinitions 5(   
   �� 5)      $��* �6   .+�5+   
   �� $��0q�6   $��0q�6#   $
��Auto=�6.   $��0��6Q   .��5,      ]� .+�5-      �� -   5        $��AddSew��5-       " MaterialDesignFlatDarkBgButton#��[ 6'   $	��Top=�6_   $��Right=�5.      $
��12 4q�65    � ToolTip 5/      '  !mSwDllWPFUtils.TranslateExtension  *	Add Agv+.��50      �� 	 �� Orientation$	 
Horizontal=�6!   .+�51      �� $��Center=�52   $   ��51   $     *	Add Agv+
   	Uppercase$

 True��52   $     53         Kind$
 Plus=�62   54      55      �� 56      $��Stretch=�57      $��Stretch=�63   ��56      �� *	AgvsModel+��58      �� 59      .R�5:      �� $	 Vertical=�6%   5;      ��5=      �� 5>      .R�5?      �� $
��12 4q�5@   )    
 Caliburn.Micro.View   Model 5?   )   �� 5@   )   5A      5C      5D      5E      � 5G   
    % !MaterialDesignCircularProgressBar#��[ 6    � Value$ 0Q�5H      $��50��6    $��50��6+   
������� J� Opacity$	 0.7Q�6J   $��Center=�5I      $��Center=�61    � IsIndeterminate$
 True��5J      y�5K      �� *	IsLoading+ �� 	Converter# [ 5L      5N      