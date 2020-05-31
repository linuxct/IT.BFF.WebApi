using System;
using Telegraph.Net.Models;

namespace IT.BFF.Domain.Contracts
{
    public static class TelegraphExtensions
    {
        public static TelegraphSimplePage ToSimpleTelegraphDto(this Page page, DateTimeOffset date)
        {
            return new TelegraphSimplePage
            {
                Date = date,
                Title = page.Title.ToTitleWithoutDate(),
                PageContents = page.Content.ToTextArray()
            };
        }
    }
}