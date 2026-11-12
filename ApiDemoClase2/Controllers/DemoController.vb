
Imports Microsoft.AspNetCore.Http.HttpResults
Imports Microsoft.AspNetCore.Mvc

<Route("api/[controller]"), ApiController>
Public Class DemoController
    Inherits ControllerBase

    <HttpGet("saludo/{nombre}/{edad}")>
    Public Function Saludo(nombre As String, edad As Integer)
        Return Ok("Hola " & nombre & " , tengo " & edad & " años")
    End Function

    <HttpGet("obtener-alumno")>
    Public Function ObtenerAlumno()
        Dim alumno As New Alumno With {
        .Id = 1,
        .Nombre = "Juan",
        .Apellido = "Pérez",
        .Edad = 25
        }

        Return Ok(alumno)
    End Function

    <HttpPost("crear-alumno")>
    Public Function CrearAlumno(alumno As Alumno)

        Return Ok(alumno)

    End Function

    <HttpPost("evaluar-alumno")>
    Public Function EvaluarAlumno(alumno As Alumno)
        If alumno.Edad >= 18 Then
            Return Ok("El alumno es mayor de edad")
        Else
            Return Ok("El alumno es menor de edad")
        End If
    End Function

End Class
