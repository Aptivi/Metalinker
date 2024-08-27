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

namespace Metalinker.Instances
{
    /// <summary>
    /// Metalink instance
    /// </summary>
	public class Metalink
	{
        public string Generator { get; internal set; }
        public string Origin { get; internal set; }
        public bool Dynamic { get; internal set; }
        public string PublishDate { get; internal set; }
        public string Publisher { get; internal set; }
        public string PublisherUrl { get; internal set; }
    }
}
