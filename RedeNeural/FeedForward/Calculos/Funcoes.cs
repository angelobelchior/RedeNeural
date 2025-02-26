using System.Globalization;

namespace RedeNeural.FeedForward.Calculos;

public static class Funcoes
{
    public static IFuncoes Sigmoid => new Sigmoid();
    public static IFuncoes TangenteHiperbolica => new TangenteHiperbolica();
    public static IFuncoes ReLU => new ReLU();

    public static double ObterValorAleatorio()
        => (Random.Shared.NextDouble() - 0.5) * 0.1;

    public static string Join(double[] array, int arredondar = 2, int padLeft = 3)
        => string.Join(",", array.Select(a => Format(a, arredondar, padLeft)));

    public static string Format(double value, int arredondar = 2, int padLeft = 3)
        => Math.Round(value, arredondar)
            .ToString(CultureInfo.InvariantCulture)
            .Replace(",", ".")
            .PadLeft(padLeft);
}