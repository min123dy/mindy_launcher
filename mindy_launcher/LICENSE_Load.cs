using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mindy_launcher
{
    public partial class LICENSE_Load : Form
    {
        public LICENSE_Load()
        {
            InitializeComponent();
        }
        private void LICENSE_Load_Load(object sender, EventArgs e)
{
            try
            {

            
            string filePath = Path.Combine(AppContext.BaseDirectory, "LICENSE.txt");
string[] lines = File.ReadAllLines(filePath);
LICENSE_textBox.Lines = lines; 
            }
             catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
}


        private void CloseBtn_Click(object sender, EventArgs e)
{
    
    this.Close(); 
}
    }
}
