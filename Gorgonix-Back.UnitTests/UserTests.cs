using FluentAssertions;
using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Enums;
using Xunit;

namespace Gorgonix_Back.UnitTests.Domain;

public class UserTests
{
    // 1. PRUEBA DE CREACIÓN EXITOSA
    [Fact]
    public void Constructor_WithValidData_Should_CreateUser()
    {
        // Arrange (Preparar datos)
        string name = "Juan";
        string lastName = "Perez";
        string emailStr = "juan.perez@gorgonix.com";
        string userName = "jperez";
        string passwordHash = "hashed_secret_123";
        UserRole role = UserRole.User;
        bool isDeleted = false;

        // Act (Ejecutar el constructor real)
        var user = new User(name, lastName, emailStr, userName, passwordHash, role, isDeleted);

        // Assert (Verificar que todo se asignó bien)
        user.Should().NotBeNull();
        user.Id.Should().NotBeEmpty(); // El ID se genera en el constructor
        user.Name.Should().Be(name);
        user.LastName.Should().Be(lastName);
        user.UserName.Should().Be(userName);
        user.UserRole.Should().Be(role);
        user.IsDeleted.Should().BeFalse();
        
        // Verificación especial para el Value Object Email
        user.Email.Should().NotBeNull();
        user.Email.Value.Should().Be(emailStr);
    }

    // 2. PRUEBA DE VALIDACIONES DEL CONSTRUCTOR (Nombre Requerido)
    [Theory] // Theory permite probar varios valores en un solo test
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_Should_ThrowArgumentException(string invalidName)
    {
        // Act
        // Intentamos crear el usuario con un nombre inválido
        Action act = () => new User(
            invalidName, 
            "Perez", 
            "juan@test.com", 
            "jperez", 
            "hash", 
            UserRole.User, 
            false
        );

        // Assert
        // Verificamos que lance la excepción exacta que pusiste en tu entidad
        act.Should().Throw<ArgumentException>()
           .WithMessage("Nombre requerido");
    }

    // 3. PRUEBA DE EMAIL INVÁLIDO (Probando el Value Object a través de la Entidad)
    [Fact]
    public void Constructor_WithInvalidEmail_Should_ThrowException_From_ValueObject()
    {
        // Arrange
        string invalidEmail = "esto-no-es-un-email";

        // Act
        Action act = () => new User(
            "Juan", 
            "Perez", 
            invalidEmail, 
            "jperez", 
            "hash", 
            UserRole.User, 
            false
        );

        // Assert
        // Aquí esperamos la excepción que lanza tu clase 'Email'
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Email inválido*"); // El * actúa como comodín por si el mensaje varía un poco
    }

    // 4. PRUEBA DE MÉTODO UPDATE
    [Fact]
    public void UpdateUser_Should_Modify_Properties()
    {
        // Arrange
        var user = new User("Juan", "OldLast", "j@t.com", "oldUser", "hash", UserRole.User, false);
        string newName = "Pedro";
        string newLast = "NewLast";
        string newUser = "newUser";

        // Act
        user.UpdateUser(newName, newLast, newUser);

        // Assert
        user.Name.Should().Be(newName);
        user.LastName.Should().Be(newLast);
        user.UserName.Should().Be(newUser);
    }

    // 5. PRUEBA DE SOFT DELETE
    [Fact]
    public void SoftDelete_Should_Set_IsDeleted_To_True()
    {
        // Arrange
        var user = new User("Juan", "Perez", "j@t.com", "u", "h", UserRole.User, false);

        // Act
        user.SoftDelete();

        // Assert
        user.IsDeleted.Should().BeTrue();
    }
}