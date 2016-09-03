using System;

namespace ninja.marching.flatstates
{
	public class IsWeapon:Axiom
	{
		public readonly IBindable<Item> Item;
		public IsWeapon (IBindable<Item> item) : base ("IsWeapon", new Type[]{typeof(Item)}, item)
		{
			Item = item;
		}
	}
}

