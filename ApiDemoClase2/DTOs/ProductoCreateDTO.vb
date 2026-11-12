
Imports System.ComponentModel.DataAnnotations

Public Class ProductoCreateDTO

    <Required(ErrorMessage:="El nombre es obligatorio")>
    <StringLength(50)>
    Public Property Nombre As String

    <StringLength(200)>
    Public Property Descripcion As String

    <Range(GetType(Decimal), "0.01", "999999999999.99",
        ParseLimitsInInvariantCulture:=True,
        ConvertValueInInvariantCulture:=True,
        ErrorMessage:="El precio debe ser mayor a cero")>
    Public Property Precio As Decimal

    <Range(0, Integer.MaxValue)>
    Public Property Stock As Integer

    <Range(1, Integer.MaxValue,
        ErrorMessage:="Debe seleccionar una categoría")>
    Public Property CategoriaID As Integer

End Class
