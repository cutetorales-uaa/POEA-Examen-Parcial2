Imports System.Text
Imports Microsoft.AspNetCore.Authentication.JwtBearer
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.IdentityModel.Tokens


Module Program
    Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)
        builder.Services.AddControllers()

        'Swagger
        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()

        builder.Services.AddDbContext(Of AppDbContext)(
        Sub(options)
            options.UseSqlServer(
            builder.Configuration("ConnectionStrings:ConexionBD")
            )
        End Sub
        )

        builder.Services.AddScoped(Of ProductoRepository)
        builder.Services.AddScoped(Of ProductoService)
        builder.Services.AddScoped(Of CategoriaRepository)
        builder.Services.AddScoped(Of CategoriaService)

        Dim jwtKey As String =
            builder.Configuration("Jwt:Key")

        Dim issuer As String =
            builder.Configuration("Jwt:Issuer")

        Dim audience As String =
            builder.Configuration("Jwt:Audience")

        ' Validar la clave JWT
        If String.IsNullOrWhiteSpace(jwtKey) OrElse
       Encoding.UTF8.GetByteCount(jwtKey) < 32 Then

            Throw New InvalidOperationException(
        "La clave JWT debe tener al menos 32 bytes.")

        End If

        ' Validar emisor y audiencia
        If String.IsNullOrWhiteSpace(issuer) OrElse
           String.IsNullOrWhiteSpace(audience) Then

            Throw New InvalidOperationException(
                "Debe configurar Jwt:Issuer y Jwt:Audience.")

        End If

        ' Registrar el esquema de autenticación JWT
        builder.Services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
        ).AddJwtBearer(
         Sub(options)

             options.TokenValidationParameters =
            New TokenValidationParameters With {
                .ValidateIssuer = True,
                .ValidIssuer = issuer,
                .ValidateAudience = True,
                .ValidAudience = audience,
                .ValidateIssuerSigningKey = True,
                .IssuerSigningKey =
                    New SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),
                .ValidateLifetime = True,
                .ClockSkew = TimeSpan.FromSeconds(30)
            }

         End Sub)

        builder.Services.AddAuthorization()



        Dim app = builder.Build()

        app.UseMiddleware(Of ErrorHandlingMiddleware)()

        'Habilitar Swagger
        If app.Environment.IsDevelopment() Then
            app.UseSwagger()
            app.UseSwaggerUI()
        End If

        app.UseAuthentication()
        app.UseAuthorization()

        app.MapControllers()
        app.Run()
    End Sub
End Module
