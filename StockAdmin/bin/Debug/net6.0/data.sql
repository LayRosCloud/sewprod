-- MySQL dump 10.13  Distrib 8.0.36, for Linux (x86_64)
--
-- Host: 127.0.0.1    Database: stock
-- ------------------------------------------------------
-- Server version	8.0.36-0ubuntu0.20.04.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `stock`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `stock` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `stock`;

--
-- Table structure for table `actions`
--

DROP TABLE IF EXISTS `actions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actions` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(30) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `actions`
--

LOCK TABLES `actions` WRITE;
/*!40000 ALTER TABLE `actions` DISABLE KEYS */;
INSERT INTO `actions` VALUES (1,'Чтение'),(2,'Добавление'),(3,'Редактирование'),(4,'Удаление');
/*!40000 ALTER TABLE `actions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ages`
--

DROP TABLE IF EXISTS `ages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ages` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(30) NOT NULL,
  `description` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ages`
--

LOCK TABLES `ages` WRITE;
/*!40000 ALTER TABLE `ages` DISABLE KEYS */;
INSERT INTO `ages` VALUES (1,'Взрослые (М)','Взрослые мужчины'),(2,'Мужское 2','170-176 '),(3,'Женские','168'),(4,'Мужское 1','164-168'),(5,'Мужское 3','182-188');
/*!40000 ALTER TABLE `ages` ENABLE KEYS */;
UNLOCK TABLES;



--
-- Table structure for table `histories`
--


DROP TABLE IF EXISTS `materials`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `materials` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(30) NOT NULL,
  `description` varchar(255) NOT NULL,
  `uid` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `materials`
--

LOCK TABLES `materials` WRITE;
/*!40000 ALTER TABLE `materials` DISABLE KEYS */;
INSERT INTO `materials` VALUES (1,'Шелк','Описание шелка','SH'),(2,'Кулир ',' ',' '),(3,'Бархат','Описание Бархата','B'),(4,'Материал','Описание материала','M');
/*!40000 ALTER TABLE `materials` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `modelOperations`
--

DROP TABLE IF EXISTS `modelOperations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `modelOperations` (
  `id` int NOT NULL AUTO_INCREMENT,
  `modelId` int NOT NULL,
  `operationId` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `modelOperations_modelId_operationId_unique` (`modelId`,`operationId`),
  KEY `operationId` (`operationId`),
  CONSTRAINT `modelOperations_ibfk_1` FOREIGN KEY (`modelId`) REFERENCES `models` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `modelOperations_ibfk_2` FOREIGN KEY (`operationId`) REFERENCES `operations` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `modelOperations`
--

LOCK TABLES `modelOperations` WRITE;
/*!40000 ALTER TABLE `modelOperations` DISABLE KEYS */;
INSERT INTO `modelOperations` VALUES (10,1,2),(2,1,4),(3,2,1),(4,2,4),(5,2,5),(9,3,1),(13,3,2),(15,3,3),(14,3,4),(16,4,1),(17,4,2),(19,4,3),(18,4,4),(20,5,1),(21,5,2),(23,5,4),(22,5,5);
/*!40000 ALTER TABLE `modelOperations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `modelPrices`
--

DROP TABLE IF EXISTS `modelPrices`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `modelPrices` (
  `id` int NOT NULL AUTO_INCREMENT,
  `modelId` int NOT NULL,
  `priceId` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `modelPrices_modelId_priceId_unique` (`modelId`,`priceId`),
  KEY `priceId` (`priceId`),
  CONSTRAINT `modelPrices_ibfk_1` FOREIGN KEY (`modelId`) REFERENCES `models` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `modelPrices_ibfk_2` FOREIGN KEY (`priceId`) REFERENCES `prices` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `models` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `codeVendor` varchar(30) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `models`
--

LOCK TABLES `models` WRITE;
/*!40000 ALTER TABLE `models` DISABLE KEYS */;
INSERT INTO `models` VALUES (1,'Пижамные штаны тонкие','Пижамные штаны тонкие'),(2,'Боксеры муж','Боксеры муж'),(3,'Футболка дет однотон','Футболка дет однотон'),(4,'Модель 1','M'),(5,'Модель 2','M2');
/*!40000 ALTER TABLE `models` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `operations`
--

DROP TABLE IF EXISTS `operations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `operations` (
  `id` int NOT NULL AUTO_INCREMENT,
  `uid` varchar(5) NOT NULL,
  `name` varchar(30) NOT NULL,
  `description` varchar(255) NOT NULL,
  `percent` double NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operations`
--

LOCK TABLES `operations` WRITE;
/*!40000 ALTER TABLE `operations` DISABLE KEYS */;
INSERT INTO `operations` VALUES (1,'Over','Overlock','Особая строчка',1.5),(2,'YN','Универсальная','Univer',20),(3,'PL','Плоская','Плоская машина',20),(4,'GL','Глажка','Глажка описание',20),(5,' ','Бейка',' ',2.6);
/*!40000 ALTER TABLE `operations` ENABLE KEYS */;
UNLOCK TABLES;

DROP TABLE IF EXISTS `persons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `persons` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(50) NOT NULL,
  `password` varchar(300) NOT NULL,
  `lastName` varchar(40) NOT NULL,
  `firstName` varchar(40) NOT NULL,
  `patronymic` varchar(50) DEFAULT NULL,
  `birthDay` date NOT NULL,
  `uid` varchar(20) NOT NULL,
  `dateRegistration` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email` (`email`),
  UNIQUE KEY `uid` (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `persons`
--

LOCK TABLES `persons` WRITE;
/*!40000 ALTER TABLE `persons` DISABLE KEYS */;
INSERT INTO `persons` VALUES (1,'anna_fetisova@gmail.com','242ca47ff134998060a83cc17ebc6b40a60288740bec34c1789473c0b11c65e8','Фетисова','Анна','Николаевна','1987-10-10','anna','2023-11-11 00:00:00'),(2,'vogistv@gmail.com','242ca47ff134998060a83cc17ebc6b40a60288740bec34c1789473c0b11c65e8','Степанов','Виталий','Викторович','2003-12-01','vita','2023-11-30 04:00:49'),(3,'second@mail.ru','242ca47ff134998060a83cc17ebc6b40a60288740bec34c1789473c0b11c65e8','Амельи ','Катя',' ','2023-11-30','К','2023-11-30 05:09:01');
/*!40000 ALTER TABLE `persons` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `parties`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `parties` (
  `id` int NOT NULL AUTO_INCREMENT,
  `modelId` int NOT NULL,
  `personId` int NOT NULL,
  `priceId` int NOT NULL,
  `dateStart` date NOT NULL,
  `dateEnd` date DEFAULT NULL,
  `cutNumber` varchar(10) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `modelId` (`modelId`),
  KEY `personId` (`personId`),
  KEY `priceId` (`priceId`),
  CONSTRAINT `parties_ibfk_1` FOREIGN KEY (`modelId`) REFERENCES `models` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `parties_ibfk_2` FOREIGN KEY (`personId`) REFERENCES `persons` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `parties_ibfk_3` FOREIGN KEY (`priceId`) REFERENCES `prices` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


DROP TABLE IF EXISTS `packages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `packages` (
  `id` int NOT NULL AUTO_INCREMENT,
  `personId` int NOT NULL,
  `partyId` int NOT NULL,
  `sizeId` int NOT NULL,
  `count` int NOT NULL,
  `materialId` int NOT NULL,
  `isEnded` tinyint(1) NOT NULL DEFAULT '0',
  `isRepeat` tinyint(1) NOT NULL DEFAULT '0',
  `isUpdated` tinyint(1) NOT NULL DEFAULT '0',
  `createdAt` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `personId` (`personId`),
  KEY `partyId` (`partyId`),
  KEY `sizeId` (`sizeId`),
  KEY `materialId` (`materialId`),
  CONSTRAINT `packages_ibfk_1` FOREIGN KEY (`personId`) REFERENCES `persons` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `packages_ibfk_2` FOREIGN KEY (`partyId`) REFERENCES `parties` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `packages_ibfk_3` FOREIGN KEY (`sizeId`) REFERENCES `sizes` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `packages_ibfk_4` FOREIGN KEY (`materialId`) REFERENCES `materials` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=111 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `packages`
--

--
-- Table structure for table `parties`
--

--
-- Table structure for table `clothOperations`
--

DROP TABLE IF EXISTS `clothOperations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clothOperations` (
  `id` int NOT NULL AUTO_INCREMENT,
  `operationId` int NOT NULL,
  `packageId` int NOT NULL,
  `priceId` int NOT NULL,
  `isEnded` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `operationId` (`operationId`),
  KEY `packageId` (`packageId`),
  KEY `priceId` (`priceId`),
  CONSTRAINT `clothOperations_ibfk_1` FOREIGN KEY (`operationId`) REFERENCES `operations` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `clothOperations_ibfk_2` FOREIGN KEY (`packageId`) REFERENCES `packages` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `clothOperations_ibfk_3` FOREIGN KEY (`priceId`) REFERENCES `prices` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=223 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;


DROP TABLE IF EXISTS `clothOperationsPersons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clothOperationsPersons` (
  `id` int NOT NULL AUTO_INCREMENT,
  `personId` int NOT NULL,
  `clothOperationId` int NOT NULL,
  `dateStart` datetime NOT NULL,
  `isEnded` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `personId` (`personId`),
  KEY `clothOperationId` (`clothOperationId`),
  CONSTRAINT `clothOperationsPersons_ibfk_1` FOREIGN KEY (`personId`) REFERENCES `persons` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `clothOperationsPersons_ibfk_2` FOREIGN KEY (`clothOperationId`) REFERENCES `clothOperations` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
);


DROP TABLE IF EXISTS `posts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `posts` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(30) NOT NULL,
  `description` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
);
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `posts`
--

LOCK TABLES `posts` WRITE;
/*!40000 ALTER TABLE `posts` DISABLE KEYS */;
INSERT INTO `posts` VALUES (1,'EMPLOYEE','Работник'),(2,'ADMIN','Администратор данных'),(3,'CUTTER','Кройщик');
/*!40000 ALTER TABLE `posts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permissions`
--

DROP TABLE IF EXISTS `permissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permissions` (
  `id` int NOT NULL AUTO_INCREMENT,
  `personId` int NOT NULL,
  `postId` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `permissions_postId_personId_unique` (`personId`,`postId`),
  KEY `postId` (`postId`),
  CONSTRAINT `permissions_ibfk_1` FOREIGN KEY (`personId`) REFERENCES `persons` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `permissions_ibfk_2` FOREIGN KEY (`postId`) REFERENCES `posts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
);
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permissions`
--

LOCK TABLES `permissions` WRITE;
/*!40000 ALTER TABLE `permissions` DISABLE KEYS */;
INSERT INTO `permissions` VALUES (1,1,1),(2,1,2),(3,1,3),(4,2,1),(5,2,3),(6,3,1),(7,4,1),(8,4,3);
/*!40000 ALTER TABLE `permissions` ENABLE KEYS */;
UNLOCK TABLES;


DROP TABLE IF EXISTS `prices`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prices` (
  `id` int NOT NULL AUTO_INCREMENT,
  `number` float NOT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`)
);
/*!40101 SET character_set_client = @saved_cs_client */;


DROP TABLE IF EXISTS `sizes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sizes` (
  `id` int NOT NULL AUTO_INCREMENT,
  `number` varchar(10) NOT NULL,
  `ageId` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `ageId` (`ageId`),
  CONSTRAINT `sizes_ibfk_1` FOREIGN KEY (`ageId`) REFERENCES `ages` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
);
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sizes`
--

LOCK TABLES `sizes` WRITE;
/*!40000 ALTER TABLE `sizes` DISABLE KEYS */;
INSERT INTO `sizes` VALUES (1,'42',1),(2,'44',2),(3,'46',2),(10,'40-42',3),(11,'42-44',3),(12,'44-46',3);
/*!40000 ALTER TABLE `sizes` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-04-07 18:36:16

CREATE TABLE `links` (
  `id` int NOT NULL AUTO_INCREMENT,
  `rel` varchar(10) NOT NULL,
  `href` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
);


DROP TABLE IF EXISTS `histories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `histories` (
  `id` int NOT NULL AUTO_INCREMENT,
  `personId` int NOT NULL,
  `actionId` int NOT NULL,
  `tableName` varchar(30) NOT NULL,
  `value` varchar(100) NOT NULL,
  `createdAt` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `personId` (`personId`),
  KEY `actionId` (`actionId`),
  CONSTRAINT `histories_ibfk_1` FOREIGN KEY (`personId`) REFERENCES `persons` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `histories_ibfk_2` FOREIGN KEY (`actionId`) REFERENCES `actions` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1148 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

