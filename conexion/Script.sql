USE master;
GO

IF DB_ID(N'db_distribuidora_licores') IS NOT NULL
BEGIN
    ALTER DATABASE db_distribuidora_licores SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE db_distribuidora_licores;
END
GO

CREATE DATABASE db_distribuidora_licores;

GO
USE db_distribuidora_licores;
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


CREATE TABLE CategoriasProducto (
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
    SKU             NVARCHAR(40) NOT NULL UNIQUE,
    Nombre          NVARCHAR(150) NOT NULL,
    IdCategoria     INT NOT NULL,
    IdImpuesto      INT NULL,
    VolumenML       INT NOT NULL CHECK (VolumenML > 0),               
    GradAlcoholico  DECIMAL(5,2) NOT NULL CHECK (GradAlcoholico BETWEEN 0 AND 80),
    PrecioLista     DECIMAL(12,2) NOT NULL CHECK (PrecioLista >= 0), 
    Activo          BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Prod_Cat FOREIGN KEY (IdCategoria) REFERENCES CategoriasProducto(IdCategoria),
    CONSTRAINT FK_Prod_Imp FOREIGN KEY (IdImpuesto)  REFERENCES Impuestos(IdImpuesto)
);
GO

-- (8) Almacenes / Bodegas
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

-- (12) Detalles de pedido de compra
CREATE TABLE DetallesPedidosCompras (
    IdDetallePC    INT IDENTITY(1,1) PRIMARY KEY,
    IdPedidoCompra INT NOT NULL,
    IdProducto     INT NOT NULL,
    Cantidad       INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario DECIMAL(12,2) NOT NULL CHECK (PrecioUnitario >= 0),
    CONSTRAINT FK_DPC_PC   FOREIGN KEY (IdPedidoCompra) REFERENCES PedidosCompra(IdPedidoCompra),
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


CREATE TABLE DetallesPedidoVentas (
    IdDetallePV     INT IDENTITY(1,1) PRIMARY KEY,
    IdPedidoVenta   INT NOT NULL,
    IdProducto      INT NOT NULL,
    Cantidad        INT NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario  DECIMAL(12,2) NOT NULL CHECK (PrecioUnitario >= 0),
    IdImpuesto      INT NULL,
    CONSTRAINT FK_DPV_PV    FOREIGN KEY (IdPedidoVenta) REFERENCES PedidosVenta(IdPedidoVenta),
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
    CONSTRAINT FK_Pagos_PV FOREIGN KEY (IdPedidoVenta) REFERENCES PedidosVenta(IdPedidoVenta)
);
GO
