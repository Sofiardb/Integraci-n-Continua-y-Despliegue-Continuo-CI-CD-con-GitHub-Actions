using CICDDemo.Api.Controllers;
using CICDDemo.Api.Models;
using CICDDemo.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CICDDemo.Api.Tests.Controllers;

/// <summary>
/// Tests unitarios para el controlador de calculadora
/// </summary>
public class CalculadoraControllerTests
{
    private readonly Mock<ICalculadoraService> _mockCalculadoraService;
    private readonly CalculadoraController _controller;

    public CalculadoraControllerTests()
    {
        _mockCalculadoraService = new Mock<ICalculadoraService>();
        _controller = new CalculadoraController(_mockCalculadoraService.Object);
    }

    #region Sumar Tests

    [Fact]
    public void Sumar_RetornaOkConResultado()
    {
        // Arrange
        _mockCalculadoraService.Setup(s => s.Sumar(5, 3)).Returns(8);

        // Act
        var result = _controller.Sumar(5, 3);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultadoOperacion = Assert.IsType<ResultadoOperacion>(okResult.Value);
        Assert.Equal(8, resultadoOperacion.Resultado);
        Assert.Equal("Suma", resultadoOperacion.Operacion);
    }

    #endregion

    #region Restar Tests

    [Fact]
    public void Restar_RetornaOkConResultado()
    {
        // Arrange
        _mockCalculadoraService.Setup(s => s.Restar(10, 3)).Returns(7);

        // Act
        var result = _controller.Restar(10, 3);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultadoOperacion = Assert.IsType<ResultadoOperacion>(okResult.Value);
        Assert.Equal(7, resultadoOperacion.Resultado);
        Assert.Equal("Resta", resultadoOperacion.Operacion);
    }

    #endregion

    #region Multiplicar Tests

    [Fact]
    public void Multiplicar_RetornaOkConResultado()
    {
        // Arrange
        _mockCalculadoraService.Setup(s => s.Multiplicar(4, 5)).Returns(20);

        // Act
        var result = _controller.Multiplicar(4, 5);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultadoOperacion = Assert.IsType<ResultadoOperacion>(okResult.Value);
        Assert.Equal(20, resultadoOperacion.Resultado);
        Assert.Equal("Multiplicación", resultadoOperacion.Operacion);
    }

    #endregion

    #region Dividir Tests

    [Fact]
    public void Dividir_DivisionValida_RetornaOkConResultado()
    {
        // Arrange
        _mockCalculadoraService.Setup(s => s.Dividir(10, 2)).Returns(5);

        // Act
        var result = _controller.Dividir(10, 2);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultadoOperacion = Assert.IsType<ResultadoOperacion>(okResult.Value);
        Assert.Equal(5, resultadoOperacion.Resultado);
        Assert.Equal("División", resultadoOperacion.Operacion);
    }

    [Fact]
    public void Dividir_DivisionPorCero_RetornaBadRequest()
    {
        // Arrange
        _mockCalculadoraService
            .Setup(s => s.Dividir(10, 0))
            .Throws(new DivideByZeroException("No se puede dividir por cero"));

        // Act
        var result = _controller.Dividir(10, 0);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    #endregion

    #region Potencia Tests

    [Fact]
    public void Potencia_RetornaOkConResultado()
    {
        // Arrange
        _mockCalculadoraService.Setup(s => s.Potencia(2, 3)).Returns(8);

        // Act
        var result = _controller.Potencia(2, 3);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultadoOperacion = Assert.IsType<ResultadoOperacion>(okResult.Value);
        Assert.Equal(8, resultadoOperacion.Resultado);
        Assert.Equal("Potencia", resultadoOperacion.Operacion);
    }

    #endregion

    #region RaizCuadrada Tests

    [Fact]
    public void RaizCuadrada_NumeroValido_RetornaOk()
    {
        // Arrange
        _mockCalculadoraService.Setup(s => s.RaizCuadrada(16)).Returns(4);

        // Act
        var result = _controller.RaizCuadrada(16);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void RaizCuadrada_NumeroNegativo_RetornaBadRequest()
    {
        // Arrange
        _mockCalculadoraService
            .Setup(s => s.RaizCuadrada(-4))
            .Throws(new ArgumentException("No se puede calcular la raíz cuadrada de un número negativo"));

        // Act
        var result = _controller.RaizCuadrada(-4);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    #endregion

    #region EsPrimo Tests

    [Fact]
    public void EsPrimo_RetornaOkConResultado()
    {
        // Arrange
        _mockCalculadoraService.Setup(s => s.EsPrimo(7)).Returns(true);

        // Act
        var result = _controller.EsPrimo(7);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    #endregion

    #region Factorial Tests

    [Fact]
    public void Factorial_NumeroValido_RetornaOk()
    {
        // Arrange
        _mockCalculadoraService.Setup(s => s.Factorial(5)).Returns(120);

        // Act
        var result = _controller.Factorial(5);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void Factorial_NumeroNegativo_RetornaBadRequest()
    {
        // Arrange
        _mockCalculadoraService
            .Setup(s => s.Factorial(-1))
            .Throws(new ArgumentException("No se puede calcular el factorial de un número negativo"));

        // Act
        var result = _controller.Factorial(-1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    #endregion
}

