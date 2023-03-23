using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Data.ViewModels
{
    public class UrlViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public string UserId { get; set; }
    }
}
