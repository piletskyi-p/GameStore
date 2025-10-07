using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.Bll.DTO;
using GameStore.Bll.Interfaces;
using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Translate;
using GameStore.Dal.Interfaces;

namespace GameStore.Bll.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventLogger _eventLogger;

        public GenreService(IUnitOfWork unitOfWork, IEventLogger eventLogger)
        {
            _eventLogger = eventLogger;
            _unitOfWork = unitOfWork;
        }

        public void Create(GenreDTO genreDto)
        {
            var genre = Mapper.Map<Genre>(genreDto);
            foreach (var translate in genre.GenreTranslates)
            {
                translate.Language = _unitOfWork.LanguageRepository.FindById(translate.LanguageId);
            }

            _unitOfWork.GenreRepository.Create(genre);
            _unitOfWork.Save();
            _eventLogger.LogCreate(Mapper.Map<Genre>(genre));
        }

        public void Delete(int id)
        {
            var genre = _unitOfWork.GenreRepository.FindById(id);
            if (genre != null)
            {
                _unitOfWork.GenreRepository.Delete(id);
                _unitOfWork.Save();
                _eventLogger.LogDelete(Mapper.Map<Genre>(genre));
            }
        }

        public IEnumerable<GenreDTO> GetAll(string lang)

        {
            var genre = _unitOfWork.GenreRepository.Get(
                genreRepository => genreRepository.GenreTranslates,
                genreRepository => genreRepository.Parent,
                genreRepository => genreRepository.GenreTranslates.Select(tr => tr.Language)).ToList();

            if (genre.Any())
            {
                var genreDto = Mapper.Map<List<Genre>, List<GenreDTO>>(genre);
                for (int i = 0; i < genre.Count(); ++i)
                {
                    genreDto[i].Name = genre[i].GenreTranslates
                        .FirstOrDefault(tr => tr.Language.Key == lang)?.Name;
                }

                return genreDto;
            }

            return null;
        }

        public IEnumerable<GenreDTO> GetByGameKey(string key, string lang)
        {
            var temp = _unitOfWork.GameRepository.Get(gm => gm.Key == key,
                genreRepository => genreRepository.Genres,
                genreRepository => genreRepository.Genres.Select(genre => genre.GenreTranslates),
                genreRepository => genreRepository.Genres.Select(genre => genre.GenreTranslates
                    .Select(genres => genres.Language)),
                genreRepository => genreRepository.Genres.Select(genre => genre.Parent),
            genreRepository => genreRepository.Genres.Select(genre => genre.Games));
            var gameGenres = temp.FirstOrDefault()?.Genres.ToList();

            if (gameGenres != null)
            {
                var genresDto = Mapper.Map<List<Genre>, List<GenreDTO>>(gameGenres);
                for (int i = 0; i < genresDto.Count(); ++i)
                {
                    genresDto[i].Name = gameGenres[i].GenreTranslates.FirstOrDefault(tr => tr.Language.Key == lang)?.Name;
                }

                return genresDto;
            }

            return new List<GenreDTO>();
        }

        public GenreDTO GetById(int id, string lang)
        {
            var genre = _unitOfWork.GenreRepository.Get(
                    genres => genres.Id == id, 
                    genreRepository => genreRepository.GenreTranslates,
                    genreRepository => genreRepository.GenreTranslates
                        .Select(genres => genres.Language),
                    genreRepository => genreRepository.Parent)
                .FirstOrDefault();

            if (genre != null)
            {
                var genreDto = Mapper.Map<GenreDTO>(genre);
                genreDto.Name = genre.GenreTranslates.FirstOrDefault(tr => tr.Language.Key == lang)?.Name;

                return genreDto;
            }

            return null;
        }

        public void Update(GenreDTO genreDto)
        {
            var genre = _unitOfWork.GenreRepository.FindById(genreDto.Id,
                repository => repository.Games,
                repository => repository.GenreTranslates);

            var genreOldStr = genre.GenreTranslates;

            genre.GenreTranslates = Mapper.Map<ICollection<GenreTranslate>>(genreDto.GenreTranslates);

            _unitOfWork.GenreRepository.Update(genre);
            _unitOfWork.Save();

            genre.GenreTranslates = genreOldStr;
            genre.Games = null;

            _eventLogger.LogUpdate(Mapper.Map<Genre>(genre), Mapper.Map<Genre>(genreDto));
        }
    }
}