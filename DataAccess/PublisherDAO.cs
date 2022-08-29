using BusinessObject;
using eBookStoreLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PublisherDAO
    {
        private static PublisherDAO instance = null;
        private static readonly object instanceLock = new object();

        private PublisherDAO()
        {

        }

        public static PublisherDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new PublisherDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<Publisher>> GetPublishersAsync()
        {
            var db = new eStoreContext();
            return await db.Publishers.ToListAsync();
        }

        public async Task<Publisher> GetPublisherAsync(int publisherId)
        {
            var db = new eStoreContext();
            return await db.Publishers.FindAsync(publisherId);
        }

        public async Task<Publisher> AddPublisherAsync(Publisher newPublisher)
        {
            CheckPublisher(newPublisher);
            var db = new eStoreContext();
            await db.Publishers.AddAsync(newPublisher);
            await db.SaveChangesAsync();

            return newPublisher;
        }

        public async Task<Publisher> UpdatePublisherAsync(Publisher updatedPublisher)
        {
            if (await GetPublisherAsync(updatedPublisher.PublisherId) == null)
            {
                throw new ApplicationException("Publisher does not exist!!");
            }
            CheckPublisher(updatedPublisher);
            var db = new eStoreContext();
            db.Publishers.Update(updatedPublisher);
            await db.SaveChangesAsync();
            return updatedPublisher;
        }

        public async Task DeletePublisherAsync(int publisherId)
        {
            Publisher deletedPublisher = await GetPublisherAsync(publisherId);
            if (deletedPublisher == null)
            {
                throw new ApplicationException("Publisher does not exist!!");
            }
            var db = new eStoreContext();
            db.Publishers.Remove(deletedPublisher);
            await db.SaveChangesAsync();
        }

        private void CheckPublisher(Publisher publisher)
        {
            publisher.PublisherName.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "Publisher Name is required!!",
                minLength: 2,
                minLengthErrorMessage: "Publisher Name must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "Publisher Name is limited to 255 characters!!"
                );

            publisher.City.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "",
                minLength: 2,
                minLengthErrorMessage: "Publisher City must have at least 2 characters!!",
                maxLength: 100,
                maxLengthErrorMessage: "Publisher City is limited to 100 characters!!"
                );

            publisher.State.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "",
                minLength: 2,
                minLengthErrorMessage: "Publisher State must have at least 2 characters!!",
                maxLength: 100,
                maxLengthErrorMessage: "Publisher State is limited to 100 characters!!"
                );

            publisher.Country.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "",
                minLength: 2,
                minLengthErrorMessage: "Publisher Country must have at least 2 characters!!",
                maxLength: 100,
                maxLengthErrorMessage: "Publisher Country is limited to 100 characters!!"
                );
        }
    }
}
