using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface ITrieStructBuilderService
    {
        TrieNode BuildTrieStruct(List<string> words);

        // TBD, maybe export this method to a different interface
        List<string> GetSuggestionsForPrefix(TrieNode rootNode, string prefix);
    }
}
