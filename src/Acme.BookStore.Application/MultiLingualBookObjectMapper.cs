using Acme.BookStore.Books;
using Acme.BookStore.MultiLingualObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Acme.BookStore
{
    public class MultiLingualBookObjectMapper : IObjectMapper<Book, BookDto>, ITransientDependency
    {
        private readonly MultiLingualObjectManager _multiLingualObjectManager;

        private readonly ISettingProvider _settingProvider;

        public MultiLingualBookObjectMapper(
            MultiLingualObjectManager multiLingualObjectManager,
            ISettingProvider settingProvider)
        {
            _multiLingualObjectManager = multiLingualObjectManager;
            _settingProvider = settingProvider;
        }

        public BookDto Map(Book source)
        {
            var translation = AsyncHelper.RunSync(async () =>
                _multiLingualObjectManager.FindTranslationAsync<Book, BookTranslation>(source));

            return new BookDto
            {
                Id = source.Id,
                //AuthorId = source.AuthorId,
                Type = source.Type,
                Name = translation?.Name ?? source.Name,
                PublishDate = source.PublishDate,
                Price = source.Price,
                language = translation?.language ?? AsyncHelper.RunSync(() => _settingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage)),
                CreationTime = source.CreationTime,
                CreatorId = source.CreatorId,
                LastModificationTime = source.LastModificationTime,
                LastModifierId = source.LastModifierId,
       
                
            };
        }

        public BookDto Map(Book source, BookDto destination)
        {
            return default;
        }
    }
}
