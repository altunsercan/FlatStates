using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using System;

using System.Collections.Generic;
using Castle.Core.Internal;
using FlatStates.Scripts;
using ninja.marching.flatstates;

public class MonstersTest
{
    [Test]
	public void FindWhoLetTheDogsOut()
    {
        //Arrange
		InMemoryState state = new InMemoryState ();

        string[] monster = new[] {"Cursed Crocodile", "Goblin Berserker", "Ancient Golem", "Cursed Elf"};
        string[] weapon = new[] {"Cursed Sword", "Old Mans Socks", "Hammer of Doom", "Cursed Spear of Light"};
        
        monster.ForEach( _ => state.Add(new IsMonster(_)));
        monster.ForEach( _ => { if(_.StartsWith("Cursed")) state.Add(new IsCursed(_)); });

        weapon.ForEach(_ => state.Add(new IsWeapon(_)));
        weapon.ForEach(_ => { if (_.StartsWith("Cursed")) state.Add(new IsCursed(_)); });

        List<Axiom> queryPredicates = new List<Axiom>();
        queryPredicates.Add(new IsMonster("?CursedMonster"));
        queryPredicates.Add(new IsCursed("?CursedMonster"));

        List<List<Substitution>> subset;
        if (Unification.Query (state, queryPredicates, out subset)) {
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
	}

	
}
