namespace RedeNeural.FeedForward.Calculos;

public static class Funcoes
{
    public static IFuncoes Sigmoid => new Sigmoid();
    public static IFuncoes TangenteHiperbolica => new TangenteHiperbolica();
    public static IFuncoes ReLU => new ReLU();

    public static IFuncoes ObterFuncao(string nome)
        => nome switch
        {
            nameof(Sigmoid) => Sigmoid,
            _ => throw new ArgumentException("Função não encontrada.")
        };

    public static double ObterValorAleatorio()
        => (Random.Shared.NextDouble() - 0.5) * 0.1;
}