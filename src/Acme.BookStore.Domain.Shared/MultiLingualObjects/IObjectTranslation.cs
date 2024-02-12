using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.MultiLingualObjects
{
    public interface IObjectTranslation
    {
        string language { get; set; }
    }
}
