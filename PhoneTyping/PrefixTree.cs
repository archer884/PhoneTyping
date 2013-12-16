using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneTyping
{
    public class Node
    {
        public IDictionary<char, Node> Descendants { get; set; }
        public char Key { get; set; }

        public Node()
        {
            Descendants = new Dictionary<char, Node>();
        }

        public Node(char key)
            : this()
        {
            Key = key;
        }
    }

    public class PrefixTree : IEnumerable<string>
    {
        public IDictionary<char, Node> Root { get; set; }

        public PrefixTree() 
        {
            Root = new Dictionary<char, Node>();
        }

        public void Add(string key)
        {
            if (!Root.ContainsKey(key.First()))
                Root.Add(key.First(), new Node(key.First()));
            
            AddKey(Root[key.First()], key.Skip(1));
        }

        public void AddKey(Node node, IEnumerable<char> key)
        {
            var currentKey = key.First();
            
            if (!node.Descendants.ContainsKey(currentKey))
                node.Descendants.Add(currentKey, new Node(currentKey));

            if (key.Skip(1).Any())
                AddKey(node.Descendants[currentKey], key.Skip(1));
        }

        #region ienumerable
        public IEnumerable<IEnumerable<Node>> GetDescendantNodes(Node node)
        {
            foreach (var descendantNode in node.Descendants)
            {
                yield return descendantNode.Value.Descendants;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            var product = Root
                .Select(node => GetDescendantNodes(node.Value).Select(n => n.Key))
                .CartesianProduct();

            foreach (var sequence in product)
            {
                yield return sequence.AsString();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        } 
        #endregion
    }
}
