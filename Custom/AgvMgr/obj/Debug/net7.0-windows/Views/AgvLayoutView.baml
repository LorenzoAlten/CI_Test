   M S B A M L   `   `   `  ����A  =AgvMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:AgvMgr.ViewsAgvMgr.Views  5      5clr-namespace:AgvMgr.ViewModelsAgvMgr.ViewModels  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5      5clr-namespace:AgvMgr.ConvertersAgvMgr.Converters  5	      G4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5
      G4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5      I4clr-namespace:mSwAgilogDll.SEW;assembly=mSwAgilogDllmSwAgilogDll.SEW 5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5           AgvMgr.Views.AgvLayoutView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R	 NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml   	  5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006   	  5      @d2http://schemas.microsoft.com/expression/blend/2008   	  5      ,localclr-namespace:AgvMgr.Views   	  5      5	viewModelclr-namespace:AgvMgr.ViewModels   	  5      V
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils   	  5      9agvconvertersclr-namespace:AgvMgr.Converters   	  5	      IWPFUtils4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils   	  5
      b
 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 
 5      ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll  5      >sew4clr-namespace:mSwAgilogDll.SEW;assembly=mSwAgilogDll  5      0sys&clr-namespace:System;assembly=mscorlib  5      ] YHelixToolkit.Core.Wpf, Version=2.23.0.0, Culture=neutral, PublicKeyToken=52aa3500039caf0d$hhttp://helix-toolkit.org/wpf  5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5         BooleanToVisibilityConverter6   0AgvMgr.Converters.MultiFlagToVisibilityConverter " MultiFlagToVisibilityConverter-   'AgvMgr.Converters.MultiConverterOneTrue  MultiConverterOneTrue.   (AgvMgr.Converters.MultiConverterAllFalse  MultiConverterAllFalse3  -mSwDllWPFUtils.Converters.InvertBoolConverter  InvertBoolConverter �� 
TargetType   MaterialDesignAccentCheckBox �� BasedOn �� Property �� Value=  7System.Windows.Controls.Primitives.DataGridColumnHeader &	 "MaterialDesignDataGridColumnHeader' 
 !MaterialDesignThemes.Wpf.PackIcon 
 dataFromEnum S� 
MethodName S� 
ObjectTypeU QSystem.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a  System.Enum	 S� MethodParameters
 M� TypeName��%�  &	       &	      &	 &     &	 9     &	 L     '�� _     7  '  �     7 	 '      &	
 �    �� 5   
   6'     5   
   67     5   
   6.     5   
   6/     5    
   6)   �� 5!   
   " ��6   8   .�5"      ��  � ��$ 0q�6'   �� 5#       � ��$ 0q�6(   5$      �� 5%   
   "  6   8   .�5&      ��  � ��$ Center=�6;   5'      �� 5(   
   "  6   .�5)      ��  � ��$ Center=�62   �� 5*       � ��$ 20��6'   �� 5+       � ��$ 20��6&   5,      S� 5-   
   $ GetNames��62   "  5.      	 5/      M� 50      $
 sew:Agv_Station_Type��6   52      53      .��54      �  � RowDefinitions 55   
   �� 56      $��* �6   6)   �� 57      $
��Auto �6   6,   �� 58      $
��Auto �6   6,    � ColumnDefinitions 5:   
   �� 5;      $��* �6   6+   �� 5<      $
��Auto �6   6.   .+�5>   
   �� $
��Auto=�6   $
��Auto=�6<   
�������.��5?      �� $��0��68   $��0��6H   $��Stretch=�5@      $��Stretch=�65   ��5?      �� *View3D+5@   5   5A      9� 5B   
   $��1��6   $��0��6#    9� ExpandDirection$ Right=�60   -	 
 'MaterialDesignThemes.Wpf.ExpanderAssist 	  VerticalHeaderPadding$ 0q�6H   ��5C      '
  !mSwDllWPFUtils.TranslateExtension
  *	Options+ 
  	Uppercase$
 True����5D      �� 5E      .R�5F      �� $��0 0 0 0q�6    $��Center=�61    $  MaterialDesignSubtitle2TextBlock#��[ 5G       ��5H       ��  � LayoutTransform 5I      �� 5J       �� Angle$ 90Q�6.   5L      5M      .��5O      �� $
��Auto=�6   .��5P      � $	��320��6    5Q      �� 5R      $
��Auto �6(   �� 5S      $
��Auto �6(   �� 5T      $
��Auto �6(   �� 5U      $
��Auto �6(   �� 5V      $��* �6(   �� 5W      $
��Auto �6(   .+�5Y      9� $��0��6   $��0 5 5 0q�6,    	  HorizontalHeaderPadding$ 0q�5Z   Q     SecondaryHueMidBrush#��[ 5[       " SecondaryHueMidForegroundBrush#��[ 6R   ��5Z      
  *	Options+$
 True����5\      �� 5]      .R�5^   "   �� $	��8 6q�6,   $��Center=�69   #��[ 5_   ,   ��5`   ,   �� 5a      .��5c      ��  �� Orientation$ Vertical=�6%      MaterialDesignCardBackground#��[ 6<   #���   5d   %    5e      ��%\   '��       7  �� 5f   "   " ��6(   8   .�5g   &   ��  � ��$	 4 2q�6?   5h   #   5i      .+�5j      �� ��6'   
  *Show Transponders+$
 True�� d� 	IsChecked 6f   �� *ShowTransponders+�� 5k      ��6'   
  *Show Background+$
 True�� 6d   �� *ShowBackground+�� 5l      ��6'   
  *Show Conveyor+$
 True�� 6b   �� *ShowConveyor+�� 5m      ��6'   
  *Show Agvs Path+$
 True�� 6c   �� *ShowAgvsPath+5n      5o      9� 5p      $��1��6   $��0 5 5 0q�6,   $ 0q�5r      #��[ 5s      #��[ 6R   y�5p   =   �� *SimulationEnabled+ �� 	Converter# [ ��5q      
  *
Simulation+$
 True����5t      �� 5u      .R�5v   "   �� $	��8 6q�6,   $��Center=�69   #��[ 5w   ,   ��5x   ,   �� 5y      .��5      �  5�      �� 5�   "   $
��Auto �60   �� 5�   "   $
��Auto �60   .+�5�      �� #��[ 6%   $��0��6`   ._�5�   "   &   System.Windows.Controls.DataGrid  -   5        $��AgvStations��5�   +   $��Stretch=�6>      SelectionUnit$ FullRow=�6Z      SelectionMode$ Single=�6r   $	��150��5�   +   #��[ 6;   "   VerticalScrollBarVisibility$
 Auto=�5�   +      AutoGenerateColumns$ False��5�   +      CanUserAddRows$ False��6G      CanUserDeleteRows$ False��6^   - 
 'MaterialDesignThemes.Wpf.DataGridAssist   CellPadding$ 5q�5�   +      ColumnHeaderPadding$ 4q�5�   +      Columns 5�   &   0  *System.Windows.Controls.DataGridTextColumn  5�   *   ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly$
 True��5�   9       Width5  /System.Windows.Controls.DataGridLengthConverter$  * 6K   !   MinWidth$! 70Q�6U   "   Header" 5�   =   
  *Code+$
 True��1  +System.Windows.Controls.DataGridBoundColumn#   Binding# 5�   9   �� *
MOD_Code+5�   U     5�   *   $
 True��5�   9   $  * 6K   $	! 100Q�6U   " 5�   =   
  *Udc+$
 True��# 5�   9   �� *Udc+5�   U     5�   *   $
 True��5�   9   $  * 6K   $! 50Q�6U   " 5�   =   
  *Queue+$
 True��# 5�   9   �� *PalletInCoda+5�   U   5�   #   5�      ( 
 "MaterialDesignThemes.Wpf.ColorZone  5�      $   Mode$$ PrimaryLight=�67   $��1��6K    5�   "    & "MaterialDesignToolForegroundButton��%�   '��       7  �� 5�   &   " ��6,   8   .�5�   *   ��  � ��$ 0q�6D   �� 5�   *    � ��$	 4 0q�6C   �� 5�   *    � ��$ Stretch=�6W   5�   '   5�   #   .��5�   "   A� % A� Rows$% 1��6.   & A� Columns$& 4��67   $
��Left=�6C   {�5�   2   �� *SimulationEnabled+.+�5�   &   �� -   5        $��StartSimulation��5�   -   {�6D   �� *SimulationOff+' � ToolTip' 5�   -   
  *Start+.��5�   *     (   Kind$
( Play=�6B   5�   '   �� 5�   &   -   5        $��StopSimulation��5�   -   {�6C   �� *SimulationOn+' 5�   -   
  *Stop+.��5�   *     $
( Stop=�6B   5�   '   �� 5�   &   -   5        $��ResetSimulation��5�   -   {�6D   �� *SimulationOff+' 5�   -   
  *Reset+.��5�   *     $( WindowClose=�6B   5�   '   5�   #   5�      5�      5�      9� 5�      $��2��6   $��0 5 5 0q�6,   $ 0q�5�      #��[ 5�      #��[ 6R   $y�	Collapsed=�5�      ��5�      
  *Handling Calls+$
 True����5�      �� 5�      .R�5�   "   �� $	��8 6q�6,   $��Center=�69   #��[ 5�   ,   ��5�   ,   �� 5�      .��5�      �� #��[ 6!   ._�5�        -   5        $��Stations��5�   '   $��Stretch=�6\   $ FullRow=�6x   $ Single=�6�   $	��150��5�   )   #��[ 69   $
 Auto=�5�   %   $ False��5�   %   $ False��6A   $ False��6X   $ 5q�5�   %   $ 4q�5�   %   ��5�   7   �� *AgvStations+ 5�   "     5�   &   $
 True��5�   9   $  55 6K   " 5�   9   
  *Code+$
 True��# 5�   9   �� *
MOD_Code+5�   K     5�   &   $
 True��5�   9   $	  160 6K   " 5�   9   
  *Udc+$
 True��# 5�   9   �� *Udc+5�   K   4  .System.Windows.Controls.DataGridTemplateColumn  5�   &   $
 True��5�   9   $  * 6K   $! 50Q�6U   " 5�   =   
  *Status+$
 True��)   CellTemplate) 5�   *   �� 5�   .   .R�5�   2   �� $
��Left=�6=   $ 
Horizontal=�6X   .+�5�   6     $( Export=�6N   $��18��6\   $��18��6g   * J� IsHitTestVisible$* False��6s   $��0q�6�   $��0q�6�   y�5�   :   `� 5�   >   + `� 	Converter#+ [ 6K   .1�5�   B   �� , �� Path$, Status�6J   �� 5�   B   )  #mSwAgilogDll.SEW.Agv_Station_Status-   	Expulsion. �� Source#. Z- 6J   / �� Mode$/ OneWay=�6   5�   ?   5�   7     5�   6   $( Import=�6N   $��18��6\   $��18��6g   $* False��6s   $��0q�6�   $��0q�6�   y�5�   :   `� 5�   >   #+ [ 6K   .1�5�   B   �� $, Status�6J   �� 5�   B   0   Request#. Z0 6J   $/ OneWay=�6}   5�   ?   5�   7     5�   6   $( AlertOutline=�6N   $��18��6b   $��18��6m   $* False��6y   $��0q�6�   $��0q�6�   y�5�   :   `� 5�   >   #+ [ 6K   .1�5�   B   �� $, Status�6J   �� 5�   B   1   Alarm#. Z1 6J   $/ OneWay=�6{   5�   ?   5�   7     5�   6   $
( Play=�6N   $��18��6Z   $��18��6e   $* False��6q   $��0q�6�   $��0q�6�   y�5�   :   `� 5�   >   #+ [ 6K   .1�5�   B   �� $, Status�6J   �� 5�   B   2   Auto#. Z2 6J   $/ OneWay=�6z   5�   ?   5�   7   5�   3   5�   /   5�   '   5�      5�      5�      9� 5      $��3��6   $��0 5 5 0q�6,   $��0q�5  b   $ 0q�5     #��[ 5     #��[ 6R   ��5     
  *Handling Configuration+$
 True����5     �� 5     .R�5  "   �� $	��8 6q�6,   $��Center=�69   #��[ 5  ,   ��5  ,   �� 5	     .��5     ]�  5     ��%�  '��       7��'�� v     7��'�� �     7���� 5  "   " ��6(   8   .�5  &   ��  � ��$ Center=�6J   �� 5  &    � ��$	 0 4q�6?   5  #   �� 5  "   " ��6(   8   .�5  &   ��  � ��$ Center=�6J   �� 5  &    � ��$	 0 4q�6?   5  #   �� 5  "   " ��6(   8   .�5  &   ��  � ��$ Center=�6J   �� 5  &    � ��$	 0 4q�6?   5  #   5     .+�5     ]� $	��Top=�6(   #��[ 6=   {�5  "   `� 5  &   #+ [ 63   .1�5  *   �� $, StationInsert�62   �� 5  *   $, StationEdit�62   5  '   .+�5!  "   � $	��Top=�6'   $
��10 0q�6<   #���   6J    5"  &   �� 5#  *   $��* �6;   �� 5$  *   $��* �6;    5&  &   �� 5'  *   $
��Auto �68   .+�5)  &   �� $��Station code��60   $��0��6D   $��0��6Q   �� 5*  &   $��0��6.   $��1��6;   ��6K   �� *SelectedStation.MOD_Code+{�6u   �� *StationInsert+5+  #   � 5,  "   $
��10 0q�6'   $��Bottom=�65   #���   5-  !   3 � DataContext3 6_   �� *SelectedStation+ 5.  &   �� 5/  *   $��* �6;   �� 50  *   $��* �6;    52  &   �� 53  *   $
��Auto �68   �� 54  *   $
��Auto �68   �� 55  *   $
��Auto �68   �� 56  *   $
��Auto �68   �� 57  *   $
��Auto �68   �� 58  *   $
��Auto �68   �� 59  *   $
��Auto �68   �� 5:  *   $
��Auto �68   �� 5;  *   $
��Auto �68   �� 5<  *   $
��Auto �68   �� 5=  *   $
��Auto �68   �� 5>  *   $
��Auto �68   .+�5A  &   �� $��PartData��60   $��0��6@   $��0��6M   �� 5B  &   $��0��6.   $��1��6;   ��6K   �� *MOD_Code_Num+�� 5D  &   $
��Type��60   $��1��6<   $��0��6I   �� 5E  &   $��1��6/   $��1��6<   4 �� 
IsEditable$4 False��6L   5 �� SelectedItem5 5F  /   �� *MOD_PLN_Code+��6U   �� #. [
 �� 5H  &   $��X��60   $��2��69   $��0��6F   �� 5I  &   $��2��6.   $��1��6;   ��6K   �� *
MOD_QuotaX+�� 5K  &   $��Y��60   $��3��69   $��0��6F   �� 5L  &   $��3��6.   $��1��6;   ��6K   �� *
MOD_QuotaY+�� 5N  &   $��Angle��60   $��4��6=   $��0��6J   �� 5O  &   $��4��6.   $��1��6;   ��6K   �� *	MOD_Angle+5P  #   5Q       5R     $$ PrimaryLight=�67   $��Bottom=�6K   $��Stretch=�6c    5S  "   ��%�   '��       7  �� 5T  &   " ��6,   8   .�5U  *   ��  � ��$ 0q�6D   �� 5V  *    � ��$	 4 0q�6C   �� 5W  *    � ��$ Stretch=�6W   5X  '   5Y  #   .��5Z  "   ]� $��False��6,   .+�5\  &   A� $& 4��62   $
��Left=�6?   .+�5]  *   �� -   5        $��
AddStation��5]  1   ' 5^  -   
  *Add+{�5_  .   `� 5`  2   #+ [ 6?   .1�5a  6   �� $, StationInsert�6>   �� 5b  6   $, StationEdit�6>   5c  3   .��5e  .     $
( Plus=�6F   5f  +   �� 5g  *   -   5        $��EditStation��5g  1   ' 5h  -   
  *Edit+{�5i  .   `� 5j  2   #+ [ 6?   .1�5k  6   �� $, StationInsert�6>   �� 5l  6   $, StationEdit�6>   5m  3   .��5o  .     $
( Edit=�6F   5p  +   �� 5q  *   -   5        $��DeleteStation��5q  1   ' 5r  -   
  *Delete+{�5s  .   `� 5t  2   #+ [ 6?   .1�5u  6   �� $, StationInsert�6>   �� 5v  6   $, StationEdit�6>   5w  3   .��5y  .     $( TrashCan=�6F   5z  +   �� 5{  *   -	   5        $��SetTrack��5{  1   ' 5|  -   
  *Track+{�5}  .   `� 5~  2   #+ [ 6?   .1�5  6   �� $, StationInsert�6>   �� 5�  6   $, StationEdit�6>   5�  3   .��5�  .     $( PackageVariantClosed=�6F   5�  +   5�  '   A� 5�  &   $& 2��62   $��Right=�6>   .+�5�  *   �� -
   5        $��SaveStation��5�  1   ' 5�  -   
  *Save+{�5�  .   `� 5�  2   #+ [ 6?   .1�5�  6   �� $, StationInsert�6>   �� 5�  6   $, StationEdit�6>   5�  3   .��5�  .     $( Floppy=�6F   5�  +   �� 5�  *   -   5        $��CancelChange��5�  1   ' 5�  -   
  *Cancel+{�5�  .   `� 5�  2   #+ [ 6?   .1�5�  6   �� $, StationInsert�6>   �� 5�  6   $, StationEdit�6>   5�  3   .��5�  .     $( UndoVariant=�6F   5�  +   5�  '   5�  #   5�     5�     5�     �� 5�     $��5��6!   $ Vertical=�6.   .+�5�     �� $��14(�6E   ��6$   �� *CameraCordinate+6U   �� 5�     -   5        $��	ZoomReset��5�  !   $��5q�6j   ��62   
  *
Reset Zoom+$
 True��6j   5�     5�     5�     5�     9� 5�  
   -   5        6 9�Name$6 
ExpDetails��5�  8   $��0��6L   $��1��6\   $��2��6i   $
 Down=�6}   $ 3q�6�   7 9� 
IsExpanded7 6   �� *ExpandDetails+��5�     �� 5�     $��Center=�6   $��Center=�69   #��[ 5�     ��5�     
  *Agv Details+$
 True��5�     .��5�     ]� .+�5�     �� $ 
Horizontal=�6   $	��Top=�66   $
��12 2q�6K   .+�5�     �� -   5        $��PrevAgv��5�     $��2q�6,   $��0q�67   $��24��6C   $��24��6N    " MaterialDesignFlatAccentButton#��[ 5�     .��5�       $( ArrowLeftBoldCircle=�62   $��24��6M   $��24��6X   5�     �� 5�     -   5        $��NextAgv��5�     $��2q�6,   $��0q�67   $��24��6C   $��24��6N   #��[ 5�     .��5�       $( ArrowRightBoldCircle=�62   $��24��6N   $��24��6Y   5�     5�     �� 5�     $
��12 4q�6J   3 6!   �� *SelectedAgvModel+  Caliburn.Micro.View8   Model8 6X   �� 5�     5�     5�     �� 5�  
   $��Right=�6   $��0��61   $��2��6A   $��2��6N   $��0 4 6 0q�6b   $ 
Horizontal=�5�      5�      $  MaterialDesignRaisedAccentButton��%\   '��       7  �� 5�     " ��6   8   .�5�     ��  � ��$	 5 2q�6/   5�     5�     .+�5�     �� -   5        $��ReloadTrackDataAllAgv��5�     ' 5�     
  *Reload+.��5�     �� $ 
Horizontal=�6   .+�5�     ��  ! MaterialDesignButtonTextBlock#��[ 6Q   $��Center=�6�   ��6    
  *Reload+$
 True��6�     5�     $( Reload=�6.   5�     5�     �� 5�     -   5        $��StartAllAgv��5�     ' 5�     
  *Start+.��5�     �� $ 
Horizontal=�6   .+�5�     �� #��[ 6P   $��Center=�6�   ��6    
  *Start+$
 True��6�     5�     $
( Play=�6.   5�     5�     �� 5�     -   5        $��
StopAllAgv��5�     ' 5�     
  *Stop+.��5�     �� $ 
Horizontal=�6   .+�5�     �� #��[ 6O   $��Center=�6�   ��6    
  *Stop+$
 True��6�     5�     $
( Stop=�6.   5�     5�     �� 5�     -   5        $��ResetAllAgv��5�     ' 5�     
  *Reset+.��5�     �� $ 
Horizontal=�6   .+�5�     �� #��[ 6P   $��Center=�6�   ��6    
  *Reset+$
 True��6�     5�     $( RedoVariant=�6.   5�     5�     �� 5�     -   5        $��	SimulaAll��5�     $��Right=�5�     $��Bottom=�61   ��5�  &   
  *Simulate all+$
 True��' 6`   
  *Simulate all+5�  1   5�     5�     5�     