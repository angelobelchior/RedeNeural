using static RedeNeural.FeedForward.Calculos.Funcoes;
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
        using var _ = new MedidorDeTempo(nameof(Treinar));

        Console.WriteLine();
        Console.WriteLine("Treinando a rede neural...");
        Console.WriteLine("Quantidade de épocas: " + quantidadeDeEpocas);
        Console.WriteLine("Taxa de aprendizagem: " + taxaDeAprendizagem);
        Console.WriteLine("Função de ativação: " + _funcoes.GetType().Name);

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
        var resultadoCamadaDeEntrada = Calcular(_camadaDeEntrada, entrada);
        var resultadoCamadaDeSaida = Calcular(_camadaSaida, resultadoCamadaDeEntrada);

        var deltasCamadaDeSaida = CalcularDeltaErroCamadaSaida(resultadoCamadaDeSaida, saidaEsperada);
        var deltasCamadaDeEntrada =
            CalcularDeltaErroCamadaDeEntrada(resultadoCamadaDeEntrada, deltasCamadaDeSaida, _camadaSaida.ObterPesos());

        AtualizarPesosEVies(_camadaSaida, resultadoCamadaDeEntrada, deltasCamadaDeSaida, taxaDeAprendizagem);
        AtualizarPesosEVies(_camadaDeEntrada, entrada, deltasCamadaDeEntrada, taxaDeAprendizagem);

        //TODO: Incluir nível de erro para parar a execução das épocas
    }

    public MatrizDeConfusao Testar(Dataset dataset)
    {
        using var _ = new MedidorDeTempo(nameof(Testar));

        Console.WriteLine();
        Console.WriteLine("Testando a rede neural...");
        Console.WriteLine("Quantidade de Registros: " + dataset.Entrada.Length);
        Console.WriteLine("Classes: " + string.Join(", ", dataset.Classes));
        Console.WriteLine("Função de ativação: " + _funcoes.GetType().Name);

        var matrizDeConfusao = new MatrizDeConfusao(_tamanhoDosDadosDeSaida, dataset.Classes);

        for (var i = 0; i < dataset.Entrada.Length; i++)
        {
            var resultadoCamadaDeEntrada = Calcular(_camadaDeEntrada, dataset.Entrada[i]);
            var resultadoCamadaDeSaida = Calcular(_camadaSaida, resultadoCamadaDeEntrada);

            // Colocar linha de corte: ex: se o valor for menor que 0.5, então é 0, senão é 1

            var indiceEsperado = Array.IndexOf(dataset.SaidaEsperada[i], dataset.SaidaEsperada[i].Max());
            var indicePredito = Array.IndexOf(resultadoCamadaDeSaida, resultadoCamadaDeSaida.Max());

            matrizDeConfusao.AdicionarResultado(indiceEsperado, indicePredito);


            Console.Write($"Entrada: [{Join(dataset.Entrada[i])}] ");
            Console.Write($"| Esperado: [{Join(dataset.SaidaEsperada[i])}] ");
            Console.Write($"| Saída: [{Join(resultadoCamadaDeSaida, arredondar: 2, padLeft: 5)}] ");
            Console.WriteLine();
        }

        return matrizDeConfusao;
    }

    public double[] Predizer(double[] entrada)
    {
        using var _ = new MedidorDeTempo(nameof(Predizer));

        var resultadoCamadaDeEntrada = Calcular(_camadaDeEntrada, entrada);
        var resultadoCamadaDeSaida = Calcular(_camadaSaida, resultadoCamadaDeEntrada);

        return resultadoCamadaDeSaida;
    }

    private double[] Calcular(Camada camada, double[] entrada)
    {
        var saida = new double[camada.Neuronios.Length];
        for (var i = 0; i < camada.Neuronios.Length; i++)
        {
            var somaPonderada = CalcularSomaPonderada(camada.Neuronios[i], entrada);
            saida[i] = _funcoes.Ativar(somaPonderada + camada.Neuronios[i].Vies);
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

    private void AtualizarPesosEVies(Camada camada, double[] entrada, double[] delta, double taxaDeAprendizagem)
    {
        for (var i = 0; i < camada.Neuronios.Length; i++)
            AtualizarPesosEVies(camada.Neuronios[i], entrada, delta[i], taxaDeAprendizagem);
    }

    private void AtualizarPesosEVies(Neuronio neuronio, double[] entrada, double delta, double taxaDeAprendizagem)
    {
        for (var i = 0; i < neuronio.Pesos.Length; i++)
            neuronio.Pesos[i] -= taxaDeAprendizagem * delta * entrada[i];

        neuronio.Vies -= taxaDeAprendizagem * delta;
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
}