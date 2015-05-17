using System;
using System.IO;
using System.Net;
using Comic_Reader;
using System.Collections.Generic;
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
                if (tokens.Length == 2 || tokens.Length > 4) {
                    Assert.Fail("Misformatted token string! " + tokens[0]);
                }

                if (!definitionFile.Contains(tokens[0])) {
                    Assert.Fail("Unused token string! " + tokens[0]);
                    return;
                }

            }
        }

        [TestMethod]
        public void DefinitionsLookOK() {
            //Not really a unit test but handy. Run all comics, and look for 404s.
            foreach (string currentDefinition in definitionFileList) {
                string[] definition = currentDefinition.Split('|');
                if (definition.Length != 3) {
                    //It's not what we expect ... report it.
                    string err = "Bad definition! ";
                    if (definition.Length > 0) {
                        err += definition[0];
                    }
                    Assert.Fail(err);
                }
            }
        }


        /**
         * So this test is terrible and breaks every rule of testing I know about, but it's
         * terribly useful.
         * 
         * Go to comics setup and activate every comic. Then run this test. Every comic that works
         * will be turned off, meaning you're left looking at only broken links to fix.
         * 
         * Fix them, then run this again. It only checks turned on strips, and turns off any
         * that are fixed - meaning you can just run this to find broken things, fix them,
         * and iterate over that until everything works!
         * 
         * */
        [TestMethod]
        public void GlobalCheck() {
            //Not really a unit test but handy. Run all comics, and look for 404s.
            List<string> list = new List<string>();

            foreach (string currentDefinition in definitionFileList) {
                string[] definition = currentDefinition.Split('|');
                //Only check ones that are active
                if (definition[2].StartsWith("N")) {
                    list.Add(string.Join("|", definition));
                    continue;
                }
                Boolean worked = processLine(definition);
                if (worked) {
                    definition[2] = "N1";
                } else {
                    definition[2] = "Y1";
                }
                list.Add(string.Join("|", definition));
            }

            //Now write the definitions back!
            System.IO.File.WriteAllLines(@"C:\code\comics5\test\ComicSetup.ini", list);
        }

        public Boolean processLine(string[] definition) {

            string comicImage = definition[1];
            if (comicImage.IndexOf("img src") == -1) {
                //This is a line with no image. That's fine, and expected, but has no work.
                return true;
            }

            foreach (string currentToken in tokenFile) {
                string[] tokens = currentToken.Split('|');
                if (tokens.Length != 3 && tokens.Length != 4) {
                    continue;
                }
                if (comicImage.Contains(tokens[0])) {
                    string resultToken = fileProcessor.getToken(tokens);
                    comicImage = comicImage.Replace(tokens[0], resultToken);
                }
            }

            //Check for special tokens ...
            comicImage = fileProcessor.processHtmlLine(comicImage);

            //All tokens processed...now get the file!
            int loc = comicImage.IndexOf("img src=\"");
            Assert.IsTrue(loc >= 0, "Bad line for " + definition[0]);
            loc += 9;

            int endLoc = comicImage.IndexOf("\"", loc);
            Assert.IsTrue(loc >= 0, "Bad end line for " + definition[0]);

            comicImage = comicImage.Substring(loc, endLoc - loc);
            try {
                WebClient client = new WebClient();
                byte[] thepage = client.DownloadData(comicImage);
            } catch (WebException e) {
                System.Console.WriteLine("Did not get comic " + definition[0]);
                return false;
            }

            return true;
            // System.Console.WriteLine("Unused token string! " + tokens[0]);
        }
    }
}
