using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Comic_Reader
{
    public partial class ReaderWindow : Form
    {
        public ReaderWindow()
        {
            InitializeComponent();
        }

        private void ReaderWindow_Shown(object sender, EventArgs e)
        {
            //Three parts.
            //First, read the ini files
            string htmlFile = File.ReadAllText("comic_in.htm");
            string[] tokenFile = File.ReadAllLines("comics.ini");

            //Second, process the ini files to create the daily file
            FileProcessor fileProcessor = new FileProcessor(htmlFile, tokenFile);
            string resultFile = fileProcessor.processTheFiles();

            //Third, launch the default web browser on the daily file
            File.WriteAllText("comic_out.htm", resultFile);

            Application.Exit();
            return;
        }

        private void ReaderWindow_Load(object sender, EventArgs e) {

        }
    }
}
