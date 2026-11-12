
Imports System.ComponentModel.DataAnnotations

Public Class CategoriaCreateDTO

    <Required(ErrorMessage:="El nombre es obligatorio")>
    <StringLength(50)>
    Public Property Nombre As String

    <StringLength(200)>
    Public Property Descripcion As String

End Class
