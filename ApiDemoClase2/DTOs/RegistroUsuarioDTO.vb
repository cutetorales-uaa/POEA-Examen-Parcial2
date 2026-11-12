Imports System.ComponentModel.DataAnnotations

Public Class RegistroUsuarioDTO

    <Required>
    <StringLength(30)>
    Public Property nombre As String

    <Required>
    <EmailAddress>
    <StringLength(30)>
    Public Property Email As String

    <Required>
    <StringLength(50, MinimumLength:=8)>
    Public Property Password As String

End Class
