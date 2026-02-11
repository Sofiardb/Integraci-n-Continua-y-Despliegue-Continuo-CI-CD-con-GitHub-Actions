namespace CICDDemo.Api.Services;

/// <summary>
/// Interface para el servicio de calculadora
/// </summary>
public interface ICalculadoraService
{
    double Sumar(double a, double b);
    double Restar(double a, double b);
    double Multiplicar(double a, double b);
    double Dividir(double a, double b);
    double Potencia(double baseNum, double exponente);
    double RaizCuadrada(double numero);
    bool EsPrimo(int numero);
    int Factorial(int numero);
}

