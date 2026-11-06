using System.Net.NetworkInformation;

namespace GestionnaireReseauWindows.Services;

public sealed record NetworkInterfaceInfo(
    string Name,
    string Description,
    string OperationalStatus,
    bool IsWireless,
    IReadOnlyList<string> IpAddresses);

public static class NetworkService
{
    public static IReadOnlyList<NetworkInterfaceInfo> GetInterfaces()
    {
        return NetworkInterface
            .GetAllNetworkInterfaces()
            .Select(ni =>
            {
                var addresses = ni.GetIPProperties()
                    .UnicastAddresses
                    .Where(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(ip => ip.Address.ToString())
                    .Where(ip => !string.IsNullOrWhiteSpace(ip))
                    .Distinct()
                    .ToList();

                return new NetworkInterfaceInfo(
                    Name: ni.Name,
                    Description: ni.Description,
                    OperationalStatus: ni.OperationalStatus.ToString(),
                    IsWireless: ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211,
                    IpAddresses: addresses);
            })
            .ToList();
    }

    public static IReadOnlyList<string> GetIpAddresses()
    {
        return GetInterfaces()
            .SelectMany(i => i.IpAddresses)
            .Distinct()
            .ToList();
    }

    public static string GetPrimaryIpv4Address()
    {
        var address = GetIpAddresses()
            .FirstOrDefault(ip => !ip.StartsWith("127.") && ip.Contains('.'));

        return string.IsNullOrWhiteSpace(address) ? "Aucune adresse IPv4 détectée." : address;
    }
}
