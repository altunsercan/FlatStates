using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using System;

using System.Collections.Generic;
using ninja.marching.flatstates;

public class ChestTest{

    [Test]
	public void FindChestsWithItem()
    {
        //Arrange
		StateContainer container = new StateContainer ();

		Container chest = new Container("chest01", "Weapons Chest");
		chest.TermDispatcher.Subscribe (container.ChangeListener );

		Weapon sword = new Weapon ("weapon01","Iron Sword", 10);
		sword.TermDispatcher.Subscribe (container.ChangeListener);
		sword.Broadcast();

		Item shield = new Item ("shield01","Wooden Roundshield", 3);

		chest.AddItem (sword);
		chest.AddItem (shield);

		List<Predicate> queryPredicates = new List<Predicate> ();
		queryPredicates.Add (new ContainsItem (new Binding<Container> ("ChestToFind"), sword));

		List<List<Substitution>> subset;
		if (container.Query (queryPredicates, out subset)) {
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
		StateContainer container = new StateContainer ();

		Container chest = new Container("chest01","Weapons Chest");
		chest.TermDispatcher.Subscribe (container.ChangeListener );

		Weapon sword = new Weapon ("weapon01","Iron Sword", 10);
		sword.TermDispatcher.Subscribe (container.ChangeListener);
		sword.Broadcast();

		Weapon sword2 = new Weapon ("weapon02","Silver Sword", 10);
		sword.TermDispatcher.Subscribe (container.ChangeListener);
		sword.Broadcast();

		Item shield = new Item ("shield01","Wooden Roundshield", 3);

		chest.AddItem (sword);
		chest.AddItem (sword2);
		chest.AddItem (shield);

		List<Predicate> queryPredicates = new List<Predicate> ();
		var itemToFind = new Binding<Item> ("ItemToFind");
		queryPredicates.Add (new ContainsItem (chest, itemToFind));
		queryPredicates.Add (new IsWeapon (itemToFind) );

		List<List<Substitution>> subset;
		if (container.Query (queryPredicates, out subset)) {
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
