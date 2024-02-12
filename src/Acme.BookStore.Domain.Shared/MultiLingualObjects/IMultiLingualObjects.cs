using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.MultiLingualObjects
{
    public  interface IMultiLingualObjects<ITranslation> where ITranslation : class,IObjectTranslation
    {
        ICollection<ITranslation> Translations { get; set; }
    }
}
