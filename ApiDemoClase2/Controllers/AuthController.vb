Imports System.IdentityModel.Tokens.Jwt
Imports System.Net
Imports System.Security.Claims
Imports System.Text
Imports Microsoft.AspNetCore.Identity
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.IdentityModel.Tokens

<ApiController>
<Route("api/[controller]")>
Public Class AuthController
    Inherits ControllerBase

    Private ReadOnly contexto As AppDbContext
    Private ReadOnly hasher As PasswordHasher(Of Usuario)
    Private ReadOnly Configuration As IConfiguration

    Public Sub New(
        contexto As AppDbContext,
        configuration As IConfiguration)

        Me.contexto = contexto
        Me.Configuration = configuration
        Me.hasher = New PasswordHasher(Of Usuario)()

    End Sub

    <HttpPost("registro")>
    Public Async Function Registrar(
    dto As RegistroUsuarioDTO) _
    As Task(Of IActionResult)

        Dim email As String = dto.Email.Trim()

        Dim existe As Boolean =
        Await contexto.Usuario.AnyAsync(
            Function(u) u.Email = email)

        If existe Then
            Return Conflict(
            New With {
                .mensaje = "No se pudo registrar la cuenta."
            })
        End If

        Dim usuario As New Usuario With {
                .nombre = dto.nombre.Trim(),
                .Email = email,
                .Rol = "Usuario",
                .Activo = True
        }

        usuario.PasswordHash =
            hasher.HashPassword(usuario, dto.Password)

        contexto.Usuario.Add(usuario)

        Await contexto.SaveChangesAsync()

        Return StatusCode(
            201,
                New With {
        .mensaje = "Usuario registrado correctamente."
    })

    End Function


    <HttpPost("login")>
    Public Async Function Login(
    dto As LoginDTO) _
    As Task(Of IActionResult)

        Dim email As String = dto.Email.Trim()

        Dim usuario As Usuario =
            Await contexto.Usuario.FirstOrDefaultAsync(
                Function(u) u.Email = email)

        If usuario Is Nothing OrElse
            Not usuario.Activo Then

            Return Unauthorized(
                New With {
                    .mensaje = "Credenciales Incorrectas"
            })

        End If

        Dim resultado =
            hasher.VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            dto.Password)

        If resultado =
           PasswordVerificationResult.Failed Then
            Return Unauthorized(
            New With {
                .mensaje = "Credenciales Incorrectas."
            })
        End If

        Dim respuesta As TokenDTO =
            GenerarToken(usuario)

        Return Ok(respuesta)

    End Function

    Private Function GenerarToken(
    usuario As Usuario) As TokenDTO

        Dim key As String = Configuration("Jwt:Key")

        Dim issuer As String =
            Configuration("Jwt:Issuer")

        Dim audience As String =
            Configuration("Jwt:Audience")

        Dim minutos As Integer

        If Not Integer.TryParse(
            Configuration("Jwt:ExpirationMinutes"),
            minutos) OrElse minutos <= 0 Then

            Throw New InvalidOperationException(
                "La expiración JWT no es válida.")
        End If

        Dim securityKey As New SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key))


        Dim credentials As New SigningCredentials(
             securityKey,
             SecurityAlgorithms.HmacSha256)

        Dim claims As Claim() = {
         New Claim(
            JwtRegisteredClaimNames.Sub,
            usuario.usuarioID.ToString()),
            New Claim(
                JwtRegisteredClaimNames.Email,
                usuario.Email),
            New Claim(
                ClaimTypes.Name,
                usuario.nombre),
            New Claim(
                ClaimTypes.Role,
                usuario.Rol)
        }

        Dim expiracion As DateTime =
        DateTime.UtcNow.AddMinutes(minutos)

        Dim token As New JwtSecurityToken(
            issuer:=issuer,
            audience:=audience,
            claims:=claims,
            expires:=expiracion,
            signingCredentials:=credentials)

        Dim handler As New JwtSecurityTokenHandler()

        Return New TokenDTO With {
            .AccessToken = handler.WriteToken(token),
            .TipoToken = "Bearer",
            .ExpiresIn = minutos * 60
        }
    End Function
End Class
