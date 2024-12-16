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
    /// Metalink file instance
    /// </summary>
	public class MetalinkResource
	{
        /// <summary>
        /// URL to the resource
        /// </summary>
        public string? URL { get; internal set; }
        /// <summary>
        /// Resource schema type (http, https, ftp, ...)
        /// </summary>
        public string? Type { get; internal set; }
        /// <summary>
        /// Country of the resource
        /// </summary>
        public string? Location { get; internal set; }
        /// <summary>
        /// Preference order
        /// </summary>
        public int Preference { get; internal set; }
    }
}
