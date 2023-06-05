namespace tpot_links_seeder;

public class WordpressPaper
{
    public int id { get; set; }
    public string date { get; set; }
    public string date_gmt { get; set; }
    public Guid guid { get; set; }
    public string modified { get; set; }
    public string modified_gmt { get; set; }
    public string slug { get; set; }
    public string status { get; set; }
    public string type { get; set; }
    public string link { get; set; }
    public Title title { get; set; }
    public Content content { get; set; }
    public Excerpt excerpt { get; set; }
    public int author { get; set; }
    public int featured_media { get; set; }
    public int parent { get; set; }
    public int menu_order { get; set; }
    public string comment_status { get; set; }
    public string ping_status { get; set; }
    public string template { get; set; }
    public object[] meta { get; set; }
    public int[] categories { get; set; }
    public object[] tags { get; set; }
    public object[] acf { get; set; }
    public string yoast_head { get; set; }
    public Yoast_head_json yoast_head_json { get; set; }
    public _links _links { get; set; }
}