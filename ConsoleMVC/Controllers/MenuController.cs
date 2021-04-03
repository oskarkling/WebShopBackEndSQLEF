using System.Diagnostics;
using System;
using System.Threading;
using Inlamningsuppgift2WebbShopAPI;
using Inlamningsuppgift2WebbShopAPI.Database;
using Inlamningsuppgift2WebbShopAPI.Models;
using Views;
using Pages;
using Structs;

namespace Controllers
{
    internal class MenuController
    {
        ViewConsole view;
        PageController pageController;
        UserInputController userInputController;
        internal MenuController()
        {
            view = new ViewConsole(); 
            pageController = new PageController();
            userInputController = new UserInputController();
        }

        /// <summary>
        /// Returns true if user want to try aggin.
        /// </summary>
        /// <returns>True if successful else false</returns>
        internal bool TryAgain()
        {
            view.PrintToConsole("Try again? Y or N");
            if(userInputController.GetStringInput(out string validUserInput, out bool userWantToExitOut, out string errormsg))
            {
                if(validUserInput == "Y" || validUserInput == "y")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                userWantToExitOut = true;
                return false;
            }
        }
        
        /// <summary>
        /// Returns LoginDetails of a users input.
        /// </summary>
        /// <param name="tempDetails"></param>
        /// <param name="userWantToExitOut"></param>
        /// <param name="newUser"></param>
        /// <returns>True if sucessful, else false. Bool if user want to exit and string with errormessage</returns>
        internal bool LoginMenu(out LoginDetails tempDetails, out bool userWantToExitOut, out bool newUser)
        {
            tempDetails = new LoginDetails();
            userWantToExitOut = false;
            newUser = false;

            while(true)
            {
                //Prints the login menu
                view.Display(pageController.GetPage(PageType.LoginMenu));              

                //Gets a userinput and if its valid and if user want to exit
                bool IsUserInputValid = userInputController.GetIntInput(out int validMenuUserInput, 
                pageController.GetPage(PageType.LoginMenu).NrOfMenuOptions, 
                out bool userWantToExitIn,
                out string errorMsg);

                if(userWantToExitIn)
                {
                    userWantToExitOut = true;
                    return false;
                }

                if(IsUserInputValid)
                {
                    switch(validMenuUserInput)
                    {
                        case 1:
                            if(GetLoginDetails(out LoginDetails userLoginDetails, out bool userWantToExitOut2, out string returnErrorMsg))
                            {
                                if(userWantToExitOut2)
                                {
                                    userWantToExitOut = true;
                                    return false;
                                }
                                tempDetails = userLoginDetails;
                            }
                            else
                            {
                                view.PrintToConsole(returnErrorMsg);                         
                            }
                            break;
                        case 2:                         
                            if(Register(out LoginDetails newUserLoginDetails, out bool userWantToExitOut3, out string returnErrorMsg2))
                            {
                                if(userWantToExitOut3)
                                {
                                    userWantToExitOut = true;
                                    return false;
                                }
                                tempDetails = newUserLoginDetails;
                                newUser = true;
                                                                   
                            }
                            else
                            {
                                view.PrintToConsole(returnErrorMsg2);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                }
                else
                {
                    view.PrintToConsole(errorMsg);
                }
            }
            return true;
        }

        internal bool AdminMenu(out int menuChoice, out bool userWantToExitOut, out string ErrorMsgOut)
        {
            menuChoice = 0;
            ErrorMsgOut = "";
            userWantToExitOut = false;

            view.Display(pageController.GetPage(PageType.AdminMenu));

            bool isInputValid = userInputController.GetIntInput(out int validUserInput, 
            pageController.GetPage(PageType.AdminMenu).NrOfMenuOptions,
            out bool userWantToExitIn, out string errorMsgIn);

            if(userWantToExitIn)
            {
                userWantToExitOut = true;
                return false;
            }

            if(isInputValid)
            {
                menuChoice = validUserInput;
                return true;
            }
            else
            {
                ErrorMsgOut = errorMsgIn;
                return false;
            }
        }

        /// <summary>
        /// Shows the menu to the user
        /// also returns the menuc hoice from the user.
        /// </summary>
        /// <returns>True if successful, else false. Also returns bool if user want to exit and a string error message</returns>
        internal bool UserMenu(out int menuChoice, out bool userWantToExitOut, out string ErrorMsgOut)
        {
            menuChoice = 0;
            ErrorMsgOut = "";
            userWantToExitOut = false;

            view.Display(pageController.GetPage(PageType.UserMenu));

            bool isInputValid = userInputController.GetIntInput(out int validUserInput, 
            pageController.GetPage(PageType.UserMenu).NrOfMenuOptions,
            out bool userWantToExitIn, out string errorMsgIn);

            if(userWantToExitIn)
            {
                userWantToExitOut = true;
                return false;
            }

            if(isInputValid)
            {
                menuChoice = validUserInput;
                return true;
            }
            else
            {
                ErrorMsgOut = errorMsgIn;
                return false;
            }
        }

        internal bool GetSearchStringInput(string searchMsg, out string input, out bool userWantToExitOut, out string returnErrorMsg)
        {
            input = "";
            userWantToExitOut = false;
            returnErrorMsg = "";

            view.PrintToConsole(searchMsg);

            if(userInputController.GetStringInput(out string validUserInput, out bool userWantToExitIn, out string errorMsgIn))
            {
                if(userWantToExitIn)
                {
                    userWantToExitOut = true;
                    return false;
                }

                input = validUserInput;
                return true;
            }
            else
            {
                returnErrorMsg = errorMsgIn;
                return false;
            }
        }

        internal bool GetSearchIntInput(string searchMsg, out int input, out bool userWantToExitOut, out string returnErrorMsg)
        {
            input = 0;
            userWantToExitOut = false;
            returnErrorMsg = "";

            view.PrintToConsole(searchMsg);

            if(userInputController.GetIntInput(out int validUserInput, Int32.MaxValue, out bool userWantToExitIn, out string errorMsgIn))
            {
                if(userWantToExitIn)
                {
                    userWantToExitOut = true;
                    return false;
                }

                input = validUserInput;
                return true;
            }
            else
            {
                returnErrorMsg = errorMsgIn;
                return false;
            }
        }
        
        internal void ClearConsole()
        {
            view.ClearConsole();
        }

        internal void PrintResults(string msg)
        {
            ClearConsole();
            view.PrintToConsole(msg);
        }

        /// <summary>
        /// Prints a string to the view.
        /// </summary>
        /// <param name="errorMsg"></param>
        internal void ErrorPrint(string errorMsg) 
        {
            view.PrintToConsole(errorMsg);
        }

        /// <summary>
        /// Prints a string to the view.
        /// </summary>
        /// <param name="msg"></param>
        internal void PrintMsg(string msg)
        {
            view.PrintToConsole(msg);
        }

        internal void Welcome()
        {
            view.Display(pageController.GetPage(PageType.Welcome));
            view.ChangeForeGroundColor(ConsoleColor.Green);
        }

        /// <summary>
        /// Returns the LoginDetails of a users input. Also returns if the user wants to exit, and error message as a string.
        /// </summary>
        /// <param name="userLoginDetails"></param>
        /// <param name="userWantToExitOut"></param>
        /// <param name="returnErrorMsg"></param>
        /// <returns>True if sucessful, else false. Bool if user want to exit and string with errormessage</returns>
        private bool GetLoginDetails(out LoginDetails userLoginDetails, out bool userWantToExitOut, out string returnErrorMsg)
        {
            userLoginDetails = new LoginDetails();
            returnErrorMsg ="";
            userWantToExitOut = false;


            view.PrintToConsole("Enter Username");
            if(userInputController.GetStringInput(out string validUserName, out bool userWantToExitIn, out string errorMsg))
            {
                if(userWantToExitIn)
                {
                    userWantToExitOut = true;
                    return false;
                }

                view.PrintToConsole("Enter Password");
                if(userInputController.GetStringInput(out string validPassword, out bool userWantToExitIn2, out string errorMsg2))
                {
                    if(userWantToExitIn2)
                    {
                        userWantToExitOut = true;
                        return false;
                    }
                    userLoginDetails = new LoginDetails(validUserName, validPassword, validPassword);
                    view.PrintToConsole("Trying to log in...");
                    return true;
                }
                else
                {
                    returnErrorMsg = errorMsg2;
                    return false;
                }
            }
            else
            {
                returnErrorMsg = errorMsg;
                return false;
            }
        }

        /// <summary>
        /// Returns the Register Details as LoginDetails type - of a users input. 
        /// Also returns if the user wants to exit, and error message as a string.
        /// </summary>
        /// <param name="newUserLoginDetails"></param>
        /// <param name="verifiedPasswordOut"></param>
        /// <param name="userWantToExitOut"></param>
        /// <param name="returnErrorMsg"></param>
        /// <returns>True if sucessful, else false. Bool if user want to exit and string with errormessage</returns>
        private bool Register(out LoginDetails newUserLoginDetails, out bool userWantToExitOut, out string returnErrorMsg)
        {
            newUserLoginDetails = new LoginDetails();
            returnErrorMsg ="";
            userWantToExitOut = false;

            view.PrintToConsole("---Register new account---");
            view.PrintToConsole("Enter a Username");
            if(userInputController.GetStringInput(out string validUserName, out bool userWantToExitIn, out string errorMsg))
            {
                if(userWantToExitIn)
                {
                    userWantToExitOut = true;
                    return false;
                }

                view.PrintToConsole("Enter a Password");
                if(userInputController.GetStringInput(out string validPassword, out bool userWantToExitIn2, out string errorMsg2))
                {
                    if(userWantToExitIn2)
                    {
                        userWantToExitOut = true;
                        return false;
                    }

                    view.PrintToConsole("Enter your password again to verify it");
                    if(userInputController.GetStringInput(out string verifiedPasswordIn, out bool userWantToExitIn3, out string errorMsg3))
                    {
                        if(userWantToExitIn3)
                        {
                            userWantToExitOut = true;
                            return false;
                        }
                        newUserLoginDetails = new LoginDetails(validUserName, validPassword, verifiedPasswordIn);                   
                        view.PrintToConsole("Trying to register..");
                        return true;
                    }
                    else
                    {
                        returnErrorMsg = errorMsg3;
                        return false;
                    }     
                }
                else
                {
                    returnErrorMsg = errorMsg2;
                    return false;
                }
            }
            else
            {
                returnErrorMsg = errorMsg;
                return false;
            }
        }
    }
}