   M S B A M L   `   `   `  ����G  CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullI4clr-namespace:mSwAgilogDll.SEW;assembly=mSwAgilogDllmSwAgilogDll.SEW  5      A0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll  5	      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtilsmSwDllWPFUtils.Converters 5
      A =AgvMgr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AgvMgr.ConvertersAgvMgr.Converters 5      G4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      +clr-namespace:AgvMgr.ViewsAgvMgr.Views 5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5         AgvMgr.Views.SEWAgvView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R	 NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml   	  5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006   	  5      @d2http://schemas.microsoft.com/expression/blend/2008   	  5      b
 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 
 5      ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5      D	agilogsew4clr-namespace:mSwAgilogDll.SEW;assembly=mSwAgilogDll  5      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll  5	      P
converters?clr-namespace:mSwDllWPFUtils.Converters;assembly=mSwDllWPFUtils  5
      3agvconvertersclr-namespace:AgvMgr.Converters  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5      %viewclr-namespace:AgvMgr.Views  5      0sys&clr-namespace:System;assembly=mscorlib  5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5        dataFromEnum S� 
MethodName S� 
ObjectTypeU QSystem.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a  System.Enum S� MethodParameters M� TypeName3  -mSwDllWPFUtils.Converters.EnumToBoolConverter  EnumToBoolConverter3  -AgvMgr.Converters.MultiFlagToBooleanConverter  MultiFlagToBooleanConverter6  0AgvMgr.Converters.MultiFlagToVisibilityConverter " MultiFlagToVisibilityConverter8  2AgvMgr.Converters.MultiConverterVisibilityAllEqual $  MultiConverterVisibilityAllEqual �� 
TargetType  MaterialDesignCardGroupBox �� BasedOn	 �� Property
 �� Value 	 MaterialDesignTextBox 
 MaterialDesignRaisedButton  MaterialDesignTextBlock ( $MaterialDesignUserForegroundCheckBox��%�  &	       &	 y     &	 �     &	 �     &	 �     '�� �     7  '��     7 	 '�� c    7 
 '�� �    7  '��      7  S� 5   
   $ 	GetValues��62   "  5       5      M� 5      $ agilog:AGV_Enabling_Mode��6   5        5   
   6)     5    
   64     5!   
   67     5"   
   69   �� 5#   
   " ��6   8   .�5%      �� 	 � ��$
 Stretch=�62   5&      �� 5'   
   " ��6   8   .�5(      �� 	 � ��$
 5q�6'   5)      �� 5*   
   " ��6   8   .�5+      �� 	 � ��$
 5q�6'   5,      �� 5-   
   " ��6   8   .�5.      �� 	 � ��$
 Center=�62   50      �� 51   
   " ��6   8   .�52      �� 	 � ��$
 Center=�62   �� 53      	 � ��$

 10 5q�6'   54      55      .��56      �� $��0q�57      . 
 (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�58      ��56      �� *Agv.AGV_Code+��59   
   �� 5:      .R�5;      �� $��Center=�6   $
��12 8q�67    $  MaterialDesignSubtitle2TextBlock#��[ 5<       ��5=       �� 5>      .��5@   
   ��  �� Orientation$ Vertical=�6   $��0 4 0 0q�6,   .+�5A      � $
��10 0q�6   .+�5B      ]� $��False��6    5C      ��%�   '��       �� 5D      " ��6    .�5E      �� 	 �@��Orientation$
 
Horizontal=�6<   �� 5F      	 � ��$	
 200��69   5G      5H      .+�5I      �� .+�5J      �� $��0��6`   $��0��6m   ��6$   '  !mSwDllWPFUtils.TranslateExtension  *Stato+   Suffix$ :��   	Uppercase$
 True��6m   �� 5K      $��0��6J   $��1��6W   $	��5 0q�5L      #��[ 6)   $��Center=�5M      ��5K   $   �� *Agv.AgvRequest.State+5M      5N      �� 5P      .+�5Q      �� $��0��6d   $��3��6q   ��6$     *	Part Data+$ :��$
 True��6q   �� 5R      $��0��6D   $��4��6Q   $	��5 0q�5S      #��[ 6)   $��Center=�5T      ��5R   $   �� *Agv.Part_data+5T      5U      �� 5W      .+�5X      �� $��0��6b   $��5��6o   ��6$     *	Station+$ :��$
 True��6o   �� 5Y      $��0��6B   $��6��6O   $	��5 0q�5Z      #��[ 6)   $��Center=�5[      ��5Y   $   �� *Agv.Station+5[      5\      �� 5^      .+�5_      �� ��6$     *	Stato AGV+$ :��$
 True���� 5`      $	��5 0q�6C   $	��250��6P   #��[ 5a   (   $��Center=�5b   (   ��5`   $   �� *Agv.Agv_State+5b   (   5c      �� 5e      .+�5f      �� ��6$     *Stop Reason+$ :��$
 True���� 5g      $	��5 0q�6E   #��[ 5h   $   $��Center=�5i   $   ��5g   $   �� *Agv.Stop_Reason+5i   $   5j      �� 5l      $��Right=�6!   .+�5m      �� ��6$     *Pallet loaded+$ :��$
 True���� 5n      $	��5 0q�5o      #��[ 6)   $��Center=�5p      ��5n   $   �� *Agv.AgvRequest.AGV_NumPallet+5p      5q      5r      �� 5t      $	��5 0q�6    $  MaterialDesignRaisedAccentButton#��[ 6'   	  Caliburn.Micro.Message 	  Attach$ Alarm��5u      $��Right=�64   $��0��6P   $��3��6]    � ToolTip 5v        *Alarms+{�5w      `� 5x       `� 	Converter# [ 6'   .1�5y      ��  �� Path$ Agv.AgvRequest.State�6&   �� 5z      
   mSwAgilogDll.State_Flags 
  Alarm �� Source# Z 6&    �� Mode$ OneWay=�6S   5{      .��5}      ' 
 !MaterialDesignThemes.Wpf.PackIcon     Kind$ Warning=�6.   $��24��5~   .   $��24��6:   5      5�      �� 5�      $ 
Horizontal=�6   .+�5�      ��  J� IsHitTestVisible$ False��6N   $|�False��6g   ��6     *Ready+$
 True�� d� 	IsChecked 5�      `� 5�      # [ 6'   .1�5�      �� $ Agv.Agv_State�6&   �� 5�      &    mSwAgilogDll.SEW.SEW_State_Flags   Ready# Z 6&   $ OneWay=�6Z   5�      5�      �� 5�      $ False��6P   ��6     *	Running+$
 True�� 5�      `� 5�      # [ 6'   .1�5�      �� $ Agv.Agv_State�6&   �� 5�         Running# Z 6&   $ OneWay=�6\   5�      5�      �� 5�      $ False��6N   ��6     *Error+$
 True�� 5�      `� 5�      # [ 6'   .1�5�      �� $ Agv.Agv_State�6&   �� 5�         Error# Z 6&   $ OneWay=�6Z   5�      5�      �� 5�      $ False��6P   ��6     *	Warning+$
 True�� 5�      `� 5�      # [ 6'   .1�5�      �� $ Agv.Agv_State�6&   �� 5�         Warning# Z 6&   $ OneWay=�6\   5�      5�      �� 5�      $ False��6P   ��6     *	Forward+$
 True�� 5�      `� 5�      # [ 6'   .1�5�      �� $ Agv.Agv_State�6&   �� 5�         Direction_forward# Z 6&   $ OneWay=�6f   5�      5�      �� 5�      $ False��6Y   ��6     *Battery charging+$
 True�� 5�      `� 5�      # [ 6'   .1�5�      �� $ Agv.Agv_State�6&   �� 5�         Battery_charging# Z 6&   $ OneWay=�6e   5�      5�      �� 5�      $ False��6Z   ��6     *Charging required+$
 True�� 5�      `� 5�      # [ 6'   .1�5�      �� $ Agv.Agv_State�6&   �� 5�          Battery_charging_required# Z 6&   $ OneWay=�6n   5�      5�      �� 5�      $ False��6S   ��6     *
Simulation+$
 True�� 5�      `� 5�      # [ 6'   .1�5�      �� $ Agv.Agv_State�6&   �� 5�          
Simulation# Z  6&   $ OneWay=�6_   5�      5�      5�      ( 
 "MaterialDesignThemes.Wpf.ColorZone  5�      !   Mode$! PrimaryDark=�6'   $��Center=�6:   $��4q�6U   .��5�      ]� $��False��6   .+�5�      �� $ 
Horizontal=�6!   .+�5�      �� $
��10 0q�5�   $   ��5�   $     *	Mission+$
 True��5�   $   # 
 MaterialDesignThemes.Wpf.Card  5�      $	��200��6.     PrimaryHueLightBrush#���  6:    " PrimaryHueLightForegroundBrush#���  6n   $��Center=�5�   -   .��5�      �� $��Center=�6T   ��6(   �� *Agv.AgvRequest.AGV_Mission+6T   5�        5�      $ ArrowRightBoldOutline=�62   $��Center=�6O   $��0q�6j   $��10 0 0 0q�6v   �� 5�      $	��200��6Q   $��Center=�6]   " �� TextAlignment$" Center=�6z   ��6$   �� *Agv.AgvRequest.MissionState+6z     5�      $ 
Fullscreen=�62   $��0 0 10 0q�5�   :   ��5�   D   �� # �� RelativeSource# �� $ �� AncestorType$ 
StackPanel$ ActualHeight���5�   :   �� # �� *Self+$ ActualHeight�y�5�   L   �� *Agv.AgvRequest.AGV_Loaded+  InverseBoolToVisConverter% �� 	Converter#% [   5�      $ CubeScan=�62   $��0 0 10 0q�5�   :   ��5�   B   �� # �� $ 	DockPanel$ ActualHeight���5�   :   �� # �� *Self+$ ActualHeight�y�5�   L   �� *Agv.AgvRequest.AGV_Loaded+   BooleanToVisibilityConverter#% [ �� 5�      #��[ 6!   $ EditMission��5�   !   $	��5 0q�6B    5�   !     *Edit mission+.��5�        $
 Edit=�66   $��24��5�   6   $��24��6B   5�      �� 5�      #��[ 6!   $ OpenCradles��5�   !   $	��5 0q�6B   $	��5 0q�6P    5�   !     *	Track UDC+.��5�        $ 	BorderAll=�66   $��24��5�   6   $��24��6B   5�      �� 5�       0 ,MaterialDesignFloatingActionMiniAccentButton#��[ 6!   $��Right=�5�   !   $��24��5�   !   $��24��6-   $ AdditionalData��5�   !   .��5�        $ AboutVariant=�66   $��24��5�   6   $��24��6B   5�      5�      �� 5�      $ 
Horizontal=�6!   $��Right=�6:   .+�5�      �� -   5        $��Continue��5�   5   #��[ 5�   !   $��5q�6[   $��CONTINUE��5�   E   �� 5�      -   5        $��SetSingleTrack��5�   5   #��[ 5�   !   $��0 5 50 5q�6[   $��SINGLE TRACK��5�   K   �� 5�      -   5        $��Start��5�   5   #��[ 5�   !   $��5q�6[   $��START��5�   B   �� 5�      -   5        $
��Stop��5�   5   #��[ 5�   !   $
�� 0 5q�6[   $
��STOP��5�   A   �� 5�      -   5        $��
AlarmReset��5�   5   #��[ 5�   !   $��50 5 0 5q�6[   $��RESET��5�   G   5�      5�      5�      �� 5�      $ Vertical=�6   $
��10 5q�60    5�      ��%�   '��       �� 5�      " ��6   .�5�      �� 	 �@��Orientation$
 
Horizontal=�68   �� 5�      	 � ��$	
 200��65   5�      5�      .+�5�      ]� 5�      �� 5      $ 
Horizontal=�6   .+�5     �� .+�5     �� ��6$     *Transponder+$ :��$
 True���� 5     $	��5 0q�6E   #��[ 5  (   $��Center=�5  (   ��5  $   �� *Agv.Transponder+5  (   5     �� 5     .+�5     �� ��6$     *VTrack+$ :��$
 True���� 5	     $	��5 0q�6J   #��[ 5
  (   $��Center=�5  (   ��5	  $   �� *Agv.Virtual_track_ID+5  (   5     �� 5     .+�5     �� ��6$     *Single Track+$ :��$
 True���� 5     $	��5 0q�6G   #��[ 5  (   $��Center=�5  $   ��5  $   �� *Agv.Single_tracks+5  $   5     �� 5     .+�5     �� ��6$     *Single Track Assigned+$ :��$
 True���� 5     $	��5 0q�6M   #��[ 5  (   $��Center=�5  $   ��5  $   �� *Agv.AgvRequest.AGV_Zone+5  $   5     �� 5     .+�5     �� ��6$     *Travel Mode+$ :��$
 True���� 5     $	��5 0q�6D   #��[ 5  (   $��Center=�5  $   ��5  $   �� *Agv.TravelMode+5  $   5     �� 5     .+�5      �� ��6$     *Area+$ :��$
 True���� 5!     $	��5 0q�6A   #��[ 5"  (   $��Center=�5#  $   ��5!  $   �� *Agv.Area_ID+5#  $   5$     5%     �� 5&     $ 
Horizontal=�6   .+�5'     �� .+�5(     �� ��6$     *x+$ :��$
 True���� 5)     $	��5 0q�6D   #��[ 5*  (   $��Center=�5+  (   ��5)  $   �� *Agv.Absolute_X+5+  (   5,     �� 5-     .+�5.     �� ��6$     *y+$ :��$
 True���� 5/     $	��5 0q�6D   #��[ 50  (   $��Center=�51  (   ��5/  $   �� *Agv.Absolute_Y+51  (   52     �� 53     .+�54     �� ��6$     *Angle+$ :��$
 True���� 55     $	��5 0q�6?   #��[ 56  (   $��Center=�57  (   ��55  $   �� *	Agv.Angle+57  (   58     �� 59     .+�5:     �� ��6$     *Transponder Distance+$ :��$
 True���� 5;     $	��5 0q�6O   #��[ 5<  (   $��Center=�5=  (   ��5;  $   �� *Agv.DistanceToTransponder+5=  (   5>     �� 5?     .+�5@     �� ��6$     *Speed+$ :��$
 True���� 5A     $	��5 0q�6?   #��[ 5B  (   $��Center=�5C  (   ��5A  $   �� *	Agv.Speed+5C  (   5D     �� 5E     .+�5F     �� ��6$     *Power+$ :��$
 True���� 5G     $	��5 0q�6?   #��[ 5H  (   $��Center=�5I  (   ��5G  $   �� *	Agv.Power+5I  (   5J     �� 5K     .+�5L     �� ��6$     *Energy+$ :��$
 True���� 5M     $	��5 0q�6F   #��[ 5N  (   $��Center=�5O  (   ��5M  $   �� *Agv.Energy_level+5O  (   5P     5Q     ]� 5R     .+�5S     �� $ Vertical=�6!   .+�5T     ]� $��False��6$   .+�5U     �� $ 
Horizontal=�6)   .+�5V  "   ]� {�6,   �� *,*Agv.AgvRequest.ModifyAllowed_Enabling_Mode+.+�5W  &   �� $��0 0 10 0q�61   & �� Padding$& 0q�6C   $��Center=�6P   ��5X  =     *Modalità AGV+$ :��$
 True���� 5Y  &   $	��150��6/   $; 5[Event SelectionChanged] = [Action SetEnablingMode()]��5[  ;   ��5Y  ;   �� # [ ' �� SelectedValue' 5Z  ;   �� *&$Agv.AgvRequest.SelectedEnabling_Mode+5[  ;   � 5\  &    % !MaterialDesignCircularProgressBar#��[ 62   ( � IsIndeterminate$
( True��6m   $��Stretch=�5]  >   $��5 5 10 5q�6Z   y�5^  *   `� 5_  .   # [ 6;   .1�5`  2   �� $* $Agv.AgvRequest.SelectedEnabling_Mode�6:   �� 5a  2   $" Agv.AgvRequest.Enabling_Mode�6:   5b  /   5d  '   �� 5e  &   -   5        $��
LoadUnload��5g  9   #��[ 5e  -   $��0 5 10 5q�6g   $��LOAD/UNLOAD��5g  K   {�5f  9   �� *Agv.AgvRequest.Enabling_Mode+#% [ ) �� ConverterParameter$) Semiautomatic��5g  K   �� 5h  &   #��[ 6-   $ SetPartData��5j  9   $��SET PART DATA��6Z   {�5i  9   �� *Agv.AgvRequest.Enabling_Mode+#% [ $) Semiautomatic��5j  Z   �� 5k  &   $��10 0 0 0q�6.   $��0q�6@   $	��100��6L   $��Center=�6X   * �� AcceptsReturn$
* True��6s   ��5l  :   �� *Agv.Part_data_To_send+5m  #   �� 5n  "   -   5        $��
cbModifica��5n  +   $��Modifica��6=   $��10 0 0 0q�6�    6P   �� *,*Agv.AgvRequest.ModifyAllowed_Enabling_Mode+6�   5o     �� 5p     $ 
Horizontal=�6)   $��Right=�6B   .+�5q  "   �� $��0 0 5 0q�6n   $& 0q�6   $��Center=�6�   ��6,     *Transponder+$ :��$
 True��6�   �� 5r  "   $��5q�6+   $��50��66   $��Center=�6A   ��6\   �� *Agv.SimulatedTransponder+�� 5s  "   $��0 0 5 0q�6j   $& 0q�6{   $��Center=�6�   ��6,     *	Station+$ :��$
 True��6�   �� 5t  "   $��5q�6+   $��50��66   $��Center=�6A   ��6\   �� *Agv.SimulatedStation+�� 5u  "   -   5        $��Simula��5w  -   #��[ 5u  )   $��5 5 20 5q�5v  -   $��SIMULA��5w  ;   {�5x  &   `� 5y  *   # [ 67   .1�5z  .   �� $ Agv.Agv_State�66   �� 5{  .   # Z  66   $ OneWay=�6o   5|  +   5~  #   y�5  "   `� 5�  &   # [ 63   .1�5�  *   �� $ Agv.Agv_State�62   �� 5�  *   # Z  62   $ OneWay=�6k   5�  '   5�     5�     5�     5�     5�     5�     5�     5�     