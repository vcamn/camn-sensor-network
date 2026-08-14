# Raspberry Pi Git Development Setup

This guide documents the Git and GitHub configuration used on Raspberry Pi Zero edge development devices in the CAMN Sensor Network.

It covers:

* Installing Git
* Configuring GitHub SSH authentication
* Managing SSH keys
* Configuring SSH to use the correct GitHub identity
* Testing GitHub connectivity
* Using Git sparse-checkout with the CAMN monorepo

> **Scope:** This configuration is intended for Raspberry Pi development devices. Production deployment, repository access, credentials, and device provisioning may use a different process.

## Prerequisites

This guide assumes:

* Raspberry Pi Zero 2 W
* Debian GNU/Linux
* Network connectivity to GitHub
* A GitHub account with access to the CAMN repository
* Terminal access to the Raspberry Pi

## Install Git

Update the package list:

```bash
sudo apt update
```

Install Git:

```bash
sudo apt install git -y
```

Verify the installation:

```bash
git --version
```

## Configure GitHub SSH Authentication

Git operations should use SSH authentication rather than requiring GitHub credentials for each operation.

### Check for Existing SSH Keys

Inspect the SSH directory:

```bash
ls -al ~/.ssh
```

If `~/.ssh` does not exist, the user does not currently have SSH configuration in the default location.

If the directory exists, look for an existing public/private key pair, such as:

```text
id_ed25519
id_ed25519.pub
```

or:

```text
id_rpi_zero_vcamn
id_rpi_zero_vcamn.pub
```

An existing key can be used if appropriate. Otherwise, generate a dedicated key for the Raspberry Pi.

> **Important:** Never copy a private SSH key into the Git repository or commit it to source control.

## Generate a New SSH Key

Generate an Ed25519 SSH key, replacing the email address with the email associated with your GitHub account:

```bash
ssh-keygen -t ed25519 -C "your_email@example.com"
```

When prompted for the file in which to save the key:

```text
Enter file in which to save the key (/home/zero/.ssh/id_ed25519):
```

Press **Enter** to accept the default name, or specify a descriptive name for the CAMN development device:

```text
/home/zero/.ssh/id_rpi_zero_vcamn
```

Using a descriptive filename can make it easier to distinguish this key from other SSH identities on the device.

## Add the SSH Key to ssh-agent

Start `ssh-agent`:

```bash
eval "$(ssh-agent -s)"
```

The command should return an agent process ID similar to:

```text
Agent pid 59566
```

Add the private key:

```bash
ssh-add ~/.ssh/id_rpi_zero_vcamn
```

If the default `id_ed25519` filename was used instead:

```bash
ssh-add ~/.ssh/id_ed25519
```

## Add the Public Key to GitHub

Display the public key:

```bash
cat ~/.ssh/id_rpi_zero_vcamn.pub
```

Copy the complete output and add it to the appropriate GitHub account under:

**GitHub → Settings → SSH and GPG keys → New SSH key**

Only the `.pub` file should be added to GitHub.

## Configure SSH for GitHub

Create or edit the SSH configuration file:

```bash
nano ~/.ssh/config
```

> **Note:** `sudo` should not normally be used when editing files under the current user's `~/.ssh` directory. The SSH configuration and keys should belong to that user.

Add the following configuration:

```text
Host github.com
    HostName github.com
    User git
    IdentityFile ~/.ssh/id_rpi_zero_vcamn
    AddKeysToAgent yes
    IdentitiesOnly yes
```

This explicitly tells SSH which identity should be used when connecting to GitHub.

### Set SSH Permissions

Ensure the `.ssh` directory is only accessible by the current user:

```bash
chmod 700 ~/.ssh
```

Protect the SSH configuration:

```bash
chmod 600 ~/.ssh/config
```

Protect the private key:

```bash
chmod 600 ~/.ssh/id_rpi_zero_vcamn
```

The public key can use normal read permissions:

```bash
chmod 644 ~/.ssh/id_rpi_zero_vcamn.pub
```

If a different key filename was selected, substitute that filename in the commands above.

## Test GitHub SSH Connectivity

Test authentication:

```bash
ssh -T git@github.com
```

On the first connection, SSH may prompt you to verify GitHub's host key. Verify the fingerprint against GitHub's published SSH host key fingerprints before accepting it.

After successful authentication, GitHub should report that authentication succeeded while noting that GitHub does not provide shell access.

If VS Code was already running while the SSH configuration was changed, completely close and reopen VS Code. When using Remote SSH, disconnect and reconnect to the Raspberry Pi.

## Clone the CAMN Monorepo Using Sparse Checkout

The CAMN repository is a monorepo containing edge, cloud, application, database, and infrastructure components. An edge development device generally does not need every repository component in its working tree.

Git sparse-checkout allows the Raspberry Pi to populate only the required portion of the repository while retaining access to the repository's Git history.

### Clone the Repository

Clone the repository using partial clone and sparse-checkout:

```bash
git clone --filter=blob:none --sparse git@github.com:vcamn/camn-sensor-network.git
```

Enter the repository:
```bash
cd camn-sensor-network
```

The options serve different purposes:

* `--sparse` initializes sparse-checkout for the repository.
* `--filter=blob:none` avoids downloading Git blobs until their contents are required.

Together, these reduce unnecessary repository data and working-tree contents on the Raspberry Pi.

## Configure the Sparse Working Tree

Set the directory required by the Raspberry Pi:

```bash
git sparse-checkout set services/edge/sensor-node
```

Git will populate the working tree with the selected directory along with repository-level files Git includes as part of cone-mode sparse checkout.

Verify the current sparse-checkout configuration:

```bash
git sparse-checkout list
```

If the expected files are not present after configuring sparse-checkout, update the local repository:

```bash
git pull
```

## Change the Sparse-Checkout Directories

The `set` command defines the **complete set** of directories that should be present in the sparse working tree.

For example, if two directories are required:

```bash
git sparse-checkout set services/edge/sensor-node services/edge/shared
```

Running:

```bash
git sparse-checkout set services/edge/sensor-node
```

again would remove `services/edge/shared` from the sparse-checkout definition.

To add another directory without replacing the existing selection, Git also supports:

```bash
git sparse-checkout add <directory>
```

For example:

```bash
git sparse-checkout add services/edge/shared
```

Verify the resulting configuration:

```bash
git sparse-checkout list
```

## Disable Sparse Checkout

If the complete monorepo is later required on the Raspberry Pi:

```bash
git sparse-checkout disable
```

This restores the full working tree. Because the repository was cloned using partial clone, Git may download additional objects as they become necessary.

## Troubleshooting

### GitHub Authentication Fails

Verify which SSH key is being used:

```bash
ssh -vT git@github.com
```

Check that the expected identity appears in the diagnostic output.

Also verify:

```bash
ssh-add -l
```

If the key is missing, add it again:

```bash
ssh-add ~/.ssh/id_rpi_zero_vcamn
```

### Incorrect SSH Key Permissions

Verify permissions:

```bash
ls -al ~/.ssh
```

The private key and SSH configuration should not be accessible to other users.

Correct them if necessary:

```bash
chmod 700 ~/.ssh
chmod 600 ~/.ssh/config
chmod 600 ~/.ssh/id_rpi_zero_vcamn
```

### Repository Uses HTTPS Instead of SSH

Check the configured remote:

```bash
git remote -v
```

The GitHub remote should use the SSH form:

```text
git@github.com:vcamn/camn-sensor-network.git
```

If necessary, change it:

```bash
git remote set-url origin git@github.com:vcamn/camn-sensor-network.git
```

## Resources

* [GitHub — Generating a new SSH key and adding it to the ssh-agent](https://docs.github.com/en/authentication/connecting-to-github-with-ssh/generating-a-new-ssh-key-and-adding-it-to-the-ssh-agent)
* [GitHub — Testing your SSH connection](https://docs.github.com/en/authentication/connecting-to-github-with-ssh/testing-your-ssh-connection)
* [Git — sparse-checkout documentation](https://git-scm.com/docs/git-sparse-checkout)
* [Git — partial clone documentation](https://git-scm.com/docs/partial-clone)
