using System;

namespace ninja.marching.flatstates
{

	public abstract class Variable//:Bindable<Binding>
    {
		private string Identifier;

		protected Type ValueType;
		public Type GetValueType()
		{
			return ValueType;
		}

		public Variable(Type valueType, string identifier)
		{
			Identifier = identifier;
			ValueType = valueType;
		}

	    public abstract Term ToTerm();

        public abstract Variable CreateVariableOfSameType(string identifier);

        public override string ToString ()
		{
			return string.Format("v_Bind([{0}]{1})", ValueType.Name, Identifier );
		}

		#region IIdentifiable implementation

		public string UniqueID {
			get {
				return Identifier; 
			}
		}

        #endregion

        public static bool operator ==(Variable x, Variable y)
        {
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
            {
                return object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null);
            }

            if (x.ValueType != y.ValueType)
            {
                return false;
            }
            
            return x.UniqueID == y.UniqueID;
        }
        public static bool operator !=(Variable x, Variable y)
        {
            return !(x == y);
        }
        

    }

	public class Variable<T>: Variable, Bindable<T> where T:Bindable<T>
	{
		
		public Variable (string identifier):base( typeof(T), identifier)
		{
		}

	    public override Term ToTerm()
	    {
	        return new Term<T>(this);
	    }

	    public override Variable CreateVariableOfSameType(string identifier)
	    {
	        return new Variable<T>(identifier);
	    }


        public static bool operator ==(Variable<T> x, Variable<T> y)
        {
            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
            {
                return object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null);
            }

            return x.UniqueID == y.UniqueID;
        }

        public static bool operator !=(Variable<T> x, Variable<T> y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

