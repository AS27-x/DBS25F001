-- TRIGGERS
DELIMITER $$

DROP TRIGGER IF EXISTS trg_BillPaid$$
DROP TRIGGER IF EXISTS trg_PreventDoctorDelete$$

-- 1. Auto-log when a bill is marked Paid
CREATE TRIGGER trg_BillPaid
AFTER UPDATE ON Bills
FOR EACH ROW
BEGIN
    IF NEW.Status = 'Paid' AND OLD.Status != 'Paid' THEN
        INSERT INTO ErrorLogs(ErrorMessage, LoggedAt, LoggedBy)
        VALUES (
            CONCAT('Bill #', NEW.BillID, ' status changed to: Paid'),
            NOW(),
            'System'
        );
    END IF;
END$$

-- 2. Prevent deleting a Doctor with active appointments
CREATE TRIGGER trg_PreventDoctorDelete
BEFORE DELETE ON Doctors
FOR EACH ROW
BEGIN
    DECLARE active_count INT;
    
    SELECT COUNT(*) INTO active_count
    FROM Appointments
    WHERE DoctorID = OLD.DoctorID 
    AND Status = 'Scheduled';
    
    IF active_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Cannot delete doctor with active appointments.';
    END IF;
END$$

DELIMITER ;