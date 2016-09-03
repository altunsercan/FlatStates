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

		public override string ToString ()
		{
			return string.Format("Bind([{0}]{1})", ValueType.Name, Identifier );
		}

		#region IIdentifiable implementation

		public string UniqueID {
			get {
				return Identifier; 
			}
		}

		#endregion

	}

	public class Variable<T>: Variable, Bindable<T>
	{
		
		public Variable (string identifier):base( typeof(T), identifier)
		{
		}
        
    }
}

