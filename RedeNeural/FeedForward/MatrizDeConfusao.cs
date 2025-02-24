namespace RedeNeural.FeedForward;

using System;

public class MatrizDeConfusao(int numClasses, params string[] nomesDasClasses)
{
    private readonly int[,] _matriz = new int[numClasses, numClasses];

    public void AdicionarResultado(int esperado, int predito)
        => _matriz[esperado, predito]++;

    private double CalcularAcuracia()
    {
        int corretos = 0, total = 0;
        for (var i = 0; i < numClasses; i++)
        {
            for (var j = 0; j < numClasses; j++)
            {
                total += _matriz[i, j];
                if (i == j) corretos += _matriz[i, j];
            }
        }

        return total == 0 ? 0.0 : (double)corretos / total;
    }

    private Dictionary<string, double> CalcularPrecisaoPorClasse()
    {
        var precisao = new Dictionary<string, double>(numClasses);
        for (var i = 0; i < numClasses; i++)
        {
            var tp = _matriz[i, i];
            var fp = 0;
            for (var j = 0; j < numClasses; j++)
            {
                if (i != j) fp += _matriz[j, i];
            }

            var valor = (tp + fp) == 0 ? 0.0 : (double)tp / (tp + fp);
            precisao[nomesDasClasses[i]] = valor;
        }

        return precisao;
    }

    private Dictionary<string, double> CalcularSensibilidadePorClasse()
    {
        var sensibilidade = new Dictionary<string, double>(numClasses);
        for (var i = 0; i < numClasses; i++)
        {
            var tp = _matriz[i, i];
            var fn = 0;
            for (var j = 0; j < numClasses; j++)
            {
                if (i != j) fn += _matriz[i, j];
            }

            var valor = (tp + fn) == 0 ? 0.0 : (double)tp / (tp + fn);
            sensibilidade[nomesDasClasses[i]] = valor;
        }

        return sensibilidade;
    }

    private Dictionary<string, double> CalcularF1ScorePorClasse()
    {
        var precisao = CalcularPrecisaoPorClasse();
        var sensibilidade = CalcularSensibilidadePorClasse();
        var f1 = new Dictionary<string, double>(numClasses);

        for (var i = 0; i < numClasses; i++)
        {
            var soma = precisao[nomesDasClasses[i]] + sensibilidade[nomesDasClasses[i]];
            var valor = soma == 0
                ? 0.0
                : 2 * (precisao[nomesDasClasses[i]] * sensibilidade[nomesDasClasses[i]]) / soma;
            f1[nomesDasClasses[i]] = valor;
        }

        return f1;
    }

    public void ExibirMatriz()
    {
        Console.WriteLine("Matriz de Confusão:");
        for (var i = 0; i < numClasses; i++)
        {
            for (var j = 0; j < numClasses; j++)
            {
                Console.Write($"{_matriz[i, j],5}");
            }

            Console.WriteLine();
        }

        Console.WriteLine($"\nAcurácia: {CalcularAcuracia():F2}");

        var precisao = CalcularPrecisaoPorClasse();
        Console.WriteLine("Precisão por classe:");
        foreach (var (classe, valor) in precisao)
            Console.WriteLine($"    {classe}: {valor:F2}");


        var sensibilidade = CalcularSensibilidadePorClasse();
        Console.WriteLine("Sensibilidade por classe:");
        foreach (var (classe, valor) in sensibilidade)
            Console.WriteLine($"    {classe}: {valor:F2}");

        var f1 = CalcularF1ScorePorClasse();
        Console.WriteLine("F1-Score por classe:");
        foreach (var (classe, valor) in f1)
            Console.WriteLine($"    {classe}: {valor:F2}");
    }
}