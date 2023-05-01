namespace dirty_tpot_links_seeder;

public class TPOTPaper
{
    public int id { get; set; } = -9999;
    public DateTime Date { get; set; }

    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;
    public string RawText { get; set; } = string.Empty;
    public string Markdown { get; set; } = string.Empty;


    public override string ToString() {
        return "my paper";
    }
}
