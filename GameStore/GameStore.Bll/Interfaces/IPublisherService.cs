using System.Collections.Generic;
using GameStore.Bll.DTO;

namespace GameStore.Bll.Interfaces
{
    public interface IPublisherService
    {
        void NewPublisher(PublisherDTO obj);
        void RemovePublisher(string name);
        void RemovePublisherById(int id);
        IEnumerable<PublisherDTO> GetAllPublisher(string lang);
        PublisherDTO GetById(int id, string lang);
        PublisherDTO GetByName(string name, string lang);
        void EditPublisher(PublisherDTO obj);
    }
}
