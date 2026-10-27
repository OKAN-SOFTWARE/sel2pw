---
title: "feat(javaIdiomMap): Java lambda `param -> body` → TypeScript `param => body`"
labels: ["enhancement", "help wanted"]
---

Selenium 4 BiDi / CDP tests use Java lambdas heavily. Examples from `saucelabs-training/demo-java/selenium-junit5-examples`:

```java
.filter(req -> req.getMethod() == HttpMethod.POST)
network.onConsoleEntry(entry -> logs.add(entry.getText()))
(driver as HasLogEvents).onLogEvent(domMutation(e -> mutations.push(e.getElement())))
```

sel2pw currently passes these through unchanged, producing TypeScript syntax errors on every occurrence.

**Suggested fix**

Add a transformer rule in `src/transformers/javaIdiomMap.ts` that converts Java lambda arrow `->` to TypeScript arrow `=>`:

- Single-param: `param -> body` → `param => body`
- Multi-param: `(a, b) -> body` → `(a, b) => body`
- No-param: `() -> body` → `() => body`
- Block body: `param -> { ... }` → `param => { ... }`

Be careful not to rewrite the Pandoc-style `->` arrows in comments or string literals (apply the rule to code only, not inside `//` / `/* */` / string regions).

**Impact**

Observed on the saucelabs-training/demo-java/selenium-junit5-examples validation module during JOSS pre-submission scoping. Closes approximately 25 TypeScript errors on that single module.

**Related**

Issue #TBD covers Java method references (`Class::method`) which is the natural follow-up — but keep this issue scoped to the lambda arrow only.

**Acceptance**

- New rule in `src/transformers/javaIdiomMap.ts` with a dedicated fixture
- No false positives on string-literal `->` content (fixture must cover this)
- No TS regressions on the full `examples/validation-batch-v2/` benchmark
