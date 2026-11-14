using Microsoft.AspNetCore.Mvc;
using BandirmaCepte.Services;

namespace BandirmaCepte.Controllers;

public class HomeController : Controller
{
    private readonly MunicipalNewsService _municipalNews;

    public HomeController(MunicipalNewsService municipalNews)
    {
        _municipalNews = municipalNews;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> News(CancellationToken cancellationToken)
    {
        try
        {
            return View(await _municipalNews.GetLatestAsync(cancellationToken));
        }
        catch (HttpRequestException)
        {
            ViewBag.NewsUnavailable = true;
            return View(Array.Empty<MunicipalNewsItem>());
        }
    }
}
