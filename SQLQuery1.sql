CREATE TABLE Admin(
	AdminID int IDENTITY(1,1) PRIMARY KEY,
	AdminName varchar(100) NOT NULL,
	AdminMob int NOT NULL
);

CREATE TABLE Students(
	StudentID int IDENTITY(1,1) PRIMARY KEY,
	StudentName varchar(100),
	StudentAge int,
	StudentMob int,
	AdminID int,
	CONSTRAINT FK_STUDENT_ADMIN
	FOREIGN KEY (AdminID) REFERENCES Admin(AdminID)
);

CREATE PROCEDURE Admin_SelectAll
AS
BEGIN
	SELECT * FROM Admin
END;

CREATE PROCEDURE Admin_Insert
@adminName varchar(100),
@adminMob varchar(100)
AS
BEGIN
INSERT INTO Admin(AdminName,AdminMob) VALUES (@adminName, @adminMob)
END;

CREATE PROCEDURE Student_Insert
@studentName varchar(100),
@studentAge int,
@studentMob int,
@adminID int
AS
BEGIN
	INSERT INTO Students(StudentName,StudentAge,StudentMob,AdminID) VALUES (@studentName,@studentAge,@studentMob,@adminID)
END;

CREATE PROCEDURE Student_Update
@studentID int,
@studentName varchar(100),
@studentAge int,
@studentMob int,
@adminID int
AS
BEGIN
	UPDATE Students set StudentName=@studentName, StudentAge=@studentAge, StudentMob=@studentMob, AdminID=@adminID
	WHERE StudentID=@studentID
END;

CREATE PROCEDURE Student_SelectById
@studentID int
AS
BEGIN
	SELECT * FROM Students WHERE StudentID=@studentID
END;

CREATE PROCEDURE Student_SelectAll
AS
BEGIN
SELECT s.StudentID,s.StudentName,s.StudentAge,s.StudentMob,a.AdminID,a.AdminName,a.AdminMob
FROM Students s 
inner join Admin a
on s.AdminID=a.AdminID
END;

CREATE PROCEDURE Student_Delete
    @studentID INT
AS
BEGIN
    DELETE FROM Students WHERE StudentID=@studentID
END