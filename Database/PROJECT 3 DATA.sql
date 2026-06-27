USE hospital_management_system;

INSERT INTO Users (Username, PasswordHash, Role, IsActive)
VALUES ('AAA', '206714', 'Admin', 1);
USE hospital_management_system;
-- DEPARTMENT
INSERT INTO Departments (DeptName, Location, ContactNumber) VALUES
('Cardiology', 'Block A', '0300-1111111'),
('Neurology', 'Block B', '0300-2222222'),
('ENT', 'Block C', '0300-3333333'),
('Orthopedics', 'Block D', '0300-4444444'),
('Pediatrics', 'Block E', '0300-5555555'),
('Gynecology', 'Block F', '0300-6666666'),
('General Surgery', 'Block G', '0300-7777777'),
('Dermatology', 'Block H', '0300-8888888'),
('Radiology', 'Block I', '0300-9999999'),
('Emergency', 'Block J', '0300-0000000');

-- Doctors
INSERT INTO Doctors (FirstName, LastName, Specialization, DeptID, ContactNumber, Email, Salary) VALUES
('Ahmed', 'Khan', 'Cardiologist', 1, '03001234567', 'ahmed.khan@hospital.com', 150000),
('Sara', 'Ali', 'Neurologist', 2, '03002345678', 'sara.ali@hospital.com', 140000),
('Usman', 'Malik', 'ENT Specialist', 3, '03003456789', 'usman.malik@hospital.com', 130000),
('Fatima', 'Raza', 'Orthopedic Surgeon', 4, '03004567890', 'fatima.raza@hospital.com', 160000),
('Bilal', 'Sheikh', 'Pediatrician', 5, '03005678901', 'bilal.sheikh@hospital.com', 120000),
('Ayesha', 'Tariq', 'Gynecologist', 6, '03006789012', 'ayesha.tariq@hospital.com', 145000),
('Omar', 'Hussain', 'General Surgeon', 7, '03007890123', 'omar.hussain@hospital.com', 155000),
('Zara', 'Baig', 'Dermatologist', 8, '03008901234', 'zara.baig@hospital.com', 125000);

-- Nurses
INSERT INTO Nurses (FirstName, LastName, DeptID, ContactNumber, Shift) VALUES
('Hina', 'Nawaz', 1, '03011111111', 'Morning'),
('Sana', 'Iqbal', 2, '03022222222', 'Evening'),
('Rabia', 'Javed', 3, '03033333333', 'Night'),
('Nadia', 'Cheema', 4, '03044444444', 'Morning'),
('Amna', 'Butt', 5, '03055555555', 'Evening');

-- Staff
INSERT INTO Staff (FirstName, LastName, Role, ContactNumber, Salary) VALUES
('Kamran', 'Akbar', 'Receptionist', '03061111111', 45000),
('Sobia', 'Nazir', 'Accountant', '03072222222', 55000),
('Tariq', 'Mehmood', 'IT Support', '03083333333', 50000),
('Uzma', 'Farooq', 'Lab Technician', '03094444444', 48000),
('Asif', 'Rehman', 'Pharmacist', '03005555555', 52000);

-- Patients
INSERT INTO Patients (FirstName, LastName, DateOfBirth, Gender, ContactNumber, Address, BloodGroup, EmergencyContact) VALUES
('Ali', 'Hassan', '1990-05-15', 'M', '03101111111', 'House 12, Street 4, Lahore', 'A+', '03101111112'),
('Zainab', 'Mirza', '1985-08-22', 'F', '03202222222', 'Flat 5, Block B, Karachi', 'B+', '03202222223'),
('Hamza', 'Siddiqui', '2000-03-10', 'M', '03303333333', 'Village Road, Multan', 'O+', '03303333334'),
('Mariam', 'Chaudhry', '1978-11-30', 'F', '03404444444', 'Plot 22, Islamabad', 'AB+', '03404444445'),
('Tariq', 'Butt', '1995-07-18', 'M', '03505555555', 'Gulberg, Lahore', 'A-', '03505555556'),
('Sadia', 'Awan', '1992-01-25', 'F', '03606666666', 'DHA, Karachi', 'B-', '03606666667'),
('Umar', 'Farooq', '1988-09-05', 'M', '03707777777', 'Model Town, Lahore', 'O-', '03707777778'),
('Noor', 'Fatima', '2005-12-12', 'F', '03808888888', 'Johar Town, Lahore', 'AB-', '03808888889'),
('Kashif', 'Raza', '1975-04-20', 'M', '03909999999', 'Saddar, Rawalpindi', 'A+', '03909999990'),
('Hira', 'Aslam', '1998-06-08', 'F', '03001010101', 'Cantt, Lahore', 'B+', '03001010102');

-- Rooms
INSERT INTO Rooms (RoomNumber, RoomType, PricePerDay, IsAvailable) VALUES
('R101', 'General', 2000, 1),
('R102', 'General', 2000, 1),
('R201', 'Private', 5000, 1),
('R202', 'Private', 5000, 0),
('R301', 'ICU', 15000, 1),
('R302', 'ICU', 15000, 0),
('E101', 'Emergency', 8000, 1),
('E102', 'Emergency', 8000, 1);

-- Appointments
INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, Status, Notes) VALUES
(1, 1, '2026-06-20 09:00:00', 'Completed', 'Routine checkup'),
(2, 2, '2026-06-21 10:30:00', 'Completed', 'Headache and dizziness'),
(3, 3, '2026-06-22 11:00:00', 'Completed', 'Ear infection'),
(4, 4, '2026-06-23 14:00:00', 'Completed', 'Knee pain'),
(5, 1, '2026-06-25 09:30:00', 'Scheduled', 'Follow-up visit'),
(6, 6, '2026-06-26 11:00:00', 'Scheduled', 'Monthly checkup'),
(7, 7, '2026-06-27 15:00:00', 'Scheduled', 'Abdomen pain'),
(8, 5, '2026-06-28 10:00:00', 'Scheduled', 'Child fever'),
(9, 8, '2026-06-24 13:00:00', 'Cancelled', 'Patient cancelled'),
(10, 2, '2026-06-29 09:00:00', 'Scheduled', 'MRI follow-up');

-- Admissions
INSERT INTO Admissions (PatientID, RoomID, AdmissionDate, DischargeDate, AdmittedBy) VALUES
(1, 1, '2026-06-15', '2026-06-18', 1),
(2, 3, '2026-06-16', '2026-06-20', 2),
(3, 2, '2026-06-18', '2026-06-21', 3),
(4, 4, '2026-06-20', NULL, 4),
(5, 5, '2026-06-22', NULL, 1);

-- MedicalRecords
INSERT INTO MedicalRecords (PatientID, DoctorID, Diagnosis, Treatment, RecordDate) VALUES
(1, 1, 'Hypertension', 'Prescribed Amlodipine 5mg daily', '2026-06-20'),
(2, 2, 'Migraine', 'Prescribed Sumatriptan, rest advised', '2026-06-21'),
(3, 3, 'Otitis Media', 'Prescribed antibiotics and ear drops', '2026-06-22'),
(4, 4, 'Osteoarthritis', 'Physiotherapy and pain management', '2026-06-23'),
(5, 1, 'Chest Pain', 'ECG done, prescribed nitrates', '2026-06-25'),
(6, 6, 'Pregnancy checkup', 'Normal, follow-up in 4 weeks', '2026-06-26'),
(7, 7, 'Appendicitis', 'Surgery recommended', '2026-06-27'),
(8, 5, 'Viral Fever', 'Paracetamol and rest', '2026-06-28');

-- Medicines
INSERT INTO Medicines (MedicineName, Category, StockQuantity, UnitPrice, ExpiryDate) VALUES
('Paracetamol 500mg', 'Analgesic', 500, 5, '2027-12-31'),
('Amlodipine 5mg', 'Antihypertensive', 200, 15, '2027-06-30'),
('Amoxicillin 500mg', 'Antibiotic', 300, 25, '2026-12-31'),
('Metformin 500mg', 'Antidiabetic', 250, 10, '2027-03-31'),
('Omeprazole 20mg', 'Antacid', 400, 8, '2027-09-30'),
('Ibuprofen 400mg', 'Anti-inflammatory', 350, 12, '2027-01-31'),
('Cetirizine 10mg', 'Antihistamine', 150, 7, '2026-11-30'),
('Sumatriptan 50mg', 'Antimigraine', 100, 45, '2027-05-31'),
('Nitroglycerine 0.5mg', 'Antianginal', 80, 30, '2026-10-31'),
('Azithromycin 500mg', 'Antibiotic', 5, 35, '2026-08-31');

-- Prescriptions
INSERT INTO Prescriptions (RecordID, MedicineID, Dosage, Duration, Quantity) VALUES
(1, 2, '1 tablet daily', '30 days', 30),
(2, 8, '1 tablet as needed', '7 days', 7),
(3, 3, '1 capsule 3x daily', '7 days', 21),
(4, 6, '1 tablet 2x daily', '14 days', 28),
(5, 9, '1 tablet as needed', '14 days', 14),
(6, 1, '2 tablets 3x daily', '5 days', 30),
(7, 5, '1 capsule daily', '14 days', 14),
(8, 1, '1 tablet 4x daily', '5 days', 20);

-- Bills
INSERT INTO Bills (PatientID, AdmissionID, TotalAmount, PaidAmount, BillDate, Status) VALUES
(1, 1, 25000, 25000, '2026-06-18', 'Paid'),
(2, 2, 45000, 30000, '2026-06-20', 'Partial'),
(3, 3, 18000, 18000, '2026-06-21', 'Paid'),
(4, 4, 35000, 0, '2026-06-26', 'Unpaid'),
(5, 5, 60000, 0, '2026-06-26', 'Unpaid');

-- BillItems
INSERT INTO BillItems (BillID, Description, Amount) VALUES
(1, 'Room Charges (3 days General)', 6000),
(1, 'Doctor Consultation', 5000),
(1, 'Medicines', 3000),
(1, 'Lab Tests', 8000),
(1, 'Nursing Charges', 3000),
(2, 'Room Charges (4 days Private)', 20000),
(2, 'Doctor Consultation', 8000),
(2, 'MRI Scan', 12000),
(2, 'Medicines', 5000),
(3, 'Room Charges (3 days General)', 6000),
(3, 'Doctor Consultation', 5000),
(3, 'Medicines', 4000),
(3, 'Ear Treatment', 3000);