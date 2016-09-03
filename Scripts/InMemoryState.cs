﻿using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace ninja.marching.flatstates
{
    public class InMemoryState:State
	{
		private HashSet<Axiom> stateList;
		private ILookup<string, Axiom> axiomNameLookup;


		public InMemoryState ()
		{
			stateList = new HashSet<Axiom>(new AxiomEqualityComparer());
		    axiomNameLookup = stateList.ToLookup(a => a.Name);
		}
      
		private struct QuerySearchNode
		{
			List<Substitution> substitutions;
			List<Axiom> terms;
		}


        public void Add(Axiom axiom)
        {
            stateList.Add(axiom);
            axiomNameLookup = stateList.ToLookup(a => a.Name);
        }

        public bool Remove(Axiom axiom)
        {
            bool tmp = stateList.Remove(axiom);
            axiomNameLookup = stateList.ToLookup(a => a.Name);
            return tmp;
        }

        public bool Has(Axiom axiom)
        {
            return stateList.Contains(axiom);
        }

        public IEnumerator<Axiom> AllAxiomsByName(string axiomName)
        {
            return axiomNameLookup[axiomName].GetEnumerator();
        }
	}
}

