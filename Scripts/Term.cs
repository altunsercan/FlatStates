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

		public readonly Bindable ValueObject;
		public readonly Type ValueType;

		protected Term(Type valueType, Bindable valueObject)
		{
			if (valueObject is Variable) {
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
				if (other.ValueObject.UniqueID == this.ValueObject.UniqueID) {
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
				if (other.ValueObject.UniqueID == this.ValueObject.UniqueID) {
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
			return "t_"+ValueObject;
		}

		public static bool operator ==(Term x, Term y)
		{
			if( object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)  )
			{
				return object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null) ;
			}

		    if (x.internalStatus == STATUS.UNBOUND && y.internalStatus == STATUS.UNBOUND)
		    {
		        Variable xVar = x.ValueObject as Variable;
                Variable yVar = y.ValueObject as Variable;

                return xVar == yVar;
            }else if (x.internalStatus == STATUS.BOUND && y.internalStatus == STATUS.BOUND)
            {
                return x.ValueObject.UniqueID == y.ValueObject.UniqueID;
            }

            return x.ValueObject == y.ValueObject;
		}

		public static bool operator !=(Term x, Term y)
		{
			return !(x == y);
		}

	}

	public class Term<T>:Term where T:Bindable<T>
    {
		public Term(Bindable<T> value):base(typeof(Bindable<T>), value)
		{
		}

        public static implicit operator Term<T>(String strDef) 
        {
            if (strDef.StartsWith("?"))
            {
                string varName = strDef.Substring(1);
                Variable<T> variable = new Variable<T>(varName);
                
                return new Term<T>(variable);
            }

            BindableIdentityProxy<T> identifier = new BindableIdentityProxy<T>(strDef);

            return new Term<T>(identifier);
        }

        public static implicit operator Term<T>(T bindable)
        {
            return new Term<T>(bindable);
        }

        public static implicit operator Term<T>(Variable<T> variable)
        {
            return new Term<T>(variable);
        }
    }

    public class BindableIdentityProxy<T>:Bindable<T> where T : Identifiable,Bindable<T>
    {
        private string _identifier;
        public BindableIdentityProxy(string identifier)
        {
            _identifier = identifier;
        }

        public string UniqueID
        {
            get { return _identifier; }
        }

        public override string ToString()
        {
            return string.Format("p_Bind([{0}]{1})", typeof(T).Name, _identifier);
        }
    }
}