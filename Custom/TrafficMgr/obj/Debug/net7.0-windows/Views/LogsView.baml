   M S B A M L   `   `   `  ����I  EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils  5      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5       
TrafficMgr   TrafficMgr.Views.LogsView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Eresx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils     5      Cagilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll     5      b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5         MaterialDesignBody#���   5	      $��Regular"�5
      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5       � 	Resources 5      %   mSwDllWPFUtils.DecimalConverter  DecimalConverter   Decimals$  mSwAgilogDll.LogLevelConverter  LogLevelConverter  DecimalStyle �� 
TargetType �� BasedOn �� Property �� Value*  $System.Windows.Controls.DataGridCell��%�   &	       &	      &	 .     7��'  ~     7   5   
   $ 3��69     5   
   6#   �� 5   
   " ��6%   8   .�5      ��  � ��$ Right=�64   5      �� 5   
   "  6   8   .�5      ��  � ��$ 0q�60   5      5      .��5      ]� .+�5   
   ( 	 "MaterialDesignThemes.Wpf.ColorZone  $	��Top=�6#   $��16q�68   + 	 %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�6E   	   Mode$	 
PrimaryMid=�6v   
 K� ZIndex$
 1��5   #   .��5      �� -   5        $��DisplayName��5      $��Center=�6+   $��Center=�6H   $��22(�6c   $	��Top=�6q   5       &   System.Windows.Controls.DataGrid  5&   
   -   5        $
��Logs��5&      $��Stretch=�6      AutoGenerateColumns$ False��5'         CanUserAddRows$ False��6/      CanUserDeleteRows$ False��6F   - 	 'MaterialDesignThemes.Wpf.DataGridAssist   CellPadding$ 12 8 8 8q�5(         ColumnHeaderPadding$ 4q�5)      "   VerticalScrollBarVisibility$
 Auto=�5*      
������    Columns 5,      0	  *System.Windows.Controls.DataGridTextColumn	  5-      ,
  &System.Windows.Controls.DataGridColumn 
  
IsReadOnly$
 True��5/   %    
  Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 67    
  MinWidth$	 100Q�6D    
  Header 5-   %   '   !mSwDllWPFUtils.TranslateExtension  *	Date/Time+   	Uppercase$
 True��1  +System.Windows.Controls.DataGridBoundColumn   Binding 5.   %   �� *
DateTime+ �� StringFormat$ dd/MM/yy HH:mm:ss��5/   D   	  50      $
 True��52   %   $ * 67   $	 300Q�6A    50   %     *	Message+$
 True�� 51   %   �� *	Message+52   A   	  53      $
 True��55   %   $
 Auto 67   $	 100Q�6D    53   %     *User+$
 True�� 54   %   �� *User+55   D   	  56      $
 True��58   %   $
 Auto 67   $	 100Q�6D    56   %     *Level+$
 True�� 57   %   �� *Level+ �� 	Converter# [ 58   D   5:      5;      5<      