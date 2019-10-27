﻿using System;
using System.Collections.Generic;


namespace RSS_Demo.Data
{
    public class Podcast
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Intervall { get; set; }
        public string Category { get; set; }
        public string Link { get; set; }
        public int EpisodeCount { get; set; }
        public List<Episode> EpisodeList {get; set;}

        public Podcast(string title, string description, int intervall, string category, string link, int episodeCount, List<Episode> episodeList)
        {

            this.Title = title;
            this.Description = description;
            this.Intervall = intervall;
            this.Category = category;
            this.Link = link;
            this.EpisodeList = episodeList;
            this.EpisodeCount = episodeCount;
        }
        public Podcast() { }
    }
}

