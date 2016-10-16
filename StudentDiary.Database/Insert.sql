USE StudentDiary
GO

SET IDENTITY_INSERT tblTeachers ON
INSERT INTO tblTeachers (TeacherId, FirstName, MiddleName, LastName, TeacherDescription)
	VALUES
	(1, 'Мар`ян', 'Євгенович', 'Іванов' , 'Строгий, але справедливий'),
	(2, 'Антон', 'Володимирович', 'Осипов', 'Любить жартувати'),
	(3, 'Катерина', 'Ігорівна', 'Щербакова', 'Часто запізнюється'),
	(4, 'Дар`я', 'Станіславівна', 'Сороківна', 'На парі краще не розмовляти'),
	(5, 'Давид', 'Аркадієвич', 'Жуков', ''),
	(6, 'Афанасій', 'Олегович', 'Русін', 'Часто викликає до дошки'),
	(7, 'Валентин', 'Вікторович', 'Соколов', 'Говорить монотонно'),
	(8, 'Любов', 'Тарасівна', 'Кузьміна', ''),
	(9, 'Яна', 'Василівна', 'Петрів', ''),
	(10, 'Антон', 'Килилович', 'Сикінов', '')
SET IDENTITY_INSERT tblTeachers OFF
GO

SET IDENTITY_INSERT tblSubject ON
INSERT INTO tblSubject (SubjectId, Name, SubjectDescription, TeacherId)
	VALUES
	(1, 'Основи охорони праці', 'ЛК/ПР 18ауд.',1),
	(2, 'Квантова механіка', '18 ауд. в Понеділок, 11 ауд. у Вівторок',2),
	(3, 'Філософія', 'Семінар в 12 ауд. Можна користуватись ел. пристроями',3),
	(4, 'Термодинаміка і стат. фізика', 'Тільки ПР, пари іноді в 19 ауд.',4),
	(5, 'Історія української культури', 'ЛК/ПР 12ауд.',1),
	(6, 'Фізичне матеріалознавство', 'ЛК 15ауд.',2),
	(7, 'Квант. стат. фізика', 'Тільки ЛБ в 11 ауд.',3),
	(8, 'Квантова електрон. і нелінійн.опт', 'Обов`язково мати з собою методичку',5),
	(9, 'Логіка','Присутність не обов`язкова',6),
	(10,'Суперсиметрія у квантовій механіці','Цей предмет часто ведуть практиканти',7)
SET IDENTITY_INSERT tblSubject OFF
GO

SET IDENTITY_INSERT tblTypesOfWeek ON
INSERT INTO tblTypesOfWeek (TypeId,Name)
	VALUES
	(1,'Numerator'),
	(2,'Denominator')
SET IDENTITY_INSERT tblTypesOfWeek OFF
GO

SET IDENTITY_INSERT tblDaysOfTheWeek ON
INSERT INTO tblDaysOfTheWeek (DayId,Name)
	VALUES
	(1,'Monday'),
	(2,'Tuesday'),
	(3,'Wednesday'),
	(4,'Thursday'),
	(5,'Friday'),
	(6,'Saturday'),
	(7,'Sunday')
SET IDENTITY_INSERT tblDaysOfTheWeek OFF
GO

INSERT INTO tblSemester (SemesterNumber, YearValue,	NumberOfWeeks, StartDate, StartPartOfWeekId)
	VALUES
	(1,2016, 18,'20160901',1)
GO

SET IDENTITY_INSERT tblPairTimes ON
INSERT INTO tblPairTimes(PairNumber,PairStart,PairEnd)
	VALUES
	(1,'8:30:00','9:50:00'),
	(2,'10:10:00','11:30:00'),
	(3,'11:50:00','13:10:00'),
	(4,'13:30:00','14:50:00'),
	(5,'15:05:00','16:25:00'),
	(6,'16:40:00','18:00:00'),
	(7,'18:10:00','19:30:00'),
	(8,'19:40:00','21:00:00')
SET IDENTITY_INSERT tblPairTimes OFF
GO

SET IDENTITY_INSERT tblPairTypes ON
INSERT INTO tblPairTypes(TypeId,TypeStringId,TypeName)
	VALUES
	(1,'ЛК','Лекція'),
	(2,'ПР','Практика'),
	(3,'ЛБ','Лабораторна'),
	(4,'СЕМ','Семінар')
SET IDENTITY_INSERT tblPairTypes OFF
GO

SET IDENTITY_INSERT tblSchedule ON
INSERT INTO tblSchedule(Id, DayId, SubjectId, PairNumber, WeekTypeId, WeekNumber, YearValue, SemesterNumber,PairTypeId)
	VALUES
	(1,4,8,2,1,1,2016,1,1),
	(2,4,9,3,1,1,2016,1,1),
	(3,4,6,4,1,1,2016,1,3),
	(4,4,8,5,1,1,2016,1,3),
	(5,5,4,3,1,1,2016,1,1),
	(6,1,1,3,2,2,2016,1,1),
	(7,1,2,2,2,2,2016,1,2),
	(8,2,3,2,2,2,2016,1,4),
	(9,2,2,3,2,2,2016,1,1),
	(10,2,4,4,2,2,2016,1,2),
	(11,3,5,1,2,2,2016,1,1),
	(12,3,6,2,2,2,2016,1,1),
	(13,3,3,3,2,2,2016,1,1),
	(14,4,8,2,2,2,2016,1,1),
	(15,4,10,3,2,2,2016,1,1),
	(16,4,10,4,2,2,2016,1,3),
	(17,5,4,3,2,2,2016,1,1),
	(18,1,1,3,1,3,2016,1,2),
	(19,1,2,2,1,3,2016,1,2),
	(20,2,3,2,1,3,2016,1,4),
	(21,2,2,3,1,3,2016,1,1),
	(22,2,4,4,1,3,2016,1,2),
	(23,3,5,1,1,3,2016,1,2),
	(24,3,4,2,1,3,2016,1,1),
	(25,3,7,3,1,3,2016,1,3)
SET IDENTITY_INSERT tblSchedule OFF
GO

SET IDENTITY_INSERT tblTasks ON
INSERT INTO tblTasks(TaskId,TaskDescription,SubjectId,DeadLineDate,TaskPriority)
	VALUES
	(1,'Опрацювати лекційний матеріал',1,'20161002',1),
	(2,'Виорішити 1б,2с,3г на стор.123',2,'20161002',2),
	(3,'Підготуватись до 3 лабораторної',7,'20161005',10),
	(4,'Взяти з собою зошит для лекцій',8,'20161006',2)
SET IDENTITY_INSERT tblTasks OFF
GO

SET IDENTITY_INSERT tblUsers ON
INSERT INTO tblUsers(UserId,[Login],[Password],Name)
	VALUES
	(1,'user1','827ccb0eea8a706c4c34a16891f84e7b','Rostyslav'),    --12345
	(2,'user2','827ccb0eea8a706c4c34a16891f84e7b','Ihor')	--12345
SET IDENTITY_INSERT tblUsers OFF
GO
