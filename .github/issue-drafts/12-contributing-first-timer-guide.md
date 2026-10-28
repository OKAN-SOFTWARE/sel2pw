---
title: "docs: add 'Your first contribution' section to CONTRIBUTING.md"
labels: ["documentation", "good first issue"]
---

The current `CONTRIBUTING.md` covers the local dev workflow (`npm install`, `npm run build`, `npm test`, `npm run convert:sample`). That's useful for someone who's already decided to contribute, but it doesn't help a newcomer who clicked "good first issue" and is looking for an on-ramp.

**Proposed addition**

A new section titled **"Your first contribution"** at the TOP of CONTRIBUTING.md (above the current content), covering:

1. **How to pick an issue** — point at the `good first issue` label on the issue tracker; mention that any issue without "roadmap" or "discussion" labels is actively claimable.

2. **How to run the test suite locally** — minimum commands needed to go from fresh clone to a green `npm test`.

3. **How to add a transformer rule with its fixture** — the one-paragraph how-to:
   - Add the input Java in a new fixture under `tests/fixtures/`
   - Add the expected TS output alongside
   - Add the rule to `src/transformers/<appropriate>.ts`
   - Run `npm test -- -u` to snapshot
   - Verify the full benchmark via `node dist/cli.js convert examples/validation-batch-v2/sources/<any> --out /tmp/check` doesn't regress

4. **What reviewers will look for** — a short checklist:
   - Rule traced to at least one real repo failing in a specific way (not synthetic)
   - Fixture + unit test
   - CHANGELOG entry under the next unreleased version
   - `npm run lint` and `tsc --noEmit` clean

**Length**

Target under 50 added lines. Should feel like a quick-start, not a wall of text.

**Why it matters**

Lowers the activation energy for first-time contributors. Makes it clear how anyone can contribute.
