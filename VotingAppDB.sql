/*
SQLyog Community v13.3.0 (64 bit)
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `candidates` */

insert  into `candidates`(`CandidateID`,`FirstName`,`LastName`,`Position`) values 
(8,'Marcus','Cole','Treasurer'),
(10,'Christopher','Smith','President');

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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `users` */

insert  into `users`(`UserID`,`FirstName`,`LastName`,`Email`,`Username`,`PasswordHash`,`Role`,`CreatedAt`) values 
(1,'System','Admin','admin@example.com','admin','admin123','Admin','2025-09-29 13:55:10'),
(2,'Kyle','Verdida','kyleverdida@Gmail.com','kyleverdida','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','Voter','2025-09-29 13:57:52'),
(3,'','','','admin2','240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9','Admin','2025-09-29 14:03:59'),
(4,'k','k','k','k','8254c329a92850f6d539dd376f4816ee2764517da5e0235514af433164480d7a','Voter','2025-09-29 21:10:15'),
(5,'a','b','c','d','3f79bb7b435b05321651daefd374cdc681dc06faa65e374e38337b88ca046dea','Voter','2025-09-30 11:44:55');

/*Table structure for table `voters` */

DROP TABLE IF EXISTS `voters`;

CREATE TABLE `voters` (
  `VoterID` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` enum('Voter') NOT NULL DEFAULT 'Voter',
  `CreatedAt` timestamp NOT NULL DEFAULT current_timestamp(),
  `HasVoted` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`VoterID`),
  UNIQUE KEY `Email` (`Email`),
  UNIQUE KEY `Username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `voters` */

insert  into `voters`(`VoterID`,`FirstName`,`LastName`,`Email`,`Username`,`PasswordHash`,`Role`,`CreatedAt`,`HasVoted`) values 
(1,'Kyle','Verdida','@email.com','kyleverdida','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','Voter','2025-09-26 10:25:54',0),
(2,'K','k','k','t','a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3','Voter','2025-09-29 13:48:24',0);

/*Table structure for table `votes` */

DROP TABLE IF EXISTS `votes`;

CREATE TABLE `votes` (
  `VoteID` int(11) NOT NULL AUTO_INCREMENT,
  `VoterID` int(11) NOT NULL,
  `CandidateID` int(11) NOT NULL,
  `VoteDate` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`VoteID`),
  KEY `VoterID` (`VoterID`),
  KEY `CandidateID` (`CandidateID`),
  CONSTRAINT `votes_ibfk_1` FOREIGN KEY (`VoterID`) REFERENCES `voters` (`VoterID`),
  CONSTRAINT `votes_ibfk_2` FOREIGN KEY (`CandidateID`) REFERENCES `candidates` (`CandidateID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*Data for the table `votes` */

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

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_CastVote`(IN in_voterid INT, IN in_candidateid INT)
BEGIN
    INSERT INTO Votes (VoterID, CandidateID)
    VALUES (in_voterid, in_candidateid);

    UPDATE Voters
    SET HasVoted = 1
    WHERE VoterID = in_voterid;
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
    SELECT CandidateID, FirstName, LastName, Position FROM candidates;
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
  SELECT VoterID, FirstName, LastName, Email, HasVoted   FROM Voters
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
    SELECT UserID, Username, PasswordHash, Role
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
    IN FirstName VARCHAR(50),
    IN LastName VARCHAR(50),
    IN Email VARCHAR(100),
    IN Username VARCHAR(50),
    IN PasswordHash VARCHAR(255)
)
BEGIN
    INSERT INTO Users (FirstName, LastName, Email, Username, PasswordHash, Role)
    VALUES (FirstName, LastName, Email, Username, PasswordHash, 'Voter');
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
