-- procedures

DELIMITER $$

-- 1. Admit a Patient
CREATE PROCEDURE sp_AdmitPatient(
    IN p_PatientID INT,
    IN p_RoomID INT,
    IN p_DoctorID INT
)
BEGIN
    START TRANSACTION;
        INSERT INTO Admissions(PatientID, RoomID, AdmittedBy)
        VALUES (p_PatientID, p_RoomID, p_DoctorID);
        
        UPDATE Rooms SET IsAvailable = 0 WHERE RoomID = p_RoomID;
    COMMIT;
END$$

-- 2. Discharge Patient & Generate Bill
CREATE PROCEDURE sp_DischargePatient(
    IN p_AdmissionID INT,
    IN p_DischargeDate DATE
)
BEGIN
    DECLARE v_RoomID INT;
    DECLARE v_PatientID INT;
    
    START TRANSACTION;
        UPDATE Admissions 
        SET DischargeDate = p_DischargeDate
        WHERE AdmissionID = p_AdmissionID;
        
        SELECT RoomID, PatientID 
        INTO v_RoomID, v_PatientID
        FROM Admissions 
        WHERE AdmissionID = p_AdmissionID;
        
        UPDATE Rooms SET IsAvailable = 1 WHERE RoomID = v_RoomID;
        
        INSERT INTO Bills(PatientID, AdmissionID)
        VALUES (v_PatientID, p_AdmissionID);
    COMMIT;
END$$

-- 3. Search Patients
CREATE PROCEDURE sp_SearchPatients(
    IN p_SearchTerm VARCHAR(100)
)
BEGIN
    SELECT * FROM Patients
    WHERE FirstName LIKE CONCAT('%', p_SearchTerm, '%')
       OR LastName LIKE CONCAT('%', p_SearchTerm, '%')
       OR ContactNumber LIKE CONCAT('%', p_SearchTerm, '%');
END$$

DELIMITER ;