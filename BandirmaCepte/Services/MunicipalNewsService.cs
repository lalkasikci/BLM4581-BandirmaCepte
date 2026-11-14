using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;

namespace BandirmaCepte.Services;

public sealed class MunicipalNewsService
{
    private const string NewsPage = "https://www.bandirma.bel.tr/haberler?page=1";
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private static readonly Regex ArticleLink = new(
        "<a\\b[^>]*href=[\\\"'](?<url>(?:https://www\\.bandirma\\.bel\\.tr)?/haber/[^\\\"']+)[\\\"'][^>]*>(?<body>.*?)</a>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
    private static readonly Regex Tags = new("<[^>]+>", RegexOptions.Compiled);

    public MunicipalNewsService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<IReadOnlyList<MunicipalNewsItem>> GetLatestAsync(CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue("municipal-news", out IReadOnlyList<MunicipalNewsItem>? cached) && cached is not null)
            return cached;

        var gate = await _cache.GetOrCreateAsync("municipal-news-refresh", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            return await LoadLatestAsync(cancellationToken);
        });
        if (gate is not null)
        {
            _cache.Set("municipal-news", gate, TimeSpan.FromMinutes(10));
            return gate;
        }

        return Array.Empty<MunicipalNewsItem>();
    }

    private async Task<IReadOnlyList<MunicipalNewsItem>> LoadLatestAsync(CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(NewsPage, cancellationToken);
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync(cancellationToken);
        var items = new List<MunicipalNewsItem>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var candidates = new List<(string Title, string Url)>();
        foreach (Match match in ArticleLink.Matches(html))
        {
            var url = WebUtility.HtmlDecode(match.Groups["url"].Value);
            var title = CleanText(match.Groups["body"].Value);
            title = Regex.Replace(title, @"\s+\d{1,2}\s+\p{L}+\s+20\d{2}.*$", "").Trim();
            if (title.Length < 8 || !seen.Add(url)) continue;

            var articleUrl = url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                ? url
                : $"https://www.bandirma.bel.tr{url}";
            candidates.Add((title, articleUrl));
            if (candidates.Count == 20) break;
        }

        var summaries = new string?[candidates.Count];
        await Parallel.ForEachAsync(Enumerable.Range(0, candidates.Count),
            new ParallelOptions { MaxDegreeOfParallelism = 6, CancellationToken = cancellationToken },
            async (index, token) => summaries[index] = await GetSummaryAsync(candidates[index].Url, token));

        for (var index = 0; index < candidates.Count; index++)
            items.Add(new MunicipalNewsItem(candidates[index].Title, "Bandırma Belediyesi", candidates[index].Url, summaries[index]));

        return items;
    }

    private async Task<string?> GetSummaryAsync(string articleUrl, CancellationToken cancellationToken)
    {
        try
        {
            var html = await _httpClient.GetStringAsync(articleUrl, cancellationToken);
            var description = Regex.Match(html,
                "<meta\\b(?=[^>]*(?:name|property)=[\\\"'](?:description|og:description)[\\\"'])(?=[^>]*content=[\\\"'](?<text>[^\\\"']+)[\\\"'])[^>]*>",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var summary = description.Success ? CleanText(description.Groups["text"].Value) : string.Empty;

            if (summary.Length < 30)
            {
                var paragraphs = Regex.Matches(html, "<p\\b[^>]*>(?<text>.*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                summary = paragraphs.Select(match => CleanText(match.Groups["text"].Value))
                    .FirstOrDefault(text => text.Length >= 50) ?? summary;
            }

            if (summary.Length > 240)
                summary = summary[..summary.LastIndexOf(' ', 240)] + "…";

            return string.IsNullOrWhiteSpace(summary) ? null : summary;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    private static string CleanText(string value) =>
        WebUtility.HtmlDecode(Tags.Replace(value, " ")) is string text
            ? Regex.Replace(text, @"\s+", " ").Trim()
            : string.Empty;
}

public sealed record MunicipalNewsItem(string Title, string Source, string Url, string? Summary);
