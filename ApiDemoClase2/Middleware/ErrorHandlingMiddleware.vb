
Imports Microsoft.AspNetCore.Http
Imports Microsoft.Extensions.Logging

Public Class ErrorHandlingMiddleware

    Private ReadOnly siguiente As RequestDelegate
    Private ReadOnly logger As ILogger(Of ErrorHandlingMiddleware)

    Public Sub New(
        siguiente As RequestDelegate,
        logger As ILogger(Of ErrorHandlingMiddleware))

        Me.siguiente = siguiente
        Me.logger = logger

    End Sub

    Public Async Function InvokeAsync(
        contexto As HttpContext) As Task

        Dim codigo As Integer = 500
        Dim mensaje As String = "Error interno del servidor"

        Try

            Await siguiente(contexto)
            Return

        Catch ex As Exception

            If contexto.Response.HasStarted Then
                Throw
            End If

            If TypeOf ex Is ConflictoNegocioException Then

                codigo = 409
                mensaje = ex.Message

            ElseIf TypeOf ex Is KeyNotFoundException Then

                codigo = 404
                mensaje = ex.Message

            ElseIf TypeOf ex Is ArgumentException Then

                codigo = 400
                mensaje = ex.Message

            Else

                logger.LogError(ex,
                    "Error inesperado en la API")

            End If

        End Try

        contexto.Response.StatusCode = codigo

        Await contexto.Response.WriteAsJsonAsync(
            New With {.mensaje = mensaje})

    End Function

End Class

