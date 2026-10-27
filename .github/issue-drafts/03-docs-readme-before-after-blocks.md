---
title: "docs: fill the README migration-patterns table with 10 concrete before/after code blocks"
labels: ["documentation", "good first issue"]
---

The README's "What it converts" section (10 subsections, ~90 mappings) currently shows input → output in a terse table form. For engineers reading it to decide whether to try sel2pw, a short concrete code snippet pair for each top-N pattern would be more persuasive than the table alone.

**Pick any 10 of these and add a code-block pair for each:**

1. `driver.get(url)` → `await page.goto(url)`
2. `@FindBy(id = "x") WebElement el;` → `readonly el: Locator = page.locator('#x');`
3. `new WebDriverWait(driver, 10).until(ExpectedConditions.visibilityOf(el))` → `await el.waitFor({ state: 'visible' })` (then note it's usually unnecessary)
4. `assertThat(items, hasItem("Home"))` _(Hamcrest)_ → `expect(items).toContain('Home')`
5. `assertThat(actual).isEqualTo(x).isNotNull()` _(AssertJ)_ → `expect(actual).toBe(x); expect(actual).not.toBeNull();`
6. `new Actions(driver).moveToElement(el).click().perform()` → `await el.hover(); await el.click();`
7. `new Select(dropdown).selectByVisibleText("EN")` → `await dropdown.selectOption({ label: 'EN' });`
8. `@BeforeMethod` + `@AfterMethod` → `test.beforeEach(...)` + `test.afterEach(...)`
9. `@Test(dataProvider = "rows")` + provider method → parameterised test loop pattern
10. `((JavascriptExecutor)driver).executeScript(js)` → `await page.evaluate(() => { ... })`

**Format for each entry**

~2 lines Java, ~2 lines TypeScript, placed inline under the corresponding subsection in `README.md`.

**Acceptance**

- 10 code-block pairs added to README.md under the existing category headings.
- No existing text removed; this is additive.
