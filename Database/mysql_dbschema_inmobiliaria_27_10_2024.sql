-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         5.7.41 - MySQL Community Server (GPL)
-- SO del servidor:              Linux
-- HeidiSQL Versión:             12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para inmobiliariaalbornoz
CREATE DATABASE IF NOT EXISTS `inmobiliariaalbornoz` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_spanish_ci */;
USE `inmobiliariaalbornoz`;

-- Volcando estructura para tabla inmobiliariaalbornoz.contrato
CREATE TABLE IF NOT EXISTS `contrato` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdInmueble` int(11) NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `Desde` date NOT NULL,
  `Hasta` date NOT NULL,
  `DniGarante` varchar(30) COLLATE utf8mb4_spanish_ci NOT NULL,
  `NombreGarante` varchar(100) COLLATE utf8mb4_spanish_ci NOT NULL,
  `TelefonoGarante` varchar(30) COLLATE utf8mb4_spanish_ci NOT NULL,
  `EmailGarante` varchar(56) COLLATE utf8mb4_spanish_ci DEFAULT NULL,
  `Valido` tinyint(4) NOT NULL DEFAULT '1' COMMENT 'Informa si el contrato fue roto o no.',
  `Monto` decimal(20,6) unsigned NOT NULL DEFAULT '0.000000',
  PRIMARY KEY (`Id`,`IdInmueble`,`IdInquilino`),
  KEY `fk_inmueble` (`IdInmueble`),
  KEY `fk_inquilino` (`IdInquilino`),
  CONSTRAINT `fk_inmueble` FOREIGN KEY (`IdInmueble`) REFERENCES `inmueble` (`Id`),
  CONSTRAINT `fk_inquilino` FOREIGN KEY (`IdInquilino`) REFERENCES `inquilino` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla inmobiliariaalbornoz.error_log
CREATE TABLE IF NOT EXISTS `error_log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `controller` varchar(56) NOT NULL,
  `action` varchar(56) NOT NULL,
  `message` varchar(1024) NOT NULL COMMENT 'detalles de la excepción.',
  `date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'momento de la excepción.',
  `user` varchar(56) DEFAULT NULL COMMENT 'email del usuario.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COMMENT='informe de excepciones';

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla inmobiliariaalbornoz.inmueble
CREATE TABLE IF NOT EXISTS `inmueble` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Direccion` varchar(200) COLLATE utf8_spanish_ci NOT NULL,
  `Tipo` int(11) NOT NULL,
  `Uso` int(11) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Precio` decimal(10,0) NOT NULL,
  `Disponible` tinyint(4) NOT NULL DEFAULT '1',
  `IdPropietario` int(11) NOT NULL,
  `ImageUrl` varchar(100) COLLATE utf8_spanish_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_propietario` (`IdPropietario`),
  CONSTRAINT `fk_propietario` FOREIGN KEY (`IdPropietario`) REFERENCES `propietario` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=123 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla inmobiliariaalbornoz.inquilino
CREATE TABLE IF NOT EXISTS `inquilino` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Dni` varchar(16) COLLATE utf8_spanish_ci NOT NULL,
  `Nombre` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `FechaN` date NOT NULL,
  `DomicilioTrabajo` varchar(200) COLLATE utf8_spanish_ci NOT NULL,
  `Telefono` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `Email` varchar(56) COLLATE utf8_spanish_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fecha_n` (`FechaN`)
) ENGINE=InnoDB AUTO_INCREMENT=105 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla inmobiliariaalbornoz.pago
CREATE TABLE IF NOT EXISTS `pago` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdContrato` int(11) NOT NULL,
  `Fecha` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `FechaCorrespondiente` date NOT NULL,
  `Monto` decimal(10,0) NOT NULL,
  `Tipo` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `Anulado` tinyint(4) NOT NULL DEFAULT '0' COMMENT 'Indica si el pago fue anulado',
  `Numero` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_IdContrato` (`IdContrato`),
  CONSTRAINT `fk_IdContrato` FOREIGN KEY (`IdContrato`) REFERENCES `contrato` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=121 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla inmobiliariaalbornoz.propietario
CREATE TABLE IF NOT EXISTS `propietario` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Dni` varchar(16) COLLATE utf8_spanish_ci NOT NULL,
  `Nombre` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `FechaN` date NOT NULL,
  `Domicilio` varchar(200) COLLATE utf8_spanish_ci NOT NULL,
  `Telefono` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `Email` varchar(56) COLLATE utf8_spanish_ci NOT NULL,
  `Clave` varchar(256) COLLATE utf8_spanish_ci NOT NULL,
  `AvatarUrl` varchar(200) COLLATE utf8_spanish_ci DEFAULT NULL,
  `Apellido` varchar(100) COLLATE utf8_spanish_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`),
  KEY `fecha_n` (`FechaN`)
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla inmobiliariaalbornoz.usuario
CREATE TABLE IF NOT EXISTS `usuario` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) COLLATE utf8mb4_spanish_ci NOT NULL,
  `Apellido` varchar(100) COLLATE utf8mb4_spanish_ci NOT NULL,
  `AvatarUrl` varchar(100) COLLATE utf8mb4_spanish_ci DEFAULT NULL,
  `Email` varchar(100) COLLATE utf8mb4_spanish_ci NOT NULL,
  `Clave` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
  `Rol` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `usuario_email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

-- La exportación de datos fue deseleccionada.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
