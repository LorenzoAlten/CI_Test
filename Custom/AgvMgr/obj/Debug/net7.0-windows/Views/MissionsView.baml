   M S B A M L   `   `   `  ����I  EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils  5      ]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters  5      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5	      
 AgvMgr   AgvMgr.Views.MissionsView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 	 5      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils 	 5      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 	 5	      ^
 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 
 5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5      %   mSwDllWPFUtils.DecimalConverter  DecimalConverter   Decimals   BooleanToVisibilityConverter<   6mSwDllWPFUtils.Converters.BooleanToVisibilityConverter & "InvertBooleanToVisibilityConverter�   �mSwDllWPFUtils.Converters.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False'  !mSwAgilogDll.MissionTypeConverter  MissionTypeConverter)  #mSwAgilogDll.MissionResultConverter  MissionResultConverter)  #mSwAgilogDll.MissionCausalConverter  MissionCausalConverter �� 
TargetType #	 MaterialDesignRaisedLightButton �� BasedOn �� Property	 �� Value' 	 !MaterialDesignThemes.Wpf.PackIcon)	  #System.Windows.Controls.DataGridRow
 �� Triggers �� Binding �� Value*
  $System.Windows.Controls.DataGridCell&   System.Windows.Controls.DataGrid 
 MaterialDesignDataGrid  MaterialDesignCardGroupBox  
ToolButton & "MaterialDesignToolForegroundButton  DecimalStyle�� 5   
   %_  &	       &	      &	 .     &	 l     &	      &	 �     '�� �     7 	 '  �    '	  �    7	 '
  S    7
 '  �    7 
 '�� �    7  &	 `    7  &	 g    7��  5      $ 3��6=   �� 5      6+     5      $ 	Collapsed=�5   6   $ Visible=�5   6     5      6*     5      6,     5      6,   �� 5      " ��6   8   .�5      ��  � ��$	 5q�6+   �� 5        � ��$	 0q�6,   �� 5!       � |�$	 False��6.   �� 5"       � ��$	 60��6+   �� 5#       � ��$	 75��6*   �� 5$       � ��$	 Stretch=�6?   5%      �� 5&      "  6   .�5'      ��  � ��$	 5q�6+   �� 5(       � ��$	 24��6+   �� 5)       � ��$	 24��6*   �� 5*       � ��$	 Center=�68   �� 5+       � ��$	 Center=�66   5,      �� 5-      " 	 6   8   
 5.      �� 5/       6"   �� *MIS_L1_MST_Code+ ERR6F   .a�50      ��  � ��
	 �����67   �� 51       � ��
	 �  ��67   52      54      �� 55      " 
 6   8   .�56      ��  � ��$	 0q�64   57      �� 58      "  6   8   .�59      ��  � ��$	 0q�6+   5:      �� 5;      " ��6   8   .�5<      ��  � ��$	 0q�6+   �� 5=       � ��$	 Stretch=�66   5>      �� 5?      " ��6'   8   .�5@      ��  � ��$	 5q�6+   �� 5A       � ��$	 0q�6,   �� 5B       � |�$	 False��6.   �� 5C       � ��$	 50��6+   �� 5D       � ��$	 50��6*   �� 5E       � ��$	 Stretch=�6?   5F      �� 5G      " ��6)   8   .�5H      ��  � ��$	 Right=�68   5I      5J      .��5M      � .+�5N   
   �� $��0��6   $��0q�6    $
��12 4q�6,   $	��150��6:   . 	 (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�5P      ��5O      '   !mSwDllWPFUtils.TranslateExtension  *
Missions+   	Uppercase$
 True����5Q      �� 5R      .R�5S      �� $��8q�6    $��Center=�6+    $  MaterialDesignSubtitle2TextBlock#��[ 5T   (   ��5U   (   �� 5V      .��5X      ]� .+�5Y      ( 	 "MaterialDesignThemes.Wpf.ColorZone     Mode$ PrimaryLight=�6+   $��Bottom=�6?    5Z      ��%�   '��       7  �� 5[      " ��6    8   .�5\      ��  � ��$	 0q�68   �� 5]       � |�$	 False��6:   �� 5^       � ��$	 Stretch=�6K   5_      5`      .��5a      �� -   5        $��
RefreshAll��5a      $��Right=�6/    � ToolTip 5b        *	Refresh+.��5c           Kind$ Refresh=�62   5d      5e      ' 	 !MaterialDesignThemes.Wpf.Snackbar  5g      $��Bottom=�5h   *      MessageQueue 5g   *   4 	 .MaterialDesignThemes.Wpf.MessageQueueExtension  -   'mSwDllWPFUtils.SnackBarCustomProperties   SnackBarNotification 5i   *   �� *SnackBarMessage+  5k      -   5        $��MissionsList��5k      $��Stretch=�6/      SelectionUnit$ FullRow=�6K      SelectionMode$ Single=�6c   $��1��6z   "   VerticalScrollBarVisibility$
 Auto=�5l   L      AutoGenerateColumns$ False��5m   #      CanUserAddRows$ False��6?      CanUserDeleteRows$ False��6V   - 	 'MaterialDesignThemes.Wpf.DataGridAssist   CellPadding$ 12 8 8 8q�5n   #      ColumnHeaderPadding$
 12 4q�5o   #    
 Caliburn.Micro.Message   Attach$C =[Event MouseDoubleClick] = [Action MissionMouseDoubleClick()]��5p   #    �� SelectedItem 5l   #   �� *SelectedMission+ 5q        RowMenu � DataContext �� RelativeSource  m� Command! �� AncestorType" �� Path# m� Icon$ �� 	Converter��%2  &	       7  7  7  7  �� 5r       66   �� *PlacementTarget.DataContext+ �� *Self+.@�5s      m� ��6'     *Load unit details+  6U   ��  �� "!  $#" DataContext.RowDetailsCommand�# 5t   "     5u   &   $ PackageVariantClosed=�6>   5w      �� 5x      m� 5y      ��6'     *Retry command+  6Q   ��  �� "!  $!" DataContext.RowRetryCommand�y�5z   /   �� *CanStcRetry+8$   # 5{   "     5|   &   $ UndoVariant=�6>   5~      m� 5      ��6'     *Abort+  6G   ��  �� "!  $!" DataContext.RowAbortCommand�# 5�   "     5�   &   $ DeleteForeverOutline=�6>   5�      m� 5�      ��6'     *Complete mission+  6R   ��  �� "!  $$" DataContext.RowCompleteCommand�y�5�   /   �� *CanStcComplete+8$  # 5�   "     5�   &   $# CheckboxMultipleMarkedOutline=�6>   5�      m� 5�      ��6'     *Abort+  6G   ��  �� "!  $$" DataContext.RowAbortHndCommand�y�5�   /   �� *CanActionHnd+8$  # 5�   "     5�   &   $ DeleteForeverOutline=�6>   5�      m� 5�      ��6'     *
Evacuate+  6J   ��  �� "!  $$" DataContext.RowEvacuateCommand�y�5�   /   �� *CanActionHnd+8$  # 5�   "     5�   &   $ Export=�6>   5�      5�      5�      %   RowStyle% 5�      �� 5�      " 	 6    # [B	 69   .�5�      ��  �@�ContextMenu#	 [ 6<   5�      &   Columns& 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn'   
IsReadOnly$
' True��5�   5   (   MinWidth$	( 100Q�6G   )   Header) 5�   -     *ID+$
 True��1  +System.Windows.Controls.DataGridBoundColumn*   Binding* 5�   5   �� *MIS_Id+5�   G     5�      $
' True��5�   5   $	( 100Q�6G   ) 5�   -     *	Load unit+$
 True��* 5�   5   �� *MIS_UDC_Code+5�   G     5�      $
' True��5�   -   $	( 100Q�6?   ) 5�   -     *	State 1+$
 True��* 5�   -   �� *MIS_L1_MST_Desc+5�   ?     5�      $
' True��5�   -   $	( 100Q�6?   ) 5�   -     *	State 2+$
 True��* 5�   -   �� *MIS_L2_MST_Desc+5�   ?     5�      $
' True��5�   5   $	( 100Q�6G   ) 5�   -     *Causal+$
 True��* 5�   5   �� *MIS_CAU_Code+#$ [ 5�   G     5�      $
' True��5�   5   $	( 100Q�6G   ) 5�   -     *Source+$
 True��* 5�   5   �� *
MIS_Source+5�   G     5�      $
' True��5�   5   $	( 100Q�6G   ) 5�   -     *Destination+$
 True��* 5�   5   �� *
MIS_Dest+5�   G     5�      $
' True��5�   5   $	( 100Q�6G   ) 5�   -     *
Priority+$
 True��* 5�   5   �� *MIS_Priority+5�   G     5�      $
' True��5�   5   $	( 100Q�6G   ) 5�   -     *Agv+$
 True��* 5�   5   �� *Agv+5�   G   5�      5�      5�      � 5�   
    % !MaterialDesignCircularProgressBar#��[ 6   + � Value$+ 0Q�5�      $��40��6    $��40��6+   
�������, J� Opacity$	, 0.7Q�6J   $��Center=�5�      $��Center=�61   - � IsIndeterminate$
- True��5�      y�5�      �� *	IsLoading+#$ [ 5�      5�      