   M S B A M L   `   `   `  ����D  @WhsViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:mSwAgilogDll.ViewsmSwAgilogDll.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      G4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5
      &     WhsViewer.Views.RunImmediateView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:mSwAgilogDll.Views     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      CWPFUtils4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5      ^	 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 5	      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 	 5
         MaterialDesignBody#���   5      $��Regular"�5      $��16(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5      $��10q�5       � 	Resources 5      (  "mSwDllWPFUtils.InvertBoolConverter  invertBoolConverter%  mSwDllWPFUtils.DecimalConverter  decimalConverter   Decimals �� 
TargetType % !MaterialDesignFloatingHintTextBox �� BasedOn �� Property �� Value(  "mSwDllWPFUtils.AutoCompleteTextBox��%�  &	       &	      '�� .     7  '  �     7    5   
   6'     5   
   $ 0��6=   �� 5   
   " ��6   8   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 35��6*   �� 5       � ��$	 4 8q�6'   5      �� 5    
   "  6   8   .�5!      ��  � ��$ Center=�62   �� 5"       � ��$ 35��6*   �� 5#       � ��$	 4 8q�6'   5%      5&      .��5(      � -   5        $��DataIsVisible��5(       � ColumnDefinitions 5)   
   �� 5*      $��* �6   �� 5+      $��* �6   	 � RowDefinitions	 5-   
   �� 5.      $
��Auto �6   �� 5/      $��10 �6   �� 50      $
��Auto �6   �� 51      $
��Auto �6   �� 52      $��* �6   �� 53      $
��Auto �6   �� 54      $
��Auto �6   .+�57   
   �� -   5        $��txtUDC��57      $��0��6    $��0��6-   $��1��6=   )  #MaterialDesignThemes.Wpf.HintAssist
   FloatingOffset$
 0, -20&�58      $
��Bold"�6D      
IsFloating$
 True��59       �� 
IsReadOnly 6>   �� *UdcReadonly+   Hint 5:      '  !mSwDllWPFUtils.TranslateExtension  *	Load unit+   	Uppercase$
 True����5;      ��  �� Path$ UDC_Code� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�� 5E   
   $��0��6   $��1��6    5F      �� 5G      $
��Auto �6#   $	��150��60   �� 5H      $��* �6#   5J      �� 5v   
   $��2��67   $��2��6D     MaterialDesignGroupBox#���  5w      .  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�5x      +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�5y      ��5v        *	Content+��5z      �� 5{      .R�5|      �� $��4 0 0 0q�6    $��Center=�5}       ��5~       �� 5      .��5�      &	   System.Windows.Controls.DataGrid	  $��Stretch=�6   $��Stretch=�65    	  
IsReadOnly$
 True��6Q    	  AutoGenerateColumns$ False��6c   
������ " 	  VerticalScrollBarVisibility$
 Auto=�5�       	  EnableRowVirtualization$
 True��5�      $	��100��66   -
  'MaterialDesignThemes.Wpf.DataGridAssist 
  CellPadding$ 4 2 2 2q�5�       
  ColumnHeaderPadding$ 4 2 2 2q�5�      ��5�      �� *Udc.Compartments+ 	  Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Header$  ��6)      
IsReadOnly$
 True��63      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 5�   G      MinWidth$ 40Q�6T   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6)   �� *	UCM_Index+6T     5�      $
 True��6Y   $
 Auto 5�   G    5�   )     *Owner+$
 True�� 5�   )   �� *	UCM_Owner+6G     5�      $
 True��6X   $
 Auto 5�   K    5�   )     *Item+$
 True�� 5�   )   �� *Item.ITM_Desc+6K     5�      $
 True��6\   $
 Auto 5�   o    5�   )     *
Quantity+$
 True�� 5�   )   �� *	UCM_Stock+  �� StringFormat$  F0��! �� ConverterCulture$! IT��6o     5�      $
 True��6Y   $
 Auto 5�   G    5�   )     *Batch+$
 True�� 5�   )   �� *	UCM_Batch+6G     5�      $
 True��6Z   $
 Auto 5�   K    5�   )     *Serial+$
 True�� 5�   )   �� *UCM_SerialNum+6K     5�      $
 True��6e   $
 Auto 5�   s    5�   )     *Expiration date+$
 True�� 5�   )   �� *UCM_ExpiringDate+$! IT��$  d��6s   5�      5�      �� 5�   
   $	��150��6   $��3��6B   $��0��6O   " �� SelectedValuePath$" Value��5�      # �� DisplayMemberPath$# Description��6-   $
 True��5�      $
 0, -20&�5�       & "MaterialDesignFloatingHintComboBox#��[ 5�      $��10q�5�      ��5�   "   �� *	Causals+$ �� SelectedValue$ 5�      �� *Causal+$ TwoWay=�$ PropertyChanged=� 5�        *Reason for movement+5�      �� 5�   
   $	��150��6   $��3��6?   $��1��6L   $" Value��5�      $# Description��6-   $
 True��5�      $
 0, -20&�5�      #��[ 5�      $��10q�5�      ��5�   "   �� *Bays+$ 5�      �� *	BAY_Num+$ TwoWay=�$ PropertyChanged=� 5�        *Destination bay+{�5�      �� $ 	SelectBay�$ OneWay=�$ PropertyChanged=��� 5�   
   $��10,10,10,20q�6   $��4��6'   $��2��64   $	��Top=�5�      $��Stretch=�6*   $��80��6H   % �� AcceptsReturn$
% True��5�      & �� TextWrapping$
& Wrap=�6'   "' �� VerticalScrollBarVisibility$
' Auto=�5�       5�        *,*You can enter some notes for the operation+��5�      �� *Notes+$ TwoWay=�$ PropertyChanged=��� 5�   
   $��Center=�6   $��5��62   $��2��6?   $��Bottom=�6S   .+�5�      �� $��Center=�6   $
��0 10q�64   $��Center=�6B   ( �� Orientation$( 
Horizontal=�6_    5�       # MaterialDesignRaisedLightButton'  !MaterialDesignThemes.Wpf.PackIcon��%;  '��       7  '  �     �� 5�      " ��6   8   .�5�      ��  � ��$ 2q�63   �� 5�       � ��$ 30��63   �� 5�       � ��$ 30��62   �� 5�       � ��$ 0q�64   5�      �� 5�      "  6   .�5�      ��  � ��$ 24��63   �� 5�       � ��$ 24��62   5�      5�      .+�5�      �� -   5        $��QuantityDown��5�      .��5�        )   Kind$) Minus=�6.   5�      #  mSwDllWPFUtils.DecimalTextBox  5�      $	��100��6*   $��Center=�69   $��Center=�6]   $
��10 0q�6x   $��18(�6�    	 MaterialDesignTextBox#��[	 5�   *   ��5�   *   (  "mSwDllWPFUtils.CultureAwareBinding  $ RequestedQuantity�$ TwoWay=�$ PropertyChanged=�* �� 	Converter#* [ 5�   *   �� 5�      -   5        $��
QuantityUp��5�      .��5�        $
) Plus=�6.   5�      5�      �� 5�      $( 
Horizontal=�6    5�      ��%�   '        �� 5�      "  6   .�5�      ��  � ��$
 10 5q�63   �� 5�       � ��$ 24��63   �� 5�       � ��$ 24��62   �� 5�       � ��$ Center=�6@   �� 5�       � ��$ Center=�6>   5�      5�      .+�5�      �� -   5        $��Accept��5�       $
  MaterialDesignRaisedAccentButton#��[
 6'   $��5q�6a   $��40��6l   {�5�      �� $ OperationIsValid�$ PropertyChanged=�.��5�      �� $( 
Horizontal=�6!   .+�5�        $) Check=�62   �� 5�       ! MaterialDesignButtonTextBlock#��[ 5�   $   $��Center=�5�   $   $��0 0 10 0q�5�   $   ��5�   $     *Accept+$
 True��5�   $   5�      5�      �� 5�      -   5        $��Refuse��5�        MaterialDesignFlatButton#��[ 6'   $��5,5,10,5q�6Y   $��40��6k   $
{�True��5�      .��5�      �� $( 
Horizontal=�6!   .+�5�        $) Cancel=�62   �� 5�      #��[ 5�   $   $��Center=�5�   $   $��0 0 10 0q�5�   $   ��5�   $     *Cancel+$
 True��5�   $   5�      5�      5�      5�      � 5�   
   -   5        $��LoadingIsVisible��5�      $��10��6'   $��10��69   $y�	Collapsed=�6N   
�������+ J� Opacity$	+ 0.7Q�5�   "   	 5�      �� 5�      $��* �6    .+�5�      �  % !MaterialDesignCircularProgressBar#��[ 6   , � Value$, 0Q�5�      $��40��6$   $��40��6/   - � IsIndeterminate$
- True��5�      5�      5�      5      