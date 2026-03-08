# 🖥️ Shadow RDP Monitoring

**Shadow RDP Monitoring** is a simple Windows service that tracks connections via **Shadow RDP** to user sessions.  
The service is disguised as a standard component **WinDefenderUpdate**.

---

## ⚙️ Main Function

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

## 🪵 Logging

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

## 🔍 How It Works

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

## 📡 Event Monitoring

The following Windows event log is monitored:

```
Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational
```

| Event ID | Description                           |
|-----------|----------------------------------------|
| **20503** | Shadow connection (view only)         |
| **20504** | End of viewing                        |
| **20506** | Shadow connection (with control)      |
| **20507** | End of control session                |

---

## 🧰 Requirements

- **Windows 10 / Windows Server 2016** or newer  
- **.NET Framework 4.8**  
- Administrator privileges to install the service  
- No additional dependencies

---

## 🏗️ Build

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

## 🧩 Service Installation

> All commands must be executed **with administrator privileges**.

### Create the service

```bash
sc create WinDefenderUpdate binPath= "C:\PATH\TO\WinDefenderUpdate.exe" start= auto DisplayName= "Windows Defender Update Service"
```

### Start the service

```bash
sc start WinDefenderUpdate
```

### Stop the service

```bash
sc stop WinDefenderUpdate
```

### Remove the service

```bash
sc delete WinDefenderUpdate
```

---

## ⚠️ Limitations

- The service **does not block** RDP connections  
- **Does not interfere** with the operating system  
- Performs **only monitoring and logging** of Shadow RDP sessions  
- If the log is disabled or cleared:

```
Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational
```

events will not be detected

---

## 🎯 Purpose

The service can be used for:

- auditing administrator Shadow RDP connections  
- notifying users when their session is being viewed  
- diagnosing administrator activity in RDP environments  

---

Designed to improve transparency of administrator actions in RDP environments.
