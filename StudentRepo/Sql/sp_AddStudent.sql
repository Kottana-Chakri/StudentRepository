CREATE OR ALTER PROCEDURE sp_AddStudent
    @Name NVARCHAR(100),
    @Age INT,
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(20)
AS
BEGIN
    INSERT INTO Students(Name, Age, Email, PhoneNumber)
    VALUES(@Name, @Age, @Email, @PhoneNumber)
END