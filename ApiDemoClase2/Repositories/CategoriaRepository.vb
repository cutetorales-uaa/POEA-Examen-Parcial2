
Imports Microsoft.EntityFrameworkCore

Public Class CategoriaRepository

    Private ReadOnly contexto As AppDbContext

    Public Sub New(contexto As AppDbContext)
        Me.contexto = contexto
    End Sub

    Public Function ObtenerTodos() As List(Of Categoria)
        Return contexto.Categorias.ToList()
    End Function

    Public Function ObtenerporId(id As Integer) As Categoria
        Return contexto.Categorias.Find(id)
    End Function

    Public Function ObtenerConProductos(id As Integer) As Categoria

        Return contexto.Categorias.
            Include(Function(c) c.Productos).
            FirstOrDefault(Function(c) c.Id = id)

    End Function

    Public Sub Agregar(categoria As Categoria)
        contexto.Categorias.Add(categoria)
        contexto.SaveChanges()
    End Sub

    Public Sub Modificar(categoria As Categoria)
        contexto.Categorias.Update(categoria)
        contexto.SaveChanges()
    End Sub

    Public Sub Eliminar(categoria As Categoria)
        contexto.Categorias.Remove(categoria)
        contexto.SaveChanges()
    End Sub

    Public Function ExistePorNombre(
        nombre As String,
        Optional excluirId As Integer = 0
    ) As Boolean

        Dim nombreNormalizado = nombre.Trim().ToLower()

        Return contexto.Categorias.Any(
            Function(c) c.Nombre.ToLower() = nombreNormalizado AndAlso
                        c.Id <> excluirId)

    End Function

    Public Function TieneProductos(id As Integer) As Boolean

        Return contexto.Productos.Any(
            Function(p) p.CategoriaID = id)

    End Function

End Class

