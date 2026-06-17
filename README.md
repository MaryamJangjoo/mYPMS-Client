```markdown
<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-purple.svg" />
  <img src="https://img.shields.io/badge/Blazor-WebAssembly-512BD4.svg" />
  <img src="https://img.shields.io/badge/Realtime-WebSocket-green.svg" />
  <img src="https://img.shields.io/badge/ALPR-Iranian%20Plate-blue.svg" />
  <img src="https://img.shields.io/badge/License-MIT-yellow.svg" />
</p>

<h1 align="center">🚗 mYPMS Client</h1>
<h3 align="center">Real-time Blazor Web Dashboard for Iranian ALPR System</h3>

---

## 📌 Overview

**mYPMS Client** is the frontend dashboard of the mYPMS ecosystem.  
Built with **Blazor WebAssembly**, it provides a real-time interface for monitoring Iranian license plate recognition results.

It connects to **mYPMS Server** via REST API and WebSocket to deliver live vehicle detection data.

---

## 🧠 System Architecture

```text
Browser (Blazor WASM)
        │
        ├── REST API (Search / History)
        ├── WebSocket (Live Plates)
        ▼
mYPMS Server (FastAPI + Nginx)
        │
        ├── ALPR Engine (Viseron + YOLOv8)
        ├── Database (PostgreSQL)
        └── RTSP Cameras
```

---

## ✨ Features

- ⚡ Real-time license plate detection dashboard
- 🎥 Live event stream via WebSocket
- 🔍 Search & filter historical records
- 📊 Analytics dashboard (traffic & detection stats)
- 🚨 Blacklist / whitelist alerts
- 📱 Responsive UI (W3.CSS)

---

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Running [mYPMS Server](https://github.com/MaryamJangjoo/mYPMS-Server)

---

### 1️⃣ Clone Repository

```bash
git clone https://github.com/MaryamJangjoo/mYPMS-Client.git
cd mYPMS-Client
```

---

### 2️⃣ Configure API Settings

Edit `appsettings.json`:

```json
{
  "Alpr": {
    "BaseUrl": "{ALPR_BASE_URL}",
    "MinConfidence": 0.4,
    "TimeoutSeconds": 10
  }
}
```

---

### 3️⃣ Run the Application

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run with HTTP (default)
dotnet run --urls "http://localhost:5000"

# Or run with HTTPS
dotnet run --urls "https://localhost:5001"
```

---

### 4️⃣ Open in Browser

```
http://localhost:5000
```

---

## 🔗 Real-time Flow

```text
Camera → Viseron → Server → WebSocket → Blazor UI → Live Dashboard
```

---

## 📁 Project Structure

```
mYPMS-Client/
├── 📂 Pages/              # Dashboard & UI pages
├── 📂 Shared/             # Reusable components
├── 📂 Services/           # API + WebSocket clients
├── 📂 Models/             # Data models
├── 📂 wwwroot/            # Static assets (css, js, lib)
├── 📂 Data/               # Database context
├── 📂 Controllers/        # MVC Controllers
├── 📂 Views/              # Razor Views
├── 📄 Program.cs          # App entry point
├── 📄 appsettings.json    # Configuration
├── 📄 mYPMS.csproj        # Project file
└── 📄 README.md           # This file
```

---

## 🔧 Common Issues

### ❌ WebSocket connection failed

- Check if **mYPMS Server** is running
- Ensure `WebSocketUrl` in `appsettings.json` is correct
- Use **HTTPS** for `wss://` connections

### ❌ API returns 404

- Verify `BaseUrl` in `appsettings.json` points to the correct server
- Check if FastAPI is running on port 8000

---

## 📄 License

MIT License — see [LICENSE](LICENSE) for details.


---

<p align="center">
  <strong>⭐ If you find this project useful, please give it a star! ⭐</strong>
```

---

