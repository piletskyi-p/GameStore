using GameStore.Dal.Entities;
using GameStore.Dal.Entities.Translate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using GameStore.Dal.Entities.Entities;

namespace GameStore.Dal
{
    public class DataBaseConnectionInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DataBaseConnection>
    {
        protected override void Seed(DataBaseConnection context)
        {
            var languageList = new List<Language>
                            {
                                new Language
                                {
                                    Id = 1,
                                    Key = "en",
                                    Name = "English"
                                },
                                new Language
                                {
                                    Id = 2,
                                    Key = "ru",
                                    Name = "Russian"
                                }
                            };

            context.Languages.AddRange(languageList);
            context.SaveChanges();

            var genreList = new List<Genre>
                            {
                                new Genre
                                {
                                    Id = 1,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 1,
                                            Name = "Strategy",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 1,
                                            Name = "Стратегия",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = null,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 2,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 2,
                                            Name = "RTS",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 2,
                                            Name = "RTS",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = 1,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 3,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 3,
                                            Name = "TBS",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 3,
                                            Name = "TBS",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = 1,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 4,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 4,
                                            Name = "RPG",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 4,
                                            Name = "RPG",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = null,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 5,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 5,
                                            Name = "Sport",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 5,
                                            Name = "Спорт",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = null,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 6,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 6,
                                            Name = "Races",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 6,
                                            Name = "Гонки",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = null,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 7,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 7,
                                            Name = "Rally",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 7,
                                            Name = "Ралли",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = 6,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 8,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 8,
                                            Name = "Arcade",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 8,
                                            Name = "Аркада",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = 6,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 9,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 9,
                                            Name = "Formula",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 9,
                                            Name = "Формула",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = 6,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 10,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 10,
                                            Name = "Off-road",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 10,
                                            Name = "Внедорожный",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = 6,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 11,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 11,
                                            Name = "Action",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 11,
                                            Name = "Действие",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = null,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 12,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 12,
                                            Name = "FPS",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 12,
                                            Name = "FPS",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = 11,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 13,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 13,
                                            Name = "TPS",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 13,
                                            Name = "TPS",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = 11,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 14,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 14,
                                            Name = "Misc.",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 14,
                                            Name = "Misc.",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = 11,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 15,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 15,
                                            Name = "Adventure",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 15,
                                            Name = "Приключение",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId =null,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 16,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 16,
                                            Name = "Puzzle & Skill",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 16,
                                            Name = "Головоломки и умения",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = null,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 17,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 17,
                                            Name = "Misc.",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 17,
                                            Name = "Misc.",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = null,
                                    Games = new List<Game>()
                                },
                                new Genre
                                {
                                    Id = 18,
                                    GenreTranslates = new List<GenreTranslate>
                                    {
                                        new GenreTranslate
                                        {
                                            GenreId = 17,
                                            Name = "Other",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new GenreTranslate
                                        {
                                            GenreId = 17,
                                            Name = "Другие",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    ParentId = null,
                                    Games = new List<Game>()
                                }
                            };

            context.Genres.AddRange(genreList);
            context.SaveChanges();

            var platformList = new List<Platform>
                            {
                                new Platform
                                {
                                    Id = 1,
                                    PlatformTranslates = new List<PlatformTranslate>
                                    {
                                        new PlatformTranslate
                                        {
                                            PlatformId = 1,
                                            Type = "Mobile",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PlatformTranslate
                                        {
                                            PlatformId = 1,
                                            Type = "Мобильные",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                },
                                new Platform
                                {
                                    Id = 2,
                                    PlatformTranslates = new List<PlatformTranslate>
                                    {
                                        new PlatformTranslate
                                        {
                                            PlatformId = 2,
                                            Type = "Browser",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PlatformTranslate
                                        {
                                            PlatformId = 2,
                                            Type = "Браузер",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                },
                                new Platform
                                {
                                    Id = 3,
                                    PlatformTranslates = new List<PlatformTranslate>
                                    {
                                        new PlatformTranslate
                                        {
                                            PlatformId = 3,
                                            Type = "Desktop",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PlatformTranslate
                                        {
                                            PlatformId = 3,
                                            Type = "Компьютерная",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                },
                                new Platform
                                {
                                    Id = 4,
                                    PlatformTranslates = new List<PlatformTranslate>
                                    {
                                        new PlatformTranslate
                                        {
                                            PlatformId = 4,
                                            Type = "Console",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PlatformTranslate
                                        {
                                            PlatformId = 4,
                                            Type = "Консоль",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                },
                                new Platform
                                {
                                    Id = 5,
                                    PlatformTranslates = new List<PlatformTranslate>
                                    {
                                        new PlatformTranslate
                                        {
                                            PlatformId = 5,
                                            Type = "Other",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PlatformTranslate
                                        {
                                            PlatformId = 5,
                                            Type = "Другие",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                }
                        };

            context.PlatformTypes.AddRange(platformList);
            context.SaveChanges();

            var publisherList = new List<Publisher>
                            {
                                new Publisher
                                {
                                    Id = 1,
                                    CompanyName = "Ubisoft",
                                    PublisherTranslate = new List<PublisherTranslate>
                                    {
                                        new PublisherTranslate
                                        {
                                            PublisherId = 1,
                                            Description = "Ubisoft is a French video game publisher headquartered " +
                                                          "in Montreuil. It is known for publishing games for " +
                                                          "several acclaimed video game franchises including " +
                                                          "Assassin's Creed, Far Cry, Just Dance, Prince of Persia," +
                                                          " Rayman, Raving Rabbids, and Tom Clancy's. It is the fourth" +
                                                          " largest publicly-traded game company in the Americas and " +
                                                          "Europe after Activision Blizzard, Electronic Arts, and Take-Two " +
                                                          "Interactive in terms of revenue and market capitalisation.[5]",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PublisherTranslate
                                        {
                                            PublisherId = 1,
                                            Description = "Ubisoft - французский издатель видеоигр со штаб-квартирой" +
                                                          "в Монтрёе. Он известен изданием игр для" +
                                                          "несколько известных франшиз видеоигр, включая" +
                                                          "Символ Assassin's Creed, Far Cry, Just Dance, Prince of Persia»," +
                                                          "Rayman, Raving Rabbids и Tom Clancy's. Это четвертый" +
                                                          "крупнейшая публичная торговая компания в Америке и" +
                                                          "Европа после Activision Blizzard, Electronic Arts и Take-Two" +
                                                          "Интерактивная с точки зрения доходов и рыночной капитализации [5].",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    HomePage = "www.ubisoft.com",
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                },
                                new Publisher
                                {
                                    Id = 2,
                                    CompanyName = "Gameloft",
                                    PublisherTranslate = new List<PublisherTranslate>
                                    {
                                        new PublisherTranslate
                                        {
                                            PublisherId = 2,
                                            Description = "Gameloft SE is a French video game publisher based in " +
                                                          "Paris, founded in 1999 by Ubisoft co-founder Michel Guillemot. " +
                                                          "The company operates 21 development studios worldwide, and " +
                                                          "publishes games with a special focus on the mobile games market." +
                                                          " Formerly public company traded at the Paris Bourse, " +
                                                          "Gameloft was fully acquired by French media conglomerate " +
                                                          "Vivendi in throughout 2016.",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PublisherTranslate
                                        {
                                            PublisherId = 2,
                                            Description = "Gameloft SE - французский издатель видеоигр, основанный на" +
                                                            "Париж, основанный в 1999 году соучредителем Ubisoft Мишелем Гиллемотом" +
                                                         "В компании работает 21 студия разработки по всему миру, а" +
                                                            "публикует игры с особым акцентом на рынке мобильных игр»." +
                                                        "Ранее публичная компания торговала в Парижской фондовой бирже" +
                                                        "Gameloft был полностью приобретен французским конгломератом СМИ" +
                                                        "Vivendi в течение 2016 года",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    HomePage = "www.gameloft.com",
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                },
                                new Publisher
                                {
                                    Id = 3,
                                    CompanyName = "Activision",
                                    PublisherTranslate = new List<PublisherTranslate>
                                    {
                                        new PublisherTranslate
                                        {
                                            PublisherId = 3,
                                            Description = "Activision Publishing, Inc. is an American video game publisher. " +
                                                          "It was founded on October 1, 1979[4] and was the world's first " +
                                                          "independent developer and distributor of video games for gaming " +
                                                          "consoles. Its first products were cartridges for the Atari 2600 " +
                                                          "video console system published from July 1980 for the US market " +
                                                          "and from August 1981 for the international market (UK).",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PublisherTranslate
                                        {
                                            PublisherId = 3,
                                            Description = "Activision Publishing, Inc. является издателем американской видеоигры." +
                                            "Он был основан 1 октября 1979 года [4] и был первым в мире" +
                                            "независимый разработчик и дистрибьютор видеоигр для игр" +
                                            "Первыми продуктами были картриджи для Atari 2600" +
                                            "система видеоконференций, опубликованная с июля 1980 года для американского рынка" +
                                            "и с августа 1981 года для международного рынка (Великобритания)",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    HomePage = "www.activision.com/",
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                },
                                new Publisher
                                {
                                    Id = 4,
                                    CompanyName = "Capcom",
                                  PublisherTranslate = new List<PublisherTranslate>
                                    {
                                        new PublisherTranslate
                                        {
                                            PublisherId = 4,
                                            Description = "Capcom Co., Ltd. (Hepburn: Kabushiki-gaisha" +
                                                          " Kapukon) is a Japanese video game developer and publisher[4] known " +
                                                          "for creating numerous multi-million selling game franchises, " +
                                                          "including Street Fighter, Mega Man, Resident Evil, Dino Crisis," +
                                                          " Devil May Cry, Ace Attorney, Monster Hunter, and Dead Rising, as " +
                                                          "well as games based on the Disney animated properties. Established " +
                                                          "in 1979,[5] it has become an international enterprise with " +
                                                          "subsidiaries in North America, Europe, and East Asia.[6]",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PublisherTranslate
                                        {
                                            PublisherId = 4,
                                            Description = "Capcom Co., Ltd. (Хепберн: Кабушики-гайша" +
                                            "Kapukon» - японский разработчик и издатель видеоигр [4], известный " +
                                            "dля создания многочисленных многомиллионных франшиз для продажи игр" +
                                            "включая Street Fighter, Mega Man, Resident Evil, Dino Crisis»," +
                                            "Дьявол может кричать, туз-прокурор, охотник за монстрами и« мертвый рост », как" +
                                            "а также игры, основанные на анимированных свойствах Диснея." +
                                            "в 1979 году [5] он стал международным предприятием с" +
                                            "дочерние компании в Северной Америке, Европе и Восточной Азии [6].",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    HomePage = "www.capcom.com/",
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                },
                                new Publisher
                                {
                                    Id = 5,
                                    CompanyName = "Other",
                                    PublisherTranslate = new List<PublisherTranslate>
                                    {
                                        new PublisherTranslate
                                        {
                                            PublisherId = 5,
                                            Description = "Unknown",
                                            LanguageId = 1,
                                            Language = context.Languages.First(tr => tr.Id == 1)
                                        },
                                        new PublisherTranslate
                                        {
                                            PublisherId = 5,
                                            Description = "Неизвестный",
                                            LanguageId = 2,
                                            Language = context.Languages.First(tr => tr.Id == 2)
                                        }
                                    },
                                    HomePage = "Unknow",
                                    IsDeleted = false,
                                    Games = new List<Game>()
                                }
                            };

            context.Publisher.AddRange(publisherList);
            context.SaveChanges();

            var imageList = new List<Image>
                            {
                                new Image
                                {
                                    Id = 1,
                                    Name = "ME3.png",
                                    GameKey = "ME3"
                                },
                                new Image
                                {
                                    Id = 2,
                                    Name = "TheWithcer3.jpg",
                                    GameKey = "TW3"
                                },
                                new Image
                                {
                                    Id = 3,
                                    Name = "Darkness2.jpg",
                                    GameKey = "DII"
                                },
                                new Image
                                {
                                    Id = 4,
                                    Name = "Crysis.jpg",
                                    GameKey = "CR"
                                },
                                new Image
                                {
                                    Id = 5,
                                    Name = "NeedForSpeedHotPursuit.jpg",
                                    GameKey = "NFSHP"
                                },
                                new Image
                                {
                                    Id = 6,
                                    Name = "NeedForSpeedUndercover.jpg",
                                    GameKey = "NFSU"
                                },
                                new Image
                                {
                                    Id = 7,
                                    Name = "PES.jpg",
                                    GameKey = "PES"
                                },
                                new Image
                                {
                                    Id = 8,
                                    Name = "Sonic4EpisodeII.jpg",
                                    GameKey = "Sonic"
                                },
                                new Image
                                {
                                    Id = 9,
                                    Name = "AssassinCreed.jpg",
                                    GameKey = "Assassins"
                                },
                                new Image
                                {
                                    Id = 10,
                                    Name = "FIFA18.jpg",
                                    GameKey = "FIFA18"
                                },
                                new Image
                                {
                                    Id = 11,
                                    Name = "CityMotoRacer.jpg",
                                    GameKey = "CMR"
                                },
                                new Image
                                {
                                    Id = 12,
                                    Name = "Fortnite.jpg",
                                    GameKey = "FN"
                                }
                            };

            context.Images.AddRange(imageList);
            context.SaveChanges();

            var gameList = new List<Game>();
            Game g = new Game
            {
                Id = 1,
                Key = "ME3",
                Name = "Mass Effect 3",
                ImageId = 1,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 1,
                                        Description = "Mass Effect 3 looks great, and the set pieces are really " +
                                                      "impressive. The cover based combat seems even tighter than in " +
                                                      "the previous game too, and Bioware are so confident about the " +
                                                      "quality of the action that players can choose an \'Action\' mode " +
                                                      "that removes all the RPG choice elements of conversations. At " +
                                                      "the other end of the spectrum you can choose to focus on the " +
                                                      "story, with less challenging combat. Most players will probably " +
                                                      "stick with the Goldilocks RPG mode, which is give you the most " +
                                                      "complete experience.\r\n\r\nThe first Mass Effect took story " +
                                                      "telling and player choice to new levels in video gaming, while " +
                                                      "the second was more accessible and perfected the third person " +
                                                      "shooting action. Mass Effect 3 has a lot to live up to, but has " +
                                                      "a fantastic platform to build upon.",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 1,
                                        Description = "Mass Effect 3 выглядит великолепно, и набор штук действительно" +
                                        "впечатляет. Бой на обложке кажется еще более жестким, чем в" +
                                        "предыдущая игра тоже, и Bioware настолько уверены в" +
                                        "качество действия, которое игроки могут выбрать в режиме действия" +
                                        ", который удаляет все элементы выбора для выбора RPG. При значении" +
                                        "на другом конце спектра вы можете сосредоточиться на" +
                                        "история, с менее сложной битвой. Большинство игроков, вероятно," +
                                        "придерживайтесь режима RPG Goldilocks, который дает вам больше всего" +
                                        "Полный опыт. \r \n \r \n Первый Mass Effect взял историю" +
                                        "говорить и выбирать игрока на новые уровни в видеоиграх, в то время как" +
                                        "второй был более доступным и совершенным третьим лицом" +
                                        "Массовый эффект 3 имеет много возможностей, чтобы соответствовать, но имеет" +
                                        "фантастическая платформа, на которой можно опираться.",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 50,
                UnitInStock = 25,
                IsDiscontinued = false,
                PublisherId = 4,
                RatingMarks = "3,3,4,1,2,4,5",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2012, 2, 4, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 11),
                                    genreList.First(id => id.Id == 4)
                        },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 2),
                                    platformList.First(id => id.Id == 3)
                        }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 2,
                Key = "TW3",
                Name = "The Witcher 3: Wild Hunt ",
                ImageId = 2,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 2,
                                        Description = "The Witcher 3: Wild Hunt is an open world role playing " +
                                                      "game (RPG), which invites you once again to play the part " +
                                                      "of a Witcher, Geralt of Rivia. It's a vast and expansive " +
                                                      "adventure, set in a fascinating world that's varied, original," +
                                                      " and memorable. ",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 2,
                                        Description = "Ведьмак 3: Дикая охота - это игра в открытый мир" +
                                        "игра (RPG), которая приглашает вас еще раз сыграть роль" +
                                        "Ведьмак, Геральт Ривский, это огромный и экспансивный" +
                                        "приключение, установленное в увлекательном мире, который разнообразен, оригинален»," +
                                        "и запоминающимся».",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 100,
                UnitInStock = 40,
                IsDiscontinued = false,
                PublisherId = 3,
                Rating = 4.2,
                RatingMarks = "1, 3, 4, 2,2, 5, 4, 5",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2016, 2, 4, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 11),
                                    genreList.First(id => id.Id == 3),
                                    genreList.First(id => id.Id == 4)
                        },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 2),
                                    platformList.First(id => id.Id == 3)
                        }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 3,
                Key = "DII",
                Name = "The Darkness II",
                ImageId = 3,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 3,
                                        Description = "The Darkness II sees the return of Jackie Estacado, the " +
                                                      "mobster with gruesome supernatural powers. This demo requires " +
                                                      "the application Steam to run.\r\n\r\nThe game is a direct sequel, " +
                                                      "set two years after the excellent first game. Jackie has controlled " +
                                                      "his Darkness powers, but of course events leave him with no alternative " +
                                                      "but to use them again. Like its predecessor, The Darkness II alters the " +
                                                      "traditional FPS with the two Darkness tentacles that give you interesting " +
                                                      "abilities and gruesome killing moves.",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 3,
                                        Description = "Тьма II видит возвращение Джеки Эстакадо," +
                                        "гангстер с ужасающими сверхъестественными способностями». Это демо требует" +
                                        "приложение Steam для запуска. \r \n \r \n Игры - это прямое продолжение" +
                                        "установил два года после отличной первой игры. Джеки контролировал" +
                                        "его сила Тьмы, но, конечно, события оставляют его без альтернативы" +
                                        "но использовать их снова. Как и его предшественник, The Darkness II изменяет" +
                                        "традиционный FPS с двумя щупальцами Тьмы, которые дают вам интересные" +
                                        "способности и ужасные убийственные движения».",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 25,
                UnitInStock = 60,
                IsDiscontinued = false,
                PublisherId = 1,
                Rating = 4.4,
                RatingMarks = "3, 3, 1, 1, 2, 2, 2",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2014, 2, 4, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 11),
                                    genreList.First(id => id.Id == 3)
                        },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 2),
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 1)
                        }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 4,
                Key = "CR",
                Name = "Crysis",
                ImageId = 4,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 4,
                                        Description = "Crysis is a futuristic first-person-shooter that " +
                                                      "will immerse you into some action-packed missions in a " +
                                                      "remote island that has been invaded by an alien race. As a " +
                                                      "member of a highly qualified squadron, you\'ll take part in " +
                                                      "the US government plan to recover the island and wipe out those " +
                                                      "nasty aliens.\r\n\r\nThis Crysis trailer showcases the fantastic" +
                                                      " graphics in all their CryEngine 2-feulled glory. It\'s a very short" +
                                                      " trailer though, and feels a little rough around the edges. That said," +
                                                      " it gives you a great insight into Crysis and is the perfect stage to" +
                                                      " show off the DirectX 10 abilities of Vista and Windows 7.",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 4,
                                        Description = "Crysis - футуристический шутер от первого лица, который" +
                                        "погрузит вас в несколько боевых миссий в" +
                                        "отдаленный остров, который был захвачен инопланетной расой. Как" +
                                        "член высококвалифицированной эскадры, вы примете участие в" +
                                        "правительство США планирует восстановить остров и уничтожить тех, кто" +
                                        "противные инопланетяне». Этот трейлер Crysis демонстрирует " +
                                        "графика во всей их критической славе CryEngine 2. Это очень короткая" +
                                        "трейлер, хотя и чувствует себя немного грубо по краям. Это говорит:" +
                                        "это дает вам отличное представление о Crysis и является идеальной стадией для" +
                                        "продемонстрировать возможности DirectX 10 для Vista и Windows 7.",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 40,
                UnitInStock = 70,
                IsDiscontinued = false,
                PublisherId = 4,
                Rating = 3.3,
                RatingMarks = "3, 2, 4, 1, 2, 5, 5",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2013, 2, 4, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 11)
                                },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 1)
                                }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 5,
                Key = "NFSHP",
                Name = "Need For Speed: Hot Pursuit",
                ImageId = 5,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 5,
                                        Description = "Need For Speed: Hot Pursuit is a refreshing entry in " +
                                                      "the long running series. Made by Burnout developers Criterion, " +
                                                      "it\'s a blisteringly fast arcade racing game of cops and " +
                                                      "robbers.\r\n\r\nThe setting in Need For Speed: Hot Pursuit is " +
                                                      "Seacrest County, which is an absolutely gorgeous speeding playground, " +
                                                      "with varied scenery to accompany the big sweeping roads, with minimal " +
                                                      "traffic. There are night and day races, and great weather effects. All " +
                                                      "of this serves as eye candy for the racing, which is firmly at the arcade " +
                                                      "end of the spectrum.",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 5,
                                        Description = "Need For Speed: Hot Pursuit - это освежающая запись в" +
                                        "длинная серия» Сделано разработчиками Burnout Criterion, " +
                                        "это невероятно быстрая аркадная гоночная игра полицейских и" +
                                        "Грабители. \r \n \r \nУстановка в Need For Speed: Hot Pursuit -" +
                                        "Seacrest County, которая является абсолютно великолепной скоростной площадкой" +
                                        "с разнообразными пейзажами, чтобы сопровождать большие широкие дороги, с минимальными" +
                                        "Движение: есть ночные и дневные гонки и отличные погодные эффекты». Все " +
                                        "из этого служит глазная конфета для гонок, которая прочно находится в аркаде" +
                                        "конец спектра»",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 100,
                UnitInStock = 70,
                IsDiscontinued = false,
                PublisherId = 4,
                Rating = 2,
                RatingMarks = "1, 1, 4, 1, 2, 4, 5",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2013, 2, 4, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 6),
                                    genreList.First(id => id.Id == 7),
                                    genreList.First(id => id.Id == 8),
                                },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 1)
                                }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 6,
                Key = "NFSU",
                Name = "Need For Speed: Undercover",
                ImageId = 6,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 6,
                                        Description = "Need For Speed is one of the best know franchises " +
                                                      "in racing games, and this installment, Need for Speed: " +
                                                      "Undercover, features the same mix of high octane arcade-style " +
                                                      "driving.\r\n\r\nNeed for Speed: Undercover returns to the police " +
                                                      "chases of earlier games, with big-budget production, cinematic cut " +
                                                      "scenes and flashy graphics that make the game look like an enticing " +
                                                      "prospect. The races and missions you will undertake are nothing new " +
                                                      "for the series, and in fact players will struggle to find many " +
                                                      "differences between this and some earlier titles.",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 6,
                                        Description = "Need For Speed - одна из лучших франшиз" +
                                        "в гоночных играх, и этот взнос, Need for Speed:" +
                                        "Undercover, имеет то же самое сочетание высокооктанового аркадного стиля" +
                                        "Вождение. \r \n \r \n Нид для скорости: тайный возвращается в полицию" +
                                        "преследования предыдущих игр, с крупнобюджетным производством, кинематографическим вырезом" +
                                        "сцены и кричащая графика, которые делают игру похожей на соблазнительную" +
                                        "Перспективы.Гонки и миссии, которые вы возьмете, ничего нового" +
                                        "для серии, и на самом деле игроки будут бороться, чтобы найти много" +
                                        "различия между этим и некоторыми более ранними названиями",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 90,
                UnitInStock = 50,
                IsDiscontinued = false,
                PublisherId = 4,
                Rating = 2.6,
                RatingMarks = "3, 4, 4, 1, 2, 1, 1",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2012, 2, 4, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 6),
                                    genreList.First(id => id.Id == 7),
                                    genreList.First(id => id.Id == 8),
                                },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 1)
                                }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 7,
                Key = "PES",
                Name = "Pro Evolution Soccer",
                ImageId = 7,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 7,
                                        Description = "Pro Evolution Soccer 2011 represents a new chapter in the " +
                                                      "long-running story of the popular football sim " +
                                                      "series.\r\n\r\nThe game features a series of significant, " +
                                                      "much-needed improvements to PES 2010. Gameplay has been tweaked " +
                                                      "to improve control, more game modes have been added, and the " +
                                                      "graphics and overall presentation of Pro Evolution Soccer 2011 " +
                                                      "are breathtaking.",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 7,
                                        Description = "Pro Evolution Soccer 2011 представляет новую главу в" +
                                        "Долгоиграющая история популярного футбольного сима" +
                                        "серия. \r \n \r \n В игре есть серия значимых" +
                                        "очень необходимые улучшения для PES 2010. Геймплей был изменен" +
                                        "чтобы улучшить контроль, добавлено больше игровых режимов, а" +
                                        "графика и общая презентация Pro Evolution Soccer 2011" +
                                        "захватывают дух",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 75,
                UnitInStock = 60,
                IsDiscontinued = false,
                PublisherId = 4,
                Rating = 4.8,
                RatingMarks = "4, 3, 2, 1, 2, 4, 1",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2012, 2, 4, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 5)
                                },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 1)
                                }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 8,
                Key = "Sonic",
                Name = "Sonic 4 Episode II",
                ImageId = 8,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 8,
                                        Description = "Sonic 4 Episode II is a pretty light game " +
                                                      "that does not require as much storage than most " +
                                                      "games in the Games category. Since the program was " +
                                                      "added to our catalog in 2017, it has obtained 1,917 " +
                                                      "downloads.\r\n\r\nThe current version of the software " +
                                                      "is 1.4 and its last update in our catalog happened on " +
                                                      "03/02/2015. It\'s available for users with the operating " +
                                                      "system Android 2.3 and more recent versions, and it is " +
                                                      "available in English. ",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 8,
                                        Description = "Sonic 4 Episode II - довольно легкая игра" +
                                        "это не требует столько хранения, сколько большинство" +
                                        "игры в категории« Игры ». Поскольку программа была" +
                                        ", добавленный в наш каталог в 2017 году, он получил 1,917" +
                                        "Загрузка. \r \n \r \n Текущая версия программного обеспечения" +
                                        "1,4, и последнее обновление в нашем каталоге произошло на" +
                                        "03/02/2015. Он доступен для пользователей с действующим" +
                                        "система Android 2.3 и более поздние версии, и это" +
                                        "доступно на английском языке",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 15,
                UnitInStock = 25,
                IsDiscontinued = false,
                PublisherId = 4,
                Rating = 3.5,
                RatingMarks = "3, 1, 4, 5, 3, 4, 3",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2018, 2, 4, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 8)
                                },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 2),
                                    platformList.First(id => id.Id == 1)
                                }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 9,
                Key = "Assassins",
                Name = "Assassin's Creed",
                ImageId = 9,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 9,
                                        Description = "Assassin\'s Creed is one of the more " +
                                                      "successful action and adventure franchises in " +
                                                      "recent years. This first installment offers you " +
                                                      "a totally absorbing story with a huge map to " +
                                                      "explore.\r\n\r\nAssassin\'s Creed sees you as Altaïr, a " +
                                                      "member of a secret society who murders Templars in cold " +
                                                      "blood to prevent them from ruling over the world. Your " +
                                                      "ability to run and jump, as well as to hide and go " +
                                                      "stealthily when necessary will prove crucial in the " +
                                                      "large variety of missions you’ll be assigned in Assassin’s " +
                                                      "Creed.",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 9,
                                        Description = "Assassin's Creed - один из самых" +
                                        "успешные действия и приключения франшизы в" +
                                        "последние годы. Этот первый взнос предлагает вам" +
                                        "полностью поглощающая история с огромной картой" +
                                        "исследуйте. \r \n \r \n Критика Ассассина видит вас как Altaïr," +
                                        "член тайного общества, которое убивает тамплиеров в холоде" +
                                        "кровь, чтобы помешать им править по всему миру." +
                                        "способность бегать и прыгать, а также скрываться и уходить" +
                                        "украдкой, когда это необходимо, окажется решающим в" +
                                        "Большое разнообразие миссий, которые вы будете назначать в« Убийце " +
                                        "Символ веры",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 120,
                UnitInStock = 55,
                IsDiscontinued = false,
                PublisherId = 1,
                Rating = 4.4,
                RatingMarks = "1, 3, 1, 1, 3, 3, 5",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2017, 2, 12, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 4),
                                    genreList.First(id => id.Id == 11)
                                },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 2),
                                    platformList.First(id => id.Id == 1)
                                }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 10,
                Key = "FIFA18",
                Name = "FIFA 18",
                ImageId = 10,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 10,
                                        Description = "FIFA 18 is the most complete version of the soccer " +
                                                      "sim to date, and is a strong rival to PES 2013 in " +
                                                      "the battle to be king of the soccer games. There have " +
                                                      "been several improvements over FIFA 12, which help to " +
                                                      "make this version even better.",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 10,
                                        Description = "FIFA 18 - самая полная версия футбола" +
                                        "на сегодняшний день, и является сильным соперником PES 2013 в" +
                                        "битва, чтобы быть королем футбольных игр. Там есть" +
                                        "было несколько улучшений по сравнению с FIFA 12, которые помогают" +
                                        "сделайте эту версию еще лучше",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 40,
                UnitInStock = 55,
                IsDiscontinued = false,
                PublisherId = 2,
                Rating = 3,
                RatingMarks = "3, 3, 1, 1, 2, 4, 1",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2018, 2, 2, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 5)
                                },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 4),
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 2),
                                    platformList.First(id => id.Id == 1)
                                }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 11,
                Key = "CMR",
                Name = "City Moto Racer",
                ImageId = 11,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 11,
                                        Description = "Rush through the stream of constantly moving cars " +
                                              "and enjoy the great adrenaline boost in the adventurous " +
                                              "game City Moto Racer! Are you a true daredevil? Let’s " +
                                              "check it! Now you have a perfect chance to accept a " +
                                              "challenge and show everybody that you are a true moto " +
                                              "racing star! Sometimes we have a strong desire to test " +
                                              "our skills and understand if we ready for undertaking " +
                                              "situations or not. Let’s do it right now! At the very " +
                                              "beginning you have only 1 motorbike at your disposal – " +
                                              "“Hunter”, but later on you can buy more powerful bikes:" +
                                              " Eagle, Racer, Racer-X and even F1-Bike. Can you imagine " +
                                              "that you are rushing at full speed on the busy streets of " +
                                              "the megapolis? But in the racing game City Moto Racer " +
                                              "everything is possible – just choose the bike and maneuver " +
                                              "among the cars! Collect golden coins to upgrade your bike " +
                                              "and gather bonuses to make the game easier! The magnet " +
                                              "power-up will help you get all the coins on your way even " +
                                              "if they will be far from you. And the nitro power-up boosts " +
                                              "your speed and you’ll feel like a true racer! Enjoy the real " +
                                              "power of the bike while using nitro acceleration! The better " +
                                              "its engine power is - the higher is the speed. Moreover, when " +
                                              "you ride close to the car and don’t ram into it, you’ll get extra " +
                                              "coins. But don’t abuse your luck – sometimes fortune leaves even " +
                                              "the fortune favorites! In the game City Moto Racer there are 2 ways " +
                                              "of playing – if you want to control everything on the road, keep the " +
                                              "camera above it. But if it’s better for you to observe everything as " +
                                              "a moto racer, change the viewing angle and you’ll see what is happening " +
                                              "on the road with your own eyes. Drive fast and become a champion! ",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 11,
                                        Description = "Раздайте поток постоянно движущихся автомобилей" +
                "и наслаждайтесь мощным усилением адреналина в авантюрном" +
                "игра City Moto Racer! Вы настоящий смельчак? Давайте" +
                "проверьте это! Теперь у вас есть прекрасный шанс принять" +
                "вызов и показать всем, что вы настоящий мото" +
                "гоночная звезда! Иногда у нас есть сильное желание испытать" +
                "наши навыки и понять, готовы ли мы к проведению" +
                "ситуации или нет. Давайте сделаем это прямо сейчас! На самом" +
                "у вас в вашем распоряжении всего 1 мотоцикл" +
                "Охотник», но позже вы можете купить более мощные велосипеды: " +
                "Eagle, Racer, Racer-X и даже F1-Bike. Можете ли вы представить себе" +
                "что вы спешите на полной скорости на оживленных улицах" +
                "мегаполис? Но в гоночной игре City Moto Racer" +
                "все возможно - просто выберите велосипед и маневр" +
                "среди автомобилей! Собирайте золотые монеты, чтобы обновить свой велосипед" +
                "и собирать бонусы, чтобы сделать игру проще! Магнит" +
                "Power-Up поможет вам получить все монеты на вашем пути даже" +
                "если они будут далеки от вас, и нитроустойчивость повысит" +
                "Ваша скорость, и вы почувствуете себя настоящим гонщиком! Наслаждайтесь настоящим" +
                "сила велосипеда при использовании нитроускорения! Чем лучше" +
                "его мощность двигателя - чем выше скорость. Более того, когда" +
                "вы едете близко к машине и не садитесь в нее, вы получите дополнительные" +
                                              "монеты, но не злоупотребляйте своей удачей - иногда удача оставляет даже" +
                                              "удачи фаворитов! В игре City Moto Racer есть 2 способа" +
                                              "игры - если вы хотите контролировать все на дороге, сохраните" +
                                              "камера над ним, но если вам лучше наблюдать все как" +
                                              "мото-гонщик, измените угол обзора, и вы увидите, что происходит" +
                                              "По дороге своими глазами. Двигайся быстро и становишься чемпионом!",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 10,
                UnitInStock = 10,
                IsDiscontinued = false,
                PublisherId = 4,
                Rating = 2.3,
                RatingMarks = "3, 3, 4, 1, 2, 4, 5",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2010, 2, 2, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 6),
                                    genreList.First(id => id.Id == 8)
                                },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 4),
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 2),
                                    platformList.First(id => id.Id == 1)
                                }
            };
            gameList.Add(g);

            g = new Game
            {
                Id = 12,
                Key = "FN",
                Name = "Fortnite",
                ImageId = 12,
                GameTranslates = new List<GameTranslate>
                                {
                                    new GameTranslate
                                    {
                                        GameId = 12,
                                        Description = "Fortnite is an online video game created in 2017, " +
                                                      "developed by Epic Games, and released as different software " +
                                                      "packages having different game modes that otherwise share " +
                                                      "the same general gameplay and game engine. The game modes " +
                                                      "include Fortnite: Save the World, a cooperative shooter-survival" +
                                                      " game for up to four players to fight off zombie-like husks and " +
                                                      "defend objects with fortifications they can build, and Fortnite " +
                                                      "Battle Royale, a free-to-play battle royale game where up to 100 " +
                                                      "players fight in increasingly-smaller spaces to be the last person " +
                                                      "standing. Both game modes were released in 2017 as early access " +
                                                      "titles; Save the World is available only for Microsoft Windows, " +
                                                      "macOS, PlayStation 4, and Xbox One, while Battle Royale has been " +
                                                      "released for those platforms, Nintendo Switch, and iOS and Android devices.",
                                        LanguageId = 1,
                                        Language = context.Languages.First(tr => tr.Id == 1)
                                    },
                                    new GameTranslate
                                    {
                                        GameId = 12,
                                        Description = "Fortnite - это онлайн-видеоигра, созданная в 2017 году" +
                                        "разработанный Epic Games и выпущенный как различное программное обеспечение" +
                                        "пакеты, имеющие разные игровые режимы, которые в противном случае разделяют" +
                                        "тот же общий игровой процесс и игровой движок. Игровые режимы" +
                                        "включить Fortnite: Save the World, совместный шутер-выживание" +
                                        "игра для четырех игроков, чтобы сражаться с зомби-подобной шелухой и" +
                                        "защищать объекты с укреплениями, которые они могут построить, и Fortnite" +
                                        "Battle Royale, игра для битвы в свободной игре, где до 100" +
                                        "игроки сражаются во все меньших пространствах, чтобы быть последним" +
                                        "Оба режима игры были выпущены в 2017 году как ранний доступ" +
                                        "Заголовки, Сохранить мир »доступны только для Microsoft Windows, " +
                                        "macOS, PlayStation 4 и Xbox One, в то время как Battle Royale был" +
                                        "выпущенный для этих платформ, Nintendo Switch, iOS и Android-устройств .",
                                        LanguageId = 2,
                                        Language = context.Languages.First(tr => tr.Id == 2)
                                    }
                                },
                Price = 60,
                UnitInStock = 10,
                IsDiscontinued = false,
                PublisherId = 2,
                Rating = 4.3,
                RatingMarks = "1, 3, 4, 1, 3, 4, 3 ",
                PopularityCounter = 0,
                UploadDate = DateTime.UtcNow,
                PublicationDate = new DateTime(2017, 2, 2, 10, 15, 30),
                Comments = new List<Comment>(),
                Genres = new List<Genre>
                                {
                                    genreList.First(id => id.Id == 11)
                                },
                Platforms = new List<Platform>
                                {
                                    platformList.First(id => id.Id == 4),
                                    platformList.First(id => id.Id == 3),
                                    platformList.First(id => id.Id == 2),
                                    platformList.First(id => id.Id == 1)
                                }
            };
            gameList.Add(g);

            var markList = new List<double>();
            foreach (var game in gameList)
            {
                var marks = game.RatingMarks.Split(',');
                foreach (var mark in marks)
                {
                    if (double.TryParse(mark, out var temp))
                    {
                        markList.Add(temp);
                    }
                }

                game.Rating = GetMedian(markList);
            }

            gameList.ForEach(s => context.Games.Add(s));
            context.SaveChanges();

            var rolesList = new List<Role>
                            {
                                new Role
                                {
                                    Id = 1,
                                    Name = "Administrator",
                                    IsDeleted = false
                                },
                                new Role
                                {
                                    Id = 2,
                                    Name = "Manager",
                                    IsDeleted = false
                                },
                                new Role
                                {
                                    Id = 3,
                                    Name = "Moderator",
                                    IsDeleted = false
                                },
                                new Role
                                {
                                    Id = 4,
                                    Name = "User",
                                    IsDeleted = false
                                },
                                new Role
                                {
                                    Id = 4,
                                    Name = "Publisher",
                                    IsDeleted = false
                                }
                            };

            rolesList.ForEach(s => context.Roles.Add(s));
            context.SaveChanges();

            var userList = new List<User>
                            {
                                new User
                                {
                                    Id = 1,
                                    Name = "Ivan",
                                    Surname = "Ivanov",
                                    Email = "moderator",
                                    Password = "moderator",
                                    IsPersistent = false,
                                    Roles = new List<Role>
                                    {
                                        rolesList.First(role => role.Name == "Moderator")
                                    },
                                    Rates = new List<Rate>(),
                                    BannedUntil = DateTime.UtcNow
                                },
                                new User
                                {
                                    Id = 2,
                                    Name = "Pavel",
                                    Surname = "Piletskyi",
                                    Email = "admin",
                                    Password = "admin",
                                    IsPersistent = false,
                                    Roles = new List<Role>
                                    {
                                        rolesList.First(role => role.Name == "Administrator")
                                    },
                                    Rates = new List<Rate>(),
                                    BannedUntil = DateTime.UtcNow
                        },
                                new User
                                {
                                    Id = 2,
                                    Name = "Pavel",
                                    Surname = "Piletskyi",
                                    Email = "user",
                                    Password = "user",
                                    IsPersistent = false,
                                    Roles = new List<Role>
                                    {
                                        rolesList.First(role => role.Name == "User")
                                    },
                                    Rates = new List<Rate>(),
                                    BannedUntil = DateTime.UtcNow
                                },

                                new User
                                {
                                    Id = 3,
                                    Name = "Alex",
                                    Surname = "Piletskyi",
                                    Email = "manager",
                                    Password = "manager",
                                    IsPersistent = false,
                                    Roles = new List<Role>
                                    {
                                        rolesList.First(role => role.Name == "Manager")
                                    },
                                    Rates = new List<Rate>(),
                                    BannedUntil = DateTime.UtcNow
                        }
                            };

            userList.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }

        public double GetMedian(IEnumerable<double> marks)
        {
            double[] sortedPNumbers = (double[])marks.ToArray().Clone();
            Array.Sort(sortedPNumbers);

            int size = sortedPNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? (double)sortedPNumbers[mid] : ((double)sortedPNumbers[mid] + (double)sortedPNumbers[mid - 1]) / 2;

            return median;
        }
    }
}