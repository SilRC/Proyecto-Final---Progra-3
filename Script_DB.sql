CREATE TABLE Usuarios (
UserId INT IDENTITY(1,1) PRIMARY KEY,
NombreUsuario VARCHAR(50) NOT NULL,
Email VARCHAR(100) UNIQUE NOT NULL,
Contrasena VARCHAR(100) NOT NULL
);

CREATE TABLE Cuentas (
CuentaId INT IDENTITY(1,1) PRIMARY KEY,
UserId INT,
NombreCuenta VARCHAR(100) NOT NULL,
SaldoInicial DECIMAL(10,2) DEFAULT 0,
FechaCreacion DateTime,
CONSTRAINT FK_UsuarioId FOREIGN KEY (UserId) REFERENCES Usuarios(UserId)
);

CREATE TABLE Categorias (
CategoriaId INT IDENTITY(1,1) PRIMARY KEY,
NombreCategoria VARCHAR(100) NOT NULL
);

CREATE TABLE Gastos (
GastoId INT IDENTITY(1,1) PRIMARY KEY,
CuentaId INT,
CategoriaId INT,
Descripcion VARCHAR(50) NOT NULL,
Monto DECIMAL(10,2) NOT NULL,
FechaGasto DATE NOT NULL,
CONSTRAINT FK_CuentaId FOREIGN KEY (CuentaId) REFERENCES Cuentas(CuentaId),
CONSTRAINT FK_CategoriaId FOREIGN KEY (CategoriaId) REFERENCES Categorias(CategoriaId)
);

CREATE TABLE Ingresos (
IngresoId INT IDENTITY(1,1) PRIMARY KEY,
CuentaId INT,
Descripcion VARCHAR(50) NOT NULL,
Monto DECIMAL(10,2) NOT NULL,
FechaIngreso DATE NOT NULL,
CONSTRAINT FK_Ingreso_CuentaId FOREIGN KEY (CuentaId) REFERENCES Cuentas(CuentaId)
);



INSERT INTO Usuarios (NombreUsuario, Email, Contrasena)
VALUES ('Admin', 'admin@email.com', 'admin123');

INSERT INTO Usuarios (NombreUsuario, Email, Contrasena)
VALUES ('User', 'user@email.com', 'user123');


select * from Usuarios;

