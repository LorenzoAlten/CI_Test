   M S B A M L   `   `   `  ����C  ?SimulaRV, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SimulaRV.ViewsSimulaRV.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5	          SimulaRV.Views.AppView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      .localclr-namespace:SimulaRV.Views     5      b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils 	 5      =agilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll 	 5	      ^
 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 
 5
         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5      .��5      ]� .+�5   
   � $��Bottom=�6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      $��* �6#   �� 5      $
��Auto �6#   .+�5       �� $��0��6   $	��5 0q�6U   ��6+   �� *ManagerCollection+��5!      �� 5"      .R�5#      ��  �� Orientation$ 
Horizontal=�6%   5$      ��5&      �� 5'      .R�5(      �� $ 
Horizontal=�6%   .+�5)      �� $ 
Horizontal=�6)   $��5q�6B   .+�5*   "   �� ��6,   ��  �� Path$
 Name�*  $mSwAgilogDll.TrafficManagerStateIcon  5+   "   $	��5,0q�6A   �� 5,   "   $��:��6,   $��0,0,5,0q�65   �� 5-   "   ��6/   �� *ControllerCollection+��5.   &   �� 5/   *   .R�50   .   �� $ 
Horizontal=�69   51   +   ��53   &   �� 54   *   .R�55   .   �� $ 
Horizontal=�69   $��0,0,5,0q�6R   .+�56   2   �� ��6<   �� $
 Desc�-  'mSwAgilogDll.TrafficControllerStateIcon  57   2   $	��5,0q�6T   58   /   59   +   5;   #   5<      �� 5=      ���#333333$��1��67   $��0q�6A   5>      5?      5A      �� 5B      $��1��6   $	��5 0q�6\   ��6+   �� *GenericManagerCollection+��5C      �� 5D      .R�5E      �� $ 
Horizontal=�6%   5F      ��5H      �� 5I      .R�5J      �� $ 
Horizontal=�6%   .+�5K      �� $ 
Horizontal=�6)   $��5q�6B   .+�5L   "   �� ��6,   �� $
 Name�  5M   "   $	��5,0q�6A   5N      �� 5O      ���#333333$��1��67   $��0q�6A   5P      5Q      5S      �� 5U      -   5        $��Reset��5U       . *MaterialDesignFloatingActionMiniDarkButton#��[ 6"   $��20��5V      $��20��6!   $��-5q�6,   $��2��68   .��5W      ' 	 !MaterialDesignThemes.Wpf.PackIcon     Kind$ Refresh=�6*   $��16��69   $��16��6E   5X      �� 5Z      $��4��6R   $
��10 5q�6b   $y�	Collapsed=�6p   ��6   �� $	 Now� �� StringFormat$ dd/MM/yy HH:mm:ss��6p   5[      �� 5\   
   $��Bottom=�6   $	��5 0q�6,   � 5^   
   $��Bottom=�6    5_      �� 5`      $��* �6#   .+�5c      �� $��0��6   $	��5 0q�6Q   ��6+   �� *ScbCollection+��5d      �� 5e      .R�5f      �� $ 
Horizontal=�6%   5g      ��5i      �� 5j      .R�5k      �� $ 
Horizontal=�6%   .+�5l      �� $ 
Horizontal=�6)   $��5q�6B   .+�5m   "   �� ��6,   �� $
 Name�  5n   "   $	��5,0q�6D   5o      �� 5p      ���#333333$��1��67   $��0q�6A   5q      5r      5t      5u      �� 5v   
   $��Bottom=�6   $	��5 0q�6,   � 5x   
   .+�5y      ]� .+�5z      ]� $	��Top=�6   .+�5{      �� $��Right=�6   $	��150��5      $
��12 0q�6.   ��5|      �� *ControllerCollection+ �� SelectedValue 5}      �� *SelectedController+ �� DisplayMemberPath 5~      �� *Desc+5   .   �� 5�      ��6   �� *	Screens+	 �� SelectedItem	 5�      �� *
ActiveItem+��5�      �� 5�      .R�5�   "   �� $ 
Horizontal=�6-   5�      ��5�      �� 5�      .R�5�   "   �� $
��16 8q�5�   ,    $  MaterialDesignSubtitle1TextBlock#��[ 6:    
 Caliburn.Micro.Message
   Attach$K
 E[Event MouseLeftButtonDown] = [ActivateItemAsync($dataContext, null)]��5�   ,   ��5�   ,   �� *DisplayName+5�   ,   5�      5�      5�      �� 5�      -   5         ��Name$ 
ActiveItem��5�   !   5�      � 5�       % !MaterialDesignCircularProgressBar#��[ 6    � Value$ 0Q�5�      $��40��6$   $��40��6/   
������� J� Opacity$	 0.7Q�6N   $��Center=�5�      $��Center=�65    � IsIndeterminate$
 True��5�      y�5�      �� *	IsLoading+   BooleanToVisibilityConverter �� 	Converter# [ 5�      5�      5�      