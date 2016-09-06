
using System;
using FlatStates.Example.Types;

namespace ninja.marching.flatstates
{
    public class IsCursed : Axiom
    {
        public readonly Term<Cursable> Cursable;
        public IsCursed(Term<Cursable> cursable) : base ("IsCursed", cursable)
		{
            Cursable = cursable;
        }
    }
}
