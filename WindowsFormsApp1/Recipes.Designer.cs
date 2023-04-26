namespace WindowsFormsApp1
{
    partial class Recipes
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
            this.inputSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.flowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonToSaved = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inputSearch
            // 
            this.inputSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.inputSearch.Location = new System.Drawing.Point(242, 12);
            this.inputSearch.Name = "inputSearch";
            this.inputSearch.Size = new System.Drawing.Size(532, 26);
            this.inputSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.btnSearch.Location = new System.Drawing.Point(799, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(93, 32);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // flowLayout
            // 
            this.flowLayout.AutoScroll = true;
            this.flowLayout.BackColor = System.Drawing.Color.Coral;
            this.flowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayout.Location = new System.Drawing.Point(0, 0);
            this.flowLayout.Name = "flowLayout";
            this.flowLayout.Padding = new System.Windows.Forms.Padding(0, 50, 0, 50);
            this.flowLayout.Size = new System.Drawing.Size(1224, 688);
            this.flowLayout.TabIndex = 2;
            // 
            // buttonToSaved
            // 
            this.buttonToSaved.BackColor = System.Drawing.Color.Transparent;
            this.buttonToSaved.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonToSaved.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.buttonToSaved.Location = new System.Drawing.Point(908, 9);
            this.buttonToSaved.Name = "buttonToSaved";
            this.buttonToSaved.Size = new System.Drawing.Size(145, 32);
            this.buttonToSaved.TabIndex = 1;
            this.buttonToSaved.Text = "Saved recipes";
            this.buttonToSaved.UseVisualStyleBackColor = false;
            this.buttonToSaved.Click += new System.EventHandler(this.buttonToSaved_Click);
            // 
            // Recipes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1224, 688);
            this.Controls.Add(this.buttonToSaved);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.inputSearch);
            this.Controls.Add(this.flowLayout);
            this.Name = "Recipes";
            this.Text = "Recipes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.FlowLayoutPanel flowLayout;
        private System.Windows.Forms.Button buttonToSaved;
    }
}