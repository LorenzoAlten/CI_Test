   M S B A M L   `   `   `  ����D  @WhsViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WhsViewer.ViewsWhsViewer.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      G4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5	      '    !WhsViewer.Views.WarehouseCellView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:WhsViewer.Views     5      IWPFUtils4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5	      b	 ^SharpVectors.Converters.Wpf, Version=1.7.1.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/ 	 5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5      y�5      �� *	HasCell+   BooleanToVisibilityConverter �� 	Converter# [  � 	Resources 5       �� 
TargetType  MaterialDesignTextBlock �� BasedOn �� Property �� Value�� 5   
   %�   '��       7  �� 5      " ��6   8   .�5      ��  � ��$ Center=�66   �� 5       � ��$ 6q�6+   5      5      .��5      � .+�5    
   #  MaterialDesignThemes.Wpf.Card  .  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�6   .��5!      ]� .+�5"      (  "MaterialDesignThemes.Wpf.ColorZone  	   Mode$	 Accent=�6+   $	��Top=�69   $��35��6N   .��5#      � $
��12 8q�6    5$       $  MaterialDesignSubtitle2TextBlock��%_   '��       7  �� 5%      " ��6$   8   .�5&   "   ��  � ��$ Center=�6F   5'      5(      
 � ColumnDefinitions
 5)      �� 5*      $
��Auto �6/   $��80��6<   �� 5+      $
��Auto �6/   �� 5,      $
��Auto �6/   $��40��6<   �� 5-      $
��Auto �6/   �� 5.      $
��Auto �6/   $��40��6<   �� 5/      $
��Auto �6/   �� 50      $
��Auto �6/   $��40��6<   .+�53      �� $��0��6Q   ��6$   '  !mSwDllWPFUtils.TranslateExtension  *Cell+   	Uppercase$
 True��6Q   �� 54      $��1��6H   ��6$     *X+   Suffix$ :��6H   �� 55      $��2��6E   ��6$   �� *WarehouseCell.X+6E   �� 56      $��3��6H   ��6$     *Y+$ :��6H   �� 57      $��4��6E   ��6$   �� *WarehouseCell.Y+6E   �� 58      $��5��6I   ��6$     *ID+$ :��6I   �� 59      $��6��6J   ��6$   �� *WarehouseCell.CEL_Id+6J   5:      5;      '  !MaterialDesignThemes.Wpf.Snackbar  5=      $��Bottom=�6W   $��Stretch=�5>   &      MessageQueue 5=   *   4  .MaterialDesignThemes.Wpf.MessageQueueExtension  -  'mSwDllWPFUtils.SnackBarCustomProperties   SnackBarNotification 5?   &   �� *SnackBarMessage+]� 5A      .+�5C      �� $	��100��6!   $	��100��6.   $
��Left=�6:   $��8q�5D   !   .+�5E      �� $��90��5F       $��90��6,    �� Left$ 15��5G       ��5E       /	 	 )SharpVectors.Converters.SvgImageExtension	  */Images/cell_dimensions.svg+5G       �� 5I      #��[ 6$    �� Bottom$ 45��5K   $   $ 5��67   ��5J   $   �� *$"WarehouseCell.HeightClass.HGT_Desc+ � ToolTip 5L   $     *Height+ � LayoutTransform 5M      �� 5N   "    �� Angle$	 270Q�62   5P      �� 5R      #��[ 6$   $ 0��5T   (   $ 25��6:   ��5S   (   �� *#!WarehouseCell.WidthClass.WDT_Desc+ 5U   (     *Width+ 5V      �� 5W   "   $ 30Q�62   5Y      5d      � 5f      $
��12 8q�6    � RowDefinitions 5g      �� 5h      $
��Auto �6,   �� 5i      $
��Auto �6,   �� 5j      $
��Auto �6,   �� 5k      $
��Auto �6,   �� 5l      $
��Auto �6,   �� 5m      $
��Auto �6,   .+�5p      �� $��0��6]     SecondaryHueMidBrush#��[ 5q       $��Center=�6S   ��5p   $     * Reserved for order preparation+y�5r       �� *!WarehouseCell.CEL_ReservedForPr+# [ �� 5t       �� Orientation$ 
Horizontal=�6%   $��1��6>   $��Center=�5u   !   .+�5v      �� ��6(     *Disabled in+$
 True���� 5w       $  MaterialDesignSwitchToggleButton#��[ 6'    d� 	IsChecked 5x   #   �� *	EnabledIN+{�5y   #   �� *
UserPerm+�� 5z      ��6(     *
Enabled in+$
 True��5{      �� 5}      $ 
Horizontal=�6%   $��2��6>   $��Center=�5~   !   .+�5      �� ��6(     *Disabled out+$
 True���� 5�      #��[ 6'    5�   #   �� *
EnabledOUT+{�5�   #   �� *
UserPerm+�� 5�      ��6(     *Enabled out+$
 True��5�      �� 5�      -   5        $��	Recompact��5�      $��3��5�   X   $��5q�6e      MaterialDesignOutlinedButton#��[ 5�      $��Center=�5�      $y�	Collapsed=�5�   .   ��5�   !     *	Recompact+$
 True��5�   .   5�      5�      5�      5�        5�   
      UniformCornerRadius$ 25Q�6   $��50��67   $��50��6B   
�������+
  %MaterialDesignThemes.Wpf.ShadowAssist 
  ShadowDepth$ Depth3=�6a   $��Center=�5�      $��Center=�69   y�5�      �� *	IsLoading+# [ .��5�      �  %	 !MaterialDesignCircularProgressBar#��[	 6    � IsIndeterminate$
 True��5�       � Value$ 0Q�61   $��40��6;   $��40��6F   5�      5�      5�      