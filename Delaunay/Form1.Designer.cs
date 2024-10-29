namespace indiv_9
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов Windows Form Designer

        private void InitializeComponent()
        {
            this.btnTriangulate = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pictureBoxField = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxField)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTriangulate
            // 
            this.btnTriangulate.Location = new System.Drawing.Point(12, 12);
            this.btnTriangulate.Name = "btnTriangulate";
            this.btnTriangulate.Size = new System.Drawing.Size(120, 30);
            this.btnTriangulate.TabIndex = 0;
            this.btnTriangulate.Text = "Triangulate";
            this.btnTriangulate.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(150, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 30);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // pictureBoxField
            // 
            this.pictureBoxField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxField.BackColor = System.Drawing.Color.White;
            this.pictureBoxField.Location = new System.Drawing.Point(12, 50);
            this.pictureBoxField.Name = "pictureBoxField";
            this.pictureBoxField.Size = new System.Drawing.Size(760, 400);
            this.pictureBoxField.TabIndex = 2;
            this.pictureBoxField.TabStop = false;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.pictureBoxField);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnTriangulate);
            this.Name = "Form1";
            this.Text = "Delaunay Triangulation";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxField)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnTriangulate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.PictureBox pictureBoxField;
    }
}
