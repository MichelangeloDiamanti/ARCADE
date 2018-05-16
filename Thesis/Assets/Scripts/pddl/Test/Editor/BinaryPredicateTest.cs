using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BinaryPredicateTest {

	[Test]
	public void BinaryPredicateCannotBeNull() {
		Assert.That(()=> new BinaryPredicate(null,null,null), Throws.ArgumentNullException);
	}

}
