# Edge Integration Branching Strategy

## Purpose

The `edge-integration` branch provides a dedicated path for developing and validating edge collector changes on Raspberry Pi hardware before those changes are promoted to `main`.

This intentionally lightweight strategy supports the current workflow:

```text
Local development (WSL + uv)
    -> edge-integration
    -> Raspberry Pi Zero hardware integration testing
    -> pull request
    -> main
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
4. Merge the pull request after review.

Do not merge changes that failed hardware integration testing.

## Resynchronizing `edge-integration`

After the pull request is merged, update the local integration branch from `main`:

```bash
git switch main
git pull --ff-only origin main
git switch edge-integration
git merge --ff-only main
git push origin edge-integration
```

If `--ff-only` fails, the branches have diverged. Review the branch history and resolve the divergence locally rather than forcing an update from the Raspberry Pi.

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

