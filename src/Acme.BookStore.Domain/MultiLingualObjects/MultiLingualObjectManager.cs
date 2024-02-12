using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.MultiLingualObjects
{
    public class MultiLingualObjectManager : ITransientDependency
    {
        protected const int MaxCultureFallbackDepth = 5;

        public  TTranslation FindTranslationAsync<TMultiLingual, TTranslation>(
            TMultiLingual multiLingual,
            string culture = null,
            bool fallbackToParentCultures = true)
            where TMultiLingual : IMultiLingualObjects<TTranslation>
            where TTranslation : class, IObjectTranslation
        {
            culture ??= CultureInfo.CurrentUICulture.Name;

            if (multiLingual.Translations.IsNullOrEmpty())
            {
                return null;
            }

            var translation = multiLingual.Translations.FirstOrDefault(pt => pt.language == culture);
            if (translation != null)
            {
                return translation;
            }

            if (fallbackToParentCultures)
            {
                translation = GetTranslationBasedOnCulturalRecursive(
                    CultureInfo.CurrentUICulture.Parent,
                    multiLingual.Translations,
                    0
                );

                if (translation != null)
                {
                    return translation;
                }
            }

            return null;
        }

        protected TTranslation GetTranslationBasedOnCulturalRecursive<TTranslation>(
            CultureInfo culture, ICollection<TTranslation> translations, int currentDepth)
            where TTranslation : class, IObjectTranslation
        {
            if (culture == null ||
                culture.Name.IsNullOrWhiteSpace() ||
                translations.IsNullOrEmpty() ||
                currentDepth > MaxCultureFallbackDepth)
            {
                return null;
            }

            var translation = translations.FirstOrDefault(pt => pt.language.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));
            return translation ?? GetTranslationBasedOnCulturalRecursive(culture.Parent, translations, currentDepth + 1);
        }
    }
}
