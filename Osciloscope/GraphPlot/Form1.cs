using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace GraphPlot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Starting time in milliseconds
        int tickStart = 0;
        //http://zedgraph.dariowiz.com/index3061.html

        private void Form1_Load(object sender, EventArgs e)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;


            //GraphPane myPane1 = zedGraphControl1.GraphPane;
            //var list1 = new RollingPointPairList(1200);
            //LineItem myCurve = myPane1.AddCurve("Porsche", list1, Color.Red, SymbolType.None);



            myPane.Title.Text = "Test of Dynamic Data Update with ZedGraph\n" + "(After 25 seconds the graph scrolls)";
            myPane.XAxis.Title.Text = "Time, Seconds";
            myPane.YAxis.Title.Text = "Sample Potential, Volts";

            // Save 1200 points.  At 50 ms sample rate, this is one minute
            // The RollingPointPairList is an efficient storage class that always
            // keeps a rolling set of point data without needing to shift any data values
            var list = new RollingPointPairList(1200);

            // Initially, a curve is added with no data points (list is empty)
            // Color is blue, and there will be no symbols
            LineItem curve = myPane.AddCurve("Voltage", list, Color.Blue, SymbolType.None);
       
            curve.IsY2Axis = true;

            // Sample at 50ms intervals
            timer1.Interval = 50;
            timer1.Enabled = true;
            timer1.Start();

            // Just manually control the X axis range so it scrolls continuously
            // instead of discrete step-sized jumps
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 30;
            myPane.XAxis.Scale.MinorStep = 1;
            myPane.XAxis.Scale.MajorStep = 5;

            // Scale the axes
            zedGraphControl1.AxisChange();

            // Save the beginning time for reference
            tickStart = Environment.TickCount;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Make sure that the curvelist has at least one curve
            if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                return;

            // Get the first CurveItem in the graph
            LineItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
            if (curve == null)
                return;


            // Get the PointPairList
            IPointListEdit list = curve.Points as IPointListEdit;
            // If this is null, it means the reference at curve.Points does not
            // support IPointListEdit, so we won't be able to modify it
            if (list == null)
                return;

            // Time is measured in seconds
            double time = (Environment.TickCount - tickStart) / 1000.0;

            // 3 seconds per cycle
            list.Add(time, Math.Sin(2.0 * Math.PI * time / 3.0));
            

            // Keep the X scale at a rolling 30 second interval, with one
            // major step between the max X value and the end of the axis
            Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
            if (time > xScale.Max - xScale.MajorStep)
            {
                xScale.Max = time + xScale.MajorStep;
                xScale.Min = xScale.Max - 30.0;
            }


            //========Linea horizontal Marker============

     
            var threshHoldLine = new LineObj(Color.Red, 0, 1, time , 1);
            zedGraphControl1.GraphPane.GraphObjList.Add(threshHoldLine);

            //=============================================


            // Make sure the Y axis is rescaled to accommodate actual data
            zedGraphControl1.AxisChange();
            // Force a redraw
            zedGraphControl1.Invalidate();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        // Set the size and location of the ZedGraphControl
        private void SetSize()
        {
            // Control is always 10 pixels inset from the client rectangle of the form
            Rectangle formRect = this.ClientRectangle;
            formRect.Inflate(-10, -10);

            if (zedGraphControl1.Size != formRect.Size)
            {
                zedGraphControl1.Location = formRect.Location;
                zedGraphControl1.Size = formRect.Size;
            }
        }
    }
}
