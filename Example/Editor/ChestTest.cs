using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using System;

using System.Collections.Generic;
using System.Linq;
using FlatStates.Scripts;
using ninja.marching.flatstates;

public class ChestTest
{
    
    [Test]
	public void FindChestsWithItem()
    {
        //Arrange
		InMemoryState state = new InMemoryState ();
        ReactiveStateDecorator container = new ReactiveStateDecorator(state);
        
		Weapon sword = new Weapon ("weapon01","Iron Sword", 10);
		Item shield = new Item ("shield01","Wooden Roundshield", 3);
        
        Container chest = new Container("chest01", "Weapons Chest");
        container.Add( new ContainsItem(chest, sword));
        container.Add( new ContainsItem(chest, shield));

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

		Assert.AreEqual(subset.Count, 1);
		Assert.AreEqual (chest,subset [0] [0].substituted.ValueObject);
	}

	[Test]
	public void FindWeaponInChest()
	{
		//Arrange
		InMemoryState state = new InMemoryState ();
        ReactiveStateDecorator container = new ReactiveStateDecorator(state);

		Weapon sword = new Weapon ("weapon01","Iron Sword", 10);
        container.Add(new IsWeapon(sword));
        Weapon sword2 = new Weapon ("weapon02","Silver Sword", 10);
        container.Add(new IsWeapon(sword2));
        Item shield = new Item ("shield01","Wooden Roundshield", 3);
        
        Container chest = new Container("chest01", "Weapons Chest");
        container.Add(new ContainsItem(chest, sword));
        container.Add(new ContainsItem(chest, sword2));
        container.Add(new ContainsItem(chest, shield));

		List<Axiom> queryPredicates = new List<Axiom> ();
        queryPredicates.Add (new ContainsItem (chest, "?ItemToFind"));
		queryPredicates.Add (new IsWeapon ("?ItemToFind") );

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

		Assert.AreEqual(subset.Count, 2);
		//Assert.AreEqual (chest,subset [0] [0].substituted.ValueObject);
	}
}
