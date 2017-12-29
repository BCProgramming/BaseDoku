using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASeDoku
{
    public interface ISodokuBoardHandler
    {
        void CellEvent(SodokuCellEvent eventarg);
        SodokuCell GetCellAtPosition(int pX, int pY);
    }

    public abstract class SodokuCellEvent
    {
        public SodokuCell Sender { get; private set; }
        protected SodokuCellEvent(SodokuCell pSender)
        {
            Sender = pSender;
        }
    }

    public class SodokuCellEvent_Changed : SodokuCellEvent
    {
        public int Value { get; private set; }
        public SodokuCellEvent_Changed(SodokuCell pSender,int pValue):base(pSender)
        {
            
        }

    }
    public class SodokuCellEvent_Changing : SodokuCellEvent
    {
        public bool Cancelled { get; set; } 
        public int OldValue { get; private set; }
        public int NewValue { get; private set; }
        public SodokuCellEvent_Changing(SodokuCell pSender, int pOldValue, int pNewValue):base(pSender)
        {
            OldValue = pOldValue;
            NewValue = pNewValue;
        }


    }
}
