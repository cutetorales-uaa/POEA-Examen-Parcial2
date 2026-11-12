
Imports Microsoft.EntityFrameworkCore

Public Class AppDbContext
    Inherits DbContext

    Public Sub New(options As DbContextOptions(Of AppDbContext))
        MyBase.New(options)
    End Sub

    Public Property Productos As DbSet(Of Producto)

    Public Property Categorias As DbSet(Of Categoria)

    Public Property Usuario As DbSet(Of Usuario)

    Protected Overrides Sub OnModelCreating(
        modelBuilder As ModelBuilder)

        MyBase.OnModelCreating(modelBuilder)

        modelBuilder.Entity(Of Producto)() _
            .HasOne(Function(p) p.Categoria) _
            .WithMany(Function(c) c.Productos) _
            .HasForeignKey(Function(p) p.CategoriaID) _
            .OnDelete(DeleteBehavior.Restrict)

        modelBuilder.Entity(Of Producto)() _
            .Property(Function(p) p.Precio) _
            .HasPrecision(18, 2)

    End Sub

End Class
