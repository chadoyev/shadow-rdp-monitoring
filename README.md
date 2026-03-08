Shadow RDP Monitoring

Shadow RDP Monitoring is a simple Windows service that tracks Shadow RDP
sessions connected to user sessions.

The service can be disguised as WinDefenderUpdate.

When an administrator connects to a user’s session through Shadow RDP
(view or control), the service records the event, writes it to a log
file, and notifies the user.

The project is written using .NET Framework 4.8 and runs as a standard
Windows Service.

------------------------------------------------------------------------

Service Function

The service monitors the Windows event log:

Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational

It reacts to events related to Shadow RDP connections.

The following events are monitored:

  Event ID   Description
  ---------- --------------------------------
  20503      Shadow connection (view only)
  20504      End of viewing
  20506      Shadow connection with control
  20507      End of control session

------------------------------------------------------------------------

What Happens During a Connection

When someone connects to a user session via Shadow RDP:

1.  A log entry is created
2.  A sound notification is played
3.  A message is sent to the user via msg.exe

Example notification:

Shadow RDP Connection

WITH CONTROL User: DOMAIN Computer: ADMIN-PC To: User1

------------------------------------------------------------------------

Logging

All events are written to:

C:.log

Example log entry:

[2025-03-08 12:14:03] Shadow CONTROL connected: DOMAINfrom ADMIN-PC to
User1

The log contains:

-   Event timestamp
-   Administrator account
-   Administrator computer
-   Target user session
-   Connection type (VIEW / CONTROL)

The folder is automatically created on the first run.

------------------------------------------------------------------------

How It Works

The service uses the Windows Event Log API:

System.Diagnostics.Eventing.Reader

Event subscription is implemented through EventLogWatcher.

When a new event appears in the Terminal Services log:

1.  The service receives the EventRecord
2.  Extracts event parameters
3.  Determines the Shadow connection type
4.  Writes information to the log
5.  Sends a notification to the user

------------------------------------------------------------------------

Requirements

-   Windows 10 / Windows Server 2016 or newer
-   .NET Framework 4.8
-   Administrator privileges to install the service

No additional dependencies are required.

------------------------------------------------------------------------

Build

The project can be built using standard .NET Framework tools.

Using MSBuild:

msbuild “Shadow RDP Monitoring.sln” /p:Configuration=Release

After compilation, the executable file will be created:

WinDefenderUpdate.exe

------------------------------------------------------------------------

Service Installation

Commands must be executed with administrator privileges.

Create the service:

sc create WinDefenderUpdate binPath= “C:\PATH\TO\WinDefenderUpdate.exe” start= auto DisplayName=
“Windows Defender Update Service”

Start the service:

sc start WinDefenderUpdate

Stop the service:

sc stop WinDefenderUpdate

Remove the service:

sc delete WinDefenderUpdate

------------------------------------------------------------------------

Limitations

-   The service does not block RDP connections
-   It does not interfere with the operating system
-   It only performs monitoring and logging of Shadow RDP sessions

If the log

Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational

is disabled or cleared by policies, the events will not be detected.

------------------------------------------------------------------------

Purpose

The project can be used for:

-   auditing administrator Shadow RDP connections
-   notifying users when their session is viewed
-   diagnosing administrator activity in RDP environments
