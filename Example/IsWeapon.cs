using System;

namespace ninja.marching.flatstates
{
	public class IsWeapon:Axiom
	{
		public readonly Term<Item> Item;
		public IsWeapon (Term<Item> item) : base ("IsWeapon", item)
		{
			Item = item;
		}
	}
}

