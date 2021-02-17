using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    // UnityOfWork representa o salvamento de uma ou várias 
    // entidades de uma forma transacional (todas de uma vez)
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }

        IFrequencyRepository FrequencyRepository { get; }

        IServiceRepository ServiceRepository { get; }

        IOrderHeaderRepository OrderHeaderRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }

        void Save();
    }
}
