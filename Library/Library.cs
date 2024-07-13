#pragma warning disable CS8602, CS8604
using LibraryDAL;

using System;
using System.Collections.Generic;
using System.IO;

namespace Interface
{
    class Library
    {

        static void Main()
        {
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
                            Menue.ViewAllBooks();
                            break;
                        case "11":
                            Menue.ViewBorrowedBooksByBorrower();
                            break;
                        case "12":
                            Console.WriteLine("Exiting...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice, Try again.");
                            break;
                    }
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }

            }
        }
    }
}