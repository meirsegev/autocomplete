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

        public List<string> GetSuggestions(string prefix)
        {
            if (!_isInitialized)
            {
                _logger.LogError("GetSuggestions was called before auto complete service initialization");
                return new List<string>();
            }
            return TrieStruct.SearchWord(_trieNode, prefix);
        }

        public async Task InitializeAsync(CancellationToken ct)
        {
            _logger.LogDebug("Initialization of AutoCompleteService started ...");

            if (_isInitialized)
                return;
                        
            var words = await GetInitialWordsListAsync(ct);
            _trieNode = TrieStruct.BuildTrieStruct(words);
            
            _isInitialized = true;
            _logger.LogDebug("AutoCompleteService initialized");
        }

        private async Task<List<string>> GetInitialWordsListAsync(CancellationToken ct)
        {
            // in real implementation this should be an asnyc read from db of
            // all city names and priority, here just from a file.
            var cities = await CsvParser.GetAllCitiesAsync(ct);
            return cities.Select(v => v.Name).ToList();
        }

        // This method is called before the server start serving requests,
        // So at the moment the server start serving requests it will be
        // ready to serve the auto-complete callss
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
