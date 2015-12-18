using System;
using UniRx;

using System.Collections.Generic;

namespace ninja.marching.flatstates
{
	public class PredicateChangeDispatcher:IObservable<PredicateChangeEvent>
	{
		private List<IObserver<PredicateChangeEvent>> observers;

		public PredicateChangeDispatcher (out Action<Predicate,bool> PublishChange)
		{
			observers = new List<IObserver<PredicateChangeEvent>> ();

			PublishChange = Publish;
		}


		private void Publish(Predicate term, bool add)
		{
			var enumerator = observers.GetEnumerator ();
		
			PredicateChangeEvent evt = new PredicateChangeEvent ();
			evt.term = term;
			evt.added = add;

			while (enumerator.MoveNext ()) {
				enumerator.Current.OnNext(evt); 
			}
		}

		#region IObservable implementation

		public IDisposable Subscribe (IObserver<PredicateChangeEvent> observer)
		{
			observers.Add (observer);

			Action UnSubscribe = delegate() {
				observers.Remove (observer);
			};

			return Disposable.Create(UnSubscribe);
		}

		#endregion
	}

	public struct PredicateChangeEvent
	{
		public Predicate term;
		public bool added;
	}
}

