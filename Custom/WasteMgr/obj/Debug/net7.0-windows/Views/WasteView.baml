   M S B A M L   `   `   `  ����C  ?WasteMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WasteMgr.ViewsWasteMgr.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5	      D @AgilogDll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:AgilogDll;assembly=AgilogDll	AgilogDll 5
          WasteMgr.Views.WasteView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:WasteMgr.Views     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils  5	      7custom*clr-namespace:AgilogDll;assembly=AgilogDll  5
      ^	 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 5      Bi:http://schemas.microsoft.com/expression/2010/interactivity 	 5      -   5           MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5      $��Roboto)�5       � 	Resources 5      <  6mSwDllWPFUtils.Converters.BooleanToVisibilityConverter & "InvertBooleanToVisibilityConverter�  �mSwDllWPFUtils.Converters.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False �� 
TargetType   MaterialDesignBody2TextBlock �� BasedOn �� Property �� Value  MaterialDesignRaisedButton  MaterialDesignCardGroupBox��%�  &	       '�� 6     7  '�� �     7  '�� �    7    5   
   $ Visible=�6]   $ 	Collapsed=�6l   �� 5   
   " ��6   8   .�5      ��  � ��$ Center=�62   �� 5       � ��$
 10 2q�6'   �� 5       � ��$ 20(�6)   �� 5       � ��$ Normal&�6,   5      �� 5    
   " ��6   8   .�5!      ��  � ��$ 4q�6'   �� 5"       � ��$ Center=�64   �� 5#       � ��$ 40��6*   �� 5$       � ��$	 120��6)   5%      �� 5&   
   " ��6   8   .�5'      ��  � ��$ 	12 6 12 4q�6'   �� 5(       � ��$ 0q�6(   �� 5)       � ��$ Stretch=�62   5*      5+      .��5-        AgilogDll.DpiDecorator  ._�5.   
   � .+�5/      �� $��1��6   .  (MaterialDesignThemes.Wpf.ColorZoneAssist	   Mode$	 Accent=�6'   ��50      �� 51       $  MaterialDesignSubtitle2TextBlock#��[ 6S   $��8q�6�    " SecondaryHueMidForegroundBrush#���  52   $   ��51       '  !mSwDllWPFUtils.TranslateExtension  *	Details+
   	Uppercase$

 True��52   $   .��54      ]� .+�55      '  !MaterialDesignThemes.Wpf.Snackbar  $��Bottom=�6[      MessageQueue 6.   4  .MaterialDesignThemes.Wpf.MessageQueueExtension  6[   � 5<       � ColumnDefinitions 5=      �� 5>      $
��Auto �6/   �� 5?      $��5* �6/   .+�5B      �� $��0 8 0 4q�6!    �� Padding$ 8q�62   $��5q�6>     SecondaryHueMidBrush#��[ 6R   +	  %MaterialDesignThemes.Wpf.ShadowAssist 	  ShadowDepth$ Depth1=�5C   .   ._�5D      �� $
��Auto=�6+   $
��Auto=�6N   .��5E   "   �  5F   &   �� 5G   *   $��* �6;   �� 5H   *   $��10 �6;   �� 5I   *   $��* �6;    � RowDefinitions 5L   &   �� 5M   *   $��* �68   �� 5N   *   $��* �68   �� 5O   *   $��* �68   �� 5P   *   $��* �68   �� 5Q   *   $��* �68   �� 5R   *   $��* �68   �� 5S   *   $��* �68   �� 5T   *   $��* �68   �� 5U   *   $��* �68   .+�5X   &   �� $��0��60   $��0��6=   ��6M     *
Udc code+$

 True���� 5Y   &   $��0��60   $��2��6=   ��6M   �� *Udc.UDC_Code+�� 5[   &   $��1��60   $��0��6=   ��6M     *Weight+$

 True���� 5\   &   $��1��60   $��2��6=   ��6M   �� *Udc.UDC_Weight_Real+ �� StringFormat$ N0�� �� ConverterCulture$ IT���� 5^   &   $��2��60   $��0��6=   ��6M     *Pallet Layers+$

 True���� 5_   &   $��2��60   $��2��6=   ��6M   �� *Item.ITM_PalletLayers+�� 5a   &   $��3��60   $��0��6=   ��6M     *Box per layer+$

 True���� 5b   &   $��3��60   $��2��6=   ��6M   �� *Item.ITM_BoxPerLayer+�� 5d   &   $��4��60   $��0��6=   ��6M     *	Item Code+$

 True���� 5e   &   $��4��60   $��2��6=   ��6M   �� *Item.ITM_SubCode+�� 5g   &   $��5��60   $��0��6=   ��6M     *Item Description+$

 True���� 5h   &   $��5��60   $��2��6=   ��6M   �� *Item.ITM_Desc+�� 5j   &   $��6��60   $��0��6=   ��6M     *X (mm)+$

 True���� 5k   &   $��6��60   $��2��6=   ��6M   �� *Item.ITM_SizeX+�� 5m   &   $��7��60   $��0��6=   ��6M     *Y (mm)+$

 True���� 5n   &   $��7��60   $��2��6=   ��6M   �� *Item.ITM_SizeY+�� 5p   &   $��8��60   $��0��6=   ��6M     *Z (mm)+$

 True���� 5q   &   $��8��60   $��2��6=   ��6M   �� *Item.ITM_SizeZ+5r   #   5s      5t      �� 5v      $��1��6'   $��Stretch=�67   $
��Auto=�6U   $
��Auto=�6x   .��5w      � $��Center=�6#   $��1��6@    5x   "   �� 5y   &   $��* �64   �� 5z   &   $
��Auto �64   .+�5}   "   #
  MaterialDesignThemes.Wpf.Card
  $��0��66   
���  ��$	��700��5~   &   $
��Auto��62   $��Center=�6@   $	��Top=�6]   $��0 8 0 4q�5   &   $��8q�67   $ Depth1=�5�   &   y�5�   &   �� *ShapeControlVisible+  	 BooleanToVisibilityConverter �� 	Converter# [	 .��5�   &   ]� .+�5�   *   � $	��0 4q�6/    5�   .   �� 5�   2   $
��Auto �6@   �� 5�   2   $
��Auto �6@   �� 5�   2   $
��Auto �6@   �� 5�   2   $
��Auto �6@   �� 5�   2   $
��Auto �6@   �� 5�   2   $
��Auto �6@   �� 5�   2   $
��Auto �6@   �� 5�   2   $
��Auto �6@   .+�5�   .   �� $��0��68   ��5�   <   �� *HeightClass.HGT_Desc+y�5�   <   �� *HeightClass.HGT_Waste+# [	 �� 5�   .   $��1��68   ��5�   <   �� *WidthClass.WDT_Desc+y�5�   <   �� *WidthClass.WDT_Waste+# [	 �� 5�   .   $��2��68   ��5�   <   �� *LengthClass.LNG_Desc+y�5�   <   �� *LengthClass.LNG_Waste+# [	 �� 5�   .   $��3��68   ��5�   <   �� *WeightClass.WGH_Desc+y�5�   <   �� *WeightClass.WGH_Waste+# [	 �� 5�   .   $��4��68   ��5�   <     *Etichetta non leggibile+$

 True��y�5�   <   �� *BarcodeError+# [	 �� 5�   .   $��5��68   ��5�   <     *Etichetta non riconosciuta+$

 True��y�5�   <   �� *
NotExist+# [	 �� 5�   .   $��6��68   ��5�   <     *Barcode troppo lungo+$

 True��y�5�   <   �� *BarcodeLength+# [	 �� 5�   .   $��7��68   ��5�   <     *Etichetta duplicata+$

 True��y�5�   <   �� *
Duplicated+# [	 5�   +   5�   '   5�   #   �� 5�   "   $��UniformToFill=�6G   $��1��6_   $��0��6l   ��6(   �� *PalletImage+ � LayoutTransform 5�   &   �� 5�   *    �� ScaleX$	 0.4Q�69    �� ScaleY$	 0.4Q�6F   5�   #   5�      5�      5�      5�      5�      5�      5�      5�      