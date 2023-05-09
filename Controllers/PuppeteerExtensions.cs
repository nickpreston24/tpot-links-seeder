using CodeMechanic.Diagnostics;
using CodeMechanic.Types;
using PuppeteerSharp;

namespace CodeMechanic.PuppeteerExtensions;

/// <summary>
/// Based on:
/// https://www.puppeteersharp.com/examples/Page.WaitForSelectorAsync.Searching.html
/// </summary>
public static class PuppeteerExtensions
{
    public static async Task<object> GetHtmlPropertyFromElement(
        this IPage page
        , string element = "h1"
        , string html_property = "innerText"
    )
    {
        var pageHeaderHandle = await page.QuerySelectorAsync(element);
        var innerTextHandle = await pageHeaderHandle.GetPropertyAsync(html_property);
        var property_found = await innerTextHandle.JsonValueAsync();

        property_found.Dump(html_property);

        return property_found;
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