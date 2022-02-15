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
        public bool IsInitialized() => _isInitialized;
        private bool _isInitialized = false;
        private TrieNode _trieNode = null;
        private ILogger<AutoCompleteService> _logger;

        public AutoCompleteService(ILogger<AutoCompleteService> logger)
        {
            _logger = logger;
        }

        public async Task<List<string>> GetSuggestionsAsync(string prefix)
        {
            if (!_isInitialized)
                return new List<string>();

            var t = Task.Run(() => TrieNode.SearchWord(_trieNode, prefix));
            await t;
            return t.Result;
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            // TBD - make this function thread safe for case that multiple users 
            // connected in the same time.
            var t = Task.Run(() =>
            {
                var words = GetInitialWordsList();
                _trieNode = TrieNode.BuildTrieStruct(words);
            });
            await t;
            _isInitialized = true;
        }

        private List<string> GetInitialWordsList()
        {
            var cities = City.GetAllCities();
            return cities.Select(v => v.Name).ToList();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // nothing to do.
        }
    }
}
