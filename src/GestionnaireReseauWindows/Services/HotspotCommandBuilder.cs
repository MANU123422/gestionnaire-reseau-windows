namespace GestionnaireReseauWindows.Services;

public static class HotspotCommandBuilder
{
    public static string BuildStartCommand(string ssid, string password)
    {
        if (string.IsNullOrWhiteSpace(ssid))
        {
            throw new ArgumentException("Le nom du réseau doit être renseigné.", nameof(ssid));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Le mot de passe doit être renseigné.", nameof(password));
        }

        if (password.Length < 8)
        {
            throw new ArgumentException("Le mot de passe doit contenir au moins 8 caractères.", nameof(password));
        }

        return $"netsh wlan set hostednetwork mode=allow ssid={ssid} key={password}";
    }

    public static string BuildStopCommand()
    {
        return "netsh wlan stop hostednetwork";
    }

    public static string BuildStatusCommand()
    {
        return "netsh wlan show hostednetwork";
    }
}
