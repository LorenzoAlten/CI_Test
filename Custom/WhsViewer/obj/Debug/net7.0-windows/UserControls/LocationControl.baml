   M S B A M L   `   `   `  ����D  @WhsViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:WhsViewer	WhsViewer  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      G CmSwAgilogDll, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDllmSwAgilogDll 5          WhsViewer.LocationControl   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:WhsViewer     5      Eresx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils     5      Cagilog0clr-namespace:mSwAgilogDll;assembly=mSwAgilogDll     5      b	 ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 5	        � 	Resources  5       �� 
TargetType �� BasedOn �� Property �� Value    BooleanToVisibilityConverter��%�   '��       7��&	  �     �� 5   
   " ��6   8   .�5      ��  � ��$ Center=�64   �� 5       � ��$ Center=�62   �� 5       � ��$ 5q�6'   5      �� 5   
   6'   5      .��5      �� -   5         ��Name$ Border��5      ��6   �� *	ViewWidth+��69   �� *
ViewHeight+��5   
   �� 5      " ��6   .�5      ��  �@��CornerRadius$ 2��61   �� 5       � ��$ 1q�64   �� 5       � ��
 �   �60    �� Triggers 5      �� 5       �� Binding 6"   �� *
IsSelected+ �� Value True6A   .a�5      ��  �@��Padding$ 0q�64   �� 5       � ��$ 2q�6<   �� 5        � ��  SecondaryHueMidBrush# [ 68   5!      �� 5"       6"   �� *LOC_LOS_Code+ RIS6C   .a�5#      ��  � y�$ Visible=�67   5$      5&      ._�5(   
   � .+�5+      �� -   5        	 ��Name$	 Button��5+      ��6%   �� *
LOC_Desc+��5,      �� 
 �� ElementName$
 Border�� �� Path$ ActualWidth���6L   �� $
 Border��$ ActualHeight���5-      �� 5.      " ��6     MaterialDesignFlatButton# [ 60   .�5/      ��  � ��
 �����67   �� 50       � ��
 �   �67   �� 51       � ��$ 0q�64   �� 52       � ��$ 12(�65    53      �� 54       6*   �� *LOC_LOS_Code+ O6K   .a�55   "   ��  � �� �#0080006?   �� 56   "    � ��
 �����6?   57      �� 58       6*   �� *LOC_LOS_Code+ EP6K   .a�59   "   ��  � ��
 �  ��6?   �� 5:   "    � ��
 �����6?   5;      �� 5<       6*   �� *LOC_LOS_Code+ IP6K   .a�5=   "   ��  � ��
 � ���6?   �� 5>   "    � ��
 �   �6?   5?      �� 5@       6*   �� *LOC_LOS_Code+ IV6K   .a�5A   "   ��  � �� �#0072C66?   �� 5B   "    � ��
 �����6?   5C      �� 5D       6*   �� *LOC_LOS_Code+ RIS6K   .a�5E   "   ��  � ��
 �����6?   �� 5F   "    � ��
 �   �6?   �� 5G   "    �@J�Opacity$	 .15Q�6<   5H      �� 5I       6*   �� *	LOC_Error+ True6H   .a�5J   "   ��  � ��
 �  ��6?   �� 5K   "    � ��
 �����6?   5L      5N      5P      �� 5R      $��Center=�6   ��60   �� $
 Button��$ ActualWidth���6g   �� $
 Button��$ ActualHeight���5S      �� 5T      " ��6   .�5U      ��  � y�$ 	Collapsed=�67    5V      �� 5W       6*   �� *LOC_Enabled+	 False6J   .a�5X   "   ��  � y�$ Visible=�6?   5Y      5[      .+�5]      ��  �� Stretch$
 None=�6   	 �� X1$ 0��6Z   	 �� Y1$ 0��6a   
���   �$��2��6�   	 �� X2 6&   �� $
 Button��$ ActualWidth�	 �� Y2 6h   �� $
 Button��$ ActualHeight�6�   5^      5_      5`      5a      