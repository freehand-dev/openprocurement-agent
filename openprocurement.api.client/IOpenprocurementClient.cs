using openprocurement.api.client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace openprocurement.api.client
{
    public interface IOpenprocurementClient
    {
        
        /// <summary>
        /// Getting list of all tenders
        /// https://public.api.openprocurement.org/api/2.3/tenders?offset=2020-07-21T14:44:02.5005518+03:00&limit=10
        /// http://api-docs.openprocurement.org/en/latest/tenders.html?highlight=tenders
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Task<TendersResponse> GetTendersAsync(DateTime offset, int limit = 0);

        public Task<DataResponse<Tender>> GetTenderAsync(string Id);

        public Task<DataResponse<List<TenderDocument>>> GetTenderDocumentsAsync(string Id);

        public Task<DataResponse<List<TenderContract>>> GetTenderContractsAsync(string Id);

        /// <summary>
        /// Reading the tender documents list
        /// https://public.api.openprocurement.org/api/2.5/tenders/62a722e0afcb42eea8dd2c57f8c868f4/contracts
        /// http://api-docs.openprocurement.org/en/latest/tenders.html?highlight=tenders
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Task<DataResponse<List<TenderAward>>> GetTenderAwardsAsync(string Id);
    }
}
