using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.ViewModels.Home.Overview
{
    public class OverviewViewModel
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ArticlePicture { get; set; }
        public string Author { get; set; }

        public DateTime CreatedTime { get; set; }

        public string GetCreatedTime()
        {
            return CreatedTime.ToString("dd-MM-yyyy HH:mm");
        }
    }
}
