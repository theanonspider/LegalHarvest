# 🕷️ LegalHarvest V2 — Post‑Exploitation Framework

> ⚠️ **AVERTISSEMENT** — Usage exclusivement éducatif et défensif.  
> Toute utilisation non autorisée est **ILLÉGALE** et engage votre responsabilité.

---

## 📖 Pourquoi LegalHarvest V2 ?

**LegalHarvest V2** est un framework de post‑exploitation avec **61 modules** pour Windows (C# .NET Framework).

Il collecte tout ce qu'un attaquant pourrait voler : mots de passe, cookies, sessions, wallets, fichiers sensibles, et maintenant **SSH, RDP, VPN et API keys**.

---

## 🧩 Modules (61)

| Catégorie | Modules |
|-----------|---------|
| **Navigateurs** | Chrome, Edge, Brave, Firefox, Opera (passwords, cookies, history, credit cards) |
| **Sessions web** | Facebook, Instagram, Twitter, LinkedIn, Discord, Telegram, TikTok |
| **Wallets crypto** | MetaMask, Exodus, Electrum, Binance, Coinbase |
| **Fichiers** | SSH, RDP, VPN (NordVPN, OpenVPN, WireGuard, ProtonVPN) |
| **API keys** | AWS, Azure, GCP, GitHub, GitLab, `.env` |
| **Système** | Screenshots, keylogger, sysinfo, software, processes, sensitive files |
| **Post‑exploit** | Persistance, dump LSASS (Mimikatz), UAC Bypass, exfiltration, auto‑destruction, anti‑VM |

### ⬇️ Nouveaux modules

| Module | Fonction |
|--------|----------|
| `SSH Stealer` | Vol de clés privées (`id_rsa`), `known_hosts`, `config` |
| `RDP Stealer` | Vol de fichiers `.rdp`, certificats, MRU |
| `VPN Stealer` | Vol de configurations NordVPN, OpenVPN, WireGuard, ProtonVPN |
| `API Key Stealer` | Vol de clés AWS, Azure, GCP, GitHub, GitLab, `.env` |

---

## 🔐 Sécurité

```bash
echo "LEGALHARVEST_AUTHORIZED" > legalharvest.token
```

---

## ⚙️ Installation

**Prérequis :** Windows 10/11, .NET Framework 4.8, Visual Studio 2022.

```bash
git clone https://github.com/theanonspider/LegalHarvest-V2.git
cd LegalHarvest-V2
# Ouvrir dans Visual Studio → Build
echo "LEGALHARVEST_AUTHORIZED" > legalharvest.token
```

---

## 🚀 Exemples d’utilisation

```bash
LegalHarvest.exe
# Interface GUI → sélectionner les modules → EXECUTE
```

---

## 📄 Sortie

Rapport JSON dans `%TEMP%\LegalHarvest_<timestamp>\harvest.json`.

---

## ⚖️ Licence

Usage éducatif et défensif uniquement.

---

## 👤 Auteur

**@theanonspider** — Cybersécurité éthique. 🐺
