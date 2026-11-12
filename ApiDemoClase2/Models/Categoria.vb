
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("Categoria")>
Public Class Categoria

    Public Property Id As Integer

    <Required(ErrorMessage:="El nombre es obligatorio")>
    <StringLength(50)>
    Public Property Nombre As String

    <StringLength(200)>
    Public Property Descripcion As String

    Public Property Productos As List(Of Producto) =
        New List(Of Producto)()

End Class
