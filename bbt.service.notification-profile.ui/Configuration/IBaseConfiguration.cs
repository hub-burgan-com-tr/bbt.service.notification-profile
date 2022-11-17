using System;
using System.Collections.Generic;


namespace bbt.service.notification.ui.Configuration
{
    public interface IBaseConfiguration
    {
        TClass Get<TClass>();
    }
}
