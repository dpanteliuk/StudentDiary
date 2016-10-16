CREATE DATABASE StudentDiary
GO

USE StudentDiary
GO

CREATE TABLE tblUsers
(
	UserId int NOT NULL IDENTITY(1,1),
	[Login] varchar(30) NOT NULL, 
	[Password] varchar(50) NOT NULL,
	Name varchar(50) NOT NULL
	
	CONSTRAINT PK_tblUsers_UserId PRIMARY KEY(UserId),
	CONSTRAINT UQ_tblUsers_Login UNIQUE([Login])
)

CREATE TABLE tblTeachers
(
	TeacherId int NOT NULL IDENTITY(1,1),
	FirstName nvarchar(30) NOT NULL,
	MiddleName nvarchar(30) NOT NULL,
	LastName nvarchar(30) NOT NULL,
	TeacherDescription text NULL,
	
	CONSTRAINT PK_tblTeachers_Id PRIMARY KEY(TeacherId),
	CONSTRAINT UQ_tblTeachers_FirstName_MiddleName_LastName UNIQUE(FirstName,MiddleName,LastName)
)

CREATE TABLE tblSubject
(
	SubjectId int NOT NULL IDENTITY(1,1),
	Name nvarchar(50) NOT NULL,
	SubjectDescription text NULL,
	TeacherId int NULL,

	CONSTRAINT PK_tblSubject_SubjectId PRIMARY KEY(SubjectId),
	CONSTRAINT FK_tblSubject_tblTeachers FOREIGN KEY (TeacherId) REFERENCES tblTeachers(TeacherId)
										 ON DELETE SET NULL,
	CONSTRAINT UQ_tblSubject_Name UNIQUE(Name)
)

CREATE TABLE tblDaysOfTheWeek
(
	DayId int NOT NULL IDENTITY(1,1),
	Name nvarchar(10) NOT NULL,

	CONSTRAINT PK_tblDaysOfTheWeek_DayId PRIMARY KEY(DayId),
	CONSTRAINT UQ_tblDaysOfTheWeek_Name UNIQUE(Name),
	CONSTRAINT CK_tblDaysOfTheWeek_DayId CHECK (DayID<8)
)


-- Describes types of the week, like numerator or denominator
CREATE TABLE tblTypesOfWeek
(
	TypeId int NOT NULL IDENTITY(1,1),
	Name varchar(15) NOT NULL,

	CONSTRAINT PK_tblTypesOfWeek_TypeId PRIMARY KEY(TypeId),
	CONSTRAINT UQ_tblTypesOfWeek_Name UNIQUE(Name),
	CONSTRAINT CK_tblTypesOfWeek_TypeId CHECK (TypeId<5)
)

CREATE TABLE tblSemester
(
	SemesterNumber int NOT NULL,
	YearValue int NOT NULL,
	NumberOfWeeks int NOT NULL,
	StartDate date NOT NULL,
	StartPartOfWeekId int NOT NULL,

	CONSTRAINT PK_tblSemester_Year PRIMARY KEY(SemesterNumber, YearValue),
	CONSTRAINT FK_tblSemester_tblTypesOfWeek FOREIGN KEY (StartPartOfWeekId) REFERENCES tblTypesOfWeek(TypeId)
)

-- Contains information of such things like hometasks, course work, individual tasks
CREATE TABLE tblTasks
(
	TaskId int NOT NULL IDENTITY(1,1),
	TaskDescription text NOT NULL,
	SubjectId int NOT NULL,
	DeadLineDate date NOT NULL,
	TaskPriority int NOT NULL,

	CONSTRAINT PK_tblTasks_Id PRIMARY KEY (TaskId),
	CONSTRAINT FK_tblTasks_tblSubject FOREIGN KEY (SubjectId) REFERENCES tblSubject(SubjectId)
									  ON DELETE CASCADE
)

CREATE TABLE tblPairTimes
(
	PairNumber int NOT NULL IDENTITY(1,1),
	PairStart time NOT NULL,
	PairEnd time NOT NULL,

	CONSTRAINT PK_tblPairTimes_PairNumber PRIMARY KEY(PairNumber),
	CONSTRAINT CK_PairStartIsLessThenPairEnd CHECK (PairStart<PairEnd)
)

CREATE TABLE tblPairTypes
(
	TypeId int NOT NULL IDENTITY(1,1),
	TypeStringId varchar(5) NOT NULL,
	TypeName varchar(20) NOT NULL

	CONSTRAINT PK_tblPairTypes_TypeId PRIMARY KEY(TypeId),
	CONSTRAINT UQ_tblPairTypes_TypeName UNIQUE(TypeName),
	CONSTRAINT UQ_tblPairTypes_TypeStringId UNIQUE(TypeStringId)
)

CREATE TABLE tblSchedule
(
	Id int NOT NULL IDENTITY(1,1),
	DayId int NOT NULL,
	SubjectId int NULL,
	PairNumber int NOT NULL,
	WeekTypeId int NOT NULL,
	WeekNumber int NOT NULL,
	YearValue int NOT NULL,
	SemesterNumber int NOT NULL,
	PairTypeId int NOT NULL,
	Modified bit NOT NULL DEFAULT 0

	CONSTRAINT PK_tblSchedule_Id PRIMARY KEY(Id),
	CONSTRAINT FK_tblSchedule_tblDaysOfTheWeek FOREIGN KEY (DayId) REFERENCES tblDaysOfTheWeek(DayId),
	CONSTRAINT FK_tblSchedule_tblSubject FOREIGN KEY (SubjectId) REFERENCES tblSubject(SubjectId)
										 ON DELETE CASCADE,
	CONSTRAINT FK_tblSchedule_tblTypesOfWeek FOREIGN KEY (WeekTypeId) REFERENCES tblTypesOfWeek(TypeId),
	CONSTRAINT FK_tblSchedule_tblSemester_Year_Semester FOREIGN KEY (SemesterNumber,YearValue) REFERENCES tblSemester(SemesterNumber,YearValue)
														ON DELETE CASCADE,
	CONSTRAINT UQ_tblSchedule_DayId_SubjectId_WeekNumber_Year UNIQUE(DayId,SubjectId,WeekNumber,YearValue,PairNumber),
	CONSTRAINT FK_tblSchedule_tbl_PairTimes_PairNumber FOREIGN KEY (PairNumber) REFERENCES tblPairTimes(PairNumber),
	CONSTRAINT FK_tblSchedule_tblPairTypes FOREIGN KEY (PairTypeId) REFERENCES tblPairTypes (TypeId)
)