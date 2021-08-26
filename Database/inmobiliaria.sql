-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1:3306
-- Tiempo de generación: 25-08-2021 a las 01:31:54
-- Versión del servidor: 8.0.21
-- Versión de PHP: 7.3.21

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
CREATE TABLE IF NOT EXISTS `inquilino` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Dni` varchar(16) COLLATE utf8_spanish_ci NOT NULL,
  `Nombre` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `FechaN` date NOT NULL,
  `DomicilioTrabajo` varchar(200) CHARACTER SET utf8 COLLATE utf8_spanish_ci NOT NULL,
  `Telefono` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `Email` varchar(56) COLLATE utf8_spanish_ci NOT NULL,
  `DniGarante` varchar(16) COLLATE utf8_spanish_ci NOT NULL,
  `NombreGarante` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `TelefonoGarante` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `EmailGarante` varchar(56) COLLATE utf8_spanish_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fecha_n` (`FechaN`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`Id`, `Dni`, `Nombre`, `FechaN`, `DomicilioTrabajo`, `Telefono`, `Email`, `DniGarante`, `NombreGarante`, `TelefonoGarante`, `EmailGarante`) VALUES
(1, '36222777', 'Franco Ezequiel', '2021-08-29', 'av 2 de abril', '1115151515', 'eze@correo.com', '34256456', 'Mario Avaca', '2664151515', 'mario@correo.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

DROP TABLE IF EXISTS `propietario`;
CREATE TABLE IF NOT EXISTS `propietario` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Dni` varchar(16) COLLATE utf8_spanish_ci NOT NULL,
  `Nombre` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `FechaN` date NOT NULL,
  `Domicilio` varchar(200) COLLATE utf8_spanish_ci NOT NULL,
  `Telefono` varchar(30) COLLATE utf8_spanish_ci NOT NULL,
  `Email` varchar(56) COLLATE utf8_spanish_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fecha_n` (`FechaN`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`Id`, `Dni`, `Nombre`, `FechaN`, `Domicilio`, `Telefono`, `Email`) VALUES
(1, '36222777', 'Franco Ezequiel', '2021-08-17', 'asdasd', '233423534', 'eze@correo.com'),
(2, '29000567', 'MarioAvaca', '2021-08-16', 'dfgdfhdfgh', '45346456', 'mario@correo.com');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
