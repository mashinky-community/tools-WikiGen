using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MashinkyWikiGen
{
    /// <summary>
    /// Corrects malformed XML files from the Mashinky game data.
    /// Handles various XML parsing issues including invalid entities, comments, and disposition formatting.
    /// </summary>
    class XMLCorrector
    {
        /// <summary>
        /// The current iteration count for generating unique error file names.
        /// </summary>
        private int iteration;
        
        /// <summary>
        /// Initializes a new instance of the XMLCorrector class.
        /// </summary>
        public XMLCorrector()
        {
            iteration = 0;
        }
        /// <summary>
        /// Corrects a standard XML file by removing comments, declarations, and invalid entities.
        /// Wraps the content in a root element for proper XML structure.
        /// </summary>
        /// <param name="path">The path to the XML file to correct</param>
        /// <returns>A corrected XDocument, or throws an exception if correction fails</returns>
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

        /// <summary>
        /// Corrects a buildings XML file with additional fixes for disposition and sign issues.
        /// Applies specialized corrections for building-specific XML malformations.
        /// </summary>
        /// <param name="path">The path to the buildings XML file to correct</param>
        /// <returns>A corrected XDocument, or throws an exception if correction fails</returns>
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
        /// <summary>
        /// Removes XML comments from the input stream.
        /// Uses regex to find and remove <!-- comment --> blocks.
        /// </summary>
        /// <param name="stream">The stream reader containing the XML content</param>
        /// <returns>The XML content with comments removed</returns>
        public string RemoveComments(StreamReader stream)
        {
            string removed = Regex.Replace(stream.ReadToEnd(), "<!--.*?-->", "", RegexOptions.Singleline);
            return removed;
        }

        /// <summary>
        /// Removes XML declarations from the input string.
        /// Uses regex to find and remove <?xml ?> declarations.
        /// </summary>
        /// <param name="s">The XML string to process</param>
        /// <returns>The XML string with declarations removed</returns>
        public string RemoveDeclaration(string s)
        {
            string removed = Regex.Replace(s, "<\\?.*?\\?>", "", RegexOptions.Singleline);
            return removed;
        }
        /// <summary>
        /// Removes invalid XML entities and corrects various formatting typos.
        /// Fixes ampersand entities, control characters, and malformed tag endings.
        /// </summary>
        /// <param name="s">The XML string to process</param>
        /// <returns>The XML string with invalid entities removed</returns>
        public string RemoveInvalidEntities(string s)
        {
            string removed = Regex.Replace(s, "&", "");
            removed = Regex.Replace(removed, "\u0004", "");
            removed = Regex.Replace(removed, "/n>", "");
            removed = Regex.Replace(removed, "/ >", "/>");
            return removed;
        }

        /// <summary>
        /// Wraps the XML content in a root element.
        /// Required for proper XML document structure when the original lacks a single root.
        /// </summary>
        /// <param name="s">The XML content to wrap</param>
        /// <returns>The XML content wrapped in a root element</returns>
        public string AddRoot(string s)
        {
            return "<root>" + s + "</root>";
        }
        /// <summary>
        /// Corrects invalid comparison signs in building autobuild rules.
        /// Replaces < and > characters that conflict with XML syntax with letter equivalents.
        /// </summary>
        /// <param name="s">The XML string to process</param>
        /// <returns>The XML string with corrected comparison signs</returns>
        public string CorrectInvalidSigns(string s)
        {
            string corrected = Regex.Replace(s, "epoch<", "epochL");
            corrected = Regex.Replace(corrected, "epoch>", "epochG");
            corrected = Regex.Replace(corrected, "townsize>", "townsizeG");
            corrected = Regex.Replace(corrected, "townsize<", "townsizeL");
            return corrected;
        }
        /// <summary>
        /// Corrects building disposition strings that contain invalid XML characters.
        /// Replaces directional characters (>, <, ^) with valid letters (R, L, U) within disposition attributes.
        /// </summary>
        /// <param name="input">The XML string to process</param>
        /// <returns>The XML string with corrected disposition formatting</returns>
        public string CorrectDispositions(string input)
        {
            string corrected = Regex.Replace(input, @"(?<=\bdisposition=""[^""]*)>", "R");
            corrected = Regex.Replace(corrected, @"<(?<=\bdisposition=""[^""]*)", "L");
            corrected = Regex.Replace(corrected, "\\^", "U");
            return corrected;
        }
    }
}
