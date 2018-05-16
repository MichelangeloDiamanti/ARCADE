using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnaryPredicateTest {
	
	[Test]
	public void UnaryPredicateCannotBeNull() {
		Assert.That(()=> new UnaryPredicate(null,null), Throws.ArgumentNullException);
	}

}
