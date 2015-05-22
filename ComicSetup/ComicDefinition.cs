using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComicSetup {
    class ComicDefinition {

        public ComicDefinition(string[] tokens) {
            name = tokens[0];
            data = tokens[1];
            activeNumber = tokens[2];
        }

        public string name { get; set; }
        public string data { get; set; }
        public string activeNumber { get; set; }

        public override string ToString() {
            return name;
        }

        public CheckState isChecked() {
            if (activeNumber.StartsWith("Y")) {
                return CheckState.Checked;
            }
            return CheckState.Unchecked;
        }
    }
}
