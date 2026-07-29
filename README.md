# LegalHarvest

> ⚠️ **AVERTISSEMENT** — Cet outil est un **simulateur d'infostealer** conçu exclusivement pour :
> - Des tests d'intrusion **autorisés** (pentests, Red Team)
> - La **formation** et la **recherche** en cybersécurité défensive
> - Des démonstrations d'impact dans un cadre **contractuel**
>
> **Toute utilisation sur un système sans autorisation écrite est ILLÉGALE.**
> L'auteur décline toute responsabilité en cas d'usage malveillant.

---

## 📖 Description

**LegalHarvest** est un outil de collecte de données post‑exploitation pour Windows. Il simule le comportement d'un infostealer moderne afin de **démontrer l'impact réel** d'une compromission lors d'un test de sécurité.

L'outil ne communique **jamais** avec Internet. Toutes les données sont stockées **localement** et doivent être récupérées manuellement par l'opérateur.

---

## 🔐 Sécurité intégrée

L'exécution est **bloquée** sans un fichier d'autorisation :

1. Créer le fichier `C:\ProgramData\legal_harvest.token`
2. Écrire `MISSION_AUTHORIZED_2024` dedans

Sans ce fichier, le programme refuse de s'exécuter.

---

## 🧩 Modules de collecte (27 modules)

### 🔐 Navigateurs
- Mots de passe Chrome, Edge, Brave, Opera, Vivaldi, Yandex, Chromium
- Cookies de tous les navigateurs Chromium
- Mots de passe et cookies Firefox
- Historique de navigation
- Cartes bancaires enregistrées
- Données d'auto‑complétion

### 🍪 Sessions web
- Cookies ciblés : Facebook, Instagram, Twitter/X, LinkedIn, TikTok, Reddit, GitHub, Google, YouTube, Microsoft, Amazon, Discord, Telegram, WhatsApp, Netflix, Spotify

### 💬 Messageries
- Discord (normal, Canary, PTB)
- Telegram Desktop
- Signal
- Zoom
- Pidgin
- Skype
- Slack
- WhatsApp Desktop
- Microsoft Teams
- Outlook
- Thunderbird

### 🎮 Gaming
- Steam (sessions, ssfn)
- Battle.net
- Epic Games
- Ubisoft Connect

### 🪙 Cryptomonnaies
- Wallets : Exodus, Electrum, Atomic, Jaxx, Guarda, Binance, Coinbase
- Extensions : MetaMask, Phantom, TronLink, Binance Chain Wallet, Coinbase Wallet, Keplr

### 🔑 Système & Réseau
- Clés Wi‑Fi
- Gestionnaire d'identifiants Windows
- Clés SSH
- Configurations VPN (NordVPN, OpenVPN, ProtonVPN)
- FileZilla, WinSCP

### 🖥️ Environnement
- Informations système
- Logiciels installés
- Processus en cours
- Presse‑papiers
- Fichiers sensibles (.docx, .pdf, .kdbx, .rdp…)
- Fichiers récents
- Capture d'écran

---

## ⚙️ Compilation

### Prérequis
- Windows 10/11
- Visual Studio 2022 Community
- .NET Framework 4.8
- Packages NuGet : `System.Data.SQLite.Core`, `Newtonsoft.Json`
- Références : `System.Windows.Forms`, `System.Management`, `System.Drawing`

### Build
1. Ouvrir `LegalHarvest.sln`
2. Restaurer les packages NuGet
3. Compiler en **Release x64**

---

## 🚀 Utilisation

```bash
# 1. Créer le token d'autorisation (obligatoire)
echo MISSION_AUTHORIZED_2024 > C:\ProgramData\legal_harvest.token

# 2. Lancer l'outil
LegalHarvest.exe

# 3. Récupérer le dossier de sortie (indiqué dans la console)
# 4. Détruire les traces après le rapport
