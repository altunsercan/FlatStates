using System;
using System.ComponentModel;

namespace ninja.marching.flatstates
{
    public interface Identifiable
    {
        string UniqueID { get; }
    }

	public interface Bindable:Identifiable
	{
	}

    public interface Bindable<T> : Bindable
    {
        
    }
}

