Imports System.Security.Cryptography.X509Certificates
Imports Microsoft.AspNetCore.Authorization
Imports Microsoft.AspNetCore.DataProtection.Repositories
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.IdentityModel.Tokens


''
'' Códigos de estado de respuesta HTTP
'' 
''
''
' 100-199 = Respuestas informativas
' 200-299 = Respuestas satisfactorias
' 300-399 = Redirecciones
' 400-499 = Errores de los clientes
' 500-599 = Errores de los servidores

' Ejemplos
' 200 = OK
' 201 = Creado
' 400 = Solicitud Incorrecta
' 404 = No encontrado
' 500 = Error en el servidor

' <Authorize>
<Route("api/[controller]")>
<ApiController>
Public Class ProductosController
    Inherits ControllerBase

    Private ReadOnly servicio As ProductoService
    Private ReadOnly _repository As ProductoRepository
    Public Sub New(servicio As ProductoService, repository As ProductoRepository)
        Me.servicio = servicio
        Me._repository = repository
    End Sub

    'GET: api/productos
    <HttpGet>
    Public Function ObtenerProductos()
        Dim productos = servicio.ObtenerTodos()
        Return Ok(productos)
    End Function

    'GET: api/productos/1
    <HttpGet("{id}")>
    Public Function ObtenerPorId(id As Integer)
        Dim producto = servicio.ObtenerporId(id)
        If producto Is Nothing Then
            Return NotFound("Producto no encontrado")
        End If

        Return Ok(producto)
    End Function

    'POST: api/productos
    <HttpPost>
    Public Function Crear(
    <FromBody> dto As ProductoCreateDTO
) As IActionResult

        Dim productoCreado = servicio.Crear(dto)

        Return CreatedAtAction(
        NameOf(ObtenerPorId),
        New With {.id = productoCreado.Id},
        productoCreado)

    End Function


    'PUT: api/productos/1

    <HttpPut("{id}")>
    Public Function Modificar(
    id As Integer,
    <FromBody> dto As ProductoUpdateDTO
) As IActionResult

        Dim producto = servicio.Modificar(id, dto)

        If producto Is Nothing Then
            Return NotFound(New With {
            .mensaje = "Producto no encontrado"
        })
        End If

        Return Ok(producto)

    End Function

    'DELETE: api/productos/1
    <HttpDelete("{id}")>
    Public Function Eliminar(id As Integer) As IActionResult

        Dim eliminado = servicio.Eliminar(id)

        If Not eliminado Then
            Return NotFound(New With {
            .mensaje = "Producto no encontrado"
        })
        End If

        Return NoContent()

    End Function

    <HttpGet("/api/producto/categoria/{categoriaId:int}")>
    Public Function ObtenerPorCategoria(
    <FromRoute> categoriaId As Integer) As IActionResult

        Dim productos = servicio.ObtenerPorCategoria(categoriaId)

        If productos Is Nothing Then
            Return NotFound(New With {
                .mensaje = "La categoria solicitada no existe"
            })
        End If

        Return Ok(productos)

    End Function

    'GET: api/productos/1
    <HttpGet("buscar")>
    Public Async Function Buscar(<FromQuery> nombre As String) As Task(Of IActionResult)
        If String.IsNullOrWhiteSpace(nombre) Then
            Return BadRequest("Debe ingresar un nombre")
        End If

        Dim productos = Await _repository.BuscarPorNombre(nombre)
        Return Ok(productos)
    End Function

    'GET: api/productos/1
    <HttpGet("precio")>
    Public Async Function BuscarPorPrecio(<FromQuery> minimo As Integer, <FromQuery> maximo As Integer) As Task(Of IActionResult)
        If minimo < 0 OrElse maximo < 0 Then
            Return BadRequest("Los precios no pueden ser negativos")
        End If

        If minimo > maximo Then
            Return BadRequest("El precio minimo no puede ser mayor que el máximo")
        End If

        Dim productos = Await _repository.BuscarPorPrecio(minimo, maximo)
        Return Ok(productos)
    End Function



    <HttpGet("preciomayora")>
    Public Async Function BuscarPorPrecioMayorA(
    <FromQuery> minimo As Integer
) As Task(Of IActionResult)

        Dim productos = Await _repository.BuscarPorPrecioMayorA(minimo)

        Return Ok(productos)

    End Function

End Class
