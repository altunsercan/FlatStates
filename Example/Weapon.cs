using System;

namespace ninja.marching.flatstates
{
	public class Weapon:Item
	{
		public AxiomChangeDispatcher TermDispatcher;
		private Action<Axiom,bool> Dispatch;


		public Weapon (string identifier, string name, int basePrice):base(identifier,name,basePrice)
		{
			TermDispatcher = new AxiomChangeDispatcher (out Dispatch);
		}

		public void Broadcast()
		{
			Dispatch(new IsWeapon(this), true);
		}
	}
}

