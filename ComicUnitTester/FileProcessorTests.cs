using System;
using System.IO;
using Comic_Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComicUnitTester {
    [TestClass]
    public class FileProcessorTests {

        string htmlFile = File.ReadAllText(@"C:\code\comics5\test\comic_in.htm");
        string[] tokenFile = File.ReadAllLines(@"C:\code\comics5\test\comics.ini");

        protected FileProcessor fileProcessor;

        [TestInitialize]
        public void TestInitialize() {
            fileProcessor = new FileProcessor(htmlFile, tokenFile);
        }   


        [TestMethod]
        public void TestMethodGlobalCheck() {
            //Not really a unit test but handy. Run all comics, and look for 404s.
            foreach (string currentline in tokenFile) {
                string[] tokens = currentline.Split('|');
                processLine(tokens);
            }
        }

        public void processLine(string[] tokens) {
            if (tokens.Length != 3) {
                //It's not what we expect ... report it.
                System.Console.WriteLine("Bad token string! " + tokens[0]);
                return;
            }

            //Does this token exist inside the HTML file?
            if (!htmlFile.Contains(tokens[0])) {
                System.Console.WriteLine("Unused token string! " + tokens[0]);
                return;
            }

           // System.Console.WriteLine("Unused token string! " + tokens[0]);
        }
    }
}
