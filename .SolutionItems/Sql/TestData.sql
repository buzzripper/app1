-- =============================================================================
-- Test Data: 20 Patients with 0-5 Invoices each
-- =============================================================================

-- Clear existing data
DELETE FROM [dbo].[Invoice];
DELETE FROM [dbo].[Patient];
GO

-- Patient GUIDs
DECLARE @p1  UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000001';
DECLARE @p2  UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000002';
DECLARE @p3  UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000003';
DECLARE @p4  UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000004';
DECLARE @p5  UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000005';
DECLARE @p6  UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000006';
DECLARE @p7  UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000007';
DECLARE @p8  UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000008';
DECLARE @p9  UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000009';
DECLARE @p10 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000010';
DECLARE @p11 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000011';
DECLARE @p12 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000012';
DECLARE @p13 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000013';
DECLARE @p14 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000014';
DECLARE @p15 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000015';
DECLARE @p16 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000016';
DECLARE @p17 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000017';
DECLARE @p18 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000018';
DECLARE @p19 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000019';
DECLARE @p20 UNIQUEIDENTIFIER = 'A1000001-0000-0000-0000-000000000020';

-- =====================
-- Insert Patients
-- =====================
INSERT INTO [dbo].[Patient] ([Id], [LastName], [FirstName], [Email], [IsActive]) VALUES
(@p1,  'Smith',     'John',      'john.smith@email.com',       1),
(@p2,  'Johnson',   'Emily',     'emily.johnson@email.com',    1),
(@p3,  'Williams',  'Michael',   NULL,                         1),
(@p4,  'Brown',     'Sarah',     'sarah.brown@email.com',      0),
(@p5,  'Jones',     'David',     'david.jones@email.com',      1),
(@p6,  'Garcia',    'Maria',     NULL,                         1),
(@p7,  'Miller',    'James',     'james.miller@email.com',     1),
(@p8,  'Davis',     'Jennifer',  'jennifer.davis@email.com',   0),
(@p9,  'Rodriguez', 'Robert',    NULL,                         1),
(@p10, 'Martinez',  'Linda',     'linda.martinez@email.com',   1),
(@p11, 'Hernandez', 'William',   'william.h@email.com',        0),
(@p12, 'Lopez',     'Elizabeth',  NULL,                         1),
(@p13, 'Gonzalez',  'Richard',   'richard.g@email.com',        1),
(@p14, 'Wilson',    'Barbara',   'barbara.wilson@email.com',   1),
(@p15, 'Anderson',  'Joseph',    NULL,                         0),
(@p16, 'Thomas',    'Susan',     'susan.thomas@email.com',     1),
(@p17, 'Taylor',    'Charles',   'charles.taylor@email.com',   1),
(@p18, 'Moore',     'Jessica',   NULL,                         1),
(@p19, 'Jackson',   'Thomas',    'thomas.jackson@email.com',   0),
(@p20, 'Martin',    'Karen',     'karen.martin@email.com',     1);

-- =====================
-- Insert Invoices
-- =====================

-- Patient 1 (Smith, John) - 3 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p1, 150.00, 'Initial consultation',          @p1),
(NEWID(), @p1, 75.50,  'Follow-up visit',               @p1),
(NEWID(), @p1, 220.00, 'Lab work - blood panel',        @p1);

-- Patient 2 (Johnson, Emily) - 5 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p2, 300.00, 'Annual physical exam',           @p2),
(NEWID(), @p2, 45.00,  'Prescription refill consult',    @p2),
(NEWID(), @p2, 1250.75,'MRI scan',                       @p2),
(NEWID(), @p2, 85.00,  'Follow-up appointment',          @p2),
(NEWID(), @p2, 60.00,  'Telehealth visit',               @p2);

-- Patient 3 (Williams, Michael) - 0 invoices

-- Patient 4 (Brown, Sarah) - 1 invoice
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p4, 500.00, 'Emergency room visit',           @p4);

-- Patient 5 (Jones, David) - 2 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p5, 175.00, 'Dental cleaning',                @p5),
(NEWID(), @p5, 950.00, 'Root canal procedure',           @p5);

-- Patient 6 (Garcia, Maria) - 4 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p6, 200.00, 'Specialist referral visit',      @p6),
(NEWID(), @p6, 125.00, 'X-ray imaging',                  @p6),
(NEWID(), @p6, 80.00,  'Physical therapy session',       @p6),
(NEWID(), @p6, 80.00,  'Physical therapy session 2',     @p6);

-- Patient 7 (Miller, James) - 0 invoices

-- Patient 8 (Davis, Jennifer) - 2 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p8, 350.00, 'Outpatient surgery consult',     @p8),
(NEWID(), @p8, 4500.00,'Outpatient surgery',             @p8);

-- Patient 9 (Rodriguez, Robert) - 1 invoice
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p9, 90.00,  'Routine checkup',                @p9);

-- Patient 10 (Martinez, Linda) - 3 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p10, 275.00, 'Dermatology consultation',      @p10),
(NEWID(), @p10, 150.00, 'Skin biopsy',                   @p10),
(NEWID(), @p10, 60.00,  'Follow-up dermatology',         @p10);

-- Patient 11 (Hernandez, William) - 5 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p11, 100.00, 'Urgent care visit',             @p11),
(NEWID(), @p11, 55.00,  'Flu shot administration',       @p11),
(NEWID(), @p11, 200.00, 'Cardiology referral',           @p11),
(NEWID(), @p11, 800.00, 'Stress test',                   @p11),
(NEWID(), @p11, 125.00, 'Cardiology follow-up',          @p11);

-- Patient 12 (Lopez, Elizabeth) - 0 invoices

-- Patient 13 (Gonzalez, Richard) - 2 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p13, 185.00, 'Orthopedic evaluation',         @p13),
(NEWID(), @p13, 3200.00,'Knee arthroscopy',              @p13);

-- Patient 14 (Wilson, Barbara) - 4 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p14, 95.00,  'Eye exam',                      @p14),
(NEWID(), @p14, 250.00, 'Contact lens fitting',          @p14),
(NEWID(), @p14, 400.00, 'Allergy testing panel',         @p14),
(NEWID(), @p14, 70.00,  'Allergy follow-up',             @p14);

-- Patient 15 (Anderson, Joseph) - 1 invoice
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p15, 600.00, 'Sleep study',                   @p15);

-- Patient 16 (Thomas, Susan) - 3 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p16, 110.00, 'Well-woman exam',               @p16),
(NEWID(), @p16, 325.00, 'Ultrasound imaging',            @p16),
(NEWID(), @p16, 50.00,  'Lab work - basic metabolic',    @p16);

-- Patient 17 (Taylor, Charles) - 0 invoices

-- Patient 18 (Moore, Jessica) - 2 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p18, 175.00, 'Prenatal visit',                @p18),
(NEWID(), @p18, 175.00, 'Prenatal follow-up',            @p18);

-- Patient 19 (Jackson, Thomas) - 5 invoices
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p19, 425.00, 'ENT consultation',              @p19),
(NEWID(), @p19, 1800.00,'Sinus surgery',                 @p19),
(NEWID(), @p19, 90.00,  'Post-op follow-up 1',           @p19),
(NEWID(), @p19, 90.00,  'Post-op follow-up 2',           @p19),
(NEWID(), @p19, 65.00,  'Final post-op clearance',       @p19);

-- Patient 20 (Martin, Karen) - 1 invoice
INSERT INTO [dbo].[Invoice] ([Id], [PersonId], [Amount], [Memo], [PatientId]) VALUES
(NEWID(), @p20, 140.00, 'Nutrition counseling session',  @p20);

GO
