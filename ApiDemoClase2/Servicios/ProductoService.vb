Imports Microsoft.AspNetCore.Mvc


Public Class ProductoService

    Private ReadOnly repository As ProductoRepository
    Private ReadOnly categoriaRepository As CategoriaRepository

    Public Sub New(
        repository As ProductoRepository,
        categoriaRepository As CategoriaRepository)

        Me.repository = repository
        Me.categoriaRepository = categoriaRepository

    End Sub

    Private Sub ValidarProducto(
        nombre As String,
        descripcion As String,
        precio As Decimal,
        stock As Integer,
        categoriaId As Integer,
        Optional excluirId As Integer = 0)

        If String.IsNullOrWhiteSpace(nombre) OrElse
           nombre.Trim().Length > 50 Then
            Throw New ArgumentException(
                "El nombre es obligatorio y debe tener máximo 50 caracteres")
        End If

        If descripcion IsNot Nothing AndAlso
           descripcion.Length > 200 Then
            Throw New ArgumentException(
                "La descripción no puede superar 200 caracteres")
        End If

        If precio <= 0 Then
            Throw New ArgumentException(
                "El precio debe ser mayor a cero")
        End If

        If stock < 0 Then
            Throw New ArgumentException(
                "El stock no puede ser negativo")
        End If

        If Not repository.ExisteCategoria(categoriaId) Then
            Throw New ConflictoNegocioException(
                "La categoría indicada no existe")
        End If

        If repository.ExistePorNombre(nombre, excluirId) Then
            Throw New ConflictoNegocioException(
                "Ya existe un producto con ese nombre")
        End If

    End Sub

    Private Function ConvertirDTO(
        producto As Producto) As ProductoResponseDTO

        Return New ProductoResponseDTO With {
            .Id = producto.Id,
            .Nombre = producto.Nombre,
            .Descripcion = producto.Descripcion,
            .Precio = producto.Precio,
            .Stock = producto.Stock,
            .FechaRegistro = producto.FechaRegistro,
            .CategoriaID = producto.CategoriaID,
            .NombreCategoria =
                If(producto.Categoria Is Nothing,
                   Nothing,
                   producto.Categoria.Nombre)
        }

    End Function

    Public Function Crear(
        dto As ProductoCreateDTO) As ProductoResponseDTO

        ValidarProducto(
            dto.Nombre,
            dto.Descripcion,
            dto.Precio,
            dto.Stock,
            dto.CategoriaID)

        Dim producto As New Producto With {
            .Nombre = dto.Nombre.Trim(),
            .Descripcion = dto.Descripcion,
            .Precio = dto.Precio,
            .Stock = dto.Stock,
            .FechaRegistro = DateTime.UtcNow,
            .CategoriaID = dto.CategoriaID
        }

        repository.Agregar(producto)

        Return ConvertirDTO(repository.ObtenerporId(producto.Id))

    End Function

    Public Function ObtenerTodos() As List(Of ProductoResponseDTO)

        Return repository.ObtenerTodos().
            Select(Function(p) ConvertirDTO(p)).
            ToList()

    End Function


    Public Function ObtenerporId(id As Integer) As ProductoResponseDTO

        Dim producto = repository.ObtenerporId(id)

        If producto Is Nothing Then
            Return Nothing
        End If

        Return ConvertirDTO(producto)

    End Function



    Public Function Modificar(
    id As Integer,
    dto As ProductoUpdateDTO
) As ProductoResponseDTO

        Dim producto = repository.ObtenerporId(id)

        If producto Is Nothing Then
            Return Nothing
        End If

        ValidarProducto(
        dto.Nombre,
        dto.Descripcion,
        dto.Precio,
        dto.Stock,
        dto.CategoriaID,
        id)

        producto.Nombre = dto.Nombre.Trim()
        producto.Descripcion = dto.Descripcion
        producto.Precio = dto.Precio
        producto.Stock = dto.Stock
        producto.CategoriaID = dto.CategoriaID
        producto.Categoria = categoriaRepository.ObtenerporId(dto.CategoriaID)

        repository.Modificar(producto)

        Return ConvertirDTO(producto)

    End Function


    Public Function Eliminar(id As Integer) As Boolean

        Dim producto = repository.ObtenerporId(id)

        If producto Is Nothing Then
            Return False
        End If

        repository.Eliminar(producto)

        Return True

    End Function

    Public Function ObtenerPorCategoria(
        categoriaId As Integer) As List(Of ProductoResponseDTO)

        If Not repository.ExisteCategoria(categoriaId) Then
            Return Nothing
        End If

        Return repository.ObtenerPorCategoria(categoriaId).
            Select(Function(p) ConvertirDTO(p)).
            ToList()

    End Function

End Class
