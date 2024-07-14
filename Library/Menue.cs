#pragma warning disable CS8602, CS8604, CS8601, CS8600
using LibraryDAL;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Menue
    {
        static DAL DAL = new DAL();

        #region Menu Function
        public static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome To The Library\n" +
                    "------------------------------------------------------------------------------------\n");
            Console.WriteLine("\nMenu Options:");
            Console.WriteLine("1. Add a new book");
            Console.WriteLine("2. Remove a book");
            Console.WriteLine("3. Update a book");
            Console.WriteLine("4. Register a new borrower");
            Console.WriteLine("5. Update a borrower");
            Console.WriteLine("6. Delete a borrower");
            Console.WriteLine("7. Borrow a book");
            Console.WriteLine("8. Return a book");
            Console.WriteLine("9. Search for books by title, author, or Genre");
            Console.WriteLine("10. View all books");
            Console.WriteLine("11. View borrowed books by a specific borrower");
            Console.WriteLine("12. Exit the application");
        }


        public static void AddNewBook()
        {
            Console.Clear();
            Book newBook = new Book();
            Console.Write("Input Book Details \n" +
                "-------------------------------------------------------------------------------------" +
                "\nBook Title :");
            newBook.Title = Console.ReadLine().Trim();
            Console.Write("Author :");
            newBook.Author = Console.ReadLine().Trim();
            Console.Write("Genre :");
            newBook.Genre = Console.ReadLine().Trim();
            DAL.addBook(newBook);
        }


        public static void RemoveBook()
        {
            Console.Clear();

            Console.Write("Book Deletion Menu \n" +
                "-------------------------------------------------------------------------------------" +
                "\n");

            Console.WriteLine("\nLibrary Catalogue:\n\n");
            ViewAllBooks();

            Console.Write("\nEnter Id of the book you want to delete: ");
            int id = int.Parse(Console.ReadLine().Trim());
            DAL.removeBook(id);
            Console.Write("\n\nPress any key to return to main menu...");
            Console.ReadKey(false);
        }


        public static void UpdateBook()
        {
            Console.Clear();

            Console.Write("Book Update Menu \n" +
                "-------------------------------------------------------------------------------------" +
                "\n");
            Console.WriteLine("\nLibrary Catalogue:\n\n");
            ViewAllBooks();

            
            Console.Write("\n\nEnter Id of the book you want to update: ");
            int id = int.Parse(Console.ReadLine().Trim());

            if (DAL.GetBookById(id) != null)
            {
                Book newBook = new Book();
                newBook.ID = id;
                Console.Write("Book Title :");
                newBook.Title = Console.ReadLine().Trim();
                Console.Write("Author :");
                newBook.Author = Console.ReadLine().Trim();
                Console.Write("Genre :");
                newBook.Genre = Console.ReadLine().Trim();
                DAL.updateBook(newBook);
            }
            else
                Console.WriteLine("No such book in the library");
            
            Console.Write("\n\nPress any key to return to main menu...");
            Console.ReadKey(false);
        }

        public static void RegisterNewBorrower()
        {
            Console.Clear();
            Console.Write("Borrower Registeration \n" +
           "-------------------------------------------------------------------------------------" +
                            "\n");
            
            Borrower newBorrower = new Borrower();
            Console.Write("Enter Name: ");
            newBorrower.Name = Console.ReadLine().Trim();
            Console.Write("Enter Email: ");
            newBorrower.Email = Console.ReadLine().Trim();
            DAL.registerBorrower(newBorrower);

            Console.WriteLine("Press any key to return to main menu");
            Console.ReadKey(false);
        }

        public static void UpdateBorrower()
        {
            Console.Clear();
            Console.Write("Update Borrower \n" +
           "-------------------------------------------------------------------------------------" +
                            "\n");
            Console.WriteLine("\nBorrowers Catalogue:\n\n");
            displayBorrowers();

            Borrower updatedBorrower = new Borrower();
            Console.Write("\n\nEnter Borrower ID to update: ");
            updatedBorrower.BorrowerId = int.Parse(Console.ReadLine().Trim());
            Console.Write("Enter new Name: ");
            updatedBorrower.Name = Console.ReadLine().Trim();
            Console.Write("Enter new Email: ");
            updatedBorrower.Email = Console.ReadLine().Trim();
            DAL.updateBorrower(updatedBorrower);

            Console.WriteLine("\n\nPress any key to return to main menu.....");
            Console.ReadKey(false);
        }

        public static void DeleteBorrower()
        {
            Console.Clear();
            Console.Write("Delete Borrower \n" +
           "-------------------------------------------------------------------------------------" +
                            "\n");
            Console.WriteLine("\nBorrowers Catalogue:\n\n");
            displayBorrowers();


            Console.Write("\n\nEnter Borrower ID to delete: ");
            int borrowerId = int.Parse(Console.ReadLine().Trim());
            DAL.DeleteBorrower(borrowerId);


            Console.WriteLine("\n\nPress any key to return to main menu.....");
            Console.ReadKey(false);
        }

        public static void BorrowBook()
        {
            Console.Clear();

            Console.Write("Book Borrow Menu \n" +
                "-------------------------------------------------------------------------------------" +
                "\n");
            Console.WriteLine("\nLibrary Catalogue:\n\n");
            ViewAllBooks();

            LibraryDAL.Transaction newTransaction = new LibraryDAL.Transaction();
            Console.Write("\n\nEnter ID of book you wanna Borrow: ");
            newTransaction.BookID = int.Parse(Console.ReadLine().Trim());
            Console.Write("Enter Borrower ID: ");
            newTransaction.BorrowerId = int.Parse(Console.ReadLine().Trim());
            newTransaction.Date = DateTime.Now;
            newTransaction.IsBorrowed = true;
            DAL.recordTransaction(newTransaction);

            Console.WriteLine("Press any key to return to main menu.....");
            Console.ReadKey(false);
        }

        public static void ReturnBook()
        {
            Console.Clear();
            Console.Write("Book Return Menu \n" +
                "-------------------------------------------------------------------------------------" +
                "\n");

            LibraryDAL.Transaction newTransaction = new LibraryDAL.Transaction();
            Console.Write("Enter Book ID: ");
            newTransaction.BookID = int.Parse(Console.ReadLine().Trim());
            Console.Write("Enter Borrower ID: ");
            newTransaction.BorrowerId = int.Parse(Console.ReadLine().Trim());
            newTransaction.Date = DateTime.Now;
            newTransaction.IsBorrowed = false;
            DAL.recordTransaction(newTransaction);

            Console.WriteLine("\nPress any key to return to main menu.....");
            Console.ReadKey(false);
        }

        public static void SearchBooks()
        {
            Console.Clear();
            Console.Write("Search Menu \n" +
                "-------------------------------------------------------------------------------------" +
                "\n");

            Console.Write("\n\nEnter search query (title, author, or Genre): ");
            string query = Console.ReadLine().Trim();
            List<Book> foundBooks = DAL.searchBooks(query);
            printBooks(foundBooks);

            Console.WriteLine("\nPress any key to continue....");
            Console.ReadKey(false);
        }

        public static void ViewAllBooks()
        {
            //Console.Clear();
            List<Book> foundBooks = DAL.GetAllBooks();
            
            if (foundBooks.Count > 0)
            {
                Console.WriteLine($"{"ID",-15} {"Title",-30} {"Author",-20} {"Genre",-15} {"Available",-10}");
                Console.WriteLine(new string('-', 100));

                // Print each book's details in tabular form
                foreach (var foundBook in foundBooks)
                {
                    Console.WriteLine($"{foundBook.ID,-15} {foundBook.Title,-30} {foundBook.Author,-20} {foundBook.Genre,-15} {foundBook.IsAvaliable,-10}");
                }
            }
            else
                Console.WriteLine("No books found!");

            //Console.Write("\n\nPress any key to return to main menu...");
            //Console.ReadKey(false);
        }

        public static void ViewBorrowedBooksByBorrower()
        {
            Console.Clear();
            Console.Write("Borrowers Account\n" +
                "-------------------------------------------------------------------------------------" +
                "\n");
            Console.Write("Enter Borrower ID to view borrowed books till today: ");
            int borrowerId = int.Parse(Console.ReadLine().Trim());
            List<LibraryDAL.Transaction> borrowedBooks = DAL.getBorrowedBookByBorrower(borrowerId);

            if (borrowedBooks.Count > 0)
            {
                Console.WriteLine("Borrowed books by Borrower ID {0}, are books with id :", borrowerId);
               foreach (var trans in borrowedBooks)
                    Console.WriteLine(trans.BookID);
            }
            else
                Console.WriteLine("No book borrowed by this borrower");


            Console.Write("\n\nPress any key to return to main menu...");
            Console.ReadKey(false);
        }

        #endregion



        #region Helper Functions

        static private void printBooks(List<Book> foundBooks)
        {
            if (foundBooks.Count > 0)
            {
                Console.WriteLine($"\n\n{"ID",-15} {"Title",-30} {"Author",-20} {"Genre",-15} {"Available",-10}");
                Console.WriteLine(new string('-', 100));

                // Print each book's details in tabular form
                foreach (var foundBook in foundBooks)
                {
                    Console.WriteLine($"{foundBook.ID,-15} {foundBook.Title,-30} {foundBook.Author,-20} {foundBook.Genre,-15} {foundBook.IsAvaliable,-10}");
                }
            }
            else
                Console.WriteLine("No books found!");

        }

        static private void displayBorrowers()
        {
            List<Borrower> foundBorrowers = DAL.GetAllBorrowers();

            if (foundBorrowers.Count > 0)
            {
                Console.WriteLine($"{"ID",-15} {"Name",-30} {"Email",-35}");
                Console.WriteLine(new string('-', 70));

                // Print each book's details in tabular form
                foreach (var found in foundBorrowers)
                {
                    Console.WriteLine($"{found.BorrowerId,-15} {found.Name,-30} {found.Email,-35} ");
                }
            }
            else
                Console.WriteLine("No books found!");
        }



        #endregion
    }

}
