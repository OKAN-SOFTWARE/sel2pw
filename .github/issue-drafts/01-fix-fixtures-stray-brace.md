---
title: "fix(fixtures): stray closing brace left in tests/fixtures.ts when BaseTest has `if (driver != null) { driver.quit(); }`"
labels: ["bug", "good first issue"]
---

**What's wrong**

Many Selenium projects tear down the browser like this:

```java
@AfterMethod
public void tearDown() {
    if (driver != null) {
        driver.quit();
    }
}
```

sel2pw correctly removes `driver.quit()` (Playwright closes the browser for you), but it leaves the `if` block's closing brace behind. The generated `tests/fixtures.ts` then looks like this and does not compile:

```ts
    // ----- after each test -----
        // driver.quit() — handled by Playwright fixture
    }        // <-- orphan brace
  },
});
```

**How to reproduce**

```bash
npm install && npm run build
node dist/cli.js convert examples/functional-equivalence-target --out ./tmp-check
# open ./tmp-check/tests/fixtures.ts and look at the line after the comment
```

**Where to look**

- `src/transformers/baseTestExtractor.ts`: builds the fixture body from the Java `@AfterMethod`.
- `stripJavaDriverBoilerplate()` in `src/transformers/bodyTransformer.ts`: where driver-cleanup lines are removed.
- The `driver.quit()` rule in `src/transformers/apiMap.ts` (around line 52).

A good fix is to drop the whole `if (driver != null) { ... }` block when its only content is `driver.quit()`. Handle the one-line form `if (driver != null) driver.quit();` too.

**Done when**

- [ ] `./tmp-check/tests/fixtures.ts` from the steps above has no orphan brace.
- [ ] A unit test covers both shapes (with braces and the one-line form).
- [ ] `npm test` and `npm run lint` pass.

---

**New here?** Comment on this issue to claim it, and we'll assign it to you. Setup takes about five minutes: see [CONTRIBUTING.md](https://github.com/javvadivijayprasad/sel2pw/blob/main/CONTRIBUTING.md). Questions are welcome in the comments, and no question is too small.
