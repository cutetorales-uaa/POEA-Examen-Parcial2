-- POEA - Examen Parcial 2
-- API REST VB.NET / EF Core 10 / SQL Server
-- Ejecutar en la MISMA instancia de SQL Server que utiliza la aplicación.
-- Este script crea una base independiente: InventarioParcial2.
-- No es una migración generada por EF Core.

USE [master];
GO

IF DB_ID(N'InventarioParcial2') IS NULL
BEGIN
    EXEC(N'CREATE DATABASE [InventarioParcial2]');
END;
GO

USE [InventarioParcial2];
GO

-- Categoria: relación 1:N con Producto.
IF OBJECT_ID(N'dbo.Categoria', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categoria
    (
        Id           INT IDENTITY(1,1) NOT NULL,
        Nombre       NVARCHAR(50) NOT NULL,
        Descripcion  NVARCHAR(200) NULL,
        CONSTRAINT PK_Categoria PRIMARY KEY (Id)
    );
END;
GO

-- Producto: la restricción FK exige una categoría existente.
IF OBJECT_ID(N'dbo.Producto', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Producto
    (
        Id             INT IDENTITY(1,1) NOT NULL,
        Nombre         NVARCHAR(50) NOT NULL,
        Descripcion    NVARCHAR(200) NULL,
        Precio         DECIMAL(18,2) NOT NULL,
        Stock          INT NOT NULL,
        FechaRegistro  DATETIME2(7) NOT NULL,
        CategoriaID    INT NOT NULL,
        CONSTRAINT PK_Producto PRIMARY KEY (Id),
        CONSTRAINT FK_Producto_Categoria_CategoriaID
            FOREIGN KEY (CategoriaID)
            REFERENCES dbo.Categoria(Id)
            ON DELETE NO ACTION,
        CONSTRAINT CK_Producto_Precio_Positivo CHECK (Precio > 0),
        CONSTRAINT CK_Producto_Stock_NoNegativo CHECK (Stock >= 0)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Producto_CategoriaID'
      AND object_id = OBJECT_ID(N'dbo.Producto')
)
BEGIN
    CREATE INDEX IX_Producto_CategoriaID
    ON dbo.Producto (CategoriaID);
END;
GO

-- Compatibilidad con el DbSet(Of Usuario) presente en AppDbContext.
-- Las columnas coinciden con la clase Usuario del repositorio original.
IF OBJECT_ID(N'dbo.Usuario', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuario
    (
        usuarioID    INT IDENTITY(1,1) NOT NULL,
        nombre       NVARCHAR(MAX) NULL,
        Email        NVARCHAR(MAX) NULL,
        PasswordHash NVARCHAR(MAX) NULL,
        Rol          NVARCHAR(MAX) NULL,
        Activo       BIT NOT NULL,
        CONSTRAINT PK_Usuario PRIMARY KEY (usuarioID)
    );
END;
GO

-- Comprobaciones: deben aparecer Categoria, Producto y Usuario.
SELECT t.name AS Tabla
FROM sys.tables AS t
WHERE t.name IN (N'Categoria', N'Producto', N'Usuario')
ORDER BY t.name;

SELECT fk.name AS ClaveForanea,
       OBJECT_NAME(fk.parent_object_id) AS TablaHija,
       OBJECT_NAME(fk.referenced_object_id) AS TablaPadre
FROM sys.foreign_keys AS fk
WHERE fk.name = N'FK_Producto_Categoria_CategoriaID';
GO
