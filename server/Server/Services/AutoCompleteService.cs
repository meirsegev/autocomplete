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

        // TBD, move the trieNode to something like "TrieBuilderAndSearcherService"
        // which is a singleton and holds the one and only trieStruct
        private TrieNode _rootNode = null;
        private ILogger<AutoCompleteService> _logger;
        private IWordsListLoaderService _wordsListLoaderService;
        private ITrieStructBuilderService _trieStructBuilderService;

        public AutoCompleteService(ILogger<AutoCompleteService> logger, 
            IWordsListLoaderService wordsListLoaderService,
            ITrieStructBuilderService trieStructBuilderService)
        {
            _logger = logger;
            _wordsListLoaderService = wordsListLoaderService;
            _trieStructBuilderService = trieStructBuilderService;
        }

        public List<string> GetSuggestions(string prefix)
        {
            if (!_isInitialized)
            {
                _logger.LogError("GetSuggestions was called before auto complete service initialization");
                return new List<string>();
            }
            return _trieStructBuilderService.GetSuggestionsForPrefix(_rootNode, prefix);
        }

        public async Task InitializeAsync(CancellationToken ct)
        {
            _logger.LogDebug("Initialization of AutoCompleteService started ...");

            if (_isInitialized)
                return;
                        
            try
            {
                // TBD, validate the "words" returned from the initial words service
                var words = await _wordsListLoaderService.LoadInitialWordsListAsync(ct);
                _rootNode = _trieStructBuilderService.BuildTrieStruct(words);
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Exception via InitializeAsync: {ex.StackTrace}");
                throw ex;
            }
            
            _logger.LogDebug("AutoCompleteService initialized");
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
