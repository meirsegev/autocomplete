using Server.Interfaces;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class TrieStructBuilderService : ITrieStructBuilderService
    {
        public TrieNode BuildTrieStruct(List<string> words)
        {
            return TrieStruct.BuildTrieStruct(words);
        }

        public List<string> GetSuggestionsForPrefix(TrieNode rootNode, string prefix)
        {
            return TrieStruct.GetSuggestionsForPrefix(rootNode, prefix);
        }
    }
}
