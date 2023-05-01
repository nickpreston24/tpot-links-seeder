using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using CodeMechanic.Extensions;
using CodeMechanic.Async;
using CodeMechanic.FS;
using CodeMechanic.Advanced.Extensions;

namespace dirty_tpot_links_seeder.Controllers;

[ApiController]
[Route("[controller]")]
public class TPOTPaperController : ControllerBase
{
    private static readonly string[] Categories = new[]
    {
        "Faith", "Jesus Christ", "Holy Spirit", "Marriage", "Obedience"
    };

    private readonly ILogger<TPOTPaperController> _logger;
    private readonly IWebHostEnvironment _env;

    public TPOTPaperController(
        ILogger<TPOTPaperController> logger
        , IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    [HttpGet]
    public IEnumerable<TPOTPaper> GetPaper(int id = -1)
    {
        Console.WriteLine(id);
        return Enumerable.Range(1, 50).Select(index => new TPOTPaper
        {
            // Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Date = DateTime.Now,
            Content = Categories[Random.Shared.Next(Categories.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public async Task<TPOTPaper> CreatePaper([FromBody]TPOTPaper incoming_paper)
    {           

        // string PAT = Environment.GetEnvironmentVariable("NUGS_PAT");
        // string base_key = Environment.GetEnvironmentVariable("NUGS_BASE_KEY");

        incoming_paper.Dump("incoming paper");   
        string root_folder = Path.Combine(_env.ContentRootPath.GoUp(), "tpot_static_wip").Dump("root");

        var watch = new Stopwatch();
        watch.Start();

        var queue = new SerialQueue();
        var tasks = new List<Task<TPOTPaper>>();
        // var all_papers = Enumerable.Repeat<TPOTPaper>(new TPOTPaper(), 100);

        var grepper = new Grepper ()
            .With(grep => 
            {
                grep.RootPath = root_folder;
                grep.FileNamePattern = @".*\.md";
            });

        var all_files = grepper.GetFileNames()/*.Dump("Files")*/;

        var markdown_extraction_pattern = @"""
        
            (id:\s*)?(?<id>\d+)\s*(title:\s*)?(?<title>(\w+\s+))+\s*(slug:\s*)(?<slug>[a-zA-Z-_\s]+)\s*(link:\s*)(?<link>[:\/.a-zA-Z-_\s]+.htm)\s*(type:\s*)(?<type>[a-zA-Z-_\s]+)\s*(status:.*)\s*(date:.*)\s*(modified:.*)\s*(cover_image:.*)\s*(author:.*)\s*(tags:.*\s-\s*\d+)\s*(comment_status:.*)\s*(template:.*)\s*(meta:.*)\s*(custom:.*)\s*(excerpt:\s*)(?<excerpt>.*\s*)*?(---)(?<markdown>(.*\s*)*)$


        """;

        var all_papers = all_files
            .Select(file_path => new TPOTPaper()
            .With(p => {
                p.FilePath = file_path;
                p.RawText = System.IO.File.ReadAllText(file_path);
            }));

        foreach (var paper in all_papers.Skip(1).Take(1)) {
            tasks.Add(queue.Enqueue(async () => {
                // paper.Dump("paper");
                var updated = paper.RawText.Dump("raw text").Extract<TPOTPaper>(markdown_extraction_pattern).FirstOrDefault().Dump("updated paper");
                paper.Markdown = updated?.Markdown/*.Dump("extracted markdown")*/;
                return paper;
            }));
        }

        var results = (await Task.WhenAll(tasks)).ToList();

        var pages_with_markdown = results.Where(page=> !string.IsNullOrEmpty( page?.Markdown)).ToList();


        results.Count.Dump("total pages");
        pages_with_markdown.Count.Dump("total pages w/ markdown extracted");

        watch.Elapsed.Dump("All tasks Done in");

        return default;
    }
}
