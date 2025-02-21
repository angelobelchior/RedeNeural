namespace RedeNeural.FeedForward.Calculos;

public interface IFuncoes
{
    double Ativar(double valor);
    double Derivada(double valor);
}

public class Sigmoid : IFuncoes
{
    public double Ativar(double valor) => 1.0 / (1.0 + Math.Exp(-valor));

    public double Derivada(double valorDaSigmoide) 
        => valorDaSigmoide * (1 - valorDaSigmoide);
}

public class TangenteHiperbolica : IFuncoes
{
    public double Ativar(double valor)
    {
        var x = Math.Exp(valor);
        var negativo = Math.Exp(-valor);
        return (x - negativo) / (x + negativo);
    }

    public double Derivada(double valor)
    {
        var x = Math.Exp(valor);
        var negativo = Math.Exp(-valor);
        var denominador = x + negativo;
        return 4 * x * negativo / (denominador * denominador);
    }
}

public class ReLU : IFuncoes
{
    public double Ativar(double valor) => Math.Max(0, valor);

    public double Derivada(double valor) => valor > 0 ? 1 : 0;
}