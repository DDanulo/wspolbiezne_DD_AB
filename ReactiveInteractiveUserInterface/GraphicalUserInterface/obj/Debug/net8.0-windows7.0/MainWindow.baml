   M S B A M L   `   `   `  ����i  eTP.ConcurrentProgramming.PresentationViewModel, Version=4.2.0.0, Culture=neutral, PublicKeyToken=null�uclr-namespace:TP.ConcurrentProgramming.Presentation.ViewModel;assembly=TP.ConcurrentProgramming.PresentationViewModel/TP.ConcurrentProgramming.Presentation.ViewModel  5   	   - )TP.ConcurrentProgramming.PresentationView:   4TP.ConcurrentProgramming.PresentationView.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   �vmuclr-namespace:TP.ConcurrentProgramming.Presentation.ViewModel;assembly=TP.ConcurrentProgramming.PresentationViewModel     5   	     � Title$5  /Presentation layer targeting reactive behaviour��5   	   $
��auto��5	   	   $
��auto��6   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6"   $��5q�5      $	��120��5      ��5      �� *BallCountText+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   $��0��6   $��1��6!   $��5q�5      $	��120��5      $��Balls��5       ��5!      �� *StartCommand+�� 5#   
   -   5        $��BoardBorder��5#      $��1��6$   $��0��64   $��2��6A   $��4q�5$      
��� � �$	��400��5%      $	��400��6   ._�5&      �� ��6   �� *Balls+��5'      �� 5(      .R�5)      �� 5*      ��5,      �� 5-      .R�5.      A� ��6"   �� *
Diameter+��5/   "   �� *
Diameter+��50   "   �� *Colour+   
RgbToBrush �� 	Converter# [  52      ��54      �� 55       �� 
TargetType" ��6   .�56      ��  �� Property �@��Top �� Value 67   �� *Top+�� 57       �@��Left 68   �� *Left+58      5:      5;      5<      5=      