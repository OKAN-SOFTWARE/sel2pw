---
title: "docs: migration-cost case studies from the 409-file benchmark corpus"
labels: ["documentation", "help wanted"]
---

`docs/migration-playbook.md` was drafted earlier and covers the general sel2pw migration workflow. It would be considerably more useful if it included three concrete case studies drawn from the actual 15-repository benchmark corpus.

**Three case-study slots to fill**

1. **"Cleanly converted" repo** — e.g. `jdbc-pure` (1 TypeScript error after conversion). Walk through the full flow: input repo stats, exact `sel2pw convert` command, output stats, the single residual TS error and how to fix it in minutes.

2. **"Mixed cleanup" repo** — e.g. `eliasnogueira-lean` (12 residual TS errors). More involved: categorise the errors, describe which are mechanical fixes (import additions, type annotations) vs. architectural (missing fixture, DI pattern mismatch). Estimate the hand-cleanup time.

3. **"Stubs heavy" repo** — e.g. `anhtester-selenium` (23 residual TS errors, several stubs in `_legacy-stubs/`). Show how to decide which stubs to migrate first, which to delete, which to rewrite from scratch.

**Format for each**

- Repo name + pinned commit SHA (reproducibility)
- Input stats: file count, LoC, Selenium version, test framework
- Conversion command used
- Output stats from `conversion-result.json`
- Breakdown of residual issues
- Narrative of the human cleanup pass with estimated time

**Length**

Target ~400 words per case study. Add to `docs/migration-playbook.md` as a new "Case studies" section.

**Why it matters**

Converts abstract "sel2pw does X" into "here's exactly what happened on this real repo", which is the single most persuasive doc content for teams evaluating the tool.
