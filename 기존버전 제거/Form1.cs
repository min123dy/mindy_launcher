using System.Diagnostics;

namespace 기존버전_제거
{
    public partial class Form1 : Form
    {
        string productCode = "{0B6A63C4-8BD7-43F9-AD4F-5D886E8A16D7}";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                Process.Start("msiexec.exe", $"/x {productCode} /quiet /norestart").WaitForExit();
              MessageBox.Show("구버전 제거 완료");
            }
            catch (Exception ex)
            {
                Console.WriteLine("구버전 없음");
            }
        }
    }
}
