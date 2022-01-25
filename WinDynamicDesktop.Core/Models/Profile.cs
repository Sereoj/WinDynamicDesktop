﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WinDynamicDesktop.Core.Models
{
    public class Profile
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string country { get; set; }
        public string avatar { get; set; }
        public string cover { get; set; }
        public string users_like_count { get; set; }
        public string subscribers_count { get; set; }
        public string subscriptions_count { get; set; }
        public string posts_count { get; set; }
        public List<Thumb> posts { get; set; }
    }
}
