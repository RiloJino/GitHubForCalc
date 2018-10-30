/*  Created By Parker Jones
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */





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
    private bool automaticTimes = false;

	public void ButtonPressed()
    {
		if (infix.Contains("Invalid")) {
            inputText.text = "";
            infix = "";
            operatorPressed = false;
            openBracket = false;
            oldAnswer = false;
        }
        string buttonValue = EventSystem.current.currentSelectedGameObject.name;
        if (buttonValue == "=" && infix != "") {
            if(infix[infix.Length-1] == '.') {
                inputText.text = inputText.text.Substring(0, infix.Length - 1);
                infix = infix.Substring(0, infix.Length - 1);
            }
            if (!BracketMiss(infix)) {
                infix = "Invalid";
                inputText.text = "Invalid";
            } else if (operatorPressed) {
				infix = "Invalid";
				inputText.text = "Invalid";
			}else {
				Stack <string> postFix = InfixToPostfix (infix);
				if (infix.Contains ("Infinity")) {
					infix = "Invalid";
					inputText.text = "Invalid";
				} else {
					string answer = Calculate (postFix);
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
            oldAnswer = false;

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
                    if (buttonValue == "="){

                    }else{
                        inputText.text += buttonValue;
                        infix += buttonValue;
                    }
				}
			} else if (operatorPressed) {
                if ("+-*/^.".IndexOf(buttonValue) >= 0)
                {
                    if(infix[infix.Length-1] == '.')
                    {
                        inputText.text += "0";
                        infix += "0";
                    }
                    if(infix[infix.Length - 1].Equals('(') && buttonValue == "-")
                    {
                        inputText.text += "0" + buttonValue;
                        infix += "0" + buttonValue;
                    }
                }
                else if (infix[infix.Length - 1].Equals('('))
                {
                    if (buttonValue == ")")
                    {
                        if (infix.Length > 1) {
                        inputText.text = inputText.text.Substring(0, infix.Length - 2);
                        infix = infix.Substring(0, infix.Length - 2);
                        }
                        else
                        {
                            inputText.text = inputText.text.Substring(0, infix.Length - 1);
                            infix = infix.Substring(0, infix.Length - 1);
                        }
                        if (infix.Length > 0)
                        {
                            if ("-*/^.(".IndexOf(infix[infix.Length - 1]) >= 0)
                            {
                                operatorPressed = true;

                            }
                            else
                            {
                                if(infix[infix.Length - 1] == '*' && automaticTimes)
                                {
                                    inputText.text = inputText.text.Substring(0, infix.Length - 1);
                                    infix = infix.Substring(0, infix.Length - 1);
                                    oldAnswer = true;
                                }
                                operatorPressed = false;
                            }
                        }
                        else
                        {
                            infix = "";
                            inputText.text = "";
                            operatorPressed = false;
                            openBracket = false;
                        }
                        buttonValue = null;
                    }
                    else
                    {
                        inputText.text += buttonValue;
                        infix += buttonValue;
                        operatorPressed = false;
                        openBracket = false;
                    }
                }
                else if (buttonValue == "(")
                {
                    inputText.text += buttonValue;
                    infix += buttonValue;
                    operatorPressed = true;
                    openBracket = true;
                }
                else if (buttonValue == ")")
                {
                    inputText.text += buttonValue;
                    infix += buttonValue;
                    openBracket = false;
                }
                else
                {
					inputText.text += buttonValue;
					infix += buttonValue;
					operatorPressed = false;
				}
			} else {
				if ("+-*/^".IndexOf (buttonValue) >= 0) {
                    if (infix[infix.Length - 1] == '.')
                    {
                        inputText.text += "0";
                        infix += "0";
                    }
                    inputText.text += buttonValue;
					infix += buttonValue;
					operatorPressed = true;
                    oldAnswer = false;
                }else{
					if (oldAnswer) {
						if (buttonValue == ")" && !openBracket) {

						} else if (buttonValue != null) {
							
                            if (buttonValue == "(")
                            {
                                if (infix[infix.Length - 1] == '.')
                                {
                                    inputText.text += "0";
                                    infix += "0";
                                }
                                inputText.text += "*" + buttonValue;
                                infix += "*" + buttonValue;
                                automaticTimes = true;
                                openBracket = true;
                                operatorPressed = true;
                            }
                            else
                            {
                                if (buttonValue != ".")
                                {
                                    if (infix[infix.Length - 1] == '.')
                                    {
                                        inputText.text += "0";
                                        infix += "0";
                                    }
                                    inputText.text += "+" + buttonValue;
                                    infix += "+" + buttonValue;
                                    operatorPressed = false;
                                }
                            }
                            oldAnswer = false;
						}
					} else {
                        if (buttonValue == "(")
                        {
                            if (infix[infix.Length - 1] == '.')
                            {
                                inputText.text += "0";
                                infix += "0";
                            }
                            inputText.text += "*" + buttonValue;
                            infix += "*" + buttonValue;
                            automaticTimes = true;
                            openBracket = true;
                            operatorPressed = true;
                        }
                        else if (buttonValue != null) {
                            inputText.text += buttonValue;
							infix += buttonValue;
							operatorPressed = false;
						}
					}
				}

			}
		}
			
    }

	private bool BracketMiss(string possibleInfix){
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


	private Stack <string> InfixToPostfix(string infix)
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
                    if (number[number.Length-1] == '.')
                    {
                        number += "0";
                    }
                    postStack.Push (number);
					number = "";
				} else if (currentElement == '-' && i == 0) {
					postStack.Push ("0");
				} else if (currentElement == '.') {
                    if (!number.Contains("."))
                    {
                        number += currentElement;
                    }
				}
				if (operatorStack.Peek() == "B") {
					operatorStack.Push (currentElement.ToString());
				} else if (CompareOperators (operatorStack.Peek (), currentElement.ToString())) {
					postStack.Push (operatorStack.Pop ());
					operatorStack.Push (currentElement.ToString());
				} else {
					operatorStack.Push (currentElement.ToString());
				}
			} else if (currentElement == '(') {
				operatorStack.Push (currentElement.ToString());
				if (number.Length > 0) {
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
			operatorStack.Push (postStack.Pop ());
		}
		return operatorStack;
	}

	private bool CompareOperators(string operator1, string operator2){
		if(operator1 == "^" && operator2 == "^"){
			return false;
		}
		int oper1 = OperatorOrder(operator1);
		int oper2 = OperatorOrder(operator2);
		if (oper1 >= oper2) {
			return true;
		}else{
			return false;
		}
	}

	private int OperatorOrder(string oper){
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

	private string Calculate(Stack <string> postFix)
	{
        Stack <float> numbers = new Stack<float> ();
		while(postFix.Count > 0){

			string currentElement = postFix.Pop ();
            if ('0' <= currentElement[0] && currentElement[0] <= '9') {
                numbers.Push(float.Parse(currentElement));
			} else {
				if (currentElement [0] == '+') {
                    float secondNumber = numbers.Pop();
                    float firstNumber = numbers.Pop();
					numbers.Push (firstNumber + secondNumber);

				}else if (currentElement [0] == '-') {
                    float secondNumber = numbers.Pop();
                    float firstNumber = numbers.Pop();
					numbers.Push (firstNumber - secondNumber);

				}else if (currentElement [0] == '/') {
                    float secondNumber = numbers.Pop();
                    float firstNumber = numbers.Pop();
					numbers.Push (firstNumber / secondNumber);

				}else if (currentElement [0] == '*') {
                    float secondNumber = numbers.Pop();
                    float firstNumber = numbers.Pop();
					numbers.Push (firstNumber * secondNumber);

				}else if (currentElement [0] == '^') {
                    float secondNumber = numbers.Pop();
                    float firstNumber = numbers.Pop();
                    float answer = 1;
                    if (secondNumber < 54)
                    {
                        if (secondNumber >= 0)
                        {
                            for (int j = 0; j < secondNumber; j++)
                            {
                                answer = answer * firstNumber;
                            }
                        }
                        else
                        {
                            for (float j = secondNumber; j < 0; j++)
                            {
                                answer = answer / firstNumber;
                            }
                        }
                        numbers.Push(answer);
                    }
                    else
                    {
                        return "Invalid: number too large";
                    }
				}
			}
		}
		return Convert.ToString(numbers.Pop());
	}
}
