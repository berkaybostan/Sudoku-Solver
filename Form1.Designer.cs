namespace SudokuSolver
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.showSln3 = new System.Windows.Forms.TextBox();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.showSlnSteps = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtIslemsuresi = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(13, 13);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "Dosya Yukle";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(94, 32);
            this.txtValue.Multiline = true;
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(147, 150);
            this.txtValue.TabIndex = 1;
            // 
            // showSln3
            // 
            this.showSln3.Location = new System.Drawing.Point(258, 85);
            this.showSln3.Multiline = true;
            this.showSln3.Name = "showSln3";
            this.showSln3.Size = new System.Drawing.Size(536, 346);
            this.showSln3.TabIndex = 4;
            // 
            // statusBox
            // 
            this.statusBox.Location = new System.Drawing.Point(258, 32);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(322, 20);
            this.statusBox.TabIndex = 5;
            // 
            // showSlnSteps
            // 
            this.showSlnSteps.Location = new System.Drawing.Point(258, 58);
            this.showSlnSteps.Name = "showSlnSteps";
            this.showSlnSteps.Size = new System.Drawing.Size(124, 23);
            this.showSlnSteps.TabIndex = 6;
            this.showSlnSteps.Text = "Adimlari Goster";
            this.showSlnSteps.UseVisualStyleBackColor = true;
            this.showSlnSteps.Click += new System.EventHandler(this.showSlnSteps_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(537, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Kazanan Thread\'in Islem Suresi:";
            // 
            // txtIslemsuresi
            // 
            this.txtIslemsuresi.Location = new System.Drawing.Point(712, 59);
            this.txtIslemsuresi.Name = "txtIslemsuresi";
            this.txtIslemsuresi.Size = new System.Drawing.Size(82, 20);
            this.txtIslemsuresi.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 443);
            this.Controls.Add(this.txtIslemsuresi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.showSlnSteps);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.showSln3);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.btnOpenFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sudoku";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.TextBox showSln3;
        private System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.Button showSlnSteps;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIslemsuresi;
    }
}

