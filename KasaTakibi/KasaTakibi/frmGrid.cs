using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication12
{
    public partial class frmGrid : Form
    {
        public DataGridView lgvgriddata;
        public DataGridView lgvgriddata2;
        public frmGrid()
        {
            InitializeComponent();
            lgvgriddata = datagrid;
            lgvgriddata2 = datagrid1;
        }

        private void denemesql_Load(object sender, EventArgs e)
        {
            

        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
         

        }
    }
}
