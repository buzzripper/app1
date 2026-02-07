INSERT INTO dbo.Patient (Id, LastName, FirstName, Email) VALUES
('11111111-1111-1111-1111-111111111111','Smith','John','john.smith@test.local'),
('22222222-2222-2222-2222-222222222222','Johnson','Mary','mary.johnson@test.local'),
('33333333-3333-3333-3333-333333333333','Williams','Robert','robert.williams@test.local'),
('44444444-4444-4444-4444-444444444444','Brown','Patricia','patricia.brown@test.local'),
('55555555-5555-5555-5555-555555555555','Jones','Michael','michael.jones@test.local'),
('66666666-6666-6666-6666-666666666666','Garcia','Linda','linda.garcia@test.local'),
('77777777-7777-7777-7777-777777777777','Miller','William','william.miller@test.local'),
('88888888-8888-8888-8888-888888888888','Davis','Elizabeth','elizabeth.davis@test.local'),
('99999999-9999-9999-9999-999999999999','Rodriguez','David','david.rodriguez@test.local'),
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa','Martinez','Barbara','barbara.martinez@test.local'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb','Hernandez','James','james.hernandez@test.local'),
('cccccccc-cccc-cccc-cccc-cccccccccccc','Lopez','Susan','susan.lopez@test.local'),
('dddddddd-dddd-dddd-dddd-dddddddddddd','Gonzalez','Joseph','joseph.gonzalez@test.local'),
('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee','Wilson','Jessica','jessica.wilson@test.local'),
('ffffffff-ffff-ffff-ffff-ffffffffffff','Anderson','Thomas','thomas.anderson@test.local'),
('12121212-1212-1212-1212-121212121212','Taylor','Sarah','sarah.taylor@test.local'),
('13131313-1313-1313-1313-131313131313','Moore','Charles','charles.moore@test.local'),
('14141414-1414-1414-1414-141414141414','Jackson','Karen','karen.jackson@test.local'),
('15151515-1515-1515-1515-151515151515','Martin','Daniel','daniel.martin@test.local'),
('16161616-1616-1616-1616-161616161616','Lee','Nancy','nancy.lee@test.local');


SET NOCOUNT ON;

-- Insert 0–5 invoices per patient
INSERT INTO dbo.Invoice (Id, PersonId, Amount, Memo, PatientId)
SELECT
    NEWID() AS Id,
    NEWID() AS PersonId,
    CAST(10 + (ABS(CHECKSUM(NEWID())) % 50000) / 100.0 AS decimal(18,2)) AS Amount,
    CONCAT('Test invoice ', v.n) AS Memo,
    p.Id AS PatientId
FROM dbo.Patient p
CROSS APPLY
(
    -- Random number of invoices per patient: 0–5
    SELECT TOP (ABS(CHECKSUM(NEWID())) % 6)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM (VALUES (1),(2),(3),(4),(5),(6)) x(n)
) v;
