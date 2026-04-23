CREATE OR ALTER PROCEDURE sp_UpdateStudent
    @Id INT,
    @Name NVARCHAR(100) = NULL,
    @Age INT = 0,
    @Email NVARCHAR(100) = NULL,
    @PhoneNumber NVARCHAR(20) = NULL
AS
BEGIN
    UPDATE Students
    SET 
        Name = ISNULL(NULLIF(LTRIM(RTRIM(@Name)), ''), Name), 
        Age = CASE WHEN @Age > 0 THEN @Age ELSE Age END, 
        Email = ISNULL(NULLIF(LTRIM(RTRIM(@Email)), ''), Email), 
        PhoneNumber = ISNULL(NULLIF(LTRIM(RTRIM(@PhoneNumber)), ''), PhoneNumber)
    WHERE Id = @Id
END