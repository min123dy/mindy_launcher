using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CmlLib.Core.Utils;

namespace 마크런처
{
    //MIT License

    //Copyright(c) 2020 권세인(AlphaBs)

    //Permission is hereby granted, free of charge, to any person obtaining a copy
    //of this software and associated documentation files (the "Software"), to deal
    //in the Software without restriction, including without limitation the rights
    //to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    //copies of the Software, and to permit persons to whom the Software is
    //furnished to do so, subject to the following conditions:

    //The above copyright notice and this permission notice shall be included in all
    //copies or substantial portions of the Software.

    //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    //IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    //FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    //AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    //LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    //OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    //SOFTWARE.


    //MIT License

    //Copyright(c) 2023 CmlLib

    //Permission is hereby granted, free of charge, to any person obtaining a copy
    //of this software and associated documentation files (the "Software"), to deal
    //in the Software without restriction, including without limitation the rights
    //to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    //copies of the Software, and to permit persons to whom the Software is
    //furnished to do so, subject to the following conditions:

    //The above copyright notice and this permission notice shall be included in all
    //copies or substantial portions of the Software.

    //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    //IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    //FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    //AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    //LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    //OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    //SOFTWARE.
    public partial class Form3 : Form
    {
        public string Path { get; set; }
        string okey;
        string oValue;
        public Form3(string path)
        {
            this.MaximizeBox = false;
            this.Path = path;
            InitializeComponent();
        }
        GameOptionsFile? optionFile;

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                optionFile = GameOptionsFile.ReadFile(this.Path);
                foreach (var item in optionFile)
                {
                    var listViewItem = new ListViewItem(new[] { item.Key, item.Value });
                    listViewItem.Name = item.Key;  // Set the Name property to the key
                    listView1.Items.Add(listViewItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"먼저 마인크래프트를 설치하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void OpenPanel(string key, string value, bool enableKey)
        {
            pKeyValue.Visible = true;
            txtKey.Text = key;
            txtValue.Text = value;
            txtKey.Enabled = enableKey;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            var key = listView1.SelectedItems[0].Text;
            listView1.Items.RemoveByKey(key);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            var key = listView1.SelectedItems[0].Text;
            var value = listView1.SelectedItems[0].SubItems[1].Text;

            okey = key;  // Set original key
            oValue = value;  // Set original value

            OpenPanel(key, value, false);
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtKey.Enabled)
                {
                    var listViewItem = new ListViewItem(new[] { txtKey.Text, txtValue.Text });
                    listViewItem.Name = txtKey.Text;  // Set the Name property to the key
                    listView1.Items.Add(listViewItem);
                }
                else
                {
                    var item = listView1.Items.Find(okey, false).FirstOrDefault();
                    if (item != null)
                    {
                        item.SubItems[1].Text = txtValue.Text;
                    }
                }
                pKeyValue.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (optionFile == null)
            {
                MessageBox.Show("Load the option file first");
                return;
            }

            foreach (ListViewItem item in listView1.Items)
            {
                optionFile.SetRawValue(item.Text, item.SubItems[1].Text);
            }

            optionFile.Save();
            this.Close();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            pKeyValue.Visible = false;
        }

        private void btnReset_Click_1(object sender, EventArgs e)
        {
            txtKey.Text = okey;
            txtValue.Text = oValue;
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {

        }
    }
}