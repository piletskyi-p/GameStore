using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Dal.Entities.Entities
{
    public class Rate : BaseEntity
    {
        public Game Game { get; set; }
        
        public double Rating { get; set; }

        public string RatingMarks { get; set; }
    }
}
