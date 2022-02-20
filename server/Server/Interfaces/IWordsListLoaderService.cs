using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IWordsListLoaderService
    {
        Task<List<string>> LoadInitialWordsListAsync(CancellationToken ct);
    }
}
