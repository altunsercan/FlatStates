
using ninja.marching.flatstates;

namespace FlatStates.Example.Axioms
{
    public class InInventory : Axiom
    {
        public readonly Term<NPC> NPC;
        public readonly Term<Item> Item;
        public InInventory(Term<NPC> npc, Term<Item> item) : base("InInventory", npc, item)
        {
            NPC = npc;
            Item = item;
        }
    }
}
