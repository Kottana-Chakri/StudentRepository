CREATE OR ALTER PROCEDURE sp_UpdateStudent
    @Id INT,
    @Name NVARCHAR(100),
    @Age INT
AS
BEGIN
    UPDATE Students
    SET Name = @Name, Age = @Age
    WHERE Id = @Id
END