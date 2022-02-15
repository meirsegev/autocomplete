using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IAutoCompleteService : IHostedService
    {
        Task <List<string>> GetSuggestionsAsync(string prefix);
        Task InitializeAsync();
        bool IsInitialized();
    }
}
