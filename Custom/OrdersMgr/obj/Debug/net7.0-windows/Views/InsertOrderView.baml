   M S B A M L   `   `   `  ����D  @OrdersMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:OrdersMgr.ViewsOrdersMgr.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5
      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5      %    OrdersMgr.Views.InsertOrderView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:OrdersMgr.Views     5      b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 	 5      ^
 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 
 5	      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils 
 5
      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 
 5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5      %  mSwDllWPFUtils.DecimalConverter  DecimalConverter   Decimals(  "mSwDllWPFUtils.InvertBoolConverter  InvertBoolConverter   BooleanToVisibilityConverter<  6mSwDllWPFUtils.Converters.BooleanToVisibilityConverter & "InvertBooleanToVisibilityConverter�  �mSwDllWPFUtils.Converters.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False*  $mSwAgilogDll.OrderDetStatusConverter  OrderDetStatusConverter �� 
TargetType $  MaterialDesignSubtitle1TextBlock �� BasedOn �� Property	 �� Value %	 !MaterialDesignFloatingHintTextBox &
 "MaterialDesignFloatingHintComboBox(  "System.Windows.Controls.DatePicker ( $MaterialDesignFloatingHintDatePicker(  "mSwDllWPFUtils.AutoCompleteTextBox'	 	 !MaterialDesignThemes.Wpf.PackIcon�� 5   
   %�  &	       &	      &	 .     &	 A     &	      '�� �     7  '��     7 	 '�� �    7 
 '  �    7  '  �    7 	 '	  W      5      $ 3��6=     5      6'   �� 5      6+     5      $ 	Collapsed=�5   6   $ Visible=�5   6     5      6-   �� 5      " ��6   8   .�5       ��  � ��$	 0 20 0 0q�6+   �� 5!       � ��$
	 Bold"�6/   5"      �� 5#      " ��6   8   .�5$      ��  � ��$	 35��6.   �� 5%       � ��$		 0 5q�6+   �� 5&       � ��$	 15(�6-   5'      �� 5(      " ��6   8   .�5)      ��  � ��$
	 Left=�68   �� 5*       � ��$	 35��6.   �� 5+       � ��$		 200��6-   �� 5,       � ��$	 0 15 0 0q�6+   �� 5-       � ��$	 15(�6-   5.      �� 5/      "  6   8   .�50      ��  � ��$	 Center=�66   �� 51       � ��$
	 Left=�68   �� 52       � ��$	 Center=�6=   �� 53       � ��$	 35��6.   �� 54       � ��$		 200��6-   �� 55       � ��$	 0 15 0 5q�6+   �� 56       � ��$	 15(�6-   57      �� 58      "  6   8   .�59      ��  � ��$	 35��6.   �� 5:       � ��$	 0 5 0 0q�6+   �� 5;       � ��$	 15(�6-   5<      �� 5=      " 	 6   .�5>      ��  � ��$	 15q�6+   �� 5?       � ��$	 24��6+   �� 5@       � ��$	 24��6*   �� 5A       � ��$	 Center=�68   �� 5B       � ��$	 Center=�66   5C      5D      .��5G      � .+�5H   
   ]� y�6   �� *	IsLoading+
 �� 	Converter#
 [ .+�5I      ��  �� Orientation$ 
Horizontal=�6   $��Bottom=�62   $��Center=�5J       5K        MaterialDesignFlatButton��%~   '��       7  �� 5L      " ��6   8   .�5M      ��  � ��$	 4q�63   �� 5N       � ��$	 40��63   5O      5P      .+�5R      �� -   5        $
��Save��5R       $  MaterialDesignRaisedAccentButton#��[ 6%   ��5S      '
  !mSwDllWPFUtils.TranslateExtension
  *Save+ 
  	Uppercase$
 True���� 5U      -   5        $��Cancel��5U      ��5V      
  *Cancel+$
 True��5W      �� 5Y      $	��Top=�6   $
��Auto=�60   .��5Z      �� $
��12 4q�6   .+�5[      �� ��6   �� *	OrderCode+ �� NotifyOnValidationError$
 True�� �� ValidatesOnDataErrors$
 True�� �� ValidatesOnExceptions$
 True��{�5\      ��  �� Path$ Order.LoadedFromDb�#
 [ ) 	 #MaterialDesignThemes.Wpf.HintAssist   Hint 5]      
  *Code+�� 5_      $��Stretch=�5a      ��5_      �� *Description+$
 True��$
 True��$
 True�� 5`      
  *Description+5a      � 5c       � RowDefinitions 5d      �� 5e      $
��Auto �6,   �� 5f      $
��Auto �6,    � ColumnDefinitions 5h      �� 5i      $
��0.5* �6/   �� 5j      $��* �6/   .+�5m      �� $��0��5n   $   $��0��64   $��2��6A   ��5m   $   
  *Business partner+5n   A     5p      $��0��63   $��1��6C    
 Caliburn.Micro.Message   Attach$H B[Event TextAutoCompleted] = [Action CodeAutoCompleted($eventArgs)]��5q   /      FilterMinLength$ 2��5r   /      
Dictionary 6C   �� $&  Dictionary_BusinessPartnersCodes���5s   /   �� *
BPA_Code+$
 True��$
 True��$
 True�� 5t   /   
  *Code+  5u      $��1��63   $��1��6C   $��10 5 0 0q�6P   $O I[Event TextAutoCompleted] = [Action DescriptionAutoCompleted($eventArgs)]��5v   /   $ 2��5w   /    6C   �� $- 'Dictionary_BusinessPartnersDescriptions���5x   /   �� *
BPA_Desc+$
 True��$
 True��$
 True�� 5y   /   
  *Description+5z      �� 5|       �� SelectedValuePath$ MIT_Code��5~       �� DisplayMemberPath$ MIT_Desc��68    �� SelectedValue 5|      �� *
MIT_Code+$
 True��$
 True��$
 True����5}      �� *MisCfgTypeList+ 5      
  *	Operation+  5�         SelectedDate 6!   �� *	DueDate+$
 True��$
 True��$
 True�� 5�   !   
  *Fulfillment date+�� 5�       6   �� *
Priority+$
 True��$
 True��$
 True����5�      �� *PriorityList+ �� Mode$ OneTime=� 5�      
  *
Priority+��5�      �� 5�      .R�5�   "   �� $��0q�5�   ,   $��Normal"�67   ��5�   ,   �� 5�   7   5�      5�      5�      5�      ( 	 "MaterialDesignThemes.Wpf.ColorZone  5�         Mode$ PrimaryLight=�6'   $��Stretch=�6;   $��Right=�6W   .��5�      ��  5�       & "MaterialDesignToolForegroundButton��%  '��       7  �� 5�      " ��6    8   .�5�      ��  � ��$	 5q�67   �� 5�       � ��$	 0q�68   �� 5�       � |�$	 False��6:   �� 5�       � ��$	 50��67   �� 5�       � ��$	 50��66   �� 5�       � ��$	 Stretch=�6K   5�      5�      .+�5�      �� -   5        $��NewRow��5�      $��Right=�6+   .��5�      	   	  Kind$
 Plus=�62   5�      �� 5�      -   5        $��EditRow��5�      .��5�      	  $ Pencil=�62   5�      �� 5�      -   5        $��	DeleteRow��5�      .��5�      	  $ Delete=�62   5�      5�      5�      �� 5�      $��3��6   $��8q�6$   $��4 4 4 0q�60   $��Stretch=�6A     MaterialDesignGroupBox#���  5�      . 	 (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�5�      ��5�      
  *Order details+$
 True����5�      �� 5�      .R�5�      �� $��8 0 0 0q�6$   $��Center=�65   $
��Bold"�6P   ��5�   $   �� 5�      .��5�      �� $
��Auto=�6   $��1��6B   .��5�      &   System.Windows.Controls.DataGrid  -   5        $��OrdersDetailList��5�      $��Stretch=�67      AutoGenerateColumns$ False��5�          CanUserAddRows$  False��6;   !   CanUserDeleteRows$! False��6R   - 	 'MaterialDesignThemes.Wpf.DataGridAssist"   CellPadding$" 12 8 8 8q�5�      #   ColumnHeaderPadding$# 4q�5�      $ �� SelectedItem$ 5�      �� *
Selected+%   Columns% 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn&   
IsReadOnly$
& True��5�   -   '   Header' 5�   1   
  *Line+$
 True��1  +System.Windows.Controls.DataGridBoundColumn(   Binding( 5�   -   �� *
ODT_Line+5�   -     5�      $
& True��5�   -   )   Width5  /System.Windows.Controls.DataGridLengthConverter$) * 6?   ' 5�   1   
  *Item+$
 True��( 5�   -   �� *Item.ITM_Desc+5�   ?     5�      $
& True��5�   -   $) * 6?   ' 5�   1   
  *UDC+$
 True��( 5�   -   �� *ODT_UDC_Code+5�   ?     5�      $
& True��5�   -   ' 5�   1   
  *Compartment+$
 True��( 5�   -   �� *ODT_UCM_Index+5�   -     5�      $
& True��5�   -   ' 5�   1   
  *Required quantity+$
 True��( 5�   -   �� *
ODT_QtyR+#
 [ 5�   -     5�      $
& True��5�   -   ' 5�   1   
  *Committed quantity+$
 True��( 5�   -   �� *
ODT_QtyI+#
 [ 5�   -     5�      $
& True��5�   -   ' 5�   1   
  *Evaded quantity+$
 True��( 5�   -   �� *
ODT_QtyE+#
 [ 5�   -     5�      $
& True��5�   1   ' 5�   1   
  *Status+$
 True��( 5�   1   �� *ODT_ODS_Code+#
 [ 5�   1   4  .System.Windows.Controls.DataGridCheckBoxColumn  5�      $
& True��5�   1   ' 5�   5   
  *Closed+$
 True��( 5�   1   �� *
ODT_Closed+5�   1     5�      $
& True��5�   1   ' 5�   5   
  *
In error+$
 True��( 5�   1   �� *	ODT_Error+5�   1   5�      5�      5�      5�      � 5�   
    % !MaterialDesignCircularProgressBar#��[ 6   * � Value$* 0Q�5�      $��40��6    $��40��6+   
�������+ J� Opacity$	+ 0.7Q�6J   $��Center=�5�      $��Center=�61   , � IsIndeterminate$
, True��5�      y�5�      �� *	IsLoading+#
 [ 5�      5�      