# POEA — Examen Parcial 2: API REST de Inventario

API REST desarrollada en **Visual Basic .NET** con **ASP.NET Core**, **Entity Framework Core**, **SQL Server** y **Swagger**, para administrar productos y categorías.

## Tecnologías

- .NET 10 / ASP.NET Core Web API (Visual Basic .NET)
- Entity Framework Core 10 y proveedor SQL Server
- SQL Server Express
- Swagger / OpenAPI

## Estructura del proyecto

La solución se denomina `ApiLibro` y el proyecto se encuentra en `ApiDemoClase2/TareaProducto.vbproj`.

- `Controllers/`: endpoints HTTP de categorías y productos.
- `Models/`: entidades `Producto` y `Categoria`.
- `DTOs/`: contratos de entrada y salida y validaciones.
- `Data/`: `AppDbContext` y configuración de la relación uno a muchos.
- `Repositories/`: acceso a datos con Entity Framework Core.
- `Servicios/`: lógica y reglas de negocio.
- `Exceptions/` y `Middleware/`: errores de negocio y tratamiento centralizado de excepciones.

## Funciones principales

- CRUD de productos y categorías.
- Relación **1:N**: una categoría contiene varios productos y cada producto pertenece a una categoría.
- Consultas de productos con la información de la categoría, productos filtrados por categoría y categoría con sus productos.
- Validaciones de nombre, descripción, precio, stock y categoría.
- Restricción de duplicados por nombre, sin distinguir mayúsculas y minúsculas.
- Impedimento de eliminar categorías con productos asociados.

## Base de datos

El script `BaseDatos/InventarioParcial2.sql` permite reconstruir la estructura de la base `InventarioParcial2` en SQL Server.

1. Abrir SQL Server Management Studio y conectarse a la instancia correspondiente (por ejemplo, `.\\SQLEXPRESS`).
2. Ejecutar `BaseDatos/InventarioParcial2.sql`.
3. Verificar que existan las tablas `Categoria`, `Producto` y `Usuario` y la clave foránea de `Producto` a `Categoria`.

**Nota:** el script SQL se preparó manualmente; no debe confundirse con una migración generada por EF Core.

## Configuración local

1. Abrir `ApiLibro.slnx` en Visual Studio.
2. Restaurar las dependencias NuGet.
3. Configurar localmente `ConnectionStrings:ConexionBD` para que apunte a `InventarioParcial2` en la instancia de SQL Server utilizada.
4. Configurar los parámetros `Jwt:Key`, `Jwt:Issuer` y `Jwt:Audience`, si se utiliza la autenticación JWT del proyecto.
5. **No subir contraseñas, claves JWT reales ni cadenas de conexión con credenciales al repositorio.** Para el desarrollo local puede utilizarse la opción **Manage User Secrets** del proyecto.
6. Ejecutar la API (F5 o Ctrl+F5) y abrir la interfaz Swagger disponible en `/swagger`.

## Endpoints principales

| Método | Ruta | Uso |
|---|---|---|
| GET | `/api/Categorias` | Listar categorías |
| GET | `/api/Categorias/{id}` | Consultar categoría |
| POST | `/api/Categorias` | Crear categoría |
| PUT | `/api/Categorias/{id}` | Modificar categoría |
| DELETE | `/api/Categorias/{id}` | Eliminar categoría si no tiene productos |
| GET | `/api/Categorias/{id}/productos` | Categoría con productos |
| GET | `/api/Productos` | Listar productos con categoría |
| GET | `/api/Productos/{id}` | Consultar producto |
| POST | `/api/Productos` | Crear producto |
| PUT | `/api/Productos/{id}` | Modificar producto |
| DELETE | `/api/Productos/{id}` | Eliminar producto |
| GET | `/api/producto/categoria/{categoriaId}` | Filtrar productos por categoría (ruta del proyecto original) |

## Códigos de respuesta HTTP

- `200 OK`: consulta o modificación correcta.
- `201 Created`: creación correcta de categoría o producto.
- `204 No Content`: eliminación correcta.
- `400 Bad Request`: datos de entrada no válidos.
- `404 Not Found`: recurso inexistente.
- `409 Conflict`: reglas de negocio, como nombres duplicados o eliminación de categoría con productos.
- `500 Internal Server Error`: error inesperado, sin revelar información interna al consumidor.

## Pruebas y evidencias de Swagger

Las capturas se colocan en la carpeta `Evidencias/` y muestran los endpoints, datos de entrada y códigos HTTP:

1. Registro de categoría (201).
2. Registro de producto con categoría (201).
3. Listado de productos con categoría (200).
4. Listado de productos por categoría (200).
5. Registro de producto duplicado (409).
6. Registro de producto con categoría inexistente (409).
7. Intento de eliminar una categoría con productos (409).
8. Modificación de producto (200).
9. Eliminación de producto (204).

## Entrega

Se incluyen el código fuente, el script de reconstrucción de base de datos, el presente README y las evidencias de las pruebas de Swagger.
