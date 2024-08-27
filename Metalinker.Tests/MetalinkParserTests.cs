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
using System.IO;

namespace Metalinker.Tests
{
    [TestClass]
    public class MetalinkParserTests
    {
        [TestMethod]
        public void TestParseMetalink3()
        {
            Stream? stream = MetalinkLoader.LoadMetalinkStream("slackware15.iso.metalink");
            if (stream is null)
                Assert.Fail("Stream is null");
            var metalink = MetalinkParser.GetMetalinkFromStream(stream);
        }

        [TestMethod]
        public void TestParseMetalink4()
        {
            Stream? stream = MetalinkLoader.LoadMetalinkStream("slackware15.iso.meta4");
            if (stream is null)
                Assert.Fail("Stream is null");
            var metalink = MetalinkParser.GetMetalinkFromStream(stream);
        }
    }
}
