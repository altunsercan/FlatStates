using System.Collections.Generic;

namespace ninja.marching.flatstates
{
    public interface State
    {
        void Add(Axiom axiom);
        bool Remove(Axiom axiom);
        bool Has(Axiom axiom);
        
        IEnumerator<Axiom> AllAxiomsByName(string axiomName);
    }
}