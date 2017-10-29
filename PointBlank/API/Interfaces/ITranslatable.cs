﻿using System;
using System.Collections.Generic;
using PointBlank.API.Collections;

namespace PointBlank.API.Interfaces
{
    /// <summary>
    /// Makes a class translatable by adding translations to it
    /// </summary>
<<<<<<< HEAD
    public abstract class ITranslatable
    {
        #region Variables
        private TranslationList _Translations = null;
=======
    public abstract class Translatable
    {
        #region Variables
        private TranslationList _translations = null;
>>>>>>> master
        #endregion

        #region Properties
        /// <summary>
        /// The list of translations
        /// </summary>
        public TranslationList Translations
        {
            get
            {
<<<<<<< HEAD
                if (_Translations == null)
                    _Translations = DefaultTranslations;
                return _Translations;
=======
                if (_translations == null)
                    _translations = DefaultTranslations;
                return _translations;
>>>>>>> master
            }
        }
        #endregion

        #region Abstract Functions
        /// <summary>
        /// The directory inside the translations folder where the file will be save(leave null or empty to use default path)
        /// </summary>
        public abstract string TranslationDirectory { get; }
        /// <summary>
        /// The list of translations
        /// </summary>
        public abstract TranslationList DefaultTranslations { get; }
        /// <summary>
        /// The dictionary to save the ITranslatable instance to(set to null if the Translations and TranslationDirectory are static)
        /// </summary>
<<<<<<< HEAD
        public abstract Dictionary<Type, ITranslatable> TranslationDictionary { get; }
=======
        public abstract Dictionary<Type, Translatable> TranslationDictionary { get; }
>>>>>>> master
        #endregion
    }
}
