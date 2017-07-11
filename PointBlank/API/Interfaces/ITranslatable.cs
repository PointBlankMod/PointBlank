using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Collections;

namespace PointBlank.API.Interfaces
{
    /// <summary>
    /// Makes a class translatable by adding translations to it
    /// </summary>
    public interface ITranslatable
    {
        /// <summary>
        /// The directory inside the translations folder where the file will be save(leave null or empty to use default path)
        /// </summary>
        string TranslationDirectory { get; }
        /// <summary>
        /// The list of translations
        /// </summary>
        TranslationList Translations { get; }
        /// <summary>
        /// The dictionary to save the ITranslatable instance to(set to null if the Translations and TranslationDirectory are static)
        /// </summary>
        Dictionary<Type, ITranslatable> TranslationDictionary { get; }
    }
}
