   M S B A M L   `   `   `  ����H  DMissionViewer, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:MissionViewer.ViewsMissionViewer.Views  5      D @AgilogDll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:AgilogDll;assembly=AgilogDll	AgilogDll 5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5	      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5
      C&clr-namespace:MissionViewer.ViewModelsMissionViewer.ViewModels  5      $    MissionViewer.Views.BackupView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R	 NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml   	  5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006   	  5      @d2http://schemas.microsoft.com/expression/blend/2008   	  5      3local!clr-namespace:MissionViewer.Views   	  5      b
 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 
 5      7custom*clr-namespace:AgilogDll;assembly=AgilogDll 
 5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 
 5	      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 
 5
      ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5      /vm&clr-namespace:MissionViewer.ViewModels  5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5      $��Roboto)�5      .��5        AgilogDll.DpiDecorator  ._�5   
   � .+�5      ]� .+�5      ��  �� Orientation$ 
Horizontal=�6   $	��Top=�66   $
��12 4q�6K   .+�5      ��  ( $MaterialDesignHeaderedContentControl#��[ 5      $	��6 4q�5      ��5      '  !mSwDllWPFUtils.TranslateExtension  *	Mission+.��5      �� -   5        $��MisId��5   "   $	��100��6/   . 
 (MaterialDesignThemes.Wpf.TextFieldAssist   HasClearButton$
 True��5      5       �� 5"      #��[ 5#      $	��6 4q�5$      ��5"        *	Load unit+.��5%      �� -   5        $��UdcCode��5%   "   $	��150��61   $
 True��5&      5'      �� 5)      #��[ 5*      $	��6 4q�5+      ��5)        *Order+.��5,      �� -   5        $��	OrderCode��5,   "   $	��150��63   $
 True��5-      5.      �� 50      #��[ 51      $	��6 4q�52      ��50        *From+.��53      �� $ 
Horizontal=�6%   .+�54      (  "System.Windows.Controls.DatePicker  -   5        $��FromDate��54   )   $	��100��55   "   $��0 0 6 0q�61   ) 
 #MaterialDesignThemes.Wpf.TimePicker  56         	Is24Hours$
 True��57   0      SelectedTime 56   8   �� *
FromTime+ �� UpdateSourceTrigger$ 	LostFocus=�57   0   58      59      �� 5:      #��[ 5;      $	��6 4q�5<      ��5:        *To+.��5=      �� $ 
Horizontal=�6%   .+�5>        -   5          Name$ ToDate��5>   )   $	��100��5?   "   $��0 0 6 0q�61     5@      $
 True��5A   8    5@   8   �� *ToTime+$ 	LostFocus=�5A   8   5B      5C      �� 5D      -   5        $��LoadData��5D      $
��10 0q�5E       0 ,MaterialDesignFloatingActionMiniAccentButton#��[ 5F      .��5G      ' 
 !MaterialDesignThemes.Wpf.PackIcon     Kind$ Refresh=�62   $��20��6A   $��20��6M   5H      5I      �� 5K      ��6   �� *MissionsList+��5L      �� 5M      .R�5N      ��   Caliburn.Micro.View	   Model	 6-   �� 5O      ��5Q      �� 5R      .R�5S      $� 
 %� IsVirtualizing$

 True��65    %� VirtualizationMode$ 	Recycling=�5T      5U      ��5W      �� 5X       �� 
TargetType" ��6*   .R�5Y      �� #��z��6%   #��z��5Z   !   #��z��5[   !   ._�5\   "   �� $
��True��6/   #��z��5]   +   $|�False��5^   +   .��5_   &   �� 5`   #   5a      5b      5d      5e      � 5g       % !MaterialDesignCircularProgressBar#��[ 6    � Value$ 0Q�5h      $��40��6    $��40��6+   
������� J� Opacity$	 0.7Q�6J   $��Center=�5i      $��Center=�61    � IsIndeterminate$
 True��5j      y�5k      �� *	IsLoading+   BooleanToVisibilityConverter �� 	Converter# [ 5l      5m      5n      