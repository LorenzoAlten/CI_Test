   M S B A M L   `   `   `  ����D  @WhsViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WhsViewer.ViewsWhsViewer.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5	      #    WhsViewer.Views.FrontRackView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:WhsViewer.Views     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils  5	      ^	 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 5
      .��5      � .+�5      ]� .+�5   
   ��  $   MaterialDesignHeadline6TextBlock#��[  5      $��Center=�5      $	��Top=�6/   ��5      �� *CurrentRack.RCK_Desc+5   /   � 5        � ColumnDefinitions  5      �� 5      $
��Auto �6'   �� 5      $
��Auto �6'    � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� $��0��5      $��1��6/   ��5      �� *RowNr+��5      �� 5       .R�5!      A�  A� Columns$ 1��6*   5"      ��5$      �� 5%      .R�5&      �� $��50��6-   $��20��69   $��0q�5'   %   $	��0 3q�61   .��5(   "   ��  " MaterialDesignCaptionTextBlock#��[ 5)   !   $��Black"�5*   !   $��Center=�5+   !   $��Center=�6>   ��5(   ,   �� 5+   >   5,      5-      5/      �� 51      $��1��52      $��0��6/   ��51      �� *	Columns+��53      �� 54      .R�55      A�  A� Rows$ 1��6*   56      ��58      �� 59      .R�5:      �� $��20��6-   $��50��69   $��0q�5;   %   $	��3 0q�61   .��5<   "   �� #��[ 5=   !   $��Black"�5>   !   $��Center=�5?   !   $��Center=�6>   ��5<   ,   �� *Index+5?   >   5@      5A      5C      �� 5E      $��3q�6?   $��1��5F      $��1��6/      MaterialDesignCardBackground#��[ 6<   $
��Left=�5G      $	��Top=�6:   ��5E      �� *	Columns+��5H      �� 5I      .R�5J      A� $ 1��6*   5K      ��5M      �� 5N      .R�5O      �� ��6+   �� *Rows+��5P   "   �� 5Q   &   .R�5R   *   A� $ 1��66   5S   '   ��5U   "   �� 5V   &   .R�5W   *   ��  	 Caliburn.Micro.Message   Attach$3 -[Event Click] = [Action Select($dataContext)]��5Y   1   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth1=�5Z   1   $��50��5[   1   $��50��6=   $��0q�5\   1   $��3q�6=   ��5W   1   �� *WarehouseCell.CEL_Id+y�5X   1   �� *HasLocations+   BooleanToVisibilityConverter �� 	Converter# [ ��5]   .   �� 5^   2    �� 
TargetType" ��68     MaterialDesignRaisedButton �� BasedOn# [ 6U   .�5_   6   �� 	 �� Property	 � ��
 �� Value

 �����6S   �� 5`   6   	 � ��

 �����6T   �� 5a   6   	 � ��

 �   �6S    �� Triggers 5b   6   �� 5c   :    �� Binding 6F   �� *HasUDC+ �� Value True6a   .a�5d   >   �� 	 � ��
 �#0080006[   �� 5e   >   	 � ��
 �#0080006\   �� 5f   >   	 � ��

 �����6[   5g   ;   �� 5h   :    6F   �� *WarehouseCell.IsSelected+ True6s   .a�5i   >   �� 	 � ��$
 -1q�6W   �� 5j   >   	 � ��$
 2q�6`   �� 5k   >   	 � ��  SecondaryHueMidBrush#
 [ 6\   5l   ;   5n   3   5p   +   5q   '   5s      5t      5v      5w      5y      #  MaterialDesignThemes.Wpf.Card  5{   
      UniformCornerRadius$ 25Q�6   $��50��67   $��50��6B   
�������$ Depth3=�6a   $��Center=�5}      $��Center=�69   y�5|      �� *	IsLoading+# [ .��5~      �  % !MaterialDesignCircularProgressBar#��[ 6    � IsIndeterminate$
 True��5   &    � Value$ 0Q�6=   $��40��6G   $��40��6R   5�      5�      5�      