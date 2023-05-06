namespace tpot_links_seeder;

public class HugoPaper
{
    /**
      frontmatter = "---\n" + yaml + "---\n"
        text = frontmatter + "\n" + content
    */
    public string FrontMatter { get; set; } = string.Empty;

    public string RawMarkdown { get; set; } = string.Empty;
}
