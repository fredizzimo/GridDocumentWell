using Microsoft.VisualStudio.PlatformUI.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridDocumentWell
{
    public enum Direction
    {
        Horizontal,
        Vertical,
    }

    public class VSGrid
    { 
        public Direction Direction { get; set; }

        public int NumColumns
        {
            get
            {
                return Direction == Direction.Horizontal ? Items.Count : 1;
            }
        }
        public int NumRows
        {
            get
            {
                return Direction == Direction.Vertical ? Items.Count : 1;
            }
        }

        public VSGrid()
        {
            Items = new List<object>();
        }

        public List<object> Items { get; }


    }
}
