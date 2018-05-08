using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsBehaviour : MonoBehaviour {

	public GameObject button;
	public GameObject gameManager;
	private PointAndClick pointAndClick;

    // Use this for initialization
    void Start () {
		Button btn = button.GetComponent<Button>();
		// PointAndClick x = FindObjectOfType<PointAndClick>();

		pointAndClick = gameManager.GetComponent<PointAndClick>();
		btn.onClick.AddListener(pointAndClick.MoveCharacter);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
