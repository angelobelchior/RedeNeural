using RedeNeural.FeedForward.Calculos;

namespace RedeNeural.FeedForward;

internal class Neuronio
{
    public double[] Pesos { get; set; }
    public double Bias { get; set; }

    public Neuronio(int quantidadeDeEntradas)
    {
        Pesos = new double[quantidadeDeEntradas];
        for (var i = 0; i < quantidadeDeEntradas; i++)
            Pesos[i] = Funcoes.ObterValorAleatorio();

        Bias = Funcoes.ObterValorAleatorio();
    }
}