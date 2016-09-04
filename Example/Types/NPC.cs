using System;

namespace ninja.marching.flatstates
{
	public class NPC: Bindable<NPC>
    {
        public readonly string Identifier;

        public string Name;

        public NPC(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }

        public override string ToString()
        {
            return string.Format("[NPC]'{0}'", Identifier);
        }

        #region IIdentifiable implementation
        public string UniqueID
        {
            get
            {
                return Identifier;
            }
        }
        #endregion
    }
}

