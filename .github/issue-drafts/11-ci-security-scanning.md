---
title: "chore(ci): add CodeQL + Snyk security scanning workflow"
labels: ["infrastructure", "security", "good first issue"]
---

`.github/workflows/` currently has `ci.yml` (test matrix) and `release.yml` (publish). We should add a `security.yml` workflow that runs on every PR and on a weekly schedule.

**Scope**

Two scanners, both free for public open-source repositories:

1. **GitHub CodeQL** — official GitHub static analysis. Catches a wide class of correctness and security defects in TypeScript. Zero config beyond adding the workflow.
2. **Snyk Open Source** (free tier) — dependency vulnerability scan. Flags known CVEs in direct and transitive dependencies.

**Deliverable**

A new `.github/workflows/security.yml` with two jobs: `codeql` and `snyk-oss`. Both report to the Security tab, both run on `pull_request`, `push` to `main`, and a weekly `schedule`.

**Starter template**

GitHub's own CodeQL template is a 15-line YAML. The Snyk Action has a ready-made example at `snyk/actions/node` on the marketplace.

**Acceptance**

- `.github/workflows/security.yml` exists and runs successfully on main
- Security tab shows CodeQL results
- Snyk badge in README (optional)
- README's "Development" section documents how to run the scans locally
