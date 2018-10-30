using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Calculator : MonoBehaviour {

	[SerializeField]
	Text inputText;
	[SerializeField]
	Text previousAnswerField;
	private string infix = "4^2*3-3+8/4/(1+1)";
	private bool oldAnswer = false;

	public void ButtonPressed()
    {
		string buttonValue = EventSystem.current.currentSelectedGameObject.name;
		if (buttonValue == "=") {
			Stack <string> postFix = infixToPostfix (infix);
			inputText.text = calculate(postFix);
		} else if (buttonValue == "C") {
			inputText.text = "";
			infix = "";
		} else {
			inputText.text += buttonValue;
			infix += buttonValue;
		}
			
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
					Debug.Log(number);
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
					Debug.Log(number);
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
		while(operatorStack.Peek () != "B") {
			postStack.Push (operatorStack.Pop ());
		}

		Stack <string> answer = new Stack <string> ();
		while (postStack.Count > 0) {
			answer.Push (postStack.Pop ());
		}
		return answer;
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
					Debug.Log (firstNumber + " + " + secondNumber + " = " + numbers.Peek ());
				}else if (currentElement [0] == '-') {
					double secondNumber = numbers.Pop();
					double firstNumber = numbers.Pop();
					numbers.Push (firstNumber - secondNumber);
					Debug.Log (firstNumber + " -- " + secondNumber + " = " + numbers.Peek ());

				}else if (currentElement [0] == '/') {
					double secondNumber = numbers.Pop();
					double firstNumber = numbers.Pop();
					numbers.Push (firstNumber / secondNumber);
					Debug.Log (firstNumber + " / " + secondNumber + " = " + numbers.Peek ());

				}else if (currentElement [0] == '*') {
					double secondNumber = numbers.Pop();
					double firstNumber = numbers.Pop();
					numbers.Push (firstNumber * secondNumber);
					Debug.Log (firstNumber + " * " + secondNumber + " = " + numbers.Peek ());

				}else if (currentElement [0] == '^') {
					double secondNumber = numbers.Pop();
					double firstNumber = numbers.Pop();
					double answer = 1;
					for(int j = 0; j < secondNumber; j++){
						answer = answer * firstNumber;
					}
					numbers.Push (answer);
					Debug.Log (firstNumber + " ^ " + secondNumber + " = " + numbers.Peek ());
				}
			}
		}
		return Convert.ToString(numbers.Pop());
	}
}
