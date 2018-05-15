using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRelation
{
	IPredicate getPredicate();
	bool Equals(object obj);
	string ToString();
}
