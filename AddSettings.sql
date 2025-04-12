USE AdsGeoDB;
GO

CREATE PROCEDURE AddSetting
    @SettingName NVARCHAR(100),
    @Value NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM Settings
        WHERE SettingName = @SettingName AND Value = @Value
    )
    BEGIN
   INSERT INTO Settings(SettingName, Value,CreateDate,CreateBy,IsActive)
        VALUES (@SettingName, @Value,GETDATE(),0,1);
    END
END;
GO
