namespace mindy_launcher
{
    partial class LICENSE_Load
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LICENSE_Load));
            CloseBtn = new Button();
            LICENSE_textBox = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // CloseBtn
            // 
            CloseBtn.Location = new Point(271, 707);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(75, 23);
            CloseBtn.TabIndex = 0;
            CloseBtn.Text = "닫기";
            CloseBtn.UseVisualStyleBackColor = true;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // LICENSE_textBox
            // 
            LICENSE_textBox.Location = new Point(12, 65);
            LICENSE_textBox.Multiline = true;
            LICENSE_textBox.Name = "LICENSE_textBox";
            LICENSE_textBox.ReadOnly = true;
            LICENSE_textBox.ScrollBars = ScrollBars.Vertical;
            LICENSE_textBox.Size = new Size(585, 636);
            LICENSE_textBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label1.Location = new Point(78, 9);
            label1.Name = "label1";
            label1.Size = new Size(457, 50);
            label1.TabIndex = 2;
            label1.Text = "mindy_launcher LICENSE";
            // 
            // LICENSE_Load
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(611, 742);
            Controls.Add(label1);
            Controls.Add(LICENSE_textBox);
            Controls.Add(CloseBtn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "LICENSE_Load";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LICENSE";
            Load += LICENSE_Load_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CloseBtn;
        private TextBox LICENSE_textBox;
        private Label label1;
    }
}