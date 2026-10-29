# Issue drafts

This folder holds pre-written issue bodies for the sel2pw issue tracker. They are tracked here so the project's active work queue is visible even without browsing the GitHub issue tracker, and so proposed issues can be reviewed as part of normal PRs before being opened.

**Each file is drafted but not yet opened** on the GitHub issue tracker. Maintainers open them in batches, pacing roughly 2-3 per week, to keep the tracker's activity signal honest rather than bursty.

## Current drafts

| # | File | Scope |
|---|---|---|
| 1 | `01-fix-fixtures-stray-brace.md` | Bug: dangling `}` from stripped `if (driver != null) driver.quit();` |
| 2 | `02-apimap-auto-first-on-multimatch.md` | Enhancement (help wanted): auto-add `.first()` on multi-match locators used in `.isVisible()` |
| 3 | `03-docs-readme-before-after-blocks.md` | Docs: 10 concrete Java-to-TS code-block pairs in the README |
| 4 | `04-cli-dry-run-json.md` | Enhancement: `--dry-run --format=json` for CI integration |
| 5 | `05-test-bdd-cucumber-fixture.md` | Testing: larger Cucumber BDD regression fixture |
| 6 | `06-csharp-transformer-expansion.md` | Enhancement: six missing C# transformer idioms |
| 7 | `07-java-lambda-arrow.md` | Enhancement: `param -> body` to `param => body` rewrite |
| 8 | `08-v3-fill-stubs.md` | Roadmap: v3.0 LLM `fill-stubs` subcommand (design discussion) |
| 9 | `09-research-manual-audit.md` | Research: independent 20-file sample audit of outcome classifications |
| 10 | `10-docs-migration-case-studies.md` | Docs: three real-repo migration case studies for the playbook |
| 11 | `11-ci-security-scanning.md` | Infrastructure: CodeQL security workflow |
| 12 | `12-contributing-first-timer-guide.md` | Docs: "Your first contribution" section in CONTRIBUTING.md |

## For contributors

If you want to work on one of these before it is opened as a formal issue, feel free to:

1. Open a short discussion issue referencing the draft
2. Or just open a pull request implementing it — the draft becomes the PR description

If you want to propose a new issue, draft it as a markdown file in this folder, open a PR, and discuss there. Keeps the proposal history clean.

## Format

Each file uses YAML frontmatter with `title` and `labels`, then the issue body in Markdown.
