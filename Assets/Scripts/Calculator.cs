using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Calculator : MonoBehaviour {

	[SerializeField]
	Text inputText;
	private string infix = "";
	private bool oldAnswer = false;
	private bool operatorPressed = false;
	private bool openBracket = false;

	public void ButtonPressed()
    {
		if (infix == "Invalid") {
			inputText.text = "";
			infix = "";
		}
		string buttonValue = EventSystem.current.currentSelectedGameObject.name;
		if (buttonValue == "=") {
			if (bracketMiss (infix)) {

			} else {
				Stack <string> postFix = infixToPostfix (infix);
				if (infix.Contains ("Infinity")) {
					infix = "Invalid";
					inputText.text = "Invalid";
				} else {
					string answer = calculate (postFix);
					inputText.text = answer;
					infix = answer;
					operatorPressed = false;
					oldAnswer = true;
				}
			}
		} else if (buttonValue == "C") {
			inputText.text = "";
			infix = "";
			operatorPressed = false;
			openBracket = false;
		} else {
			if (infix == "") {
				if ("+*/^)".IndexOf (buttonValue) >= 0) {
					infix = "Invalid";
					inputText.text = "Invalid";
				} else if (buttonValue == "." || buttonValue == "-") {
					inputText.text += "0" + buttonValue;
					infix += "0" + buttonValue;
					operatorPressed = true;
				} else if (buttonValue == "(") {
					inputText.text += buttonValue;
					infix += buttonValue;
					operatorPressed = true;
					openBracket = true;
				} else {
					inputText.text += buttonValue;
					infix += buttonValue;
				}
			} else if (operatorPressed) {
				if ("+-*/^.".IndexOf (buttonValue) >= 0) {
					infix = "Invalid";
					inputText.text = "Invalid";
				} else if (infix [infix.Length - 1].Equals ('(')) {
					if (buttonValue == ")") {
						inputText.text = inputText.text.Substring (0, infix.Length - 1);
						infix = infix.Substring (0, infix.Length - 1);
						buttonValue = null;
					} else {
						inputText.text += buttonValue;
						infix += buttonValue;
						operatorPressed = false;
						openBracket = false;
					}
				} else if (buttonValue == "(") {
					inputText.text += buttonValue;
					infix += buttonValue;
					operatorPressed = true;
					openBracket = true;

				} else {
					inputText.text += buttonValue;
					infix += buttonValue;
					operatorPressed = false;
					openBracket = false;
				}
			} else {
				if ("+-*/^.".IndexOf (buttonValue) >= 0) {
					inputText.text += buttonValue;
					infix += buttonValue;
					operatorPressed = true;
				} else if(buttonValue == "("){
					inputText.text += buttonValue;
					infix += buttonValue;
					operatorPressed = true;
					openBracket = true;
				}else{
					if (oldAnswer) {
						if (buttonValue == ")" && !openBracket) {

						} else if (buttonValue != null) {
							inputText.text += "+" + buttonValue;
							infix += "+" + buttonValue;
							operatorPressed = false;
						} else if (buttonValue == "(") {

						}
					} else {
						if (buttonValue == ")" && !openBracket) {

						} else if (buttonValue != null) {
							inputText.text += buttonValue;
							infix += buttonValue;
							operatorPressed = false;
						}
					}
				}

			}
		}
			
    }

	private bool bracketMiss(string possibleInfix){
		int open = 0;
		int close = 0;
		for (int i = 0; i < possibleInfix.Length; i++) {
			if (possibleInfix [i] == '(') {
				open++;
			} else if (possibleInfix [i] == ')') {
				close++;
			}
		}
		if(open == close){
		return true;
		}
		return false;
	}


	private Stack <string> infixToPostfix(string infix)
	{
		Stack <string> postStack = new Stack <string> ();
		Stack <string> operatorStack = new Stack <string> ();
		operatorStack.Push ("B");
		string number = "";
		for (int i = 0; i < infix.Length; i++) 
		{
			char currentElement = infix [i];
			if ('0' <= currentElement && currentElement <= '9') {
				number += currentElement;
			} else if ("+-*/^.".IndexOf (currentElement) >= 0) {
				if (number.Length > 0 && currentElement != '.') {
					postStack.Push (number);
					number = "";
				} else if (currentElement == '-' && i == 0) {
					postStack.Push ("0");
				} else if (currentElement == '.') {
					number += currentElement;
				}
				if (operatorStack.Peek() == "B") {
					operatorStack.Push (currentElement.ToString());
				} else if (compareOperators (operatorStack.Peek (), currentElement.ToString())) {
					postStack.Push (operatorStack.Pop ());
					operatorStack.Push (currentElement.ToString());
				} else {
					operatorStack.Push (currentElement.ToString());
				}
			} else if (currentElement == '(') {
				operatorStack.Push (currentElement.ToString());
				if (number.Length > 0) {
					Debug.Log(number);
					postStack.Push (number);
					number = "";
				}
			} else if (currentElement == ')') {
				if (number.Length > 0) {
					postStack.Push (number);
					number = "";
				}
				while (operatorStack.Peek() != "B" && operatorStack.Peek () != "(") {
					postStack.Push (operatorStack.Pop ());
				}if (operatorStack.Peek() == "B") {
					Debug.Log ("error");
				} else {
					operatorStack.Pop ();
				}
			}
		}
		if (number.Length > 0) {
			postStack.Push (number);
			number = "";
		}
		while(operatorStack.Peek () != "B") {
			postStack.Push (operatorStack.Pop ());
		}
		while (postStack.Count > 0) {
			Debug.Log (postStack.Peek ());
			operatorStack.Push (postStack.Pop ());
		}
		return operatorStack;
	}

	private bool compareOperators(string operator1, string operator2){
		if(operator1 == "^" && operator2 == "^"){
			return false;
		}
		int oper1 = operatorOrder(operator1);
		int oper2 = operatorOrder(operator2);
		if (oper1 >= oper2) {
			return true;
		}else{
			return false;
		}
	}

	private int operatorOrder(string oper){
		if (oper == "+" || oper == "-") {
			return 1;
		}
		if (oper == "*" || oper == "/") {
			return 2;
		}
		if (oper == "^") {
			return 3;
		}
		return 0;
	}

	private string calculate(Stack <string> postFix)
	{
		Stack <double> numbers = new Stack<double> ();
		while(postFix.Count > 0){
			string currentElement = postFix.Pop ();
			if ('0' <= currentElement [0] && currentElement [0] <= '9') {				
				numbers.Push (Convert.ToDouble(currentElement));
			} else {
				if (currentElement [0] == '+') {
					double secondNumber = numbers.Pop();
					double firstNumber = numbers.Pop();
					numbers.Push (firstNumber + secondNumber);

				}else if (currentElement [0] == '-') {
					double secondNumber = numbers.Pop();
					double firstNumber = numbers.Pop();
					numbers.Push (firstNumber - secondNumber);

				}else if (currentElement [0] == '/') {
					double secondNumber = numbers.Pop();
					double firstNumber = numbers.Pop();
					numbers.Push (firstNumber / secondNumber);

				}else if (currentElement [0] == '*') {
					double secondNumber = numbers.Pop();
					double firstNumber = numbers.Pop();
					numbers.Push (firstNumber * secondNumber);

				}else if (currentElement [0] == '^') {
					double secondNumber = numbers.Pop();
					double firstNumber = numbers.Pop();
					double answer = 1;
					for(int j = 0; j < secondNumber; j++){
						answer = answer * firstNumber;
					}
					numbers.Push (answer);
				}
			}
		}
		Debug.Log ("Done");
		return Convert.ToString(numbers.Pop());
	}
}
