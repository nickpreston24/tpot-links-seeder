
namespace tpot_links_seeder;


public class TPOTPapersResult
{
    public List<TPOTMarkdownPaper> Papers { get; set; } = new List<TPOTMarkdownPaper>();
    public Dictionary<string, string> Markdown { get; set; } = new Dictionary<string, string> ();
    public int Count { get; set; }
    public int valid_papers { get; set; }
    public int invalid_papers { get; set; }
    public double percent_passing => (1.0 * valid_papers / Count) * 100; 
    public int total_files_on_disk { get; set; }
    public string Elapsed { get; set; }
}
