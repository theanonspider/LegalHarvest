# 🛡️ LEGALHARVEST V1 — DOCUMENTATION OFFICIELLE

> **Simulateur d'infostealer pour tests de sécurité autorisés**
> Version publique — Open Source — Usage éducatif

---

## 📊 FICHE TECHNIQUE

| Élément | Détail |
|---------|--------|
| **Nom** | LegalHarvest |
| **Version** | 1.0 (Publique) |
| **Type** | Simulateur d'infostealer post-exploitation |
| **Licence** | MIT (usage éducatif uniquement) |
| **Langage** | C# (.NET Framework 4.8) |
| **Plateforme** | Windows 10/11 |
| **Interface** | GUI Anonymous Edition (violet) |
| **Modules** | 39 |
| **Dépôt** | github.com/theanonspider/LegalHarvest |

---

## 🧩 MODULES DE COLLECTE

### 🔐 Navigateurs
- Mots de passe Chrome, Edge, Brave, Opera, Opera GX, Vivaldi, Yandex, Chromium
- Cookies de tous les navigateurs Chromium
- Mots de passe et cookies Firefox
- Historique de navigation (Chrome + Firefox)
- Cartes bancaires enregistrées
- Données d'auto-complétion

### 🍪 Sessions web
- Facebook, Instagram, Twitter/X, LinkedIn, TikTok, Reddit, GitHub
- Google, YouTube, Microsoft, Live, Amazon, eBay, Dropbox
- Discord, Telegram, WhatsApp, Snapchat, Pinterest, Twitch, Spotify, Netflix

### 💬 Messageries
- Discord (normal, Canary, PTB)
- Telegram Desktop
- Signal, Zoom, Pidgin
- Skype, Slack, WhatsApp Desktop, Microsoft Teams
- Outlook, Thunderbird

### 🎮 Gaming
- Steam (sessions, ssfn)
- Battle.net, Epic Games, Ubisoft Connect

### 🪙 Cryptomonnaies
- Wallets : Exodus, Electrum, Atomic, Jaxx, Guarda, Binance, Coinbase
- Extensions : MetaMask, Phantom, TronLink, Binance Chain Wallet, Coinbase Wallet, Keplr

### 🔑 Gestionnaires de mots de passe
- KeePass (.kdbx), Bitwarden, NordPass

### 🌐 Réseau & VPN
- Clés Wi-Fi, Gestionnaire d'identifiants Windows
- FileZilla (configs + mots de passe déchiffrés), WinSCP
- VPN Windows, NordVPN, OpenVPN, ProtonVPN

### 🖥️ Système & Environnement
- Informations système (hostname, user, OS, IP)
- Matériel (CPU, RAM, GPU, résolution)
- Logiciels installés, processus en cours
- Presse-papiers, clés SSH
- Fichiers sensibles (.docx, .pdf, .kdbx, .rdp, etc.)
- Fichiers récemment ouverts
- Captures d'écran tous les moniteurs

---

## 🔐 SÉCURITÉ

| Mécanisme | Description |
|-----------|-------------|
| **Token d'autorisation** | Fichier `C:\ProgramData\legal_harvest.token` obligatoire |
| **Pas d'exfiltration** | Aucune connexion réseau sortante |
| **Local uniquement** | Résultats sauvegardés dans `%TEMP%` |
| **Code source ouvert** | Vérifiable par tous |

---

## ⚙️ COMPILATION

### Prérequis
- Windows 10/11
- Visual Studio 2022 Community
- .NET Framework 4.8
- Packages NuGet : `System.Data.SQLite.Core`, `Newtonsoft.Json`
- Références : `System.Windows.Forms`, `System.Management`, `System.Drawing`

### Build


1. git clone https://github.com/theanonspider/LegalHarvest.git
2. Ouvrir LegalHarvest.sln dans Visual Studio
3. Restaurer les packages NuGet
4. Compiler en Release x64

5. # 1. Créer le token
echo MISSION_AUTHORIZED_2024 > C:\ProgramData\legal_harvest.token

# 2. Lancer
LegalHarvest.exe
