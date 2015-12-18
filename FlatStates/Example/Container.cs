using System;

using System.Collections.Generic;

namespace  ninja.marching.flatstates
{
	public class Container:IBindable<Container>
	{
		public readonly string Identifier;

		public readonly string Name;

		public PredicateChangeDispatcher TermDispatcher;
		private Action<Predicate,bool> Dispatch;

		private List<Item> itemList;

		public Container (string identifier, string name)
		{
			Identifier = identifier;

			Name = name;

			itemList = new List<Item> ();
			TermDispatcher = new PredicateChangeDispatcher (out Dispatch);
		}

		public void AddItem(Item item)
		{
			ContainsItem term = new ContainsItem (this, item);
			Dispatch (term, true);
		}
		public void RemoveItem(Item item)
		{
			ContainsItem term = new ContainsItem (this, item);
			Dispatch (term, false);
		}

		public override string ToString ()
		{
			return string.Format ("[Container]'{0}'", Name);
		}

		#region IIdentifiable implementation
		public string UniqueID {
			get {
				return Identifier;
			}
		}
		#endregion
	}
}

