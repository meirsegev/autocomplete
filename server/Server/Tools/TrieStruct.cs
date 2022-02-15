using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Tools
{
    public class TrieNode
    {
        public TrieNode() { }
        public Dictionary<char, TrieNode> InnerNodes = new Dictionary<char, TrieNode>();
        public List<string> Words = new List<string>();
        public bool ContainsKey(char key) => InnerNodes.ContainsKey(key);
        public TrieNode GetNodeByKey(char key) => InnerNodes[key];
        public void AddWord(string word)
        {
            Words.Add(word);
        }

        public static TrieNode BuildTrieStruct(List<string> words)
        {
            var rootNode = new TrieNode();

            foreach (var word in words)
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

        public static List<string> SearchWord(TrieNode rootNode, string word)
        {
            if (string.IsNullOrEmpty(word))
                return new List<string>();

            var iterableNode = rootNode;

            foreach (var c in word)
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
