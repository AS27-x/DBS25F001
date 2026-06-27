create database hospital_management_system;
use hospital_management_system;
-- 1. Departments
CREATE TABLE Departments (
    DeptID INT PRIMARY KEY AUTO_INCREMENT,
    DeptName VARCHAR(100) NOT NULL UNIQUE,
    Location VARCHAR(100),
    ContactNumber VARCHAR(15)
);

-- 2. Doctors
CREATE TABLE Doctors (
    DoctorID INT PRIMARY KEY AUTO_INCREMENT,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Specialization VARCHAR(100) NOT NULL,
    DeptID INT,
    ContactNumber VARCHAR(15) NOT NULL UNIQUE,
    Email VARCHAR(100) UNIQUE,
    Salary DECIMAL(10,2) CHECK (Salary > 0),
    FOREIGN KEY (DeptID) REFERENCES Departments(DeptID)
);

-- 3. Patients
CREATE TABLE Patients (
    PatientID INT PRIMARY KEY AUTO_INCREMENT,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender CHAR(1) CHECK (Gender IN ('M','F','O')),
    ContactNumber VARCHAR(15),
    Address TEXT,
    BloodGroup VARCHAR(5),
    EmergencyContact VARCHAR(15)
);

-- 4. Rooms
CREATE TABLE Rooms (
    RoomID INT PRIMARY KEY AUTO_INCREMENT,
    RoomNumber VARCHAR(10) NOT NULL UNIQUE,
    RoomType VARCHAR(50) CHECK (RoomType IN ('General','Private','ICU','Emergency')),
    PricePerDay DECIMAL(10,2) CHECK (PricePerDay >= 0),
    IsAvailable TINYINT(1) DEFAULT 1
);

-- 5. Nurses
CREATE TABLE Nurses (
    NurseID INT PRIMARY KEY AUTO_INCREMENT,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DeptID INT,
    ContactNumber VARCHAR(15) UNIQUE,
    Shift VARCHAR(20) CHECK (Shift IN ('Morning','Evening','Night')),
    FOREIGN KEY (DeptID) REFERENCES Departments(DeptID)
);

-- 6. Staff
CREATE TABLE Staff (
    StaffID INT PRIMARY KEY AUTO_INCREMENT,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Role VARCHAR(50),
    ContactNumber VARCHAR(15),
    Salary DECIMAL(10,2) CHECK (Salary > 0)
);

-- 7. Users
CREATE TABLE Users (
    UserID INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARCHAR(256) NOT NULL,
    Role VARCHAR(20) CHECK (Role IN ('Admin','Doctor','Receptionist','Pharmacist')),
    IsActive TINYINT(1) DEFAULT 1
);

-- 8. Appointments
CREATE TABLE Appointments (
    AppointmentID INT PRIMARY KEY AUTO_INCREMENT,
    PatientID INT,
    DoctorID INT,
    AppointmentDate DATETIME NOT NULL,
    Status VARCHAR(20) DEFAULT 'Scheduled' CHECK (Status IN ('Scheduled','Completed','Cancelled')),
    Notes TEXT,
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
);

-- 9. Admissions
CREATE TABLE Admissions (
    AdmissionID INT PRIMARY KEY AUTO_INCREMENT,
    PatientID INT,
    RoomID INT,
    AdmissionDate DATE NOT NULL DEFAULT (CURRENT_DATE),
    DischargeDate DATE,
    AdmittedBy INT,
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
    FOREIGN KEY (RoomID) REFERENCES Rooms(RoomID),
    FOREIGN KEY (AdmittedBy) REFERENCES Doctors(DoctorID)
);

-- 10. MedicalRecords
CREATE TABLE MedicalRecords (
    RecordID INT PRIMARY KEY AUTO_INCREMENT,
    PatientID INT,
    DoctorID INT,
    Diagnosis TEXT NOT NULL,
    Treatment TEXT,
    RecordDate DATE DEFAULT (CURRENT_DATE),
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
);

-- 11. Medicines
CREATE TABLE Medicines (
    MedicineID INT PRIMARY KEY AUTO_INCREMENT,
    MedicineName VARCHAR(100) NOT NULL UNIQUE,
    Category VARCHAR(50),
    StockQuantity INT DEFAULT 0 CHECK (StockQuantity >= 0),
    UnitPrice DECIMAL(10,2) CHECK (UnitPrice >= 0),
    ExpiryDate DATE
);

-- 12. Prescriptions
CREATE TABLE Prescriptions (
    PrescriptionID INT PRIMARY KEY AUTO_INCREMENT,
    RecordID INT,
    MedicineID INT,
    Dosage VARCHAR(100),
    Duration VARCHAR(50),
    Quantity INT CHECK (Quantity > 0),
    FOREIGN KEY (RecordID) REFERENCES MedicalRecords(RecordID),
    FOREIGN KEY (MedicineID) REFERENCES Medicines(MedicineID)
);

-- 13. Bills
CREATE TABLE Bills (
    BillID INT PRIMARY KEY AUTO_INCREMENT,
    PatientID INT,
    AdmissionID INT,
    TotalAmount DECIMAL(10,2) DEFAULT 0,
    PaidAmount DECIMAL(10,2) DEFAULT 0,
    BillDate DATE DEFAULT (CURRENT_DATE),
    Status VARCHAR(20) DEFAULT 'Unpaid' CHECK (Status IN ('Paid','Unpaid','Partial')),
    FOREIGN KEY (PatientID) REFERENCES Patients(PatientID),
    FOREIGN KEY (AdmissionID) REFERENCES Admissions(AdmissionID)
);

-- 14. BillItems
CREATE TABLE BillItems (
    ItemID INT PRIMARY KEY AUTO_INCREMENT,
    BillID INT,
    Description VARCHAR(200),
    Amount DECIMAL(10,2) CHECK (Amount >= 0),
    FOREIGN KEY (BillID) REFERENCES Bills(BillID)
);

-- 15. ErrorLogs
CREATE TABLE ErrorLogs (
    LogID INT PRIMARY KEY AUTO_INCREMENT,
    ErrorMessage TEXT,
    StackTrace TEXT,
    LoggedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    LoggedBy VARCHAR(100)
);