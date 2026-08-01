# 🕷️ LegalHarvest — Post‑Exploitation Tool

> ⚠️ **AVERTISSEMENT** — Usage exclusivement éducatif et défensif.  
> Toute utilisation non autorisée est **ILLÉGALE** et engage votre responsabilité.

---

## 📖 Pourquoi LegalHarvest ?

**LegalHarvest** est un outil de post‑exploitation pour Windows (C# .NET Framework).  
Il collecte tout ce qu’un attaquant pourrait voler : mots de passe, cookies, sessions, wallets, informations système, etc.

Il est conçu pour les **tests d’intrusion autorisés** et les **exercices Red Team**.

---

## 🧩 Modules (39)

| Catégorie | Modules |
|-----------|---------|
| **Navigateurs** | Chrome, Edge, Brave, Firefox, Opera (passwords, cookies) |
| **Sessions web** | Facebook, Instagram, Twitter, LinkedIn, Discord, Telegram |
| **Wallets crypto** | MetaMask, Exodus, Electrum, Binance |
| **Système** | Screenshots, keylogger, sysinfo, software, processes |
| **Post‑exploit** | Persistance, exfiltration, auto‑destruction |

---

## 🔐 Sécurité

Un token est obligatoire pour exécuter l'outil :

```bash
echo "LEGALHARVEST_AUTHORIZED" > legalharvest.token
```

---

## ⚙️ Installation

**Prérequis :** Windows 10/11, .NET Framework 4.8, Visual Studio 2022.

```bash
git clone https://github.com/theanonspider/LegalHarvest.git
cd LegalHarvest
# Ouvrir dans Visual Studio → Build
echo "LEGALHARVEST_AUTHORIZED" > legalharvest.token
```

---

## 🚀 Exemples d’utilisation

```bash
# Interface graphique
LegalHarvest.exe

# Sélectionner les modules → cliquer sur EXECUTE
# Les résultats sont dans %TEMP%\LegalHarvest_<timestamp>
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
