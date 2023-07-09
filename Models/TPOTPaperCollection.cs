public class TPOTPaperCollection<T>
{
    private string json;
    private List<T> records = new List<T>();
/**
      "date": "2016-06-05T15:30:50",
            "date_gmt": "2016-06-05T21:30:50",
            "guid": {
                "rendered": "http://www.thepathoftruth.com/newsite/german___praying-for-salvation-of-souls-german-htm.htm"
            },
            "modified": "2022-09-07T18:56:21",
            "modified_gmt": "2022-09-08T00:56:21",
            "slug": "german___praying-for-salvation-of-souls-german-htm",
            "status": "publish",
            "type": "page",
            "link": "https://www.thepathoftruth.com/german/praying-for-salvation-of-souls-german.htm",
            */

    public TPOTPaperCollection(string json = """
        {
            "id": 6315     
        }
        """) 
    {
        this.json = json;
        // this.records = new List<T>( JsonConvert.DeserializeObject<T>(json));

        /// https://www.newtonsoft.com/json/help/html/SerializingJSONFragments.htm

        // JObject search = JObject.Parse(json);

        // var results = search.Children();

        // // get JSON result objects into a list
        // // IList<JToken> results = search["records"]
        // //     .Children()/*.Dump("children")*/["fields"]
        // //     /*.Dump("fields children")*/
        // //     .ToList();
  
        // // serialize JSON results into .NET objects
        // IList<T> records = new List<T>();
        // foreach (JToken result in results)
        // {
        //     // JToken.ToObject is a helper method that uses JsonSerializer internally
        //     T instance = result.ToObject<T>();
        //     this.records.Add(instance);
        // }
        // // this.records.Dump("extracted records");

    }

    public void Deconstruct(
        out string raw_json,
        out List<T> records
    )
    {
        raw_json = this.json;
        records = this.records/*.Dump("returned records")*/;
    }
}