using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRelation
{
	IPredicate getPredicate();
	bool Value{ get; set;}
	bool EqualsThroughPredicate(object obj);
	string ToString();
    bool Equals(object obj); 
	bool EqualsWithoutValue(object obj);
	IRelation Clone();
}
