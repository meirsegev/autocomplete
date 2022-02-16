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
    }
}
