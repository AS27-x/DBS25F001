-- views

-- 1. Active Appointments with Doctor & Patient names
CREATE VIEW vw_ActiveAppointments AS
SELECT a.AppointmentID, 
       p.FirstName + ' ' + p.LastName AS PatientName,
       d.FirstName + ' ' + d.LastName AS DoctorName,
       a.AppointmentDate, a.Status
FROM Appointments a
JOIN Patients p ON a.PatientID = p.PatientID
JOIN Doctors d ON a.DoctorID = d.DoctorID
WHERE a.Status = 'Scheduled';

-- 2. Patient Billing Summary
CREATE VIEW vw_PatientBillSummary AS
SELECT b.BillID, p.FirstName + ' ' + p.LastName AS PatientName,
       b.TotalAmount, b.PaidAmount,
       (b.TotalAmount - b.PaidAmount) AS Balance, b.Status
FROM Bills b JOIN Patients p ON b.PatientID = p.PatientID;

-- 3. Doctor Patient Count
CREATE VIEW vw_DoctorPatientCount AS
SELECT d.DoctorID, d.FirstName + ' ' + d.LastName AS DoctorName,
       COUNT(a.AppointmentID) AS TotalAppointments
FROM Doctors d
LEFT JOIN Appointments a ON d.DoctorID = a.DoctorID
GROUP BY d.DoctorID, d.FirstName, d.LastName;

-- 4. Available Rooms
CREATE VIEW vw_AvailableRooms AS
SELECT RoomID, RoomNumber, RoomType, PricePerDay
FROM Rooms WHERE IsAvailable = 1;

-- 5. Medicine Stock Alert (Low Stock)
CREATE VIEW vw_LowStockMedicines AS
SELECT MedicineID, MedicineName, StockQuantity, ExpiryDate
FROM Medicines WHERE StockQuantity < 10;