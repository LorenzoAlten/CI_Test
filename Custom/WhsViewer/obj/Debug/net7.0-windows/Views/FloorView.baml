   M S B A M L   `   `   `  ����D  @WhsViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WhsViewer.ViewsWhsViewer.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5          WhsViewer.Views.FloorView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:WhsViewer.Views     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5         MaterialDesignBody#���   5	      $��Regular"�5
      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5       �� 
TargetType  MaterialDesignTextBlock �� BasedOn �� Property �� Value��%�   '��       7  �� 5   
   " ��6   8   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 6q�6'   5      5      .��5      � .+�5   
   #  MaterialDesignThemes.Wpf.Card  .  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�6   .��5      ]� .+�5      (  "MaterialDesignThemes.Wpf.ColorZone     Mode$ Accent=�6+   $	��Top=�69   $��35��6N   .��5      �� 	 �� Orientation$	 
Horizontal=�6!   $
��12 8q�6:    5       $  MaterialDesignSubtitle2TextBlock��%_   '��       7  �� 5      " ��6$   8   .�5   "   ��  � ��$ Center=�6F   5       5!      .+�5"      �� $
��Bold"�6R   ��6$   '  !mSwDllWPFUtils.TranslateExtension  *Floor+
   	Uppercase$

 True��6R   �� 5#      $	��5 0q�6B   $
��Bold"�6O   ��6$   �� *CurrentLevel+6O   5$      5%      '  !MaterialDesignThemes.Wpf.Snackbar  5'      $��Bottom=�6W   $��Stretch=�5(   &      MessageQueue 5'   *   4  .MaterialDesignThemes.Wpf.MessageQueueExtension  -  'mSwDllWPFUtils.SnackBarCustomProperties   SnackBarNotification 5)   &   �� *SnackBarMessage+� 5+      $
��12 8q�6   $��Stretch=�6%    � ColumnDefinitions 5,      �� 5-      $
��Auto �6+   $	��200��68   �� 5.      $��* �6+    � RowDefinitions 50      �� 51      $
��Auto �6(   �� 52      $
��Auto �6(   �� 53      $
��Auto �6(   �� 54      $��0 �6(   �� 55      $��0 �6(   �� 56      $��0 �6(   �� 57      $
��Auto �6(   .+�5:      �� $	 
Horizontal=�6!   $��0��6:   $��2��6G   $��Center=�5;   !   .+�5<      �� ��6$     *Disabled in+$

 True���� 5=       $  MaterialDesignSwitchToggleButton#��[ 6#    d� 	IsChecked 5>   #   �� *	EnabledIN+{�5?   #   �� *
UserPerm+�� 5@      ��6$     *
Enabled in+$

 True��5A      �� 5C      $	 
Horizontal=�6!   $��1��6:   $��2��6G   $��Center=�5D   !   .+�5E      �� ��6$     *Disabled out+$

 True���� 5F      #��[ 6#    5G   #   �� *
EnabledOUT+{�5H   #   �� *
UserPerm+�� 5I      ��6$     *Enabled out+$

 True��5J      �� 5K      -   5        $
��Save��5K      $��2��5L   !   $��2��6.   $��5 5 5 20q�6B   $��Right=�5M   !    # MaterialDesignRaisedLightButton#��[ 5N   !   $	��100��5O   S   ��6!     *Save+$

 True��{�5P   !   �� *HasPermission+�� 5R      $��3��6]   $��0��6j   ��6      *Filling percentage+$

 True��6j   �� 5S      $��3��6    $��1��6-   $
��Bold"�6=   �� 5T      �6"   �� *FillingPercentage+ �� StringFormat$ N2�� �� ConverterCulture$ IT�� �� 5U      $�%��6"   5V      �� 5X      $��4��6^   $��0��6k   ��6      *Unusable percentage+$

 True��6k   �� 5Y      $��4��6    $��1��6-   $
��Bold"�6=   �� 5Z      �6"   �� *UnusablePercentage+$ N2��$ IT�� �� 5[      $�%��6"   5\      �� 5^      $��5��6]   $��0��6j   ��6      *Cells to recompact+$

 True��6j   �� 5_      $��5��6B   $��1��6O   $
��Bold"�6_   ��6    �� *CellsToRecompact+6_   5`      5a      5b        5d   
      UniformCornerRadius$ 25Q�6   $��50��67   $��50��6B   
�������+	  %MaterialDesignThemes.Wpf.ShadowAssist 	  ShadowDepth$ Depth3=�6a   $��Center=�5f      $��Center=�69   y�5e      �� *	IsLoading+   BooleanToVisibilityConverter �� 	Converter# [ .��5g      �  % !MaterialDesignCircularProgressBar#��[ 6    � IsIndeterminate$
 True��5h       � Value$ 0Q�61   $��40��6;   $��40��6F   5i      5j      5k      