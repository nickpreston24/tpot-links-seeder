namespace tpot_links_seeder;

public class TPOTMarkdownPaper
{
    public int id { get; set; } = -9999;

    public string Markdown { get; set; } = string.Empty;
    public string FrontMatter { get; set; } = string.Empty;


    // public string Category { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string RawText { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;

    public Dictionary<string, string> FrontmatterPairs { get; set; } = new Dictionary<string, string>();
    // public string Title { get; set; } = string.Empty;
    // public string Link { get; set; } = string.Empty;
    // public string cover_image { get; set; } = string.Empty;
    // public string template { get; set; } = string.Empty;
    // public string comment_status { get; set; } = string.Empty;
    // public string comments { get; set; } = string.Empty;
    // public string custom { get; set; } = string.Empty;
    // public string status { get; set; } = string.Empty;
    // public DateTime Modified { get; set; }
    // public DateTime Date { get; set; }
    // public int author { get; set; } = -1;
}

public class PapersCollection
{
    public List<WordpressPaper> Papers { get; set; } = new List<WordpressPaper>();
}

public class Guid
{
    public string rendered { get; set; }
}

public class Title
{
    public string rendered { get; set; }
}

public class Content
{
    public string rendered { get; set; }
    // public bool protected { get; set; }
}

public class Excerpt
{
    public string rendered { get; set; }
    // public bool protected { get; set; }
}

public class Yoast_head_json
{
    public string title { get; set; }
    public string description { get; set; }
    public Robots robots { get; set; }
    public string canonical { get; set; }
    public string og_locale { get; set; }
    public string og_type { get; set; }
    public string og_title { get; set; }
    public string og_description { get; set; }
    public string og_url { get; set; }
    public string og_site_name { get; set; }
    public string article_publisher { get; set; }
    public Og_image[] og_image { get; set; }
    public string twitter_card { get; set; }
    public string twitter_site { get; set; }
    public Twitter_misc twitter_misc { get; set; }
    public Schema schema { get; set; }
}

public class Robots
{
    public string index { get; set; }
    public string follow { get; set; }
    public string max_snippet { get; set; }
    public string max_image_preview { get; set; }
    public string max_video_preview { get; set; }
}

public class Og_image
{
    public int width { get; set; }
    public int height { get; set; }
    public string url { get; set; }
    public string type { get; set; }
}

public class Twitter_misc
{
    public string Est__reading_time { get; set; }
}

public class Schema
{
    public string _context { get; set; }
    public _graph[] _graph { get; set; }
}

public class _graph
{
    public string _type { get; set; }
    public string _id { get; set; }
    public string url { get; set; }
    public string name { get; set; }
    public IsPartOf isPartOf { get; set; }
    public string datePublished { get; set; }
    public string dateModified { get; set; }
    public string description { get; set; }
    public Breadcrumb breadcrumb { get; set; }
    public string inLanguage { get; set; }
    public PotentialAction[] potentialAction { get; set; }
    public ItemListElement[] itemListElement { get; set; }
    public Publisher publisher { get; set; }
    public string[] sameAs { get; set; }
    public Logo logo { get; set; }
    public Image image { get; set; }
}

public class IsPartOf
{
    public string _id { get; set; }
}

public class Breadcrumb
{
    public string _id { get; set; }
}

public class PotentialAction
{
    public string _type { get; set; }
    public object target { get; set; }
    public string query_input { get; set; }
}

public class ItemListElement
{
    public string _type { get; set; }
    public int position { get; set; }
    public string name { get; set; }
    public string item { get; set; }
}

public class Publisher
{
    public string _id { get; set; }
}

public class Logo
{
    public string _type { get; set; }
    public string inLanguage { get; set; }
    public string _id { get; set; }
    public string url { get; set; }
    public string contentUrl { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public string caption { get; set; }
}

public class Image
{
    public string _id { get; set; }
}

public class _links
{
    // public Self[] self { get; set; }
    // public Collection[] collection { get; set; }
    // public About[] about { get; set; }
    // public Author[] author { get; set; }
    // public Replies[] replies { get; set; }
    // public Version_history[] version_history { get; set; }
    // public Predecessor_version[] predecessor_version { get; set; }
    // public Wp_attachment[] wp_attachment { get; set; }
    // public Wp_term[] wp_term { get; set; }
    // public Curies[] curies { get; set; }
}

public class Self
{
    public string href { get; set; }
}

public class Collection
{
    public string href { get; set; }
}

public class About
{
    public string href { get; set; }
}

public class Author
{
    public bool embeddable { get; set; }
    public string href { get; set; }
}

public class Replies
{
    public bool embeddable { get; set; }
    public string href { get; set; }
}

public class Version_history
{
    public int count { get; set; }
    public string href { get; set; }
}

public class Predecessor_version
{
    public int id { get; set; }
    public string href { get; set; }
}

public class Wp_attachment
{
    public string href { get; set; }
}

public class Wp_term
{
    public string taxonomy { get; set; }
    public bool embeddable { get; set; }
    public string href { get; set; }
}

public class Curies
{
    public string name { get; set; }
    public string href { get; set; }
    public bool templated { get; set; }
}

public class FrontmatterPair
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}


/*

---\nid: 13564\ntitle: Chat\nslug: chat-htm\nlink: https://www.thepathoftruth.com/chat-htm\ntype: page\nstatus: publish\ndate: '2016-08-21T02:07:48'\nmodified: '2017-02-12T14:26:59'\ncover_image: 0\nauthor: 10\ntags:\n- 1\ncomment_status: open\ntemplate: ''\nmeta: []\ncustom: []\nexcerpt: <p>Chat</p>\ncomments: []\n---


*/