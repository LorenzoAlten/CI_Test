   M S B A M L   `   `   `  ����D  @OrdersMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:OrdersMgr.ViewsOrdersMgr.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5
      +    %OrdersMgr.Views.InsertOrderDetailView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:OrdersMgr.Views     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5      ^	 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 5	      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils 	 5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5         BooleanToVisibilityConverter<  6mSwDllWPFUtils.Converters.BooleanToVisibilityConverter & "InvertBooleanToVisibilityConverter�  �mSwDllWPFUtils.Converters.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False �� 
TargetType % !MaterialDesignFloatingHintTextBox �� BasedOn �� Property �� Value & "MaterialDesignFloatingHintComboBox(  "mSwDllWPFUtils.AutoCompleteTextBox%  mSwDllWPFUtils.DecimalConverter  decimalConverter	   Decimals�� 5   
   %�  &	       &	      '�� Q     7  '�� �     7  '  �    7  &	 m    �� 5      6+     5      $ 	Collapsed=�5   6   $ Visible=�5   6   �� 5      " ��6   8   .�5      ��  � ��$ 35��6.   �� 5       � ��$ 0 15 0 0q�6+   �� 5       � ��$ 15(�6-   5      �� 5       " ��6   8   .�5!      ��  � ��$
 Left=�68   �� 5"       � ��$ 35��6.   �� 5#       � ��$	 200��6-   �� 5$       � ��$ 0 15 0 0q�6+   �� 5%       � ��$ 15(�6-   5&      �� 5'      "  6   8   .�5(      ��  � ��$ 35��6.   �� 5)       � ��$ 0 15 0 0q�6+   �� 5*       � ��$ 15(�6-   5+        5-      $	 3��6=   5.      .��51      � .+�52   
   ]� y�6   �� *	IsLoading+
 �� 	Converter#
 [ .+�53      ��  �� Orientation$ 
Horizontal=�6   $��Bottom=�62   $��Center=�54       55        MaterialDesignFlatButton��%Z   '��       7  �� 56      " ��6   8   .�57      ��  � ��$ 4q�63   58      59      .+�5;      �� -   5        $
��Save��5;       $	  MaterialDesignRaisedAccentButton#��[	 6%   ��5<      '  !mSwDllWPFUtils.TranslateExtension  *Save+   	Uppercase$
 True���� 5>      -   5        $��Cancel��5>      ��5?        *Cancel+$
 True��5@      �� 5B      $	��Top=�6   $
��Auto=�60   .��5C      �� $
��12 4q�6   .+�5E      �  � ColumnDefinitions 5F      �� 5G      $
��0.5* �6/   �� 5H      $��* �6/   .+�5J        $��0��63    	 Caliburn.Micro.Message   Attach$L F[Event TextAutoCompleted] = [Action ItemCodeAutoCompleted($eventArgs)]��5K   /      FilterMinLength$ 2��5L   /      
Dictionary 6C   ��  �� Path$ Dictionary_ItemCodes���5M   /   �� *
ITM_Code+ �� NotifyOnValidationError$
 True�� �� ValidatesOnDataErrors$
 True�� �� ValidatesOnExceptions$
 True��)  #MaterialDesignThemes.Wpf.HintAssist   Hint 5N   /     *	Item code+  5O      $��1��63   $��10 0 0 0q�6C   $S M[Event TextAutoCompleted] = [Action ItemDescriptionAutoCompleted($eventArgs)]��5P   /   $ 2��5Q   /    6C   �� $! Dictionary_ItemDescriptions���5R   /   �� *
ITM_Desc+$
 True��$
 True��$
 True�� 5S   /     *Item description+5T      #	  mSwDllWPFUtils.DecimalTextBox	  5V      $��1��6*   $��1��67    6G     *Required quantity+.��6�   ��5W      (
  "mSwDllWPFUtils.CultureAwareBinding
  5X      $
 QtyR�67   $
 True��5Y   7   $
 True��6V   $
 True��6s   #
 [ 5Z   7   5[      .��5\   (   ��5]      �� 5^      " ��6$   # [B��69   5`      .��5a   )   5b      �� 5d       �� SelectedValuePath$ UDC_Code��5f       �� DisplayMemberPath$ UDC_Code��68    �� SelectedValue 5d      �� *
UDC_Code+$
 True��$
 True��$
 True����5e      �� *	UdcList+ 5g        *UDC+�� 5i       6   �� *
Priority+$
 True��$
 True��$
 True����5j      �� *PriorityList+ �� Mode$ OneTime=� 5k        *
Priority+�� 5m       �� TextWrapping$
 Wrap=�5o       �� AcceptsReturn$
 True��5p      " �� VerticalScrollBarVisibility$
 Auto=�5q       �� 	IsEnabled$
 True��5r      $��Stretch=�5s      ��5m      �� *Notes+$
 True��$
 True��$
 True�� 5n        *Notes+5s      �� 5u      $ BAY_Num��5w       5u      �� *	BAY_Num+$
 True��$
 True��$
 True����5v      �� *
BaysList+ 5x        *Bay+��5y      �� 5z      .R�5{   "   �� �� 5|   "   �6&   �� *	BAY_Num+ �� 5}   "   $�-��6&    �� 5~   "   �6&   �� *
BAY_Desc+5   #   5�      5�      5�      5�      5�      � 5�   
    %
 !MaterialDesignCircularProgressBar#��[
 6    � Value$ 0Q�5�      $��40��6    $��40��6+   
������� J� Opacity$	 0.7Q�6J   $��Center=�5�      $��Center=�61     � IsIndeterminate$
  True��5�      y�5�      �� *	IsLoading+#
 [ 5�      5�      