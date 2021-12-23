// See https://aka.ms/new-console-template for more information
using SpesaMaster.ConsoleApp;

Console.WriteLine("Benvenuto nell'applicazione della Spesa");

bool continua = true;

do
{
    Console.WriteLine("\n[1] Inserisci una nuova spesa" +
        "\n[2] Approva una spesa esistente" +
        "\n[3] Cancella una spesa esistente" +
        "\n[4] Mostra l'elenco delle spese approvate" +
        "\n[5] Mostra l'elenco delle spese di uno specifico utente" +
        "\n[6] Mostra il totale delle spese per categoria" +
        "\n[7] Esci\n");

    int scelta;
    do
    {
        Console.WriteLine("Seleziona un'opzione");
    }
    while (!(int.TryParse(Console.ReadLine(), out scelta) && scelta >= 1 && scelta <= 7));
    
    switch (scelta)
    {
        case 1:
            DisconnectedModeClient.InsertSpesa();
            break;
        case 2:
            ConnectedModeClient.ApprovaSpesa();
            break;
        case 3:
            ConnectedModeClient.EliminaSpesa();
            break;
        case 4:
            ConnectedModeClient.SpeseApprovate();
            break;
        case 5:
            ConnectedModeClient.SpesePerUtente();
            break;
        case 6:
            ConnectedModeClient.TotaleSpesePerCategoria();
            break;
        case 7:
            Console.WriteLine("Arrivederci");
            continua = false;
            break;
    }



}
while (continua == true);


