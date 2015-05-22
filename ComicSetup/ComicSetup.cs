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

namespace ComicSetup {
    public partial class SetupForm : Form {
        public SetupForm() {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void SetupForm_Load(object sender, EventArgs e) {
            string[] definitionFile = File.ReadAllLines("ComicSetup.ini");
            foreach (string definition in definitionFile) {
                string[] tokens = definition.Split('|');
                if (tokens.Length != 3) {
                    continue;
                }
                ComicDefinition def = new ComicDefinition(tokens);
                comicSelectorList.Items.Add(def, def.isChecked());               
            }
        }
        
        private void buttonOK_Click(object sender, EventArgs e) {
            StringBuilder sb = new StringBuilder();
            int yes = 1;
            int no = 1;
            //Walk the checkbox list, and write up the file!

            foreach (ComicDefinition def in comicSelectorList.Items) {
                sb.Append(def.name);
                sb.Append("|");
                sb.Append(def.data);
                sb.Append("|");
                if (comicSelectorList.CheckedItems.Contains(def)) {
                    sb.Append("Y");
                    sb.Append(yes++);
                } else {
                    sb.Append("N");
                    sb.Append(no++);
                }
                sb.Append(Environment.NewLine);
            }
            File.WriteAllText("ComicSetup.ini", sb.ToString());
            this.Close();
        }

        private void loadCheckboxList(string[] tokenFile) {

        }

    }
}
