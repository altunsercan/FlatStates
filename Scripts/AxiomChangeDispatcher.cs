using System;
using UniRx;

using System.Collections.Generic;

namespace ninja.marching.flatstates
{
	public class AxiomChangeDispatcher:IObservable<PredicateChangeEvent>
	{
		private List<IObserver<PredicateChangeEvent>> observers;

		public AxiomChangeDispatcher (out Action<Axiom,bool> PublishChange)
		{
			observers = new List<IObserver<PredicateChangeEvent>> ();

			PublishChange = Publish;
		}


		private void Publish(Axiom term, bool add)
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
		public Axiom term;
		public bool added;
	}
}

