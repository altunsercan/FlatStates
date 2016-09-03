using System;

namespace  ninja.marching.flatstates
{
	public class Item:Bindable<Item>
	{
		public readonly string Identifier;

		public string Name;

		public int BasePrice;

		public Item (string identifier, string name, int basePrice)
		{
			Identifier = identifier;
			Name = name;
			BasePrice = BasePrice;
		}

		public override string ToString ()
		{
			return string.Format ("[Item]'{0}'", Identifier);
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

