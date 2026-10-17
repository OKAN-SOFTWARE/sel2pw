# sel2pw C# NUnit → Playwright TypeScript — experimental conversion artifact

This folder is the **C# path evidence** for the sel2pw JOSS paper. It documents what the experimental C# front-end produces today, honestly and with numbers, so a reader can judge the current scope of C# support.

**Source:** `examples/csharp-nunit-target/` — a purpose-built minimal Selenium + NUnit source. Two Page Objects (`LoginPage`, `InventoryPage`), one TestFixture with two tests (positive and negative login). Targets `https://www.saucedemo.com`. Pattern mirrors the Java TestNG `functional-equivalence-target/` so both front-ends can be compared directly.

**Conversion command:**

```bash
npx @vijaypjavvadi/sel2pw@2.0.10 convert \
  examples/csharp-nunit-target \
  --out examples/csharp-nunit-output
```

## Result on v2.0.10

- **Files scanned:** 3
- **Files emitted:** 3 (2 pages + 1 spec)
- **`tsc --noEmit` on output:** **30 TypeScript errors across 3 files**
  - TS2304 (Cannot find name): 20 — undefined references (e.g. `bool`, `driver`, `options`, `loginPage`, `inventoryPage`)
  - TS7031 (Binding element implicitly any): 4
  - TS2307 (Cannot find module `@playwright/test`): 4 — resolved by `npm install`
  - TS2552 (Did you mean): 2

**Cleanly converted files: 0 of 3** (as defined in `STATUS.md`).

## What this evidences

The C# front-end emits a plausible-looking Playwright scaffold but does not currently produce TypeScript that compiles. The pipeline **detects the structural shapes** — Page Objects, test fixtures, `[Test]` methods — and lays them out in a canonical Playwright project. The transformer bank, however, is Java-primary; many Java-side rewrites do not yet have a C# analogue:

- C# `bool` is not rewritten to TypeScript `boolean`
- `IWebElement` is not rewritten to `Locator`
- `driver.Navigate().GoToUrl(url)` is not rewritten to `page.goto(url)`
- `Assert.That(x, Is.EqualTo(y))` is not rewritten to `expect(x).toBe(y)`
- PascalCase method names (`.Open()`, `.LoginAs(...)`) are not lowercased to the camelCase idiom
- `[SetUp]` / `[TearDown]` lifecycle bodies are not stripped of driver boilerplate the way `@BeforeMethod` is

Each of these is a one- or two-line addition to a transformer file. None is architectural. The reason they are not implemented is empirical: the public C# + NUnit + Selenium corpus is small compared to the Java + TestNG + Selenium corpus, and the maintainer has prioritised Java first. The limitation is scope, not design.

## Position in the paper

This artifact is the honest companion to the Java functional-equivalence artifact (`examples/functional-equivalence-output/`, 2/2 passing). The two together establish the current state:

- **Java front-end:** AST-driven, 36.7% of 409 benchmark files cleanly converted, one controlled target passing end-to-end.
- **C# front-end:** regex and balanced-brace extraction, structural output succeeds, transformer coverage incomplete, no cleanly compiled files on the controlled sample yet, explicitly scoped as experimental in README and STATUS.

Scaling the C# transformer bank to match the Java bank is tracked as planned future work.

## Reproducibility

- sel2pw version: 2.0.10
- Target framework: Selenium + NUnit (`Selenium.WebDriver 4.15.0`, `NUnit 4.0.1`)
- Target site: `https://www.saucedemo.com`
- Node: v20 or v22; TypeScript: `tsc` as installed by `npm install` in the output folder
