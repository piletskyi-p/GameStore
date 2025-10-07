using System.Collections.Generic;
using GameStore.Bll.DTO.TranslateDto;

namespace GameStore.Bll.DTO
{
    public class GenreDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public GenreDTO Parent { get; set; }

        public ICollection<GenreTranslateDto> GenreTranslates { get; set; }

        public int? ParentId { get; set; }

        public bool IsDeleted { get; set; }

        public List<GameDTO> Games { get; set; }
    }
}
