using E1Validation.Lib.Models;
using E1Validation.Lib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E1Validation.Desktop
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            RefreshTreeView();
        }

        private void RefreshTreeView()
        {
            //Create a header Node for conversions
            TreeNode conversionNode = treeView1.Nodes.Add("Conversions");

            //Get the list of conversions from the database
            IList<Conversion> conversions = ConfigurationData.GetConversions();

            //Put these into an array
            foreach (Conversion c in conversions)
            {
                TreeNode newNode = new TreeNode();
                newNode.Text = c.DisplayName;
                conversionNode.Nodes.Add(newNode);
            }

        }



    }
}
