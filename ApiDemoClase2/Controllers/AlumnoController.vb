
Imports Microsoft.AspNetCore.Mvc

<Route("api/[controller]")>
<ApiController>
Public Class AlumnoController
    Inherits ControllerBase
    <HttpGet>
    Public Function GetAlumnos()
        Dim alumnos = New List(Of Alumno) From
            {
            New Alumno With {
            .Id = 1,
            .Nombre = "Juan"
            },
            New Alumno With {
            .Id = 2,
            .Nombre = "Pedro"
            },
            New Alumno With {
            .Id = 3,
            .Nombre = "María"
            }
        }

        Return Ok(alumnos)
    End Function
End Class
