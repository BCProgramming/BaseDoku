using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASeDoku
{
    public class MiniGrid
    {
        private Dictionary<Tuple<int, int>, SudokuCell> MiniGridData = new Dictionary<Tuple<int, int>, SudokuCell>(); 

        public int GridX { get; set; }
        public int GridY { get; set; }
        
        public int[] RowNumbers { get; set; }
        public int [] ColumnNumbers { get; set; }
        //A standard board has 9 "minigrids", arranged in a standard grid pattern. Each 3x3 square is a "Minigrid".
        public MiniGrid(ISudokuBoardHandler pHandler,int pMiniGridX,int pMiniGridY)
        {
            if(pMiniGridX <1 || pMiniGridX > 3) throw new ArgumentException("pMiniGridX");
            if (pMiniGridY < 1 || pMiniGridY > 3) throw new ArgumentException("pMiniGridY");
            GridX = pMiniGridX;
            GridY = pMiniGridY;
            for(int x=1;x<=3;x++)
            {
                for(int y=1;y<=3;y++)
                {
                    Tuple<int, int> NewTuple = new Tuple<int, int>(x, y);
                    int UseX = ((pMiniGridX - 1) * 3) + x;
                    int UseY = ((pMiniGridY - 1) * 3) + y;
                    SudokuCell GrabCell = pHandler.GetCellAtPosition(UseX, UseY);
                    MiniGridData.Add(NewTuple,GrabCell);
                }
            }
            ColumnNumbers = new int[] { ((pMiniGridX - 1) * 3) + 1, ((pMiniGridX - 1) * 3) + 2, ((pMiniGridX - 1) * 3) + 3 };
            RowNumbers = new int[] { ((pMiniGridY - 1) * 3) + 1, ((pMiniGridY - 1) * 3) + 2, ((pMiniGridY - 1) * 3) + 3 };
        }

        public SudokuCell GetCellAtPosition(int pX, int pY)
        {
            if(pX < 1 || pX > 3) throw new ArgumentException("pX");
            if (pY < 1 || pY > 3) throw new ArgumentException("pY");
            
            Tuple<int, int> FindKey = new Tuple<int, int>(pX, pY);
            if (MiniGridData.ContainsKey(FindKey)) return MiniGridData[FindKey];
            return null;
        }
        public IEnumerable<SudokuCell> AllCells()
        {
            return MiniGridData.Values;
        }


    }
}
