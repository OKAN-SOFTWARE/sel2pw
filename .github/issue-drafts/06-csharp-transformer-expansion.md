---
title: "feat(csharp): expand transformer bank with the six missing C# idioms (bool, IWebElement, Navigate, Assert.That, PascalCase, NUnit lifecycle)"
labels: ["enhancement", "help wanted", "csharp"]
---

C# frontend is scoped as experimental (see README and STATUS.md). The structural pipeline works — scanner identifies Page Objects and test fixtures, emitter writes a canonical Playwright scaffold — but the transformer bank is Java-primary, so emitted C# output currently reports 30 `tsc --noEmit` errors across a 3-file controlled sample (0 of 3 cleanly converted).

Honest write-up with the exact gaps: `examples/csharp-nunit-output/MANUAL_EDITS.md`.

**Six specific gaps to close**

Each is one or two lines added to a transformer file; each should land with a fixture:

1. `bool` return type → `boolean`
2. `IWebElement` field type → `Locator`
3. `driver.Navigate().GoToUrl(url)` → `await page.goto(url)`
4. `Assert.That(x, Is.EqualTo(y))` → `expect(x).toBe(y)` and the full `Is.*` constraint family
5. PascalCase method names on Page Objects → camelCase (`.LoginAs()` → `.loginAs()`)
6. `[SetUp]` / `[TearDown]` body driver boilerplate stripping (mirror the Java `@BeforeMethod` behaviour)

**Acceptance**

- Six new rule entries (or one combined C# transformer file) under `src/transformers/`
- Six corresponding fixtures under `tests/fixtures/csharp-nunit/`
- Re-running the controlled sample in `examples/csharp-nunit-target/` should drop the error count from 30 → ≤ 5
- `MANUAL_EDITS.md` in the output folder updated to reflect which gaps are now closed by the tool

**Context for first-time contributors**

Comfortable with C# Selenium? Pick 1 or 2 gaps, open a PR. Don't need to do all six in one go.
