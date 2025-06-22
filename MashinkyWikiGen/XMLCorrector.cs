using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MashinkyWikiGen
{
    class XMLCorrector
    {
        private int iteration;
        public XMLCorrector()
        {
            iteration = 0;
        }
        public XDocument CorrectXmlFile(string path)
        {
            iteration++;
            //  string testPath = Path.Combine(Environment.CurrentDirectory, path);
            try
            {
                StreamReader originalXml = new StreamReader(path);
                string validXml = RemoveComments(originalXml);
                validXml = RemoveDeclaration(validXml);
                validXml = RemoveInvalidEntities(validXml);
                validXml = AddRoot(validXml);
                //File.WriteAllText(Settings.Path + @"\Wagon_Types.xml", validXml);

                XDocument correctedFile = XDocument.Parse(validXml);
                return correctedFile;
            }
            catch (Exception e)
            {
                StreamReader originalXml = new StreamReader(path);
                string invalidXml = RemoveComments(originalXml);
                invalidXml = RemoveDeclaration(invalidXml);
                invalidXml = RemoveInvalidEntities(invalidXml);
                invalidXml = AddRoot(invalidXml);
                File.AppendAllText("traceWikiGen.txt", $"\nFailed to correct {path} due to following error: {e.Message}");
                File.WriteAllText($"invalid_XML{iteration}.xml", invalidXml);
                File.AppendAllText($"invalid_XML{iteration}.xml", e.Message + path);
                throw;
            }

        }

        public XDocument CorrectBuildingsXmlFile(string path)
        {
            //  string testPath = Path.Combine(Environment.CurrentDirectory, path);
            try
            {
                StreamReader originalXml = new StreamReader(path);
                string validXml = RemoveComments(originalXml);
                validXml = RemoveDeclaration(validXml);
                validXml = RemoveInvalidEntities(validXml);
                validXml = CorrectInvalidSigns(validXml);
                validXml = CorrectDispositions(validXml);
                validXml = AddRoot(validXml);

                XDocument correctedFile = XDocument.Parse(validXml);
                return correctedFile;
            }
            catch (Exception e)
            {
                StreamReader originalXml = new StreamReader(path);
                string invalidXml = RemoveComments(originalXml);
                invalidXml = RemoveDeclaration(invalidXml);
                invalidXml = RemoveInvalidEntities(invalidXml);
                invalidXml = CorrectInvalidSigns(invalidXml);
                invalidXml = CorrectDispositions(invalidXml);
                invalidXml = AddRoot(invalidXml);
                File.AppendAllText("invalid_BT.xml", invalidXml);
                File.AppendAllText("invalid_BT.xml", e.Message);
                throw;
            }

        }
        public string RemoveComments(StreamReader stream)
        {
            string removed = Regex.Replace(stream.ReadToEnd(), "<!--.*?-->", "", RegexOptions.Singleline);
            return removed;
        }

        public string RemoveDeclaration(string s)
        {
            string removed = Regex.Replace(s, "<\\?.*?\\?>", "", RegexOptions.Singleline);
            return removed;
        }
        /// <summary>
        /// Corrects various typos
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string RemoveInvalidEntities(string s)
        {
            string removed = Regex.Replace(s, "&", "");
            removed = Regex.Replace(removed, "\u0004", "");
            removed = Regex.Replace(removed, "/n>", "");
            removed = Regex.Replace(removed, "/ >", "/>");
            return removed;
        }

        public string AddRoot(string s)
        {
            return "<root>" + s + "</root>";
        }
        /// <summary>
        /// Corrects buildings autobuild rules
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string CorrectInvalidSigns(string s)
        {
            string corrected = Regex.Replace(s, "epoch<", "epochL");
            corrected = Regex.Replace(corrected, "epoch>", "epochG");
            corrected = Regex.Replace(corrected, "townsize>", "townsizeG");
            corrected = Regex.Replace(corrected, "townsize<", "townsizeL");
            return corrected;
        }
        /// <summary>
        /// corrects buildings dispositions
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string CorrectDispositions(string input)
        {
            string corrected = Regex.Replace(input, @"(?<=\bdisposition=""[^""]*)>", "R");
            corrected = Regex.Replace(corrected, @"<(?<=\bdisposition=""[^""]*)", "L");
            corrected = Regex.Replace(corrected, "\\^", "U");
            return corrected;
        }
    }
}
