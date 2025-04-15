ALTER TABLE [Project]
ADD StartDate DATETIME NULL, EndDate DATETIME NULL

ALTER TABLE [Project]
ADD CONSTRAINT CHK_Project_EndDate_After_StartDate
CHECK (EndDate >= StartDate);