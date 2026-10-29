---
title: "feat(apiMap): auto-add .first() when .isDisplayed()/.isVisible() is called on a multi-match locator"
labels: ["enhancement", "help wanted"]
---

Selenium's `findElement(By.css(".inventory_item"))` silently returns the first match. Playwright's `page.locator(".inventory_item")` matches ALL elements and `.isVisible()` on an ambiguous locator throws a strict-mode violation at runtime. This is a documented fundamental semantic gap between the two frameworks.

The manual fix observed during functional-equivalence testing (`examples/functional-equivalence-output/MANUAL_EDITS.md` Edit #2) was to append `.first()` to the locator initialiser. We can detect this pattern at convert time.

**Pattern to detect**

A field declared via `@FindBy(css=...)` or `@FindBy(className=...)` whose value is referenced in a boolean-returning method body (`isVisible()`, `isEnabled()`, `isChecked()`, `isDisplayed()` call sites in the Java source).

**Suggested implementation**

In the Page Object emitter, when a field matches the above shape, emit the locator initialiser as `page.locator(...).first()` instead of `page.locator(...)`.

**Acceptance**

- New fixture under `tests/fixtures/realworld/` covering: `@FindBy(css=".item")` + `isDisplayed()` → emit `.first()`.
- Existing fixtures where the locator is used in `click()` / `fill()` and iteration should NOT get `.first()`.
- `examples/functional-equivalence-output/` no longer needs its MANUAL_EDITS.md Edit #2 after this ships.
