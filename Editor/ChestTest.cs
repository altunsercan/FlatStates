using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using System;

using System.Collections.Generic;
using FlatStates.Scripts;
using ninja.marching.flatstates;

public class ChestTest{


    [Test]
	public void FindChestsWithItem()
    {
        //Arrange
		InMemoryState state = new InMemoryState ();
        ReactiveStateDecorator container = new ReactiveStateDecorator(state);

		Container chest = new Container("chest01", "Weapons Chest");
		chest.TermDispatcher.Subscribe (container.ChangeListener );

		Weapon sword = new Weapon ("weapon01","Iron Sword", 10);
		sword.TermDispatcher.Subscribe (container.ChangeListener);
		sword.Broadcast();

		Item shield = new Item ("shield01","Wooden Roundshield", 3);

		chest.AddItem (sword);
		chest.AddItem (shield);

		List<Axiom> queryPredicates = new List<Axiom> ();
		queryPredicates.Add (new ContainsItem ("?ChestToFind", sword));

		List<List<Substitution>> subset;
		if (Unification.Query (container, queryPredicates, out subset)) {
			string str = "";

			for (int index = 0; index < subset.Count; index++) {
				str += string.Format ("Solution {0}: \n", index+1);
				for (int subIndex = 0; subIndex < subset [index].Count; subIndex++) {
					str += string.Format ("{0} \n", subset [index] [subIndex]);
				}
			}

			Debug.Log (str);
		}

		Assert.AreNotEqual(subset.Count, 0);
		Assert.AreEqual (chest,subset [0] [0].substituted.ValueObject);
	}

	[Test]
	public void FindWeaponInChest()
	{
		//Arrange
		InMemoryState state = new InMemoryState ();
        ReactiveStateDecorator container = new ReactiveStateDecorator(state);

        Container chest = new Container("chest01","Weapons Chest");
		chest.TermDispatcher.Subscribe (container.ChangeListener );

		Weapon sword = new Weapon ("weapon01","Iron Sword", 10);
		sword.TermDispatcher.Subscribe (container.ChangeListener);
		sword.Broadcast();

		Weapon sword2 = new Weapon ("weapon02","Silver Sword", 10);
		sword2.TermDispatcher.Subscribe (container.ChangeListener);
		sword2.Broadcast();

		Item shield = new Item ("shield01","Wooden Roundshield", 3);

		chest.AddItem (sword);
		chest.AddItem (sword2);
		chest.AddItem (shield);

		List<Axiom> queryPredicates = new List<Axiom> ();
		var itemToFind = new Variable<Item> ("ItemToFind");
		queryPredicates.Add (new ContainsItem (chest, itemToFind));
		queryPredicates.Add (new IsWeapon (itemToFind) );

		List<List<Substitution>> subset;
		if (Unification.Query (container, queryPredicates, out subset)) {
			string str = "";

			for (int index = 0; index < subset.Count; index++) {
				str += string.Format ("Solution {0}: \n", index+1);
				for (int subIndex = 0; subIndex < subset [index].Count; subIndex++) {
					str += string.Format ("{0} \n", subset [index] [subIndex]);
				}
			}

			Debug.Log (str);
		}

		Assert.AreNotEqual(subset.Count, 0);
		//Assert.AreEqual (chest,subset [0] [0].substituted.ValueObject);
	}
}
