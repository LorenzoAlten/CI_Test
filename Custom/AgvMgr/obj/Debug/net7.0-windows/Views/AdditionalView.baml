   M S B A M L   `   `   `  ����I  EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils  5      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters  5      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5	      
 AgvMgr!   AgvMgr.Views.AdditionalView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 	 5      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils 	 5      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 	 5	      ^
 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 
 5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5      %   mSwDllWPFUtils.DecimalConverter  DecimalConverter   Decimals   BooleanToVisibilityConverter<   6mSwDllWPFUtils.Converters.BooleanToVisibilityConverter & "InvertBooleanToVisibilityConverter�   �mSwDllWPFUtils.Converters.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False �� 
TargetType # MaterialDesignRaisedLightButton �� BasedOn �� Property	 �� Value*  $System.Windows.Controls.DataGridCell&   System.Windows.Controls.DataGrid  MaterialDesignDataGrid  MaterialDesignCardGroupBox 	 DecimalStyle' 	 !MaterialDesignThemes.Wpf.PackIcon�� 5   
   %9  &	       &	      &	 .     '�� l     7  '      7 '  N    7  '�� �    7  &		     7��'  _      5      $ 3��6=   �� 5      6+     5      $ 	Collapsed=�5   6   $ Visible=�5   6   �� 5      " ��6   8   .�5      ��  � ��$	 0q�6+   �� 5       � ��$	 0q�6,   �� 5       � |�$	 False��6.   5      �� 5       "  6   8   .�5!      ��  � ��$	 0q�64   5"      �� 5#      "  6   8   .�5$      ��  � ��$	 0q�6+   5%      �� 5&      " ��6   8   .�5'      ��  � ��$	 0q�6+   �� 5(       � ��$	 Stretch=�66   5)      �� 5*      " ��6)   8   .�5+      ��  � ��$	 Right=�68   5,      �� 5-      "  6   .�5.      ��  � ��$	 24��6+   �� 5/       � ��$	 24��6*   50      51      .��53      �� .+�54   
   ( 	 "MaterialDesignThemes.Wpf.ColorZone  
   Mode$
 Accent=�6#   $	��Top=�61    � DataContext 6F   �� *Agv+.��55      �� $��Center=�6    $
  MaterialDesignSubtitle2TextBlock#��[
 56      $��8q�57      �� 58      �6   �� *
AGV_Code+ �� 59      $�:��6    �� 5:      �6   '	   !mSwDllWPFUtils.TranslateExtension	  *Additional Data+ 	  	Uppercase$
 True��5;      5<        5=   
   -   5        $��AdditionalList��5=   1   $��Stretch=�6   $��Stretch=�6G      SelectionUnit$ FullRow=�6c      SelectionMode$ Single=�6{   "   VerticalScrollBarVisibility$
 Auto=�5>         AutoGenerateColumns$ False��5?         CanUserAddRows$ False��6/      CanUserDeleteRows$ False��6F   -
 	 'MaterialDesignThemes.Wpf.DataGridAssist 
  CellPadding$ 12 8 8 8q�5@       
  ColumnHeaderPadding$ 4q�5A         Columns 5B      0  *System.Windows.Controls.DataGridTextColumn  5C      ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly$
 True��5E   5      MinWidth$	 100Q�6G      Header 5C   %   	  *Data+$
 True��1  +System.Windows.Controls.DataGridBoundColumn   Binding 5D   5   �� *Data+5E   G     5F      $
 True��5H   5   $	 100Q�6G    5F   %   	  *Value+$
 True�� 5G   5   �� *Value+5H   G   5J        5L   
   $
 Accent=�6#   $	��Top=�61    6F   �� *Agv+.��5M      �� $��Center=�6   #��[
 5N      $��8q�5O      �� 5P      �6   �� *
AGV_Code+ �� 5Q      $�:��6    �� 5R      �6   	  *Additional Command+$
 True��5S      5T      ]� 5U   
   $��4q�6   .+�5V      �� $��Right=�6    * &MaterialDesignFloatingActionMiniButton#��[ 5W       
 Caliburn.Micro.Message   Attach$ ReloadTrackData��5X      .��5Y           Kind$ ArrowRightCircleOutline=�6*   5Z      �� 5[      $��Center=�6T   ��6   	  *Reload Track data+$
 True��6T   5\      ' 	 !MaterialDesignThemes.Wpf.Snackbar  5]   
   $��Bottom=�6O      MessageQueue 6"   4 	 .MaterialDesignThemes.Wpf.MessageQueueExtension  -   'mSwDllWPFUtils.SnackBarCustomProperties   SnackBarNotification 5^   "   �� *SnackBarMessage+5`      5a      