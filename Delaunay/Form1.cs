using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace indiv_9
{
    public partial class Form1 : Form
    {
        private List<PointF> points = new List<PointF>();
        private List<(PointF, PointF, PointF)> triangles = new List<(PointF, PointF, PointF)>();
        private HashSet<PointF> usedPoints = new HashSet<PointF>(); // Отслеживаем уже добавленные точки

        public Form1()
        {
            InitializeComponent();
            pictureBoxField.Paint += PictureBoxField_Paint;
            pictureBoxField.MouseClick += PictureBoxField_MouseClick;
            btnTriangulate.Click += BtnTriangulate_Click;
            btnClear.Click += BtnClear_Click;
        }

        private void PictureBoxField_MouseClick(object sender, MouseEventArgs e)
        {
            points.Add(new PointF(e.X, e.Y));
            pictureBoxField.Invalidate();
        }

        private void BtnTriangulate_Click(object sender, EventArgs e)
        {
            if (points.Count < 3)
            {
                MessageBox.Show("Please add at least three points for triangulation.");
                return;
            }

            triangles.Clear();
            usedPoints.Clear(); // Очищаем перед новой триангуляцией
            List<Edge> liveEdges = new List<Edge>();

            // Шаг 1: Найти первое начальное ребро с выпуклой оболочки
            var hull = ConvexHull(points);
            if (hull.Count < 2)
            {
                MessageBox.Show("Could not initialize triangulation with the given points.");
                return;
            }

            // Взять первое ребро из выпуклой оболочки
            var firstEdge = new Edge(hull[1], hull[0]);
            liveEdges.Add(firstEdge);
            usedPoints.Add(hull[1]);
            usedPoints.Add(hull[0]);

            // Основной цикл Делоне
            while (liveEdges.Count > 0)
            {
                var edge = liveEdges[0];

                // Найти лучшую точку справа от текущего ребра для создания треугольника
                PointF? bestPoint = FindBestPointForEdge(edge);

                if (bestPoint != null)
                {
                    AddTriangle(edge, bestPoint, liveEdges);
                }else
                {
                    liveEdges.RemoveAt(0);
                }
            }
            //pictureBoxField.Invalidate();
        }
        private void AddTriangle(Edge edge, PointF? bestPoint, List<Edge> liveEdges)
        {
            var triangle = (edge.Start, edge.End, bestPoint.Value);
            triangles.Add(triangle);
            usedPoints.Add(bestPoint.Value); 
            UpdateLiveEdges(edge, liveEdges, triangle);
            pictureBoxField.Refresh();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            points.Clear();
            triangles.Clear();
            pictureBoxField.Invalidate();
        }

        private void PictureBoxField_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;

            foreach (var point in points)
            {
                graphics.FillRectangle(Brushes.Black, point.X, point.Y, 2, 2);
            }

            foreach (var (p1, p2, p3) in triangles)
            {
                graphics.DrawLine(Pens.Blue, p1, p2);
                graphics.DrawLine(Pens.Blue, p2, p3);
                graphics.DrawLine(Pens.Blue, p3, p1);
            }
        }

        private List<PointF> ConvexHull(List<PointF> points)
        {
            var hull = new List<PointF>();
            var start = points.OrderBy(p => p.X).ThenBy(p => p.Y).First();
            var current = start;

            do
            {
                hull.Add(current);
                var next = points[0];

                foreach (var p in points)
                {
                    if (p == current) continue;
                    float crossProduct = CrossProduct(current, next, p);
                    if (next == current || crossProduct > 0 || (crossProduct == 0 && Distance(current, p) > Distance(current, next)))
                        next = p;
                }

                current = next;
            } while (current != start);

            return hull;
        }

        private float CrossProduct(PointF a, PointF b, PointF c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        private float Distance(PointF a, PointF b)
        {
            return (b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y);
        }

        private PointF? FindBestPointForEdge(Edge edge)
        {
            PointF? bestPoint = null;
            double minRadius = double.MaxValue;

            foreach (var point in points)
            {
                // Пропустить точку, если она уже использована
                //if (usedPoints.Contains(point) || point == edge.Start || point == edge.End) continue;

                // Проверка, что точка находится справа от ребра
                if (!ClassifyPoint(point, edge)) continue;

                double radius = CalculateCircumcenterDistance(edge, point);

                if (radius < minRadius)
                {
                    minRadius = radius;
                    bestPoint = point;
                }
            }

            return bestPoint;
        }

        private double CalculateCircumcenterDistance(Edge edge, PointF point)
        {
            float ax = edge.Start.X, ay = edge.Start.Y;
            float bx = edge.End.X, by = edge.End.Y;
            float cx = point.X, cy = point.Y;

            float d = 2 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));
            if (Math.Abs(d) < 1e-6) return double.MaxValue;

            float ux = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
            float uy = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;
            PointF cntr = new PointF(ux, uy);
            if (!ClassifyPoint(cntr, edge))
                return -Distance(new PointF(ux, uy), edge.Start);
            else
                return Distance(new PointF(ux, uy), edge.Start);
        }

        /*private bool IsPointRightOfEdge(Edge edge, PointF point)
        {
            // Перенос ребра к началу координат
            float xA = edge.End.X - edge.Start.X;
            float yA = edge.End.Y - edge.Start.Y;
            float xB = point.X - edge.Start.X;
            float yB = point.Y - edge.Start.Y;

            return yB * xA - xB * yA < 0;
        }*/
        public static bool ClassifyPoint(PointF p, Edge edge)
        {
            double result = (edge.End.X - edge.Start.X) * (p.Y - edge.Start.Y) - (edge.End.Y - edge.Start.Y) * (p.X - edge.Start.X);

            if (result > 0)
                return true;
            else if (result < 0)
                return false;
            else
                return false;
        }

        private void UpdateLiveEdges(Edge edge, List<Edge> liveEdges, (PointF, PointF, PointF) triangle)
        {
            var edges = new[]
            {
                new Edge(triangle.Item1, triangle.Item2),
                new Edge(triangle.Item3, triangle.Item2),
                new Edge(triangle.Item1, triangle.Item3)
            };

            foreach (var e in edges)
            {
                if (liveEdges.Any(le => le.Equals(e)))
                    liveEdges.Remove(liveEdges.First(le => le.Equals(e)));
                else
                    liveEdges.Add(e);
            }
        }
    }

    public struct Edge
    {
        public PointF Start;
        public PointF End;

        public Edge(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Edge other))
                return false;

            return (Start == other.Start && End == other.End) ||
                   (Start == other.End && End == other.Start);
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }
    }
}
