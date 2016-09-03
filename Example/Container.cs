using System;
using System.Collections.Generic;
using UnityEngine;

namespace ninja.marching.flatstates
{
    public class ContainerLogic : MonoBehaviour
    {
        [SerializeField] private string _containerID;
        [SerializeField] private string _name;

        private Container container;

        public readonly AxiomChangeDispatcher TermDispatcher;
        private Action<Axiom, bool> Dispatch;

        public ContainerLogic()
        {
            container = new Container(_containerID, _name);
            TermDispatcher = new AxiomChangeDispatcher(out Dispatch);
        }

        public void Start()
        {
        }

        public void Update()
        {
        }

        public void PutItem(Item item)
        {
            ContainsItem term = new ContainsItem(container, item);
            Dispatch(term, true);
        }

        public void TakeItem(Item item)
        {
            ContainsItem term = new ContainsItem(container, item);
            Dispatch(term, false);
        }
    }

    public class Container : Bindable<Container>
    {
        public readonly string Identifier;

        public readonly string Name;


        private List<Item> itemList;

        public Container(string identifier, string name)
        {
            Identifier = identifier;

            Name = name;

            itemList = new List<Item>();
        }
        
        public override string ToString()
        {
            return string.Format("[Container]'{0}'", Name);
        }

        #region IIdentifiable implementation

        public string UniqueID
        {
            get { return Identifier; }
        }

        #endregion
    }
}