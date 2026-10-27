---
title: "test: add regression fixture for Cucumber BDD .feature + step-defs → playwright-bdd"
labels: ["enhancement", "help wanted", "testing"]
---

sel2pw ships Cucumber BDD support via the `--bdd-mode preserve|flatten` CLI flag, but the regression test suite only covers the tiny fixture in `tests/fixtures/realworld/bdd-cucumber/`. Real projects have multiple feature files, multiple step-def classes, hooks, tags, and Scenario Outlines.

**Scope**

Add a second fixture that exercises a more realistic Cucumber+Selenium shape:

- 2-3 `.feature` files, at least one with a Scenario Outline + Examples
- 1 step-defs class with Given/When/Then annotations across all features
- 1 Page Object the steps consume
- 1 hooks class with `@Before` / `@After`

Verify both modes produce clean output:

- `--bdd-mode preserve` → emits playwright-bdd step-def TypeScript files alongside the preserved `.feature` files
- `--bdd-mode flatten` → emits one `test(...)` per Scenario, with step-def bodies inlined into the test body, and `Examples:` rows externalised to JSON

**Acceptance**

- New fixture folder under `tests/fixtures/realworld/bdd-cucumber-realistic/`
- Snapshot tests for both modes
- Document the expected output shape in a README inside the fixture folder
