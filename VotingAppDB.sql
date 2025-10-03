/*
SQLyog Community v13.1.5  (64 bit)
MySQL - 10.4.32-MariaDB : Database - votingsystem
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`votingsystem` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;

USE `votingsystem`;

/*Table structure for table `candidates` */

DROP TABLE IF EXISTS `candidates`;

CREATE TABLE `candidates` (
  `CandidateID` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Position` varchar(50) NOT NULL,
  PRIMARY KEY (`CandidateID`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `candidates` */

insert  into `candidates`(`CandidateID`,`FirstName`,`LastName`,`Position`) values 
(10,'Christopher','Smith','President'),
(11,'Christian','Nabunturan','Mayor'),
(12,'John','Anderson','President'),
(13,'Sarah','Mitchell','President'),
(14,'David','Chen','President'),
(15,'Maria','Rodriguez','President'),
(16,'Robert','Thompson','Mayor'),
(17,'Jennifer','Williams','Mayor'),
(18,'Michael','Brown','Mayor'),
(19,'Lisa','Davis','Mayor');

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` enum('Admin','Voter') NOT NULL DEFAULT 'Voter',
  `CreatedAt` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Email` (`Email`),
  UNIQUE KEY `Username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `users` */

insert  into `users`(`UserID`,`FirstName`,`LastName`,`Email`,`Username`,`PasswordHash`,`Role`,`CreatedAt`) values 
(1,'System','Admin','admin@example.com','admin','admin123','Admin','2025-09-29 13:55:10'),
(3,'','','','admin2','240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9','Admin','2025-09-29 14:03:59');

/*Table structure for table `voters` */

DROP TABLE IF EXISTS `voters`;

CREATE TABLE `voters` (
  `VoterID` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Course` varchar(50) NOT NULL,
  `YearLevel` varchar(50) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` enum('Voter') NOT NULL DEFAULT 'Voter',
  `CreatedAt` timestamp NOT NULL DEFAULT current_timestamp(),
  `HasVoted` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`VoterID`),
  UNIQUE KEY `Email` (`Email`),
  UNIQUE KEY `Username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `voters` */

insert  into `voters`(`VoterID`,`FirstName`,`LastName`,`Email`,`Course`,`YearLevel`,`Username`,`PasswordHash`,`Role`,`CreatedAt`,`HasVoted`) values 
(1,'Kyle','Verdida','@email.com','','','kyleverdida','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','Voter','2025-09-26 10:25:54',1),
(6,'k','k','k','','','k','8254c329a92850f6d539dd376f4816ee2764517da5e0235514af433164480d7a','Voter','2025-10-01 18:50:14',0),
(8,'1','2','3','','','4','ef2d127de37b942baad06145e54b0c619a1f22327b2ebbcfbec78f5564afe39d','Voter','2025-10-01 18:50:14',0),
(9,'n','n','n','','','n','1b16b1df538ba12dc3f97edbb85caa7050d46c148134290feba80f8236c83db9','Voter','2025-10-01 19:44:20',1),
(10,'b','b','b','','','b','3e23e8160039594a33894f6564e1b1348bbd7a0088d42c4acb73eeaed59c009d','Voter','2025-10-01 19:54:57',1);

/*Table structure for table `votes` */

DROP TABLE IF EXISTS `votes`;

CREATE TABLE `votes` (
  `VoteID` int(11) NOT NULL AUTO_INCREMENT,
  `VoterID` int(11) NOT NULL,
  `CandidateID` int(11) NOT NULL,
  `VoteDate` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`VoteID`),
  UNIQUE KEY `unique_vote_per_position` (`VoterID`,`CandidateID`),
  KEY `CandidateID` (`CandidateID`),
  CONSTRAINT `votes_ibfk_1` FOREIGN KEY (`VoterID`) REFERENCES `voters` (`VoterID`),
  CONSTRAINT `votes_ibfk_2` FOREIGN KEY (`CandidateID`) REFERENCES `candidates` (`CandidateID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `votes` */

insert  into `votes`(`VoteID`,`VoterID`,`CandidateID`,`VoteDate`) values 
(1,1,10,'2025-10-01 12:04:47'),
(2,1,11,'2025-10-01 12:11:00'),
(5,9,11,'2025-10-01 19:54:16'),
(6,9,15,'2025-10-01 19:54:23'),
(7,10,17,'2025-10-01 19:55:18'),
(8,10,13,'2025-10-01 19:55:23');

/* Procedure structure for procedure `sp_AddCandidate` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_AddCandidate` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_AddCandidate`(IN FirstName VARCHAR(100), IN LastName VARCHAR(100), IN Position VARCHAR(100))
BEGIN
    INSERT INTO candidates (FirstName, LastName, Position) VALUES (FirstName, LastName, Position);
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_AddUser` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_AddUser` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_AddUser`(
  IN in_username VARCHAR(100),
  IN in_password VARCHAR(100),
  IN in_fullname VARCHAR(150),
  IN in_role VARCHAR(10)
)
BEGIN
  INSERT INTO Users (Username, Password, FullName, Role)
  VALUES (in_username, in_password, in_fullname, in_role);
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_AddVote` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_AddVote` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_AddVote`(IN CandidateID INT)
BEGIN
    INSERT INTO votes (CandidateID) VALUES (CandidateID);
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_AddVoter` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_AddVoter` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_AddVoter`(IN in_fn VARCHAR(100), IN in_ln VARCHAR(100), IN in_email VARCHAR(150))
BEGIN
  INSERT INTO Voters (FirstName, LastName, Email)
  VALUES (in_fn, in_ln, in_email);
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_CastVote` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_CastVote` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_CastVote`(
    IN in_voterid INT, 
    IN in_candidateid INT
)
BEGIN
    DECLARE candidate_position VARCHAR(100);
    DECLARE vote_count INT;
    DECLARE total_positions INT;
    DECLARE voted_positions INT;
    
    -- Get the position of the candidate they're trying to vote for
    SELECT Position INTO candidate_position
    FROM candidates
    WHERE CandidateID = in_candidateid;
    
    -- Check if voter has already voted for this position
    SELECT COUNT(*) INTO vote_count
    FROM votes v
    INNER JOIN candidates c ON v.CandidateID = c.CandidateID
    WHERE v.VoterID = in_voterid 
      AND c.Position = candidate_position;
    
    -- If already voted for this position, raise an error
    IF vote_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'You have already voted for this position';
    ELSE
        -- Cast the vote
        INSERT INTO Votes (VoterID, CandidateID)
        VALUES (in_voterid, in_candidateid);
        
        -- Count total distinct positions available
        SELECT COUNT(DISTINCT Position) INTO total_positions 
        FROM candidates;
        
        -- Count how many distinct positions this voter has now voted for
        SELECT COUNT(DISTINCT c.Position) INTO voted_positions
        FROM votes v
        INNER JOIN candidates c ON v.CandidateID = c.CandidateID
        WHERE v.VoterID = in_voterid;
        
        -- Only mark as HasVoted = 1 if they've voted for ALL positions
        IF voted_positions >= total_positions THEN
            UPDATE Voters SET HasVoted = 1 WHERE VoterID = in_voterid;
        END IF;
    END IF;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_CheckIfAlreadyVoted` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_CheckIfAlreadyVoted` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_CheckIfAlreadyVoted`(
    IN in_voterid INT,
    IN in_position VARCHAR(100)
)
BEGIN
    SELECT COUNT(*) AS VoteCount
    FROM votes v
    INNER JOIN candidates c ON v.CandidateID = c.CandidateID
    WHERE v.VoterID = in_voterid 
      AND c.Position = in_position;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_DeleteCandidate` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_DeleteCandidate` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_DeleteCandidate`(IN p_CandidateID INT)
BEGIN 
    DELETE FROM candidates WHERE CandidateID = p_CandidateID;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_DeleteVoter` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_DeleteVoter` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_DeleteVoter`(IN in_id INT)
BEGIN
  DELETE FROM Voters WHERE VoterID=in_id;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetAllCandidates` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetAllCandidates` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetAllCandidates`()
BEGIN
    SELECT FirstName, LastName, Position FROM candidates;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetAllPositions` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetAllPositions` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetAllPositions`()
BEGIN
    SELECT DISTINCT Position FROM candidates ORDER BY Position;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetAllVoters` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetAllVoters` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetAllVoters`()
BEGIN
  SELECT VoterID, FirstName, LastName,Course, YearLevel, Email, HasVoted   FROM Voters
    ORDER BY VoterId DESC;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetCandidatesByPosition` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetCandidatesByPosition` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetCandidatesByPosition`(IN in_position VARCHAR(100))
BEGIN
    SELECT CandidateID, FirstName, LastName, Position FROM candidates WHERE Position = in_position;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetResults` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetResults` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetResults`()
BEGIN
    SELECT 
        c.Position,
        c.CandidateID,
        CONCAT(c.FirstName, ' ', c.LastName) AS Candidate,
        COALESCE(COUNT(v.VoteId), 0) AS VoteCount
    FROM candidates c
    LEFT JOIN votes v ON c.CandidateID = v.CandidateID
    GROUP BY c.Position, c.CandidateID, c.FirstName, c.LastName
    ORDER BY c.Position, VoteCount DESC;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetResultsByPosition` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetResultsByPosition` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetResultsByPosition`(IN in_position VARCHAR(100))
BEGIN
    SELECT 
        c.Position,
        c.CandidateID,
        CONCAT(c.FirstName, ' ', c.LastName) AS Candidate,
        COALESCE(COUNT(v.VoteId), 0) AS VoteCount
    FROM candidates c
    LEFT JOIN votes v ON c.CandidateID = v.CandidateID
    WHERE c.Position = in_position
    GROUP BY c.Position, c.CandidateID, c.FirstName, c.LastName
    ORDER BY VoteCount DESC;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetUserByUsername` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetUserByUsername` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetUserByUsername`(
    IN in_username VARCHAR(50)
)
BEGIN
    SELECT UserID, Username, PasswordHash, ROLE
    FROM Users
    WHERE Username = in_username;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetVoterByID` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetVoterByID` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetVoterByID`(IN in_voterid INT)
BEGIN
  SELECT VoterID, FirstName, LastName, HasVoted
  FROM Voters
  WHERE VoterID = in_voterid;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetVoterByUsername` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetVoterByUsername` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetVoterByUsername`(
    IN in_username VARCHAR(50)
)
BEGIN
    SELECT VoterID, Username, PasswordHash, Role
    FROM Voters
    WHERE Username = in_username;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_Login` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_Login` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_Login`(
    IN p_Username VARCHAR(50)
)
BEGIN
    -- First check if user is an admin
    IF EXISTS (SELECT 1 FROM users WHERE Username = p_Username) THEN
        SELECT UserID AS ID, FirstName, LastName, Email, Username, PasswordHash, Role, 'Admin' AS UserType
        FROM users 
        WHERE Username = p_Username;
    -- Then check if user is a voter
    ELSEIF EXISTS (SELECT 1 FROM voters WHERE Username = p_Username) THEN
        SELECT VoterID AS ID, FirstName, LastName, Email, Username, PasswordHash, Role, 'Voter' AS UserType
        FROM voters 
        WHERE Username = p_Username;
    ELSE
        -- Return empty result if user not found
        SELECT NULL AS ID, NULL AS FirstName, NULL AS LastName, NULL AS Email, 
               NULL AS Username, NULL AS PasswordHash, NULL AS Role, NULL AS UserType;
    END IF;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_RegisterUser` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_RegisterUser` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_RegisterUser`(
    IN in_fn VARCHAR(100),
    IN in_ln VARCHAR(100),
    IN in_email VARCHAR(255),
    IN in_role ENUM('Admin','Voter')
)
BEGIN
    INSERT INTO Users (FirstName, LastName, Email, Role)
    VALUES (in_fn, in_ln, in_email, in_role);
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_RegisterVoter` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_RegisterVoter` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_RegisterVoter`(
    IN p_FirstName VARCHAR(50),
    IN p_LastName VARCHAR(50),
    IN p_Email VARCHAR(100),
    IN p_Username VARCHAR(50),
    IN p_PasswordHash VARCHAR(255)
)
BEGIN
   -- Check if username already exists in voters table
    IF EXISTS (SELECT 1 FROM voters WHERE Username = p_Username) THEN
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Username already exists';
    END IF;
    
    -- Check if email already exists in voters table
    IF EXISTS (SELECT 1 FROM voters WHERE Email = p_Email) THEN
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Email already exists';
    END IF;
    
    -- Insert new voter into voters table
    INSERT INTO voters (FirstName, LastName, Email, Username, PasswordHash, Role, HasVoted)
    VALUES (p_FirstName, p_LastName, p_Email, p_Username, p_PasswordHash, 'Voter', 0);
    
    SELECT LAST_INSERT_ID() AS VoterID;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_UpdateCandidate` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_UpdateCandidate` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_UpdateCandidate`(
    IN in_id INT,
    IN in_fn VARCHAR(100),
    IN in_ln VARCHAR(100),
    IN in_position VARCHAR(100)
)
BEGIN
    UPDATE candidates
    SET FirstName = in_fn,
        LastName = in_ln,
        Position = in_position
    WHERE CandidateID = in_id;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_UpdateVoter` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_UpdateVoter` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_UpdateVoter`(IN in_id INT, IN in_fn VARCHAR(100), IN in_ln VARCHAR(100), IN in_email VARCHAR(150))
BEGIN
    UPDATE voters 
    SET 
        FirstName = in_fn, 
        LastName = in_ln, 
        Email = in_email 
    WHERE 
        VoterID = in_id;
        END */$$
DELIMITER ;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
