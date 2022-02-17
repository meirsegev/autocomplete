using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IAutoCompleteService : IHostedService
    {
        List<string> GetSuggestions(string prefix);
        Task InitializeAsync(CancellationToken ct);
    }
}
