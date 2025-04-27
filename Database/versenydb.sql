-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1:3307
-- Létrehozás ideje: 2025. Ápr 26. 12:24
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `versenydb`
--
CREATE DATABASE IF NOT EXISTS `versenydb` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `versenydb`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `birok`
--

CREATE TABLE `birok` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Telefon` varchar(20) NOT NULL,
  `Jelszo` varchar(100) NOT NULL,
  `TitkosKulcs` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `birok`
--

INSERT INTO `birok` (`Id`, `Nev`, `Email`, `Telefon`, `Jelszo`, `TitkosKulcs`) VALUES
(10, 'Szalay Miklós', 'turbomiki06@gmail.com', '+36203413002', '5c34b0be85ad43ab838270cabee5f32f45c1141977c6d4fb52b8420b5529aa3e', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'),
(11, 'Teszt Elek', 'teszt@teszt.hu', '+36201234567', 'da79250286f9dd54cb243147b7d4a92dd6891248801c2ad92fb16108374bf1f9', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `csapatok`
--

CREATE TABLE `csapatok` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(100) NOT NULL,
  `Letszam` int(11) NOT NULL,
  `ZeneLink` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `csapatok`
--

INSERT INTO `csapatok` (`Id`, `Nev`, `Letszam`, `ZeneLink`) VALUES
(13, 'Tolószékesek', 1, NULL);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `csapattagok`
--

CREATE TABLE `csapattagok` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(100) NOT NULL,
  `CsapatId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `csapattagok`
--

INSERT INTO `csapattagok` (`Id`, `Nev`, `CsapatId`) VALUES
(27, 'Gáspár', 13);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `eredmenyek`
--

CREATE TABLE `eredmenyek` (
  `Id` int(11) NOT NULL,
  `NevezesId` int(11) NOT NULL,
  `BiroId` int(11) NOT NULL,
  `Pontszam` int(11) DEFAULT NULL,
  `Rogzitve` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `eredmenyek`
--

INSERT INTO `eredmenyek` (`Id`, `NevezesId`, `BiroId`, `Pontszam`, `Rogzitve`) VALUES
(34, 13, 10, 50, '2025-04-26 11:18:54');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `kategoriak`
--

CREATE TABLE `kategoriak` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `kategoriak`
--

INSERT INTO `kategoriak` (`Id`, `Nev`) VALUES
(1, 'Junior'),
(2, 'Felnőtt'),
(3, 'Senior'),
(4, 'Gyermek'),
(5, 'Profi');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `nevezesek`
--

CREATE TABLE `nevezesek` (
  `Id` int(11) NOT NULL,
  `BiroId` int(11) DEFAULT NULL,
  `VersenyId` int(11) NOT NULL,
  `CsapatId` int(11) NOT NULL,
  `KategoriaId` int(11) DEFAULT NULL,
  `Datum` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `nevezesek`
--

INSERT INTO `nevezesek` (`Id`, `BiroId`, `VersenyId`, `CsapatId`, `KategoriaId`, `Datum`) VALUES
(13, NULL, 8, 13, 1, '2025-04-26 11:18:26');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `versenyek`
--

CREATE TABLE `versenyek` (
  `Id` int(11) NOT NULL,
  `Nev` varchar(100) NOT NULL,
  `Idopont` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `versenyek`
--

INSERT INTO `versenyek` (`Id`, `Nev`, `Idopont`) VALUES
(8, 'Lorigo', '2025-04-27 15:20:00');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `versenykategoriak`
--

CREATE TABLE `versenykategoriak` (
  `VersenyId` int(11) NOT NULL,
  `KategoriaId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `versenykategoriak`
--

INSERT INTO `versenykategoriak` (`VersenyId`, `KategoriaId`) VALUES
(8, 1),
(8, 2),
(8, 3),
(8, 4),
(8, 5);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `birok`
--
ALTER TABLE `birok`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `csapatok`
--
ALTER TABLE `csapatok`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `csapattagok`
--
ALTER TABLE `csapattagok`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `CsapatId` (`CsapatId`);

--
-- A tábla indexei `eredmenyek`
--
ALTER TABLE `eredmenyek`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `NevezesId` (`NevezesId`),
  ADD KEY `BiroId` (`BiroId`);

--
-- A tábla indexei `kategoriak`
--
ALTER TABLE `kategoriak`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `nevezesek`
--
ALTER TABLE `nevezesek`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `BiroId` (`BiroId`),
  ADD KEY `VersenyId` (`VersenyId`),
  ADD KEY `CsapatId` (`CsapatId`),
  ADD KEY `fk_nevezesek_kategoriak` (`KategoriaId`);

--
-- A tábla indexei `versenyek`
--
ALTER TABLE `versenyek`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `versenykategoriak`
--
ALTER TABLE `versenykategoriak`
  ADD PRIMARY KEY (`VersenyId`,`KategoriaId`),
  ADD KEY `KategoriaId` (`KategoriaId`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `birok`
--
ALTER TABLE `birok`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT a táblához `csapatok`
--
ALTER TABLE `csapatok`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT a táblához `csapattagok`
--
ALTER TABLE `csapattagok`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT a táblához `eredmenyek`
--
ALTER TABLE `eredmenyek`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- AUTO_INCREMENT a táblához `kategoriak`
--
ALTER TABLE `kategoriak`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `nevezesek`
--
ALTER TABLE `nevezesek`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT a táblához `versenyek`
--
ALTER TABLE `versenyek`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `csapattagok`
--
ALTER TABLE `csapattagok`
  ADD CONSTRAINT `csapattagok_ibfk_1` FOREIGN KEY (`CsapatId`) REFERENCES `csapatok` (`Id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `eredmenyek`
--
ALTER TABLE `eredmenyek`
  ADD CONSTRAINT `eredmenyek_ibfk_1` FOREIGN KEY (`NevezesId`) REFERENCES `nevezesek` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `eredmenyek_ibfk_2` FOREIGN KEY (`BiroId`) REFERENCES `birok` (`Id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `nevezesek`
--
ALTER TABLE `nevezesek`
  ADD CONSTRAINT `fk_nevezesek_kategoriak` FOREIGN KEY (`KategoriaId`) REFERENCES `kategoriak` (`Id`) ON DELETE SET NULL,
  ADD CONSTRAINT `nevezesek_ibfk_1` FOREIGN KEY (`BiroId`) REFERENCES `birok` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `nevezesek_ibfk_2` FOREIGN KEY (`VersenyId`) REFERENCES `versenyek` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `nevezesek_ibfk_3` FOREIGN KEY (`CsapatId`) REFERENCES `csapatok` (`Id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `versenykategoriak`
--
ALTER TABLE `versenykategoriak`
  ADD CONSTRAINT `versenykategoriak_ibfk_1` FOREIGN KEY (`VersenyId`) REFERENCES `versenyek` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `versenykategoriak_ibfk_2` FOREIGN KEY (`KategoriaId`) REFERENCES `kategoriak` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
