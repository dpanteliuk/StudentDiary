   M S B A M L   `   `   `  ���� J  FStudentDiary.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:StudentDiary.UIStudentDiary.UI  K*clr-namespace:StudentDiary.UI.UserControlsStudentDiary.UI.UserControls  G(clr-namespace:StudentDiary.UI.UICommandsStudentDiary.UI.UICommands       StudentDiary.UI.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:StudentDiary.UI     CuserControls*clr-namespace:StudentDiary.UI.UserControls     ?
uiCommands(clr-namespace:StudentDiary.UI.UICommands       � WindowStartupLocation$  CenterScreen=� � Title$ Student Diary�� � SizeToContent$ Width=� � 
ResizeMode$ NoResize=�$	��800��$	��480�� � Icon$ Resources/titleicon.png�� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$1 +uiCommands:TeacherCommands.CmdDeleteTeacher��  -   $1 +uiCommands:TeacherCommands.CmdModifyTeacher��  -   $5 /uiCommands:SubjectCommands.CmdGetSubjectTeacher��  -   $1 +uiCommands:SubjectCommands.CmdDeleteSubject��  -   $3 -uiCommands:SubjectCommands.CmdGetSubjectTasks��  -   $1 +uiCommands:SubjectCommands.CmdModifySubject��  -   $< 6uiCommands:TimeTableCommands.CmdDeleteTimeTableSubject��  -   $< 6uiCommands:TimeTableCommands.CmdModifyTimeTableSubject��  -	   $= 7uiCommands:TimeTableCommands.CmdTimeTableGetSubjectInfo��  -
   $= 7uiCommands:TimeTableCommands.CmdTimeTableGetTeacherInfo��  -   $; 5uiCommands:TimeTableCommands.CmdTimeTableGetTasksInfo��  -   $1 +uiCommands:TimeTableCommands.CmdChangeTimes��  -   $+ %uiCommands:TaskCommands.CmdDeleteTask��  -   $+ %uiCommands:TaskCommands.CmdModifyTask��.���  � ColumnDefinitions �� $	��100 ��� .+��� -    ��Name$ GeneralMenu��	 �� SelectionMode$	 Single=�$��2q�
 � 	Resources
  �� 
TargetType �� Property �� Value��%4   '��       �� " ��.���  �@��Padding$ 4q�.@��� 
 � Tag ��  �� Orientation$ Vertical=�.+��� $��40��
�������$ 
Horizontal=�.+��� $��Time Table for week №��$��20(�$��Georgia)� �� Padding$ 
10 10 0 10q��� -   $��
WeekNumber��$
 2 10q�$��20(�$��Georgia)�$��10���� $��Start Date:��$��20(�
����  �$��Georgia)�$ 
10 10 0 10q��� -   $��WeekStartDate��$
 2 10q�$��20(�$��Georgia)��� $��	End Date:��$��20(�
����  �$��Georgia)�$ 
10 10 0 10q��� -   $��WeekEndDate��$
 2 10q�$��20(�$��Georgia)��� $ 
Horizontal=�.+��� $��10 0 5 0q�$��Center=�$��15(�
Semester #�� -   $��	Semesters��$��5q�$��75������ .R��� $ 
Horizontal=�.+��� $ 2q�$��13(����� *SemesterNumber+�� $ 2q�$��13(����� *	YearValue+ �� StringFormat$ , {0}���� -   $	��5 5q�$	��5 0q�.��Create semester�� -   $	��5 5q�$	��5 0q�.��Delete semester�� -   $	��5 5q�$��Right=�$��<- Previous week��$��30��$	��130���� -   $	��5 5q�$��Right=�$��Next week ->��$��30��$	��130���� $��Center=�$ 
Horizontal=�.+��� 
�������$ Vertical=�.+��� $��Center=�.��Times�� -   $��SubjectTimes��$��5q�
����������� .R��� $ 
Horizontal=�.+��� $��0 0 2 0q�$��12(����� *	PairStart+$ hh\:mm���� $��12(�$��-���� $��12(����� *	PairEnd+$ hh\:mm��3   -StudentDiary.UI.UserControls.TimeTableControl  -     Name$ SubjectControl���� $��Center=�$ Vertical=�.+��� -   $��70��$��0q�
�������.���� $��Resources/plus.ico���� -   $��70��$��0q�
�������.����  J� RenderTransformOrigin$ 0.5,0.5&�$��Resources/plus.ico��z���  �� Angle$ 45Q��� $ 
Horizontal=�.+��� $ Vertical=�.+��� -   $��SubjectInfo��$=��7uiCommands:TimeTableCommands.CmdTimeTableGetSubjectInfo��$��10q�$��5q�$��Subject Info���� -   $��TeacherInfo��$=��7uiCommands:TimeTableCommands.CmdTimeTableGetTeacherInfo��$��5q�$��10q�$��Teacher Info���� -   $��SubjectTasks��$;��5uiCommands:TimeTableCommands.CmdTimeTableGetTasksInfo��$��5q�$��10q�$��Subject Tasks���� $ Vertical=�.+��� $��10q�$��5q�$��Remove selected subject��$<��6uiCommands:TimeTableCommands.CmdDeleteTimeTableSubject���� $��10q�$��5q�$<��6uiCommands:TimeTableCommands.CmdModifyTimeTableSubject��$��Modify selected subject���� -   $��10q�$��5q�$��Add new subject���� $ Vertical=�.+��� $��10q�$��5q�$��Adjust selected times��$1��+uiCommands:TimeTableCommands.CmdChangeTimes���� $ Vertical=�.+��� -    $��TimeTableDescriptionType��$��0 10 0 0q�$ 5q�$��14(��� -!   $��TimeTableDescriptionName��$ 5q�$��14(��� -"   $!��TimeTableDescriptionTeacher��$ 5q�$��14(��� -#   $&�� TimeTableDescriptionNuberOfTasks��$ 5q�$��14(�.���� $��	TimeTable���� -$   $��SubjectsTab�� �� $ Vertical=�.+��� $��Subjects��$��20(�$��Georgia)�$��40��$ 10q�
��������� $��Center=�$ 
Horizontal=�.+��� $ Vertical=�.+��� $ 
19 15 10 0q�$��17(�List of all subjects :�� -%   $��SubjectList��$	��250��$	��250��
�������$
��auto��$��20q����� .R��� ���� *Name+�� $ Vertical=�.+��  � RowDefinitions �� �� �� �� �� .+��� $5��/uiCommands:SubjectCommands.CmdGetSubjectTeacher��$��0��$��5 55 5 5q�$��Subject Teacher��$��30���� $3��-uiCommands:SubjectCommands.CmdGetSubjectTasks��$��1��$��5q�$��Subject Tasks��$��30���� $1��+uiCommands:SubjectCommands.CmdDeleteSubject��$��2��$��5q�$��Delete Subject��$��30���� -&   $��3��$��5q�$��Add new Subject��$��30���� $1��+uiCommands:SubjectCommands.CmdModifySubject��$��4��$��5 5 5 10q�$��Modify Subject��$��30���� -'   $��SubjectDescription��$��50��$��WrapWithOverflow=�$	��250��.���� $��Subjects���� -(   $��TasksTab�� �� $ Vertical=�.+��� $��40��
�������$ 
Horizontal=�.+��� $��	All Tasks��$��20(�$��Georgia)�$ 
10 10 0 10q��� $ 
Horizontal=�.+�&   System.Windows.Controls.DataGrid  -)   $��	TasksGrid��   SelectionMode$ Single=�$	��370��"   VerticalScrollBarVisibility$
 Auto=�   RowBackground
 �����$��0q�
�������$��	10 10 0 0q�   
IsReadOnly .    AutoGenerateColumns .     Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Subject��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *TaskTableItemSubject.Name+     Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 $ Description�� �� *#!TaskTableItemTask.TaskDescription+  $ 	Dead line�� �� * TaskTableItemTask.DeadLineDate+$ 
yyyy-MM-dd��  $ Priority�� �� * TaskTableItemTask.TaskPriority+�� $ Vertical=�.+��� -*    ��Name$ CertainSubjectTasks��$��
10 10 10 3q�$	��150��$��Certain subject only���� -+     ��Name$  AwailableSubjects��{�.  $��	10 3 10 3q�$	��150��$��5q����� .R��� ���� *Name+�� -,   $��	10 3 10 3q�$��5q�$	��150��$��Add new task���� $��5q�$��	10 3 10 3q�$+��%uiCommands:TaskCommands.CmdDeleteTask��$��Delete task���� $��5q�$��	10 3 10 3q�$+��%uiCommands:TaskCommands.CmdModifyTask��$��Modify task���� --   $��TaskDescription��$ 10q�$	��150��$��WrapWithOverflow=�.���� $��Tasks���� -.   $��TeachersTab�� �� $ Vertical=�.+��� $��Teachers��$��20(�$��Georgia)�$��40��$ 10q�
��������� $��Center=�$ 
Horizontal=�.+��� .+��� $ 	19 15 5 0q�$��17(�List of all teachers :�� -/   $ TeachersList��$��5q�
�������$	 Single=�$
��Left=�$	��300��$��
20 20 0 20q�$	��250������ .R��� $ 
Horizontal=�.+��� $��0 0 5 0q����� *	FirstName+�� ���� *
MiddleName+�� $��20q�! �� Padding$! 
10 0 10 10q�._��  �� �� �� �� .+��� $	��200��$��0��$��Center=�.+��� -0   $��5q�$��0 37 0 5q�.��Add new teacher�� $��5q�$1��+uiCommands:TeacherCommands.CmdDeleteTeacher��$	��0 5q�.��Delete teacher�� $��5q�$1��+uiCommands:TeacherCommands.CmdModifyTeacher��$��0 5 0 10q�.��Modify teacher information�� $��1��$��0 15 0 0q�$ Vertical=�.+��� -1   $��TeacherName��$ 5 0 0 0q�$��WrapWithOverflow=�$	��200���� $��2��$ Vertical=�.+��� -2   $��TeacherSubjects��$��WrapWithOverflow=�$	��200��$ 5 0 0 0q��� $��3��$ Vertical=�.+��� -3   $��TeacherDescription��$��WrapWithOverflow=�$	��200��$ 5 0 0 0q�.���� $��Teachers���� $��1��#��Z��$��1q�$��2q�._��� ���� *SelectedItem.Tag+" �� ElementName$" GeneralMenu��# �� UpdateSourceTrigger$# PropertyChanged=�