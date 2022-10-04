﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Model
{
    public enum TileValue 
    { 
        EMPTY, RED, YELLOW 
    }

    internal class Tile
    {
        public TileValue Value { get; set; }

        public Tile()
        {
            Value = TileValue.EMPTY;
        }
        public Tile(TileValue value)
        {
            Value = value;
        }

        public bool IsEmpty () { return Value == TileValue.EMPTY;  }
        public bool IsRed   () { return Value == TileValue.RED;    }
        public bool IsYellow() { return Value == TileValue.YELLOW; }
    }
}

