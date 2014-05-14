using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MSChart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Senosoidal();
        }

        private void GenerateValues()
        {
            // Fill series data
            double yValue = 50.0;
            Random random = new Random();
            for (int pointIndex = 0; pointIndex < 20000; pointIndex++)
            {
                yValue = yValue + (random.NextDouble() * 10.0 - 5.0);
                chart1.Series["Series1"].Points.AddY(yValue);
            }

            // Set fast line chart type
            chart1.Series["Series1"].ChartType = SeriesChartType.FastLine;
        }

        int tickStart = 0;
        private void Senosoidal()
        {
            //time, Math.Sin(2.0 * Math.PI * time / 3.0)

            //chart1.Series["Series1"].Points.AddY(Math.Sin(2.0 * Math.PI * time / 3.0));


            double time = (Environment.TickCount - tickStart) / 1000.0;
            chart1.Series["Series1"].Points.AddXY(time, Math.Sin(2.0 * Math.PI * time / 3.0));

            chart1.Series["Series1"].ChartType = SeriesChartType.Column;

            chart1.Series["Series1"].ChartType = SeriesChartType.Spline;
            //chart1.Series["Series1"].IsValueShownAsLabel = true;
                      
            chart1.Series["Series1"].Color = System.Drawing.Color.DarkBlue;
         

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tickStart = Environment.TickCount;
        }
    }
}
