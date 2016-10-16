USE StudentDiary
GO

CREATE PROCEDURE spGetAllTeachers
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM tblTeachers;
END;
GO

CREATE PROCEDURE spGetTeacher
	@teacherId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT *  
	FROM tblTeachers
	WHERE TeacherId = @teacherId;
END;
GO

CREATE PROCEDURE spGetSubjectsByTeacherId
	@teacherId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * 
	FROM tblSubject
	JOIN tblTeachers ON tblSubject.TeacherId = tblTeachers.TeacherId
	WHERE tblTeachers.TeacherId = @teacherId
END;
GO

CREATE PROCEDURE spDeleteTeacher
	@teacherId int
AS
BEGIN
	DELETE tblTeachers 
	WHERE TeacherId = @teacherId;
END;
GO

CREATE PROCEDURE spAddTeacher
	@FirstName nvarchar(30),
	@MiddleName nvarchar(30),
	@LastName nvarchar(30),
	@Description text
AS
BEGIN
	IF(EXISTS(SELECT * FROM tblTeachers WHERE FirstName=@FirstName AND MiddleName=@MiddleName AND LastName=@LastName))
		THROW 50000, 'Teacher with that name already exists!', 1;
	IF(CAST(@description as nvarchar(5))!='')
		INSERT tblTeachers VALUES (@FirstName,@MiddleName,@LastName,@description)
	ELSE
		INSERT tblTeachers VALUES (@FirstName,@MiddleName,@LastName,NULL)
	SELECT SCOPE_IDENTITY();
END;
GO

CREATE PROCEDURE spModifyTeacher
	@TeacherId int,
	@ModifiedFirstName nvarchar(30),
	@ModifiedMiddleName nvarchar(30),
	@ModifiedLastName nvarchar(30),
	@ModifiedDescription text
AS
BEGIN
	IF(NOT EXISTS(SELECT TOP(1) * FROM tblTeachers WHERE TeacherId = @TeacherId))
		THROW 50001, 'There is no teacher in database with that Id', 1;
	UPDATE tblTeachers
	SET FirstName = @ModifiedFirstName,MiddleName = @ModifiedMiddleName,LastName = @ModifiedLastName,TeacherDescription = @ModifiedDescription
	WHERE TeacherId = @TeacherId
END;
GO

CREATE FUNCTION fnCheckSubjectsExistInCertainDay
(
	@dayName varchar(15),
	@year int,
	@semesterNumber int,
	@weekNumber int
)
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT;
	IF EXISTS
	(
		SELECT * FROM tblSchedule
		WHERE DayId = (SELECT DayId FROM tblDaysOfTheWeek WHERE Name = @dayName) AND
			  YearValue = @year AND
			  SemesterNumber = @semesterNumber AND
			  WeekNumber = @weekNumber AND
			  SubjectId IS NOT NULL
	)
		SET @result = 1;
	ELSE
		SET @result = 0;
	RETURN @result;
	END
GO

CREATE PROCEDURE spGetSubjectsInCertainDay
	@dayName varchar(15),
	@year int,
	@semesterNumber int,
	@weekNumber int
AS
BEGIN
	SET NOCOUNT ON;

	IF dbo.fnCheckSubjectsExistInCertainDay(@dayName,@year,@semesterNumber,@weekNumber) = 1
		SELECT * 
		FROM tblSubject
		RIGHT JOIN tblSchedule ON tblSchedule.SubjectId = tblSubject.SubjectId
		JOIN tblDaysOfTheWeek ON tblDaysOfTheWeek.DayId = tblSchedule.DayId
		JOIN tblPairTypes ON tblSchedule.PairTypeId = tblPairTypes.TypeId
		WHERE tblDaysOfTheWeek.Name = @dayName 
			AND YearValue = @year
			AND SemesterNumber = @semesterNumber
			AND WeekNumber = @weekNumber;
	ELSE
	BEGIN
		IF @weekNumber > (SELECT NumberOfWeeks FROM tblSemester WHERE YearValue=@year AND SemesterNumber=@semesterNumber)
			THROW 50003, 'Week number is out of allowed range',1;
		
		DECLARE	@weekCounter int;
		SET @weekCounter = @weekNumber-2;
		WHILE 1=1
		BEGIN
			IF dbo.fnCheckSubjectsExistInCertainDay(@dayName,@year,@semesterNumber,@weekCounter) = 0
				BEGIN
				SET @weekCounter = @weekCounter - 2;
				IF @weekCounter < 1
					BREAK;
				END;
			ELSE 
			BEGIN
				INSERT INTO tblSchedule 
					SELECT DayId,SubjectId,PairNumber,WeekTypeId,@weekNumber AS WeekNumber,YearValue,SemesterNumber,PairTypeId,Modified 
					FROM tblSchedule WHERE YearValue = @year 
						AND SemesterNumber = @semesterNumber 
						AND WeekNumber = @weekCounter 
						AND DayId = (SELECT DayId FROM tblDaysOfTheWeek WHERE Name = @dayName)
				EXEC dbo.spGetSubjectsInCertainDay @dayName, @year, @semesterNumber, @weekNumber;
				BREAK;
			END
		END
	END
END
GO

CREATE PROCEDURE spGetWeekDays
AS
	SELECT * FROM tblDaysOfTheWeek
GO

CREATE PROCEDURE spAllPairTimes
AS
	SELECT * FROM tblPairTimes
GO

CREATE PROCEDURE spGetAllSubjects
AS
	SELECT * FROM tblSubject
GO

CREATE PROCEDURE spGetSubjectById
	@subjectId int
AS
	SELECT * FROM tblSubject WHERE SubjectId = @subjectId;
GO

CREATE PROCEDURE spGetAllTasks
AS
	SELECT * FROM tblTasks
GO

CREATE PROCEDURE spGetSemester
	@YearValue int,
	@SemesterNumber int
AS
	SELECT * FROM tblSemester WHERE SemesterNumber = @SemesterNumber AND YearValue = @YearValue
GO

CREATE PROCEDURE spRemoveSubjectsInCertainDay
	@dayName varchar(15),
	@year int,
	@semesterNumber int,
	@weekNumber int
AS
BEGIN
	UPDATE tblSchedule
	SET SubjectId = NULL,Modified = 1 WHERE DayId=(SELECT DayId FROM tblDaysOfTheWeek WHERE Name = @dayName) AND YearValue=@year AND SemesterNumber=@semesterNumber AND WeekNumber = @weekNumber;
END;
GO

CREATE PROCEDURE spRemoveCertainSubject
    @year int,
    @semesterNumber int,
    @weekNumber int,
    @dayName varchar(15),
    @pairNumber int
AS
BEGIN
	UPDATE tblSchedule
	SET SubjectId = NULL,Modified = 1 
	WHERE DayId=(SELECT DayId FROM tblDaysOfTheWeek WHERE Name = @dayName) 
		  AND YearValue=@year 
		  AND SemesterNumber=@semesterNumber 
		  AND WeekNumber = @weekNumber
		  AND PairNumber = @pairNumber;
END;
GO

CREATE PROCEDURE spGetAllPairTypes
AS
BEGIN
	SELECT * FROM tblPairTypes;
END;
GO

CREATE PROCEDURE spAddModifyCertainSubject
    @year int,
    @semesterNumber int,
    @weekNumber int,
    @dayName varchar(15),
    @pairNumber int,
	@selectedSubjectId int,
	@selectedSubjectPairTypeId int
AS
BEGIN
	DECLARE @dayId int = (SELECT DayId FROM tblDaysOfTheWeek WHERE Name = @dayName);
	IF EXISTS
	(SELECT * FROM tblSchedule 
	 WHERE YearValue = @year	
		AND SemesterNumber = @semesterNumber
		AND WeekNumber = @weekNumber
		AND DayId = @dayId
		AND PairNumber = @pairNumber)
		UPDATE tblSchedule
			SET SubjectId = @selectedSubjectId, PairTypeId = @selectedSubjectPairTypeId
			WHERE YearValue = @year	
			AND SemesterNumber = @semesterNumber
			AND WeekNumber = @weekNumber
			AND DayId = @dayId
			AND PairNumber = @pairNumber
	ELSE
		INSERT INTO tblSchedule VALUES
		(@dayId,@selectedSubjectId,@pairNumber,1,@weekNumber,@year,@semesterNumber,@selectedSubjectPairTypeId,1) --weektypeid - problem
END;
GO

--CREATE PROCEDURE spModifyCertainSubject
--    @year int,
--    @semesterNumber int,
--    @weekNumber int,
--    @dayName varchar(15),
--    @pairNumber int,
--	@selectedSubjectId int,
--	@selectedSubjectPairTypeId int
--AS
--BEGIN
--	DECLARE @dayId int = (SELECT DayId FROM tblDaysOfTheWeek WHERE Name = @dayName);
--	UPDATE tblSchedule
--		SET SubjectId = @selectedSubjectId, PairTypeId = @selectedSubjectPairTypeId, Modified = 1
--		WHERE YearValue = @year	
--		AND SemesterNumber = @semesterNumber
--		AND WeekNumber = @weekNumber
--		AND DayId = @dayId
--		AND PairNumber = @pairNumber
--END;
--GO

CREATE PROCEDURE spDeleteTaskById
	@taskId int
AS
	DELETE tblTasks WHERE TaskId = @taskId;
GO

CREATE PROCEDURE spModifyTask
	@taskId int,
	@selectedDate Date,
	@selectedTaskDescription text,
	@selectedTaskPriority int,
	@selectedSubjectId int
AS
	UPDATE tblTasks 
		SET TaskDescription = @selectedTaskDescription,
			SubjectId = @selectedSubjectId,
			DeadLineDate = @selectedDate,
			TaskPriority = @selectedTaskPriority
		WHERE TaskId = @taskId
GO

CREATE PROCEDURE spAddTask
    @subjectId int,
    @taskPriority int,
    @deadline Date,
    @description text
AS
BEGIN
	SET NOCOUNT ON;
	INSERT tblTasks VALUES (@description,@subjectId,@deadline,@taskPriority);
	SELECT SCOPE_IDENTITY();
END;
GO

CREATE PROCEDURE spGetAllTasksBySubjectId
	@subjectId int
AS
	SELECT * FROM tblTasks WHERE SubjectId = @subjectId;
GO

CREATE PROCEDURE spDeleteSubjectById
	@subjectId int
AS
	DELETE tblSubject 
	WHERE SubjectId = @subjectId
GO

CREATE PROCEDURE spAddNewSubject
	@subjectName varchar(50),
    @teacherId int,
    @description text
AS
	IF(EXISTS(SELECT * FROM tblSubject WHERE Name = @subjectName))
		THROW 50005, 'Subject with that name already exists!', 1;
	INSERT tblSubject VALUES (@subjectName,@description,@teacherId);
	SELECT SCOPE_IDENTITY()
GO

CREATE PROCEDURE spModifySubject
	@id int,
	@subjectName varchar(50),
    @teacherId int,
    @description text
AS
	UPDATE tblSubject
	SET Name = @subjectName,
		SubjectDescription = @description,
		TeacherId = @teacherId
	WHERE SubjectId = @id;
GO

CREATE PROCEDURE spGetAllSemesters
AS
	SELECT * FROM tblSemester;
GO

CREATE PROCEDURE spAddNewSemester
    @year int,
    @number int,
    @startDate date,
    @numberOfWeeks int,
    @startPartOfWeek int
AS
	INSERT tblSemester VALUES (@number,@year,@numberOfWeeks,@startDate,@startPartOfWeek)
GO

CREATE PROCEDURE spDeleteSemester
    @year int,
    @number int
AS
	DELETE tblSemester WHERE YearValue = @year AND SemesterNumber = @number
GO

CREATE PROCEDURE spAdjustPairTime
    @pairNumber int,
    @pairStart time,
    @pairEnd time
AS
	UPDATE tblPairTimes SET PairStart = @pairStart,PairEnd = @pairEnd
	WHERE PairNumber = @pairNumber;
GO

CREATE PROCEDURE spLoginUser
    @login varchar(50),
    @pass varchar(50)
AS
	SELECT * FROM tblUsers 
	WHERE [Login] = @login AND [Password] = @pass;
GO