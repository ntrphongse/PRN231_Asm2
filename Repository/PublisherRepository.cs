using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PublisherRepository : IPublisherRepository
    {
        public async Task<Publisher> AddPublisherAsync(Publisher newPublisher)
            => await PublisherDAO.Instance.AddPublisherAsync(newPublisher);

        public async Task DeletePublisherAsync(int publisherId)
            => await PublisherDAO.Instance.DeletePublisherAsync(publisherId);

        public async Task<Publisher> GetPublisherAsync(int publisherId) => await PublisherDAO.Instance.GetPublisherAsync(publisherId);

        public async Task<IEnumerable<Publisher>> GetPublishersAsync()
            => await PublisherDAO.Instance.GetPublishersAsync();

        public async Task<Publisher> UpdatePublisherAsync(Publisher updatedPublisher) => await PublisherDAO.Instance.UpdatePublisherAsync(updatedPublisher);
    }
}
