#pragma warning disable CS8602, CS8604
using LibraryDAL;
using System;
using System.Collections.Generic;
using System.IO;

namespace Interface
{
    class App
    {
        static void Main()
        {

            #region Menu Control
            while (true)
            {
                
                Menue.ShowMenu();

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine().Trim();
                try
                {
                    switch (choice)
                    {
                        case "1":
                            Menue.AddNewBook();
                            break;
                        case "2":
                            Menue.RemoveBook();
                            break;
                        case "3":
                            Menue.UpdateBook();
                            break;
                        case "4":
                            Menue.RegisterNewBorrower();
                            break;
                        case "5":
                            Menue.UpdateBorrower();
                            break;
                        case "6":
                            Menue.DeleteBorrower();
                            break;
                        case "7":
                            Menue.BorrowBook();
                            break;
                        case "8":
                            Menue.ReturnBook();
                            break;
                        case "9":
                            Menue.SearchBooks();
                            break;
                        case "10":
                            Console.Clear();
                            Console.Write("Library Catalogue \n" +
             "-------------------------------------------------------------------------------------"                +"\n\n\n");  
                            Menue.ViewAllBooks();
                            Console.Write("\n\nPress any key to return to main menu...");
                            Console.ReadKey(false);
                            break;
                        case "11":
                            Menue.ViewBorrowedBooksByBorrower();
                            break;
                        case "12":
                            if (Confirm())
                                return;
                            else
                                break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid choice!");
                            Console.WriteLine("Press Any key to return Main Menu");
                            Console.ReadKey(false);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.Write("\nPress Any Key To Continue....");
                    Console.ReadKey();
                }

            }
            #endregion

        }


        #region Helper Functions
        static bool Confirm()
        {
            Console.Clear();
            while (true)
            {

                Char key;

                Console.Write("\nAre You Sure to exit (Y/N) :");
                key = Console.ReadKey().KeyChar;


                if (key == 'Y' || key == 'y')
                {
                    Console.WriteLine("\nExiting.......");
                    return true;
                }
                else if (key == 'N' || key == 'n')
                    return false;
                else
                {
                    Console.WriteLine("\nMake a definit choice");
                }
            }
        }
        #endregion
    }
}