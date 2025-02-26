using RedeNeural.FeedForward.Calculos;

namespace RedeNeural.FeedForward;

internal class Neuronio
{
    public double[] Pesos { get; set; }
    public double Vies { get; set; }

    public Neuronio(int quantidadeDeEntradas)
    {
        Pesos = new double[quantidadeDeEntradas];
        for (var i = 0; i < quantidadeDeEntradas; i++)
            Pesos[i] = Funcoes.ObterValorAleatorio();

        Vies = Funcoes.ObterValorAleatorio();
    }
}