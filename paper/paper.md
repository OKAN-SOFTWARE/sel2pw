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
date: 27 October 2026
bibliography: paper.bib
---

# Summary

Many organizations own large automated browser-test suites written in Java with Selenium WebDriver [@selenium] and are moving to Playwright [@playwright] for its broader modern-browser coverage, auto-waiting locators, and faster execution. Hand-migrating a real suite costs weeks of engineering time. `sel2pw` automates the mechanical part of that migration — Page Objects, locators, assertions, lifecycle hooks, driver-manager infrastructure, and the Playwright project layout — and ships what it cannot translate as either a typed stub with a migration recipe or a flagged review item. The output is a Playwright TypeScript scaffold accompanied by three plain-text reports — `CONVERSION_REVIEW.md`, `FILE_MAPPING.md`, and `conversion-result.json` — that record what the tool did, what it skipped, and what still needs a human. Every rewrite rule is a readable transform the migration engineer can audit, extend, or disable; nothing is silently rewritten.

# Statement of need

Organizations with legacy Selenium estates face three unattractive migration paths. Hand-rewriting takes months per suite. Fully automated large-language-model (LLM) rewrites change behaviour without warning and leave no audit trail. Re-recording user flows with capture tools throws away everything already in the repository. `sel2pw` fills the gap between these: it is a deterministic, rule-based source-to-source translator that produces a reviewable scaffold rather than claiming a finished migration.

`sel2pw` is used in the author's ongoing study of mechanical refactoring limits in test-suite migration. The 15-repository, 409-file benchmark described below provides the empirical basis for distinguishing which Selenium patterns yield to deterministic rewrite rules, which land as stubs requiring human migration recipes, and which require runtime inspection that a source-code-only tool cannot provide. The outcome taxonomy and threats-to-validity analysis published alongside this paper form the first iteration of that study; the controlled functional-equivalence artifact (`examples/functional-equivalence-output/`) is a second strand. The target audience is quality-engineering teams and test-automation researchers who need to move a Selenium estate to Playwright with full visibility into what the tool did on every file.

# State of the field

Several categories of tool exist adjacent to the Selenium-to-Playwright migration problem.

*Record-and-playback tools* — Selenium IDE [@selenium-ide] and Playwright Codegen [@playwright-codegen] — capture fresh user flows in the browser and emit test code. They do not translate existing source. Teams with a decade of accumulated Page Objects, custom helpers, and TestNG lifecycle plumbing cannot use these to preserve prior test coverage; they would have to re-author every scenario.

*LLM-based migration prompts and agents* — various GPT-style and Claude-style assistants, Cursor-driven refactors, and custom pipelines — can translate source and are increasingly used in industry. Their output is non-deterministic, varies between runs and providers, and is hard to audit at scale. For regulated or safety-critical contexts their silent behaviour change is unacceptable; for general use the lack of reproducibility makes them unsuitable as a reproducible research instrument.

*Hand-written migration guides* and *refactoring textbooks* [@fowler-refactoring; @meszaros-xunit] cover the principles but leave the mechanical rewriting to the engineer.

To the author's knowledge no deterministic, open-source, rule-based source-to-source Selenium-to-Playwright translator exists at the scale targeted by `sel2pw` (240+ documented rewrite rules, 15-repository public benchmark, hybrid AST + regex frontend). We built rather than contributing to Selenium IDE or Playwright Codegen because those are capture tools and solving the migration problem is categorically different work. We built rather than contributing to an LLM-based tool because the determinism and auditability properties are architectural, not add-on features.

# Software design

`sel2pw` is distributed as the npm package `@vijaypjavvadi/sel2pw` and as a standalone Windows binary. The pipeline has four stages: a scanner reads the input tree; an intermediate-representation stage pulls out Page Objects, test classes, enums, exceptions, and infrastructure classes; a transformer bank rewrites Selenium idioms; and an emitter bank writes TypeScript. The design reflects four deliberate trade-offs.

**Deterministic rules over LLM translation.** Every transformation is a human-written rewrite rule committed under `src/transformers/`. The same input produces the same output on every run — auditable, debuggable, reproducible, properties an LLM-only pipeline cannot provide. The cost is that constructs with no matching rule become explicit stubs rather than silent misinterpretations. An LLM fallback for one-off unknown constructs is shipped but off by default; its empirical contribution is left for a follow-up release.

**Hybrid AST + regex extraction.** The Java front-end uses the `java-parser` abstract syntax tree [@java-parser] built on Chevrotain [@chevrotain], with a regex extractor as a defensive per-file fallback. On the benchmark the fallback rate is 0% across the 105-file sub-sample on which the counter has been measured, so the regex path is defensive, not load-bearing. Each run reports the measured rate in `conversion-result.json`. The C# front-end is regex and balanced-brace only and is scoped as experimental.

**Scaffold rather than finished output, with canonical Playwright layout.** Every emitted file is paired with explicit entries in `CONVERSION_REVIEW.md` and `FILE_MAPPING.md`. Constructs with no Playwright equivalent (driver managers, browser factories, Sauce Labs cloud wrappers) are detected and skipped rather than translated into something that looks like but is not Playwright code; the replacement path is noted inline. The output folder matches standard Playwright shape — `pages/`, `tests/`, `types/`, `data/`, `playwright.config.ts` — so projects drop into existing Playwright toolchains unchanged.

**Rule catalogue grown from real failures.** The transformer catalogue was built iteratively: convert a real OSS repository, inspect `tsc --noEmit` errors, extend a rewrite rule, add a fixture, re-run the benchmark, repeat. Every rule traces back to at least one real repository failing in a specific way; synthetic rules without a motivating repo are not accepted.

# Research impact statement

The sel2pw repository has been under active public development since April 2026 (`0923c93` is the first commit). The repository ships 15 open-source Selenium codebases under `examples/validation-batch-v2/sources/`, 409 Java files in total, each pinned by git commit SHA so the benchmark reproduces against the archived v2.0.10 tag [@zenodo-sel2pw].

| Tier | Slug | Pinned SHA | Framework | TS errors |
| ---: | --- | --- | --- | ---: |
| 1 | `jdbc-learning` | `500449624f` | DB + UI BDD | 2 |
| 1 | `jdbc-pure` | `ce0f3e9fb0` | DB inline | 1 |
| 1 | `jdbc-test-lab` | `9837a739ad` | DB + UI BDD | 86 |
| 2 | `ui-api-bookknight` | `79964ba684` | UI + API BDD | 2 |
| 2 | `ui-api-chiranjeevi` | `e180ecf472` | UI + API BDD | 41 |
| 2 | `ui-api-pavleciric` | `fb3aad6ac9` | UI + API BDD | 48 |
| 3 | `anhtester-selenium` | `04b3c9bd58` | Production | 23 |
| 3 | `eliasnogueira-lean` | `a47281d605` | TestContainers | 12 |
| 3 | `master-selenium` | `9103307697` | Production | 26 |
| 4 | `appium-gauravkarvir` | `e6c78869d7` | Appium + Sauce Labs | 25 |
| 4 | `appium-maciejd` | `47bece169b` | Appium + Web + API | 22 |
| 5 | `bdd-anhtester` | `80051566be` | BDD | 38 |

Of the 409 files: 150 (36.7%) are *cleanly converted* — `sel2pw` emits a TypeScript file and `tsc --noEmit` reports zero errors on that file; 152 (37.2%) are *stubbed* with a documented migration recipe in the file header; 96 (23.5%) are *mixed cleanup* and compile with between one and twenty residual type errors; and 11 (2.7%) are *skipped* as infrastructure Playwright replaces natively. There were no crashes or missing outputs. The exact definitions live in [`STATUS.md`](../STATUS.md); "no failed conversions" is a crash-safety claim, not a claim about compilation or behavioural equivalence.

Behavioural equivalence is tested separately against a controlled target. The folder `examples/functional-equivalence-target/` contains a purpose-built Selenium plus TestNG source. Its Playwright-converted output (`examples/functional-equivalence-output/`) runs 2 of 2 tests against `https://www.saucedemo.com` [@saucedemo] after two documented hand edits (one trivial sel2pw bug and one `page.locator('.inventory_item').first()` disambiguation). Scaling this up is bounded by target-site drift — the attempt against `eliasnogueira/selenium-java-lean-test-architecture` [@selenium-architecture] found its target site had been taken offline since 2018 — which is a problem no source-to-source tool can solve by itself.

The C# path was evaluated against a 3-file Selenium plus NUnit source. The scanner identifies the Page Objects and test fixture and the emitter writes a canonical scaffold, but the output reports 30 `tsc --noEmit` errors (0 of 3 cleanly converted), consistent with the experimental scoping — the structural pipeline works but the C# rewrite catalogue is not yet complete. The honest write-up is in `examples/csharp-nunit-output/MANUAL_EDITS.md`.

As distribution signals, the npm package has been downloaded approximately 2,500 times across tagged releases and the Visual Studio Marketplace extension has 56 installs; both include CI pulls and are reported as engagement rather than distinct-user adoption. External research citations have not yet accrued; the primary research use is the author's own benchmarking study above, with a follow-up study on LLM-assisted stub completion planned for v3.0.

# Threats to validity

The full list is in [`STATUS.md`](../STATUS.md) under "Threats to validity". The main ones: the benchmark corpus is filtered by GitHub stars and recent activity, which biases away from the genuinely legacy private suites `sel2pw` is really aimed at; the compiled-error count is a rough proxy for human cleanup effort, not a direct measurement; the outcome classifications are produced by the tool itself and an independent sample audit is planned future work; functional equivalence is demonstrated on one controlled target because historical training apps keep going offline; the LLM fallback is shipped but not empirically evaluated; and a single-reviewer classification should be independently replicated. The repository ships `package-lock.json` so `npm ci` against the archived v2.0.10 tag resolves every transitive dependency to the exact version used in the reported benchmark. `sel2pw` reads source files, parses them, and writes new files; it does not execute user code, which bounds the runtime attack surface to parser input handling, output path construction, and the optional LLM egress noted above. The threat model and vulnerability-reporting process are in [`SECURITY.md`](../SECURITY.md).

# AI usage disclosure

Generative AI tools were used during the development of sel2pw and the preparation of this manuscript, in line with the Journal of Open Source Software's AI Usage Policy.

**Tools used:** Anthropic Claude (Sonnet 4.7 and Opus 4.7, used interactively between April and October 2026).

**Where:** code review and incremental refactoring of individual transformer rules under `src/` (candidate regex patterns against documented failing cases, test fixture drafts); drafting and iterative revision of this manuscript's prose; drafting of the "Outcome definitions" and "Threats to validity" sections in `STATUS.md`, release notes in `CHANGELOG.md`, and incremental `README.md` edits.

**Confirmation of review:** All generated code was built, linted, tested against the benchmark suite, and in several cases rejected or rewritten before commit. All drafted prose was reviewed and revised by the author line by line before commit; factual claims were verified against the repository artifacts. The core design decisions — deterministic rule-based architecture, hybrid AST + regex strategy, scaffold-not-auto-migration positioning, honest reframe of outcome definitions after an earlier peer-review cycle, scoping of C# as experimental, deferral of LLM empirical evaluation — are the author's own. The author takes full responsibility for all AI-assisted outputs. Generative AI was not used for any communication with Journal of Open Source Software editors or reviewers.

# Acknowledgements

Thanks to the maintainers of `java-parser` and Chevrotain for the Java AST foundations, to the Playwright team at Microsoft for both the target framework and its documentation, and to Elias Nogueira for publishing `selenium-java-lean-test-architecture` as a reproducible benchmark. Sauce Labs' public `saucedemo.com` is the stable live target used for the functional-equivalence test. No financial support was received for this work.

# References
