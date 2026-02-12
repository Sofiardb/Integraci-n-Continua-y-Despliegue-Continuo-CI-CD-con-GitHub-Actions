using CICDDemo.Api.Services;
using Xunit;

namespace CICDDemo.Api.Tests.Services;

/// <summary>
/// Tests unitarios para el servicio de calculadora
/// </summary>
public class CalculadoraServiceTests
{
    private readonly CalculadoraService _calculadoraService;

    public CalculadoraServiceTests()
    {
        _calculadoraService = new CalculadoraService();
    }

    #region Suma Tests

    [Fact]
    public void Sumar_DosNumerosPositivos_RetornaResultadoCorrecto()
    {
        // Arrange
        double a = 5;
        double b = 3;

        // Act
        var resultado = _calculadoraService.Sumar(a, b);

        // Assert
        Assert.Equal(8, resultado);
    }

    [Fact]
    public void Sumar_NumeroPositivoYNegativo_RetornaResultadoCorrecto()
    {
        // Arrange
        double a = 10;
        double b = -3;

        // Act
        var resultado = _calculadoraService.Sumar(a, b);

        // Assert
        Assert.Equal(7, resultado);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 2)]
    [InlineData(-5, -5, -10)]
    [InlineData(100, 200, 300)]
    public void Sumar_VariosValores_RetornaResultadosCorrecto(double a, double b, double esperado)
    {
        // Act
        var resultado = _calculadoraService.Sumar(a, b);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Resta Tests

    [Fact]
    public void Restar_DosNumerosPositivos_RetornaResultadoCorrecto()
    {
        // Arrange
        double a = 10;
        double b = 3;

        // Act
        var resultado = _calculadoraService.Restar(a, b);

        // Assert
        Assert.Equal(7, resultado);
    }

    [Theory]
    [InlineData(10, 5, 5)]
    [InlineData(0, 0, 0)]
    [InlineData(-5, -3, -2)]
    [InlineData(100, 150, -50)]
    public void Restar_VariosValores_RetornaResultadosCorrecto(double a, double b, double esperado)
    {
        // Act
        var resultado = _calculadoraService.Restar(a, b);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Multiplicación Tests

    [Fact]
    public void Multiplicar_DosNumerosPositivos_RetornaResultadoCorrecto()
    {
        // Arrange
        double a = 5;
        double b = 4;

        // Act
        var resultado = _calculadoraService.Multiplicar(a, b);

        // Assert
        Assert.Equal(20, resultado);
    }

    [Theory]
    [InlineData(2, 3, 6)]
    [InlineData(0, 100, 0)]
    [InlineData(-2, 3, -6)]
    [InlineData(-2, -3, 6)]
    public void Multiplicar_VariosValores_RetornaResultadosCorrecto(double a, double b, double esperado)
    {
        // Act
        var resultado = _calculadoraService.Multiplicar(a, b);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region División Tests

    [Fact]
    public void Dividir_DosNumerosPositivos_RetornaResultadoCorrecto()
    {
        // Arrange
        double a = 10;
        double b = 2;

        // Act
        var resultado = _calculadoraService.Dividir(a, b);

        // Assert
        Assert.Equal(5, resultado);
    }

    [Fact]
    public void Dividir_PorCero_LanzaDivideByZeroException()
    {
        // Arrange
        double a = 10;
        double b = 0;

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _calculadoraService.Dividir(a, b));
    }

    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(9, 3, 3)]
    [InlineData(-10, 2, -5)]
    [InlineData(7, 2, 3.5)]
    public void Dividir_VariosValores_RetornaResultadosCorrecto(double a, double b, double esperado)
    {
        // Act
        var resultado = _calculadoraService.Dividir(a, b);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Potencia Tests

    [Fact]
    public void Potencia_BaseYExponentePositivos_RetornaResultadoCorrecto()
    {
        // Arrange
        double baseNum = 2;
        double exponente = 3;

        // Act
        var resultado = _calculadoraService.Potencia(baseNum, exponente);

        // Assert
        Assert.Equal(8, resultado);
    }

    [Theory]
    [InlineData(2, 0, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(3, 2, 9)]
    [InlineData(10, 3, 1000)]
    public void Potencia_VariosValores_RetornaResultadosCorrecto(double baseNum, double exponente, double esperado)
    {
        // Act
        var resultado = _calculadoraService.Potencia(baseNum, exponente);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Raíz Cuadrada Tests

    [Fact]
    public void RaizCuadrada_NumeroPositivo_RetornaResultadoCorrecto()
    {
        // Arrange
        double numero = 16;

        // Act
        var resultado = _calculadoraService.RaizCuadrada(numero);

        // Assert
        Assert.Equal(4, resultado);
    }

    [Fact]
    public void RaizCuadrada_NumeroNegativo_LanzaArgumentException()
    {
        // Arrange
        double numero = -4;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _calculadoraService.RaizCuadrada(numero));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(4, 2)]
    [InlineData(25, 5)]
    [InlineData(100, 10)]
    public void RaizCuadrada_VariosValores_RetornaResultadosCorrecto(double numero, double esperado)
    {
        // Act
        var resultado = _calculadoraService.RaizCuadrada(numero);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Es Primo Tests

    [Theory]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(5, true)]
    [InlineData(7, true)]
    [InlineData(11, true)]
    [InlineData(13, true)]
    public void EsPrimo_NumerosPrimos_RetornaTrue(int numero, bool esperado)
    {
        // Act
        var resultado = _calculadoraService.EsPrimo(numero);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(4, false)]
    [InlineData(6, false)]
    [InlineData(8, false)]
    [InlineData(9, false)]
    [InlineData(10, false)]
    public void EsPrimo_NumerosNoPrimos_RetornaFalse(int numero, bool esperado)
    {
        // Act
        var resultado = _calculadoraService.EsPrimo(numero);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    #endregion

    #region Factorial Tests

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 6)]
    [InlineData(4, 24)]
    [InlineData(5, 120)]
    [InlineData(6, 720)]
    public void Factorial_NumerosPositivos_RetornaResultadoCorrecto(int numero, int esperado)
    {
        // Act
        var resultado = _calculadoraService.Factorial(numero);

        // Assert
        Assert.Equal(esperado, resultado);
    }

    [Fact]
    public void Factorial_NumeroNegativo_LanzaArgumentException()
    {
        // Arrange
        int numero = -1;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _calculadoraService.Factorial(numero));
    }

    #endregion
}

