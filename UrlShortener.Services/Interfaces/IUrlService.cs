using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Models;
using UrlShortener.Data.ViewModels;

namespace UrlShortener.Services.Interfaces
{
    public interface IUrlService
    {
        public void AddUrl(URL url);
        public void DeleteUrl(URL url);
        public void AddShortUrlToUrl(URL url);
        public UrlInfoViewModel GetUrlByIdForInfo(int id);
        public URL GetUrlByUrl(string url);
        public URL GetUrlById(int id);
        public IEnumerable<UrlViewModel> GetAllUrls(string schema);
        public string UrlShortening(string url);
    }
}
