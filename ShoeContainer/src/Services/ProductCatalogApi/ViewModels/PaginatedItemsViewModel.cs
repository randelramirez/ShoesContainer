using System.Collections.Generic;

namespace ProductCatalogApi.ViewModels
{
    public class PaginatedItemsViewModel<TEntity> where TEntity : class
    {
        public int PageSize { get;  }
        
        public  int PageIndex { get; }
        
        public  long Count { get; }

        public IEnumerable<TEntity> Data { get;  }

        public PaginatedItemsViewModel(int pageIndex, long count, IEnumerable<TEntity> data, int pageSize)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }
    }
}