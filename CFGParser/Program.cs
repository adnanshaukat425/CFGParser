using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA_3
{
    class Program
    {
        static void Main(string[] args)
        {
            int productionCount = 0;
            bool isValidated = false;
            Stack<char> stack = new Stack<char>();
            string[] file = File.ReadAllLines(@"C:\Users\Adnan\Downloads\PA_3\PA_3\PA_3\CFGs\CFG.txt");
            string fileText = File.ReadAllText(@"C:\Users\Adnan\Downloads\PA_3\PA_3\PA_3\CFGs\CFG.txt");
            string[] token = File.ReadAllLines(@"C:\Users\Adnan\Downloads\PA_3\PA_3\PA_3\Tokens\Token.txt");
            for (int i = 0; i < file.Length; i++)
            {
                //Counting prductions
                if (file[i] != "---")
                {
                    productionCount++;
                }
                else
                {
                    break;
                }
            }   
            //Declaring array for production
            string[] production = new string[productionCount];
            //Populating array with productions
            for (int i = 0; i < production.Length; i++)
            {
                production[i] = file[i].Substring(1, file[i].Length-1);
            }
            string[] tableData = fileText.Split(new string[] { "---" }, StringSplitOptions.RemoveEmptyEntries);
            string terminal = tableData[1];
            string[] tempData = terminal.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int terminalLength = tempData[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int nonTerminalLength = tempData[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            
            //Declaring terminal and non terminal arrays
            string[] terminalArray = new string[terminalLength];
            string[] nonTerminalArray = new string[nonTerminalLength];

            //populating terminal Array
            for (int i = 0; i < terminalLength; i++)
            {
                terminalArray[i] = tempData[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[i];
            }

            //populating terminal Array
            for (int i = 0; i < nonTerminalLength; i++)
            {
                nonTerminalArray[i] = tempData[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[i];
            }

            string[,] matrix = new string[nonTerminalLength, terminalLength];
            int temp = 2;
            int terminalIndex = -1;
            int nonTerminalIndex = -1;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (temp != tempData.Length)
                    {
                        string[] a = tempData[temp].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int k = 0; k < terminalArray.Length; k++)
                        {
                            if (a[1] == terminalArray[k])
                            {
                                terminalIndex = k;
                                break;
                            }
                        }
                        for (int k = 0; k < nonTerminalArray.Length; k++)
                        {
                            if (a[0] == nonTerminalArray[k])
                            {
                                nonTerminalIndex = k;
                                break;
                            }
                        }
                        matrix[nonTerminalIndex, terminalIndex] = a[2];
                        temp++;
                    }
                }
            }
            stack.Push('$');
            stack.Push('S');
            int row = 0;
            int col = 0;
            bool isAccepted = false;
            for (int i = 0; i < token.Length; i++)
            {
                string tokenText = token[i];
                for (int j = 0; j < tokenText.Length;)
                {
                    char stackTopValue = stack.Pop();
                    if (stackTopValue == '$')
                    {
                        if (tokenText[j] == '$')
                        {
                            isAccepted = true;
                            break;
                        }
                        else
                        {
                            isAccepted = false;
                            break;
                        }
                    }
                    for (int k = 0; k < terminalArray.Length; k++)
                    {
                        if (tokenText[j].ToString() == terminalArray[k])
                        {
                            col = k;
                            break;
                        }
                    }
                    for (int k = 0; k < nonTerminalArray.Length; k++)
                    {
                        if (stackTopValue.ToString() == nonTerminalArray[k])
                        {
                            row = k;
                            break;
                        }
                    }
                    if (row == -1 || col == -1)
                    {
                        break;
                    }
                    int productionNumber = Convert.ToInt32(matrix[row, col]);
                    row = col = -1;
                    if (productionNumber == 0)
                    {
                        break;
                    }
                    string tempProduction = production[productionNumber - 1];
                    string[] tempProductionArray = tempProduction.Split(new char[] { '=', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempProductionArray[1] != "^")
                    {
                        for (int k = tempProductionArray[1].Length - 1; k >= 0; k--)
                        {
                            stack.Push(tempProductionArray[1][k]);
                        }
                    }
                    while (stack.Peek() == tokenText[j])
                    {
                        isValidated = true;
                        stack.Pop();
                        if (stack.Count == 0)
                        {
                            isAccepted = true;
                            break;
                        }
                        j++;
                    }
                    if (!isAccepted)
                    {
                        if (stack.Peek() == '$')
                        {
                            if (tokenText[j] == '$')
                            {
                                isAccepted = true;
                                break;
                            }
                            else
                            {
                                isAccepted = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                    if (isValidated)
                    {
                        isValidated = false;
                        continue;
                    }
                    //if (stack.Peek() == tokenText[j])
                    //{
                    //    stack.Pop();
                    //}
                }
                stack = new Stack<char>();
                if (isAccepted)
                {
                    Console.WriteLine("Token " + tokenText + " Is Accepted Succesfully");
                    isAccepted = false;
                    stack.Push('$');
                    stack.Push('S');
                }
                else
                {
                    Console.WriteLine("Token " + tokenText + " Is Not Accepted Succesfully");
                    stack.Push('$');
                    stack.Push('S');
                }
            }
            Console.ReadLine();
        }
    }
}
