using System.Diagnostics;
using GestionnaireReseauWindows.Services;

var exitRequested = false;

while (!exitRequested)
{
    Console.Clear();
    Console.WriteLine("=== Gestionnaire réseau Windows ===");
    Console.WriteLine("1. Afficher les interfaces réseau");
    Console.WriteLine("2. Afficher les adresses IP");
    Console.WriteLine("3. Démarrer le point d'accès Wi‑Fi");
    Console.WriteLine("4. Arrêter le point d'accès Wi‑Fi");
    Console.WriteLine("5. Vérifier l'état du point d'accès");
    Console.WriteLine("6. Quitter");
    Console.Write("Votre choix : ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ShowInterfaces();
            break;

        case "2":
            ShowIpAddresses();
            break;

        case "3":
            StartHotspot();
            break;

        case "4":
            StopHotspot();
            break;

        case "5":
            ShowHotspotStatus();
            break;

        case "6":
            exitRequested = true;
            Console.WriteLine("Au revoir.");
            break;

        default:
            Console.WriteLine("Choix invalide.");
            break;
    }

    if (!exitRequested)
    {
        Console.WriteLine();
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }
}

static void ShowInterfaces()
{
    Console.WriteLine();
    Console.WriteLine("Interfaces réseau :");

    var interfaces = NetworkService.GetInterfaces();

    if (interfaces.Count == 0)
    {
        Console.WriteLine("Aucune interface détectée.");
        return;
    }

    foreach (var item in interfaces)
    {
        Console.WriteLine($"- {item.Name} | {item.Description}");
        Console.WriteLine($"  Statut : {item.OperationalStatus}");
        Console.WriteLine($"  Type : {(item.IsWireless ? "Wi‑Fi" : "Ethernet / Autre")}");
        Console.WriteLine($"  IPs : {(item.IpAddresses.Count > 0 ? string.Join(", ", item.IpAddresses) : "Aucune")}");
    }
}

static void ShowIpAddresses()
{
    Console.WriteLine();
    Console.WriteLine("Adresses IPv4 détectées :");

    var ips = NetworkService.GetIpAddresses();

    if (ips.Count == 0)
    {
        Console.WriteLine("Aucune adresse IPv4 détectée.");
        return;
    }

    foreach (var ip in ips)
    {
        Console.WriteLine($"- {ip}");
    }

    Console.WriteLine($"Adresse principale : {NetworkService.GetPrimaryIpv4Address()}");
}

static void StartHotspot()
{
    if (!OperatingSystem.IsWindows())
    {
        Console.WriteLine("Cette fonction est disponible uniquement sur Windows.");
        return;
    }

    Console.Write("Nom du réseau (SSID) : ");
    var ssid = Console.ReadLine() ?? "MonReseau";

    Console.Write("Mot de passe (8+ caractères) : ");
    var password = Console.ReadLine() ?? "motdepasse123";

    try
    {
        var command = HotspotCommandBuilder.BuildStartCommand(ssid, password);
        ExecuteWindowsCommand(command);
        Console.WriteLine($"Commande exécutée : {command}");
        Console.WriteLine("Le point d'accès a été configuré. Utilisez la commande de démarrage si nécessaire.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Erreur : {ex.Message}");
    }
}

static void StopHotspot()
{
    if (!OperatingSystem.IsWindows())
    {
        Console.WriteLine("Cette fonction est disponible uniquement sur Windows.");
        return;
    }

    var command = HotspotCommandBuilder.BuildStopCommand();
    ExecuteWindowsCommand(command);
    Console.WriteLine($"Commande exécutée : {command}");
}

static void ShowHotspotStatus()
{
    if (!OperatingSystem.IsWindows())
    {
        Console.WriteLine("Cette fonction est disponible uniquement sur Windows.");
        return;
    }

    var command = HotspotCommandBuilder.BuildStatusCommand();
    ExecuteWindowsCommand(command);
    Console.WriteLine($"Commande exécutée : {command}");
}

static void ExecuteWindowsCommand(string command)
{
    var psi = new ProcessStartInfo
    {
        FileName = "cmd.exe",
        Arguments = $"/c {command}",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using var process = Process.Start(psi) ?? throw new InvalidOperationException("Impossible de lancer la commande Windows.");
    var output = process.StandardOutput.ReadToEnd();
    var error = process.StandardError.ReadToEnd();
    process.WaitForExit();

    if (!string.IsNullOrWhiteSpace(output))
    {
        Console.WriteLine(output.Trim());
    }

    if (!string.IsNullOrWhiteSpace(error))
    {
        Console.WriteLine(error.Trim());
    }
}

