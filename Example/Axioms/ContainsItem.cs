using System;

namespace ninja.marching.flatstates
{
	public class ContainsItem:Axiom
	{
		public readonly Term<Container> Container;
		public readonly Term<Item> Item;
		public ContainsItem (Term<Container> container, Term<Item> item):
		base("ContainsItem", container, item)
		{
			Container = container;
			Item = item;
		}
	}
}

