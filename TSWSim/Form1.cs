using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace TSWSim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            { // CP chart
                var sim = new Simulator();
                var plot = new PlotView { Dock = DockStyle.Fill,  };
                var model = new PlotModel { Title = "Combat Power by Attack Rating" };
                foreach (var wp in new[] { 510, 475, 457, 446 })
                {
                    var series = new LineSeries { Title = wp.ToString() };
                    for (var ar = 1; ar < 6500; ++ar)
                        series.Points.Add(new DataPoint(ar, Stats.CalculateCombatPower(ar, wp)));
                    model.Series.Add(series);
                }
                plot.Model = model;
                tpCombatPower.Controls.Add(plot);
            }
        }
    }
}
