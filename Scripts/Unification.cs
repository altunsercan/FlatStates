using System;
using System.Collections.Generic;
using ninja.marching.flatstates;
using UnityEngine;

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
                currentTerm = ApplySubstitution(currentTerm, subToApply);
            }); 

            var stateTerms = state.AllAxiomsByName(currentTerm.Name);
            while (stateTerms.MoveNext())
            {
                List<Substitution> substitution = Unify(stateTerms.Current, currentTerm);

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


        public static Axiom ApplySubstitution( Axiom axiom, Substitution substitution )
        { 
            List<object> args = new List<object> ();
            for (int termIndex = 0; termIndex < axiom.Terms.Length; termIndex++) {

                if (substitution.original == axiom.Terms [termIndex]) {
                    args.Add ( substitution.substituted );
                } else {
                    args.Add ( axiom.Terms [termIndex] );
                }

            }

            return (Axiom)Activator.CreateInstance(axiom.GetType (), args.ToArray ());
        }

        public static List<Substitution> Unify( Axiom p1, Axiom p2 )
        {
            if (p1.Name != p2.Name) {
                return null;
            }

            if (p1.Terms.Length != p2.Terms.Length) {
                return null;
            }

            List<Substitution> substitution = new List<Substitution>();

            for (int termIndex = 0; termIndex < p1.Terms.Length; termIndex++) {

                var sub = p1.Terms [termIndex].Unify (p2.Terms [termIndex]);

                Debug.Log ("σ:" + sub.original + " " + sub.substituted);

                if (sub == default(Substitution)) {
                    return null;
                } else if(sub.substituted != null) {
                    substitution.Add (sub);
                }

            }
            return substitution;
        }
    }
}
