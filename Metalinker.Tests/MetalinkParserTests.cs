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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;

namespace Metalinker.Tests
{
    [TestClass]
    public class MetalinkParserTests
    {
        [TestMethod]
        public void TestParseMetalink3()
        {
            // Parse it
            Stream? stream = MetalinkLoader.LoadMetalinkStream("slackware15.iso.metalink");
            if (stream is null)
                Assert.Fail("Stream is null");
            var metalink = MetalinkParser.GetMetalinkFromStream(stream);

            // Verify the correctness
            metalink.ShouldNotBeNull();
            metalink.Dynamic.ShouldBeTrue();
            metalink.Files.ShouldNotBeEmpty();
            metalink.Files.ShouldHaveSingleItem();
            metalink.Generator.ShouldBe("MirrorBrain 2.19.0 (see http://mirrorbrain.org/)");
            metalink.Origin.ShouldBe("https://mirrors.slackware.com/slackware/slackware-iso/slackware64-15.0-iso/slackware64-15.0-install-dvd.iso.metalink");
            metalink.PublishDate.ShouldBe("Fri, 23 Aug 2024 17:42:05 GMT");
            metalink.Publisher.ShouldBe("Slackware Linux");
            metalink.PublisherUrl.ShouldBe("https://mirrors.slackware.com");

            // Verify the file
            var file = metalink.Files[0];
            file.File.ShouldBe("slackware64-15.0-install-dvd.iso");
            file.Size.ShouldBe(3780542464);
            file.Hashes.ShouldNotBeNull();
            file.Hashes.ShouldNotBeEmpty();
            file.Hashes.Length.ShouldBe(3);
            file.Resources.ShouldNotBeNull();
            file.Resources.ShouldNotBeEmpty();
            file.Resources.Length.ShouldBe(52);
            file.Signatures.ShouldNotBeNull();
            file.Signatures.ShouldNotBeEmpty();
            file.Signatures.Length.ShouldBe(1);
            file.PieceInfo.ShouldNotBeNull();

            // Verify the three hashes
            var hashes = file.Hashes;
            hashes[0].HashSum.ShouldBe("f8418ef0ec2c0a205adf5dbc2f2a1971");
            hashes[0].HashSumType.ShouldBe("md5");
            hashes[1].HashSum.ShouldBe("2f3c4a95e37d313c16b2b4d203c77342fb950419");
            hashes[1].HashSumType.ShouldBe("sha1");
            hashes[2].HashSum.ShouldBe("4b32d575097f2238fa9dc1cd753286493a4be95a3004e9b1e81748b4f1d0c5da");
            hashes[2].HashSumType.ShouldBe("sha256");

            // Verify a resource
            var resources = file.Resources;
            resources[51].Location.ShouldBe("us");
            resources[51].Preference.ShouldBe(49);
            resources[51].Type.ShouldBe("https");
            resources[51].URL.ShouldBe("https://mirrors.kernel.org/slackware/slackware-iso/slackware64-15.0-iso/slackware64-15.0-install-dvd.iso");

            // Verify a signature
            var signatures = file.Signatures;
            signatures[0].SignatureFile.ShouldBe("slackware64-15.0-install-dvd.iso.asc");
            signatures[0].SignatureType.ShouldBe("pgp");
            signatures[0].SignatureContent.ShouldContain("-----BEGIN PGP SIGNATURE-----");

            // Verify piece information
            var pieceInfo = file.PieceInfo;
            pieceInfo.Length.ShouldBe(262144);
            pieceInfo.Type.ShouldBe("sha1");
            pieceInfo.Hashes.ShouldNotBeNull();
            pieceInfo.Hashes.ShouldNotBeEmpty();
            pieceInfo.Hashes.Length.ShouldBe(14422);
        }

        [TestMethod]
        public void TestParseMetalink4()
        {
            // Parse it
            Stream? stream = MetalinkLoader.LoadMetalinkStream("slackware15.iso.meta4");
            if (stream is null)
                Assert.Fail("Stream is null");
            var metalink = MetalinkParser.GetMetalinkFromStream(stream);

            // Verify the correctness
            metalink.ShouldNotBeNull();
            metalink.Dynamic.ShouldBeTrue();
            metalink.Files.ShouldNotBeEmpty();
            metalink.Files.ShouldHaveSingleItem();
            metalink.Generator.ShouldBe("MirrorBrain/2.19.0");
            metalink.Origin.ShouldBe("https://mirrors.slackware.com/slackware/slackware-iso/slackware64-15.0-iso/slackware64-15.0-install-dvd.iso.meta4");
            metalink.PublishDate.ShouldBe("2024-08-23T17:41:59Z");
            metalink.Publisher.ShouldBe("Slackware Linux");
            metalink.PublisherUrl.ShouldBe("https://mirrors.slackware.com");

            // Verify the file
            var file = metalink.Files[0];
            file.File.ShouldBe("slackware64-15.0-install-dvd.iso");
            file.Size.ShouldBe(3780542464);
            file.Hashes.ShouldNotBeNull();
            file.Hashes.ShouldNotBeEmpty();
            file.Hashes.Length.ShouldBe(3);
            file.Resources.ShouldNotBeNull();
            file.Resources.ShouldNotBeEmpty();
            file.Resources.Length.ShouldBe(52);
            file.Signatures.ShouldNotBeNull();
            file.Signatures.ShouldNotBeEmpty();
            file.Signatures.Length.ShouldBe(1);
            file.PieceInfo.ShouldNotBeNull();

            // Verify the three hashes
            var hashes = file.Hashes;
            hashes[0].HashSum.ShouldBe("f8418ef0ec2c0a205adf5dbc2f2a1971");
            hashes[0].HashSumType.ShouldBe("md5");
            hashes[1].HashSum.ShouldBe("2f3c4a95e37d313c16b2b4d203c77342fb950419");
            hashes[1].HashSumType.ShouldBe("sha-1");
            hashes[2].HashSum.ShouldBe("4b32d575097f2238fa9dc1cd753286493a4be95a3004e9b1e81748b4f1d0c5da");
            hashes[2].HashSumType.ShouldBe("sha-256");

            // Verify a resource
            var resources = file.Resources;
            resources[51].Location.ShouldBe("us");
            resources[51].Preference.ShouldBe(52);
            resources[51].Type.ShouldBe("https");
            resources[51].URL.ShouldBe("https://mirrors.kernel.org/slackware/slackware-iso/slackware64-15.0-iso/slackware64-15.0-install-dvd.iso");

            // Verify a signature
            var signatures = file.Signatures;
            signatures[0].SignatureFile.ShouldBe("slackware64-15.0-install-dvd.iso.asc");
            signatures[0].SignatureType.ShouldBe("application/pgp-signature");
            signatures[0].SignatureContent.ShouldContain("-----BEGIN PGP SIGNATURE-----");

            // Verify piece information
            var pieceInfo = file.PieceInfo;
            pieceInfo.Length.ShouldBe(262144);
            pieceInfo.Type.ShouldBe("sha-1");
            pieceInfo.Hashes.ShouldNotBeNull();
            pieceInfo.Hashes.ShouldNotBeEmpty();
            pieceInfo.Hashes.Length.ShouldBe(14422);
        }
    }
}
