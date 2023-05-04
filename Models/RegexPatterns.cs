

public class RegexPatterns
{
    // TODO: Move these to a special JSON API (Parsely)

    // https://regex101.com/r/VvS66U/1
    public readonly static Dictionary<int, string> FacebookComments = new string [] { 
        """(class=\"(?<css_selector_see_more>x[\w\d{5}\s]+)\".*(See\s*more)<\/div>)|(<span\s*?class=\"(?<css_selector_replies>x[\w\d{5}\s]+))\"\s(dir="auto")?>(\d*\s*)(Replies)"""
    }
    .Select((s, index) => new { s, index })
    .ToDictionary(x => x.index, x => x.s.Trim());

    public readonly static Dictionary<int, string> FrontMatter = new string [] { 
        """(?<label>^[a-zA-Z_\s]+):(?<value>.*\s*?)"""
        , """(?<label>\w+:)(?<value>.*)"""
    }
    .Select((s, index) => new { s, index })
    .ToDictionary(x => x.index, x => x.s.Trim());
    
    public readonly static Dictionary<int, string> Hugos =   new string [] {
                // """^(---)?(?<frontmatter>.*)(---)(?<rawmarkdown>.*)$"""
                """(?<=(---))\s*(?<frontmatter>(([a-zA-Z_]+:\s*)(.*?)(\s+))*)(---)\s*(?<rawmarkdown>(.*\s*)*)$"""
            }
            .Select((s, index) => new { s, index })
            .ToDictionary(x => x.index, x => x.s.Trim());

    public readonly static Dictionary<int, string> MarkdownExtractor = new string [] {
            """(id:\s*)?(?<id>\d+)\s*(title:\s*)?(?<title>(\w*\s*))?+\s*(slug:\s*)?(?<slug>[a-zA-Z-_\s]+)\s*(link:\s*)?(?<link>[:\/.a-zA-Z-_\s]+.htm)?\s*(type:\s*)?(?<type>[a-zA-Z-_\s]+)?\s*(status:.*)?\s*(date:.*)\s*(modified:.*)\s*(cover_image:.*)\s*(author:.*)\s*(tags:.*\s-\s*\d+)\s*(comment_status:.*)\s*(template:.*)\s*(meta:.*)\s*(custom:.*)\s*(excerpt:\s*)(?<excerpt>.*\s*)*?(---)?(?<markdown>(.*\s*)*)$"""
            ,
            """
            (id:\s*)?(?<id>\d+)?\s*(title:\s*)?(?<title>(\w+\s+))?\s*(slug:\s*)?(?<slug>[a-zA-Z-_\s]+)?\s*(link:\s*)?(?<link>[:\/.a-zA-Z-_\s]+.htm)?\s*(type:\s*)?(?<type>[a-zA-Z-_\s]+)?\s*(status:\s*)?(?<status>[a-zA-Z-_\s]+)?\s*(date:\s*)?(?<date>.*)?\s*(modified:\s*)?(?<modified>[a-zA-Z-_\s]+)?\s*(author:\s*\d+)?
            """
            ,           
            """(id:\s*)(?<id>\d+)\s*(title:\s*)?(?<title>\w+)\s*(link:\s*)?(?<link>[:\/.a-zA-Z-_\s]+(.htm)?)?(type:\s*)?\s*.*$(?<type>[a-zA-Z-_\s]+)?\s*(?<markdown>.*)(---)?$"""
            ,
            """
            ^(---)?\s*(id:\s*)(?<id>\d+)\s*(title:\s*)?(?<title>\w+)\s*(link:\s*)?(?<link>[:\/.a-zA-Z-_\s]+(.htm)?)?(type:\s*)?\s*.*$(?<type>[a-zA-Z-_\s]+)?\s*(?<markdown>.*)(---)?$
            """
            ,

            /** 
            ---\nid: 13564\ntitle: Chat\nslug: chat-htm\nlink: https://www.thepathoftruth.com/chat-htm\ntype: page\nstatus: publish\ndate: '2016-08-21T02:07:48'\nmodified: '2017-02-12T14:26:59'\ncover_image: 0\nauthor: 10\ntags:\n- 1\ncomment_status: open\ntemplate: ''\nmeta: []\ncustom: []\nexcerpt: <p>Chat</p>\ncomments: []\n---

            */

            """
                \s*(id:\s*)(?<id>\d+)\s*
                ((title:\s*)?(?<title>\w+)\s*)
                (slug:\s*(?<slug>[a-zA-Z-_]+)\s*)?(link:\s*(?<link>[:\/.a-zA-Z-_\s]+(\.htm)?)\s*)?[\n]?             
            """

            /**
                (link:\s*(?<link>[:\/.a-zA-Z-_\s]+(\.htm)?)?\s*)?
                (type:\s*)?\s*.*$(?<type>[a-zA-Z-_\s]+)?
                (category:\s*)?\s*.*$(?<category>[a-zA-Z-_\s]+)?
                ((date:\s+)'(?<Date>.*)')?\s*((modified:\s+)'(?<modified>.*)')?\s*
                (cover_image:\s+)

                \s*(id:\s+(\d+)\s*)?(title:\s*(?<title>\w+)\s*)*(slug:\s*(?<slug>[a-zA-Z-_]+)\s*)?(link:\s*(?<link>[:\/.a-zA-Z-_\s]+(\.htm)?)\s*)?[\n]?
            */
        }
        .Select((s, index) => new { s, index })
        .ToDictionary(x => x.index, x => x.s.Trim());

}