namespace Comic_Reader
{
    partial class ReaderWindow
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
            this.comicProgress = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // comicProgress
            // 
            this.comicProgress.FormattingEnabled = true;
            this.comicProgress.Location = new System.Drawing.Point(12, 12);
            this.comicProgress.Name = "comicProgress";
            this.comicProgress.Size = new System.Drawing.Size(358, 160);
            this.comicProgress.TabIndex = 0;
            // 
            // ReaderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 193);
            this.Controls.Add(this.comicProgress);
            this.Name = "ReaderWindow";
            this.Text = "Comic Reader";
            this.Load += new System.EventHandler(this.ReaderWindow_Load);
            this.Shown += new System.EventHandler(this.ReaderWindow_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox comicProgress;
    }
}

