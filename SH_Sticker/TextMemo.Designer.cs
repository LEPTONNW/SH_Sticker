namespace SH_Sticker
{
    partial class TextMemo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextMemo));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ExButton = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SCColor = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(67, 37);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(314, 25);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(8, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "제목 :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ExButton
            // 
            this.ExButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExButton.BackColor = System.Drawing.Color.Red;
            this.ExButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ExButton.Font = new System.Drawing.Font("돋움", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ExButton.ForeColor = System.Drawing.Color.White;
            this.ExButton.Location = new System.Drawing.Point(419, -5);
            this.ExButton.Name = "ExButton";
            this.ExButton.Size = new System.Drawing.Size(36, 35);
            this.ExButton.TabIndex = 3;
            this.ExButton.Text = "X";
            this.ExButton.UseVisualStyleBackColor = false;
            this.ExButton.Click += new System.EventHandler(this.ExButton_Click);
            // 
            // Save
            // 
            this.Save.Font = new System.Drawing.Font("굴림", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Save.Location = new System.Drawing.Point(385, 36);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(54, 27);
            this.Save.TabIndex = 5;
            this.Save.Text = "저장";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // SCColor
            // 
            this.SCColor.BackColor = System.Drawing.Color.White;
            this.SCColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SCColor.Location = new System.Drawing.Point(0, 0);
            this.SCColor.Name = "SCColor";
            this.SCColor.Size = new System.Drawing.Size(39, 26);
            this.SCColor.TabIndex = 6;
            this.SCColor.Text = "색";
            this.SCColor.UseVisualStyleBackColor = false;
            this.SCColor.Click += new System.EventHandler(this.SCColor_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(11, 78);
            this.richTextBox1.Multiline = true;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(426, 376);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(45, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Text";
            this.label2.DoubleClick += new System.EventHandler(this.label2_DoubleClick);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("돋움", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(384, -2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 32);
            this.button1.TabIndex = 8;
            this.button1.Text = "ㅡ";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TextMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(450, 466);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.SCColor);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ExButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TextMemo";
            this.Load += new System.EventHandler(this.TextMemo_Load);
            this.Move += new System.EventHandler(this.TextMemo_Move);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ExButton;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button SCColor;
        private System.Windows.Forms.TextBox richTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}