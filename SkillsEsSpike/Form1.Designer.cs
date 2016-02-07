namespace SkillsEsSpike
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
            this.cmdGenerate = new System.Windows.Forms.Button();
            this.cmdSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.txtCommonSkills = new System.Windows.Forms.TextBox();
            this.txtCommonCerts = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtElasticsearchServerUri = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdGenerate
            // 
            this.cmdGenerate.Location = new System.Drawing.Point(776, 12);
            this.cmdGenerate.Name = "cmdGenerate";
            this.cmdGenerate.Size = new System.Drawing.Size(170, 23);
            this.cmdGenerate.TabIndex = 0;
            this.cmdGenerate.Text = "Rebuild Search Index";
            this.cmdGenerate.UseVisualStyleBackColor = true;
            this.cmdGenerate.Click += new System.EventHandler(this.cmdGenerate_Click);
            // 
            // cmdSearch
            // 
            this.cmdSearch.Location = new System.Drawing.Point(356, 40);
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Size = new System.Drawing.Size(75, 23);
            this.cmdSearch.TabIndex = 1;
            this.cmdSearch.Text = "Search";
            this.cmdSearch.UseVisualStyleBackColor = true;
            this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(70, 40);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(269, 22);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.Text = "_all:*";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // txtResults
            // 
            this.txtResults.Location = new System.Drawing.Point(12, 69);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResults.Size = new System.Drawing.Size(700, 474);
            this.txtResults.TabIndex = 3;
            // 
            // txtCommonSkills
            // 
            this.txtCommonSkills.Location = new System.Drawing.Point(718, 107);
            this.txtCommonSkills.Multiline = true;
            this.txtCommonSkills.Name = "txtCommonSkills";
            this.txtCommonSkills.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCommonSkills.Size = new System.Drawing.Size(227, 200);
            this.txtCommonSkills.TabIndex = 4;
            // 
            // txtCommonCerts
            // 
            this.txtCommonCerts.Location = new System.Drawing.Point(718, 367);
            this.txtCommonCerts.Multiline = true;
            this.txtCommonCerts.Name = "txtCommonCerts";
            this.txtCommonCerts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCommonCerts.Size = new System.Drawing.Size(227, 176);
            this.txtCommonCerts.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(718, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 38);
            this.label1.TabIndex = 6;
            this.label1.Text = "Frequent skills in resultset (add with AND/OR skills:xxxx)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(718, 310);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(227, 54);
            this.label2.TabIndex = 7;
            this.label2.Text = "Frequent certifications in resultset (add with AND/OR certifications:yyy)";
            // 
            // txtElasticsearchServerUri
            // 
            this.txtElasticsearchServerUri.Enabled = false;
            this.txtElasticsearchServerUri.Location = new System.Drawing.Point(70, 12);
            this.txtElasticsearchServerUri.Name = "txtElasticsearchServerUri";
            this.txtElasticsearchServerUri.Size = new System.Drawing.Size(280, 22);
            this.txtElasticsearchServerUri.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "ES Uri:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Query:";
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(12, 549);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQuery.Size = new System.Drawing.Size(631, 92);
            this.txtQuery.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 722);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtElasticsearchServerUri);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCommonCerts);
            this.Controls.Add(this.txtCommonSkills);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cmdSearch);
            this.Controls.Add(this.cmdGenerate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdGenerate;
        private System.Windows.Forms.Button cmdSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.TextBox txtCommonSkills;
        private System.Windows.Forms.TextBox txtCommonCerts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtElasticsearchServerUri;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtQuery;
    }
}

