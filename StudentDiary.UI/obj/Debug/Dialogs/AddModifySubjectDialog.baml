   M S B A M L   `   `   `  ����  StudentDiary.UI4    .StudentDiary.UI.Dialogs.AddModifySubjectDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Icon$  ../Resources/titleicon.png��5   	    � Title$ Enter subject info��5	   	   $	��350��6$   $	��300��61   .��5
      �  � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��40 �6   �� 5      $��30 �6   �� 5      $��40 �6   �� 5      $��30 �6   �� 5      $��90 �6   �� 5      $��40 �6   .+�5   
   �� $��Center=�6   $��Center=�61   $��15(�6L   $��0��6Z   Please enter subject name :6�   �� 5   
   -   5        $��SubjectName��5      $	��230��6%   $��1��61   6A   �� 5   
   $��Center=�6   $��Center=�61   $��15(�6L   $��2��6Z   !Please select subject teacher :6�   �� 5   
   -   5         ��Name$ AwailableTeachersList��5       $��3��6   $	��230��6?   $��5q�6K   ��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      ��  �� Padding$ 2q�6$   ��60   �� *	FirstName+�� 5      $ 2q�6$   ��60   �� *
MiddleName+�� 5      $ 2q�6$   ��60   �� *
LastName+5      5      5!      �� 5"   
   $��Center=�6   $��Center=�61   $��15(�6L   $��4��6Z   $"Please enter subject description :6�   �� 5#   
   -   5        $��SubjectDescription��5#   2    �� TextWrapping$ WrapWithOverflow=�6   $	��230��6L   $��5��6X   6g   �� 5$   
   -   5        $��6��5$      $	��230��64   $��OK��6@   $��5q�6M   5%      5&      