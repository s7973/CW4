﻿create procedure PromoteStudents 
@STUDIES NVARCHAR(100), 
@SEMESTER INT
AS
BEGIN
	BEGIN TRANSACTION
	DECLARE @IDSTUDY INT = (SELECT IDSTUDY FROM STUDIES WHERE NAME = @STUDIES)
	DECLARE @CHECKENROLLMENT INT = (SELECT IDENROLLMENT FROM ENROLLMENT E, STUDIES S WHERE E.IDSTUDY = S.IDSTUDY AND S.NAME = @STUDIES AND SEMESTER = (@SEMESTER + 1));
	IF @CHECKENROLLMENT IS NULL
	BEGIN
		INSERT INTO ENROLLMENT (IDENROLLMENT, SEMESTER, IDSTUDY, STARTDATE) VALUES ('30', (@SEMESTER + 1), @IDSTUDY, GETDATE());
	END;
	DECLARE @IDENROLLMENT INT = (SELECT IDENROLLMENT FROM ENROLLMENT E, STUDIES S WHERE E.IDSTUDY = S.IDSTUDY AND S.NAME = @STUDIES AND SEMESTER = (@SEMESTER + 1));
	UPDATE STUDENT SET IDENROLLMENT = @IDENROLLMENT;
	COMMIT
END;