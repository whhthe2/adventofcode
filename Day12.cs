using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventofcode2021
{
    internal static class Day12
    {
        public static HashSet<Node> nodes;
        private static List<Stack<Node>> paths = new List<Stack<Node>>();
        private static Stack<Node> currentPath = new Stack<Node>();

        private static Node end;

        public static void Solve(string input)
        {
            PuzzleInput.RowSeparator = "\n";
            PuzzleInput.ColSeparator = "-";
            nodes = new HashSet<Node>();
            PuzzleInput.ParseHashSet<Node>(input, CreateNodes);

            Node start; 
            if (!nodes.TryGetValue(new Node("start"), out start))
            {
                throw new Exception("Starting Node not found.");
            }
            if (!nodes.TryGetValue(new Node("end"), out end))
            {
                throw new Exception("Ending Node not found.");
            }

            RecursiveDFS(start);

            if (paths.Count > 0)
            {
                foreach (var p in paths)
                {
                    foreach (var n in p)
                    {
                        Console.Write($"{n} ");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                throw new Exception("no paths found!");
            }
            
        }

        public static void RecursiveDFS(Node current)
        {
            Node actualNode;
            if (!nodes.TryGetValue(current, out actualNode))
            {
                throw new Exception ($"Failed to find node {current} in {nodes}");
            }

            current = actualNode;
            currentPath.Push(current);

            if (current == end)
            {
                paths.Add(new Stack<Node>(currentPath.ToArray()));
            }

            else
            {
                foreach (var conn in current.Connections)
                {
                    if (conn.Big || !currentPath.Contains(conn))
                    {
                        RecursiveDFS(conn);
                    }
                }
            }
            currentPath.Pop();
        }

        public static Node CreateNodes(string[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception($"Incorrect number of arguments provided to node factory: {args.Length}. Expecting 2 arguments.");
            }
            var name = args[0];
            var connection = args[1];

            var tempNode = new Node(name);
            Node node;
            if (!nodes.TryGetValue(tempNode, out node) )
            {
                node = tempNode;
            }

            var tempTarget = new Node(connection);
            Node target;
            if (!nodes.TryGetValue(tempTarget, out target) )
            {
                target = tempTarget;
            }

            node.Connect(target);
            target.Connect(node);

            nodes.Add(node);
            nodes.Add(target);

            return node;
        }
        public class Node : IEquatable<Node>
        {
            public string Name;
            public bool Big;
            private HashSet<Node> connections;
            public HashSet<Node> Connections
            {
                get => connections;
            }

            public Node(string name)
            {
                Name = name;
                Big = name.ToUpper() == name;
                connections = new HashSet<Node>();
            }

            public bool Connect(Node target)
            {
                return connections.Add(target);
            }

            #nullable enable
            public bool Equals(Node? node)
            {
                if (node == null)
                {
                    return false;
                }

                if (Name == node.Name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            #nullable disable

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                
                return Equals(obj as Node);
            }
            
            public override int GetHashCode()
            {                
                return Name.GetHashCode();
            }
            
            public override string ToString()
            {
                return Name;
            }
        }
        
    }
}