   M S B A M L   `   `   `  ����D  @WhsViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WhsViewer.ViewsWhsViewer.Views  5      %clr-namespace:WhsViewer	WhsViewer  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5	      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5
      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5      %clr-namespace:WhsViewer	WhsViewer  5          WhsViewer.Views.LevelView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:WhsViewer.Views     5      *customclr-namespace:WhsViewer     5      b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 	 5	      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils 	 5
      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 	 5      ^
 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 
 5      '	WhsViewerclr-namespace:WhsViewer 
 5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5         BooleanToVisibilityConverter<  6mSwDllWPFUtils.Converters.BooleanToVisibilityConverter & "InvertBooleanToVisibilityConverter�  �mSwDllWPFUtils.Converters.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False  	LineBrush j� TileMode j� Viewport j� ViewportUnits j� Viewbox	 j� ViewboxUnits
 !� Visual J� Opacity �� 
StartPoint �� EndPoint �� 
TargetType  MaterialDesignCardGroupBox �� BasedOn �� Property �� Value�� 5   
   %�  &	       &	      &	 Q     '�� r    7  �� 5      6+     5      $ 	Collapsed=�5   6   $ Visible=�5   6   !� 5      $
 Tile=�6,   $ 	0,0,30,30�6<   $ Absolute=�5      $ 	0,0,30,30�63   $	 Absolute=�6G   
 5       �� 5!      .+�5"      F� #���   6   $��0.1cm��6M   $ .3Q�6e   ��5#      �� 5$   "   $	 0,0&�6/   $ 30,30&�6@   5&      5'      5)      �� 5*      " ��6   8   .�5+      ��  � ��$ 8q�6,   5,      5-      .��50      � .+�51   
   ]� .+�52      ( 	 "MaterialDesignThemes.Wpf.ColorZone  $	��Top=�6'   $��16q�6<   + 	 %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�6I      Mode$ 
PrimaryMid=�6z   $��1��6�    K� ZIndex$ 1��53   '   .��54      ]� .+�55      d� -   5         d�Name$ FilterToggleButton��56   +    ' #MaterialDesignHamburgerToggleButton#��[ 55   #    d� 	IsChecked 6`   �� *FilterToggleButtonIsChecked+57      �� 58      $��Center=�6G   $��Center=�6d   $��22(�6   ��6    '  !mSwDllWPFUtils.TranslateExtension  *Warehouse area+6   59      5:      �� 5<      -   5        $��Filter��5<      $	��200��5=   !   $
��Left=�6-   $
��12 4q�6C   y�5>   !   ��  �� ElementName$ FilterToggleButton�� �� Path$ 	IsChecked� �� 	Converter# [ � 5@      $��Stretch=�6    � RowDefinitions 5A      �� 5B      $��* �6$    � ColumnDefinitions 5D      �� 5E      $��* �6'   �� 5F      $
��Auto �6'   �� 5G      $	��400 �6'   .+�5J      ]� $��0��6   $��5 10 5 0q�6,   .+�5K      �� $	��Top=�6H   $y�	Collapsed=�5M      ��5K      �� *OrientationTypes+ �� SelectedItem 5L      �� *CellOrientation+5M      �� 5O      $
��Auto=�6#   $
��Auto=�6H   .��5P      ��  �� Orientation 6%   �� *CellOrientation+.+�5Q      �� .+�5R   "   �� $ 
Horizontal=�6-   $	��Top=�6F   .+�5S   &   ��  $  MaterialDesignHeadline6TextBlock#��[ 6R   $��Center=�5T   (   $��0 0 0 0q�6C   ��5S   0   �� *CurrentAisleName+5T   C   �� 5U   &   $	�� - ��60   #��[ 6;   $��Center=�5V   (   $��8 0 0 0q�6C   �� 5W   &   #��[ 6N   $��Center=�5X   (   $��8 0 0 0q�6C   ��5W   0     *Floor+5X   C   �� 5Y   &   #��[ 6N   $��Center=�5Z   (   $��8 0 0 0q�6C   ��5Y   0   �� *CurrentLevel+5Z   C   5[   #   �� 5\   "   $
��Left=�5]   %    5\   -   �� *CellOrientation+.+�5^   &   �� ��63   �� *CellsCollectionRack1+��5_   *   �� 5`   .   .R�5a   2   ��  6=   �� *WarehouseOrientation+5b   /   ��5d   *   �� 5e   .   .R�5f   2   �� $��2q�6e   $��Bottom=�6p   $
��Left=�6�    6=   �� *CellOrientation+.+�5g   6   �� $��Center=�6X   $��Center=�6u   $��5q�6�   $��15��6�    	 SecondaryHueMidBrush#��[	 5h   4   $
��Bold"�6g   ��5g   @   �� $ X�y�5i   4   �� *Hide+# [    WhsViewer.CellControl  5j   6   $��2q�6I   5k   3   5l   /   5n   '   �� 5o   &   #��[ 6-   $��30��6U   y�5p   %   �� *WarehouseVertical+# [ ._�5q   *   �� $��Center=�64   $��Center=�6Q   $
��Bold"�5r   (   #���  6:   ��5q   l   �� *CurrentAisleName+5r   :   5s   '   �� 5t   &   #��[ 6-   $��30��6U   y�5u   %   �� *WarehouseHorizontal+# [ ._�5v   *   �� $��Center=�5w   ,   $��Center=�6I   $
��Bold"�5x   ,   #���  6>   ��5v   4   �� *CurrentAisleName+ � LayoutTransform 5y   .   �� 5z   2    �� Angle$	 270Q�6B   5|   +   5}   '   �� 5~   &   ��63   �� *CellsCollectionRack2+��5   *   �� 5�   .   .R�5�   2   ��  6=   �� *WarehouseOrientation+5�   /   ��5�   *   �� 5�   .   .R�5�   2   �� $��2q�6e   $	��Top=�5�   1   $
��Left=�6I    5�   =   �� *CellOrientation+.+�5�   6     $��2q�6I   �� 5�   6   $��Center=�6X   $��Center=�6u   $��5q�6�   $��15��6�   #��[	 5�   4   $
��Bold"�6g   ��5�   @   �� $ X�y�5�   4   �� *Hide+# [ 5�   3   5�   /   5�   '   5�   #   5�      �� 5�      $��40��6&   $��40��62   �� 5�      -   5          ��Name$
  Rack��5�   -   5�      5�      5�      �� 5�      $��1��6   $��Stretch=�6/   $��Stretch=�6K   $ 1��5�      ��5�      �� 5�      ! �� 
TargetType"! ��6*   .R�5�      ' 	 !MaterialDesignThemes.Wpf.PackIcon  "   Kind$" DragVertical=�66   $��Center=�5�   6   $��Center=�6Q   5�      5�      #	 	 MaterialDesignThemes.Wpf.Card	  5�      $��2��6&   $��Stretch=�66   $��Stretch=�6T   #   ShadowEdges$
# Left=�5�   &   $ Depth2=�5�   &   #���   5�   &   .��5�      �� $��1��6!   $��2��6.   $��2q�6>   $��0q�6I   #���  6]    5�      $ ��Name% �� Padding& �� Triggers' P� Property( P� Value) �� 
TargetName��%  '��       �� 5�      " ��6$   .�5�   "   ��  � �� 5�   &   �� 5�   *   "! ��6:   .R�5�   .   � $��Panel��63   $ .5Q�6@   .+�5�   2   �� $$ ContentSite��6C   $��Center=�5�   G   $��Center=�5�   G   $��0q�5�   G   ��5�   6   �� 5�   :   #��z��6D   $% 20 10q�6d   $
��Bold"�5�   H   #��� 	 5�   H   5�   3   5�   /   & 5�   .   P� 5�   2   ' �@��
IsSelected$
( True��6P   . �5�   6   �� $) Panel��6=    �@J�Opacity$	 1.0Q�6c   5�   3   5�   +   5�   #   5�      5�      .@�5�      �� ��6"     *	Selection+*   	Uppercase$
* True��.��5�      �  5�   "   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $��* �64   .+�5�   "   �� -   5        $
��Cell��5�   1   $��0��6=   $��6q�6J   �� 5�   "   -   5        $��Location��5�   1   $��1��6A   $��6q�6N   5�      5�      �� 5�      ��6"     *	General+$
* True�� 5�      ��%�   '��       7��7   7  �� 5�   "   " ��6(   8   .�5�   &   ��  � ��$	 0 2q�6?   �� 5�   &    � ��$ Center=�6J   �� 5�   &    � ��8  6C   �� 5�   &    � y� 6C   �� *
UserPerm+8  5�   #   5�      .��5�      �� $
��Auto=�6+   .��5�   "   �� $ Vertical=�6-   .+�5�   &   �� -   5        $  Aisle��5�   5   $��6q�6D   �� 5�   &   -   5        $  Floor��5�   5   $��6q�6D   �� 5�   &   -   5        $
  Bays��5�   5   $��6q�6C   �� 5�   &   .
 	 (MaterialDesignThemes.Wpf.ColorZoneAssist+ 
  Mode$+ Accent=�6/   $��6q�6\   y�5�   3   �� *CanRequestMap+, �� Mode$, OneWay=�# [ ��5�   *   �� 5�   .   - � 	Resources- 5�   2    $
  MaterialDesignSubtitle2TextBlock��%�   '��       7 
 �� 5�   6   " ��6<   8   .�5�   :   ��  � ��$ Center=�6^   �� 5�   :    � ��$	 5 0q�6S   5�   7   5�   3   .R�5�   2   � $	��0 2q�6�   . � DataContext. 67   �� *DataContext+/ �� RelativeSource/ �� *FindAncestor+0 �� AncestorType"0 �� 5�   6   �� 5�   :   $
��Auto �6K   �� 5�   :   $
��Auto �6K   .+�5�   6   �� $
��Bold"�6q   $��0��6�   ��6@     *
Commands+$
* True��6�   5�   3   5�   /   .��5�   *   �  5�   .   �� 5�   2   $
��Auto �6C   $	��200��6P   �� 5�   2   $
��Auto �6C   �� 5�   2   $��* �6C    5�   .   �� 5�   2   $
��Auto �6@   .+�5  .   �� -	   5        $��
RequestMap��5  5   $��0��5  9   $��2��6F   $��0q�6V   $
��5 20q�6b   $��Right=�5  9   $	��Top=�6U    # MaterialDesignRaisedLightButton#��[ 5  9   $	��150��5  9   .��5  2   �� $ 
Horizontal=�6=   .+�5  6     $" MapMarkerOutline=�6N   $��Center=�6f   $��Center=�6�   $	��2 0q�6�   $��24��5  R   $��24��6]   �� 5	  6   $��Center=�6v   $��Center=�6�    " PrimaryHueLightForegroundBrush#��[ 5
  D   ��5	  @     *Request Map+$
* True��5
  D   5  3   5  /   5  +   5  '   5  #   5     5     5     5     ' 	 !MaterialDesignThemes.Wpf.Snackbar  5     $��3��6W   1   MessageQueue1 6*   4 	 .MaterialDesignThemes.Wpf.MessageQueueExtension  -  'mSwDllWPFUtils.SnackBarCustomProperties2   SnackBarNotification2 5  *   �� *SnackBarMessage+5     5     	  5  
   3 	  UniformCornerRadius$3 25Q�6   $��50��67   $��50��6B   
�������$ Depth3=�6a   $��Center=�5     $��Center=�69   y�5     �� *	IsLoading+# [ .��5     �  % !MaterialDesignCircularProgressBar#��[ 6   4 � IsIndeterminate$
4 True��5  &   5 � Value$5 0Q�6=   $��40��6G   $��40��6R   5      5!     5"     