   M S B A M L   `   `   `  ����D  @Movitrans, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Movitrans.ViewsMovitrans.Views  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5	      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5
      ;"clr-namespace:Movitrans.ViewModelsMovitrans.ViewModels  5      %clr-namespace:Movitrans	Movitrans  5          Movitrans.Views.AppView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R	 NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml   	  5      @d2http://schemas.microsoft.com/expression/blend/2008   	  5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006   	  5      /localclr-namespace:Movitrans.Views   	  5      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf   	  5      b
 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 
 5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 
 5	      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 
 5
      ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5      2	viewModel"clr-namespace:Movitrans.ViewModels  5      '	movitransclr-namespace:Movitrans  5         MaterialDesignBody#���   5      $��Regular"�5      $��14(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5      +   %Movitrans.MultiFlagToBooleanConverter  MultiFlagToBooleanConverter��%   &	         5   
   60   5      .��5      ]� .+�5   
   � $��Bottom=�6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5       $
��Auto �6#   �� 5!      $��* �6#   �� 5"      $
��Auto �6#   .+�5%      �� $��0��6   $	��5 0q�6U   ��6+   �� *ManagerCollection+��5&      �� 5'      .R�5(      ��  �� Orientation$ 
Horizontal=�6%   5)      ��5+      �� 5,      .R�5-      �� $ 
Horizontal=�6%   .+�5.      �� $ 
Horizontal=�6)   $��5q�6B   .+�5/   "   �� ��6,   ��  �� Path$
 Name�*  $mSwAgilogDll.TrafficManagerStateIcon  50   "   $	��5,0q�6A   �� 51   "   $��:��6,   $��0,0,5,0q�65   �� 52   "   ��6/   �� *ControllerCollection+��53   &   �� 54   *   .R�55   .   �� $ 
Horizontal=�69   56   +   ��58   &   �� 59   *   .R�5:   .   �� $ 
Horizontal=�69   $��0,0,5,0q�6R   .+�5;   2   �� ��6<   �� $
 Desc�-  'mSwAgilogDll.TrafficControllerStateIcon  5<   2   $	��5,0q�6T   5=   /   5>   +   5@   #   5A      �� 5B      ���#333333$��1��67   $��0q�6A   5C      5D      5F      �� 5H      $��4��6R   $
��10 5q�6b   $y�	Collapsed=�6p   ��6   �� $	 Now� �� StringFormat$ dd/MM/yy HH:mm:ss��6p   5I      �� 5J   
   $��Bottom=�6   $	��5 0q�6,   � 5K   
   .+�5L      ]� .+�5M      �� $	��Top=�6@   ��6   �� *SewMovitrans+ �� SelectedItem 5N      �� *SelectedMovitrans+��5O      �� 5P      .R�5Q      �� $ 
Horizontal=�6)   5R      ��5T      �� 5U      .R�5V      �� $
��16 8q�5W   (    $  MaterialDesignSubtitle1TextBlock#��[ 66   ��5V   (   �� *Code+5W   6   5X      5Z      � 5\      $��Bottom=�6    � DataContext 6/   �� *SelectedMovitrans+ 5]      	 �� 
TargetType
 �� BasedOn �� Property �� Value�� 5^      %�  '��       7��'�� �     7��'��     7���� 5_      "	 ��6$   8
   .�5`   "   ��  � ��$ Center=�6F   �� 5a   "    � ��$
 Bold"�6?   �� 5b   "    � ��$ 20 5 5 5q�6;   5c      �� 5d      "	 ��6$   8
   .�5e   "   ��  � ��$ Center=�6F   �� 5f   "    � ��$ 5q�6;   5g      �� 5h      "	 ��6$   8
   .�5j   "   ��  � ��$	 0 5q�6;   5k      5l       � RowDefinitions 5n      �� 5o      $
��Auto �6(   68   �� 5p      $
��Auto �6(   68   �� 5q      $	��300 �6(   67   �� 5r      $
��Auto �6(   68   �� 5s      $
��Auto �6(   68   �� 5t      $
��Auto �6(   68   �� 5u      $
��Auto �6(   68   �� 5v      $
��Auto �6(   68    5x      �� 5y      $
��0.5* �6+   6:   �� 5z      $
��0.5* �6+   6:   .+�5|      ( 
 "MaterialDesignThemes.Wpf.ColorZone  $��0��6/   $��2��6<   $��16 10q�6P   + 
 %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�6`      Mode$ 
PrimaryMid=�6�    K� ZIndex$ 1��5}   '   .��5~      ]� $��Stretch=�6$   .+�5      �� $ 
Horizontal=�6)   $��Right=�6B   .+�5�   "   �� $��Enabled��6,   $��Right=�6;   d� 5�   "   $��Right=�6M    d� 	IsChecked 6/   �� *	Enabled+6M   5�      �� 5�       $  MaterialDesignRaisedAccentButton#��[ 6%     Caliburn.Micro.Message   Attach$ EditConfiguration��5�   !   $	��5 0q�6H   $
��Left=�5�   Q    � ToolTip 6!   '  !mSwDllWPFUtils.TranslateExtension  *Edit Configuration+.��5�   "   '	 
 !MaterialDesignThemes.Wpf.PackIcon	   	  Kind$
 Edit=�6:   $��24��5�   :   $��24��6F   5�      �� 5�       �� TextAlignment$ Center=�6>   $��Stretch=�6U   $��Center=�6s   $��22(�6�   ��6(   �� *Code+6�   5�      5�      �� 5�      $��1��6    $��0��6-   $��SetPoint��6=   $��Center=�6M   6l   �� 5�      $��1��6    $��1��6-   $��Output Power��6=   $��Center=�6Q   6p   !
  LiveCharts.Wpf.AngularGauge
  5�      $
��5 10q�6'   $��0q�65   $��2��6A   $��0��6N    
  	FromValue$ 0Q�6y    
  ToValue$	 105Q�6�    
  
LabelsStep$ 10Q�5�       
  	TicksStep$	 2.5Q�6+    
  Wedge$	 300Q�6;    
  TicksForeground
 �����
�������$
��Bold"�5�      $��16(�6-    
  SectionsInnerRadius$ .5Q�5�       
  Value 5�   ^   �� *
SetPoint+ 5�      �� 5�      %Z   '��       7���� 5�   "   "	 ��6(   8
   .�5�   &   ��  � ��$ 0q�6?   5�   #   5�       
  Sections 5�      #  LiveCharts.Wpf.AngularSection     	FromValue$ 0Q�61       ToValue$  30Q�6?     PrimaryHueDarkBrush!   Fill#! �  6L     5�      $ 30Q�61   $  80Q�6@     PrimaryHueLightBrush#! �  6M     5�      $ 80Q�61   $	  105Q�6@     SecondaryHueMidBrush#! �  6N   5�      
  5�      $��1��6'   $
��5 10q�67   $��2��6E   $ 0Q�6p   $ 16Q�6~   $ 1Q�5�      $
 0.25Q�6*   $	 300Q�6;   
 �����
�������$
��Bold"�5�      $��16(�6-   $ .5Q�5�       5�   R   �� *OutputPower+ 5�      �� 5�      %Z   '��       7���� 5�   "   "	 ��6(   8
   .�5�   &   ��  � ��$ 0q�6?   5�   #   5�       5�        $ 0Q�61   $	  3.2Q�6?   #! �  6M     5�      $	 3.2Q�61   $  8Q�6A    	 PrimaryHueMidBrush#! � 	 6M     5�      $ 8Q�61   $  13Q�6?   #! �  6L     5�      $ 13Q�61   $  16Q�6@   #! �  6M   5�      ]� 5�      $��3��6!   $��0��6.   .+�5�      �� $��Error Code:��6$   $	��100��67   �� 5�      $��1��6"   $	��100��62   $��Stretch=�6A   " �� 
IsReadOnly$
" True��6z   ��6_   �� *	ErrorCode+6z   5�      ]� 5�      $��3��6!   $��1��6.   .+�5�      �� $��Warning Code:��6$   $	��100��69   �� 5�      $��1��6"   $	��100��62   $��Stretch=�6A   $
" True��6|   ��6_   �� *WarningCode+6|   5�      ]� 5�      $��4��6!   $��0��6.   .+�5�      �� $��Temperature:��6$   $	��100��68   �� 5�      $��1��6"   $	��100��62   $��Stretch=�6A   $
" True��6|   ��6_   �� *Temperature+6|   5�      ]� 5�      $��4��6    $��1��6-   .+�5�      �� $��Utilization:��6$   $	��100��68   �� 5�      $��1��6"   $	��100��62   $��Stretch=�6A   $
" True��6|   ��6_   �� *Utilization+6|   5�      ]� 5�      $��5��6    $��2��6-   $��15q�6A   6O   � 5�      $��6��6   $��2��6(   $��	10 5 10 5q�6<    5�      �� 5�      6.   �� 5�      6.    5�      �� 5�      $��* �6/   6;   �� 5�      $��* �6/   6;   �� 5�      $��* �6/   6;   �� 5�      $��* �6/   6;   �� 5�      $��* �6/   6;   �� 5�      $��* �6/   6;   .+�5�      �� $��Enabled��6#   $��0��65   ${�False��6E    5�      `� 5�   "   # `� 	Converter## [ 6/   .1�5�   &   �� $ Status �6.   �� 5�   &      Movitrans.MOV_Status_Word$   Movitrans_Enabled% �� Source#% Z$ 6.   & �� Mode$& OneWay=�6n   5�   #   5�      �� 5�      $��TPS Volt Limit��6#   $��1��6<   ${�False��6L    5�      `� 5�   "   ## [ 6/   .1�5�   &   �� $ Status�6.   �� 5�   &   '   TPS_Voltage_Limit#% Z' 6.   $& OneWay=�6n   5�   #   5�      �� 5�      $��Error��6#   $��2��63   ${�False��6C    5�      `� 5�   "   ## [ 6/   .1�5�   &   �� $ Status�6.   �� 5�   &   (   Error#% Z( 6.   $& OneWay=�6b   5�   #   5�      �� 5�      $��Warning��6#   $��3��65   ${�False��6E    5�      `� 5�   "   ## [ 6/   .1�5�   &   �� $ Status�6.   �� 5�   &   )   Warning#% Z) 6.   $& OneWay=�6d   5�   #   5�      �� 5�      $��Device connected��6#   $��4��6>   ${�False��6N    5�      `� 5�   "   ## [ 6/   .1�5�   &   �� $ Status�6.   �� 5�   &   *   Remote_device_Connected#% Z* 6.   $& OneWay=�6t   5�   #   5�      �� 5�      $��Switch enabled��6#   $��5��6<   ${�False��6L    5�      `� 5�   "   ## [ 6/   .1�5�   &   �� $ Status�6.   �� 5�   &   +   Switch_Cabinet_Enabled#% Z+ 6.   $& OneWay=�6s   5�   #   5      �� 5     $��	TES Ready��6#   $��0��67   $��1��6G   ${�False��6T    5     `� 5  "   ## [ 6/   .1�5  &   �� $ Status�6.   �� 5  &   ,   	TES_Ready#% Z, 6.   $& OneWay=�6f   5  #   5	     �� 5
     $��TES PO enabled��6#   $��1��6<   $��1��6L   ${�False��6Y    5     `� 5  "   ## [ 6/   .1�5  &   �� $ Status�6.   �� 5  &   -   TES_PO_Data_Enabled#% Z- 6.   $& OneWay=�6p   5  #   5     �� 5     $��TES end ramp��6#   $��2��6:   $��1��6J   ${�False��6W    5     `� 5  "   ## [ 6/   .1�5  &   �� $ Status�6.   �� 5  &   .   TES_End_of_Ramp#% Z. 6.   $& OneWay=�6l   5  #   5     �� 5     $��
Init error��6#   $��3��68   $��1��6H   ${�False��6U    5     `� 5  "   ## [ 6/   .1�5  &   �� $ Status�6.   �� 5  &   /   Initialization_Error#% Z/ 6.   $& OneWay=�6q   5  #   5!     �� 5"     $��TPS connected��6#   $��4��6;   $��1��6K   ${�False��6X    5#     `� 5$  "   ## [ 6/   .1�5%  &   �� $ Status�6.   �� 5&  &   0   TPS_Connected#% Z0 6.   $& OneWay=�6j   5'  #   5)     �� 5*     $��TES connected��6#   $��5��6;   $��1��6K   ${�False��6X    5+     `� 5,  "   ## [ 6/   .1�5-  &   �� $ Status�6.   �� 5.  &   1   TES_Connected#% Z1 6.   $& OneWay=�6j   5/  #   51     52     ]� 53     $��7��6    $��2��6-   $��20q�6A   .+�54     �� -   5        $��MOVReset��54  "   $��Right=�62   #��[ 6I   .��55     ]� .+�56  "   	  $
 Sync=�6:   $��Right=�6F   $��Center=�6]   6z   �� 57  "   $
��10 0q�6,   $��Reset��6:   $
��Left=�6G   $��Center=�6]   6z   58     59     �� 5:     $
��Left=�6%   6=   5;     5<     5=     � 5?      %
 !MaterialDesignCircularProgressBar#��[
 6   2 � Value$2 0Q�5@     $��40��6$   $��40��6/   
�������3 J� Opacity$	3 0.7Q�6N   $��Center=�5A     $��Center=�65   4 � IsIndeterminate$
4 True��5B     y�5C     �� *	IsLoading+   BooleanToVisibilityConverter5 �� 	Converter#5 [ 5D     5E     5F     