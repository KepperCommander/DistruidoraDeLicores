CREATE DATABASE DISTRIBUIDORA_2;
GO
USE DISTRIBUIDORA_2;
GO


CREATE TABLE Roles (
    IdRol    INT IDENTITY(1,1) PRIMARY KEY,
    Nombre   NVARCHAR(50) NOT NULL UNIQUE, 
    EsActivo BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Empleados (
    IdEmpleado    INT IDENTITY(1,1) PRIMARY KEY,
    Nombres       NVARCHAR(100) NOT NULL,
    Apellidos     NVARCHAR(100) NOT NULL,
    Email         NVARCHAR(150) NULL UNIQUE,
    Telefono      NVARCHAR(30)  NULL,
    IdRol         INT NOT NULL,
    FechaIngreso  DATE NOT NULL DEFAULT (CAST(GETDATE() AS DATE)),
    Activo        BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Empleados_Roles FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)
);
GO

CREATE TABLE Proveedores (
    IdProveedor INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial NVARCHAR(150) NOT NULL,
    NIT         NVARCHAR(30)  NOT NULL UNIQUE,
    Email       NVARCHAR(150) NULL,
    Telefono    NVARCHAR(30)  NULL,
    Direccion   NVARCHAR(200) NULL,
    Activo      BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Clientes (
    IdCliente   INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial NVARCHAR(150) NOT NULL,
    NIT         NVARCHAR(30)  NOT NULL UNIQUE,
    Email       NVARCHAR(150) NULL,
    Telefono    NVARCHAR(30)  NULL,
    Direccion   NVARCHAR(200) NULL,
    Activo      BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE CategoriasProductos (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(100) NOT NULL UNIQUE,
    Descripcion NVARCHAR(250) NULL
);
GO

CREATE TABLE Impuestos (
    IdImpuesto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre     NVARCHAR(100) NOT NULL UNIQUE,
    Porcentaje DECIMAL(5,2) NOT NULL CHECK (Porcentaje BETWEEN 0 AND 100)
);
GO

CREATE TABLE Productos (
    IdProducto      INT IDENTITY(1,1) PRIMARY KEY,
    Codigo             NVARCHAR(40) NOT NULL UNIQUE,
    Nombre          NVARCHAR(150) NOT NULL,
    IdCategoria     INT NOT NULL,
    IdImpuesto      INT NULL,
    VolumenML       INT NOT NULL CHECK (VolumenML > 0),               
    GradAlcoholico  DECIMAL(5,2) NOT NULL CHECK (GradAlcoholico BETWEEN 0 AND 80),
    PrecioLista     DECIMAL(12,2) NOT NULL CHECK (PrecioLista >= 0), 
    Activo          BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Prod_Cat FOREIGN KEY (IdCategoria) REFERENCES CategoriasProductos(IdCategoria),
    CONSTRAINT FK_Prod_Imp FOREIGN KEY (IdImpuesto)  REFERENCES Impuestos(IdImpuesto)
);
GO

CREATE TABLE Almacenes (
    IdAlmacen   INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(100) NOT NULL UNIQUE,
    Direccion   NVARCHAR(200) NULL,
    EsPrincipal BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE InventariosProductos (
    IdInventarioProd INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto       INT NOT NULL,
    IdAlmacen        INT NOT NULL,
    CantidadUnidades INT NOT NULL CHECK (CantidadUnidades >= 0),
    CONSTRAINT UQ_InvProd UNIQUE (IdProducto, IdAlmacen),
    CONSTRAINT FK_InvProd_Prod FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto),
    CONSTRAINT FK_InvProd_Alm  FOREIGN KEY (IdAlmacen)  REFERENCES Almacenes(IdAlmacen)
);
GO

CREATE TABLE MovimientosProductos (
    IdMovProd        INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto       INT NOT NULL,
    IdAlmacen        INT NOT NULL,
    Fecha            DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Tipo             NVARCHAR(20) NOT NULL CHECK (Tipo IN ('ENTRADA','SALIDA','TRANSFER')),
    CantidadUnidades INT NOT NULL CHECK (CantidadUnidades > 0),
    Referencia       NVARCHAR(100) NULL, 
    CONSTRAINT FK_MovProd_Prod FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto),
    CONSTRAINT FK_MovProd_Alm  FOREIGN KEY (IdAlmacen)  REFERENCES Almacenes(IdAlmacen)
);
GO

CREATE TABLE PedidosCompras (
    IdPedidoCompra INT IDENTITY(1,1) PRIMARY KEY,
    Numero         NVARCHAR(30) NOT NULL UNIQUE,
    IdProveedor    INT NOT NULL,
    IdEmpleado     INT NULL, -- quien genera
    Fecha          DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Estado         NVARCHAR(20) NOT NULL DEFAULT 'ABIERTA'
                   CHECK (Estado IN ('ABIERTA','RECIBIDA','CERRADA','ANULADA')),
    CONSTRAINT FK_PC_Prov FOREIGN KEY (IdProveedor) REFERENCES Proveedores(IdProveedor),
    CONSTRAINT FK_PC_Emp  FOREIGN KEY (IdEmpleado)  REFERENCES Empleados(IdEmpleado)
);
GO

CREATE TABLE DetallesPedidosCompras (
    IdDetallePC    INT IDENTITY(1,1) PRIMARY KEY,
    IdPedidoCompra INT NOT NULL,
    IdProducto     INT NOT NULL,
    Cantidad       INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(12,2) NOT NULL CHECK (PrecioUnitario >= 0),
    CONSTRAINT FK_DPC_PC   FOREIGN KEY (IdPedidoCompra) REFERENCES PedidosCompras(IdPedidoCompra),
    CONSTRAINT FK_DPC_Prod FOREIGN KEY (IdProducto)     REFERENCES Productos(IdProducto)
);
GO

CREATE TABLE PedidosVentas (
    IdPedidoVenta INT IDENTITY(1,1) PRIMARY KEY,
    Numero        NVARCHAR(30) NOT NULL UNIQUE,
    IdCliente     INT NOT NULL,
    IdEmpleado    INT NULL, -- asesor
    Fecha         DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Estado        NVARCHAR(20) NOT NULL DEFAULT 'ABIERTA'
                  CHECK (Estado IN ('ABIERTA','FACTURADA','ENVIADA','CERRADA','ANULADA')),
    CONSTRAINT FK_PV_Cliente FOREIGN KEY (IdCliente)  REFERENCES Clientes(IdCliente),
    CONSTRAINT FK_PV_Emp     FOREIGN KEY (IdEmpleado) REFERENCES Empleados(IdEmpleado)
);
GO

CREATE TABLE DetallesPedidosVentas (
    IdDetallePV     INT IDENTITY(1,1) PRIMARY KEY,
    IdPedidoVenta   INT NOT NULL,
    IdProducto      INT NOT NULL,
    Cantidad        INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario  DECIMAL(12,2) NOT NULL CHECK (PrecioUnitario >= 0),
    IdImpuesto      INT NULL,
    CONSTRAINT FK_DPV_PV    FOREIGN KEY (IdPedidoVenta) REFERENCES PedidosVentas(IdPedidoVenta),
    CONSTRAINT FK_DPV_Prod  FOREIGN KEY (IdProducto)    REFERENCES Productos(IdProducto),
    CONSTRAINT FK_DPV_Imp   FOREIGN KEY (IdImpuesto)    REFERENCES Impuestos(IdImpuesto)
);
GO

CREATE TABLE Pagos (
    IdPago        INT IDENTITY(1,1) PRIMARY KEY,
    IdPedidoVenta INT NOT NULL,
    Fecha         DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Monto         DECIMAL(12,2) NOT NULL CHECK (Monto > 0),
    Medio         NVARCHAR(30) NOT NULL CHECK (Medio IN ('EFECTIVO','TRANSFERENCIA','TARJETA','OTRO')),
    Referencia    NVARCHAR(100) NULL,
    CONSTRAINT FK_Pagos_PV FOREIGN KEY (IdPedidoVenta) REFERENCES PedidosVentas(IdPedidoVenta)
);
GO


INSERT INTO Roles (Nombre) VALUES
('Administrador'), ('Vendedor'), ('Bodeguero'), ('Contador'), ('Repartidor');


INSERT INTO Empleados (Nombres, Apellidos, Email, Telefono, IdRol)
VALUES
('Juan', 'Pérez', 'juan.perez@empresa.com', '3001111111', 1),
('María', 'López', 'maria.lopez@empresa.com', '3002222222', 2),
('Carlos', 'Gómez', 'carlos.gomez@empresa.com', '3003333333', 3),
('Ana', 'Martínez', 'ana.martinez@empresa.com', '3004444444', 4),
('Pedro', 'Ramírez', 'pedro.ramirez@empresa.com', '3005555555', 5);


INSERT INTO Proveedores (RazonSocial, NIT, Email, Telefono, Direccion)
VALUES
('Proveedor A', '900111111', 'provA@correo.com', '3101111111', 'Cra 1 #10-20'),
('Proveedor B', '900222222', 'provB@correo.com', '3102222222', 'Cra 2 #11-21'),
('Proveedor C', '900333333', 'provC@correo.com', '3103333333', 'Cra 3 #12-22'),
('Proveedor D', '900444444', 'provD@correo.com', '3104444444', 'Cra 4 #13-23'),
('Proveedor E', '900555555', 'provE@correo.com', '3105555555', 'Cra 5 #14-24');


INSERT INTO Clientes (RazonSocial, NIT, Email, Telefono, Direccion)
VALUES
('Cliente A', '800111111', 'cliA@correo.com', '3201111111', 'Cll 10 #20-30'),
('Cliente B', '800222222', 'cliB@correo.com', '3202222222', 'Cll 11 #21-31'),
('Cliente C', '800333333', 'cliC@correo.com', '3203333333', 'Cll 12 #22-32'),
('Cliente D', '800444444', 'cliD@correo.com', '3204444444', 'Cll 13 #23-33'),
('Cliente E', '800555555', 'cliE@correo.com', '3205555555', 'Cll 14 #24-34');


INSERT INTO CategoriasProductos (Nombre, Descripcion)
VALUES
('Cerveza', 'Cervezas nacionales e importadas'),
('Ron', 'Variedades de ron'),
('Whisky', 'Whisky nacional e importado'),
('Vodka', 'Marcas de vodka'),
('Vino', 'Vinos tintos y blancos');


INSERT INTO Impuestos (Nombre, Porcentaje)
VALUES
('IVA 19%', 19.00),
('IVA 5%', 5.00),
('Exento', 0.00),
('Impuesto Licores 25%', 25.00),
('Impuesto Especial 10%', 10.00);


INSERT INTO Productos (Codigo, Nombre, IdCategoria, IdImpuesto, VolumenML, GradAlcoholico, PrecioLista)
VALUES
('P001', 'Cerveza Águila', 1, 1, 330, 4.50, 2500),
('P002', 'Ron Medellín', 2, 4, 750, 35.00, 45000),
('P003', 'Whisky Old Parr', 3, 4, 750, 40.00, 120000),
('P004', 'Vodka Absolut', 4, 4, 750, 38.00, 95000),
('P005', 'Vino Gato Negro', 5, 2, 750, 12.00, 40000);


INSERT INTO Almacenes (Nombre, Direccion, EsPrincipal)
VALUES
('Bodega Central', 'Zona Industrial #1', 1),
('Sucursal Norte', 'Av 5 #45-12', 0),
('Sucursal Sur', 'Av 80 #20-30', 0),
('Sucursal Oriente', 'Cll 50 #10-25', 0),
('Sucursal Occidente', 'Cra 100 #25-50', 0);


INSERT INTO InventariosProductos (IdProducto, IdAlmacen, CantidadUnidades)
VALUES
(1, 1, 500),
(2, 1, 200),
(3, 2, 50),
(4, 3, 70),
(5, 4, 100);


INSERT INTO MovimientosProductos (IdProducto, IdAlmacen, Tipo, CantidadUnidades, Referencia)
VALUES
(1, 1, 'ENTRADA', 100, 'Compra inicial'),
(2, 1, 'ENTRADA', 50, 'Compra inicial'),
(3, 2, 'ENTRADA', 20, 'Compra inicial'),
(4, 3, 'ENTRADA', 30, 'Compra inicial'),
(5, 4, 'ENTRADA', 40, 'Compra inicial');


INSERT INTO PedidosCompras (Numero, IdProveedor, IdEmpleado, Estado)
VALUES
('PC001', 1, 1, 'ABIERTA'),
('PC002', 2, 2, 'RECIBIDA'),
('PC003', 3, 3, 'CERRADA'),
('PC004', 4, 4, 'ANULADA'),
('PC005', 5, 5, 'ABIERTA');

INSERT INTO DetallesPedidosCompras (IdPedidoCompra, IdProducto, Cantidad, PrecioUnitario)
VALUES
(1, 1, 100, 2000),
(2, 2, 50, 40000),
(3, 3, 20, 100000),
(4, 4, 30, 90000),
(5, 5, 40, 35000);


INSERT INTO PedidosVentas (Numero, IdCliente, IdEmpleado, Estado)
VALUES
('PV001', 1, 1, 'ABIERTA'),
('PV002', 2, 2, 'FACTURADA'),
('PV003', 3, 3, 'ENVIADA'),
('PV004', 4, 4, 'CERRADA'),
('PV005', 5, 5, 'ANULADA');


INSERT INTO DetallesPedidosVentas (IdPedidoVenta, IdProducto, Cantidad, PrecioUnitario, IdImpuesto)
VALUES
(1, 1, 10, 2500, 1),
(2, 2, 5, 45000, 4),
(3, 3, 2, 120000, 4),
(4, 4, 3, 95000, 4),
(5, 5, 4, 40000, 2);


INSERT INTO Pagos (IdPedidoVenta, Monto, Medio, Referencia)
VALUES
(1, 25000, 'EFECTIVO', 'Caja001'),
(2, 225000, 'TARJETA', 'POS123'),
(3, 240000, 'TRANSFERENCIA', 'TRX998'),
(4, 285000, 'EFECTIVO', 'Caja002'),
(5, 160000, 'OTRO', 'Cheque001');
GO