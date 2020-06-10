using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IT.BFF.Domain.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegraph.Net;
using Telegraph.Net.Models;

namespace IT.BFF.Infra.TelegraphConnector
{
    public class TelegraphConnector : ITelegraphConnector
    {
        private readonly IConfiguration _configuration;
        private readonly Telegraph.Net.TelegraphClient _client;
        private readonly ITokenClient _secureClient;
        private readonly ILogger<TelegraphConnector> _logger;

        public TelegraphConnector(ILogger<TelegraphConnector> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new Telegraph.Net.TelegraphClient();
            _secureClient = _client.GetTokenClient(_configuration.GetSection("TelegraphApiKey").Value);
            _logger = logger;
        }
        
        public async Task<TelegraphSimplePage> RetrieveTodaysPost(DateTimeOffset dateTimeOffset)
        {
            Page result = null;
            var hasResultsPendingToCheck = true;
            var currentOffset = 0;

            try
            {
                while (hasResultsPendingToCheck)
                {
                    var pageList = await _secureClient.GetPageListAsync(offset: currentOffset, limit: 50);
                    if (pageList.Pages.Count == 50)
                        currentOffset += 50;
                    else
                    {
                        hasResultsPendingToCheck = false;
                        while (pageList.Pages.All(x => x.Title.FromTitleWithDate() != null))
                        {
                            currentOffset -= 50;
                            pageList = await _secureClient.GetPageListAsync(offset: currentOffset, limit: 50);
                        }
                    }

                    var (item1, item2) = Result(dateTimeOffset, pageList, hasResultsPendingToCheck);
                    result = item1;
                    hasResultsPendingToCheck = !item2 && hasResultsPendingToCheck;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("FLOOD_WAIT_"))
                {
                    _logger.LogError("Error while retrieving the posts from the API {0}, waiting 30 seconds.", e.Message);
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                } 
                else if (e.Message.Contains("ERROR"))
                {
                    _logger.LogError("The API returned an error {0}.", e.Message);
                }
            }

            if (result == null) return null;
            var pageContents = await _client.GetPageAsync(result.Path, true);
            return pageContents.ToSimpleTelegraphDto(dateTimeOffset.Date);
        }

        private (Page, bool) Result(DateTimeOffset dateTimeOffset, PageList pageList, bool hadResultsPendingToCheck)
        {
            Page result = null;
            var success = false;
            foreach (var page in pageList.Pages)
            {
                var dto = page.Title.FromTitleWithDate();
                if (dto != null && DateTimeOffset.Compare(dto.Value.Date, dateTimeOffset) == 0)
                {
                    result = page;
                    success = true;
                    break;
                }

                if (dto == null)
                {
                    result = page;
                }
            }

            if (!success && !hadResultsPendingToCheck)
            {
                UpdatePageAfterChoosingTodaysPost(result, dateTimeOffset);
            }

            return (result, success);
        }

        private async void UpdatePageAfterChoosingTodaysPost(Page result, DateTimeOffset dateTimeOffset)
        {
            // TODO REHACER
            var newTitle = result.Title.ToTitleWithDate(dateTimeOffset);
            var fullPage = await _client.GetPageAsync(result.Path, true);
            await _secureClient.EditPageAsync(result.Path, newTitle, fullPage.Content.ToArray());
        }
    }
}