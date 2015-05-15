using System;
using System.IO;
using System.Net;
using Comic_Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComicUnitTester {
    [TestClass]
    public class FileProcessorTests {

        string htmlFile = File.ReadAllText(@"C:\code\comics5\test\comic_in.htm");
        string[] tokenFile = File.ReadAllLines(@"C:\code\comics5\test\comics.ini");
        //Totally cheat with the definition file. Sometimes I just want to search it, sometimes 
        //I want to walk each definition. So load it both ways.
        string definitionFile = File.ReadAllText(@"C:\code\comics5\test\ComicSetup.ini");
        string[] definitionFileList = File.ReadAllLines(@"C:\code\comics5\test\ComicSetup.ini");

        protected FileProcessor fileProcessor;

        [TestInitialize]
        public void TestInitialize() {
            fileProcessor = new FileProcessor(htmlFile, tokenFile);
        }

        [TestMethod]
        public void FindUnusedTokens() {
            foreach (string currentline in tokenFile) {
                string[] tokens = currentline.Split('|');
                if (tokens.Length == 3 && !definitionFile.Contains(tokens[0])) {
                    Assert.Fail("Unused token string! " + tokens[0]);
                    return;
                }

                if (tokens.Length == 2 || tokens.Length > 3) {
                    Assert.Fail("Misformatted token string! " + tokens[0]);
                }
            }
        }

        [TestMethod]
        public void TestMethodGlobalCheck() {
            //Not really a unit test but handy. Run all comics, and look for 404s.
            foreach (string currentDefinition in definitionFileList) {
                string[] definition = currentDefinition.Split('|');
                processLine(definition);
            }
        }

        public void processLine(string[] definition) {

            if (definition.Length != 3) {
                //It's not what we expect ... report it.
                string err = "Bad definition! ";
                if (definition.Length > 0) {
                    err += definition[0];
                }
                Assert.Fail(err);
                return;
            }
            string comicImage = definition[1];
            if (comicImage.IndexOf("img src") == -1) {
                //This is a line with no image. That's fine, and expected, but has no work.
                return;
            }

            foreach (string currentToken in tokenFile) {
                string[] tokens = currentToken.Split('|');
                if (tokens.Length != 3) {
                    continue;
                }
                if (comicImage.Contains(tokens[0])) {
                    string resultToken = fileProcessor.getToken(tokens);
                    comicImage = comicImage.Replace(tokens[0], resultToken);
                }
            }

            //All tokens processed...now get the file!
            int loc = comicImage.IndexOf("img src");
            Assert.IsTrue(loc >= 0, "Bad line for " + definition[0]);
            loc = comicImage.IndexOf("http", loc);
            Assert.IsTrue(loc >= 0, "Bad line continuation for " + definition[0]);

            int endLoc = comicImage.IndexOf("\"", loc);
            Assert.IsTrue(loc >= 0, "Bad end line for " + definition[0]);

            comicImage = comicImage.Substring(loc, endLoc - loc);
            try {
                WebClient client = new WebClient();
                byte[] thepage = client.DownloadData(comicImage);
            } catch (WebException e) {
                Assert.Fail("Did not get comic " + definition[0]);
            }


            // System.Console.WriteLine("Unused token string! " + tokens[0]);
        }
    }
}
