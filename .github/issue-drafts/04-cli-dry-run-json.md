---
title: "feat(cli): --dry-run should support --format=json to emit machine-readable mapping"
labels: ["enhancement", "help wanted"]
---

`sel2pw convert --dry-run` currently prints the Markdown mapping table to stdout. For CI pipelines and downstream tooling (CODEOWNERS auto-assignment on PRs, migration-cost dashboards, Jira ticket seeding) it would be useful to also emit the same content as structured JSON.

**Proposed**

```bash
# Default — Markdown preview (current behaviour)
sel2pw convert . --out ./pw --dry-run

# New — same data, JSON on stdout, no file writes
sel2pw convert . --out ./pw --dry-run --format=json
```

JSON shape should match the `files[]` and `projectNotes[]` arrays already defined in `src/reports/conversionResult.ts` so downstream consumers already parsing `conversion-result.json` get the same schema.

**Implementation note**

Shouldn't require touching the pipeline — the data already exists in the `buildConversionResult` return value. The CLI just picks Markdown vs JSON renderer at output time.

**Acceptance**

- New `--format` CLI option in `src/cli.ts` with values `markdown` (default) and `json`.
- JSON schema matches `conversion-result.json` so existing consumers work unchanged.
- Documentation update in README's CLI section.
