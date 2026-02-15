using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Common.Pagination
{
    public class PaginationRequestParameters
    {
        const int maxPageSize = 50;
        private int _pageNumber = 1;
        private int _pageSize = 10;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < 1)
                    _pageSize = 1;
                else if (value > maxPageSize)
                    _pageSize = maxPageSize;
                else
                    _pageSize = value;
            }
        }
    }
}
