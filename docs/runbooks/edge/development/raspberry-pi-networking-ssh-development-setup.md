# Raspberry Pi Zero Development Setup

This guide documents the development setup for Raspberry Pi Zero edge data collection devices used by the CAMN Sensor Network.

The configuration described here is intended for **local development, testing, and device configuration**. It is not the networking or SSH configuration intended for production site deployments.

The development environment uses a direct Ethernet connection between a developer workstation and the Raspberry Pi while allowing the Raspberry Pi to independently use Wi-Fi for Internet or network access.

This guide covers:

* Raspberry Pi hardware and operating system configuration
* Direct Ethernet networking between the Raspberry Pi and a development workstation
* Wi-Fi and Ethernet routing considerations
* OpenSSH server configuration on the Raspberry Pi
* Firewall configuration
* Connecting to the Raspberry Pi from a development workstation using SSH

## Development Device Configuration

The current Raspberry Pi development configuration is:

| Setting             | Value                         |
| ------------------- | ----------------------------- |
| Model               | Raspberry Pi Zero 2 W Rev 1.0 |
| Operating System    | Debian GNU/Linux 13 (trixie)  |
| Architecture        | 64-bit                        |
| Ethernet IP Address | `x.x.x.x`                     |
| Subnet Mask         | `255.255.255.0`               |
| Ethernet Gateway    | None                          |

## Development Workstation Ethernet Configuration

Configure the Ethernet adapter on the development workstation with the following settings:

| Setting          | Value           |
| ---------------- | --------------- |
| IP Address       | `x.x.x.x`       |
| Subnet Mask      | `255.255.255.0` |
| Ethernet Gateway | None            |

The Raspberry Pi and development workstation are assigned static addresses on the same `x.x.x.0/24` subnet, allowing them to communicate directly over Ethernet:

```text
Development Workstation                Raspberry Pi Zero 2 W
┌─────────────────────┐                ┌─────────────────────┐
│ Ethernet            │                │ Ethernet            │
│ x.x.x.x             │◄──────────────►│ x.x.x.x             │
│ 255.255.255.0       │                │ 255.255.255.0       │
│ No Gateway          │                │ No Gateway          │
└─────────────────────┘                └─────────────────────┘
                                                │
                                                │ Wi-Fi
                                                ▼
                                       ┌─────────────────────┐
                                       │ Network / Internet  │
                                       │ e.g. eduroam        │
                                       └─────────────────────┘
```

### Wi-Fi and Ethernet Routing

A default gateway is not required on the direct Ethernet connection because both devices are on the same subnet.

When the Raspberry Pi is also connected to Wi-Fi, such as `eduroam`, Wi-Fi can provide the Raspberry Pi's default route for Internet and external network access while the Ethernet interface remains dedicated to direct communication with the development workstation.

This separation avoids having the Ethernet and Wi-Fi interfaces compete to provide the Raspberry Pi's default route.

## Configure OpenSSH on the Raspberry Pi
1. Update the System

    Update the package list:

    `sudo apt update`

    Upgrade installed packages:

    `sudo apt full-upgrade`

    Reboot the Raspberry Pi if required after the upgrade:

    `sudo reboot`

2. Install and Enable the SSH Server

    Check whether the SSH service is already installed and running:

    `sudo systemctl status ssh`

    If OpenSSH Server is not installed, install it:

    `sudo apt install openssh-server`

    Start the SSH service:

    `sudo systemctl start ssh`

    Configure SSH to start automatically when the Raspberry Pi boots:

    `sudo systemctl enable ssh`

    The service can be verified again with:

    `sudo systemctl status ssh`

## Configure the Firewall

If UFW is being used on the Raspberry Pi, add a rule allowing SSH connections:

`sudo ufw limit ssh`

Using limit rather than simply allow permits SSH connections while also providing basic protection against repeated connection attempts.

Verify the firewall configuration:

`sudo ufw status`

**Note:** If UFW is not enabled, adding a rule does not automatically enable the firewall. Be careful when enabling a firewall remotely because an incorrect configuration can prevent subsequent SSH access.

## Connect to the Raspberry Pi Using SSH
1. Determine the Raspberry Pi IP Address

    On the Raspberry Pi, display the IP addresses currently assigned to the device:

    `hostname -I`

    For a Raspberry Pi using the direct Ethernet configuration documented above, the expected Ethernet address is (actual address masked):

    `x.x.x.2`

    If the Raspberry Pi is connected to multiple networks, `hostname -I` may display multiple addresses.

2. Verify the SSH Client

    On a Debian-based Linux client, check whether the OpenSSH client is installed:

    `apt list --installed openssh-client`

    To view all installed packages:

    `apt list --installed`

    If necessary, install the OpenSSH client:

    `sudo apt install openssh-client`

3. Establish the SSH Connection

    The general SSH command is:

    `ssh <username>@<ip-address>`

    For example, to connect to the Raspberry Pi at x.x.x.2 using the zero account:

    `ssh zero@x.x.x.2`

    The first connection may display a message asking whether the remote host's SSH fingerprint should be trusted. Verify the host and accept the fingerprint when appropriate.

    Enter the password for the Raspberry Pi user account when prompted.

    After authentication succeeds, the shell prompt will change to indicate that commands are now being executed on the Raspberry Pi.

    To terminate the SSH session:

    `exit`

## Verify Direct Ethernet Connectivity

Before troubleshooting SSH itself, verify that the client can reach the Raspberry Pi over the network.

From the client:

`ping x.x.x.2`

If the Raspberry Pi responds but SSH does not connect, check the SSH service on the Raspberry Pi:

`sudo systemctl status ssh`

Also verify the firewall configuration:

`sudo ufw status`

If the Raspberry Pi cannot be reached with ping, verify the Ethernet connection and confirm that the devices are configured on the same subnet (actual IP address masked):
```
Client:        x.x.x.1
Raspberry Pi:  x.x.x.2
Subnet Mask:   255.255.255.0
```


## Resources
- [Enable SSH on Debian Linux](https://pimylifeup.com/enable-ssh-debian/)
- [Debian FAQ — Keeping a Debian System Up to Date](https://www.debian.org/doc/manuals/debian-faq/uptodate.en.html)
- [SSH Setup Video](https://youtu.be/TZRGzLv57mc?si=pIEDVxd30jLF8-fI)