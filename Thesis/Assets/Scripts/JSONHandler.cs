using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  
using System.Runtime.Serialization;
using System;
using System.Web.UI;

public class JSONHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
    }  

	/*	
	public static string ToJson(object obj) {
		JavaScriptSerializer serializer = new JavaScriptSerializer();
		return serializer.Serialize(obj); 
	}

	public static string ToJson(object obj, int recursionDepth) {
		JavaScriptSerializer serializer = new JavaScriptSerializer();
		serializer.RecursionLimit = recursionDepth;
		return serializer.Serialize(obj); 
	}
	*/
}
