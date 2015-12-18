﻿using System;

using UnityEngine;

using System.Collections;
using System.Collections.Generic;


namespace ninja.marching.flatstates{
	public class Predicate {

		public readonly string Name;
		public readonly Term[] Terms;
		public readonly Type[] Types;	

		public Predicate(string name, Type[] types, params object[] terms)
		{
			Name = name;

			Types = types;
			Terms = new Term[terms.Length];

			for (int i=0; i<terms.Length; i++) {
				object termValue = terms[i];

				Type ttt = typeof(Term<>).MakeGenericType (Types [i]);

				Terms [i] = (Term)Activator.CreateInstance ( typeof(Term<>).MakeGenericType( Types[i] ), termValue);
			}
		}

		public static Predicate Substitute( Predicate predicate, Substitution substitution )
		{ 
			List<object> args = new List<object> ();
			for (int termIndex = 0; termIndex < predicate.Terms.Length; termIndex++) {

				if (substitution.original == predicate.Terms [termIndex]) {
					args.Add ( substitution.substituted.ValueObject );
				} else {
					args.Add ( predicate.Terms [termIndex].ValueObject );
				}

			}

			return (Predicate)Activator.CreateInstance(predicate.GetType (), args.ToArray ());
		}

		public static List<Substitution> Unify( Predicate p1, Predicate p2 )
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

		public override string ToString ()
		{
			string str = "";		
			Array.ForEach<Term> (Terms, delegate(Term obj) {
				str += obj + ", ";
			});

			return string.Format ("{0} -> {1}", Name, str );
		}

	}

	public class PredicateEqualityComparer : IEqualityComparer<Predicate>
	{
		public bool Equals (Predicate x, Predicate y)
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

		public int GetHashCode(Predicate x)
		{
			var hash = x.Name.GetHashCode ();
			for (int i = 0; i < x.Terms.Length; i++) {
				hash += x.Terms [i].GetHashCode();
			}
			return hash;
		}

	}

	public struct Substitution
	{
		public Term original;
		public Term substituted;

		public static bool operator ==(Substitution x, Substitution y)
		{
			return x.original == y.original && x.substituted == y.substituted;
		}

		public static bool operator !=(Substitution x, Substitution y)
		{
			return !(x == y);
		}

		public override string ToString ()
		{
			return string.Format ("{0}/{1}", original.ToString(), substituted.ToString());
		}
	}
}