namespace 마크런처
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            pKeyValue = new Panel();
            btnReset = new Button();
            btnCancel = new Button();
            btnOk = new Button();
            txtValue = new TextBox();
            label3 = new Label();
            txtKey = new TextBox();
            label2 = new Label();
            pKeyValue.SuspendLayout();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listView1.Location = new Point(17, 13);
            listView1.Margin = new Padding(3, 4, 3, 4);
            listView1.Name = "listView1";
            listView1.Size = new Size(482, 508);
            listView1.TabIndex = 4;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "키";
            columnHeader1.Width = 217;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "값";
            columnHeader2.Width = 219;
            // 
            // button1
            // 
            button1.Location = new Point(12, 528);
            button1.Name = "button1";
            button1.Size = new Size(239, 23);
            button1.TabIndex = 5;
            button1.Text = "지우기";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(262, 528);
            button2.Name = "button2";
            button2.Size = new Size(237, 23);
            button2.TabIndex = 6;
            button2.Text = "편집";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            button3.Location = new Point(12, 557);
            button3.Name = "button3";
            button3.Size = new Size(487, 68);
            button3.TabIndex = 7;
            button3.Text = "저장및 종료";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // pKeyValue
            // 
            pKeyValue.BackColor = Color.Silver;
            pKeyValue.Controls.Add(btnReset);
            pKeyValue.Controls.Add(btnCancel);
            pKeyValue.Controls.Add(btnOk);
            pKeyValue.Controls.Add(txtValue);
            pKeyValue.Controls.Add(label3);
            pKeyValue.Controls.Add(txtKey);
            pKeyValue.Controls.Add(label2);
            pKeyValue.Location = new Point(36, 392);
            pKeyValue.Margin = new Padding(3, 4, 3, 4);
            pKeyValue.Name = "pKeyValue";
            pKeyValue.Size = new Size(444, 119);
            pKeyValue.TabIndex = 10;
            pKeyValue.Visible = false;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(236, 84);
            btnReset.Margin = new Padding(3, 4, 3, 4);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 29);
            btnReset.TabIndex = 6;
            btnReset.Text = "초기화";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click_1;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(155, 84);
            btnCancel.Margin = new Padding(3, 4, 3, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 29);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "닫기";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click_1;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(74, 84);
            btnOk.Margin = new Padding(3, 4, 3, 4);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 29);
            btnOk.TabIndex = 4;
            btnOk.Text = "확인";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click_1;
            // 
            // txtValue
            // 
            txtValue.Location = new Point(74, 50);
            txtValue.Margin = new Padding(3, 4, 3, 4);
            txtValue.Name = "txtValue";
            txtValue.Size = new Size(333, 23);
            txtValue.TabIndex = 3;
            txtValue.TextChanged += txtValue_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(29, 53);
            label3.Name = "label3";
            label3.Size = new Size(30, 15);
            label3.TabIndex = 2;
            label3.Text = "값 : ";
            // 
            // txtKey
            // 
            txtKey.Location = new Point(74, 16);
            txtKey.Margin = new Padding(3, 4, 3, 4);
            txtKey.Name = "txtKey";
            txtKey.Size = new Size(333, 23);
            txtKey.TabIndex = 1;
            txtKey.TextChanged += txtKey_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 20);
            label2.Name = "label2";
            label2.Size = new Size(30, 15);
            label2.TabIndex = 0;
            label2.Text = "키 : ";
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(511, 637);
            Controls.Add(pKeyValue);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listView1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(527, 676);
            MinimumSize = new Size(527, 676);
            Name = "Form3";
            Text = "게임 옵션";
            Load += Form3_Load;
            pKeyValue.ResumeLayout(false);
            pKeyValue.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button button1;
        private Button button2;
        private Button button3;
        private Panel pKeyValue;
        private Button btnReset;
        private Button btnCancel;
        private Button btnOk;
        private TextBox txtValue;
        private Label label3;
        private TextBox txtKey;
        private Label label2;
    }
}