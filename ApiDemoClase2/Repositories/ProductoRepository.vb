Imports System.Security.Cryptography.X509Certificates
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore

Public Class ProductoRepository
    Private ReadOnly contexto As AppDbContext

    Public Sub New(contexto As AppDbContext)
        Me.contexto = contexto
    End Sub


    Public Sub Agregar(producto As Producto)
        contexto.Productos.Add(producto)
        contexto.SaveChanges()
    End Sub

    Public Function ObtenerTodos() As List(Of Producto)
        Return contexto.Productos _
        .Include(Function(p) p.Categoria).ToList()
    End Function

    Public Function ObtenerporId(id As Integer) As Producto
        Return contexto.Productos _
        .Include(Function(p) p.Categoria) _
        .FirstOrDefault(Function(p) p.Id = id)
    End Function

    Public Sub Modificar(producto As Producto)
        contexto.Productos.Update(producto)
        contexto.SaveChanges()

    End Sub


    Public Sub Eliminar(producto As Producto)
        contexto.Productos.Remove(producto)
        contexto.SaveChanges()
    End Sub

    Public Function ExistePorNombre(
    nombre As String,
    Optional excluirId As Integer = 0
) As Boolean

        Dim nombreNormalizado = nombre.Trim().ToLower()

        Return contexto.Productos.Any(
        Function(p) p.Nombre.ToLower() = nombreNormalizado AndAlso
                    p.Id <> excluirId)

    End Function


    Public Function ObtenerPorCategoria(categoriaId As Integer) As List(Of Producto)

        Return contexto.Productos _
            .Include(Function(p) p.Categoria) _
            .Where(Function(p) p.CategoriaID = categoriaId) _
            .ToList()

    End Function

    Public Function ExisteCategoria(categoriaId As Integer) As Boolean

        Return contexto.Categorias _
            .Any(Function(c) c.Id = categoriaId)

    End Function


    Public Async Function BuscarPorNombre(nombre As String) As Task(Of List(Of Producto))

        Dim productos = Await contexto.Productos.Where(Function(p) p.Nombre.Contains(nombre)).ToListAsync()

        Return productos

    End Function


    Public Async Function BuscarPorPrecio(minimo As Integer, maximo As Integer) As Task(Of List(Of Producto))

        Dim productos = Await contexto.Productos.Where(Function(p) p.Precio >= minimo AndAlso p.Precio <= maximo).ToListAsync()

        Return productos

    End Function

    Public Async Function BuscarPorPrecioMayorA(
    minimo As Integer
) As Task(Of List(Of Producto))

        Dim productos = Await contexto.Productos.
            Where(Function(p) p.Precio > minimo).
            OrderBy(Function(p) p.Precio).
            ToListAsync()

        Return productos

    End Function

End Class
