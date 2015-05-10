using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Comic_Reader
{
    class Program
    {
        static void Main(string[] args)
        {
            //Three parts.
            //First, read the ini files
            string htmlFile = File.ReadAllText("comic_in.htm");
            string[] definitionFile = File.ReadAllLines("comics.ini");
            string[] tokenFile = File.ReadAllLines("ComicSetup.ini");

            //Second, process the ini files to create the daily file
            
            //Third, launch the default web browser on the daily file
            File.WriteAllText("comic_out.htm", htmlFile);
            return;
        }
    }
}
