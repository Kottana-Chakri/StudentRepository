CREATE OR ALTER PROCEDURE sp_AddStudent
    @Name NVARCHAR(100),
    @Age INT
AS
BEGIN
    INSERT INTO Students(Name, Age)
    VALUES(@Name, @Age)
END