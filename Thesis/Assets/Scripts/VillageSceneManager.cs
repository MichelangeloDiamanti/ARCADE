using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageSceneManager : MonoBehaviour {
	public string name;
	public bool flag; 
	GameObject Market1;
	GameObject Market2;
	GameObject Church1;

	GameObject Church2;

	GameObject Inn1;

	GameObject Inn2;
	List<GameObject> list = new List<GameObject>();
	

	// Use this for initialization
	void Start () {
		flag = false;

		Market1 = GameObject.Find("Market1PS");
		list.Add(Market1);
		Market1.SetActive(false);
		Church1 = GameObject.Find("Church1PS");
		list.Add(Church1);
		Church1.SetActive(false);
		Inn1 = GameObject.Find("Inn1PS");
		list.Add(Inn1);
		Inn1.SetActive(false);

		Market2 = GameObject.Find("Market2PS");
		list.Add(Market2);
		Market2.SetActive(false);
		Church2 = GameObject.Find("Church2PS");
		list.Add(Church2);
		Church2.SetActive(false);
		Inn2 = GameObject.Find("Inn2PS");
		list.Add(Inn2);
		Inn2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		if(flag)
		{
			if (name == "Market1")
			{
				Market1.SetActive(true);
			}
			else if(name == "Market2")
			{
				Market2.SetActive(true);
			}
			else if(name == "Church1")
			{
				Church1.SetActive(true);
			}
			else if(name == "Church2")
			{
				Church2.SetActive(true);
			}
			else if(name == "Inn1")
			{
				Inn1.SetActive(true);
			}
			else if(name == "Inn2")
			{
				Inn2.SetActive(true);
			}
			else{
				foreach(GameObject x in list)
				{
					if(x.activeSelf)
					{
						x.SetActive(false);
					}
			}
			}
		}else
		{
			foreach(GameObject x in list)
			{
				if(x.activeSelf)
				{
					x.SetActive(false);
				}
			}
		}
		
	}
}
