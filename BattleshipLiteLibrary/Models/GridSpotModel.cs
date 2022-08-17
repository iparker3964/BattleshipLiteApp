using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary.Models
{
    /*Libraries help with code reusability*/
    public class GridSpotModel
    {
        public string SpotLetter { get; set; }
        public int SpotNum { get; set; }
        public GridSpotStatus Status { get; set; } = GridSpotStatus.Empty;
        //Old way before ENUM 0 = empty, 1 = ship, 2 = miss, 3 = hit, 4 = sink
        /*Enum limits your selection and force you to only use those entries.*/
    }
}
