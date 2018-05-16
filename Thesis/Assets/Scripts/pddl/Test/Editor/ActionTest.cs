using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class ActionTest {

	[Test]
	public void ActionDefinitionCannotBeNull() {
		Assert.That(()=> new Action(null,null,null,null), Throws.ArgumentNullException);
	}
	
}
