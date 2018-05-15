using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPredicate
{
	string ToString();
    bool Equals(object obj); 
}
