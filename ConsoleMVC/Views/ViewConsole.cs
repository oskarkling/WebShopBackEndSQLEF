using System;
using System.Diagnostics;
using Controllers;
using Pages;

namespace Views
{
    internal class ViewConsole
    {
        internal ViewConsole()
        {
            try
            {
                Console.SetWindowSize(120, 60);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Blue;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("User is not on windows" + ex);
            }
        }

        internal void PrintToConsole(string msg)
        {
            Console.WriteLine(msg);
        }

        internal void Display()
        {
            

        }
        internal void ChangeForeGroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        internal void ClearConsole()
        {
            Console.Clear();
        }
        
        internal void Display(Page page)
        {
            
            Console.WriteLine(page.Message);
            Console.WriteLine("\nInput e to Exit program");
        }

        internal void EnterValidIntInput(int options)
        {
            Console.Clear();
            Console.WriteLine($"Please enter valid input between 1 and {options.ToString()}");
        }

        internal void EnterValidStringInput(string input)
        {
            Console.Clear();
            Console.WriteLine($"Please enter valid {input}.");
        }


        private void LoginMenu()
        {
            Console.WriteLine($@"1. Login
2. Register");

        }
        
    }
}