   M S B A M L   `   `   `  ����D  @OrdersMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:OrdersMgr.ViewsOrdersMgr.Views  5      %clr-namespace:OrdersMgr	OrdersMgr  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5	      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5
      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5           OrdersMgr.Views.OrdersView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:OrdersMgr.Views     5      (rootclr-namespace:OrdersMgr     5      b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 	 5	      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils 	 5
      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 	 5      ^
 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 
 5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5      %  mSwDllWPFUtils.DecimalConverter  DecimalConverter   Decimals  IntConverter$  mSwAgilogDll.QuantityConverter  QuantityConverter   BooleanToVisibilityConverter<  6mSwDllWPFUtils.Converters.BooleanToVisibilityConverter & "InvertBooleanToVisibilityConverter�  �mSwDllWPFUtils.Converters.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False'  !mSwAgilogDll.MissionTypeConverter  MissionTypeConverter(  "mSwAgilogDll.MissionPhaseConverter 	 MissionPhaseConverter*  $mSwAgilogDll.OrderDetStatusConverter 
 OrderDetStatusConverter/	  )mSwAgilogDll.OrderDetItemBookingConverter   OrderDetItemBookingConverter/
   )OrdersMgr.OrderMissionsCompletedConverter # OrderMissionsCompletedConverter3   -OrdersMgr.OrderLineMissionsCompletedConverter ' #OrderLineMissionsCompletedConverter �� 
TargetType # MaterialDesignRaisedLightButton �� BasedOn �� Property	 �� Value' 	 !MaterialDesignThemes.Wpf.PackIcon)  #System.Windows.Controls.DataGridRow
 �� Triggers �� Binding �� Value*  $System.Windows.Controls.DataGridCell ^� 
Conditions �� Binding �� RelativeSource �� Mode �� Path �� Value  MaterialDesignCardGroupBox  
ToolButton & "MaterialDesignToolForegroundButton�� 5   
   %�  &	       &	      &	 6     &	 I     &	 \     &	 �     &		 �     &	
 �     &	 �     &	 �     &	 �     '��     7  '      '  �    7 '  �    7 '�� [    7  &	 �    7    5      $ 3��6=     5      $ 0��69     5      6'   �� 5      6+     5      $ 	Collapsed=�5   6   $ Visible=�5   6     5       6*     5!      6+     5"      6-   	  5#      62   
  5$      63     5%      67   �� 5&      " ��6   8   .�5'      ��  � ��$	 5q�6+   �� 5(       � ��$	 0q�6,   �� 5)       � |�$	 False��6.   �� 5*       � ��$	 60��6+   �� 5+       � ��$	 75��6*   �� 5,       � ��$	 Stretch=�6?   5-      �� 5.      "  6   .�52      ��  � ��$	 Center=�68   �� 53       � ��$	 Center=�66   54      �� 55      "  6   8   
 56      �� 57       6"   �� *ORD_MOP_Code+ SCH6C   .a�58      ��  � ��	 �#00580067   �� 59       � ��
	 �����67   �� 5:       � ��$
	 Bold"�67   5;      �� 5<       6"   �� *ODT_ODS_Code+ E6C   .a�5=      ��  � ��	 �#00580067   �� 5>       � ��
	 �����67   �� 5?       � ��$
	 Bold"�67   5@      �� 5A       6"   �� *ORD_MOP_Code+ C6C   .a�5B      ��  � ��	 �#00539167   �� 5C       � ��
	 �����67   5D      �� 5E       6"   �� *ODT_ODS_Code+ C6C   .a�5F      ��  � ��	 �#00539167   �� 5G       � ��
	 �����67   5H      �� 5I       6"   �� *	ODT_Error+ True6@   .a�5J      ��  � ��
	 �  ��67   �� 5K       � ��
	 �����67   5L      5N      �� 5O      "  6   8   .�5P      ��  � ��$	 0q�64   
 5Q      ^� 5R       5S      �� 5T       6(   ��  �� $
 Self=�$ 
IsSelected� True6w   �� 5U       6(   �� *ORD_MOP_Code+ SCH6I   .0�5W      ��  � ��	 �#00800067   �� 5X       � ��
	 �����67   �� 5Y       � ��$
	 Bold"�67   5Z      ^� 5[       5\      �� 5]       6(   ��  �� $
 Self=�$ 
IsSelected� True6w   �� 5^       6(   �� *ODT_ODS_Code+ E6I   .0�5`      ��  � ��	 �#00800067   �� 5a       � ��
	 �����67   �� 5b       � ��$
	 Bold"�67   5c      ^� 5d       5e      �� 5f       6(   ��  �� $
 Self=�$ 
IsSelected� True6w   �� 5g       6(   �� *ORD_MOP_Code+ C6I   .0�5i      ��  � ��	 �#0072C667   �� 5j       � ��
	 �����67   5k      ^� 5l       5m      �� 5n       6(   ��  �� $
 Self=�$ 
IsSelected� True6w   �� 5o       6(   �� *ODT_ODS_Code+ C6I   .0�5q      ��  � ��	 �#0072C667   �� 5r       � ��
	 �����67   5s      ^� 5t       5u      �� 5v       6(   ��  �� $
 Self=�$ 
IsSelected� True6w   �� 5w       6(   �� *	ODT_Error+ True6F   .0�5y      ��  � ��
	 �  ��67   5z      5|      �� 5}      " ��6   8   .�5~      ��  � ��$	 	12 6 12 4q�6+   �� 5       � ��$	 8q�6,   �� 5�       � ��$	 Stretch=�66   5�      �� 5�      " ��6'   8   .�5�      ��  � ��$	 5q�6+   �� 5�       � ��$	 0q�6,   �� 5�       � |�$	 False��6.   �� 5�       � ��$	 50��6+   �� 5�       � ��$	 50��6*   �� 5�       � ��$	 Stretch=�6?   5�      5�      .��5�      � .+�5�   
   ]� .+�5�      ( 	 "MaterialDesignThemes.Wpf.ColorZone  $	��Top=�6'   $��16q�6<   + 	 %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�6I      Mode$ 
PrimaryMid=�6z    K� ZIndex$ 1��5�   '   .��5�      ]� .+�5�      d� -   5         d�Name$ FilterToggleButton��5�   +    ' #MaterialDesignHamburgerToggleButton#��[ 5�   #    d� 	IsChecked 6`   �� *FilterToggleButtonIsChecked+5�      �� 5�      -   5        $��DisplayName��5�       $��Center=�63   $��Center=�6P   $��22(�6k   5�      5�      �� 5�      -   5        $��FilterOrder��5�      $	��300��5�   !   $
��Left=�6-   $
��12 4q�6C   y�5�   !   ��  �� ElementName$ FilterToggleButton��$ 	IsChecked� �� 	Converter# [ � 5�      $��Stretch=�6    � RowDefinitions 5�      �� 5�      $��* �6$   �� 5�      $
��Auto �6$   �� 5�      $��* �6$   .+�5�      �� $��0��6   . 	 (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�5�      ��5�      '  !mSwDllWPFUtils.TranslateExtension  *Orders+   	Uppercase$
 True�� 5�      ��%g  '        '�� �     7  �� 5�      "  6    .�5�      ��  � ��$	 5q�67   �� 5�       � ��$	 20��67   �� 5�       � ��$	 20��66   5�      �� 5�      " ��6    8   .�5�      ��  � ��$	 0q�68   �� 5�       � ��$	 -8q�67   �� 5�       � |�$	 False��6:   �� 5�       � ��$	 Stretch=�6K   5�      5�      ��5�      �� 5�      .R�5�      ]� $	��Top=�6(   $��False��6=   .+�5�   "   A�  A� Rows$ 1��6.    A� Columns$ 1��67   $
��Left=�6C   .+�5�   &   �� $��8 0 0 0q�60   $��Center=�6A   $
��Bold"�6\   ��5�   ,   �� 5�   #   A� 5�   "   $ 1��6.   $ 1��67   $��Right=�6C   .+�5�   &   �� $��StopAll��6-    � ToolTip 5�   -     *
Stop All+.��5�   *         Kind$  
StopCircle=�6B   5�   '   5�   #   5�      5�      .��5�      ]� $��-8q�6    .+�5�        $ PrimaryLight=�63   $��Bottom=�6G    5�      ��%�   '��       7  �� 5�   "   " ��6(   8   .�5�   &   ��  � ��$	 0q�6@   �� 5�   &    � |�$	 False��6B   �� 5�   &    � ��$	 Stretch=�6S   5�   #   5�      .��5�      ]� $	��Top=�6(   $��False��6=   .+�5�   "   A� $ 1��6.   $ 5��67   $
��Left=�6C   .+�5�   &   �� -   5        $��Start��5�   -   $ Lancio COMPLETO��5�   -   {�5�   :   �� *
CanStart+.��5�   *     $  Truck=�6B   5�   '   �� 5�   &   -   5        $��	StartPick��5�   -   $ Lancio solo PICKING��5�   -   {�5�   >   �� *
CanStart+.��5�   *     $  	HandTruck=�6B   5�   '   �� 5�   &   -   5        $��Prepare��5�   -   $ Preparazione ordine��5�   -   {�5�   <   �� *
CanPrepare+.��5�   *     $  
ClockStart=�6B   5�   '   �� 5�   &   -   5        $
��Stop��5�   -   $
 Stop��5�   -   {�5�   9   �� *	CanStop+.��5�   *     $
  Stop=�6B   5�   '   �� 5�   &   -   5        $��Export��5�   -   $y�	Collapsed=�6;    5�   -     *Export+.��5�   *     $  Export=�6B   5�   '   �� 5�   &   -	   5        $��ViewUDC��5�   -   $ Vedi UDC coinvolte��5�   -   .��5�   *     $  FormatListBulleted=�6B   5�   '   5�   #   A� 5�   "   $ 1��6.   $ 4��67   $��Right=�6C   .+�5�   &   �� -
   5        $	��New��5�   -   $y�Hidden=�68    5�   -     *	New order+.��5�   *     $
  Plus=�6B   5�   '   �� 5�   &   -   5        $
��Edit��5�   -   $y�Hidden=�69    5�   -     *
Edit order+.��5�   *     $  Pencil=�6B   5�   '   �� 5�   &   -   5        $��Delete��5�   -   $ Cancella ordine��5�   -   .��5�   *     $  Delete=�6B   5�   '   �� 5�   &   -   5        $��
RefreshAll��5�   -   $ Refresh��5�   -   .��5�   *     $  Refresh=�6B   5�   '   5�   #   5      5     &   System.Windows.Controls.DataGrid  5     -   5        $��
OrdersList��5  #   $��Stretch=�65   !   SelectionUnit$! FullRow=�6Q   "   SelectionMode$" Single=�6i   #   AutoGenerateColumns$# False��5  '   $   CanUserAddRows$$ False��6C   %   CanUserDeleteRows$% False��6Z   - 	 'MaterialDesignThemes.Wpf.DataGridAssist&   CellPadding$& 12 8 8 8q�5  '   '   ColumnHeaderPadding$' 4q�5  '   "(   VerticalScrollBarVisibility$
( Auto=�5  '   $)   HorizontalScrollBarVisibility$
) Auto=�6J   
������ * �� SelectedItem* 5  '   �� *SelectedOrder++   Columns+ 5     4  .System.Windows.Controls.DataGridTemplateColumn  5  "   ,  &System.Windows.Controls.DataGridColumn,   
IsReadOnly$, False��69   -   Width5  /System.Windows.Controls.DataGridLengthConverter$
- Auto 6L   .   MinWidth$. 50Q�6Y   /   Header/ 5  &   �� 5  *   .��5  .   �� $��	2 -10 0 0q�67   $|�False��5  7      MaterialDesignAccentCheckBox#��[ 6I    5  J   �� *DataContext.SelectAll+ �� $ FindAncestor=�0 �� AncestorType0 DataGridz�5  2   �� 5  6   1 �� ScaleX$	1 1.5Q�6E   2 �� ScaleY$	2 1.5Q�6R   5  /   5  +   3   CellTemplate3 5  &   �� 5  *   .R�5  .   �� -   5        $|�False��5  7   $��	0 -12 0 0q�5  7   #��[ 6J    5  I   �� *
IsSelected+z�5  2   �� 5  6   $	1 1.5Q�6E   $	2 1.5Q�6R   5  /   5  +   5!  #   0  *System.Windows.Controls.DataGridTextColumn  5"  "   $
, True��5$  9   / 5"  5     *Owner+$
 True��1  +System.Windows.Controls.DataGridBoundColumn4   Binding4 5#  9   �� *	ORD_Owner+5$  9     5%  "   $
, True��5'  9   / 5%  5     *Order+$
 True��4 5&  9   �� *ORD_OrderCode+5'  9     5(  "   $
, True��5*  9   / 5(  5     *Creation date+$
 True��4 5)  9   �� *ORD_DocDate+5 �� ConverterCulture$5 IT��5*  9     5+  "   $
, True��5-  9   / 5+  5     *Description+$
 True��4 5,  9   �� *ORD_Description+5-  9     5.  "   $
, True��50  9   / 5.  5     *Customer Code+$
 True��4 5/  9   �� *BusinessPartner.BPA_Code+50  9     51  "   $
, True��53  9   / 51  5     *
Customer+$
 True��4 52  9   �� *BusinessPartner.BPA_Desc+53  9     54  "   $
, True��56  9   / 54  5     *Destination+$
 True��4 55  9   �� *(&BusinessPartnersDestination.BPD_Street+56  9     57  "   $
, True��59  9   / 57  5     *Delivery Date+$
 True��4 58  9   �� *ORD_DueDate+$5 IT��59  9     5:  "   $
, True��5<  9   / 5:  5     *	Operation+$
 True��4 5;  9   �� *ORD_MIT_Code+# [ 5<  9     5=  "   $
, True��5?  9   / 5=  5     *Phase+$
 True��4 5>  9   �� *ORD_MOP_Code+# [	 5?  9     5@  "   $
, True��6j   / 65     *
Missions+$
 True��4 5A  &   `� 5B  *   6 `� 	Converter#6 [ 67   1�5C  .   �� 5D  2   $ OrderPhaseType�6:   �� 5E  2   $ ORD_CompletedMissions�6:   �� 5F  2   $ ORD_TotalMissions�6:   5H  +   5J  #     5K  "   7   
Visibility$7 	Collapsed=�6j   $
, True��5M  9   / 5K  5     *
Due date+$
 True��4 5L  9   �� *ORD_DueDate+$5 IT��5M  9     5N  "   $7 	Collapsed=�6j   $
, True��5P  9   / 5N  5     *
Priority+$
 True��4 5O  9   �� *ORD_Priority+5P  9   5R     5S     5T     �� 5V     $��1��6   $��Stretch=�6,   $��Stretch=�6H   $ 1��5W     ��5X     �� 5Y     8 �� 
TargetType"8 ��6*   .R�5Z       $  DragHorizontal=�66   5[     5]     �� 5_     $��2��6   $ Accent=�5a     ��5`     �� *SelectedOrder.ORD_OrderCode+ 5b     ��%�   '        �� 5c     "  6    .�5d     ��  � ��$	 5q�67   �� 5e      � ��$	 20��67   �� 5f      � ��$	 20��66   5g     5h     ��5i     �� 5j     .R�5k     �� 9 �� Orientation$9 
Horizontal=�6)   .+�5l  "   �� $	��8 0q�6,   $��Center=�69   $
��Bold"�6T   ��5m  ,     *Order details+$
 True���� 5n  "   $��Center=�6,   $
��Bold"�6G   ��5o  ,   �� 5p     5q     .��5s     ]� $��-8q�6    .+�5t       $ PrimaryLight=�63   $��Bottom=�6G    5u     ��%�   '��       7  �� 5v  "   " ��6(   8   .�5w  &   ��  � ��$	 0q�6@   �� 5x  &    � |�$	 False��6B   �� 5y  &    � ��$	 Stretch=�6S   5z  #   5{     .��5|     �� -   5        $��RefreshRows��5|  %   $��Right=�5}  %    5~  %     *	Refresh+.��5  "     $  Refresh=�6:   5�     5�       5�     -   5        $��OrdersLinesList��5�  #   $��Stretch=�6:   $# False��5�  #   $$ False��6?   $% False��6V   $& 12 8 8 8q�5�  #   $' 4q�5�  #   $
( Auto=�5�  #   $
) Auto=�6F   
������ + 5�       5�  "   $
, True��5�  9   / 5�  5     *Owner+$
 True��4 5�  9   �� *	ODT_Owner+5�  9     5�  "   $
, True��5�  9   / 5�  5     *Order+$
 True��4 5�  9   �� *ODT_ORD_OrderCode+5�  9     5�  "   $
, True��5�  -   / 5�  5     *Line+$
 True��4 5�  -   �� *
ODT_Line+5�  -     5�  "   $
, True��5�  5   / 5�  5     *Item+$
 True��4 5�  5   �� *ODT_ITM_Code+5�  5     5�  "   $
, True��5�  5   / 5�  5     *Description+$
 True��4 5�  5   �� *Order.ORD_Description+5�  5     5�  "   $
, True��5�  5   / 5�  5     *Delivery Date+$
 True��4 5�  5   �� *Order.ORD_DueDate+$5 IT��5�  5     5�  "   $
, True��5�  -   $- * 6?   $7 	Collapsed=�6I   / 5�  5     *Booking Code+$
 True��4 5�  -   �� *ODT_BookingCode+5�  I     5�  "   $
, True��5�  -   $- * 6?   $7 	Collapsed=�6I   / 5�  5     *Batch+$
 True��4 5�  -   �� *	ODT_Batch+5�  I     5�  "   $
, True��5�  -   $- * 6?   $7 	Collapsed=�6I   / 5�  5     *UDC+$
 True��4 5�  -   �� *ODT_UDC_Code+5�  I     5�  "   $
, True��5�  -   $7 	Collapsed=�6?   / 5�  5     *Compartment+$
 True��4 5�  -   �� *ODT_UCM_Index+5�  ?     5�  "   $
, True��5�  -   / 5�  5     *Required quantity+$
 True��4 5�  -   �� *
ODT_QtyR+: �� StringFormat$: F0��$5 IT��5�  -     5�  "   $
, True��5�  -   / 5�  5     *Committed quantity+$
 True��4 5�  -   �� *
ODT_QtyI+$: F0��$5 IT��5�  -     5�  "   $
, True��5�  -   / 5�  5     *Evaded quantity+$
 True��4 5�  -   �� *
ODT_QtyE+$: F0��$5 IT��5�  -     5�  "   $
, True��5�  1   / 5�  5     *Status+$
 True��4 5�  1   �� *ODT_ODS_Code+# [
 5�  1     5�  "   $
, True��5�  9   / 5�  9     *
Complete+$
 True��3 5�  &   �� 5�  *   .R�5�  .     $  Check=�6F   $��0q�5�  F   $��Center=�6Q   y�5�  F   �� *
ODT_Closed+# [ 5�  +   5�  #     5�  "   $
, True��5�  -   $7 	Collapsed=�6?   / 5�  5     *
Priority+$
 True��4 5�  -   �� *ODT_Priority+5�  ?     5�  "   $
, True��5�  -   $7 	Collapsed=�6?   / 5�  5     *Bay+$
 True��4 5�  -   �� *ODT_BAY_Num+5�  ?     5�  "   $
, True��6j   $7 	Collapsed=�6|   / 65     *
Missions+$
 True��4 5�  &   `� 5�  *   #6 [ 67   1�5�  .   �� 5�  2   $ State�6:   �� 5�  2   $ ODT_CompletedMissions�6:   �� 5�  2   $ ODT_TotalMissions�6:   5�  +   5�  #   4  .System.Windows.Controls.DataGridCheckBoxColumn  5�  "   $
, True��5�  1   $7 	Collapsed=�6C   / 5�  9     *
In error+$
 True��4 5�  1   �� *	ODT_Error+5�  C     5�  "   $
, True��5�  9   / 5�  9     *Errors+$
 True��3 5�  &   �� 5�  *   .R�5�  .   �� #��[ 65   $	��-10q�6Y   $��0q�6f   $��30��6r   $��30��6}   ��5�  5   ��  �� "0  $ DataContext.ErrorCommand���5�  5   �� y�5�  5   �� *	ODT_Error+# [ .��5�  2     $  Launch=�6J   5�  /   5�  +   5�  #   5�     5�     5�     ' 	 !MaterialDesignThemes.Wpf.Snackbar  5�     $��3��6W   ;   MessageQueue; 6*   4 	 .MaterialDesignThemes.Wpf.MessageQueueExtension  -  'mSwDllWPFUtils.SnackBarCustomProperties<   SnackBarNotification< 5�  *   �� *SnackBarMessage+5�     5�     � 5�  
    % !MaterialDesignCircularProgressBar#��[ 6   = � Value$= 0Q�5�     $��40��6    $��40��6+   
�������> J� Opacity$	> 0.7Q�6J   $��Center=�5�     $��Center=�61   ? � IsIndeterminate$
? True��5�     @ J� RenderTransformOrigin$@ 1.575,0&�5�  p   y�6   �� *	IsLoading+# [ 6p   5�     5�     