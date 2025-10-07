/*
SQLyog Community
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
  `Program` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`CandidateID`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `candidates` */

insert  into `candidates`(`CandidateID`,`FirstName`,`LastName`,`Position`,`Program`) values 
(10,'Christopher','Smith','President','Both'),
(11,'Christian','Nabunturan','Vice President','Both'),
(12,'John','Anderson','President','Both'),
(13,'Sarah','Mitchell','President','Both'),
(14,'David','Chen','Vice President','Both'),
(15,'Maria','Rodriguez','Treasurer','Both'),
(16,'Robert','Thompson','Treasurer','Both'),
(17,'Jennifer','Williams','Secretary','Both'),
(18,'Michael','Brown','Secretary','Both'),
(19,'Lisa','Davis','Auditor','Both'),
(20,'Emma','Taylor','Treasurer','Both'),
(21,'Liam','Moore','Treasurer','Both'),
(22,'Olivia','Martin','Secretary','Both'),
(23,'Noah','Garcia','Secretary','Both'),
(24,'Ava','Martinez','Auditor','Both'),
(25,'Ethan','Lopez','Auditor','Both'),
(26,'Sophia','Gonzalez','Business Manager','Both'),
(27,'Mason','Rodriguez','Business Manager','Both'),
(28,'Isabella','Hernandez','CS PIO','CS'),
(29,'William','Perez','CS PIO','CS'),
(30,'Mia','Sanchez','IT PIO','IT'),
(31,'James','Rivera','IT PIO','IT'),
(32,'Kyle','Santos','CS PIO',NULL),
(33,'Jamie','Rivera','CS PIO',NULL),
(34,'Saito','Takahashi','IT PIO',NULL);

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
  `Program` varchar(10) NOT NULL,
  `YearLevel` int(50) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` enum('Voter') NOT NULL DEFAULT 'Voter',
  `CreatedAt` timestamp NOT NULL DEFAULT current_timestamp(),
  `HasVoted` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`VoterID`),
  UNIQUE KEY `Email` (`Email`),
  UNIQUE KEY `Username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `voters` */

insert  into `voters`(`VoterID`,`FirstName`,`LastName`,`Email`,`Program`,`YearLevel`,`Username`,`PasswordHash`,`Role`,`CreatedAt`,`HasVoted`) values 
(12,'Railey','Muyco','rmuyco@gmail.com','BSIT',3,'divided_unity','cb6f2738ae47e335659eed032f35fe48352af47d5b4c70b748f05aad8efca3dc','Voter','2025-10-04 15:08:45',0),
(13,'Maria','Magdalena','mmagdalena@gmail.com','BSIT',2,'magdalena1','b84e4623195bbea8eb537bb4abfd56e3789e7d5ca49c23357692e4af114ce5a6','Voter','2025-10-04 18:11:37',0),
(14,'Mario','Mario','mario@gmail.com','CS',3,'mario123','363b4cdcd88ab5ebe03b52429b87a3f4cd02b393ef04b89339a614588381a408','Voter','2025-10-06 09:01:57',1),
(15,'Luigi','Luigi','luigi@gmail.com','CS',2,'luigi1','fb20e2295b4b44e92039e1e6ba79b92e870b75ce45ac5246142740c4ad88926b','Voter','2025-10-06 09:15:04',0),
(16,'Chris','Chris','chris@gmail.com','CS',4,'chris1','e801351513a76ca602dde4d3875be5cd063c370d7fdfeb06cbda6bfd83b22ba2','Voter','2025-10-06 09:27:24',0),
(17,'kyle','kyle','kyle@gmail.com','CS',3,'kyle1','3d9fd8de875e681a03c1ecc5060e453d04b3b61d4310317983b4251203da21b4','Voter','2025-10-06 09:39:36',1),
(18,'bbm','bbm','bbm@gmail.com','IT',1,'bbmbobo','6497f3cc08ad95f713bc914c8b5bcbd2b806bd97f2f83e24f3bfb0fcf670b639','Voter','2025-10-06 21:13:52',1),
(19,'Gado','Gado','gado@gmail.com','IT',1,'gado123','cf2eea10c249520f873de47ee899881bafcb17b555eae67c9918c2986b83ea88','Voter','2025-10-07 10:55:54',1),
(20,'Jimin','Yu','yujimin@gmail.com','CS',3,'karinayu1','bd1d002ce8479f0ec6447f32ee07d059af6cbd1b1377f8ada94f5d44ec5fc9cf','Voter','2025-10-07 11:51:16',1),
(21,'Minjeong','Kim','winter@gmail.com','IT',3,'winter1','339db8287bc0d6cc2063bd194d55315ecf642a4728d430a23833405126d52eb4','Voter','2025-10-07 11:55:43',0),
(22,'Kyle','Christian','kkyle@gmail.com','CS',1,'kyle123','3d9fd8de875e681a03c1ecc5060e453d04b3b61d4310317983b4251203da21b4','Voter','2025-10-07 11:57:39',1);

/*Table structure for table `votes` */

DROP TABLE IF EXISTS `votes`;

CREATE TABLE `votes` (
  `VoteID` int(11) NOT NULL AUTO_INCREMENT,
  `VoterID` int(11) NOT NULL,
  `CandidateID` int(11) NOT NULL,
  `VoteDate` timestamp NOT NULL DEFAULT current_timestamp(),
  `Position` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`VoteID`),
  KEY `CandidateID` (`CandidateID`),
  KEY `votes_ibfk_1` (`VoterID`),
  CONSTRAINT `votes_ibfk_1` FOREIGN KEY (`VoterID`) REFERENCES `voters` (`VoterID`),
  CONSTRAINT `votes_ibfk_2` FOREIGN KEY (`CandidateID`) REFERENCES `candidates` (`CandidateID`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `votes` */

insert  into `votes`(`VoteID`,`VoterID`,`CandidateID`,`VoteDate`,`Position`) values 
(1,22,10,'2025-10-07 12:00:30','President'),
(2,22,11,'2025-10-07 12:00:32','Vice President'),
(3,22,17,'2025-10-07 12:00:35','Secretary'),
(4,22,15,'2025-10-07 12:00:37','Treasurer'),
(5,22,19,'2025-10-07 12:00:40','Auditor'),
(6,22,26,'2025-10-07 12:00:44','Business Manager'),
(7,22,28,'2025-10-07 12:00:49','CS PIO - 1st'),
(8,22,29,'2025-10-07 12:00:55','CS PIO - 2nd'),
(9,20,10,'2025-10-07 12:01:47','President'),
(10,20,11,'2025-10-07 12:01:50','Vice President'),
(11,20,17,'2025-10-07 12:01:53','Secretary'),
(12,20,15,'2025-10-07 12:01:56','Treasurer'),
(13,20,19,'2025-10-07 12:02:00','Auditor'),
(14,20,26,'2025-10-07 12:02:04','Business Manager'),
(15,20,32,'2025-10-07 12:02:14','CS PIO - 1st'),
(16,20,33,'2025-10-07 12:02:29','CS PIO - 2nd');

/*Table structure for table `voting_settings` */

DROP TABLE IF EXISTS `voting_settings`;

CREATE TABLE `voting_settings` (
  `SettingID` int(11) NOT NULL AUTO_INCREMENT,
  `ResultsAvailableTime` datetime DEFAULT NULL,
  `VotingStartTime` datetime DEFAULT NULL,
  `VotingEndTime` datetime DEFAULT NULL,
  `IsVotingOpen` tinyint(1) DEFAULT 0,
  `LastUpdated` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`SettingID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `voting_settings` */

insert  into `voting_settings`(`SettingID`,`ResultsAvailableTime`,`VotingStartTime`,`VotingEndTime`,`IsVotingOpen`,`LastUpdated`) values 
(1,'2025-10-06 20:30:20',NULL,NULL,0,'2025-10-06 20:22:57');

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

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_AddVoter`(IN in_fn VARCHAR(100), IN in_ln VARCHAR(100))
BEGIN
  INSERT INTO Voters (FirstName, LastName)
  VALUES (in_fn, in_ln);
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_AreResultsAvailable` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_AreResultsAvailable` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_AreResultsAvailable`()
BEGIN
    SELECT 
        CASE 
            WHEN ResultsAvailableTime IS NULL THEN FALSE
            WHEN NOW() >= ResultsAvailableTime THEN TRUE
            ELSE FALSE
        END AS IsAvailable,
        ResultsAvailableTime
    FROM voting_settings
    LIMIT 1;
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_CastVote` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_CastVote` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_CastVote`(
    IN in_voterid INT, 
    IN in_candidateid INT,
    IN in_position VARCHAR(100)  -- ADD THIS PARAMETER
)
BEGIN
    DECLARE vote_count INT;
    DECLARE total_positions INT;
    DECLARE voted_positions INT;
    DECLARE base_position VARCHAR(100);
    
    -- Extract base position (remove "- 1st" or "- 2nd" suffix)
    SET base_position = IF(in_position LIKE '%-%', 
                           SUBSTRING_INDEX(in_position, ' -', 1), 
                           in_position);
    
    -- Check if voter has already voted for this EXACT position (e.g., "CS PIO - 2nd")
    SELECT COUNT(*) INTO vote_count
    FROM votes v
    WHERE v.VoterID = in_voterid 
      AND v.Position = in_position;  -- Check against full position name
    
    -- If already voted for this exact position, raise an error
    IF vote_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'You have already voted for this position';
    ELSE
        -- Cast the vote with the full position
        INSERT INTO Votes (VoterID, CandidateID, Position, VoteDate)
        VALUES (in_voterid, in_candidateid, in_position, NOW());
        
        -- Count total distinct positions available (considering PIO votes count as 2)
        SELECT COUNT(DISTINCT POSITION) INTO total_positions 
        FROM candidates;
        
        -- Count how many positions this voter has voted for
        SELECT COUNT(*) INTO voted_positions
        FROM votes
        WHERE VoterID = in_voterid;
        
        -- Mark as HasVoted = 1 if they've completed all their votes
        -- (6 regular positions + 2 PIO votes = 8 total)
        IF voted_positions >= 8 THEN
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

/* Procedure structure for procedure `sp_CheckIfVotedForCandidate` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_CheckIfVotedForCandidate` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_CheckIfVotedForCandidate`(
    IN in_voterid INT,
    IN in_candidateid INT,
    IN in_position VARCHAR(100)
)
BEGIN
    SELECT COUNT(*) AS VoteCount
    FROM votes v
    JOIN candidates c ON v.CandidateID = c.CandidateID
    WHERE v.VoterID = in_voterid 
    AND v.CandidateID = in_candidateid
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
  SELECT VoterID, FirstName, LastName,Program, YearLevel, Email, HasVoted   FROM Voters
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

/* Procedure structure for procedure `sp_GetElectionWinners` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetElectionWinners` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetElectionWinners`()
BEGIN
    -- Create temporary table with vote counts
    CREATE TEMPORARY TABLE IF NOT EXISTS temp_vote_counts (
        Position VARCHAR(100),
        CandidateID INT,
        Candidate VARCHAR(200),
        VoteCount INT
    );
    
    -- Clear temp table if it exists
    TRUNCATE TABLE temp_vote_counts;
    
    -- Insert vote counts
    INSERT INTO temp_vote_counts
    SELECT 
        v.Position,
        c.CandidateID,
        CONCAT(c.FirstName, ' ', c.LastName) AS Candidate,
        COUNT(v.VoteID) AS VoteCount
    FROM votes v
    INNER JOIN candidates c ON v.CandidateID = c.CandidateID
    GROUP BY v.Position, c.CandidateID, c.FirstName, c.LastName;
    
    -- Get only winners (max votes per position)
    SELECT 
        t1.Position,
        t1.Candidate,
        t1.VoteCount
    FROM temp_vote_counts t1
    INNER JOIN (
        SELECT Position, MAX(VoteCount) AS MaxVotes
        FROM temp_vote_counts
        GROUP BY Position
    ) t2 ON t1.Position = t2.Position AND t1.VoteCount = t2.MaxVotes
    ORDER BY 
        CASE t1.Position
            WHEN 'President' THEN 1
            WHEN 'Vice President' THEN 2
            WHEN 'Secretary' THEN 3
            WHEN 'Treasurer' THEN 4
            WHEN 'Auditor' THEN 5
            WHEN 'Business Manager' THEN 6
            WHEN 'CS PIO - 1st' THEN 7
            WHEN 'CS PIO - 2nd' THEN 8
            WHEN 'IT PIO - 1st' THEN 9
            WHEN 'IT PIO - 2nd' THEN 10
            ELSE 99
        END,
        t1.Position;
    
    -- Clean up
    DROP TEMPORARY TABLE IF EXISTS temp_vote_counts;
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

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetVoterByID`(
    IN in_voterid INT
)
BEGIN
    SELECT 
        VoterID,
        FirstName,
        LastName,
        Email,
        Program,        -- Make sure this is included!
        YearLevel,
        Username,
        PasswordHash
    FROM voters
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

/* Procedure structure for procedure `sp_GetVoterReceipt` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetVoterReceipt` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetVoterReceipt`(
    IN in_voterid INT
)
BEGIN
    SELECT 
        v.VoteID,
        v.VoterID,
        v.CandidateID,
        v.Position,  -- Use position from votes table now
        CONCAT(c.FirstName, ' ', c.LastName) AS CandidateName,
        v.VoteDate  -- Changed from VotedAt to VoteDate
    FROM votes v
    INNER JOIN candidates c ON v.CandidateID = c.CandidateID
    WHERE v.VoterID = in_voterid
    ORDER BY 
        CASE 
            WHEN v.Position = 'President' THEN 1
            WHEN v.Position = 'Vice President' THEN 2
            WHEN v.Position = 'Secretary' THEN 3
            WHEN v.Position = 'Treasurer' THEN 4
            WHEN v.Position = 'Auditor' THEN 5
            WHEN v.Position = 'Business Manager' THEN 6
            WHEN v.Position LIKE 'CS PIO%' THEN 7
            WHEN v.Position LIKE 'IT PIO%' THEN 8
            ELSE 9
        END,
        v.Position;  -- Secondary sort by full position name
END */$$
DELIMITER ;

/* Procedure structure for procedure `sp_GetVotersByProgram` */

/*!50003 DROP PROCEDURE IF EXISTS  `sp_GetVotersByProgram` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetVotersByProgram`(IN in_program VARCHAR(50))
BEGIN
    SELECT * FROM voters 
    WHERE Program = in_program
    ORDER BY VoterId DESC;
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
    IN p_Program VARCHAR(10),
    IN p_YearLevel INT,
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
    INSERT INTO voters (FirstName, LastName, Email, Program, YearLevel, Username, PasswordHash, Role, HasVoted)
    VALUES (p_FirstName, p_LastName, p_Email, p_Program, p_YearLevel, p_Username, p_PasswordHash, 'Voter', 0);
    
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

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_UpdateVoter`(IN in_id INT, IN in_fn VARCHAR(100), IN in_ln VARCHAR(100))
BEGIN
    UPDATE voters 
    SET 
        FirstName = in_fn, 
        LastName = in_ln 
    WHERE 
        VoterID = in_id;
        END */$$
DELIMITER ;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
