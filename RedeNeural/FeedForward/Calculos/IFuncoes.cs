namespace RedeNeural.FeedForward.Calculos;

public interface IFuncoes
{
    double Ativar(double valor);
    double Derivada(double valor);
}

public class Sigmoid : IFuncoes
{
    public double Ativar(double valor)
        => 1.0 / (1.0 + Math.Exp(-valor));

    public double Derivada(double valorDaSigmoide)
        => valorDaSigmoide * (1 - valorDaSigmoide);
}

public class TangenteHiperbolica : IFuncoes
{
    public double Ativar(double valor)
        => Math.Tanh(valor);

    public double Derivada(double valorDaTangenteHiperbolica)
        => 1 - Math.Pow(valorDaTangenteHiperbolica, 2);
}

public class ReLU : IFuncoes
{
    public double Ativar(double valor)
        => Math.Max(0, valor);
    
    //SoftMax

    public double Derivada(double valorDaReLu)
        => valorDaReLu > 0 ? 1 : 0;
}