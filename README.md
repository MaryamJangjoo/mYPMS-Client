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
RTSP Camera
     │
     ▼
Viseron
     │
     ▼
mYPMS Server (FastAPI)
     │
 ┌───┴────┐
 ▼        ▼
REST    WebSocket
 ▼        ▼
Blazor WebAssembly
     │
     ▼
Browser Dashboard
```

---

## ✨ Features

* ⚡ Real-time license plate detection dashboard
* 🎥 Live event stream via WebSocket
* 🔍 Search & filter historical records
* 📊 Analytics dashboard (traffic & detection stats)
* 🚨 Blacklist / whitelist alerts
* 📱 Responsive UI (W3.CSS)

---

## 🚀 Quick Start

### Prerequisites

* .NET 8 SDK
* Running mYPMS Server

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
dotnet restore
dotnet build

# HTTP
dotnet run --urls "http://localhost:5000"

# HTTPS
dotnet run --urls "https://localhost:5001"
```

---

### 4️⃣ Open in Browser

```text
http://localhost:5000
```

---

## 🐳 Docker

Build image:

```bash
docker build -t mypms-client .
```

Run container:

```bash
docker run -p 8080:80 mypms-client
```

Open:

```text
http://localhost:8080
```

---

## 🔗 Real-time Flow

```text
Camera → Viseron → mYPMS Server → WebSocket → Blazor UI → Live Dashboard
```

---

## 📁 Project Structure

```text
mYPMS-Client/
├── Pages/
├── Shared/
├── Services/
├── Models/
├── wwwroot/
├── Program.cs
├── appsettings.json
├── mYPMS.csproj
└── README.md
```

---

## 🔧 Common Issues

### ❌ WebSocket connection failed

* Check if mYPMS Server is running
* Ensure WebSocket URL is configured correctly
* Use HTTPS when connecting through `wss://`

### ❌ API returns 404

* Verify the configured API BaseUrl
* Ensure FastAPI server is running

---

## 📄 License

MIT License — see LICENSE for details.

---

<p align="center">
  <strong>⭐ If you find this project useful, please give it a star! ⭐</strong>
</p>
