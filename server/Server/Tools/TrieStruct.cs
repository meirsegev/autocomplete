using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Tools
{
    public class TrieStruct
    {
        public static TrieNode BuildTrieStruct(List<string> words)
        {
            var rootNode = new TrieNode();

            foreach (var word in words?? new List<string>())
            {
                var iterableNode = rootNode;
                var wordAsLower = word.ToLower();
                foreach (var charVal in wordAsLower)
                {
                    if (iterableNode.ContainsKey(charVal))
                    {
                        iterableNode.GetNodeByKey(charVal).Words.Add(word);
                        iterableNode = iterableNode.InnerNodes[charVal];
                    }
                    else
                    {
                        var trieNode = new TrieNode();
                        trieNode.AddWord(word);
                        iterableNode.InnerNodes[charVal] = trieNode;
                        iterableNode = iterableNode.InnerNodes[charVal];
                    }
                }
            }
            return rootNode;
        }

        public static List<string> GetSuggestionsForPrefix(TrieNode rootNode, string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return new List<string>();

            var iterableNode = rootNode;

            foreach (var c in prefix)
            {
                if (iterableNode.ContainsKey(c))
                    iterableNode = iterableNode.GetNodeByKey(c);
                else
                    return new List<string>();
            }

            return iterableNode.Words;
        }
    }
}
