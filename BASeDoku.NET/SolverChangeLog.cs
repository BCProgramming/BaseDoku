using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASeDoku
{
    public class SolverLog
    {
    }
    
    public class SolverLogMessage
    {
        public String SolverMessage { get; set; }
        public SolverLogMessage(String pMessage)
        {
            SolverMessage = pMessage;
        }
    }
    public class SolverLogCellChanged: SolverLogMessage
    {
        public SudokuCell ChangedCell { get; set; }
        
        public SolverLogCellChanged(SudokuCell pChangedCell,String pSolverMessage):base(pSolverMessage)
        {
            ChangedCell = pChangedCell;
        }
    }
}
