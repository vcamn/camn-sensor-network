# Edge Integration Branching Strategy

## Purpose

The `edge-integration` branch provides a dedicated path for developing and validating edge collector changes on Raspberry Pi hardware before those changes are promoted to `main`.

This intentionally lightweight strategy supports the current workflow:

```text
Local development (WSL + uv)
    -> edge-integration
    -> Raspberry Pi Zero hardware integration testing
    -> pull request (squash merge)
    -> main
    -> realign edge-integration with main
```

## Branches

| Branch | Purpose |
| --- | --- |
| `main` | Stable monorepo code that has passed Raspberry Pi hardware integration testing. |
| `edge-integration` | Edge collector changes undergoing local development and Raspberry Pi validation. |

Although `edge-integration` is intended for the edge collector, it remains a branch of the entire monorepo. Changes on this branch should normally be limited to:

- `services/edge/`
- `docs/runbooks/edge/`
- Directly related repository configuration

## Working Conventions

- Do not develop or commit directly on the Raspberry Pi.
- Keep the Raspberry Pi working tree clean and use it as an integration-test target.
- Develop and run non-hardware tests locally before deploying to the Pi.
- Commit `pyproject.toml` and `uv.lock` together whenever dependencies change.
- Promote changes to `main` only after hardware integration testing succeeds.
- Use **Squash and merge** for pull requests from `edge-integration` into `main`.
- Explicitly realign `edge-integration` with `main` after each squash merge.
- Merge validated work promptly to minimize divergence between branches.
- Use short-lived feature branches only when concurrent or higher-risk work makes them useful.

## Initial Branch Setup

Create the integration branch from the latest `main`:

```bash
git switch main
git pull --ff-only origin main
git switch -c edge-integration
git push -u origin edge-integration
```

This setup is performed once. Afterward, both local development and the Raspberry Pi can track `origin/edge-integration`.

## Local Development Workflow

Start each development cycle from an up-to-date integration branch:

```bash
git switch edge-integration
git pull --ff-only origin edge-integration
```

Develop and test the edge collector from WSL:

```bash
cd services/edge/sensor-node
uv sync --locked
uv run pytest
```

Commit and push the completed changes:

```bash
git status
git add <changed-files>
git commit -m "Describe the edge collector change"
git push origin edge-integration
```

Use explicit paths with `git add` instead of staging unrelated monorepo changes.

## Raspberry Pi Integration Workflow

### 1. Confirm a clean working tree

```bash
git status --short
```

No output indicates that the working tree is clean. Resolve unexpected local changes before continuing.

### 2. Fetch and switch branches

```bash
git fetch origin
git switch edge-integration
git pull --ff-only origin edge-integration
```

### 3. Synchronize the Python environment

```bash
cd services/edge/sensor-node
uv sync --locked
```

`--locked` verifies that `uv.lock` agrees with `pyproject.toml` without modifying the lockfile. The command fails if the committed dependency metadata is inconsistent.

### 4. Run hardware integration tests

Validate the applicable sensor connections, Python entry points, log output, and systemd services. At minimum, verify:

```bash
sudo systemctl status camn-aggie-air.service --no-pager
sudo systemctl status camn-purple-air.service --no-pager
```

Review recent service logs when needed:

```bash
sudo journalctl -u camn-aggie-air.service -n 100 --no-pager
sudo journalctl -u camn-purple-air.service -n 100 --no-pager
```

The exact services tested should match the files changed in the development cycle.

## Sparse Checkout Compatibility

Sparse checkout controls which repository paths appear in the Raspberry Pi working tree. It does not prevent fetching, switching, or merging branches.

Verify the active sparse-checkout paths:

```bash
git sparse-checkout list
```

The Pi should include the edge collector and its operational documentation:

```bash
git sparse-checkout set services/edge docs/runbooks/edge
```

The sparse-checkout configuration normally remains active when switching between `main` and `edge-integration`. Avoid moving the selected directories differently between the two branches.

## Promoting Changes to `main`

After successful Raspberry Pi validation:

1. Push any remaining changes to `edge-integration`.
2. Open a pull request from `edge-integration` into `main`.
3. Confirm that the pull request contains only the intended edge collector and documentation changes.
4. Select **Squash and merge** after review.
5. Confirm that the pull request was merged successfully before realigning the integration branch.

Do not merge changes that failed hardware integration testing.

## Realigning `edge-integration` After a Squash Merge

Squash merging creates a new commit on `main` rather than preserving the individual commit IDs from `edge-integration`. The two branches therefore have different histories, and the following fast-forward merge will normally fail:

```bash
git merge --ff-only main
```

After confirming that the pull request was successfully squash merged and that `edge-integration` contains no additional unmerged work, explicitly realign it with `main`.

### 1. Update `main`

```bash
git switch main
git pull --ff-only origin main
```

### 2. Confirm a clean integration branch

```bash
git switch edge-integration
git status --short
```

No output indicates that the working tree is clean. Stop and review any unexpected changes before continuing.

### 3. Create a recovery branch

Create a temporary local pointer to the current integration branch before resetting it:

```bash
git branch "backup/edge-integration-before-realign-$(date +%Y%m%d-%H%M%S)"
```

The date suffix makes the recovery branch easy to identify later.

### 4. Realign and update the remote branch

```bash
git reset --hard main
git push --force-with-lease origin edge-integration
```

`git reset --hard main` deliberately makes the local `edge-integration` branch identical to `main`. `--force-with-lease` then updates the remote branch while refusing to overwrite unexpected remote commits.

Do not use plain `--force`. Do not perform this reset from the Raspberry Pi.

### 5. Verify the branch alignment

```bash
git rev-parse main
git rev-parse edge-integration
git rev-parse origin/edge-integration
```

All three commands should return the same commit hash. 

### 6. Realign and update the local branch directly on the Raspberry Pi Zero
```bash
git switch edge-integration # (if not aleady on the branch)
git fetch origin
git reset --hard origin/edge-integration
```

### 7. Verify alignment
```bash
git rev-parse HEAD
git rev-parse origin/edge-integration
```

Both hashes should match. Then continue:
```bash
cd services/edge/sensor-node
uv sync --locked
```

The next edge development cycle can now begin from the newly aligned `edge-integration` branch.

Keep the recovery branch until the alignment is verified. It can be deleted later with:

```bash
git branch --list 'backup/edge-integration-before-realign-*'
git branch -D backup/edge-integration-before-realign-YYYYMMDD-HHMMSS
```

Replace `YYYYMMDD-HHMMSS` with the timestamp shown by the list command. The forced local deletion is necessary because squash merging does not preserve the original integration-branch commit IDs. Delete the backup only after verifying the realignment and merged content.

This workflow requires the remote repository to permit force pushes to `edge-integration`. Keep force pushes disabled for `main`.

## Returning the Raspberry Pi to `main`

If the Pi should run only promoted code after testing:

```bash
git status --short
git fetch origin
git switch main
git pull --ff-only origin main

cd services/edge/sensor-node
uv sync --locked
```

Restart the affected services and verify their status after the branch switch.

## Troubleshooting

### Branch switching is blocked

Check for local modifications:

```bash
git status
```

Do not discard unexpected changes. Determine why the Pi working tree was modified before switching branches.

### `uv sync --locked` fails

The committed `uv.lock` may not match `pyproject.toml`. Update and test the lockfile in the local development environment, commit both files, and redeploy through `edge-integration`.

### Expected files are missing

Confirm that the paths are included in sparse checkout:

```bash
git sparse-checkout list
```

Then restore the expected paths if necessary:

```bash
git sparse-checkout set services/edge docs/runbooks/edge
```
