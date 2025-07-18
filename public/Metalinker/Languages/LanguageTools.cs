﻿//
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

using LocaleStation.Tools;
using Metalinker.Localized;

namespace Metalinker.Languages
{
    internal static class LanguageTools
    {
        private const string localType = "Metalinker";

        internal static string GetLocalized(string id)
        {
            if (!LanguageCommon.IsCustomActionDefined(localType))
                LanguageCommon.AddCustomAction(localType, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            return LanguageCommon.Translate(id);
        }
    }
}
