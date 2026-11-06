# Gestionnaire réseau Windows

Application Windows 64 bits de gestion réseau : consultation des connexions et adresses IP, avec création d’un point d’accès Wi‑Fi lorsque le matériel le permet.

## Fonctionnalités

- Analyse des interfaces réseau actives
- Consultation des adresses IPv4 associées
- Vérification de l’état du point d’accès Wi‑Fi
- Activation ou arrêt d’un hotspot Wi‑Fi via la commande `netsh wlan`
- Interface en ligne de commande simple et fiable pour un usage rapide

## Structure du projet

- `src/GestionnaireReseauWindows` : application principale
- `tests/GestionnaireReseauWindows.Tests` : tests unitaires

## Prérequis

- Windows 10/11 64 bits
- .NET SDK 10
- Carte réseau compatible avec les points d’accès Wi‑Fi

## Lancement

```bash
dotnet restore

dotnet run --project src/GestionnaireReseauWindows/GestionnaireReseauWindows.csproj
```

## Exemple de commande réseau

```powershell
netsh wlan show hostednetwork
netsh wlan set hostednetwork mode=allow ssid=MonReseau key=motdepasse123
netsh wlan start hostednetwork
netsh wlan stop hostednetwork
```

## Tests

```bash
dotnet test GestionnaireReseauWindows.slnx --nologo
```

## Notes

L’application repose sur les commandes standards Windows. Sur Linux, le mode Wi‑Fi n’est pas exécutable, mais le projet est prêt pour un build Windows 64 bits et un développement local sur machine compatible.
