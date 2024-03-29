﻿using System.Collections.Generic;

namespace RSS_Demo.Logik
{
    public class Podcast
    {
        public string FeedLink { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Link { get; set; }
        public int UpdateInterval { get; set; }
        public int EpisodeCount { get; set; }
        public List<Episode> EpisodeList { get; set; }

        public Podcast(string title, string description, string category, string link, int UpdateInterval, int episodeCount, List<Episode> episodeList)
        {
            this.Title = title;
            this.Description = description;
            this.Category = category;
            this.Link = link;
            this.EpisodeList = episodeList;
            this.UpdateInterval = UpdateInterval;
            this.EpisodeCount = episodeCount;

        }

        public Podcast()
        {        }       
    }
}