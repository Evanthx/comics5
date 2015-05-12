using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;

namespace Comic_Reader
{
    public class FileProcessor
    {
        private string htmlFile;
        private string[] tokenFile;

        public FileProcessor(string htmlFile, string[] tokenFile)
        {
            this.htmlFile = htmlFile;
            this.tokenFile = tokenFile;
        }

        public string processTheFiles(ListBox comicProgress) {
            //Walk the token file. For each line, see if that token exists in the html file. If it does - then
            //replace the token with data.
            foreach (string currentline in tokenFile) {
                string[] tokens = currentline.Split('|');
                processLine(tokens, comicProgress);
            }
            return htmlFile;
        }

        private void processLine(string[] tokens, ListBox comicProgress) {
            if (tokens.Length != 3) {
                //It's not what we expect ... ignore it.
                return;
            }

            //Does this token exist inside the HTML file?
            if (htmlFile.Contains(tokens[0])) {
                comicProgress.Items.Add(tokens[0]);
                comicProgress.Refresh();
                replaceToken(tokens);
            }
        }

        private void replaceToken(string[] tokens) {
            try {
                //Look up the token and get the value.
                WebClient client = new WebClient();
                string thepage = client.DownloadString(tokens[1]);

                //Great! We have the file. Now get the desired token out of it.
                int location = thepage.IndexOf(tokens[2]);
                if (location == -1) {
                    return;
                }
                location = location + tokens[2].Length;
                int endLocation = findEnd(thepage, location);
               

                string foundtoken = thepage.Substring(location, endLocation - location);
                htmlFile = htmlFile.Replace(tokens[0], foundtoken);
            } catch (Exception e) {
                return;
            }
        }

        private int findEnd(string thepage, int startlocation) {
            int endLocation1 = thepage.IndexOf('.', startlocation);
            int endLocation2 = thepage.IndexOf('\"', startlocation);
            int endLocation3 = thepage.IndexOf('\'', startlocation);

            //Find the shortest ... but not found will be the shortest. So crank those up.
            if (endLocation1 == -1) endLocation1 = 1000000;
            if (endLocation2 == -1) endLocation2 = 1000000;
            if (endLocation3 == -1) endLocation2 = 1000000;

            int[] array1 = { endLocation1, endLocation2, endLocation3 };
            return array1.Min();
        }

    }
}
