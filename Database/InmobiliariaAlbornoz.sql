-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1:3306
-- Tiempo de generación: 28-08-2021 a las 20:50:03
-- Versión del servidor: 5.7.31
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
-- Estructura de tabla para la tabla `contrato`
--

DROP TABLE IF EXISTS `contrato`;
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
  PRIMARY KEY (`Id`,`IdInmueble`,`IdInquilino`),
  KEY `fk_inmueble` (`IdInmueble`),
  KEY `fk_inquilino` (`IdInquilino`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id`, `IdInmueble`, `IdInquilino`, `Desde`, `Hasta`, `DniGarante`, `NombreGarante`, `TelefonoGarante`, `EmailGarante`) VALUES
(7, 4, 3, '2021-08-05', '2021-08-29', '32777999', 'Mario Raúl Avaca', '423523452345', 'mario2@correo.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
CREATE TABLE IF NOT EXISTS `inmueble` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Direccion` varchar(200) COLLATE utf8_spanish_ci NOT NULL,
  `Tipo` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `Uso` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Precio` decimal(10,0) NOT NULL,
  `Disponible` tinyint(4) NOT NULL DEFAULT '1',
  `IdPropietario` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_propietario` (`IdPropietario`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`Id`, `Direccion`, `Tipo`, `Uso`, `Ambientes`, `Precio`, `Disponible`, `IdPropietario`) VALUES
(3, 'ufff, allá por el lince', 'Fondo de comercio', 'Comercial', 2, '1500000', 1, 2),
(4, '2 de Abril', 'PH', 'Vivienda', 5, '7800500', 1, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`Id`, `Dni`, `Nombre`, `FechaN`, `DomicilioTrabajo`, `Telefono`, `Email`) VALUES
(3, '36227971', 'Ezequiel Albornoz', '2021-08-11', 'JAJAJAJAJAJA', '2664151515', 'eze@correo.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

DROP TABLE IF EXISTS `propietario`;
CREATE TABLE IF NOT EXISTS `propietario` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
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

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `fk_inmueble` FOREIGN KEY (`IdInmueble`) REFERENCES `inmueble` (`Id`),
  ADD CONSTRAINT `fk_inquilino` FOREIGN KEY (`IdInquilino`) REFERENCES `inquilino` (`Id`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `fk_propietario` FOREIGN KEY (`IdPropietario`) REFERENCES `propietario` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
