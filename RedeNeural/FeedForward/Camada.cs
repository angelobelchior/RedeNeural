using RedeNeural.FeedForward.Calculos;

namespace RedeNeural.FeedForward;

internal class Camada
{
    private readonly Neuronio[] _neuronios;
    private readonly IFuncoes _funcoes;

    public Camada(int quantidadeDeNeuronios, int quantidadeDeEntradas, IFuncoes funcoes)
    {
        _funcoes = funcoes;
        _neuronios = new Neuronio[quantidadeDeNeuronios];
        for (var i = 0; i < quantidadeDeNeuronios; i++)
            _neuronios[i] = new Neuronio(quantidadeDeEntradas, funcoes);
    }

    public double[] Processar(double[] entrada)
    {
        var saida = new double[_neuronios.Length];
        for (var i = 0; i < _neuronios.Length; i++)
            saida[i] = _neuronios[i].CalcularSomaPonderada(entrada);
        return saida;
    }

    public void AtualizarPesosEBias(double[] entrada, double[] delta, double taxaDeAprendizagem)
    {
        for (var i = 0; i < _neuronios.Length; i++)
            _neuronios[i].AtualizarPesosEBias(entrada, delta[i], taxaDeAprendizagem);
    }

    public double[][] ObterPesos()
    {
        var pesos = new double[_neuronios.Length][];
        for (var i = 0; i < _neuronios.Length; i++) pesos[i] = _neuronios[i].Pesos;
        return pesos;
    }
}