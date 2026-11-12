
Public Class CategoriaService

    Private ReadOnly repository As CategoriaRepository

    Public Sub New(repository As CategoriaRepository)
        Me.repository = repository
    End Sub

    Private Function ConvertirDTO(
        categoria As Categoria) As CategoriaResponseDTO

        Return New CategoriaResponseDTO With {
            .Id = categoria.Id,
            .Nombre = categoria.Nombre,
            .Descripcion = categoria.Descripcion
        }

    End Function

    Public Function ObtenerTodos() As List(Of CategoriaResponseDTO)

        Return repository.ObtenerTodos().
            Select(Function(c) ConvertirDTO(c)).
            ToList()

    End Function

    Public Function ObtenerPorId(id As Integer) As CategoriaResponseDTO

        Dim categoria = repository.ObtenerporId(id)

        If categoria Is Nothing Then
            Throw New KeyNotFoundException(
                "Categoría no encontrada")
        End If

        Return ConvertirDTO(categoria)

    End Function

    Public Function Crear(
        dto As CategoriaCreateDTO) As CategoriaResponseDTO

        If String.IsNullOrWhiteSpace(dto.Nombre) OrElse
           dto.Nombre.Trim().Length > 50 Then
            Throw New ArgumentException(
                "El nombre es obligatorio y debe tener máximo 50 caracteres")
        End If

        If repository.ExistePorNombre(dto.Nombre) Then
            Throw New ConflictoNegocioException(
                "Ya existe una categoría con ese nombre")
        End If

        Dim categoria As New Categoria With {
            .Nombre = dto.Nombre.Trim(),
            .Descripcion = dto.Descripcion
        }

        repository.Agregar(categoria)

        Return ConvertirDTO(categoria)

    End Function

    Public Function Modificar(
        id As Integer,
        dto As CategoriaUpdateDTO) As CategoriaResponseDTO

        Dim categoria = repository.ObtenerporId(id)

        If categoria Is Nothing Then
            Throw New KeyNotFoundException(
                "Categoría no encontrada")
        End If

        If String.IsNullOrWhiteSpace(dto.Nombre) OrElse
           dto.Nombre.Trim().Length > 50 Then
            Throw New ArgumentException(
                "El nombre es obligatorio y debe tener máximo 50 caracteres")
        End If

        If repository.ExistePorNombre(dto.Nombre, id) Then
            Throw New ConflictoNegocioException(
                "Ya existe una categoría con ese nombre")
        End If

        categoria.Nombre = dto.Nombre.Trim()
        categoria.Descripcion = dto.Descripcion

        repository.Modificar(categoria)

        Return ConvertirDTO(categoria)

    End Function

    Public Sub Eliminar(id As Integer)

        Dim categoria = repository.ObtenerporId(id)

        If categoria Is Nothing Then
            Throw New KeyNotFoundException(
                "Categoría no encontrada")
        End If

        If repository.TieneProductos(id) Then
            Throw New ConflictoNegocioException(
                "No se puede eliminar una categoría con productos asociados")
        End If

        repository.Eliminar(categoria)

    End Sub



    Public Function ObtenerConProductos(
    id As Integer
) As CategoriaConProductosDTO

        Dim categoria = repository.ObtenerConProductos(id)

        If categoria Is Nothing Then
            Throw New KeyNotFoundException(
            "Categoría no encontrada")
        End If

        Return New CategoriaConProductosDTO With {
        .Id = categoria.Id,
        .Nombre = categoria.Nombre,
        .Descripcion = categoria.Descripcion,
        .Productos = categoria.Productos.Select(
            Function(p) New ProductoResponseDTO With {
                .Id = p.Id,
                .Nombre = p.Nombre,
                .Descripcion = p.Descripcion,
                .Precio = p.Precio,
                .Stock = p.Stock,
                .FechaRegistro = p.FechaRegistro,
                .CategoriaID = p.CategoriaID,
                .NombreCategoria = categoria.Nombre
            }).ToList()
    }

    End Function


End Class
