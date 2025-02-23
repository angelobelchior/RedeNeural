namespace RedeNeural.FeedForward;

internal class Camada
{
    public Neuronio[] Neuronios { get; }

    public Camada(int quantidadeDeNeuronios, int quantidadeDeEntradas)
    {
        Neuronios = new Neuronio[quantidadeDeNeuronios];
        for (var i = 0; i < quantidadeDeNeuronios; i++)
        {
            Neuronios[i] = new Neuronio(quantidadeDeEntradas);
            Console.WriteLine($"Neurônio[{i}]=> Qtd.Entradas: {quantidadeDeEntradas} | Pesos: {string.Join(';', Neuronios[i].Pesos)} | Bias: {Neuronios[i].Bias}");
        }
    }

    public double[][] ObterPesos()
    {
        var pesos = new double[Neuronios.Length][];
        for (var i = 0; i < Neuronios.Length; i++) pesos[i] = Neuronios[i].Pesos;
        return pesos;
    }
}