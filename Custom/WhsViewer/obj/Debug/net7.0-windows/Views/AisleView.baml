   M S B A M L   `   `   `  ����D  @WhsViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WhsViewer.ViewsWhsViewer.Views  5      %clr-namespace:WhsViewer	WhsViewer  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5	          WhsViewer.Views.AisleView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:WhsViewer.Views     5      (rootclr-namespace:WhsViewer     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5	         MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5      "   WhsViewer.UpperCaseConverter  UppercaseConverter �� 
TargetType  MaterialDesignTextBlock �� BasedOn �� Property �� Value��%�   &	       '��      7    5   
   6"   �� 5   
   " ��6   8   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 6q�6'   5      5      .��5      � .+�5   
   #  MaterialDesignThemes.Wpf.Card  .  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�6   .��5      ]� .+�5      (  "MaterialDesignThemes.Wpf.ColorZone     Mode$ Accent=�6+   $	��Top=�69   $��35��6N   +  %MaterialDesignThemes.Wpf.ShadowAssist	   ShadowDepth$	 Depth0=�5   /   .��5      �� $
��12 8q�6v   $��Center=�5    $   $
��Bold"�6?    $  MaterialDesignSubtitle2TextBlock#��[ 5!   $   ��5       �� *CurrentAisle.ASL_Desc+
 �� 	Converter#
 [ 5!   $   5"      '  !MaterialDesignThemes.Wpf.Snackbar  5$      $��Bottom=�6W   $��Stretch=�5%   .      MessageQueue 5$   *   4  .MaterialDesignThemes.Wpf.MessageQueueExtension  -	  'mSwDllWPFUtils.SnackBarCustomProperties 	  SnackBarNotification 5&   .   �� *SnackBarMessage+� 5(      $
��12 8q�6   $��Stretch=�6%    � ColumnDefinitions 5)      �� 5*      $
��Auto �6+   $	��200��68   �� 5+      $��* �6+    � RowDefinitions 5-      �� 5.      $
��Auto �6(   �� 5/      $
��Auto �6(   �� 50      $
��Auto �6(   �� 51      $��0 �6(   �� 52      $��0 �6(   �� 53      $��0 �6(   �� 54      $
��Auto �6(   .+�57      ��  �� Orientation$ 
Horizontal=�6!   $��0��6:   $��2��6G   $��Center=�58   !   .+�59      �� ��6$   '
  !mSwDllWPFUtils.TranslateExtension
  *Disabled in+ 
  	Uppercase$
 True���� 5:       $  MaterialDesignSwitchToggleButton#��[ 6#    d� 	IsChecked 5;   #   �� *	EnabledIN+{�5<   #   �� *
UserPerm+�� 5=      ��6$   
  *
Enabled in+$
 True��5>      �� 5@      $ 
Horizontal=�6!   $��1��6:   $��2��6G   $��Center=�5A   !   .+�5B      �� ��6$   
  *Disabled out+$
 True���� 5C      #��[ 6#    5D   #   �� *
EnabledOUT+{�5E   #   �� *
UserPerm+�� 5F      ��6$   
  *Enabled out+$
 True��5G      �� 5I      -   5        $
��Save��5I      $��2��5J   !   $��2��6.   $��5 5 5 20q�6B   $��Right=�5K   !    # MaterialDesignRaisedLightButton#��[ 5L   !   $	��100��5M   S   ��6!   
  *Save+$
 True��{�5N   !   �� *HasPermission+�� 5P      $��3��6]   $��0��6j   ��6    
  *Filling percentage+$
 True��6j   �� 5Q      $��3��6    $��1��6-   $
��Bold"�6=   �� 5R      �6"   �� *FillingPercentage+ �� StringFormat$ N2�� �� ConverterCulture$ IT�� �� 5S      $�%��6"   5T      �� 5V      $��4��6^   $��0��6k   ��6    
  *Unusable percentage+$
 True��6k   �� 5W      $��4��6    $��1��6-   $
��Bold"�6=   �� 5X      �6"   �� *UnusablePercentage+$ N2��$ IT�� �� 5Y      $�%��6"   5Z      �� 5\      $��5��6]   $��0��6j   ��6    
  *Cells to recompact+$
 True��6j   �� 5]      $��5��6B   $��1��6O   $
��Bold"�6_   ��6    �� *CellsToRecompact+6_   �� 5_      $ 
Horizontal=�6!   $��5��6:   $��2��6G   $y�	Collapsed=�6W   .+�5`      �� -   5        $��NavigateBack��5`   !    0 ,MaterialDesignFloatingActionMiniAccentButton#��[ 5a   %   $��24��5d   %   $��24��60   $
��5 -2q�6<    � ToolTip 5b   %   
  *See previous+y�5c   %   �� *HasCompactableCells+  	 BooleanToVisibilityConverter#
 [	 .��5e      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ 	ArrowLeft=�66   $��16��6G   $��16��6S   5f      �� 5g      -   5        $��NavigateForward��5g   !   #��[ 5h   %   $��24��5k   %   $��24��60   $
��5 -2q�6<    5i   %   
  *
See next+y�5j   %   �� *HasCompactableCells+#
 [	 .��5l        $ 
ArrowRight=�66   $��16��6H   $��16��6T   5m      5n      �� 5p      -   5        $��RecompactAll��5s   !   $��6��5p   X   $��0��6e   $��3��6u   $��5q�6�   $��Center=�5r   !   $y�	Collapsed=�5s   5   ��5p      
  *Recompact All+$
 True��{�5q   !   �� *HasCompactableCells+5s   5   5t      5v      5w        5y   
      UniformCornerRadius$ 25Q�6   $��50��67   $��50��6B   
�������$	 Depth3=�6a   $��Center=�5{      $��Center=�69   y�5z      �� *	IsLoading+#
 [	 .��5|      �  %
 !MaterialDesignCircularProgressBar#��[
 6    � IsIndeterminate$
 True��5}       � Value$ 0Q�61   $��40��6;   $��40��6F   5~      5      5�      