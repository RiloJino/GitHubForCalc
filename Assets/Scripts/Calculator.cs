using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Calculator : MonoBehaviour {

	[SerializeField]
	Text inputText;
	string postfix = "";

	public void ButtonPressed()
    {
		string buttonValue = EventSystem.current.currentSelectedGameObject.name;
		if (buttonValue [0] == '=') {
			inputText.text = calculate (postfix);
		} else if (buttonValue [0] == 'C') {
			inputText.text = "";
		} else {
			inputText.text += buttonValue;
		}
			
    }

	public string calculate(string postfix)
	{
		return "Answer";
	}

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
