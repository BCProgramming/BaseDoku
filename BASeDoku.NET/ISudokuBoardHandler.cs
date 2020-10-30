using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASeDoku
{
    public interface ISudokuBoardHandler
    {
        void CellEvent(SudokuCellEvent eventarg);
        SudokuCell GetCellAtPosition(int pX, int pY);
    }

    public abstract class SudokuCellEvent
    {
        public SudokuCell Sender { get; private set; }
        protected SudokuCellEvent(SudokuCell pSender)
        {
            Sender = pSender;
        }
    }

    public class SudokuCellEvent_Changed : SudokuCellEvent
    {
        public int Value { get; private set; }
        public SudokuCellEvent_Changed(SudokuCell pSender,int pValue):base(pSender)
        {
            
        }

    }
    public class SudokuCellEvent_Changing : SudokuCellEvent
    {
        public bool Cancelled { get; set; } 
        public int OldValue { get; private set; }
        public int NewValue { get; private set; }
        public SudokuCellEvent_Changing(SudokuCell pSender, int pOldValue, int pNewValue):base(pSender)
        {
            OldValue = pOldValue;
            NewValue = pNewValue;
        }
    }
}
