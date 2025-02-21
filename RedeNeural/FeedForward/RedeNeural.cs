using RedeNeural.FeedForward.Calculos;

namespace RedeNeural.FeedForward;

internal class RedeNeural
{
    private readonly Camada _camadaOculta;
    private readonly Camada _camadaSaida;
    private readonly IFuncoes _funcoes;

    public RedeNeural(
        IFuncoes funcoes,
        int tamanhoDosDadosDeEntrada,
        int tamanhoDosDadosDeSaida)
    {
        _funcoes = funcoes;

        //Uma regra prática sugere que o número de neurônios em uma camada oculta
        //deve estar entre o número de neurônios na camada de entrada e o número de neurônios na camada de saída.
        var quantidadeDeNeuronios = (tamanhoDosDadosDeEntrada + tamanhoDosDadosDeSaida) / 2;

        _camadaOculta = new Camada(quantidadeDeNeuronios, tamanhoDosDadosDeEntrada, funcoes);
        _camadaSaida = new Camada(quantidadeDeNeuronios, tamanhoDosDadosDeSaida, funcoes);
    }

    public void Treinar(Dataset dataset, int quantidadeDeEpocas, double taxaDeAprendizagem)
    {
        Console.WriteLine();
        Console.WriteLine("Treinando a rede neural...");

        for (var epoca = 0; epoca < quantidadeDeEpocas; epoca++)
        {
            for (var i = 0; i < dataset.Entrada.Length; i++)
                Treinar(dataset.Entrada[i], dataset.SaidaEsperada[i], taxaDeAprendizagem);
        }
    }

    private void Treinar(double[] entrada, double[] saidaEsperada, double taxaDeAprendizagem)
    {
        var resultadoCamadaOcultaProcessada = _camadaOculta.Processar(entrada);
        var resultadoCamadaDeSaidaProcessada = _camadaSaida.Processar(resultadoCamadaOcultaProcessada);

        var deltaSaidas = CalcularDeltaErroCamadaSaida(resultadoCamadaDeSaidaProcessada, saidaEsperada);
        var deltasOcultos = CalcularDeltaErroCamadaOculta(resultadoCamadaOcultaProcessada, deltaSaidas, _camadaSaida.ObterPesos());

        _camadaSaida.AtualizarPesosEBias(resultadoCamadaOcultaProcessada, deltaSaidas, taxaDeAprendizagem);
        _camadaOculta.AtualizarPesosEBias(entrada, deltasOcultos, taxaDeAprendizagem);
    }

    public void Testar(Dataset dataset, out double taxaDeErro)
    {
        Console.WriteLine();
        Console.WriteLine("Testando a rede neural:");
        var somaDosErros = 0.0;

        for (var i = 0; i < dataset.Entrada.Length; i++)
        {
            var saidasOcultas = _camadaOculta.Processar(dataset.Entrada[i]);
            var saida = _camadaSaida.Processar(saidasOcultas);
            var erro = CalcularErro(saida, dataset.SaidaEsperada[i]);

            somaDosErros += erro;

            Console.Write($"Entrada: [ {string.Join("; ", dataset.Entrada[i]),-10} ] ");
            Console.Write($"| Esperado: [ {string.Join("; ", dataset.SaidaEsperada[i]),-10} ] ");
            Console.Write($"| Saída: [ {string.Join("; ", saida),-10} ] ");
            Console.Write($"| Erro: {erro,-8}");
            Console.WriteLine();
        }

        taxaDeErro = somaDosErros / dataset.Entrada.Length;
    }

    public double[] Predizer(double[] entradas)
    {
        Console.WriteLine();
        Console.WriteLine("Predizer:");
        var saidasOcultas = _camadaOculta.Processar(entradas);
        var saida = _camadaSaida.Processar(saidasOcultas);

        Console.Write($"Entrada: [ {string.Join(", ", entradas)} ] ");
        Console.Write($"| Saída: [ {string.Join(", ", saida)} ] ");
        Console.WriteLine();

        return saida;
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
    
    private double[] CalcularDeltaErroCamadaOculta(double[] saida, double[] deltaDaProximaCamada, double[][] pesosDaProximaCamada)
    {
        var delta = new double[saida.Length];
        for (var i = 0; i < saida.Length; i++)
        {
            var erro = 0.0;
            for (var j = 0; j < deltaDaProximaCamada.Length; j++)
                erro += deltaDaProximaCamada[j] * pesosDaProximaCamada[i][j];
            delta[i] = erro * _funcoes.Derivada(saida[i]);
        }

        return delta;
    }

    private static double CalcularErro(double[] saida, double[] saidaEsperada)
    {
        var erro = 0.0;
        for (var i = 0; i < saida.Length; i++) 
            erro += Math.Pow(saida[i] - saidaEsperada[i], 2);
        return erro / saida.Length;
    }
}