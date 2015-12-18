using System;

namespace ninja.marching.flatstates
{
	public class Weapon:Item
	{
		public PredicateChangeDispatcher TermDispatcher;
		private Action<Predicate,bool> Dispatch;


		public Weapon (string identifier, string name, int basePrice):base(identifier,name,basePrice)
		{
			TermDispatcher = new PredicateChangeDispatcher (out Dispatch);
		}

		public void Broadcast()
		{
			Dispatch(new IsWeapon(this), true);
		}
	}
}

