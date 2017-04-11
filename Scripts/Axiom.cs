using System;

using UnityEngine;

using System.Collections;
using System.Collections.Generic;


namespace ninja.marching.flatstates{
    using System.Linq;

    public class Axiom {

		public readonly string Name;
		public readonly Term[] Terms;

		public Axiom(string name, params Term[] terms)
		{
			Name = name;

			Terms = terms;
		}

	    public override string ToString ()
		{
			string str = string.Join(",", Terms.Select(_ => _.ToString()).ToArray());
            
			return string.Format ("{0} -> {1}", Name, str );
		}
    
    }
}
