   M S B A M L   `   `   `  ����D  @OrdersMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:OrdersMgr.ViewsOrdersMgr.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      $    OrdersMgr.Views.BookedUDCsView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:OrdersMgr.Views     5      Eresx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5         MaterialDesignBody#���   5	      $��Regular"�5
      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5      .��5      ]� .+�5   
   (  "MaterialDesignThemes.Wpf.ColorZone  $	��Top=�6#   $��16q�68   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�6E      Mode$ 
PrimaryMid=�6v    K� ZIndex$ 1��5   #   .��5      �� -   5        $��DisplayName��5      $��Center=�6+   $��Center=�6H   $��22(�6c   5      &   System.Windows.Controls.DataGrid  5   
   -   5        $
��UDCs��5      $��Stretch=�6      SelectionUnit$ FullRow=�6;      SelectionMode$ Single=�6S   "   VerticalScrollBarVisibility$
 Auto=�5   9      AutoGenerateColumns$ False��5      	   CanUserAddRows$	 False��6/   
   CanUserDeleteRows$
 False��6F   -  'MaterialDesignThemes.Wpf.DataGridAssist   CellPadding$ 12 8 8 8q�5         ColumnHeaderPadding$ 4q�5       �� SelectedItem 5      �� *SelectedItem+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly$
 True��5!   %      Header 5   %   '  !mSwDllWPFUtils.TranslateExtension  *Line+   	Uppercase$
 True��1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 5    %   �� *EXE_ODT_Line+5!   %     5"      $
 True��5$   %    5"   %     *Code+$
 True�� 5#   %   �� *
UDC_Code+5$   %     5%      $
 True��5&   %    5%   %     *Type+$
 True�� 5'      `� 5(       �� StringFormat$ 	{0} - {1}��6'   .1�5)      ��  �� Path$ UDC_UDT_Code�6&   �� 5*      $ UDT_Desc�6&   5+      5-      4
  .System.Windows.Controls.DataGridCheckBoxColumn
  5.      $
 True��50   )    5.   )     *	Wrapped+$
 True�� 5/   )   �� *UDC_Wrapped+50   )     51      $
 True��52   %    51   %     *Item+$
 True�� 53      `� 54      $ 	{0} - {1}��6'   .1�55      �� $ ITM_Code�6&   �� 56      $ ITM_Desc�6&   57      59        5:      $
 True��5<   %    5:   %     *Stock+$
 True�� 5;   %   �� *	UCM_Stock+$ F0�� �� ConverterCulture$ IT��5<   %     5=      $
 True��5?   %      MinWidth$ 50Q�67    5=   %     *Compartment+$
 True�� 5>   %   �� *	UCM_Index+5?   7     5@      $
 True��5B   %   $ 50Q�67    5@   %     *X+$
 True�� 5A   %   �� *CEL_X+5B   7     5C      $
 True��5E   %   $ 50Q�67    5C   %     *Y+$
 True�� 5D   %   �� *CEL_Y+5E   7     5F      $
 True��5H   %   $ 50Q�67    5F   %     *Z+$
 True�� 5G   %   �� *LOC_Z+5H   7     5I      $
 True��5K   %   $ 50Q�67    5I   %     *W+$
 True�� 5J   %   �� *LOC_W+5K   7   5M      5N      5O      