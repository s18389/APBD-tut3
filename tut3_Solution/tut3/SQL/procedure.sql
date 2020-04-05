CREATE PROCEDURE PromoteStudents (@StudiesName VARCHAR(100),@Semester INT)
AS

BEGIN
	SET XACT_ABORT ON;
	BEGIN TRAN

	DECLARE @IdStudies INT = (SELECT IdStudy FROM Studies WHERE [Name]=@StudiesName);
	IF @IdStudies IS NULL
	BEGIN
		RAISERROR(1,1, 1020);
	END
	
	DECLARE @IdNextEnrollment INT = (SELECT IdEnrollment FROM Enrollment WHERE IdStudy=@IdStudies AND Semester = @Semester + 1);

	DECLARE @newIDEnrollment INT = (SELECT MAX(IdEnrollment) FROM Enrollment) + 1;

	IF @IdNextEnrollment IS NULL
	BEGIN
		INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (@newIDEnrollment, @Semester + 1, @IdStudies, GETDATE());
	END
	

	UPDATE Student
	SET Student.IdEnrollment = @IdNextEnrollment
	WHERE IndexNumber=(SELECT Student.IndexNumber FROM Student, Enrollment WHERE Enrollment.IdEnrollment = Student.IdEnrollment AND Enrollment.Semester = @Semester AND Student.IdEnrollment = @IdNextEnrollment -1);
	COMMIT
END;