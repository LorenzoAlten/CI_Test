   M S B A M L   `   `   `  ����G  CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullI4clr-namespace:mSwAgilogDll.SEW;assembly=mSwAgilogDllmSwAgilogDll.SEW  5      A0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll  5	      A =AgvMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AgvMgr.ViewModelsAgvMgr.ViewModels 5
      5clr-namespace:AgvMgr.ConvertersAgvMgr.Converters 5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5      +clr-namespace:AgvMgr.ViewsAgvMgr.Views 5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      '   !AgvMgr.Views.ConfigurationSEWView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R	 NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml   	  5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006   	  5      @d2http://schemas.microsoft.com/expression/blend/2008   	  5      b
 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 
 5      ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5      D	agilogsew4clr-namespace:mSwAgilogDll.SEW;assembly=mSwAgilogDll  5      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll  5	      /	viewModelclr-namespace:AgvMgr.ViewModels  5
      3agvconvertersclr-namespace:AgvMgr.Converters  5      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils  5      %viewclr-namespace:AgvMgr.Views  5      0sys&clr-namespace:System;assembly=mscorlib  5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5      3  -mSwDllWPFUtils.Converters.EnumToBoolConverter  EnumToBoolConverter3  -AgvMgr.Converters.MultiFlagToBooleanConverter  MultiFlagToBooleanConverter8  2AgvMgr.Converters.MultiConverterVisibilityAllEqual $  MultiConverterVisibilityAllEqual �� 
TargetType  MaterialDesignCardGroupBox �� BasedOn �� Property �� Value  MaterialDesignTextBox  MaterialDesignRaisedButton  	 MaterialDesignBody2TextBlock (
 $MaterialDesignUserForegroundCheckBox��%r  &	       &	      &	 &     '�� 9     7  '�� �     7  '�� �     7  '�� #    7 	 '�� �    7 
   5   
   6)     5   
   64     5   
   69   �� 5   
   " ��6   8   .�5      ��  � ��$ Stretch=�62   5      �� 5   
   " ��6   8   .�5       ��  � ��$ 4q�6'   5!      �� 5"   
   " ��6   8   .�5#      ��  � ��$ 5q�6'   5$      �� 5%   
   " ��6   8   .�5&      ��  � ��$ Center=�62   �� 5'       � ��$ 4q�6'   5(      �� 5)   
   " ��6   8   .�5*      ��  � ��$ Center=�62   �� 5+       � ��$
 10 0q�6'   5,      5-      .��5.      �� .��5/   
   �� . 
 (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�51      ��5/      �� *Code_Agv_Machine+{�50      �� *GroupBoxEnabled+��52      �� 53      .R�54      �� $��Center=�6     $  MaterialDesignSubtitle2TextBlock#��[ 55       ��56       �� 57      .��59      ]� .+�5:      A�  A� Columns$ 4��6   $��5��6*   $��9��67   $��Bottom=�5;      $��Right=�5<   "    5=       " MaterialDesignFlatAccentButton��%Z   '��       7  �� 5>      " ��6    8   .�5?      ��  � ��$ 0q�68   5@      5A      .+�5B      �� -   5        $
��Save��5B      {�6)   �� *IsEnabledSave+.��5C      ' 
 !MaterialDesignThemes.Wpf.PackIcon  	   Kind$	 ContentSave=�62   5D      �� 5F      -   5        $
��Edit��5F      {�6)   �� *	IsEnabled+.��5G        $	 Pencil=�62   5H      �� 5J      -   5        $��Delete��5J      {�6+   �� *	IsEnabled+.��5K        $	 Delete=�62   5L      �� 5N      -   5        $��Cancel��5N      {�6+   �� *IsEnabledCancel+.��5O        $	 UndoVariant=�62   5P      5Q      � 5R      
 � RowDefinitions
 5S      �� 5T      $
��Auto �6(   �� 5U      $
��Auto �6(   �� 5V      $
��Auto �6(   �� 5W      $
��Auto �6(   �� 5X      $
��Auto �6(   �� 5Y      $
��Auto �6(   �� 5Z      $��* �6(    � ColumnDefinitions 5\      �� 5]      $
��Auto �6+   �� 5^      $
��Auto �6+   $	��150��68   �� 5_      $
��Auto �6+   �� 5`      $
��Auto �6+   $	��150��68   �� 5a      $
��Auto �6+   �� 5b      $
��Auto �6+   $	��150��68   �� 5c      $
��Auto �6+   �� 5d      $
��Auto �6+   $	��150��68   �� 5e      $
��Auto �6+   �� 5f      $
��Auto �6+   $	��150��68   .+�5i      �� $	��AGV��6    $��0��6+   $��0��68   #��[ 5j       �� 5l      $��Codice:��6    $��1��6/   $��0��6<   �� 5m      $��1��6A   $��1��6N   ��6   �� *Code_Agv_Machine+ �� 
IsReadOnly 6^   �� *
IsReadOnly+�� 5o      $��Indirizzo IP:��6    $��1��65   $��2��6B   �� 5p      $��1��6;   $��3��6H   ��6   �� *
CHL_IP_Agv+ 6X   �� *
IsReadOnly+�� 5r      $��Porta:��6    $��1��6.   $��4��6;   �� 5s      $��1��6=   $��5��6J   ��6   �� *CHL_Port_Agv+ 6Z   �� *
IsReadOnly+�� 5u      $��Classe:��6    $��1��6/   $��6��6<   y�6L   �� *Combination+�� 5v      $��1��6>   $��7��6K   ��6   �� *CTR_Class_Agv+y�6[   �� *Combination+ 6~   �� *
IsReadOnly+�� 5x      $��Numero:��6    $��1��6/   $��8��6<   �� 5y      $��1��6B   $��9��6O   ��6   �� *CTR_Number_Cradle+ 5z      �� *
IsReadOnly+�� 5|      $��CRADLE��6    $��3��6.   $��0��6;   $��0 10 0 0q�6K   #��[ 5~       y�5}       �� *CradleVisible+5~       �� 5�      $��Indirizzo IP:��6    $��4��65   $��0��6B   y�6R   �� *CradleVisible+�� 5�      $��4��6>   $��1��6K   ��6   �� *CHL_IP_Cradle+ 6[   �� *
IsReadOnly+y�6}   �� *CradleVisible+�� 5�      $��Porta Send:��6    $��4��63   $��2��6@   y�6P   �� *CradleVisible+�� 5�      $��4��6B   $��3��6O   ��6   �� *CHL_Port_Cradle_S+ 6_   �� *
IsReadOnly+y�6�   �� *CradleVisible+�� 5�      $��Porta Receive:��6    $��4��66   $��4��6C   y�6S   �� *CradleVisible+�� 5�      $��4��6B   $��5��6O   ��6   �� *CHL_Port_Cradle_R+ 6_   �� *
IsReadOnly+y�6�   �� *CradleVisible+�� 5�      $��Classe:��6    $��4��6/   $��6��6<   y�6L   �� *CombinationCradle+�� 5�      $��4��6A   $��7��6N   ��6   �� *CTR_Class_Cradle+y�6^   �� *CombinationCradle+ 6�   �� *
IsReadOnly+�� 5�       �� Orientation$ 
Horizontal=�6!   $��5��6:   $��5��6G   y�5�   !   �� *SimulatorVisible+.+�5�      �� $��
Simulatore��6    �� 5�       d� 	IsChecked 6   �� *	Simulator+ � LayoutTransform 5�      �� 5�       �� ScaleX$	 1.2Q�6-    �� ScaleY$	 1.2Q�6:   5�      5�      5�      5�      5�      5�      5�      