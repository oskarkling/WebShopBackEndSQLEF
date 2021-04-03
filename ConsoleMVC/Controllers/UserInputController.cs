using System;
using System.Diagnostics;
using Pages;
using Structs;


namespace Controllers
{
    internal class UserInputController
    {
        
        internal UserInputController()
        {

        }

        internal bool GetIntInput(out int validUserInput, int nrOfMenuOptions, out bool userWantToExit, out string errorMsg)
        {
            errorMsg = "";
            validUserInput = 0;
            userWantToExit = false;
            string input = Console.ReadLine();
            if(!IsInputEmpty(input))
            {
                if(!UserWantToExit(input))
                {
                    if(IsInputValid(input, out validUserInput, nrOfMenuOptions))
                    {
                        return true;
                    }
                    else
                    {
                        errorMsg = "Input is not valid";
                        Debug.WriteLine(errorMsg);
                        return false;
                    }
                }
                else
                {
                    userWantToExit = true;                  
                    return true;
                }
            }
            else
            {
                errorMsg = "No input";
                Debug.WriteLine(errorMsg);
                return false;
            }
        }

        internal bool GetStringInput(out string validUserInput, out bool userWantToExit, out string errorMsg)
        {
            errorMsg = "";
            validUserInput = "";
            userWantToExit = false;
            string input = Console.ReadLine();
            if(!IsInputEmpty(input))
            {
                if(!UserWantToExit(input))
                {
                    validUserInput = input;
                    return true;
                }
                else
                {
                    userWantToExit = true;                  
                    return true;
                }
            }
            else
            {
                errorMsg = "No input";
                Debug.WriteLine(errorMsg);
                return false;
            }
        }

        /// <summary>
        /// Checks if input is a number and within the menu options. also returns the number out int.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="number"></param>
        /// <returns>True if input is a number, else false. Also return out int the number</returns>
        private bool IsInputValid(string input, out int number, int nrOfMenuOptions)
        {
            if(Int32.TryParse(input, out number))
            {                   
                //Checks if the number is within the range of menu options.
                if(number > 0 && number <= nrOfMenuOptions)
                {
                    return true;
                }
                else
                {
                    Debug.WriteLine("Input is not within menu choice range");
                    return false;
                }
            }
            else
            {
                Debug.WriteLine("Input is not a number");
                return false;
            }
            
        }
        
        /// <summary>
        /// Checks if the user want to exit if the input is e Or E
        /// </summary>
        /// <param name="input"></param>
        /// <returns>True if input is e/E, else false</returns>
        private bool UserWantToExit(string input)
        {
            if(input == "E" || input.ToLower() == "e")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a string is empty
        /// </summary>
        /// <param name="input"></param>
        /// <returns>True if string is empty, else false.</returns>
        private bool IsInputEmpty(string input)
        {
            if(input == string.Empty)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}