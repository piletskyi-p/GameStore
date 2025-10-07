using System.Collections.Generic;
using GameStore.Bll.DTO.TranslateDto;

namespace GameStore.Bll.DTO
{ 
    public class PlatformDTO
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public bool IsDeleted { get; set; }

        public List<GameDTO> Games { get; set; }

        public IEnumerable<PlatformTranslateDto> PlatformTranslates { get; set; }
    }
}
