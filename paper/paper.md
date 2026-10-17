---
title: 'sel2pw: A rule-based migration tool from Selenium test suites to Playwright TypeScript scaffolds'
tags:
  - test automation
  - Selenium
  - Playwright
  - source-to-source transformation
  - abstract syntax tree
  - TypeScript
  - Java
  - test migration
  - software engineering
authors:
  - name: Vijay Prasad Javvadi
    orcid: 0009-0004-1192-6906
    affiliation: 1
affiliations:
  - name: Independent Researcher, Plainsboro, NJ, USA
    index: 1
date: 19 October 2026
bibliography: paper.bib
---

# Summary

Many teams own large Selenium test suites they want to move to Playwright. Playwright has better modern-browser coverage, waits for elements automatically, and runs faster [@selenium; @playwright]. The problem is that migrating a real suite by hand takes weeks. `sel2pw` does the boring part for you: it rewrites Page Objects, locators, assertions, lifecycle hooks, and project layout. It leaves behind a Playwright TypeScript scaffold and three plain-text reports — `CONVERSION_REVIEW.md`, `FILE_MAPPING.md`, and `conversion-result.json` — that say exactly what the tool did, what it skipped, and what still needs a human. The output is a scaffold, not a finished suite. Anything the tool cannot translate becomes either a typed stub with a migration recipe or a flagged review item, so nothing is silently wrong.

# Statement of need

If you own a Selenium suite and you want Playwright, you usually have three options and none of them are good.

You can rewrite it by hand, which costs months. You can hand it to a large language model, which rewrites things it does not understand and leaves no audit trail. Or you can re-record flows with a tool like Selenium IDE [@selenium-ide], which gives you fresh tests but throws away everything already in the repository.

`sel2pw` sits in the middle. Every transformation is a readable rewrite rule — you can open the source, see what it does, and reproduce it. The Java front-end uses the `java-parser` abstract syntax tree [@java-parser] built on Chevrotain [@chevrotain], with a regex extractor as a fallback when the AST parse fails on a single file. On the 15-repository benchmark the fallback rate is 0% — the AST handled every Page Object and test class the scanner handed it, over 105 AST-eligible files in the measured sub-sample. The regex path is defensive, not load-bearing, and its invocation rate is now reported in each run's `conversion-result.json` under the `parser` key. The C# front-end is regex and balanced-brace only; it is marked experimental in the README and the status document. The full rule catalogue is in [`docs/CONVERSION_PATTERNS.md`](../docs/CONVERSION_PATTERNS.md). An LLM fallback for one-off unknown constructs exists but was off for everything reported here, and its impact is not yet measured. When the LLM path is enabled, user source is sent to the configured provider; the shipping defaults disable it precisely because a migration tool should not silently share proprietary test code with a third party, and the documentation flags this before any key is accepted.

# Software description

`sel2pw` ships as the npm package `@vijaypjavvadi/sel2pw` and as a standalone Windows binary. The pipeline is small: a scanner reads the input tree, an intermediate representation stage pulls out Page Objects and tests, a transformer bank rewrites common Selenium idioms, and an emitter bank writes TypeScript.

The rule catalogue was built iteratively against real open-source Selenium repositories. The loop was simple: run `sel2pw` against a target repo, run `tsc --noEmit` on the output, look at the highest-count error, add or extend a rewrite rule that fixes it, add a fixture, re-run the whole batch to make sure nothing regressed, repeat. Every rule in the catalogue traces back to at least one real codebase failing in a specific way; synthetic rules written without a motivating repo are not accepted. The full rule list and its history live in [`docs/CONVERSION_PATTERNS.md`](../docs/CONVERSION_PATTERNS.md) and the git log.

The rewrites cover the usual suspects. `driver.findElement(By.id("x"))` becomes `page.locator('#x')`. `WebDriverWait...until(...)` is removed because Playwright locators wait on their own. `@BeforeMethod` turns into `test.beforeEach`. Hamcrest and AssertJ matcher chains become the equivalent `expect(...)` calls. Java enums, records, and exception types land in shared files under `types/` and `data/`. Infrastructure classes with no Playwright equivalent — driver managers, browser factories, Sauce Labs cloud wrappers — are detected and skipped, with the replacement noted in the review report. The output folder matches standard Playwright project layout: `pages/`, `tests/`, `types/`, `data/`, and a working `playwright.config.ts`.

# Benchmark

The repository ships 15 open-source Selenium codebases under `examples/validation-batch-v2/`, 409 Java files in total. The sources are pinned by git commit SHA so the benchmark reproduces exactly against the archived v2.0.9 tag:

| Tier | Repository | Pinned SHA | Framework | TS errors |
| ---: | --- | --- | --- | ---: |
| 1 | `cihat-kose/cucumber-jdbc-ui-db-learning-path` | `500449624f` | DB + UI BDD | 2 |
| 1 | `kush1107/SeleniumDatabaseTesting_JDBC` | `ce0f3e9fb0` | DB inline | 1 |
| 1 | `cihat-kose/cucumber-jdbc-ui-db-test-lab` | `9837a739ad` | DB + UI BDD | 86 |
| 2 | `TheBookKnight/java-selenium-cucumber-restassured` | `79964ba684` | UI + API BDD | 2 |
| 2 | `Chiranjeevi521/testng-cucumber-selenium-restassured` | `e180ecf472` | UI + API BDD | 41 |
| 2 | `pavleciric89/UI-API-test-automation-framework` | `fb3aad6ac9` | UI + API BDD | 48 |
| 3 | `anhtester/AutomationFrameworkSelenium` | `04b3c9bd58` | Production | 23 |
| 3 | `eliasnogueira/selenium-java-lean-test-architecture` | `a47281d605` | TestContainers | 12 |
| 3 | `rajatt95/MasterSeleniumFramework` | `9103307697` | Production | 26 |
| 4 | `gauravkarvir/cucumber_testng_java` | `e6c78869d7` | Appium + Sauce Labs | 25 |
| 4 | `maciejd/appium-selenium-testng-framework` | `47bece169b` | Appium + Web + API | 22 |
| 5 | `anhtester/AutomationFrameworkCucumberTestNG` | `80051566be` | BDD | 38 |

Of the 409 files:

- 150 (36.7%) are *cleanly converted*: `sel2pw` writes a Playwright TypeScript file for them and `tsc --noEmit` reports zero errors on that file.
- 152 (37.2%) are *stubbed*: the tool writes a typed stub with a migration recipe in the file header. The engineer updates each call site and deletes the stub.
- 96 (23.5%) are *mixed cleanup*: the file compiles with between one and twenty residual type errors, typically from custom helpers or uncommon idioms.
- 11 (2.7%) are *skipped*: infrastructure that Playwright handles natively.

There were no crashes and no missing outputs. That is a crash-safety claim, not a claim about compilation or behavioural equivalence. The exact definitions for these buckets live in [`STATUS.md`](../STATUS.md).

The C# path was evaluated separately against a purpose-built Selenium plus NUnit source (`examples/csharp-nunit-target/`, 3 files, mirroring the Java functional-equivalence source). sel2pw's scanner correctly identifies the Page Objects and test fixture, and the project emitter writes a canonical Playwright TypeScript scaffold. The transformer bank is Java-primary, so the emitted output currently reports 30 `tsc --noEmit` errors across the 3 files (0 of 3 cleanly converted). This is consistent with the "experimental" scoping of the C# front-end: the structural pipeline works, but the rewrite catalogue has not yet been extended to cover C#-specific idioms (`bool` to `boolean`, `IWebElement` to `Locator`, `driver.Navigate().GoToUrl` to `page.goto`, `Assert.That` to `expect`, PascalCase method names, NUnit lifecycle attribute bodies). Each gap is a bounded addition to a transformer file; see `examples/csharp-nunit-output/MANUAL_EDITS.md` for the full honest write-up.

Behavioural equivalence is tested separately. The folder `examples/functional-equivalence-target/` contains a small Selenium plus TestNG source. Its Playwright-converted output (`examples/functional-equivalence-output/`) runs 2 of 2 tests against `https://www.saucedemo.com` after two documented hand edits. Scaling this up is hard because many Selenium training apps are no longer online — the attempt against `eliasnogueira/selenium-java-lean-test-architecture` [@selenium-architecture] found its target site had been taken down since 2018. That is a problem no source-to-source tool can solve by itself.

# Comparison with related work

Record-and-playback tools such as Selenium IDE and Playwright Codegen [@selenium-ide; @playwright-codegen] let you capture new flows, but they do not translate existing source. Tools built on large language models can translate source, but their output is hard to audit and hard to reproduce. `sel2pw` is positioned alongside, not against, these approaches: the rule catalogue is open, every outcome is written to `conversion-result.json`, and every manual-review decision is recorded in `CONVERSION_REVIEW.md`. A reader can trace any line of emitted TypeScript back to the Java input and the rule that produced it.

# Threats to validity

The full list is in [`STATUS.md`](../STATUS.md). The main ones: the benchmark corpus is filtered by GitHub stars, which biases away from the genuinely legacy private suites `sel2pw` is really aimed at; the compiled-error count is a rough proxy for human cleanup effort, not a direct measurement; the outcome classifications are produced by the tool itself (an independent audit is planned); functional equivalence is shown on one target because historical training apps keep going offline; and the LLM fallback is shipped but not evaluated.

A note on reproducibility and security. The repository ships `package-lock.json` so `npm ci` against the archived v2.0.9 tag resolves every transitive dependency to the exact version used in the reported benchmark. `sel2pw` reads source files, parses them, and writes new files; it does not execute user code, which bounds the runtime attack surface to parser input handling, output path construction, and the optional LLM egress above. The threat model is written up alongside the vulnerability-reporting process in [`SECURITY.md`](../SECURITY.md).

# Acknowledgements

Thanks to the maintainers of `java-parser` and Chevrotain for the Java AST foundations, to the Playwright team at Microsoft for both the target framework and its documentation, and to Elias Nogueira for publishing `selenium-java-lean-test-architecture` as a reproducible benchmark. Sauce Labs' public `saucedemo.com` [@saucedemo] is the stable live target used for the functional-equivalence test.

# References
