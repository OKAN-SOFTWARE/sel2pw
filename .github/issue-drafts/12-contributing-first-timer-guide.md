---
title: "docs: add a \"Your first contribution\" section to CONTRIBUTING.md"
labels: ["documentation", "good first issue"]
---

**Why**

`CONTRIBUTING.md` explains the dev workflow, but someone who has never contributed to open source needs a gentler starting point. This issue is a good way to learn the project yourself while making it easier for the next person.

**What to do**

Add a short section called **"Your first contribution"** near the top of `CONTRIBUTING.md` that covers:

1. **Picking an issue.** Look for the `good first issue` label, and comment on the issue to claim it.
2. **Getting set up.** Fork, clone, `npm install`, `npm run build`, `npm test`. Write down anything that confused you while doing this yourself; that is exactly what the section should explain.
3. **Adding a conversion rule.** Rules live in `src/transformers/` (for example `apiMap.ts`). Tests live in `tests/transformers/`. Run `npm run convert:sample` to see the effect on the bundled sample.
4. **Before opening a pull request.** `npm test` and `npm run lint` pass, and there's a line in `CHANGELOG.md` under "Unreleased".

**Done when**

- [ ] The new section is under 50 lines and sits near the top of `CONTRIBUTING.md`.
- [ ] Every command in it has been run on a fresh clone and works.

---

**New here?** Comment on this issue to claim it, and we'll assign it to you. Setup takes about five minutes: see [CONTRIBUTING.md](https://github.com/javvadivijayprasad/sel2pw/blob/main/CONTRIBUTING.md). Questions are welcome in the comments, and no question is too small.
