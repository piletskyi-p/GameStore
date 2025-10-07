using System.Collections.Generic;
using GameStore.Bll.DTO.TranslateDto;

namespace GameStore.Bll.DTO
{
    public class PublisherDTO
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<GameDTO> Games { get; set; }

        public IEnumerable<PublisherTranslateDto> PublisherTranslate { get; set; }
    }
}
