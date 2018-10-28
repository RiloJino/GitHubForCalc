using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Calculator : MonoBehaviour {

	[SerializeField]
	Text inputText;
	string buttonValue;

	public void ButtonPressed()
    {
		buttonValue = EventSystem.current.currentSelectedGameObject.name;
		inputText.text += buttonValue;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
