using System;

namespace ninja.marching.flatstates
{

	public class Binding:IBindable<Binding>
	{
		private string Identifier;

		protected Type ValueType;
		public Type GetValueType()
		{
			return ValueType;
		}

		public Binding (Type valueType, string identifier)
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

	public class Binding<T>:Binding,IBindable<T>
	{
		
		public Binding (string identifier):base( typeof(T), identifier)
		{
		}
			
	}
}

