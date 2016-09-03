using System.Collections.Generic;
using ninja.marching.flatstates;

namespace FlatStates.Scripts
{
    public class Unification
    {
        public static bool Query(State state, List<Axiom> terms, out List<List<Substitution>> solutions)
        {
            solutions = new List<List<Substitution>>();

            Search(state, terms, new List<Substitution>(), solutions);

            return (solutions.Count > 0);
        }

        private static void Search(State state, List<Axiom> termsToSearch, List<Substitution> substitutionsAdded, List<List<Substitution>> solutions)
        {
            Axiom currentTerm = termsToSearch[0];

            substitutionsAdded.ForEach(delegate (Substitution subToApply) {
                currentTerm = Axiom.ApplySubstitution(currentTerm, subToApply);
            });

            var stateTerms = state.AllAxiomsByName(currentTerm.Name);
            while (stateTerms.MoveNext())
            {
                List<Substitution> substitution = Axiom.Unify(stateTerms.Current, currentTerm);

                if (substitution == null)
                {
                    //Cannot Unify
                    continue;
                }

                List<Axiom> updatedTermsToSearch = new List<Axiom>();
                updatedTermsToSearch.AddRange(termsToSearch);
                updatedTermsToSearch.RemoveAt(0);

                List<Substitution> updatedSubstitutionsAdded = new List<Substitution>();
                updatedSubstitutionsAdded.AddRange(substitutionsAdded);
                updatedSubstitutionsAdded.AddRange(substitution);

                // TODO: Apply substitiutions to the rest of the terms so they won't be unified again

                if (updatedTermsToSearch.Count > 0)
                {
                    Search(state, updatedTermsToSearch, updatedSubstitutionsAdded, solutions);
                }
                else {
                    solutions.Add(updatedSubstitutionsAdded);
                }

            }

        }


    }
}
