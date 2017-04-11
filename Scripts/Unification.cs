using System;
using System.Collections.Generic;
using ninja.marching.flatstates;
using UnityEngine;

namespace FlatStates.Scripts
{
    using System.Linq;

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

            if (currentTerm == null)
            {
                return;
            }

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
                if (axiom.Terms[termIndex].ValueType == substitution.substituted.ValueType)
                {
                    if (substitution.original == axiom.Terms[termIndex])
                    {
                        args.Add(substitution.substituted);
                    }
                    else {
                        args.Add(axiom.Terms[termIndex]);
                    }
                }
                else if (axiom.Terms[termIndex].ValueType.IsAssignableFrom(substitution.substituted.ValueType))
                {
                    Bindable bindable = substitution.substituted.ValueObject;
                    if( bindable.GetType() == (typeof(BindableIdentityProxy<>).MakeGenericType(substitution.substituted.ValueType)) )
                    {
                        /* 
                        typeof(BindableIdentityProxy<>)
                            .MakeGenericType(axiom.Terms[termIndex].ValueType)
                        */
                        bindable = (Bindable)Activator.CreateInstance(typeof(BindableIdentityProxy<>)
                            .MakeGenericType(axiom.Terms[termIndex].ValueType), substitution.substituted.ValueObject.UniqueID);

                        Term term = (Term) Activator.CreateInstance( typeof(Term<>)
                            .MakeGenericType(axiom.Terms[termIndex].ValueType), bindable );
                        args.Add(term);
                    }
                    else
                    {
                        args.Add(bindable);
                    }
                } else {
                    args.Add(axiom.Terms[termIndex]);
                }

            }

            Type createdAxiomType = axiom.GetType();
            if (createdAxiomType.IsSubclassOf(typeof(Axiom)))
            {
                return (Axiom)Activator.CreateInstance(createdAxiomType, args.ToArray());
            }

            Term[] terms = new Term[args.Count];
            args.CopyTo(terms);
            
            return new Axiom(axiom.Name, terms);
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

                //Debug.Log ("σ:" + sub.original + " " + sub.substituted);

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
