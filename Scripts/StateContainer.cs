using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using UniRx;

namespace ninja.marching.flatstates
{
	public class StateContainer
	{
		private HashSet<Predicate> stateList;
		private ILookup<string, Predicate> predicateNameLookup;

		public IObserver<PredicateChangeEvent> ChangeListener;

		public StateContainer ()
		{
			stateList = new HashSet<Predicate>(new PredicateEqualityComparer());
			ChangeListener = Observer.Create<PredicateChangeEvent>(OnTermChange, OnTermError, OnTermDispatcherComplete) ;
		}

		#region Term Change Listener

		public void OnTermChange(PredicateChangeEvent evt)
		{
			if (evt.added) {
				stateList.Add (evt.term);
			} else {
				stateList.Remove (evt.term);
			}
			predicateNameLookup = stateList.ToLookup (p => p.Name);

		}
		public void OnTermError(Exception e)
		{

		}
		public void OnTermDispatcherComplete()
		{

		}

		#endregion

		public bool Query( List<Predicate> terms, out List<List<Substitution>> solutions)
		{
			solutions = new List<List<Substitution>>();

			Search (terms, new List<Substitution> (), solutions);

			return (solutions.Count > 0);
		}

		private void Search( List<Predicate> termsToSearch, List<Substitution> substitutionsAdded, List<List<Substitution>> solutions)
		{
			Predicate currentTerm = termsToSearch[0];

			substitutionsAdded.ForEach (delegate(Substitution subToApply) {
				currentTerm = Predicate.Substitute( currentTerm, subToApply );
			});

			var stateTerms = predicateNameLookup [currentTerm.Name].GetEnumerator();
			while (stateTerms.MoveNext ()) {

				Debug.Log("ST: "+stateTerms.Current );

				List<Substitution> substitution =  Predicate.Unify (stateTerms.Current, currentTerm);

				if( substitution == null )
				{
					//Cannot Unify
					continue;
				}

				List<Predicate> updatedTermsToSearch = new List<Predicate>();
				updatedTermsToSearch.AddRange(termsToSearch);
				updatedTermsToSearch.RemoveAt(0);

				List<Substitution> updatedSubstitutionsAdded = new List<Substitution>();
				updatedSubstitutionsAdded.AddRange(substitutionsAdded);
				updatedSubstitutionsAdded.AddRange(substitution);

				// TODO: Apply substitiutions to the rest of the terms so they won't be unified again

				if (updatedTermsToSearch.Count > 0) {
					Search (updatedTermsToSearch, updatedSubstitutionsAdded, solutions);
				} else {
					solutions.Add (updatedSubstitutionsAdded);
				}

			}

		}

		private struct QuerySearchNode
		{
			List<Substitution> substitutions;
			List<Predicate> terms;
		}


	}
}

