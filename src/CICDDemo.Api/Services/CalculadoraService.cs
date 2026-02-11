namespace CICDDemo.Api.Services;

/// <summary>
/// Implementación del servicio de calculadora
/// </summary>
public class CalculadoraService : ICalculadoraService
{
    public double Sumar(double a, double b)
    {
        return a + b;
    }

    public double Restar(double a, double b)
    {
        return a - b;
    }

    public double Multiplicar(double a, double b)
    {
        return a * b;
    }

    public double Dividir(double a, double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("No se puede dividir por cero");
        }
        return a / b;
    }

    public double Potencia(double baseNum, double exponente)
    {
        return Math.Pow(baseNum, exponente);
    }

    public double RaizCuadrada(double numero)
    {
        if (numero < 0)
        {
            throw new ArgumentException("No se puede calcular la raíz cuadrada de un número negativo");
        }
        return Math.Sqrt(numero);
    }

    public bool EsPrimo(int numero)
    {
        if (numero <= 1) return false;
        if (numero == 2) return true;
        if (numero % 2 == 0) return false;

        for (int i = 3; i <= Math.Sqrt(numero); i += 2)
        {
            if (numero % i == 0) return false;
        }
        return true;
    }

    public int Factorial(int numero)
    {
        if (numero < 0)
        {
            throw new ArgumentException("No se puede calcular el factorial de un número negativo");
        }
        if (numero <= 1) return 1;
        
        int resultado = 1;
        for (int i = 2; i <= numero; i++)
        {
            resultado *= i;
        }
        return resultado;
    }
}

