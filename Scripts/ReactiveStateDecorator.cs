using System;
using System.Collections.Generic;
using UniRx;

namespace ninja.marching.flatstates
{
    public class ReactiveStateDecorator:State
    {
        private State _decorated;

        public readonly IObserver<PredicateChangeEvent> ChangeListener;

        public ReactiveStateDecorator(State decorated)
        {
            _decorated = decorated;
            ChangeListener = Observer.Create<PredicateChangeEvent>(OnTermChange, OnTermError, OnTermDispatcherComplete);
        }

        #region Term Change Listener

        public void OnTermChange(PredicateChangeEvent evt)
        {
            if (evt.added)
            {
                _decorated.Add(evt.term);
            }
            else {
                _decorated.Remove(evt.term);
            }
        }
        public void OnTermError(Exception e)
        {

        }
        public void OnTermDispatcherComplete()
        {

        }

        #endregion

        public void Add(Axiom axiom)
        {
            _decorated.Add(axiom);
        }

        public bool Remove(Axiom axiom)
        {
            return _decorated.Remove(axiom);
        }

        public bool Has(Axiom axiom)
        {
            return _decorated.Has(axiom);
        }

        public IEnumerator<Axiom> AllAxiomsByName(string axiomName)
        {
            return _decorated.AllAxiomsByName(axiomName);
        }
    }
}