﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BASeCamp.Elementizer;
using Microsoft.Win32;

namespace BASeDoku
{
    public class SodokuCell : IXmlPersistable
    {
        public ISodokuBoardHandler Owner { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public void ResetPosition(int pX,int pY)
        {
            X = pX;
            Y = pY;
        }
        public bool Selected { get; set; }

        public bool Locked { get; set; }

        private int _Value = 0;
        public int Value { get { return _Value; }
            set
            {
                if (value != _Value)
                {
                    SodokuCellEvent_Changing changing = new SodokuCellEvent_Changing(this,_Value,value);
                    changing.Cancelled = false;
                    Owner?.CellEvent(changing);
                    if (changing.Cancelled) return;
                    _Value = value;
                    SodokuCellEvent_Changed changed = new SodokuCellEvent_Changed(this,_Value);
                    Owner?.CellEvent(changed);

                }
            } }
        public void SetHandler(ISodokuBoardHandler pHandler)
        {
            Owner = pHandler;
        }
        public SodokuCell(ISodokuBoardHandler pOwner,int pX,int pY):this(pOwner)
        {
            this.X = pX;
            this.Y = pY;
        }
        public SodokuCell(ISodokuBoardHandler pOwner)
        {
            Owner = pOwner;
        }
        public SodokuCell(XElement Source, Object pPersistenceData)
        {
            X = Source.GetAttributeInt("X", 0);
            Y = Source.GetAttributeInt("Y", 0);
            Value = Source.GetAttributeInt("Value", 0);
        }
        public XElement GetXmlData(string pNodeName, object PersistenceData)
        {
            var Result = new XElement("SodokuCell",new XAttribute("X",this.X),new XAttribute("Y",this.Y),new XAttribute("Value",this.Value));
            return Result;
        }
    }
 
}
