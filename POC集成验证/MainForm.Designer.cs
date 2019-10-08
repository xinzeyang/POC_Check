namespace POC集成验证
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CheckPoc_Box = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Col_URL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Static = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Col_Detail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_CheckExp = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.CheckPoc_Box.SuspendLayout();
            this.SuspendLayout();
            // 
            // CheckPoc_Box
            // 
            this.CheckPoc_Box.Controls.Add(this.listView1);
            this.CheckPoc_Box.Controls.Add(this.label1);
            this.CheckPoc_Box.Controls.Add(this.Btn_CheckExp);
            this.CheckPoc_Box.Controls.Add(this.textBox1);
            this.CheckPoc_Box.Location = new System.Drawing.Point(12, 46);
            this.CheckPoc_Box.Name = "CheckPoc_Box";
            this.CheckPoc_Box.Size = new System.Drawing.Size(776, 392);
            this.CheckPoc_Box.TabIndex = 0;
            this.CheckPoc_Box.TabStop = false;
            this.CheckPoc_Box.Text = "验证";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Col_URL,
            this.Col_Static,
            this.Col_Detail});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(24, 139);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(713, 97);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Col_URL
            // 
            this.Col_URL.Text = "URL";
            this.Col_URL.Width = 135;
            // 
            // Col_Static
            // 
            this.Col_Static.Text = "Static";
            this.Col_Static.Width = 138;
            // 
            // Col_Detail
            // 
            this.Col_Detail.Text = "Detail";
            this.Col_Detail.Width = 119;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL:";
            // 
            // Btn_CheckExp
            // 
            this.Btn_CheckExp.Location = new System.Drawing.Point(599, 48);
            this.Btn_CheckExp.Name = "Btn_CheckExp";
            this.Btn_CheckExp.Size = new System.Drawing.Size(75, 23);
            this.Btn_CheckExp.TabIndex = 1;
            this.Btn_CheckExp.Text = "检测";
            this.Btn_CheckExp.UseVisualStyleBackColor = true;
            this.Btn_CheckExp.Click += new System.EventHandler(this.Btn_CheckExp_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(78, 46);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(237, 25);
            this.textBox1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CheckPoc_Box);
            this.Name = "MainForm";
            this.Text = "POC集成验证";
            this.CheckPoc_Box.ResumeLayout(false);
            this.CheckPoc_Box.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox CheckPoc_Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_CheckExp;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Col_URL;
        private System.Windows.Forms.ColumnHeader Col_Static;
        private System.Windows.Forms.ColumnHeader Col_Detail;
    }
}

