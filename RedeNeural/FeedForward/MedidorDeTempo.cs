using System.Diagnostics;

namespace RedeNeural.FeedForward;

public class MedidorDeTempo : IDisposable
{
    private readonly string _nomeDoMetodo;
    private readonly Stopwatch _stopwatch;

    public MedidorDeTempo(string nomeDoMetodo)
    {
        _nomeDoMetodo = nomeDoMetodo;
        _stopwatch = new Stopwatch();
        _stopwatch.Start();

        Console.WriteLine($"**********Iniciando Método: {_nomeDoMetodo}**********");
    }
    
    public void Dispose()
    {
        _stopwatch.Stop();
        Console.WriteLine($"**********Tempo decorrido Método: {_nomeDoMetodo}: " + _stopwatch.ElapsedMilliseconds + "ms **********");
    }
}