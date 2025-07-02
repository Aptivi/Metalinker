//
// Metalinker  Copyright (C) 2024  Aptivi
//
// This file is part of Metalinker
//
// Metalinker is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Metalinker is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Metalinker.Instances;
using Metalinker.Languages;
using Metalinker.Parsers;
using System;
using System.IO;
using System.Xml;

namespace Metalinker
{
    /// <summary>
    /// Metalink 3.0 and 4.0 parsing tools
    /// </summary>
    public static class MetalinkParser
    {
        /// <summary>
        /// Gets a metalink instance from a metadata file
        /// </summary>
        /// <param name="file">Path to the metalink file</param>
        /// <returns>Metalink instance</returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Metalink? GetMetalinkFromPath(string file)
        {
            // Sanity checks
            if (!File.Exists(file))
                throw new FileNotFoundException(string.Format(LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_FILENOTFOUND"), file));
            if (!Path.HasExtension(file))
                throw new ArgumentException(string.Format(LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_NEEDSEXT"), file));
            string ext = Path.GetExtension(file);
            if (ext != ".meta4" && ext != ".metalink")
                throw new ArgumentException(string.Format(LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_EXTMISMATCH"), ext));

            // Load the document using the file path
            var document = new XmlDocument();
            document.Load(file);
            return GetMetalinkFromXmlDocument(document);
        }

        /// <summary>
        /// Gets a metalink instance from a stream
        /// </summary>
        /// <param name="stream">Stream containing metalink XML data</param>
        /// <returns>Metalink instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Metalink? GetMetalinkFromStream(Stream stream)
        {
            // Sanity checks
            if (stream is null)
                throw new ArgumentNullException(nameof(stream), LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_NEEDSMETALINKSTREAM"));

            // Open the file stream
            var document = new XmlDocument();
            document.Load(stream);
            return GetMetalinkFromXmlDocument(document);
        }

        /// <summary>
        /// Gets a metalink instance from a string
        /// </summary>
        /// <param name="metalink">String containing metalink XML data</param>
        /// <returns>Metalink instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Metalink? GetMetalinkFromXml(string metalink)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(metalink))
                throw new ArgumentNullException(nameof(metalink), LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_NEEDSMETALINK"));

            // Load the XML
            var document = new XmlDocument();
            document.LoadXml(metalink);
            return GetMetalinkFromXmlDocument(document);
        }

        /// <summary>
        /// Gets a metalink instance from an XML document
        /// </summary>
        /// <param name="metalinkDocument">XML document containing metalink XML data</param>
        /// <returns>Metalink instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public static Metalink? GetMetalinkFromXmlDocument(XmlDocument metalinkDocument)
        {
            // Sanity checks
            if (metalinkDocument is null)
                throw new ArgumentNullException(nameof(metalinkDocument), LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_NEEDSMETALINKDOCUMENT"));
            var metalinkElement = metalinkDocument["metalink"] ??
                throw new ArgumentException(LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_INVALIDMETALINKDOCUMENT"));

            // Check the version and outsource the document to the appropriate parsers
            Metalink? metalink = null;
            bool maybe3 = metalinkElement.Attributes.Count > 1;
            bool maybe4 = metalinkElement.Attributes.Count == 1;
            if (maybe3 && metalinkElement.Attributes["version"].InnerText != "3.0")
                throw new ArgumentException(LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_INVALIDMETALINKDOCUMENT_V3"));
            else if (maybe4 && !metalinkElement.Attributes["xmlns"].InnerText.Contains("metalink"))
                throw new ArgumentException(LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_INVALIDMETALINKDOCUMENT_V4"));
            else
            {
                // They are valid, but we need to figure out what we actually have as the version to avoid
                // mismatch in case of extension mismatch.
                MetalinkVersion version;
                if (maybe3 && metalinkElement.Attributes["version"].InnerText == "3.0")
                    version = MetalinkVersion.Three;
                else if (maybe4 && metalinkElement.Attributes["xmlns"].InnerText.Contains("metalink"))
                    version = MetalinkVersion.Four;
                else
                    throw new InvalidDataException(LanguageTools.GetLocalized("METALINKER_PARSER_EXCEPTION_INVALIDMETALINKVERSION"));

                // Now that we know the real version, we need to outsource the document to their appropriate
                // documents
                if (version == MetalinkVersion.Three)
                    return Metalink3.Parse(metalinkElement);
                else if (version == MetalinkVersion.Four)
                    return Metalink4.Parse(metalinkElement);
            }
            return metalink;
        }
    }
}
