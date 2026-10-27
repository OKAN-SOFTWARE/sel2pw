---
title: "fix(fixtures): stray closing brace from stripped if-block in BaseTest extractor"
labels: ["bug", "good first issue"]
---

`src/transformers/baseTestExtractor.ts` strips `driver.quit()` from Java `@AfterMethod` bodies, but when the Java source wraps it as

```java
if (driver != null) {
    driver.quit();
}
```

the closing `}` of the if-block isn't stripped and ends up as a dangling brace in the emitted `tests/fixtures.ts`, breaking the TypeScript compile with a stray-token error.

**Reproduce**

```bash
rm -rf /tmp/elias-check
npx @vijaypjavvadi/sel2pw@2.0.10 convert \
  examples/functional-equivalence-target \
  --out /tmp/elias-check
# Inspect line 15 of /tmp/elias-check/tests/fixtures.ts — orphan `}` present
```

**Suggested fix**

Extend `stripJavaDriverBoilerplate` in `src/transformers/bodyTransformer.ts` so that `if (driver != null) { driver.quit(); }` is matched and stripped as a single unit, or add a pairing pass that removes orphan `}` lines left behind by any stripped statement.

**Acceptance**

- New unit test fixture under `tests/transformers/` for the `if (driver != null) driver.quit();` shape.
- No new `tsc --noEmit` errors on the full `examples/validation-batch-v2/` benchmark.
- `examples/functional-equivalence-output/` no longer needs its MANUAL_EDITS.md Edit #1 after this ships.
