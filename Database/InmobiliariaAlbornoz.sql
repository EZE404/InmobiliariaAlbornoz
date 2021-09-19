-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1:3306
-- Tiempo de generación: 19-09-2021 a las 07:09:13
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
-- Base de datos: `inmobiliariaalbornoz`
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
  `Valido` tinyint(4) NOT NULL DEFAULT '1' COMMENT 'Informa si el contrato fue roto o no.',
  PRIMARY KEY (`Id`,`IdInmueble`,`IdInquilino`),
  KEY `fk_inmueble` (`IdInmueble`),
  KEY `fk_inquilino` (`IdInquilino`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id`, `IdInmueble`, `IdInquilino`, `Desde`, `Hasta`, `DniGarante`, `NombreGarante`, `TelefonoGarante`, `EmailGarante`, `Valido`) VALUES
(8, 4, 3, '2021-08-03', '2023-06-16', '32777999', 'Mario Raúl Avaca', '2664101010', 'mario@correo.com', 1),
(9, 9, 4, '2020-01-23', '2021-09-17', '32452452', 'Mariano Luzza', '234354567', 'mluzza@correo.com', 1),
(10, 8, 3, '2021-09-07', '2022-06-22', '32452452', 'Mariano Luzza', '234354567', 'mluzza@correo.com', 0),
(11, 9, 4, '2021-10-10', '2022-10-10', '32452452', 'Mariano Luzza', '234354567', 'mluzza@correo.com', 0);

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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`Id`, `Direccion`, `Tipo`, `Uso`, `Ambientes`, `Precio`, `Disponible`, `IdPropietario`) VALUES
(4, '2 de Abril', 'PH', 'Vivienda', 5, '7800500', 1, 1),
(5, 'Riobamba', 'Fondo de comercio', 'comercial', 2, '5000000', 1, 2),
(6, 'Martín de Loyola', 'Vivienda', 'Vivienda', 5, '9000000', 0, 2),
(7, 'Chacabuco', 'Departamento', 'VIvienda', 3, '4500000', 1, 1),
(8, 'Italia', 'Casa', 'vivienda', 4, '7800000', 1, 3),
(9, 'Tomolasta', 'depósito', 'comercial', 2, '6300000', 1, 4);

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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`Id`, `Dni`, `Nombre`, `FechaN`, `DomicilioTrabajo`, `Telefono`, `Email`) VALUES
(3, '36227971', 'Ezequiel Albornoz', '2021-08-11', 'JAJAJAJAJAJA', '2664151515', 'eze@correo.com'),
(4, '30111222', 'Mario Avaca', '2021-09-24', 'sdfdsfasfsd', '2664151515', 'mario@correo.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

DROP TABLE IF EXISTS `pago`;
CREATE TABLE IF NOT EXISTS `pago` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdContrato` int(11) NOT NULL,
  `Fecha` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `FechaCorrespondiente` date NOT NULL,
  `Monto` decimal(10,0) NOT NULL,
  `Tipo` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `Anulado` tinyint(4) NOT NULL DEFAULT '0' COMMENT 'Indica si el pago fue anulado',
  PRIMARY KEY (`Id`),
  KEY `fk_IdContrato` (`IdContrato`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`Id`, `IdContrato`, `Fecha`, `FechaCorrespondiente`, `Monto`, `Tipo`, `Anulado`) VALUES
(1, 8, '2021-09-14 13:10:26', '2021-09-13', '2750000', 'Editado', 1),
(2, 8, '2021-09-18 21:31:32', '2021-09-18', '22000', 'cuota', 0),
(3, 8, '2021-09-18 21:38:00', '2021-09-19', '21000', 'sdasd', 0),
(4, 8, '2021-09-18 21:39:56', '2021-09-15', '10000', 'holis', 0),
(5, 8, '2021-09-18 21:42:16', '2021-09-23', '23123123', 'alert', 0),
(6, 8, '2021-09-18 21:50:34', '2021-09-26', '12112121', 'cuota', 0);

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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`Id`, `Dni`, `Nombre`, `FechaN`, `Domicilio`, `Telefono`, `Email`) VALUES
(1, '36222777', 'Franco Ezequiel', '2021-08-17', 'asdasd', '233423534', 'eze@correo.com'),
(2, '29000567', 'MarioAvaca', '2021-08-16', 'dfgdfhdfgh', '45346456', 'mario@correo.com'),
(3, '29666777', 'Genaro Farías', '1975-09-09', 'Aeropuerto', '453425425', 'genaro@mail.com'),
(4, '41999888', 'Gastón Sosa', '2021-09-09', 'Edesal', '266543654', 'gaston@correo.com');

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

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_IdContrato` FOREIGN KEY (`IdContrato`) REFERENCES `contrato` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
