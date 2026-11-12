Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("Producto")>
Public Class Producto

    Public Property Id As Integer

    <Required(ErrorMessage:="El nombre es obligatorio")>
    <StringLength(50)>
    Public Property Nombre As String

    <StringLength(200)>
    Public Property Descripcion As String

    Public Property Precio As Decimal

    Public Property Stock As Integer

    Public Property FechaRegistro As DateTime =
        DateTime.UtcNow

    Public Property CategoriaID As Integer

    Public Property Categoria As Categoria

End Class

