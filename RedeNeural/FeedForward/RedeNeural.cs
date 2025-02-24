using System.Globalization;
using RedeNeural.FeedForward.Calculos;

namespace RedeNeural.FeedForward;

internal class RedeNeural
{
    private readonly Camada _camadaDeEntrada;
    private readonly Camada _camadaSaida;
    private readonly int _tamanhoDosDadosDeSaida;
    private readonly IFuncoes _funcoes;

    public RedeNeural(
        IFuncoes funcoes,
        int tamanhoDosDadosDeEntrada,
        int tamanhoDosDadosDeSaida)
    {
        _funcoes = funcoes;
        _tamanhoDosDadosDeSaida = tamanhoDosDadosDeSaida;

        //Uma regra prática sugere que o número de neurônios em uma camada
        //deve estar entre o número de neurônios na camada de entrada e o número de neurônios na camada de saída.
        var quantidadeDeNeuronios = (tamanhoDosDadosDeEntrada + tamanhoDosDadosDeSaida) / 2;

        _camadaDeEntrada = new Camada(quantidadeDeNeuronios, tamanhoDosDadosDeEntrada);
        _camadaSaida = new Camada(quantidadeDeNeuronios, tamanhoDosDadosDeSaida);
    }

    public void Treinar(Dataset dataset, int quantidadeDeEpocas, double taxaDeAprendizagem)
    {
        Console.WriteLine();
        Console.WriteLine("Treinando a rede neural...");

        for (var epoca = 0; epoca < quantidadeDeEpocas; epoca++)
        {
            for (var i = 0; i < dataset.Entrada.Length; i++)
            {
                Console.WriteLine(
                    $"Época: {Format(epoca + 1)} | Entrada[{Format(i)}] = {Join(dataset.Entrada[i])} | Saida[{Format(i)}] = {Join(dataset.SaidaEsperada[i])} ");
                Treinar(dataset.Entrada[i], dataset.SaidaEsperada[i], taxaDeAprendizagem);
            }
        }
    }

    private void Treinar(double[] entrada, double[] saidaEsperada, double taxaDeAprendizagem)
    {
        var resultadoCamadaDeEntrada = Processar(_camadaDeEntrada, entrada);
        var resultadoCamadaDeSaida = Processar(_camadaSaida, resultadoCamadaDeEntrada);
        var deltasCamadaDeSaida = CalcularDeltaErroCamadaSaida(resultadoCamadaDeSaida, saidaEsperada);
        var deltasCamadaDeEntrada =
            CalcularDeltaErroCamadaDeEntrada(resultadoCamadaDeEntrada, deltasCamadaDeSaida, _camadaSaida.ObterPesos());

        AtualizarPesosEBias(_camadaSaida, resultadoCamadaDeEntrada, deltasCamadaDeSaida, taxaDeAprendizagem);
        AtualizarPesosEBias(_camadaDeEntrada, entrada, deltasCamadaDeEntrada, taxaDeAprendizagem);
    }

    public MatrizDeConfusao Testar(Dataset dataset)
    {
        Console.WriteLine();
        Console.WriteLine("Testando a rede neural:");

        var matrizDeConfusao = new MatrizDeConfusao(_tamanhoDosDadosDeSaida, dataset.Classes);

        for (var i = 0; i < dataset.Entrada.Length; i++)
        {
            var resultadoCamadaDeEntrada = Processar(_camadaDeEntrada, dataset.Entrada[i]);
            var resultadoCamadaDeSaida = Processar(_camadaSaida, resultadoCamadaDeEntrada);

            var indiceEsperado = Array.IndexOf(dataset.SaidaEsperada[i], dataset.SaidaEsperada[i].Max());
            var indicePredito = Array.IndexOf(resultadoCamadaDeSaida, resultadoCamadaDeSaida.Max());

            matrizDeConfusao.AdicionarResultado(indiceEsperado, indicePredito);


            Console.Write($"Entrada: [{Join(dataset.Entrada[i])}] ");
            Console.Write($"| Esperado: [{Join(dataset.SaidaEsperada[i])}] ");
            Console.Write($"| Saída: [{Join(resultadoCamadaDeSaida, 22)}] ");
            Console.WriteLine();
        }

        return matrizDeConfusao;
    }

    public double[] Predizer(double[] entradas)
    {
        Console.WriteLine();
        Console.WriteLine("Predizer:");
        var resultadoCamadaDeEntrada = Processar(_camadaDeEntrada, entradas);
        var resultadoCamadaDeSaida = Processar(_camadaSaida, resultadoCamadaDeEntrada);

        Console.Write($"Entrada: [{Join(entradas)}] ");
        Console.Write($"| Saída: [{Join(resultadoCamadaDeSaida, 22)} ] ");
        Console.WriteLine();

        return resultadoCamadaDeSaida;
    }

    private double[] Processar(Camada camada, double[] entrada)
    {
        var saida = new double[camada.Neuronios.Length];
        for (var i = 0; i < camada.Neuronios.Length; i++)
        {
            var somaPonderada = CalcularSomaPonderada(camada.Neuronios[i], entrada);
            saida[i] = _funcoes.Ativar(somaPonderada + camada.Neuronios[i].Bias);
        }

        return saida;
    }

    private double CalcularSomaPonderada(Neuronio neuronio, double[] entrada)
    {
        var soma = 0.0;
        for (var i = 0; i < entrada.Length; i++)
            soma += entrada[i] * neuronio.Pesos[i];

        return soma;
    }

    private void AtualizarPesosEBias(Camada camada, double[] entrada, double[] delta, double taxaDeAprendizagem)
    {
        for (var i = 0; i < camada.Neuronios.Length; i++)
            AtualizarPesosEBias(camada.Neuronios[i], entrada, delta[i], taxaDeAprendizagem);
    }

    private void AtualizarPesosEBias(Neuronio neuronio, double[] entrada, double delta, double taxaDeAprendizagem)
    {
        for (var i = 0; i < neuronio.Pesos.Length; i++)
            neuronio.Pesos[i] -= taxaDeAprendizagem * delta * entrada[i];

        neuronio.Bias -= taxaDeAprendizagem * delta;
    }

    private double[] CalcularDeltaErroCamadaSaida(double[] saida, double[] saidaEsperada)
    {
        var deltas = new double[saida.Length];
        for (var o = 0; o < saida.Length; o++)
        {
            var erro = saida[o] - saidaEsperada[o];
            deltas[o] = erro * _funcoes.Derivada(saida[o]);
        }

        return deltas;
    }

    private double[] CalcularDeltaErroCamadaDeEntrada(double[] saida, double[] deltas,
        double[][] pesos)
    {
        var delta = new double[saida.Length];
        for (var i = 0; i < saida.Length; i++)
        {
            var erro = 0.0;
            for (var j = 0; j < deltas.Length; j++)
                erro += deltas[j] * pesos[i][j];
            delta[i] = erro * _funcoes.Derivada(saida[i]);
        }

        return delta;
    }

    private static string Join(double[] array, int padLeft = 3)
        => string.Join(",", array.Select(a => Format(a, padLeft)));

    private static string Format(double value, int padLeft = 3)
        => value.ToString(CultureInfo.InvariantCulture)
            .Replace(",", ".")
            .PadLeft(padLeft);
}