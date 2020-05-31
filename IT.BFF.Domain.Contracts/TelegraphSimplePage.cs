using System;
using Telegraph.Net.Models;

namespace IT.BFF.Domain.Contracts
{
    [Serializable]
    public class TelegraphSimplePage
    {
        public DateTimeOffset Date { get; set; }
        public string Title { get; set; }
        public string[] PageContents { get; set; }
    }

    
}