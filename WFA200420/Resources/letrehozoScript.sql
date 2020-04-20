--CREATE DATABASE tripat;
--USE tripat;

CREATE TABLE tricikli (
tricId INT PRIMARY KEY IDENTITY,
nagy BIT,
szin VARCHAR(20));

CREATE TABLE patkany (
patkanyId INT PRIMARY KEY IDENTITY,
nev VARCHAR(30),
alfaj VARCHAR(20));

CREATE TABLE kolcsonzes (
kolcsonId INT PRIMARY KEY IDENTITY,
datum DATE,
sofor INT FOREIGN KEY REFERENCES patkany(patkanyId),
jarmu INT FOREIGN KEY REFERENCES tricikli(tricId));


INSERT INTO tricikli (nagy, szin) VALUES
(0, 'piros'),
(1, 'kék'),
(0, 'zöld'),
(0, 'kék'),
(1, 'zöld'),
(0, 'sárga'),
(0, 'piros');

INSERT INTO patkany (nev, alfaj) VALUES
('Gut', 'házi patkány'),
('Bear', 'házi patkány'),
('Jerry', 'vándorpatkány'),
('Spring', 'cricetomys'),
('Snoopy', 'házi patkány'),
('Graye', 'vándorpatkány'),
('Antony', 'cricetomys');

INSERT INTO kolcsonzes (datum, sofor, jarmu) VALUES
('2019-05-02', 1, 1),
('2019-05-03', 1, 2),
('2019-05-03', 3, 4),
('2019-05-03', 6, 5),
('2019-05-04', 2, 3),
('2019-05-04', 3, 4),
('2019-05-04', 4, 5),
('2019-05-05', 2, 1),
('2019-05-05', 6, 2),
('2019-05-05', 7, 3);