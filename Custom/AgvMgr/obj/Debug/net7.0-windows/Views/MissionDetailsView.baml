   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5	      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5
      
 AgvMgr%   AgvMgr.Views.MissionDetailsView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R	 NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml   	  5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006   	  5      @d2http://schemas.microsoft.com/expression/blend/2008   	  5      b
 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 
 5      ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5      0sys&clr-namespace:System;assembly=mscorlib  5      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll  5	      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5       �� 
TargetType  MaterialDesignTextBox �� BasedOn �� Property �� Value  MaterialDesignComboBox  MaterialDesignCheckBox(  "System.Windows.Controls.DatePicker  MaterialDesignDatePicker   MaterialDesignBody2TextBlock' 
 !MaterialDesignThemes.Wpf.PackIcon  MaterialDesignCardGroupBox 	 CommandButtonStyle 
 MaterialDesignRaisedButton  MovieDataProvider S� 
MethodName S� 
ObjectType   mSwAgilogDll.EMissionState	 S� MethodParameters
 M� Type��%�  '��       7  '�� O     7  '�� �     7  '  k    7  '�� 	    7  '      '�� �    7  &		 �    7 
 &	 �    �� 5   
   " ��6   8   .�5      ��  � ��$
 12 2q�6'   5      �� 5   
   " ��6   8   .�5      ��  � ��$ 12 2 0 2q�6'   5      �� 5   
   " ��6   8   .�5      ��  � ��$ Center=�69   �� 5       � ��$ Center=�62   �� 5       � ��$
 Left=�64   �� 5       � ��$ 12 2 0 2q�6'   5       �� 5!   
   "  6   8   .�5"      ��  � ��$ 12 2 0 2q�6'   �� 5#       � ��$	 120��6)   �� 5$       � ��$
 Left=�64   5%      �� 5&   
   " ��6   8   .�5'      ��  � ��$ Center=�62   �� 5(       � ��$	 0 2q�6'   5)      �� 5*   
   "  6   .�5-      ��  � ��$ Bottom=�62   5.      �� 5/   
   " ��6   8   .�50      ��  � ��$ 10q�6'   �� 51       � ��$ 8q�6(   �� 52       � ��$	 350��6&   �� 53       � ��$ Center=�64   54      �� 55   
   " ��6+   8   .�56      ��  � ��$ 4q�6'   57      S� 59   
   $ 	GetValues��67   "  6N   	 5:      M� 5;      "
  6   5=      5>      .��5?      ]� .+�5@   
   �� #��[	 6     Caliburn.Micro.Message   Attach$&  [Event Click] = [Action Salva()]��5A   K   $��Bottom=�5B      $��Center=�61   ��5A      '  !mSwDllWPFUtils.TranslateExtension  *Save+   	Uppercase$
 True��5B   1   �� 5C   
   . 
 (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�5D      ��5C      �� *
AgvRequest+��5E      �� 5F      .R�5G      ]� .+�5H      �� $��8 0 0 0q�6$   $��Center=�65    $  MaterialDesignSubtitle2TextBlock#��[ 6P   ��5I   $     *	Mission+$
 True��( 
 "MaterialDesignThemes.Wpf.ColorZone  5J      $��2q�63   $
��12 0q�6?   $	��150��6M   $��Right=�5K   3   
�������
���   �+	 
 %MaterialDesignThemes.Wpf.ShadowAssist 	  ShadowDepth$ Depth1=�5M   /   .��5N      �� ��6&   �� *AGV_Mission+ �� TargetNullValue$  ��5O      5P      5Q      .��5T      � $	��8 4q�6    � ColumnDefinitions 5U      �� 5V      $
��Auto �6'   �� 5W      $��* �6'    � RowDefinitions 5Y      �� 5Z      $
��Auto �6$   $��35��62   �� 5[      $
��Auto �6$   $��35��62   .+�5^      �� $��0��6   $��0��6)   ��5_         *Mission State+   Suffix$  ���� 5`      $��0��6   $��1��6(    �� 
IsEditable$ False��5c       �� 
IsReadOnly$
 True��5d       �� SelectedValue 5a      �� *AgvRequest.MissionState+��5b      �� *AgvMissionStateList+5d      �� 5f      $��1��6   $��0��6)   ��5g        *
Agv Load+�� 5h      $��1��6   $��1��6(    d� 	IsChecked 5i      �� *AgvRequest.AGV_Loaded+ � LayoutTransform 5j      �� 5k       �� ScaleX$	 1.2Q�6)    �� ScaleY$	 1.2Q�66   5m      5n      5o      5q      5r      