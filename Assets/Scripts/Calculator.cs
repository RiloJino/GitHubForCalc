/* Created By Parker Jones
 * 10/26/2018
 * I pledge that I have neither given nor received help from anyone other than the instructor for all  program  components  included  here!
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Calculator : MonoBehaviour {

    //inputText is linked to output text field
	[SerializeField]
	Text inputText;
    //used as a copy of inputText for ease of formatting
	private string infix = "";
    //oldAnswer = true means there's a previous answer already in the inputText
	private bool oldAnswer = false;
    //operatorPressed = true means the last button sucessfully entered was
    private bool operatorPressed = false;
    //openBracket = true means there's a "(" earlier in the inputText.text field that hasn't been closed
	private bool openBracket = false;
    //automaticTimes = true means a "(" was placed without a previous operator statment, 
    private bool automaticTimes = false;

    /*ButtonPressed is call by UI buttons.
	* Possible button values { "(", ")", "/", "*", "^", "+", "-", ".", "=", "C", "0", "9", "8", "7", "6", "5", "4", "3", "2", "1"}
    * 
    */
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
            //if infix statement ended with a decimal, delete it to prevent errors
            if(infix[infix.Length-1] == '.') {
                inputText.text = inputText.text.Substring(0, infix.Length - 1);
                infix = infix.Substring(0, infix.Length - 1);
            }
            //if there's an unequal amount of open/close parentheses, or if the last element is a operator, return "Invalid"
            if (!BracketMiss(infix) || operatorPressed) {
                infix = "Invalid";
                inputText.text = "Invalid";
			}else {
				Stack <string> postFix = InfixToPostfix (infix);
                //Not going to work with calculating Infinity to avoid potential errors
				if (infix.Contains ("Infinity")) {
					infix = "Invalid";
					inputText.text = "Invalid";
				} else {
					string answer = Calculate (postFix);
					inputText.text = answer;
					infix = answer;
					operatorPressed = false;
                    openBracket = false;
                    oldAnswer = true;
				}
			}
        //User Clear command
		} else if (buttonValue == "C") {
			inputText.text = "";
			infix = "";
			operatorPressed = false;
			openBracket = false;
            oldAnswer = false;
        //buttonValue must be number or operator
        }else {
			if (infix == "") {
				if ("+*/^)".IndexOf (buttonValue) >= 0) {
					infix = "Invalid";
					inputText.text = "Invalid";
                //the two possible operators we can assume a "0" in front of
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
                        //if they press "=" on an empty field no need to do anything
                    }else{
                        //getting this far means the buttonValue is a number
                        inputText.text += buttonValue;
                        infix += buttonValue;
                    }
				}
			} else if (operatorPressed) {
                if ("+-*/^.".IndexOf(buttonValue) >= 0)
                {
                    //if previous element was a decimal add a 0 after to avoid errors
                    if(infix[infix.Length-1] == '.')
                    {
                        inputText.text += "0";
                        infix += "0";
                    }
                    //if we need to treat this as a "-" in the start of a new parentheses
                    if (infix[infix.Length - 1].Equals('(') && buttonValue == "-")
                    {
                        inputText.text += "0" + buttonValue;
                        infix += "0" + buttonValue;
                    }
                }
                else if (infix[infix.Length - 1].Equals('('))
                {
                    if (buttonValue == ")")
                    {
                        //if there's an empty parentheses pair with a operator before delete all 3 elements
                        if (infix.Length > 1) {
                            inputText.text = inputText.text.Substring(0, infix.Length - 2);
                            infix = infix.Substring(0, infix.Length - 2);
                            openBracket = false;
                        }
                        else
                        {
                            //empty parentheses is the only input, clear input
                            infix = "";
                            inputText.text = "";
                            operatorPressed = false;
                            openBracket = false;
                        }
                        if (infix.Length > 0)
                        {
                            //after removing any potential empty parentheses pairs check for last element type
                            if ("-*/^.(".IndexOf(infix[infix.Length - 1]) >= 0)
                            {
                                operatorPressed = true;
                            }
                            else
                            {
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
                //last element was a number
				if ("+-*/^".IndexOf (buttonValue) >= 0) {
                    //if a operator is being added right after a decimal add a "0" after the decimal
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
                    //the current element is a number, parentheses, or decimal
                    if (oldAnswer) {
                        if (buttonValue != null) {
							//format for a new parentheses block
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
                        //format for a new parentheses block
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
                            //it's a number or decimal
                            if (buttonValue != ".")
                            {
                                inputText.text += buttonValue;
                                infix += buttonValue;
                            }
                            else
                            {
                                if (infix[infix.Length - 1] != '.')
                                {
                                    inputText.text += buttonValue;
                                    infix += buttonValue;
                                }
                            }
							operatorPressed = false;
						}
					}
				}

			}
		}
			
    }
    //checks possibleInfix string for an uneven open to close parentheses, returns true if balaneced
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

    //takes a infix string, using two Stack <string> to create a postfix order
	private Stack <string> InfixToPostfix(string infix)
	{
		Stack <string> postStack = new Stack <string> ();
		Stack <string> operatorStack = new Stack <string> ();
        //used to declare the base of operatorStack
		operatorStack.Push ("B");
		string number = "";

        /*First each element is checked if it's a number,
        * then special characters are checked,
        * decimals are ignored after the first decimal in a number
         */
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
                } if (currentElement != '.' && CompareOperators (operatorStack.Peek (), currentElement.ToString())) {
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
				}if (operatorStack.Peek() != "B") {
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

    //operators are rated by mathmatical order, anything not "^+_*/" is given a value of 0, currently decimals are the only thing being ignored
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

    //given an operator, returns its mathmatical order
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

    /* Runs through postFix of strings
     * if the string is a number it's converted into a float 
     * else the string is used to determine the next operation
     * to limit extreme calculations, power by is limited to 999 to -999
    */
	private string Calculate(Stack <string> postFix)
	{
        Stack <float> numbers = new Stack<float> ();
        numbers.Push(0);
        while (postFix.Count > 0){
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
                    if (secondNumber < 1000 && secondNumber > -1000)
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
