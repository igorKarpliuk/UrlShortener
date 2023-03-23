using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Core.Models.Identity;

namespace UrlShortener.Core.Models
{
    [Index(nameof(Url), IsUnique = true)]
    [Index(nameof(ShortUrl), IsUnique = true)]
    public class URL
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual User User { get; set; }
    }
}
