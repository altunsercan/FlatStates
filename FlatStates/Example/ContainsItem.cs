using System;

namespace ninja.marching.flatstates
{
	public class ContainsItem:Predicate
	{
		public readonly IBindable<Container> Container;
		public readonly IBindable<Item> Item;
		public ContainsItem (IBindable<Container> container, IBindable<Item> item):
		base("ContainsItem", new Type[]{typeof(Container),typeof(Item)}, container, item)
		{
			Container = container;
			Item = item;
		}
	}
}

