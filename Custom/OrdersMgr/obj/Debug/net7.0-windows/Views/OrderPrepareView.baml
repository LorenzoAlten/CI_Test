   M S B A M L   `   `   `  ����D  @OrdersMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:OrdersMgr.ViewsOrdersMgr.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5	      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5
      &     OrdersMgr.Views.OrderPrepareView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:OrdersMgr.Views     5      b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 	 5      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils 	 5	      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 	 5
      ^
 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 
 5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5        CommandButtonStyle �� 
TargetType  MaterialDesignRaisedButton �� BasedOn �� Property �� Value   BooleanToVisibilityConverter<  6mSwDllWPFUtils.Converters.BooleanToVisibilityConverter & "InvertBooleanToVisibilityConverter�  �mSwDllWPFUtils.Converters.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False�� 5   
   %,  &	       7  &	 �     &	 �     �� 5      " ��6/   8   .�5      ��  � ��$ 50��6+   �� 5       � ��$	 200��6-   �� 5       � ��$ 20(�6-   �� 5       � ��$ 5q�6+   5      �� 5      6+     5      $ 	Collapsed=�5    6   $ Visible=�5!   6   5"      .��5$      � .+�5%   
   � 	 � RowDefinitions	 5&      �� 5'      $
��Auto �6    �� 5(      $��* �6    �� 5)      $��* �6    �� 5*      $
��Auto �6    .+�5-      �� -   5        $��DisplayName��5-      $	��0 5q�5.      $	��Top=�5/      $��Center=�6)      MaterialDesignTitleTextBlock#��[ 50      ��5-   +   '  !mSwDllWPFUtils.TranslateExtension  *Order preparation+50      � 52      $��1��6   $��5q�6    y�6+   �� *OperationDone+
 �� 	Converter#
 [  � ColumnDefinitions 53      �� 54      $
��Auto �6'   �� 55      $��* �6'   .+�58      ' 	 !MaterialDesignThemes.Wpf.PackIcon     Kind$ MapMarkerMultiple=�6*   $��Stretch=�6C   $��Stretch=�6_   $��5q�6}   $��24��59   *   y�66   �� *	CanLaunch+#
 [   5;      $ BlockHelper=�6*   $��Stretch=�6=   $��Stretch=�6Y   $��5q�6w   $��24��5<   *   
���  ��y�6G   �� *	CanLaunch+#
 [ �� 5>      -   5        $��RetVal��5>      $
��Wrap=�6*   $��1��6>   $��Center=�6N   5?      � 5A      $��2��6   $��5q�6    y�6+   �� *	CanLaunch+#
 [ 	 5B      �� 5C      $
��Auto �6$   �� 5D      $��* �6$   .+�5G      �� $��0��6   $��5q�6)   $
��Bold"�6�   ��64     *+)Link your order to the following customer+   Suffix$ :��6�   �� 5I      $��1��6    �� 
IsReadOnly$
 True��5J   7    �� SelectedValuePath$ BPA_Code��5M       
 Caliburn.Micro.Message   Attach$: 4[Event PreviewMouseDown] = [Action SelectCustomer()]��5N       �� Text 5J      �� *CustomerName+��5K      �� *CustomersList+ �� SelectedValue 5L      �� *CustomerCode+) 	 #MaterialDesignThemes.Wpf.HintAssist   Hint 5M   6     *Click to choose a customer+��5O      �� 5P      .R�5Q   "   $� 5R      5T      5U      � 5W      $��3��6    5X      �� 5Y      $��* �6'   �� 5Z      $��* �6'   .+�5]      �� -   5        $��Launch��5]      $��Center=�5^      $
��0 10q�66    $  MaterialDesignFlatAccentBgButton#��[ 5`      {�5]   '   �� *	CanLaunch+��5_        *
Continue+   	Uppercase$
 True��5`      �� 5b      -   5        $��TryClose��5b      $��1��6)   $��Center=�5c      $
��0 10q�66     	 MaterialDesignOutlinedButton#��[	 5e      ��5d        *Cancel+$
 True��5e      5f      5h      � 5j   
    %
 !MaterialDesignCircularProgressBar#��[
 6    � Value$ 0Q�5k      $��40��6    $��40��6+   
������� J� Opacity$	 0.7Q�6J   $��Center=�5l      $��Center=�61    � IsIndeterminate$
 True��5m      y�5n      �� *	IsLoading+#
 [ 5o      5p      