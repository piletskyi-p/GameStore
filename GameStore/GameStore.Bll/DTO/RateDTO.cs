using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Bll.DTO
{
    public class RateDTO
    {
        public int Id { get; set; }

        public GameDTO Game { get; set; }

        public double Rating { get; set; }

        public string RatingMarks { get; set; }
    }
}
