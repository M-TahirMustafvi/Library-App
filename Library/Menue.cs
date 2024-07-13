using LibraryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Menue
    {
        public static void ShowMenu()
        {
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

        static DAL DAL = new DAL();

        private static int read()
        {
            FileStream fout = new FileStream("Counter.txt", FileMode.OpenOrCreate);
            StreamReader stream = new StreamReader(fout);
            string data = stream.ReadToEnd();
            stream.Close();
            fout.Close();
            if (string.IsNullOrEmpty(data))
                return 0;
            else
                return int.Parse(data);
        }

        private static int GenerateId()
        {
            int c = read();
            FileStream fin = new FileStream("Counter.txt", FileMode.Truncate);
            StreamWriter stream = new StreamWriter(fin);
            c++;
            stream.WriteLine(c);
            stream.Close();
            fin.Close();
            return c;
        }

        public static void AddNewBook()
        {
            
                Book newBook = new Book();
                newBook.BookID = GenerateId();
                Console.Write("Input Book Details \nBook Title :");
                newBook.Title = Console.ReadLine();
                Console.Write("Author :");
                newBook.Author = Console.ReadLine();
                Console.Write("Genre :");
                newBook.Genre = Console.ReadLine();
                Console.Write("Avaliability :");
                newBook.IsAvaliable = bool.Parse(Console.ReadLine());
                DAL.addBook(newBook);
        }

        public static void RemoveBook()
        {

                Console.Write("Enter Id of the book you want to delete: ");
                int id = int.Parse(Console.ReadLine());
                DAL.removeBook(id);
        }

        public static void UpdateBook()
        {
                Book newBook = new Book();
                Console.Write("Enter Id of the book you want to update: ");
                int id = int.Parse(Console.ReadLine());
                newBook.BookID = id;
                Console.Write("Book Title :");
                newBook.Title = Console.ReadLine();
                Console.Write("Author :");
                newBook.Author = Console.ReadLine();
                Console.Write("Genre :");
                newBook.Genre = Console.ReadLine();
                Console.Write("Avaliability :");
                newBook.IsAvaliable = bool.Parse(Console.ReadLine());

                DAL.updateBook(newBook);}

        public static void RegisterNewBorrower()
        {
                Borrower newBorrower = new Borrower();
                newBorrower.BorrowerId = GenerateId();
                Console.Write("Enter Name: ");
                newBorrower.Name = Console.ReadLine();
                Console.Write("Enter Email: ");
                newBorrower.Email = Console.ReadLine();
                DAL.registerBorrower(newBorrower);
        }

        public static void UpdateBorrower()
        {
                Borrower updatedBorrower = new Borrower();
                Console.Write("Enter Borrower ID to update: ");
                updatedBorrower.BorrowerId = int.Parse(Console.ReadLine());
                Console.Write("Enter new Name: ");
                updatedBorrower.Name = Console.ReadLine();
                Console.Write("Enter new Email: ");
                updatedBorrower.Email = Console.ReadLine();
                DAL.updateBorrower(updatedBorrower);
        }

        public static void DeleteBorrower()
        {
                Console.Write("Enter Borrower ID to delete: ");
                int borrowerId = int.Parse(Console.ReadLine());
                DAL.DeleteBorrower(borrowerId);
        }

        public static void BorrowBook()
        {
                LibraryDAL.Transaction newTransaction = new LibraryDAL.Transaction();
                newTransaction.TransactionId = GenerateId();
                Console.Write("Enter Book ID: ");
                newTransaction.BookID = int.Parse(Console.ReadLine());
                Console.Write("Enter Borrower ID: ");
                newTransaction.BorrowerId = int.Parse(Console.ReadLine());
                newTransaction.Date = DateTime.Now;
                newTransaction.IsBorrowed = true;
                DAL.recordTransaction(newTransaction);
        }

        public static void ReturnBook()
        {
                LibraryDAL.Transaction newTransaction = new LibraryDAL.Transaction();
                newTransaction.TransactionId = GenerateId();
                Console.Write("Enter Book ID: ");
                newTransaction.BookID = int.Parse(Console.ReadLine());
                Console.Write("Enter Borrower ID: ");
                newTransaction.BorrowerId = int.Parse(Console.ReadLine());
                newTransaction.Date = DateTime.Now;
                newTransaction.IsBorrowed = false;
                DAL.recordTransaction(newTransaction);
        }

        public static void SearchBooks()
        {
                Console.Write("Enter search query (title, author, or Genre): ");
                string query = Console.ReadLine();
                List<Book> foundBooks = DAL.searchBooks(query);

                if (foundBooks.Count > 0)
                {
                    Console.WriteLine("Books found:");
                    foreach (var foundBook in foundBooks)
                    {
                        Console.WriteLine($"ID: {foundBook.BookID}, Title: {foundBook.Title}, Author: {foundBook.Author}, Genre: {foundBook.Genre}, Available: {foundBook.IsAvaliable}");
                    }
                }
                else
                {
                    Console.WriteLine("No books found matching the query.");
                }
        }

        public static void ViewAllBooks()
        {
                List<Book> foundBooks = DAL.GetAllBooks();

                if (foundBooks.Count > 0)
                {
                    Console.WriteLine("Books found:");
                    foreach (var foundBook in foundBooks)
                    {
                        Console.WriteLine($"ID: {foundBook.BookID}, Title: {foundBook.Title}, Author: {foundBook.Author}, Genre: {foundBook.Genre}, Available: {foundBook.IsAvaliable}");
                    }
                }
                else
                {
                    Console.WriteLine("No books found!");
                }
        }

        public static void ViewBorrowedBooksByBorrower()
        {
                Console.Write("Enter Borrower ID to view borrowed books: ");
                int borrowerId = int.Parse(Console.ReadLine());
                List<LibraryDAL.Transaction> borrowedBooks = DAL.getBorrowedBookByBorrower(borrowerId);

                if (borrowedBooks.Count > 0)
                {
                    Console.WriteLine("Borrowed books by Borrower ID {0}, are books with id :", borrowerId);
                    foreach (var trans in borrowedBooks)
                    {
                        Console.WriteLine(trans.BookID);
                    }
                }
                else
                {
                    Console.WriteLine("No book borrowed");
                }
        }
    }

}
