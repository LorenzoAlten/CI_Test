   M S B A M L   `   `   `  ����D  @WhsViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WhsViewer.ViewsWhsViewer.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5	      *    $WhsViewer.Views.WhsCellsLocationView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:WhsViewer.Views     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils  5	      ^	 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 5
      b
 ^SharpVectors.Converters.Wpf, Version=1.7.1.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/ 
 5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5      y�5      �� *HasLocation+   BooleanToVisibilityConverter �� 	Converter# [  � 	Resources 5       �� 
TargetType �� BasedOn �� Property �� Value  MaterialDesignCardGroupBox<  6mSwDllWPFUtils.Converters.BooleanToVisibilityConverter & "InvertBooleanToVisibilityConverter�  �mSwDllWPFUtils.Converters.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True	   False�� 5   
   %N  '��       7��'�� t     7��'��     7��'�� �    7  &	 �    �� 5      " ��6   8   .�5      ��  � ��$ Center=�66   �� 5       � ��$ 5q�6+   5      �� 5      " ��6   8   .�5      ��  � ��$ Center=�66   �� 5       � ��$ 5q�6+   �� 5        � ��$
 Bold"�6/   5!      �� 5"      " ��6   8   .�5#      ��  � ��$ Center=�66   �� 5$       � ��$ 5q�6+   5%      �� 5&      " ��6   8   .�5'      ��  � ��$ 8q�6,   5(        5*      $ 	Collapsed=�5+   6   $	 Visible=�5,   6   5-      .��50      � .+�51   
   #  MaterialDesignThemes.Wpf.Card  .  (MaterialDesignThemes.Wpf.ColorZoneAssist
   Mode$
 Accent=�6   .��52      ]� .+�53      (  "MaterialDesignThemes.Wpf.ColorZone     Mode$ Accent=�6+   $	��Top=�69   $��35��6N   .��54      � $��12 8 0 8q�6    55       $  MaterialDesignSubtitle2TextBlock & "MaterialDesignToolForegroundButton'  !MaterialDesignThemes.Wpf.PackIcon��%]  '��       7  '�� Q     7  '  �     �� 56      " ��6$   8   .�57   "   ��  � ��$ Center=�6F   58      �� 59      " ��6$   8   .�5:   "   ��  � ��$ Center=�6F   �� 5;   "    � ��$ 0 0 4 0q�6;   5<      �� 5=      "  6$   .�5>   "   ��  � ��$ 24��6:   �� 5?   "    � ��$ 24��6;   5@      5A       � ColumnDefinitions 5B      �� 5C      $
��Auto �6/   $��80��6<   �� 5D      $
��Auto �6/   �� 5E      $
��Auto �6/   $��40��6<   �� 5F      $
��Auto �6/   �� 5G      $
��Auto �6/   $��40��6<   �� 5H      $��* �6/   �� 5I      $
��Auto �6/   �� 5J      $
��Auto �6/   �� 5K      $
��Auto �6/   .+�5N      �� $��0��6U   ��6$   '  !mSwDllWPFUtils.TranslateExtension  *
Location+   	Uppercase$
 True��6U   �� 5O      $��1��6H   ��6$     *Z+   Suffix$ :��6H   �� 5P      $��2��6D   ��6$   �� *Location.LOC_Z+6D   �� 5Q      $��3��6H   ��6$     *W+$ :��6H   �� 5R      $��4��6D   ��6$   �� *Location.LOC_W+6D   �� 5T      -   5        $��ToMag��5T   !   $��0��5W   !   $��5��6.    � ToolTip 5U   !     *Rientro a magazzino+y�5V   !   �� *
IsBuffer+# [ .��5X           Kind$ Magnet=�66   5Y      �� 5[      -   5        $��Drive��5[   !   $��0��5^   !   $��6��6.    5\   !     *Drive to cell+y�5]   !   �� *IsMag+# [ .��5_        $ MapMarkerRadius=�66   5`      �� 5b      -   5        $
��Move��5b   !   $��0��5e   !   $��7��6.    5c   !     *Move Load Unit+y�5d   !   �� *
UserPerm+# [ .��5f        $ ArrowDecision=�66   5g      �� 5i      -   5        $��	Immediate��5i   !   $��0��5l   !   $��8��6.    5j   !     *Call immediate+y�5k   !   �� *Location.IsEmpty+# [ .��5m        $ 	ExitToApp=�66   5n      5o      5p      '	  !MaterialDesignThemes.Wpf.Snackbar	  5r      $��Bottom=�6W   $��Stretch=�5s   *    	  MessageQueue 5r   *   4
  .MaterialDesignThemes.Wpf.MessageQueueExtension
  -  'mSwDllWPFUtils.SnackBarCustomProperties   SnackBarNotification 5t   *   �� *SnackBarMessage+� 5v      $
��12 8q�6   $��Stretch=�6%    5w      �� 5x      $
��Auto �6+   �� 5y      $��* �6+   $��80��65   �� 5z      $
��Auto �6+   �� 5{      $��* �6+   $��80��65   �� 5|      $
��Auto �6+    � RowDefinitions 5      �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   $��40��66   �� 5�      $
��Auto �6(   .+�5�      ]� $��0��6    $��0��6-   $��5��6=   .+�5�      �� $	��100��6!   $	��100��6.   $
��Left=�6:   $��8q�5�   !   .+�5�      �� $��90��5�   $   $��90��60   ��5�   $   / 
 )SharpVectors.Converters.SvgImageExtension  *!/Images/location_dimensions.svg+5�   0   �� 5�      #��[ 6(    �� Bottom$ 0��5�   (    �� Left$ 50��6:   ��5�   (   �� *'%Location.UdcType.LengthClass.LNG_Desc+ 5�   (     *Length+ � LayoutTransform 5�   "   �� 5�   &    �� Angle$	 -30Q�66   5�      �� 5�      #��[ 6(   $ 0��5�   (   $ 5��6:   ��5�   (   �� *&$Location.UdcType.WidthClass.WDT_Desc+ 5�   (     *Width+ 5�   "   �� 5�   &   $ 30Q�66   5�      5�      � 5�      $	��8 0q�6   {�6,   �� *Location.CanSave+ 5�      �� 5�   "   $
��Auto �63   �� 5�   "   $��* �63    5�      �� 5�   "   $
��Auto �60   �� 5�   "   $
��Auto �60   $��35��6>   �� 5�   "   $
��Auto �60   $��35��6>   �� 5�   "   $
��Auto �60   $��35��6>   .+�5�      �� $��Center=�6(   $��2��6C   $��0 0 0 8q�5�   (   #��[ 5�   (   �� 5�   "   �6&   �� *Location.UdcType.UDT_Code+ �� 5�   "   $�-��6&    �� 5�   "   �6&   �� *Location.UdcType.UDT_Desc+5�      �� 5�      $	��Top=�5�   (   #��[ 5�   (   $��1��5�   (   $��0��65   ��5�   (     *
Enabling+$
 True��y�5�   (   �� *
UserPerm+# [ 5�   5   �� 5�      $��1��6)   $��1��66   .+�5�   "   ��  �� Orientation$ 
Horizontal=�6-   .+�5�   &   �� $	��4 0q�5�   /    d� 	IsChecked 5�   /   �� *Location.LOC_EnabledIN+y�5�   /   �� *
UserPerm+# [ 5�   /   �� 5�   &   $��Center=�5�   0      MaterialDesignBody2TextBlock#��[ 5�   0   ��5�   0     *Input+$
 True��y�5�   0   �� *
UserPerm+# [ 5�   0   5�   #   �� 5�   "   $ 
Horizontal=�6-   .+�5�   &   �� $	��4 0q�5�   /    5�   /   �� *Location.LOC_EnabledOUT+y�5�   /   �� *
UserPerm+# [ 5�   /   �� 5�   &   $��Center=�5�   0   #��[ 5�   0   ��5�   0     *Output+$
 True��y�5�   0   �� *
UserPerm+# [ 5�   0   5�   #   5�      �� 5�      $��Center=�5�   (   #��[ 5�   (   $��2��5�   (   $��0��65   ��5�   (     *State+$
 True��5�   5   �� 5�      $��2��6'   $��1��64   $ �� IsSynchronizedWithCurrentItem$
 trueY�5�   '   $	��4 0q�5�   '    �� SelectedValuePath$ LOS_Code��5�   '    �� DisplayMemberPath$ LOS_Desc��6D   ��5�   '   �� *LocationStates+ �� SelectedValue 5�   '   �� *Location.LOC_LOS_Code+5�   D   �� 5�      $��Center=�5�   (   #��[ 5�   (   $��4��5�   (   $��0��65   ��5�   (     *Error+$
 True��5�   5   �� 5�      $��4��6'   $��1��64   $	��4 0q�5�   '   $��Center=�5�   '    5�   '   �� *Location.LOC_Error+5�   '   5�      5�      �� 5�      $��1��5�       $��0��6-   $��Center=�5�       #��[ 5�       ��5�         *	Load Unit+5�       �� 5�      $��1��6   $��1��6+   $��3��6;   $
��12 4q�5�       �� 
IsReadOnly$
 True��5�      ��5�      �� *Location.LOC_UDC_Code+.��5�   0    5�      �� 5�      #���  6+   #���   5�   +   #��[ 5�   +   .@�5  "   �� $	��100��6)   $	��100��66   $��8q�5  )    � DataContext 5  )   �� *
Location+y�5  )   �� *	HasData+# [ .+�5  &   �� $��90��5  ,   $��90��68   $ 15��5  ,   ��5  ,     */Images/udc_dimensions.svg+5  ,   �� 5	  &   #��[ 60   $ 45��5  0   $ 5��6C   ��5
  0   �� *Udc.HeightClass.HGT_Desc+ 5  0     *Height+ 5  *   �� 5  .   $	 270Q�6>   5  '   �� 5  &   #��[ 60   $ 0��5  0   $ 25��6B   ��5  0   �� *Udc.WidthClass.WDT_Desc+ 5  0     *Width+ 5  *   �� 5  .   $ 30Q�6>   5  '   �� 5  &   #��[ 60   $ 0��5  0   $ 70��6B   ��5  0   �� *Udc.LengthClass.LNG_Desc+ 5  0     *Length+ 5  *   �� 5   .   $	 -30Q�6>   5"  '   5#  #   5$     .��5%  +   5&     �� 5'     -   5        $��EditUDC��5'      0	 ,MaterialDesignFloatingActionMiniAccentButton#��[	 5(     $��24��5+     $��24��6(   $��-2q�64   $��1��6@   $��4��6M   {�5)     �� *Location.CanSave+ 6D     *Edit Load unit+y�5*     �� *Location.IsEmpty+# [ .��5,       $ Pencil=�62   $��16��6@   $��16��6L   5-     &   System.Windows.Controls.DataGrid  5/         
IsReadOnly$
  True��6   !   AutoGenerateColumns$! False��61   
������ $��Stretch=�53     $��0��54     ""   VerticalScrollBarVisibility$
" Auto=�6,   #   EnableRowVirtualization$
# True��55     $
��Auto��6>   $	��100��6L   $   SelectionMode$$ Single=�6\   $��2��56     $��5��6,   $��
0,10,10,10q�6@   -  'MaterialDesignThemes.Wpf.DataGridAssist%   CellPadding$% 4 2 2 2q�57     &   ColumnHeaderPadding$& 4 2 2 2q�58     ��51     �� *Location.Udc.Compartments+y�52     �� *Location.HasData+# [ '   Columns' 59     0  *System.Windows.Controls.DataGridTextColumn  5:     ,  &System.Windows.Controls.DataGridColumn(   Header$(  ��61   )   
IsReadOnly$
) True��6;   *   Width5  /System.Windows.Controls.DataGridLengthConverter$
* Auto 5;  O   +   MinWidth$+ 40Q�6\   1  +System.Windows.Controls.DataGridBoundColumn,   Binding, 61   �� *	UCM_Index+6\     5<     $
) True��6a   $
* Auto 5=  G   ( 5<  1     *Owner+$
 True��, 5=  )   �� *	UCM_Owner+6G     5>     $
) True��6`   $
* Auto 5?  S   ( 5>  1     *Item+$
 True��, 5?  1   �� *Item.ITM_Desc+6S     5@     $
) True��6d   $
* Auto 5A  w   ( 5@  1     *
Quantity+$
 True��, 5A  1   �� *	UCM_Stock+- �� StringFormat$- F0��. �� ConverterCulture$. IT��6w     5B     $
) True��6a   $
* Auto 5C  O   ( 5B  1     *Batch+$
 True��, 5C  1   �� *	UCM_Batch+6O     5D     $
) True��6b   $
* Auto 5E  S   ( 5D  1     *Serial+$
 True��, 5E  1   �� *UCM_SerialNum+6S     5F     $
) True��6m   $
* Auto 5G  {   ( 5F  1     *Expiration date+$
 True��, 5G  1   �� *UCM_ExpiringDate+$. IT��$- d��6{   5I     �� 5K     -   5        $
��Save��5K     $��3��5L     $��3��6*   $��2��6:   $��0q�6N   $
��6 12q�6Z   $��Right=�5M     $	��Top=�69    #
 MaterialDesignRaisedLightButton#��[
 5N     $	��100��5O  O   ��6     *Save+$
 True��6O   �� 5Q     -   5        $
��Test��5Q     $��3��5R     $��1��6*   $��3��6:   $��0q�6N   $��-10 10 10 0q�6Z   $��3��6o   $
��Left=�5S     $	��Top=�68   #��[
 5T     $	��100��5U  O   y�5S  P   �� *	CanTest+# [ ��5U       *Test+$
 True��6O   5V     5W     5X       5Z  
   /   UniformCornerRadius$/ 25Q�6   $��50��67   $��50��6B   
�������+  %MaterialDesignThemes.Wpf.ShadowAssist0   ShadowDepth$0 Depth3=�6a   $��Center=�5\     $��Center=�69   y�5[     �� *	IsLoading+# [ .��5]     �  % !MaterialDesignCircularProgressBar#��[ 6   1 � IsIndeterminate$
1 True��5^     2 � Value$2 0Q�61   $��40��6;   $��40��6F   5_     5`     5a     