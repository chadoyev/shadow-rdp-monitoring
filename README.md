<div align="center">

<h1>🖥️ Shadow RDP Monitoring</h1>

<p>
  <a href="#english">English</a> |
  <a href="#russian">Русский</a>
</p>

![Platform](https://img.shields.io/badge/platform-Windows-0078D6?logo=windows&logoColor=white)
![.NET](https://img.shields.io/badge/.NET_Framework-4.8-512BD4?logo=dotnet&logoColor=white)
![Language](https://img.shields.io/badge/language-C%23-239120?logo=csharp&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-active-brightgreen)

</div>

---

<!-- ============================================================ -->
<a name="english"></a>

<div align="center">

## 🇬🇧 English

</div>

### 📋 Table of Contents

- [Overview](#en-overview)
- [Main Function](#en-function)
- [Logging](#en-logging)
- [How It Works](#en-how)
- [Event Monitoring](#en-events)
- [Requirements](#en-requirements)
- [Build](#en-build)
- [Service Installation](#en-install)
- [Limitations](#en-limits)
- [Purpose](#en-purpose)

---

<a name="en-overview"></a>
### 🖥️ Overview

**Shadow RDP Monitoring** is a lightweight Windows service that tracks connections via **Shadow RDP** to user sessions.  
The service is disguised as a standard system component — **WinDefenderUpdate**.

---

<a name="en-function"></a>
### ⚙️ Main Function

When an administrator connects to a user session via Shadow RDP (view or control), the service:

1. Creates a log entry
2. Plays a sound notification
3. Sends a message to the user using **msg.exe**

📢 Example notification:

```
Shadow RDP Connection

WITH CONTROL
User: DOMAIN
Computer: ADMIN-PC
To: User1
```

---

<a name="en-logging"></a>
### 🪵 Logging

All events are written to the file:

```
C:\RDPMonitor\rdpmonitor.log
```

📄 Example entry:

```
[2025-03-08 12:14:03] Shadow CONTROL connected: DOMAIN\from ADMIN-PC to User1
```

Each entry contains:

- Event timestamp
- Administrator account
- Administrator computer
- Target user session
- Connection type (**VIEW** or **CONTROL**)

On first run, the service automatically creates the log folder if it does not exist.

---

<a name="en-how"></a>
### 🔍 How It Works

The service uses **Windows Event Log API**:

```csharp
System.Diagnostics.Eventing.Reader
```

Subscription is implemented via **EventLogWatcher**.  
When a new event appears in the `Terminal Services` log, the service:

1. Receives an `EventRecord`
2. Extracts event parameters
3. Determines the type of Shadow connection
4. Writes information to the log
5. Sends a notification to the user

---

<a name="en-events"></a>
### 📡 Event Monitoring

The following Windows event log is monitored:

```
Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational
```

| Event ID  | Description                        |
|-----------|------------------------------------|
| **20503** | Shadow connection (view only)      |
| **20504** | End of viewing                     |
| **20506** | Shadow connection (with control)   |
| **20507** | End of control session             |

---

<a name="en-requirements"></a>
### 🧰 Requirements

- **Windows 10 / Windows Server 2016** or newer
- **.NET Framework 4.8**
- Administrator privileges to install the service
- No additional dependencies

---

<a name="en-build"></a>
### 🏗️ Build

The project can be built using standard .NET Framework tools.

Example **MSBuild** command:

```bash
msbuild "Shadow RDP Monitoring.sln" /p:Configuration=Release
```

After compilation, the executable file will be created:

```
WinDefenderUpdate.exe
```

---

<a name="en-install"></a>
### 🧩 Service Installation

> All commands must be executed **with administrator privileges**.

**Create the service**

```bash
sc create WinDefenderUpdate binPath= "C:\PATH\TO\WinDefenderUpdate.exe" start= auto DisplayName= "Windows Defender Update Service"
```

**Start the service**

```bash
sc start WinDefenderUpdate
```

**Stop the service**

```bash
sc stop WinDefenderUpdate
```

**Remove the service**

```bash
sc delete WinDefenderUpdate
```

---

<a name="en-limits"></a>
### ⚠️ Limitations

- The service **does not block** RDP connections
- **Does not interfere** with the operating system
- Performs **only monitoring and logging** of Shadow RDP sessions
- If the following log is disabled or cleared, events will not be detected:

```
Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational
```

---

<a name="en-purpose"></a>
### 🎯 Purpose

The service can be used for:

- Auditing administrator Shadow RDP connections
- Notifying users when their session is being viewed
- Diagnosing administrator activity in RDP environments

> Designed to improve transparency of administrator actions in RDP environments.

---

<div align="right"><a href="#english">🇬🇧 English</a> | <a href="#russian">🇷🇺 Русский</a> | <a href="#">⬆️ Top</a></div>

<!-- ============================================================ -->
<a name="russian"></a>

<div align="center">

## 🇷🇺 Русский

</div>

### 📋 Содержание

- [Обзор](#ru-overview)
- [Основная функция](#ru-function)
- [Логирование](#ru-logging)
- [Принцип работы](#ru-how)
- [Мониторинг событий](#ru-events)
- [Требования](#ru-requirements)
- [Сборка](#ru-build)
- [Установка службы](#ru-install)
- [Ограничения](#ru-limits)
- [Назначение](#ru-purpose)

---

<a name="ru-overview"></a>
### 🖥️ Обзор

**Shadow RDP Monitoring** — лёгкая Windows-служба для отслеживания подключений через **Shadow RDP** к пользовательским сессиям.  
Служба маскируется под стандартный системный компонент — **WinDefenderUpdate**.

---

<a name="ru-function"></a>
### ⚙️ Основная функция

При подключении администратора к пользовательской сессии через Shadow RDP (просмотр или управление) служба:

1. Создаёт запись в журнале
2. Воспроизводит звуковое уведомление
3. Отправляет сообщение пользователю через **msg.exe**

📢 Пример уведомления:

```
Shadow RDP Connection

WITH CONTROL
User: DOMAIN
Computer: ADMIN-PC
To: User1
```

---

<a name="ru-logging"></a>
### 🪵 Логирование

Все события записываются в файл:

```
C:\RDPMonitor\rdpmonitor.log
```

📄 Пример записи:

```
[2025-03-08 12:14:03] Shadow CONTROL connected: DOMAIN\from ADMIN-PC to User1
```

Каждая запись содержит:

- Временну́ю метку события
- Учётную запись администратора
- Компьютер администратора
- Целевую пользовательскую сессию
- Тип подключения (**VIEW** или **CONTROL**)

При первом запуске служба автоматически создаёт папку для журнала, если она не существует.

---

<a name="ru-how"></a>
### 🔍 Принцип работы

Служба использует **Windows Event Log API**:

```csharp
System.Diagnostics.Eventing.Reader
```

Подписка реализована через **EventLogWatcher**.  
При появлении нового события в журнале `Terminal Services` служба:

1. Получает `EventRecord`
2. Извлекает параметры события
3. Определяет тип Shadow-подключения
4. Записывает информацию в журнал
5. Отправляет уведомление пользователю

---

<a name="ru-events"></a>
### 📡 Мониторинг событий

Отслеживается следующий журнал Windows:

```
Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational
```

| Event ID  | Описание                                 |
|-----------|------------------------------------------|
| **20503** | Shadow-подключение (только просмотр)     |
| **20504** | Завершение просмотра                     |
| **20506** | Shadow-подключение (с управлением)       |
| **20507** | Завершение сессии управления             |

---

<a name="ru-requirements"></a>
### 🧰 Требования

- **Windows 10 / Windows Server 2016** или новее
- **.NET Framework 4.8**
- Права администратора для установки службы
- Дополнительные зависимости отсутствуют

---

<a name="ru-build"></a>
### 🏗️ Сборка

Проект собирается стандартными инструментами .NET Framework.

Пример команды **MSBuild**:

```bash
msbuild "Shadow RDP Monitoring.sln" /p:Configuration=Release
```

После компиляции будет создан исполняемый файл:

```
WinDefenderUpdate.exe
```

---

<a name="ru-install"></a>
### 🧩 Установка службы

> Все команды выполнять **с правами администратора**.

**Создать службу**

```bash
sc create WinDefenderUpdate binPath= "C:\PATH\TO\WinDefenderUpdate.exe" start= auto DisplayName= "Windows Defender Update Service"
```

**Запустить службу**

```bash
sc start WinDefenderUpdate
```

**Остановить службу**

```bash
sc stop WinDefenderUpdate
```

**Удалить службу**

```bash
sc delete WinDefenderUpdate
```

---

<a name="ru-limits"></a>
### ⚠️ Ограничения

- Служба **не блокирует** RDP-подключения
- **Не вмешивается** в работу операционной системы
- Выполняет **только мониторинг и логирование** Shadow RDP сессий
- Если следующий журнал отключён или очищен, события не будут обнаружены:

```
Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational
```

---

<a name="ru-purpose"></a>
### 🎯 Назначение

Служба может применяться для:

- Аудита Shadow RDP подключений администраторов
- Уведомления пользователей о просмотре их сессий
- Диагностики активности администраторов в RDP-средах

> Разработана для повышения прозрачности действий администраторов в RDP-окружениях.

---

<div align="right"><a href="#english">🇬🇧 English</a> | <a href="#russian">🇷🇺 Русский</a> | <a href="#">⬆️ Top</a></div>
