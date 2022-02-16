using Microsoft.Extensions.Logging;
using Server.Interfaces;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    public class AutoCompleteService : IAutoCompleteService
    {
        private bool _isInitialized = false;
        private TrieNode _trieNode = null;
        private ILogger<AutoCompleteService> _logger;

        public AutoCompleteService(ILogger<AutoCompleteService> logger)
        {
            _logger = logger;
        }

        public async Task<List<string>> GetSuggestionsAsync(string prefix, CancellationToken ct)
        {
            if (!_isInitialized)
                return new List<string>();

            var t = Task.Run(() => TrieStruct.SearchWord(_trieNode, prefix), ct);
            await t;

            return t.Result;
        }

        public async Task InitializeAsync(CancellationToken ct)
        {
            if (_isInitialized)
                return;

            // TBD - make this function thread safe for case that multiple users 
            // connected in the same time.
            var t = Task.Run(() =>
            {
                var words = GetInitialWordsList();
                _trieNode = TrieStruct.BuildTrieStruct(words);
            }, ct);

            await t;

            _isInitialized = true;
        }

        // TBD - connect to db with async call
        private List<string> GetInitialWordsList()
        {
            var cities = CsvParser.GetAllCities();
            return cities.Select(v => v.Name).ToList();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // nothing to do.
        }
    }
}
