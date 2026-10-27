---
title: "research: independent manual audit of a 20-file sample from the batch-v2 outcome classifications"
labels: ["research", "help wanted"]
---

**Context**

The outcome classifications used in sel2pw's 15-repository benchmark (`cleanly converted` / `stubbed` / `mixed cleanup` / `skipped` / `failed`) are currently produced by sel2pw itself and recorded in `conversion-result.json` and `CONVERSION_REVIEW.md`. Peer-review feedback flagged that evaluating the tool using classifications the tool produces is partially circular. This is honestly acknowledged in `STATUS.md` under "Threats to validity" as a planned future-work item.

**Methodology sketch**

1. Pick 20 Java files at random (seeded, reproducible) from the 409-file corpus under `examples/validation-batch-v2/sources/`
2. For each file, hand-classify by inspecting source and converted output *without looking at CONVERSION_REVIEW.md*
3. Classification options: cleanly-converted / stubbed-with-migration-recipe / mixed-cleanup / skipped-correctly / mis-skipped / crashed
4. Compare with the tool's own classification
5. Report agreement rate overall and per bucket; discuss any disagreements

**Looking for**

A volunteer with practical Selenium + Playwright experience, approximately 3 hours of focused work. Output is a signed audit note added to `examples/validation-batch-v2/` as `independent-audit.md` with the 20 sampled files, the auditor's classification for each, and the agreement rate.

**Why it matters**

Addresses one of the main peer-review concerns from the sel2pw submission cycle. Would be cited in the next JOSS submission as independent validation of the outcome taxonomy. Credit via commit history + explicit mention in CHANGELOG and paper.md Acknowledgements.
