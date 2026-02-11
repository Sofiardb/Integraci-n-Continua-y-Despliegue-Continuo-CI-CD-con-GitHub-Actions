using CICDDemo.Api.Models;
using CICDDemo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CICDDemo.Api.Controllers;

/// <summary>
/// Controller para operaciones matemáticas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CalculadoraController : ControllerBase
{
    private readonly ICalculadoraService _calculadoraService;

    public CalculadoraController(ICalculadoraService calculadoraService)
    {
        _calculadoraService = calculadoraService;
    }

    /// <summary>
    /// Suma dos números
    /// </summary>
    [HttpGet("sumar")]
    public ActionResult<ResultadoOperacion> Sumar([FromQuery] double a, [FromQuery] double b)
    {
        var resultado = _calculadoraService.Sumar(a, b);
        return Ok(new ResultadoOperacion
        {
            Valor1 = a,
            Valor2 = b,
            Operacion = "Suma",
            Resultado = resultado
        });
    }

    /// <summary>
    /// Resta dos números
    /// </summary>
    [HttpGet("restar")]
    public ActionResult<ResultadoOperacion> Restar([FromQuery] double a, [FromQuery] double b)
    {
        var resultado = _calculadoraService.Restar(a, b);
        return Ok(new ResultadoOperacion
        {
            Valor1 = a,
            Valor2 = b,
            Operacion = "Resta",
            Resultado = resultado
        });
    }

    /// <summary>
    /// Multiplica dos números
    /// </summary>
    [HttpGet("multiplicar")]
    public ActionResult<ResultadoOperacion> Multiplicar([FromQuery] double a, [FromQuery] double b)
    {
        var resultado = _calculadoraService.Multiplicar(a, b);
        return Ok(new ResultadoOperacion
        {
            Valor1 = a,
            Valor2 = b,
            Operacion = "Multiplicación",
            Resultado = resultado
        });
    }

    /// <summary>
    /// Divide dos números
    /// </summary>
    [HttpGet("dividir")]
    public ActionResult<ResultadoOperacion> Dividir([FromQuery] double a, [FromQuery] double b)
    {
        try
        {
            var resultado = _calculadoraService.Dividir(a, b);
            return Ok(new ResultadoOperacion
            {
                Valor1 = a,
                Valor2 = b,
                Operacion = "División",
                Resultado = resultado
            });
        }
        catch (DivideByZeroException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Calcula la potencia
    /// </summary>
    [HttpGet("potencia")]
    public ActionResult<ResultadoOperacion> Potencia([FromQuery] double baseNum, [FromQuery] double exponente)
    {
        var resultado = _calculadoraService.Potencia(baseNum, exponente);
        return Ok(new ResultadoOperacion
        {
            Valor1 = baseNum,
            Valor2 = exponente,
            Operacion = "Potencia",
            Resultado = resultado
        });
    }

    /// <summary>
    /// Calcula la raíz cuadrada
    /// </summary>
    [HttpGet("raiz")]
    public ActionResult<double> RaizCuadrada([FromQuery] double numero)
    {
        try
        {
            var resultado = _calculadoraService.RaizCuadrada(numero);
            return Ok(new { numero, resultado });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Verifica si un número es primo
    /// </summary>
    [HttpGet("esprimo/{numero}")]
    public ActionResult<bool> EsPrimo(int numero)
    {
        var esPrimo = _calculadoraService.EsPrimo(numero);
        return Ok(new { numero, esPrimo });
    }

    /// <summary>
    /// Calcula el factorial de un número
    /// </summary>
    [HttpGet("factorial/{numero}")]
    public ActionResult<int> Factorial(int numero)
    {
        try
        {
            var resultado = _calculadoraService.Factorial(numero);
            return Ok(new { numero, factorial = resultado });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

