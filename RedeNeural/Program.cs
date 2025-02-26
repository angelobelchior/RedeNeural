using RedeNeural.FeedForward;
using RedeNeural.FeedForward.Calculos;

var datasetDeTreino = Dataset.CriarDatasetDeTreinoIris();
var datasetDeTeste = Dataset.CriarDatasetDeTesteIris();

var tamanhoDosDadosDeEntrada = datasetDeTreino.Entrada[0].Length;
var tamanhoDosDadosDeSaida = datasetDeTreino.Classes.Length;
var redeNeural = new RedeNeural.FeedForward.RedeNeural(
    funcoes: Funcoes.Sigmoid,
    tamanhoDosDadosDeEntrada: tamanhoDosDadosDeEntrada,
    tamanhoDosDadosDeSaida: tamanhoDosDadosDeSaida,
    quantidadeDeNeuronios: 3);


redeNeural.Treinar(datasetDeTreino, quantidadeDeEpocas: 1000, 0.1);

var matrizDeConfusao = redeNeural.Testar(datasetDeTeste);
matrizDeConfusao.ExibirMatriz();

Console.WriteLine();
Console.WriteLine("Predizer :");

Predizer([5.1, 3.5, 1.4, 0.2], "Setosa");
Predizer([5.9, 3.0, 4.2, 1.5], "Versicolor");
Predizer([6.3, 3.3, 6.0, 2.5], "Virginica");

return;

void Predizer(double[] entrada, string classe)
{
    var saida = redeNeural.Predizer(entrada);
    Console.Write($"Classe: {classe,12} | ");
    Console.Write($"Entrada: [{Funcoes.Join(entrada, padLeft: 5)}] ");
    Console.Write($"| Saída: [{Funcoes.Join(saida, padLeft: 5)} ] ");
    Console.WriteLine();
}