
using System;

namespace ninja.marching.flatstates
{
    public class IsCursed : Axiom
    {
        public readonly Term<NPC> NPC;
        public IsCursed(Term<NPC> npc) : base ("IsCursed", npc)
		{
            NPC = npc;
        }
    }
}
