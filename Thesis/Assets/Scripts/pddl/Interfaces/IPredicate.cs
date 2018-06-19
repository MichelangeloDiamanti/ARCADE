using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPredicate
{
	string Name{get;}
	EntityType Source{get;}
	IPredicate Clone();
    bool Equals(object obj); 
	string ToString();
}
