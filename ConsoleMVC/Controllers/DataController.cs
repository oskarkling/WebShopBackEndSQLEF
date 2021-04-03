using System;
using System.Diagnostics;
using Inlamningsuppgift2WebbShopAPI;
using Inlamningsuppgift2WebbShopAPI.Database;
using Inlamningsuppgift2WebbShopAPI.Models;
using Structs;
using System.Timers;
using System.Collections.Generic;

namespace Controllers
{
    internal class DataController
    {
        WebbShopAPI webAPI;
        private static Timer aTimer;
        internal DataController()
        {
            webAPI = new WebbShopAPI();           
        }

        /// <summary>
        /// Logs the user into database. by Logindetails provided to the moethod.
        /// </summary>
        /// <param name="details"></param>
        /// <param name="userId"></param>
        /// <param name="errorMsg"></param>
        /// <returns>True if successful, else false. Also returns userId and string error message</returns>
        internal bool LoginUser(LoginDetails details, out int userId, out string errorMsg)
        {
            errorMsg = "";

            //Convert from int? to int.
            userId = ConvertToInteger(webAPI.Login(details.UserName, details.Password));

            if(userId != 0)
            {
                SetTimer(userId);
                return true;
            }
            else 
            {
                errorMsg = "Failed to login.";
                return false;
            }
        }

        /// <summary>
        /// Registers the user to the database by login details provided to the method. Method calls WebbShopAPI method for register. 
        /// </summary>
        /// <param name="details"></param>
        /// <param name="errorMsg"></param>
        /// <returns>True if successful, else false. Also returns a string error message if fail.</returns>
        internal bool RegisterUser(LoginDetails details, out string errorMsg)
        {
            errorMsg = "";

            // Just to be sure. Sometimes you dont know if APIs written by other people checks the passwords. I know mine does but better safe than sorry.
            if(details.Password == details.VerifiedPassword)
            {
                if(webAPI.Register(details.UserName, details.Password, details.VerifiedPassword))
                {
                    return true;
                }
                else
                {
                    errorMsg = "Unable to register user";
                    Debug.WriteLine(errorMsg);
                    return false;
                }
            }
            else
            {
                errorMsg = "Passwords does not match.";
                return false;
            }
        }
        
        /// <summary>
        /// Logs the user out from the database by user ID input.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True if successful, else false</returns>
        internal bool LogOutUser(int userId)
        {
            webAPI.Logout(userId);
            string check = webAPI.Ping(userId);
            if(check != "Pong")
            {
                return true;
            }
            else
            {
                Debug.WriteLine("User still active when logout is called");
                return false;
            }
        }

        internal bool IsUserAdmin(int userId)
        {
            if(webAPI.IsUserAdmin(userId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool IsSessionActive(int userId, out string errorMsg)
        {
            errorMsg = "";
            string answer = webAPI.Ping(userId);

            if(answer == "Pong")
            {
                return true;
            }
            else
            {
                errorMsg = "Session Timed out";
                return false;
            }
        }
        internal string GetSpecificCategory(string input, int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut ="";
                return "";
            }
            else
            {
                string results ="ID CATEGORY\n";
                var list = webAPI.GetCategories(input);
                if(list != null)
                {
                    foreach (BookCategory s in list)
                    {
                        results += s.Id + ". " + s.Category + "\n";
                        
                    }
                    return results;
                }
                else
                {
                    errorMsgOut = "Error loading books by category id";
                    Debug.WriteLine("Getcategories list by id was null");
                    return "";
                }
            }
        }
        internal string GetBooksByCategoryId(int userInput, int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return "";
            }
            else
            {
                string results = "ID TITLE AUTHOR\n";
                var list = webAPI.GetCategory(userInput);
                if(list != null)
                {
                    foreach (Book b in list)
                    {
                        
                        results += b.Id + ". " + b.Title + " " +  b.Author + "\n";                    
                    } 
                    return results;
                }
                else
                {
                    errorMsgOut = "Error loading books by category id";
                    Debug.WriteLine("Getcategory list by id was null");
                    return "";
                }
            }

        }

        internal bool SetUserActive(int userId, int setActiveUserId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.ActivateUser(userId, setActiveUserId))
                {
                    return true;
                }
                else
                {
                    errorMsg = "Could not activate user";
                    Debug.WriteLine("webapi ActivateUser() returned false");
                    return false;
                }
            }
        }

        internal bool SetUserInactive(int userId, int setDeActivateUserId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.InactivateUser(userId, setDeActivateUserId))
                {
                    return true;
                }
                else
                {
                    errorMsg = "Could not deactivate user";
                    Debug.WriteLine("webapi InactivateUser() returned false");
                    return false;
                }
            }
        }

        internal bool DemoteAUser(int userId, int demoteUserId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.Demote(userId, demoteUserId))
                {
                    return true;
                }
                else
                {
                    errorMsg = "User did not exist";
                    Debug.WriteLine("webapi Demote() returned false");
                    return false;
                }
            }
        }

        internal bool PromoteUser(int userId, int promoteUserId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.Promote(userId, promoteUserId))
                {
                    return true;
                }
                else
                {
                    errorMsg = "User did not exist";
                    Debug.WriteLine("webapi promote user returned false");
                    return false;
                }
            }
        }

        internal string GetBestCustomer(int userId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return "";
            }
            else
            {
                string results = "This is the best customer:\n";
                var user = webAPI.BestCustomer(userId);
                if(user != null)
                {
                    results += user.Name;
                    return results;
                }
                else
                {
                    errorMsg = "Could not get best customer";
                    Debug.WriteLine("webapi bestcustomer() returned null");
                    return "";
                }
            }
        }

        internal string GetMoneyEarned(int userId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return "";
            }
            else
            {
                string results = "Total Money earned by sold books:\n";
                int moneyEarned = webAPI.MoneyEarned(userId);
                results += moneyEarned.ToString();
                return results;
            }
        }

        internal string GetSoldItems(int userId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return "";
            }
            else
            {
                string results = "SOLD BOOKS:\n";
                var list = webAPI.SoldItems(userId);
                if(list != null)
                {
                    foreach(SoldBook b in list)
                    {
                        results += $"{b.Title} - by {b.Author}\n";
                    }
                    return results;
                }
                else
                {
                    errorMsg = "Could not add user";
                    Debug.WriteLine("webapi Adduser() returned false");
                    return "";
                }
            }
        }

        internal bool AddUser(int userId, string userName, string password, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.AddUser(userId, userName, password))
                {
                    return true;
                }
                else
                {
                    errorMsg = "Could not add user";
                    Debug.WriteLine("webapi Adduser() returned false");
                    return false;
                }
            }
        }

        /// <summary>
        /// Deletes category from database by parameter id input.
        /// Also checks if user has been inactive for over 15 minutes
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="catId"></param>
        /// <param name="userIsInactive"></param>
        /// <param name="errorMsg"></param>
        /// <returns>True if successful, else false. Also returns bool if userisactive and a error message</returns>
        internal bool DeleteCategor(int userId, int catId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.DeleteCategory(userId, catId))
                {
                    return true;
                }
                else
                {
                    errorMsg = "Could not delete category";
                    Debug.WriteLine("webapi deleteCategory() returned false");
                    return false;
                }
            }
        }

        internal bool UpdateCategory(int userId, int catId, string catName, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.UpdateCategory(userId, catId, catName))
                {
                    return true;
                }
                else
                {
                    errorMsg = "Could not update category";
                    Debug.WriteLine("webapi updateCategory() returned false");
                    return false;
                }
            }
        }

        internal bool AddBookToCategory(int userId, int bookId, int catId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.AddBookToCategory(userId, bookId, catId))
                {
                    return true;
                }
                else
                {
                    errorMsg = "Could not add book to category";
                    Debug.WriteLine("webapi addbooktocategory() returned false");
                    return false;
                }
            }
        }

        internal bool AddCategory(int userId, string catName, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.AddCategory(userId, catName))
                {
                    return true;
                }
                else
                {
                    errorMsg = "Could not add category";
                    Debug.WriteLine("webapi addcategory() returned false");
                    return false;
                }
            }         
        }

        internal bool DeleteBook(int userId, int bookId, out bool userIsInactive, out string errorMsg)
        {
            userIsInactive = false;
            errorMsg = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsg = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.DeleteBook(userId, bookId))
                {
                    return true;
                }
                else
                {
                    errorMsg = "Could not delete book";
                    Debug.WriteLine("webapi deletebook returned false");
                    return false;
                }
            }
        }

        internal bool UpdateBook(int userId, int bookId, string titleName, string authorName, int price, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.UpdateBook(userId, bookId, titleName, authorName, price))
                {
                    return true;
                }
                else
                {
                    errorMsgOut = "Could not update book";
                    Debug.WriteLine("UpdateBook from webapi returned false");
                    return false;
                }
            }
        }

        internal string GetUserByNameSearch(int userId, string userName, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return "";
            }
            else
            {
                string results = $"Listing users from name search of {userName}:\n";
                var list = webAPI.FindUser(userId, userName);
                if(list != null)
                {
                    foreach (User u in list)
                    {
                        string active = "No";
                        if(u.IsActive)
                        {
                            active = "Yes";
                        }

                        results += $"ID: {u.Id}\nUserName: {u.Name}\nActive: {active}\nLast login: {u.LastLogin.ToString()}\n";                       
                    } 
                    return results;
                }
                else
                {
                    errorMsgOut = "Error finding user";
                    Debug.WriteLine("List of users was null from database");
                    return string.Empty;
                }
            }
        }

        internal string GetAllUsers(int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return "";
            }
            else
            {
                string results = "All the users in the database:\n";
                var list = webAPI.ListUsers(userId);
                if(list != null)
                {
                    foreach (User u in list)
                    {
                        string active = "No";
                        if(u.IsActive)
                        {
                            active = "Yes";
                        }
                        results += $"ID: {u.Id} UserName: {u.Name}, Active: {active}, Is admin: {u.IsAdmin.ToString()}\n";
                        
                    } 
                    return results;
                }
                else
                {
                    errorMsgOut = "Error loading users";
                    Debug.WriteLine("List of users was null from database");
                    return string.Empty;
                }
            }
        }

        internal bool SetBookAmount(int userId, int bookId, int amount, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = string.Empty;

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.SetAmount(userId, bookId, amount))
                {
                    return true;
                }
                else
                {
                    errorMsgOut = "Could not change amount";
                    Debug.WriteLine("SetAmuount from webapi returned false");
                    return false;
                }
            }
        }

        internal bool AddABook(int userId, string title, string author, int price, int amount, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = string.Empty;

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return false;
            }
            else
            {
                if(webAPI.AddBook(userId, title, author, price, amount))
                {
                    return true;
                }
                else
                {
                    errorMsgOut = "Could not add book";
                    Debug.WriteLine("Addbook from webbshopapi returned false");
                    return false;
                }
            }
        }

        internal string GetCategories(int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            
            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return "";
            }
            else
            {
                string categories = "ID CATEGORY\n";
                var list = webAPI.GetCategories();
                if(list != null)
                {
                    foreach (BookCategory s in list)
                    {
                        
                        categories += s.Id + ". " + s.Category + "\n";
                        
                    } 
                    return categories;
                }
                else
                {
                    errorMsgOut = "Error loading categories";
                    Debug.WriteLine("Getcategories list was null");
                    return "";
                }
            }
        }
        
        /// <summary>
        /// Converting int? to int.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0 Or the value of Input</returns>
        private int ConvertToInteger(int? input)
        {
            return input.HasValue ? input.Value : 0;
        }

        /// <summary>
        /// Initializes a local timer to check if user has been active last 15 minutes. Work of progress
        /// </summary>
        private void SetTimer(int? userId)
        {
            //Converting from int? to int.
            int id = userId.HasValue ? userId.Value : 0;

            aTimer = new Timer(1000 * 60 * 15); // 1000 milisec * 60 * 15 = 15 minutes
            //TODO Clean up console writeline
            aTimer.Elapsed += (s, e) => webAPI.Ping(id);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        internal string GetBooksAvailableByCategoryId(int userInput, int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return "";
            }
            else
            {
                string results = "Results below\n";
                var list = webAPI.GetAvailableBooks(userInput);
                if(list != null)
                {
                    foreach (Book b in list)
                    {
                        results += $"Id: {b.Id}, Title: {b.Title}, Author: {b.Author}, Amount avaible: {b.Amount}\n";                                            
                    } 
                    return results;
                }
                else
                {
                    errorMsgOut = "Could not load books by cat id";
                    Debug.WriteLine("Get avaible books by cat id was null");
                    return "";
                }
            }
        }

        internal bool UserBuysBook(int userInput, int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return false;
            }
            else
            {   
                //Returns False if amount of book is 0. A public int GetAmount(int bookId){} of WebbShopAPI should be implemented.
                if(webAPI.BuyBook(userId, userInput))
                {
                    return true;
                }
                else
                {
                    errorMsgOut = "user did not buy book";
                    Debug.WriteLine("BuyBook from webbshopapi returned false");
                    return false;
                }
            }
        }

        internal string GetBooksByAuthorNameSearch(string userInput, int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return "";
            }
            else
            {
                string results = "Results below\n";
                var list = webAPI.GetAuthors(userInput);
                if(list != null)
                {
                    foreach(Book b in list)
                    {
                        results += $"ID: {b.Id}. Title: {b.Title}\n";  
                    }                    
                    return results;
                }
                else
                {
                    errorMsgOut = "Could not load books by author";
                    Debug.WriteLine("list of books were null");
                    return "";
                }
            }
        }

        internal string GetBookByName(string userInput, int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return "";
            }
            else
            {
                string results = "Results below\n";
                var list = webAPI.GetBooks(userInput);
                if(list != null)
                {
                    foreach(Book b in list)
                    {
                        results += $"ID: {b.Id}. Title: {b.Title}\n";  
                    }                    
                    return results;
                }
                else
                {
                    errorMsgOut = "Could not load list of books by name";
                    Debug.WriteLine("Book list was null");
                    return "";
                }
            }
        }

        internal string GetInfoOfBookById(int userInput, int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return "";
            }
            else
            {
                string results = "Results below\n";
                var book = webAPI.GetBook(userInput);
                if(book != null)
                {
                    if(book.Category != null)
                    {
                        results += $"Title: {book.Title}\nAuthor: {book.Author}\nPrice: {book.Price}\nAmount in stock: {book.Amount}\nCategory: {book.Category.Category}\n";                                          
                        return results;
                    }
                    else
                    {
                        results += $"Title: {book.Title}\nAuthor: {book.Author}\nPrice: {book.Price}\nAmount in stock: {book.Amount}\nCategory: No category added to book\n";
                        return results;
                    }
                }
                else
                {
                    errorMsgOut = "Could not load book by book id";
                    Debug.WriteLine("Book was null");
                    return "";
                }
            }
        }

        internal string GetAllBooks(int userId, out bool userIsInactive, out string errorMsgOut)
        {
            userIsInactive = false;
            errorMsgOut = "";

            if(!IsSessionActive(userId, out string errorMsgIn))
            {
                userIsInactive = true;
                errorMsgOut = errorMsgIn;
                return "";
            }
            else
            {
                string results = "Results below\n";
                var list = webAPI.GetAllBooks();
                if(list != null)
                {
                    foreach (Book b in list)
                    {
                        // results += $"Title:{b.Title}\nAuthor: {b.Author}\nPrice: {b.Price}\nAmount in stock: {b.Amount}\nCategory: {b.Category.Category}\n\n";  

                        results += $"ID: {b.Id}. Title: {b.Title}\n";                  
                    } 
                    return results;
                }
                else
                {
                    errorMsgOut = "Could not load All books";
                    Debug.WriteLine("List of books was null");
                    return "";
                }
            }
        }      
    }
}