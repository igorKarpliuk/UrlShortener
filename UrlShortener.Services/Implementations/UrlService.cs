using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Models;
using UrlShortener.Core.Models.Identity;
using UrlShortener.Data.ViewModels;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Services.Implementations
{
    public class UrlService: IUrlService
    {
        private readonly IGenericRepository<URL> _urlRepository;
        private readonly UserManager<User> _userManager;
        public UrlService(IGenericRepository<URL> urlRepository, UserManager<User> userManager)
        {
            _urlRepository = urlRepository;
            _userManager = userManager;
        }
        public void AddUrl(URL url)
        {
            _urlRepository.Add(url);
        }
        public void DeleteUrl(URL url)
        {
            _urlRepository.Delete(url);
        }
        public void AddShortUrlToUrl(URL url)
        {
            url.ShortUrl = WebEncoders.Base64UrlEncode(BitConverter.GetBytes(url.Id));
            _urlRepository.Update(url);
        }
        public UrlInfoViewModel GetUrlByIdForInfo(int id)
        {
            var url = _urlRepository.GetWithInclude(predicate: u => u.Id == id, includeProperties: (u => u.User)).FirstOrDefault();
            string name = _userManager.FindByIdAsync(url.User.Id.ToString()).Result.UserName;
            return new UrlInfoViewModel()
            {
                CreationDate = url.CreationDate,
                CreatedBy = name
            };
        }
        public URL GetUrlByUrl(string url)
        {
            return _urlRepository.Get(u => u.Url == url).FirstOrDefault();
        }
        public URL GetUrlById(int id)
        {
            return _urlRepository.GetWithInclude(predicate: u => u.Id == id, includeProperties: (u => u.User)).FirstOrDefault();
        }
        public string UrlShortening(string url)
        {
            return url;
        }
        public IEnumerable<UrlViewModel> GetAllUrls(string schema)
        {
            var all_urls = _urlRepository.GetWithInclude(u => u.User);
            IEnumerable<UrlViewModel> urls = all_urls.Select(u => new UrlViewModel() 
            {
                Id=u.Id,
                Url=u.Url,
                ShortUrl = $"{schema}/{u.ShortUrl}",
                UserId = u.User.Id.ToString()
            });
            return urls;
        }
    }
}
