
Imports Microsoft.AspNetCore.Mvc

<Route("api/[controller]")>
<ApiController>
Public Class CategoriasController
    Inherits ControllerBase

    Private ReadOnly servicio As CategoriaService

    Public Sub New(servicio As CategoriaService)
        Me.servicio = servicio
    End Sub

    ' GET: api/categorias
    <HttpGet>
    Public Function ObtenerTodos() As IActionResult

        Return Ok(servicio.ObtenerTodos())

    End Function

    ' GET: api/categorias/1
    <HttpGet("{id:int}")>
    Public Function ObtenerPorId(id As Integer) As IActionResult

        Return Ok(servicio.ObtenerPorId(id))

    End Function

    ' GET: api/categorias/1/productos
    <HttpGet("{id:int}/productos")>
    Public Function ObtenerConProductos(
        id As Integer
    ) As IActionResult

        Return Ok(servicio.ObtenerConProductos(id))

    End Function

    ' POST: api/categorias
    <HttpPost>
    Public Function Crear(
        <FromBody> dto As CategoriaCreateDTO
    ) As IActionResult

        Dim categoria = servicio.Crear(dto)

        Return CreatedAtAction(
            NameOf(ObtenerPorId),
            New With {.id = categoria.Id},
            categoria)

    End Function

    ' PUT: api/categorias/1
    <HttpPut("{id:int}")>
    Public Function Modificar(
        id As Integer,
        <FromBody> dto As CategoriaUpdateDTO
    ) As IActionResult

        Return Ok(servicio.Modificar(id, dto))

    End Function

    ' DELETE: api/categorias/1
    <HttpDelete("{id:int}")>
    Public Function Eliminar(id As Integer) As IActionResult

        servicio.Eliminar(id)

        Return NoContent()

    End Function

End Class
