using RedeNeural.FeedForward.Calculos;

namespace RedeNeural.FeedForward;

internal class Neuronio
{
    public double[] Pesos { get; }
    private double _bias;

    private readonly IFuncoes _funcoes;
    
    public Neuronio(int quantidadeDeEntradas, IFuncoes funcoes)
    {
        _funcoes = funcoes;
        Pesos = new double[quantidadeDeEntradas];
        for (var i = 0; i < quantidadeDeEntradas; i++)
            Pesos[i] = Funcoes.ObterValorAleatorio();
        _bias = Funcoes.ObterValorAleatorio();
    }

    public double CalcularSomaPonderada(double[] entrada)
    {
        var soma = 0.0;
        for (var i = 0; i < entrada.Length; i++)
            soma += entrada[i] * Pesos[i];
        
        return _funcoes.Ativar(soma + _bias);
    }

    public void AtualizarPesosEBias(double[] entrada, double delta, double taxaDeAprendizagem)
    {
        for (var i = 0; i < Pesos.Length; i++)
            Pesos[i] -= taxaDeAprendizagem * delta * entrada[i];
        _bias -= taxaDeAprendizagem * delta;
    }
}