   M S B A M L   `   `   `  ����D  @WhsViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WhsViewer.ViewsWhsViewer.Views  5      I EmSwDllWPFUtils, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtilsmSwDllWPFUtils 5      +    %WhsViewer.Views.SelectDestinationView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:WhsViewer.Views     5      b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ?resx4clr-namespace:mSwDllWPFUtils;assembly=mSwDllWPFUtils  5      ^	 ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 5	         MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#��[ 5       � 	Resources 5        CommandButtonStyle �� 
TargetType  MaterialDesignRaisedButton �� BasedOn �� Property �� Value & "MaterialDesignFloatingHintComboBox�� 5   
   %o  &	       7  '�� �     7  �� 5      " ��6/   8   .�5      ��  � ��$ 50��6+   �� 5       � ��$	 200��6-   �� 5       � ��$ 20(�6-   �� 5       � ��$ 5q�6+   5      �� 5      " ��6   8   .�5      ��  � ��$ 30��6.   �� 5       � ��$ 5 5 5 0q�6+   �� 5       � ��$ 15(�6-   5      5       .��5#      � .+�5$   
   ]� .+�5%      �� $	��Top=�5&       $  MaterialDesignHeadline6TextBlock#��[ 6-   $
��12 4q�5'      ��5%      '  !mSwDllWPFUtils.TranslateExtension  *Select destination channel+   	Uppercase$
 True��5'      �� 5)      $��Bottom=�6    �� Orientation$ 
Horizontal=�61   $��Center=�5*      $��60��66   .+�5+      �� -   5        $��Confirm��5+      #��[ 6(   ��5,        *	Confirm+$
 True���� 5-      -   5        $��Cancel��5-      #��[ 6'   y�5.      �� *	CanInsert+   BooleanToVisibilityConverter	 �� 	Converter#	 [ ��5/        *Cancel+$
 True��50      �� 52      $
��Auto=�6   $��5q�6>   .��53      �� .+�55      �� $
��2 10q�56   >   
 �� SelectedValuePath$
 Value��57       �� DisplayMemberPath$ Description��69    �� SelectedValue 55      �� *Aisle+ �� UpdateSourceTrigger$ PropertyChanged=���56      �� *Aisles+)  #MaterialDesignThemes.Wpf.HintAssist   Hint 58        *Aisle+�� 5:      $
��2 10q�5;   >   $
 Value��5<      $ Description��69    5:      �� *Level+$ PropertyChanged=���5;      �� *Levels+ 5=        *Floor+5>      �� 5@      $
��2 10q�5A   @   $
 Value��5B      $ Description��69    5@      �� *	Channel+$ PropertyChanged=���5A      �� *
Channels+ 5C        *	Channel+5D      5E      5F      � 5H   
    % !MaterialDesignCircularProgressBar#��[ 6    � Value$ 0Q�5I      $��40��6    $��40��6+   
������� J� Opacity$	 0.7Q�6J   $��Center=�5J      $��Center=�61    � IsIndeterminate$
 True��5K      y�5L      �� *	IsLoading+#	 [ 5M      5N      