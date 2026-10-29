---
title: "docs: add before/after code examples to the README's \"What it converts\" section"
labels: ["documentation", "good first issue"]
---

**Why**

The README's "What it converts" section lists about 90 mappings in tables. People deciding whether to try sel2pw find a short Java → TypeScript example much easier to read than a table row.

**What to do**

Pick **5 to 10** of the patterns below. For each one, add a small Java block and the matching TypeScript block under the right heading in `README.md`.

1. `driver.get(url)` → `await page.goto(url)`
2. `@FindBy(id = "x") WebElement el;` → a `Locator` field built in the constructor
3. `new WebDriverWait(driver, 10).until(...)` → removed, because Playwright waits automatically
4. `assertThat(items, hasItem("Home"))` (Hamcrest) → `expect(items).toContain("Home")`
5. `new Actions(driver).moveToElement(el).perform()` → `await el.hover()`
6. `new Select(dropdown).selectByVisibleText("EN")` → `await dropdown.selectOption({ label: "EN" })`
7. `@BeforeMethod` / `@AfterMethod` → `test.beforeEach` / `test.afterEach`
8. `((JavascriptExecutor) driver).executeScript(js)` → `await page.evaluate(...)`

**Tip:** don't guess the output. Put the Java snippet in a small file inside `examples/selenium-testng-sample/`, run `npm run convert:sample`, and copy what sel2pw actually produces from `examples/output-playwright/`. That way the README shows real behaviour.

**Done when**

- [ ] At least 5 Java/TypeScript pairs are added under the existing headings.
- [ ] Each TypeScript block matches what the tool really emits.
- [ ] No existing text is removed.

---

**New here?** Comment on this issue to claim it, and we'll assign it to you. Setup takes about five minutes: see [CONTRIBUTING.md](https://github.com/javvadivijayprasad/sel2pw/blob/main/CONTRIBUTING.md). Questions are welcome in the comments, and no question is too small.
