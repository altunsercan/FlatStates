using System;

using UnityEngine;

namespace ninja.marching.flatstates
{
	public abstract class Term
	{
		public enum STATUS
		{
			UNBOUND,
			BOUND
		}

		public STATUS Status{ get{ return internalStatus; }}
		protected STATUS internalStatus;

		public readonly object ValueObject;
		public readonly Type ValueType;

		protected Term(Type valueType, object valueObject)
		{
			if (valueObject is Binding) {
				internalStatus = STATUS.UNBOUND;
			}else{
				internalStatus = STATUS.BOUND;
			}

			ValueType = valueType;
			ValueObject = valueObject;
		}

		public Substitution Unify(Term other)
		{
			//Debug.Log ("Unifying Terms: " + this + " " + other);

			switch (internalStatus) {
			case STATUS.BOUND:
				return UnifyAsBound (other);
				break;
			case STATUS.UNBOUND:
				return UnifyAsUnbound (other);
				break;
			}
			return new Substitution();
		}

		private Substitution UnifyAsBound(Term other)
		{
			Substitution sub;
			switch (other.Status) {
			case STATUS.BOUND:
				if (other.ValueObject == this.ValueObject) {
					sub = new Substitution ();
					sub.original = this;
					sub.substituted = null;
					return sub;
				}
				break;
			case STATUS.UNBOUND:
				sub = new Substitution ();
				sub.original = other;
				sub.substituted = this;
				return sub;
				break;
			}
			return new Substitution();
		}
		private Substitution UnifyAsUnbound(Term other)
		{
			Substitution sub;
			switch (other.Status) {
			case STATUS.BOUND:
				sub = new Substitution ();
				sub.original = this;
				sub.substituted = other;
				return sub;
				break;

			case STATUS.UNBOUND:
				if (other.ValueObject == this.ValueObject) {
					sub = new Substitution ();
					sub.original = this;
					sub.substituted = null;
					return sub;
				} else {
					sub = new Substitution ();
					sub.original = this;
					sub.substituted = other;
					return sub;
				}
				break;
			}
			return new Substitution();
		}

		public override string ToString ()
		{
			return ""+ValueObject;
		}

		public static bool operator ==(Term x, Term y)
		{
			if( object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)  )
			{
				return object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null) ;
			}
			 
			return x.ValueObject == y.ValueObject;
		}

		public static bool operator !=(Term x, Term y)
		{
			return !(x == y);
		}

	}

	public class Term<T>:Term
	{
		
		public T Value;
		public Binding<T> Binding;

		public Term(IBindable<T> value):base(typeof(IBindable<T>), value)
		{
		}

	}
}