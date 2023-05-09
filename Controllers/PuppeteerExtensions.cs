using CodeMechanic.Diagnostics;
using CodeMechanic.Types;
using PuppeteerSharp;

namespace CodeMechanic.PuppeteerExtensions;

public static class PuppeteerExtensions
{
    public static async Task<object> GetPropertyFromElement(
        this IBrowser browser
        , string element = "h1"
        , string html_property = "innerText"
    )
    {
        using (var page = await browser.NewPageAsync())
        {
            await page.GoToAsync("https://www.hardkoded.com/blog/ui-testing-with-puppeteer-released");
            var pageHeaderHandle = await page.QuerySelectorAsync(element);
            var innerTextHandle = await pageHeaderHandle.GetPropertyAsync(html_property);
            var inner_text = await innerTextHandle.JsonValueAsync();

            inner_text.Dump(html_property);

            return inner_text;
        }
    }

    public static async Task<IEnumerable<string>> GetAllLinksFromPage(
        this IBrowser browser
        , string starting_url
        , LaunchOptions options = null)
    {
        var launch_options = options
            .ToMaybe()
            .IfNone(new LaunchOptions()
                .Map(launchOptions =>
                {
                    launchOptions.Headless = true;
                    return launchOptions;
                }));

        // using (var browser = await Puppeteer.LaunchAsync(launch_options))
        using (var page = await browser.NewPageAsync())
        {
            await page.GoToAsync(starting_url);
            var jsSelectAllAnchors = @"Array.from(document.querySelectorAll('a')).map(a => a.href);";
            var urls = await page.EvaluateExpressionAsync<string[]>(jsSelectAllAnchors);

            urls.Dump("Links detected");
            return urls;
        }
    }
}