   M S B A M L   `   `   `  ����I  EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils  5       
TrafficMgr$   TrafficMgr.Views.TelegramsView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Eresx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ^	 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 5         MaterialDesignBody#���   5	      $��Regular"�5
      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5       � 	Resources 5      %   mSwDllWPFUtils.DecimalConverter  DecimalConverter   Decimals  DecimalStyle �� 
TargetType �� BasedOn �� Property �� Value*  $System.Windows.Controls.DataGridCell��%�   &	       &	      7��'  k     7   5   
   $ 3��69   �� 5   
   " ��6%   8   .�5      ��  � ��$ Right=�64   5      �� 5   
   "  6   8   .�5      ��  � ��$ 0q�60   5      5      .��5      ]� .+�5   
   (  "MaterialDesignThemes.Wpf.ColorZone  $	��Top=�6#   $��16q�68   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�6E   	   Mode$	 
PrimaryMid=�6v   
 K� ZIndex$
 1��5   #   .��5      �  � ColumnDefinitions 5       �� 5!      $��* �6'   �� 5"      $
��Auto �6'   �� 5#      $
��Auto �6'   .+�5%      �� -   5        $��DisplayName��5%      $��Center=�6/   $��Center=�6L   $��22(�6g   �� 5'      -   5        $��Export��5'       0 ,MaterialDesignFloatingActionMiniAccentButton#��[ 6'   $��1��5(      $��4 0 4 0q�6)   .��5)      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ 
FileExport=�6.   $��24��6@   $��24��6L   5*      �� 5+      -   5        $��Refresh��5+      #��[ 6(   $��2��5,      $��4 0 8 0q�6)   .��5-        $ Refresh=�6.   $��24��6=   $��24��6I   5.      5/      50        52   
   $	��Top=�6#   $	 Standard=�68   $	��8 2q�6H   $��4q�6V      CornerRadius$ 2��6a   $
 1��6r   $ Depth1=�53   #   .��54      ]� .+�55      ��   MaterialDesignToolButton#���  6   $
��Left=�56      .��57        $ Search=�6.    J� Opacity$	 .56Q�6<   58      �� 59      $��8 0 0 0q�6W   .  (MaterialDesignThemes.Wpf.TextFieldAssist   DecorationVisibility$ Hidden=�5;      $��0q�5<      $	��200��6.   $��Center=�5=       	 Caliburn.Micro.Message   Attach$< 6[Event KeyDown] = [Action FilterTelegrams($eventArgs)]��5>      ��59      �� *Filter+ �� UpdateSourceTrigger$ PropertyChanged=�)	  #MaterialDesignThemes.Wpf.HintAssist 	  Hint 5:      '
   !mSwDllWPFUtils.TranslateExtension
  *Search message+5>      5?      5@      &   System.Windows.Controls.DataGrid  5B   
   -   5        $��Entries��5B      $��Stretch=�6"      SelectionUnit$ FullRow=�6>      SelectionMode$ Single=�6V   $��1��6m   "   VerticalScrollBarVisibility$
 Auto=�5C   9      AutoGenerateColumns$ False��5D         CanUserAddRows$ False��6/      CanUserDeleteRows$ False��6F   -  'MaterialDesignThemes.Wpf.DataGridAssist   CellPadding$ 12 8 8 8q�5E         ColumnHeaderPadding$ 4q�5F       �� SelectedItem 5C      �� *SelectedItem+   Columns 5G      0  *System.Windows.Controls.DataGridTextColumn  5H      ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly$
 True��5J   %      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 67      MinWidth$	 120Q�6D       Header  5H   %   
  *	Date/Time+! 
  	Uppercase$
! True��1  +System.Windows.Controls.DataGridBoundColumn"   Binding" 5I   %   �� *	Timestamp+# �� StringFormat$# dd/MM/yy HH:mm:ss��5J   D     5K      $
 True��5M   %   $
 Auto 67   $	 100Q�6D     5K   %   
  *Sender+$
! True��" 5L   %   �� *Sender+5M   D     5N      $
 True��5P   %   $
 Auto 67   $	 100Q�6D     5N   %   
  *Dest+$
! True��" 5O   %   �� *Dest+5P   D     5Q      $
 True��5S   %   $ * 67   $	 300Q�6A     5Q   %   
  *	Message+$
! True��" 5R   %   �� *	Message+5S   A   5U      5V      5W      