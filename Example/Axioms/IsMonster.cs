
using System;

namespace ninja.marching.flatstates
{
    public class IsMonster : Axiom
    {
        public readonly Term<NPC> NPC;
        public IsMonster(Term<NPC> npc) : base ("IsAnimal", npc)
		{
            NPC = npc;
        }
    }
}
