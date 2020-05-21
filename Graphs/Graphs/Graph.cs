using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs {
    public class Graph {
        public List<Vertex> Vertices { get; private set; }
        public List<WeightedEdge> Edges { get; private set; }

        public Graph(List<Vertex> vertices, List<WeightedEdge> edges) {
            this.Edges = edges;
            this.Vertices = vertices;
            foreach (Vertex vertex in Vertices) {
                vertex.Graph = this;
            }
            Vertex<int>.Count = 0;
        }
        public Graph(double[,] matrix) {
            Vertices = new List<Vertex>();
            Edges = new List<WeightedEdge>();
            for (int i = 0; i < matrix.GetLength(0); i++) {
                Vertices.Add(new Vertex<int>(i));
                Vertices[i].Graph = this;
            }
            for (int i = 0; i < matrix.GetLength(0); i++) {
                for (int j = 0; j < matrix.GetLength(0); j++) {
                    if (Math.Abs(matrix[i, j]) > 1e-5) this.Edges.Add(new WeightedEdge(Vertices[i], Vertices[j], matrix[i, j]));
                }
            }

        }


        public void AddEdge(Vertex from, Vertex to, double weight) {
            this.Edges.Add(new WeightedEdge(from, to, weight));
        }
        public void AddVertex(Vertex vertex) {
            this.Vertices.Add(vertex);
            vertex.Graph = this;
        }
        public void RemoveEdge(WeightedEdge edge) {
            this.Edges.Remove(edge);
        }
        public void RemoveVertex(Vertex vertex) {
            this.Edges.RemoveAll(edge => edge.From == vertex || edge.To == vertex);
            this.Vertices.Remove(vertex);
        }

        public double SumToOtherVerts(int start, int finish) {
            double[] distances = new double[Vertices.Count];
            bool[] visitedVertices = new bool[Vertices.Count];
            for (int i = 0; i < distances.Length; i++) {
                distances[i] = Double.MaxValue;
                visitedVertices[i] = false;
            }
            distances[start] = 0;
            while (visitedVertices.Contains(false)) {
                int minVert = 0;
                for (int i = 0; i < visitedVertices.Length; i++) {
                    if (!visitedVertices[i]) {
                        minVert = i;
                        break;
                    }
                }
                for (int i = 0; i < distances.Length; i++) {
                    if ((distances[minVert] > distances[i]) && (!visitedVertices[i])) {
                        minVert = i;
                    }
                }
                visitedVertices[minVert] = true;
                foreach (WeightedEdge edge in this.Vertices[minVert].OutboundEdges) {
                    int u = edge.To.Index;
                    if (!visitedVertices[u] && distances[u] > distances[minVert] + edge.Weight) {
                        distances[u] = distances[minVert] + edge.Weight;
                        Vertices[u].ShortWay.Clear();
                        foreach (Vertex vertex in Vertices[minVert].ShortWay) {
                            Vertices[u].ShortWay.Add(vertex);
                        }
                        Vertices[u].ShortWay.Add(Vertices[minVert]);
                    }
                }
            }
            for(int i = 0; i < distances.Length; i++){
                if (Math.Abs(distances[i]) >= Double.MaxValue-10) distances[i] = 0;// Нет пути
            }

            return distances.Sum();
            
        }
        public override string ToString() {
            var vertices = this.Vertices.Aggregate("", (current, vertex) => current + vertex.ToString() + ' ');
            var edges = this.Edges.Aggregate("", (current, edge) => current + edge.ToString() + '\n');
            return "Vertices: " + vertices + '\n' + "Edges:" + '\n' + edges;
        }
    }

    public abstract class Vertex {
        public Graph Graph { get; internal set; }
        public List<Vertex> ShortWay { get; set; }
        public List<WeightedEdge> OutboundEdges {
            get { return Graph.Edges.Where(e => e.From == this).ToList(); }
        }
        public List<WeightedEdge> InboundEdges {
            get { return Graph.Edges.Where(e => e.To == this).ToList(); }
        }

        public int Index { get; protected set; }
    }
    public class Vertex<T> : Vertex {
        public T Value { get; private set; }

        public Vertex(T value) {
            ShortWay = new List<Vertex>();
            Index = Count;
            Count++;
            this.Value = value;
        }
        public override string ToString() {
            return this.Value.ToString();
        }
        public static int Count = 0;
    }
    public class Edge {
        public Vertex From { get; private set; }
        public Vertex To { get; private set; }

        public Edge(Vertex from, Vertex to) {
            this.From = from;
            this.To = to;
        }
        public override string ToString() {
            return $"{this.From} -> {this.To}";
        }
    }
    public class WeightedEdge : Edge {
        public double Weight { get; private set; }

        public WeightedEdge(Vertex from, Vertex to, double weight) : base(from, to) {
            this.Weight = weight;
        }
        public override string ToString() {
            return $"{this.From} -> {this.To} [{this.Weight}]";
        }
    }

}