namespace CICDDemo.Api.Models;

/// <summary>
/// Resultado de una operación matemática
/// </summary>
public class ResultadoOperacion
{
    public double Valor1 { get; set; }
    public double Valor2 { get; set; }
    public string Operacion { get; set; } = string.Empty;
    public double Resultado { get; set; }
}

