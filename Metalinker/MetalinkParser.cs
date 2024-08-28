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
        public static Metalink? GetMetalinkFromPath(string file)
        {
            // Sanity checks
            if (!File.Exists(file))
                throw new FileNotFoundException($"Metalink file {file} doesn't exist.");
            if (!Path.HasExtension(file))
                throw new ArgumentException($"Metalink file {file} must have an extension of either .meta4 or .metalink.");
            string ext = Path.GetExtension(file);
            if (ext != ".meta4" && ext != ".metalink")
                throw new ArgumentException($"Metalink file extension {ext} is invalid. It must have an extension of either .meta4 or .metalink.");

            // Load the document using the file path
            var document = new XmlDocument();
            document.Load(file);
            return GetMetalinkFromXmlDocument(document);
        }

        public static Metalink? GetMetalinkFromStream(Stream stream)
        {
            // Sanity checks
            if (stream is null)
                throw new ArgumentNullException(nameof(stream), "Metalink stream is null.");

            // Open the file stream
            var document = new XmlDocument();
            document.Load(stream);
            return GetMetalinkFromXmlDocument(document);
        }

        public static Metalink? GetMetalinkFromXml(string metalink)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(metalink))
                throw new ArgumentNullException(nameof(metalink), "Metalink is not provided.");

            // Load the XML
            var document = new XmlDocument();
            document.LoadXml(metalink);
            return GetMetalinkFromXmlDocument(document);
        }

        public static Metalink? GetMetalinkFromXmlDocument(XmlDocument metalinkDocument)
        {
            // Sanity checks
            if (metalinkDocument is null)
                throw new ArgumentNullException(nameof(metalinkDocument), "Metalink document is not provided.");
            var metalinkElement = metalinkDocument["metalink"] ??
                throw new ArgumentException("Not a valid Metalink document.");

            // Check the version and outsource the document to the appropriate parsers
            Metalink? metalink = null;
            bool maybe3 = metalinkElement.Attributes.Count > 1;
            bool maybe4 = metalinkElement.Attributes.Count == 1;
            if (maybe3 && metalinkElement.Attributes["version"].InnerText != "3.0")
                throw new ArgumentException("Not a valid Metalink 3.0 document.");
            else if (maybe4 && !metalinkElement.Attributes["xmlns"].InnerText.Contains("metalink"))
                throw new ArgumentException("Not a valid Metalink 4.0 document.");
            else
            {
                // They are valid, but we need to figure out what we actually have as the version to avoid
                // mismatch in case of extension mismatch.
                MetalinkVersion version = MetalinkVersion.Unknown;
                if (maybe3 && metalinkElement.Attributes["version"].InnerText == "3.0")
                    version = MetalinkVersion.Three;
                else if (maybe4 && metalinkElement.Attributes["xmlns"].InnerText.Contains("metalink"))
                    version = MetalinkVersion.Four;
                else
                    throw new InvalidDataException("Can't determine Metalink version");

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
