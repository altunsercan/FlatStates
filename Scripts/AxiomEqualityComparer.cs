using System.Collections.Generic;

namespace ninja.marching.flatstates
{
    public class AxiomEqualityComparer : IEqualityComparer<Axiom>
    {
        public bool Equals (Axiom x, Axiom y)
        { 
            if (x.Name != y.Name || x.Terms.Length != y.Terms.Length)
                return false;

            for(int i = 0; i < x.Terms.Length; i++) {
                if (!x.Terms [i].Equals (y.Terms [i])){
                    return false;
                }
            }

            return true; 
        }

        public int GetHashCode(Axiom x)
        {
            var hash = x.Name.GetHashCode ();
            for (int i = 0; i < x.Terms.Length; i++) {
                hash += x.Terms [i].GetHashCode();
            }
            return hash;
        }

    }
}