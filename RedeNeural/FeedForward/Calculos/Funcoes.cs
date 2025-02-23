namespace RedeNeural.FeedForward.Calculos;

public static class Funcoes
{
    public static IFuncoes Sigmoid => new Sigmoid();
    public static IFuncoes TangenteHiperbolica => new TangenteHiperbolica();
    public static IFuncoes ReLU => new ReLU();

    public static double ObterValorAleatorio()
        => (Random.Shared.NextDouble() - 0.5) * 0.1;
}