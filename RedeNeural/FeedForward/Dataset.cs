namespace RedeNeural.FeedForward;

public record Dataset(double[][] Entrada, double[][] SaidaEsperada);

public class MatrizDeConfusao
{
    private readonly int[,] _matriz;
    private readonly int _numClasses;

    public MatrizDeConfusao(int numClasses)
    {
        _numClasses = numClasses;
        _matriz = new int[numClasses, numClasses];
    }

    public void AdicionarResultado(int esperado, int predito)
    {
        _matriz[esperado, predito]++;
    }

    public double CalcularAcuracia()
    {
        int corretos = 0, total = 0;
        for (var i = 0; i < _numClasses; i++)
        {
            for (var j = 0; j < _numClasses; j++)
            {
                total += _matriz[i, j];
                if (i == j) corretos += _matriz[i, j];
            }
        }
        return (double)corretos / total;
    }

    public double[] CalcularPrecisao()
    {
        var precisao = new double[_numClasses];
        for (var i = 0; i < _numClasses; i++)
        {
            var tp = _matriz[i, i];
            var fp = 0;
            for (var j = 0; j < _numClasses; j++)
            {
                if (i != j) fp += _matriz[j, i];
            }
            precisao[i] = tp + fp == 0 ? 0 : (double)tp / (tp + fp);
        }
        return precisao;
    }

    public double[] CalcularSensibilidade()
    {
        var sensibilidade = new double[_numClasses];
        for (var i = 0; i < _numClasses; i++)
        {
            var tp = _matriz[i, i];
            var fn = 0;
            for (var j = 0; j < _numClasses; j++)
            {
                if (i != j) fn += _matriz[i, j];
            }
            sensibilidade[i] = tp + fn == 0 ? 0 : (double)tp / (tp + fn);
        }
        return sensibilidade;
    }

    public double[] CalcularF1Score()
    {
        var precisao = CalcularPrecisao();
        var sensibilidade = CalcularSensibilidade();
        var f1 = new double[_numClasses];
        
        for (var i = 0; i < _numClasses; i++)
        {
            f1[i] = (precisao[i] + sensibilidade[i]) == 0 ? 0 : 
                    2 * (precisao[i] * sensibilidade[i]) / (precisao[i] + sensibilidade[i]);
        }
        return f1;
    }

    public void ExibirMatriz()
    {
        Console.WriteLine("Matriz de Confusão:");
        for (var i = 0; i < _numClasses; i++)
        {
            for (var j = 0; j < _numClasses; j++)
            {
                Console.Write(_matriz[i, j] + " ");
            }
            Console.WriteLine();
        }
        
        Console.WriteLine("Acertos: " + CalcularAcuracia());
        Console.WriteLine("Precisão: " + string.Join(", ", CalcularPrecisao()));
        Console.WriteLine("Sensibilidade: " + string.Join(", ", CalcularSensibilidade()));
        Console.WriteLine("F1 Score: " + string.Join(", ", CalcularF1Score()));
    }
}
