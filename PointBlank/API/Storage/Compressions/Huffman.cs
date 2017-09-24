using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Storage.Compressions
{
#if DEBUG
    internal static class Huffman
    {
        #region Public Functions
        public static byte[] Compress(string text)
        {
            List<Node> nodes = text.GroupBy(a => a).Select(a => new Node(a.Key, a.Count())).ToList();
            int highestNumber = 0;

            while(highestNumber < text.Length || nodes.Count != 1)
            {
                nodes = nodes.OrderByDescending(a => a.Frequency).ToList();
                Node[] temp = nodes.Where(a => !a.Used).ToArray();
                Node left = temp[0];
                Node right = temp[1];
                Node parent = new Node(left.Frequency + right.Frequency, left, right);

                left.Parent = parent;
                right.Parent = parent;
                nodes.Add(parent);
                highestNumber = parent.Frequency;
                left.Used = true;
                right.Used = true;
            }

            return null;
        }

        public static byte[] Decompress(string text)
        {
            return null;
        }
        #endregion

        #region Private Functions
        private static string GenerateHeader(List<Node> nodes)
        {
            string header = "";
            Node[] nodeArr = nodes.Where(a => a.Left == null && a.Right == null && a.Parent != null).ToArray(); ;

            header += (char)(nodes.Count * 2); // Header size
            foreach(Node node in nodes)
            {

            }

            return header;
        }
        #endregion

        #region Sub Classes
        private class Node
        {
            #region Properties
            public char Character { get; private set; }
            public int Frequency { get; private set; }

            public Node Left { get; set; }
            public Node Right { get; set; }
            public Node Parent { get; set; }
            public bool Used { get; set; } = false;
            #endregion

            public Node(char character, int frequency)
            {
                // Set the variables
                this.Character = character;
                this.Frequency = frequency;
            }

            public Node(int frequency, Node left, Node right)
            {
                // Set the variables
                this.Frequency = frequency;
                this.Left = left;
                this.Right = right;
            }
        }
        #endregion
    }
#endif
}
