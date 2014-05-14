using System.Drawing;

namespace MoveLines
{
    public class GraphLine
    {
        public GraphLine(float x1, float y1, float x2, float y2)
        {
            this.StartPoint = new PointF(x1, y1);
            this.EndPoint = new PointF(x2, y2);
        }
        public PointF StartPoint;
        public PointF EndPoint;
    }
}
