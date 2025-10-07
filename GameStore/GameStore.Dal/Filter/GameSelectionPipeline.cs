using System.Linq;
using GameStore.Dal.Entities;

namespace GameStore.Dal.Filter
{
    public class GameSelectionPipeline : Pipeline<IQueryable<Game>>
    {
        public override IQueryable<Game> Process(IQueryable<Game> input)
        {
            foreach (var filter in Filters)
            {
                input = filter.Execute(input);
            }

            return input;
        }
    }
}
