using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using System;

using System.Collections.Generic;
using FlatStates.Example.Axioms;
using FlatStates.Scripts;
using ninja.marching.flatstates;

public class MonstersTest
{
    [Test]
	public void FindCursedMonsters()
    {
        //Arrange
		InMemoryState state = new InMemoryState ();

        string[] monster = new[] {"Cursed Crocodile", "Goblin Berserker", "Ancient Golem", "Cursed Elf"};
        string[] weapon = new[] {"Cursed Sword", "Old Mans Socks", "Hammer of Doom", "Cursed Spear of Light"};
        
        Array.ForEach(monster, _ => state.Add(new IsMonster(_)));
        Array.ForEach(monster, _ => { if(_.StartsWith("Cursed")) state.Add(new IsCursed(_)); });

        Array.ForEach(weapon, _ => state.Add(new IsWeapon(_)));
        Array.ForEach(weapon, _ => { if (_.StartsWith("Cursed")) state.Add(new IsCursed(_)); });

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
        
        Assert.AreEqual(subset.Count, 2);
    }
    [Test]
    public void FindMonstersHoldingCursedItems()
    {
        //Arrange
        InMemoryState state = new InMemoryState();

        string[] monster = new[] { "Cursed Crocodile", "Goblin Berserker", "Ancient Golem", "Cursed Elf" };
        string[] weapon = new[] { "Cursed Sword", "Old Mans Socks", "Hammer of Doom", "Cursed Spear of Light" };

        Array.ForEach(monster, _ => state.Add(new IsMonster(_)));
        Array.ForEach(monster, _ => { if (_.StartsWith("Cursed")) state.Add(new IsCursed(_)); });

        Array.ForEach(weapon, _ => state.Add(new IsWeapon(_)));
        Array.ForEach(weapon, _ => { if (_.StartsWith("Cursed")) state.Add(new IsCursed(_)); });

        state.Add(new InInventory("Goblin Berserker", "Cursed Sword"));
        state.Add(new InInventory("Cursed Corcodile", "Old Mans Socks"));
        state.Add(new InInventory("Cursed Elf", "Cursed Spear of Light"));

        List<Axiom> queryPredicates = new List<Axiom>();
        queryPredicates.Add(new IsMonster("?Monster"));
        queryPredicates.Add(new IsWeapon("?CursedItem"));
        queryPredicates.Add(new IsCursed("?CursedItem"));
        queryPredicates.Add(new InInventory("?Monster", "?CursedItem"));

        List<List<Substitution>> subset;
        if (Unification.Query(state, queryPredicates, out subset))
        {
            string str = "";

            for (int index = 0; index < subset.Count; index++)
            {
                str += string.Format("Solution {0}: \n", index + 1);
                for (int subIndex = 0; subIndex < subset[index].Count; subIndex++)
                {
                    str += string.Format("{0} \n", subset[index][subIndex]);
                }
            }

            Debug.Log(str);
        }

        Assert.AreEqual(subset.Count, 2);
    }

}
