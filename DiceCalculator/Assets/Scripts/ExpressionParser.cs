using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public class ExpressionParser : MonoBehaviour
{
    public List<ExpressionManager> expressions = new List<ExpressionManager>();
    private int id = 0;

    void TestInitialize()
    {
        // 1d8 * (2d6 + 3)
        /* ExpressionManager expr1 = new ExpressionManager();
        expr1.value = "1d8";
        expr1.type = ExpressionType.Value;

        ExpressionManager expr2 = new ExpressionManager();
        expr2.value = "2d6";
        expr2.type = ExpressionType.Value;

        ExpressionManager expr3 = new ExpressionManager();
        expr3.value = "3";
        expr3.type = ExpressionType.Value;

        ExpressionManager expr4 = new ExpressionManager();
        expr4.value = "+";
        expr4.type = ExpressionType.Operator;
        expr4.expressions.Add(expr2);
        expr4.expressions.Add(expr3);

        ExpressionManager expr5 = new ExpressionManager();
        expr5.value = "";
        expr5.type = ExpressionType.Parentesis;
        expr5.expressions.Add(expr4);

        ExpressionManager expr6 = new ExpressionManager();
        expr6.value = "*";
        expr6.type = ExpressionType.Operator;
        expr6.expressions.Add(expr1);
        expr6.expressions.Add(expr5);

        expressions.Add(expr6); */

        // 1d6 + 2d4

        ExpressionManager expr1 = new ExpressionManager();
        expr1.value = "1d6";
        expr1.type = ExpressionType.Value;
        
        ExpressionManager expr2 = new ExpressionManager();
        expr2.value = "2d4";
        expr2.type = ExpressionType.Value;

        ExpressionManager expr3 = new ExpressionManager();
        expr3.value = "+";
        expr3.type = ExpressionType.Operator;
        expr3.expressions.Add(expr1);
        expr3.expressions.Add(expr2);

        expressions.Add(expr3);
    }

    void Start()
    {
        Debug.Log("Expression Parser");
        TestInitialize();
        string text = Parse(expressions);
        Debug.Log(text);

        LoadDiceRollerResults(text);
    }

    string ValueToString(ExpressionManager expr)
    {
        if (!expr.value.Contains("d"))
            return expr.value;
        else
        {
            string text = "[% ";
            /*
            Input : ExpressionManager (ex. expressions = [], value = "26d64", type = ExpressionType.Value)
            Output : string : "[% 26 d64]"
            */

            foreach (char c in expr.value)
            {
                if (c == 'd')
                    text += " d";
                else
                    text += c;
            }

            text += "]";

            return text;
        }
    }

    string Parse(List<ExpressionManager> exprs)
    {
        string text = "";
        
        /*
        Input:
        (Note : the inputs are instances of ExpressionManager class
        Attributes :
        - expressions (List<ExpressionManager>) -- list of sub-expressions
        - value (string) -- string of the expression
        - type (ExpressionType) -- type of the expression -- value, operator, parentesis)

        - expression types
            - value (number)
            - operator
                - addition (+)
                - subtraction (-)
                - multiplication (*)
                - division (/)
                - n dice roll (%) -> n dN
            - parentesis
        
        Output:
        - string
            - Example :
                - Input : "1d6 + 2d4"
                - Output : "[+ [% 1 d6] [% 2 d4]]"
            - Example 2 :
                - Input : "1d8 * (2d6 + 3)"
                - Output : "[* [% 1 d8] [+ [% 2 d6] 3]]"
        
        Brainstorm:
        - hypothetic input : 1d8 * (2d6 + 3)
        - wanted output : [* [% 1 d8] [+ [% 2 d6] 3]]
        text: ""
        */

        foreach (ExpressionManager expr in exprs)
        {
            if (expr.type == ExpressionType.Value)
                text += ValueToString(expr);
            
            else if (expr.type == ExpressionType.Operator)
            {
                text += "[" + expr.value;
                foreach (ExpressionManager subExpr in expr.expressions)
                {
                    List<ExpressionManager> subExprs = new List<ExpressionManager>();
                    subExprs.Add(subExpr);
                    
                    text += " " + Parse(subExprs);
                }
                text += "]";
            }

            else if (expr.type == ExpressionType.Parentesis)
                text += Parse(expr.expressions);
        }

        return text;
    }

    string GetPath(string fileName)
    {
        string path = Path.GetFullPath(fileName);
        // current : C:\Users\user\Documents\GitHub\DiceRoller\DiceRoller\fileName.txt

        // remove the last directory with a \
        // wanted : C:\Users\user\Documents\GitHub\DiceRoller\fileName.txt

        // the code :
        int index = path.LastIndexOf("\\");
        path = path.Substring(0, index);
        index = path.LastIndexOf("\\");
        path = path.Substring(0, index + 1);
        
        path = path + fileName;
        return path;
    }
    
    void SendToDiceRoller(string text)
    {
        string fullPath = GetPath("commands.txt");
        Debug.Log(fullPath);
        
        File.WriteAllText(fullPath,string.Empty);

        using (StreamWriter writer = new StreamWriter(fullPath))
        {
            writer.WriteLine(id.ToString());
            Debug.Log("Command");
            Debug.Log(id.ToString());
            writer.WriteLine(text);
            Debug.Log(text);
        }
    }

    float TextToFloat(string text)
    {
        int numbers;
        
        if (text.Contains("."))
        {
            Debug.Log($"Text : {text}, Index : {text.IndexOf('.')}, Length : {text.Length}");
            numbers = text.Length - (text.IndexOf('.') + 2);
        }
        else
            numbers = 0;

        text = text.Replace(".", "");
        
        float value = float.Parse(text);
        value /= Mathf.Pow(10, numbers);

        return value;
    }

    List<Vector2> LoadDiceRollerResults(string text)
    {
        string fullPath = GetPath("results.txt");
        
        while (true)
        {
            string readText = File.ReadAllText(fullPath);

            if (readText != "")
            {
                List<Vector2> results = new List<Vector2>();

                string[] lines = readText.Split('\n');
                int detected_id;

                int i = 0;
                foreach (string line in lines)
                {
                    if (i == 0)
                        detected_id = int.Parse(line) + 1;
                    
                    else
                    {
                        string[] values = line.Split(' ');
                        if (values.Length == 2)
                        {
                            float x = TextToFloat(values[0]);
                            float y = TextToFloat(values[1]);

                            Debug.Log($"Values : {x}, {y}");
                        }
                    }
                    i += 1;
                }

                if (detected_id != id)
                {
                    id = detected_id;
                    
                    Debug.Log("Results");
                    Debug.Log(results);
                    Debug.Log(readText);

                    return results;
                }
            }
        }
    }
}
