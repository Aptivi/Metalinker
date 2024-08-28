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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Metalinker.Parsers
{
    internal static class Metalink3
    {
        internal static Metalink? Parse(XmlElement metalinkElement)
        {
            var metalink = new Metalink();

            // First, get the attributes with checking in case of non-conformance
            var attributes = metalinkElement.Attributes;
            metalink.Origin = attributes["origin"]?.InnerText ?? "";
            metalink.Generator = attributes["generator"]?.InnerText ?? "";
            metalink.Dynamic = attributes["type"]?.InnerText == "dynamic";
            metalink.PublishDate = attributes["pubdate"]?.InnerText ?? "";

            // Then, get the publisher information
            var publisherElement = metalinkElement["publisher"];
            if (publisherElement is not null)
            {
                metalink.Publisher = publisherElement["name"]?.InnerText ?? "";
                metalink.PublisherUrl = publisherElement["url"]?.InnerText ?? "";
            }

            // Now, get the list of files in case we have a metalink of more than one file
            List<MetalinkFile> files = [];
            var filesElement = metalinkElement["files"];
            foreach (XmlElement fileElement in filesElement)
            {
                var file = new MetalinkFile
                {
                    // Get the file name
                    File = fileElement.Attributes["name"]?.InnerText ?? ""
                };

                // Get the verification method to get signatures, hashes, and pieces.
                var verificationElement = fileElement["verification"] ??
                    throw new InvalidDataException("Verification doesn't exist.");

                // This is not as simple as you might think. Does it look logical to you?
                var signaturesElement = verificationElement.ChildNodes.OfType<XmlElement>().Where((xe) => xe.Name == "signature");
                List<MetalinkSignature> signatures = [];
                foreach (var signatureElement in signaturesElement)
                {
                    var signature = new MetalinkSignature
                    {
                        SignatureFile = signatureElement.Attributes["file"]?.InnerText ?? "",
                        SignatureType = signatureElement.Attributes["type"]?.InnerText ?? "",
                        SignatureContent = signatureElement?.InnerText ?? ""
                    };
                    signatures.Add(signature);
                }
                file.Signatures = [.. signatures];
                
                // Get the hashes
                var hashesElement = verificationElement.ChildNodes.OfType<XmlElement>().Where((xe) => xe.Name == "hash");
                List<MetalinkHash> hashes = [];
                foreach (var hashElement in hashesElement)
                {
                    var hash = new MetalinkHash
                    {
                        HashSumType = hashElement.Attributes["type"]?.InnerText ?? "",
                        HashSum = hashElement?.InnerText ?? ""
                    };
                    hashes.Add(hash);
                }
                file.Hashes = [.. hashes];

                // Get the piece information
                var piecesElement = verificationElement["pieces"] ??
                    throw new InvalidDataException("No pieces.");
                var pieceInfo = new MetalinkPieceInfo
                {
                    Length = long.Parse(piecesElement.Attributes["length"]?.InnerText),
                    Type = piecesElement.Attributes["type"]?.InnerText ?? ""
                };

                // Now, get the piece hashes
                var pieceHashesElement = piecesElement.ChildNodes.OfType<XmlElement>().Where((xe) => xe.Name == "hash");
                List<string> pieceHashes = [];
                foreach (var pieceHashElement in pieceHashesElement)
                    pieceHashes.Add(pieceHashElement?.InnerText ?? "");
                pieceInfo.Hashes = [.. pieceHashes];
                file.PieceInfo = pieceInfo;

                // Get the resource information
                var resourcesElement = fileElement["resources"] ??
                    throw new InvalidDataException("No resources.");
                var urlsElement = resourcesElement.ChildNodes.OfType<XmlElement>().Where((xe) => xe.Name == "url");
                List<MetalinkResource> resources = [];
                foreach (var urlElement in urlsElement)
                {
                    var uri = new Uri(urlElement.InnerText ?? "");
                    var resourceInfo = new MetalinkResource
                    {
                        URL = uri.ToString(),
                        Type = urlElement.Attributes["type"]?.InnerText ?? "",
                        Location = urlElement.Attributes["location"]?.InnerText ?? "",
                        Preference = int.Parse(urlElement.Attributes["preference"]?.InnerText)
                    };
                    resources.Add(resourceInfo);
                }
                file.Resources = [.. resources];

                // Add the file
                files.Add(file);
            }
            metalink.Files = [.. files];
            return metalink;
        }
    }
}
