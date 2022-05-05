using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Exam.Dto.Results
{
    public class WiredSummary
    {
        public WiredSummary()
        { }

        public WiredSummary(SyndicationItem item)
        {
            this.Title = item.Title.Text;
            this.Link = item.Links.FirstOrDefault()?.Uri.ToString();
            this.Id = item.Id;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }

        public static IEnumerable<WiredSummary> GetWiredSummaries()
        {
            var url = "https://www.wired.com/feed/rss";
            using var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);

            return feed.Items.Select(x => new WiredSummary(x));
        }
    }
}
