# Raspberry Pi VS Code Remote Development

This guide documents how to use Visual Studio Code **Remote - SSH** to develop CAMN Sensor Network edge software directly on a Raspberry Pi.

With Remote - SSH, VS Code runs on the development workstation while connecting to the Raspberry Pi over SSH. Files, terminals, development tools, and selected VS Code extensions can then operate directly within the Raspberry Pi environment.

> **Scope:** This workflow is intended for Raspberry Pi edge collector development. It is not a production deployment or fleet-management procedure.

## Prerequisites

Before configuring VS Code Remote - SSH:

* The Raspberry Pi must be accessible from the development workstation over SSH.
* OpenSSH must be installed and running on the Raspberry Pi.
* The Raspberry Pi IP address or hostname must be known.
* VS Code must be installed on the development workstation.

The Raspberry Pi SSH development setup is documented separately in the CAMN edge development documentation.

## Install Remote - SSH

Install the Microsoft **Remote - SSH** extension in VS Code.

Open the **Extensions** view and search for:

```text
Remote - SSH
```

Install the extension published by Microsoft.

## Add the Raspberry Pi as an SSH Host

Open **Remote Explorer** from the VS Code Activity Bar.

Under **SSH**, select **Add New SSH Host** (`+`).

Enter the SSH command used to connect to the Raspberry Pi:

```bash
ssh <username>@<raspberry-pi-ip-address>
```

For example:

```bash
ssh zero@192.168.1.2
```

VS Code will ask which SSH configuration file should store the host configuration.

On Windows, the user-specific SSH configuration is normally located under the current user's `.ssh` directory.

After adding the host, refresh **Remote Explorer** if necessary.

The Raspberry Pi should now appear under the available SSH hosts.

## Connect to the Raspberry Pi

From **Remote Explorer**, select the Raspberry Pi and choose **Connect in New Window**.

VS Code will open a new window associated with the remote Raspberry Pi.

If password authentication is being used, enter the Raspberry Pi user's password when prompted.

VS Code will initialize its remote environment on the Raspberry Pi. The first connection may take longer because the VS Code Server components must be installed on the remote device.

After the connection succeeds, VS Code indicates that the current window is connected to the Raspberry Pi.

## Open the Repository

From the remote VS Code window, select:

**File → Open Folder**

Select the repository or development directory on the Raspberry Pi.

For the CAMN monorepo, this will normally be the local sparse-checkout repository containing the edge collector source code.

Select **OK** to open the directory.

VS Code may display a **Workspace Trust** prompt.

Only trust the workspace or parent directory when the contents and repository source are known and trusted.

After opening the folder, the VS Code Explorer displays files from the Raspberry Pi filesystem rather than the development workstation.

## Use the Remote Terminal

Open a terminal using:

**Terminal → New Terminal**

The terminal runs directly on the Raspberry Pi.

For example:

```bash
hostname
```

or:

```bash
uname -a
```

will report information about the Raspberry Pi rather than the development workstation.

Git commands also execute against the repository located on the Raspberry Pi:

```bash
git status
```

```bash
git pull
```

The same applies to application builds, tests, scripts, and other development commands.

## Local and Remote VS Code Extensions

VS Code distinguishes between extensions running locally on the development workstation and extensions installed on the SSH host.

Extensions that need access to the remote filesystem, language runtime, debugger, or development environment may need to run on the Raspberry Pi.

Because Raspberry Pi Zero devices have limited CPU and memory resources, keep the number of extensions running remotely to the minimum required for CAMN edge development.

Unnecessary extensions should remain local or be disabled for the SSH environment.

## Raspberry Pi Remote Extension Configuration

The following built-in extensions have been disabled where they are not required by the CAMN Raspberry Pi development environment.

### TypeScript and JavaScript Language Features

Search the Extensions view for:

```text
@builtin TypeScript
```

Disable **TypeScript and JavaScript Language Features** for the remote environment when it is not required.

### Node Debug Auto-attach

Search for:

```text
@builtin node
```

Disable **Node Debug Auto-attach** for the remote environment when it is not required.

### GitHub Copilot Chat

GitHub Copilot Chat may attempt to install or run components on the remote SSH host.

To prevent unwanted remote installation, review the VS Code setting:

```text
remote.defaultExtensionsIfInstalledLocally
```

Remove **GitHub Copilot Chat** from this list if it should not automatically be installed on the Raspberry Pi.

The general principle for CAMN Raspberry Pi development is:

> Install an extension on the remote host only when its functionality actually needs to execute against the Raspberry Pi environment.

## Disconnect From the Raspberry Pi

When remote development is complete, close the remote VS Code window or use the VS Code remote connection controls to close the remote connection.

Closing the integrated terminal alone does not necessarily close the VS Code Remote - SSH session.

## Troubleshooting

### Verify SSH Outside VS Code

Before troubleshooting VS Code itself, verify that normal SSH connectivity works from the development workstation:

```bash
ssh <username>@<raspberry-pi-ip-address>
```

For example:

```bash
ssh zero@192.168.1.2
```

If normal SSH does not work, troubleshoot Raspberry Pi networking and SSH before troubleshooting VS Code Remote - SSH.

### Kill the VS Code Server on the Raspberry Pi

If Remote - SSH hangs, fails to initialize, or the VS Code Server enters an invalid state, open the VS Code Command Palette:

```text
Ctrl+Shift+P
```

Run:

```text
Remote-SSH: Kill VS Code Server on Host
```

Select the Raspberry Pi host and reconnect.

VS Code will restart or reinstall the necessary remote components when appropriate.

### Remove VS Code Server Files

If killing the remote server does not resolve the problem, SSH into the Raspberry Pi using a normal terminal:

```bash
ssh <username>@<raspberry-pi-ip-address>
```

Remove the VS Code Server installation:

```bash
rm -rf ~/.vscode-server
```

Reconnect using Remote - SSH.

VS Code will install a fresh copy of its remote server components.

> **Note:** Removing `~/.vscode-server` deletes the current user's VS Code remote-server installation and associated remote extension files. Use this as a troubleshooting step rather than routine maintenance.

### Disable Problematic Remote Extensions

If connections become unstable or the Raspberry Pi experiences excessive CPU or memory usage, review extensions installed on the SSH host.

Disable or uninstall remote extensions that are not required for edge collector development.

This is especially important on resource-constrained Raspberry Pi devices.

### SSH File Permission Problems

If VS Code reports SSH configuration or key permission errors, verify the permissions of the SSH directory and configuration files.

For example:

```bash
ls -al ~/.ssh
```

SSH keys and configuration should use appropriate restrictive permissions.

Refer to the Raspberry Pi SSH and Git development documentation for the CAMN SSH configuration.

## Development Workflow

The normal CAMN Raspberry Pi development workflow is:

1. Connect the development workstation directly to the Raspberry Pi or otherwise establish network connectivity.
2. Verify SSH connectivity to the Raspberry Pi.
3. Connect to the Raspberry Pi using VS Code Remote - SSH.
4. Open the CAMN edge collector repository on the Raspberry Pi.
5. Edit source files through the VS Code remote window.
6. Build, run, test, and debug software directly within the Raspberry Pi environment.
7. Use the integrated terminal for Git and Linux commands.
8. Commit and push source changes through the Raspberry Pi Git repository as appropriate.

This keeps the development environment close to the actual Raspberry Pi runtime while retaining the VS Code editing and development experience on the workstation.

## Resources

* [VS Code — Remote Development using SSH](https://code.visualstudio.com/docs/remote/ssh)
* [VS Code — Troubleshooting Remote Development](https://code.visualstudio.com/docs/remote/troubleshooting)
* [VS Code — Troubleshooting Hanging or Failing Connections](https://code.visualstudio.com/docs/remote/troubleshooting#_troubleshooting-hanging-or-failing-connections)
* [VS Code — Fixing SSH File Permission Errors](https://code.visualstudio.com/docs/remote/troubleshooting#_fixing-ssh-file-permission-errors)
* [Random Nerd Tutorials — Raspberry Pi Remote SSH with VS Code](https://randomnerdtutorials.com/raspberry-pi-remote-ssh-vs-code/)
* [Medium — Visual Studio Code Remote SSH/SFTP on Resource-Constrained Servers](https://medium.com/good-robot/use-visual-studio-code-remote-ssh-sftp-without-crashing-your-server-a1dc2ef0936d)
