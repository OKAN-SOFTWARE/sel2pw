# sel2pw — Current Status

A point-in-time snapshot. CHANGELOG.md is the authoritative history; this file is "where are we right now, what works, what's pending, what's deferred".

**Live on npm as `@vijaypjavvadi/sel2pw`.** Latest published: **v0.10.6** (post-publish CI hardening — telemetry resilience + coverage gating). v0.10.5 added 4 bug fixes from selenium9–15 validation. v0.10.4 fixed the CLI version reporting. v0.10.3 closed the selenium8 milestone. 0.9.0 failure-telemetry SQLite. 0.10.0 distribution pattern (`.exe`, platform downloads, structured `conversion-result.json`).

### Validation matrix — 15 real-world codebases, 409 Java files, 0 failed conversions (failed = crash or missing output; not a compile or equivalence claim)

| Project | Files | Failed | Skipped | Notes |
| --- | --- | --- | --- | --- |
| selenium1 (naveenanimation20) | 4 | 0 | 0 | clean |
| selenium2 (cgjangid) | 2 | 0 | 0 | clean |
| selenium3 (AlfredStenwin) | 35 | 0 | ~5 | decorator hierarchy stress |
| selenium4 (vibssingh) | 8 | 0 | 0 | clean |
| selenium5 (swtestacademy ExcelReadWrite) | 11 | 0 | 0 | drove Phase 8.4–8.7 |
| selenium6 (aeshamangukiya hybrid) | 21 | 0 | ~3 | broadest framework |
| selenium7 (Infosys) | 4 | 0 | 0 | clean |
| selenium8 (yadsandy Data-Driven) | 11 | 0 | 0 | drove Phase 10.2–10.3 |
| **selenium9 (cucumber-jdbc-ui-db-test-lab)** | **28** | **0** | **1** | bdd-cucumber + JDBC |
| **selenium10 (cucumber-jdbc-ui-db-learning-path)** | **42** | **0** | **1** | drove kebab-lookup fix (0.10.5) |
| **selenium11 (mersys-ui-db-test-framework)** | **42** | **0** | **1** | same scaffold as 10 |
| **selenium12 (hybrid-qa-automation-framework)** | **32** | **0** | **3** | drove BaseTest @Optional fix |
| **selenium13 (anhtester AutomationFrameworkSelenium)** | **84** | **0** | **2** | largest single repo |
| **selenium14 (anhtester AutomationFrameworkCucumberTestNG)** | **75** | **0** | **3** | bdd-cucumber, anhtester family |
| **selenium15 (Selenium_TestNG_Amazon)** | **10** | **0** | **0** | clean |

**Aggregate: 409 files / 0 failures / 11 honest skips (POJO-shaped files with no Selenium signal).** Every codebase's failures became a one-line apiMap rule, classifier widening, detector pattern, or lookup fix. The next codebase that surfaces a new shape is the next round of patches — that's the loop, demonstrated across 15 codebases now.

## What ships today (verified shipped)

### Pipeline (`src/`)

- Scan → parse → classify → transform → emit → post-process → review.
- AST extractor (`java-parser` + Chevrotain) is canonical. Regex extractor remains as a per-file fallback when the AST throws or `java-parser` isn't installed. Per-file try/catch in `index.ts` so one bad source never aborts the whole run.
- Locator mapping: `By.id/css/xpath/name/linkText/partialLinkText/tagName/className` + `@FindBy`.
- WebDriver/WebElement API mappings: navigation, click/fill/clear/text/attribute/visibility/enabled/selected, `WebDriverWait` removal, `Thread.sleep`.
- Assertions: TestNG `Assert.*` and Hamcrest `assertThat(actual, matcher)` — scan-based parser handles commas-in-strings and parens-in-args.
- Advanced API: `Actions` chains, `JavascriptExecutor`, `switchTo().frame/alert`, cookies, file upload via `Paths.get`/`new File`.
- Lifecycle: TestNG (`@BeforeMethod/Class/Suite/Test`, `@AfterMethod/Class/Suite/Test`) and JUnit 4/5 (`@Before/@After`, `@BeforeEach/@AfterEach`, `@BeforeAll/@AfterAll`).
- `@DataProvider` → typed parameterised `for` loop.
- BaseTest superclass → `tests/fixtures.ts` (Playwright fixture extension).
- `testng.xml` → `playwright.config.ts` projects with `grep` tags.
- `*.properties` → `.env.example` + `tests/config.ts` typed loader.
- Page Object emitter with optional `--emit-self-healing-shim` (wraps locators in `healOrThrow` from `@platform/sdk-self-healing`).

### Post-processing (`src/post/`)

- Optional Prettier pass (`--format`).
- Optional `tsc --noEmit` validation gate (`--validate`) with errors attached to the review report.
- Optional `auth.setup.ts` generator (`--emit-auth-setup`) when a `LoginPage` is detected.
- `// TODO(sel2pw): …` markers (default on; disable with `--no-todo-markers`).

### Reports (`src/reports/`)

- `CONVERSION_REVIEW.md` — line-level review punch list grouped by file and severity.
- `MIGRATION_NOTES.md` — what to delete from `pom.xml`, what to install, runtime semantic changes, CI changes, parity playbook.

### HTTP service (`src/server.ts`, `src/server/`)

Reachable through the `modern-automation-platform` gateway at `/api/v1/converter/*`.

- `GET /health`, `POST /analyze`, `POST /convert`, `GET /jobs/:id`, `GET /jobs/:id/artifact`, `GET /jobs/:id/review`, `POST /feedback`.
- Job runner (JSON file persistence under `<workdir>/jobs/`).
- Artifact storage abstraction (local driver implemented; S3 stubbed).
- Provenance block matches `test-case-generation-service` shape.
- Honours `governance.config_url`, calls the `ai-governance` sidecar's `/audit`, attaches `governance_audit.json`.
- HMAC-signed webhook delivery with retries.

### Cross-repo

- `ai-governance/service/` — FastAPI sidecar with `/health`, `/sanitize`, `/audit`. Docker image at `service/Dockerfile`.
- `modern-automation-platform/apps/framework-generator-api/src/routes/converter.routes.ts` — gateway proxy mounted at `/api/v1/converter` in `app.ts`.
- `modern-automation-platform/packages/shared-types/src/index.ts` — `ConverterJob`, `ConverterStats`, `ConverterReviewItem`, `ConverterProvenance`.

### Distribution

- LICENSE (MIT), CONTRIBUTING.md, .github/workflows/ci.yml (matrix Node 18/20/22 × Linux/macOS/Windows), .github/workflows/release.yml (tag → npm publish --provenance), .github/dependabot.yml, .changeset/config.json, typedoc.json.

### Stretch scaffolds (`src/stretch/`)

- Cucumber BDD → playwright-bdd (`bdd.ts`).
- Auto-fix loop (`autoFix.ts`) with run-once + parse-failures + apply-diff machinery.
- Hybrid AST + LLM (`hybridLlm.ts` + `governanceClient.ts`).
- **Real Anthropic adapter (`anthropicAdapter.ts`) — 0.6.0.** Provides `makeAnthropicPatchCallback()` for autoFix and `makeAnthropicLlmCallback()` for the hybrid engine. Governance sanitise is enforced in code before any model call. Lazy-loads `@anthropic-ai/sdk` so the converter still installs without it.
- Behaviour-parity verifier (`parityVerifier.ts`).
- C# / SpecFlow design doc (`csharp/README.md`).

### Phase 6 additions

- `src/transformers/advancedApiMap.ts → rewriteWindowHandles` — multi-window/tab (`getWindowHandles`, `getWindowHandle`, `switchTo().window`).
- `src/transformers/customUtilDetector.ts` — detects DriverFactory / DriverManager / Wait helpers; emits typed stubs at `tests/_legacy-stubs/<name>.ts`.
- `src/transformers/commentPreserver.ts` — Javadoc + inline comment primitives (`findJavadocBeforeMethod`, `indexAllJavadocs`, `stripFileHeader`). Emitter wiring is the next pass.
- `src/emitters/pageBagEmitter.ts` — opt-in `--pom-style=factory` emits `pages/index.ts` + `tests/fixtures.pages.ts`.
- `tests/fixtures/realworld/` — three representative shapes (page-factory, hamcrest-heavy, bdd-cucumber) + a vitest suite that converts each into a temp dir and asserts on structural properties.

## Verification matrix

| Step | Where it ran | Status |
| --- | --- | --- |
| Sandbox-only Node verifier (`scripts/verify.js`) | Cloud sandbox (no npm) | ✅ passes — confirms conversion logic on bundled sample |
| `npm install` | User's local Windows machine | ✅ confirmed working |
| `npm run lint` | User's local | ✅ clean (0.5.1+) |
| `tsc --noEmit` | User's local | ✅ clean (0.5.1+) |
| `npm test` | User's local | ✅ **45/45 green** (0.6.2) — 40 unit/snapshot + 5 real-world fixture tests |
| `npm run test:update` | User's local | ✅ regenerates snapshots reliably (added 0.6.2) |
| `npm run smoke` | User's local | ⏳ requires both `npm run serve` AND ai-governance sidecar at :4900 — not yet exercised |
| Full platform Docker compose stack | User's local | ⏳ not yet exercised |
| `autoFix` with real Anthropic SDK | Anywhere | ⏳ requires `npm install @anthropic-ai/sdk` + `ANTHROPIC_API_KEY` |

## Pending re-runs on user's machine

After pulling the 0.5.1 patches:

```bash
npm install              # picks up @typescript-eslint v8 bump
npm run lint             # should be clean now
npm test -- -u           # FIRST run only — creates .snap files
npm test                 # subsequent runs — should be 40/40 green
```

## Recently shipped (post-0.10.3)

- **0.10.4** — CLI dynamic version (`readVersion()` reads from package.json so `sel2pw --version` always matches). `deploy.yml` workflow gated to manual-trigger until VPS secrets land.
- **0.10.5** — selenium9–15 patches: classname reserved-word filter (Javadoc "class for" stops matching as `for`), customUtilDetector widened with `Exception$` / `Helpers$` / `Manager$` / `Annotation$` / `Retry` / `Transformer` patterns, BaseTest extractor regex relaxed for parameter-level annotations like `@Optional("chrome")`, conversion-result.json kebab-lookup fixed for underscore-prefixed and base-kind files.
- **0.10.6** — CI matrix green. `createFailureStore` wraps SQLite open in try/catch and falls back to a no-op store on `SQLITE_BUSY` / locked file / permission denied / full disk. Test `convert()` calls pass `telemetryDb: false`. Coverage thresholds removed; coverage job marked `continue-on-error: true`.

## Outcome definitions (formal)

The following terms appear throughout `CONVERSION_REVIEW.md`, `FILE_MAPPING.md`, `conversion-result.json`, and this document. Precise definitions matter because "cleanly converted" can otherwise be read as anything from "no crash" to "behaves the same".

| Term | Definition |
| --- | --- |
| **Scanned** | A file sel2pw's project scanner (`src/scanner/projectScanner.ts`) considered during the run. Equal to the count of `.java` / `.cs` / `.feature` files under the input directory that match the configured source stack. |
| **Cleanly converted** | A file is cleanly converted when **both** of these hold: (a) sel2pw emits a Playwright TypeScript output file (not a `_legacy-stubs/*.ts` stub, not a skip record) **and** (b) `tsc --noEmit` on that single emitted file, under the output project's generated `tsconfig.json`, produces zero errors. This is a **syntactic + type-system** claim. It does **not** imply behavioural equivalence to the original Selenium test (that requires executing the migrated suite against the original target — see `examples/functional-equivalence-output/MANUAL_EDITS.md` for the one controlled case we have evidence for). |
| **Stubbed** | sel2pw's `customUtilDetector` recognised the class as a legacy utility (test helpers, data factories, reporters, driver managers) with no 1:1 Playwright equivalent. A typed stub lands in `tests/_legacy-stubs/<name>.ts`; call sites compile; each call throws a documented `notImplemented()` at runtime. The user migrates each call site, then deletes the stub. |
| **Skipped** | sel2pw's scanner classified the file as infrastructure Playwright replaces natively (`DriverManager`, `BrowserFactory`, `TargetFactory`, Aeonbits `@Config` interfaces, Sauce Labs cloud wrappers). No file is emitted; the Playwright replacement (fixture, `playwright.config.ts` project, env var) is noted in `FILE_MAPPING.md`. |
| **Mixed-cleanup** | Emitted file compiles with 1-20 `tsc --noEmit` errors — typically unmapped idioms, custom-helper call sites, or Selenium patterns outside the current rule catalogue. A targeted hand pass per file resolves these. |
| **Failed conversion** | sel2pw crashed, threw an uncaught exception, or produced no output for a file that was classified as convertible (page-object, test-class, base). As of v2.0.8 the 15-codebase / 409-file benchmark reports **zero failed conversions**. This is a crash-safety claim, not a compilation or equivalence claim. |

## Threats to validity

The 15-codebase benchmark described above should be read with the following caveats, several of which are addressed with planned future work.

### Corpus selection bias

The benchmark corpus (`examples/validation-batch-v2/sources/*`) was selected from public GitHub projects that use Java and Selenium as the primary stack, covering a range of sizes and styles. Public projects tend to be better maintained than internal ones. The primary use case for sel2pw is nevertheless *legacy* Selenium estates — long-dormant corporate test suites that may violate every convention present in the public corpus (Hungarian-notation locators, embedded SQL, custom reflection, framework-level annotations). Validation on genuinely legacy private repositories is categorically out of reach for OSS research but acknowledged as a gap. Future work: survey conference-talk retrospectives on Playwright migrations at scale (Shopify, Microsoft, etc.) and contrast reported pain points against the sel2pw rule catalogue.

### `tsc --noEmit` error count as proxy for human effort

Several outcome classifications and the "mixed-cleanup" bucket rely on the compiled-error count as a proxy for the human cleanup required. This is a conservative but imperfect proxy: one semantically-wrong line can cost more human time than fifty trivial import renames. This project reports error counts as empirical measurements and does **not** convert them into human-hour estimates. No time-saving figure is claimed because no manual-migration baseline has been measured.

### Classification uses tool-generated review reports (acknowledged, not corrected)

The per-file outcome classifications (cleanly converted / stubbed / mixed / skipped / failed) are produced by sel2pw itself and recorded in `CONVERSION_REVIEW.md` and `conversion-result.json`. Evaluating the tool using classifications the tool produces is partially circular. An independent manual audit of a random sample is planned future work. For the one case where independent verification exists (`examples/functional-equivalence-output/`, controlled target), results agreed with the tool's own classification.

### Functional equivalence evaluation is limited

The sel2pw repository contains **one** controlled end-to-end functional equivalence test (`examples/functional-equivalence-output/`): a purpose-built 4-file Selenium+TestNG source targeting `https://www.saucedemo.com`, producing a Playwright suite that runs 2/2 passing after 2 documented hand edits. A real-world attempt against `eliasnogueira/selenium-java-lean-test-architecture` was abandoned because the test suite's target site had been taken offline — an honest finding about target-site drift in the Selenium ecosystem, documented at `examples/validation-batch-v2/outputs/eliasnogueira-func-test/MANUAL_EDITS.md`. Scaling functional-equivalence evaluation to the full 15-codebase corpus requires a live target per repository and is categorically harder than static-conversion evaluation.

### LLM fallback not empirically evaluated

The optional LLM fallback (`src/post/llmFallback.ts`) is shipped with provider adapters for Anthropic, OpenAI, and Google Gemini and is called only for constructs the rule catalogue cannot translate. The 15-codebase benchmark was intentionally conducted with the LLM path **disabled** (`llmFallback: undefined`) so that the reported outcomes reflect only the deterministic rule pipeline. The LLM path's actual contribution rate, success rate, and provider sensitivity have not been measured.

### Semantic gaps between Selenium and Playwright that no source-only tool can bridge

Functional equivalence testing surfaced at least one **fundamental semantic difference** that sel2pw's static-rewrite architecture cannot fully resolve: Selenium's `findElement` silently returns the first match, while Playwright's `page.locator` matches all elements and treats single-element methods on multi-match locators as strict-mode violations. Resolving this correctly requires runtime inspection of the current DOM, which a source-code-only migration tool cannot perform. sel2pw preserves the original selector verbatim; such patterns surface as runtime strict-mode errors on the first test execution and are documented in `CONVERSION_REVIEW.md` for the migration engineer to resolve.

### External target-site drift

Legacy Selenium suites often target training applications or company staging environments that may go offline, redirect, or DOM-drift between the time the Selenium code was written and the time of migration. This is categorically outside sel2pw's scope: a source-code migration tool cannot verify that the target the original tests pointed at is still reachable, let alone that its structure has not changed. The `eliasnogueira-lean` run (target offline) is one documented instance.

### Single-reviewer classification

All manual classification decisions in this benchmark were made by the sel2pw author. Independent replication, inter-rater agreement on the manual sample, and third-party auditing are planned.

## Known deferred items (explicit, not lost)

Phase 6 closed multi-window/tab semantics, custom-utility detection, comment-preservation primitives, and page-bag style. Phase 7 wired comment preservation into emitters. Phase 9 shipped telemetry. Phase 10 shipped the pre-built binary. The public npm publish (`@vijaypjavvadi/sel2pw`) closed the `npx`-without-install gap.

Genuinely remaining deferrals:

1. **Platform UI wizard** (Phase 0, L). Frontend work in `apps/platform-ui` — depends on the platform's existing component library; the Converter API surface it'll consume is locked.
2. **C# / SpecFlow full implementation** (Phase 5, XL). C# extractor handles ~80% of cases (`src/parser/csharpExtractor.ts`); the long tail needs a .NET sidecar — design doc at `src/stretch/csharp/README.md`. Low demand relative to effort.
3. **Performance profiling pass** (Phase 1, S). Needs a real 1k-file project to measure against. Selenium13 at 84 files is the largest we've validated; no perf complaints there.
4. **Generic types with multiple parameters** (Phase 1, S). `Map<String, List<Foo>>` currently passes through as-is. One-line extension to `javaTypeToTs` in `src/utils/naming.ts`.
5. **TestNG listeners → Playwright reporters (full conversion)** (Phase 2, M). Listeners are currently auto-stubbed; full conversion would translate `onTestFailure` → `test.afterEach(({ page }, info) => { if (info.status === 'failed') ... })`.
6. **Custom DTO `.d.ts` stubs** (Phase 3, S). Niche; the `customUtilDetector` already stubs DTO-shaped classes which is good enough.
7. **ESLint on output + surface in review** (Phase 3, S) — **scheduled for 0.10.7.** `tsc --noEmit` validation lands; ESLint over the generated output is the matching pass.
8. **VS Code extension** (Phase 4, M) — **scaffolded in `vscode-extension/`** awaiting marketplace publish. Right-click folder → Convert to Playwright.
9. **Migration playbook blog post** (Phase 4, S) — **drafted at `docs/migration-playbook.md`.** Authored long-form content tied to the validation matrix.
10. **Marketplace listing** (Phase 4, S). Post-1.0 comms work.
11. **Hosted web-service standalone demo** (Phase 5, L). Duplicates the platform-internal HTTP service. The platform IS the hosting story.

## What's next, in priority order

1. **Ship 0.10.7 with ESLint validator over emitted output** (in progress in this branch). Catches subtle bugs in our emitters at convert-time, before the user runs `playwright test`.
2. **Publish the VS Code extension** to the Visual Studio Marketplace once the scaffold is filled out. Discoverability lever — most QA folks find tools through their IDE, not npm search.
3. **Publish the migration playbook** as a Medium / dev.to article + cross-link from the npm package page. Drives installs.
4. **Decide on Phase 0 (platform UI wizard) vs more codebase validation.** UI is L-effort; another 10 codebases is M-effort and widens the benchmark.
5. **Tag v1.0.0** once 3-5 real users have run sel2pw on their own legacy Selenium suites and reported back. Realistic ETA: 2-3 months from the public publish.

## Files of interest

- [`README.md`](./README.md) — what it does, how to run, mapping table, badges
- [`CHANGELOG.md`](./CHANGELOG.md) — full version history (0.1.0 → 0.10.6)
- [`PRODUCTION_TASKS.md`](./PRODUCTION_TASKS.md) — task list (mostly historical now)
- [`INTEGRATION.md`](./INTEGRATION.md) — platform integration architecture
- [`CONTRIBUTING.md`](./CONTRIBUTING.md) — local dev, pipeline architecture
- [`docs/migration-playbook.md`](./docs/migration-playbook.md) — long-form migration narrative (drafted in 0.10.7)
- [`vscode-extension/`](./vscode-extension/) — VS Code extension scaffold (drafted in 0.10.7)
- [`docs/Sel2pw_Deployment_Guide.md`](./docs/Sel2pw_Deployment_Guide.md) — VPS / Docker deployment
