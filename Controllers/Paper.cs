namespace tpot_links_seeder;

public class Paper
{
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Categories { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    // public string Rendered_Content { get; set; } = string.Empty;

    public string created { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Comment_status { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Links { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string last_modified_at_wp { get; set; } = string.Empty;
    public string created_at_wp { get; set; } = string.Empty;
    public string Type { get; set; }
    public int AuthorId { get; set; } = -1;
}