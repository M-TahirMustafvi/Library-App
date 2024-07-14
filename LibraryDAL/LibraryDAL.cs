#pragma warning disable CS8602, CS8604
using System;
using System.Text;
using Microsoft.Data.SqlClient;

namespace LibraryDAL
{
    public class DAL
    {

        string conStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        #region Books Function
        public void addBook(Book book)
        {
            if (book == null || book.Title == "default" || book.Author == "default" || book.Genre == "default")
            {
                Console.WriteLine("Uncompelete or wrong information, book was'nt added in library");
                return;
            }

            else
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "Insert into Books(Title, Genre, Author) values(@t, @g, @a)";
                    con.Open();
                    SqlParameter param1 = new SqlParameter("@t", book.Title);
                    SqlParameter param2 = new SqlParameter("@g", book.Genre);
                    SqlParameter param3 = new SqlParameter("@a", book.Author);
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);
                        cmd.Parameters.Add(param3);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows < 1)
                            Console.WriteLine("Transaction Failed due to an error");
                        else
                            Console.WriteLine("Book Added Succesfully to library");
                    }

                }

            }
            Console.Write("Press Any key to return to mian menu :");
            Console.ReadKey(false);
        }




        public void updateBook(Book book)
        {
            if (book == null)
            {
                Console.WriteLine("Book cannot be null, wasn't updated!");
                return;
            }

            if (book.Title == "default" || book.Author == "default" || book.Genre == "default")
            {
                Console.WriteLine("Uncompelete or wrong information, book was'nt updated in library");
                return;
            }


            using (SqlConnection con = new SqlConnection(conStr))
            {

                string query = "update books set Title = @t, Author = @a, Genre = @g where id = @identity";
                con.Open();

                SqlParameter param1 = new SqlParameter("identity", book.ID);
                SqlParameter param2 = new SqlParameter("t", book.Title);
                SqlParameter param3 = new SqlParameter("a", book.Author);
                SqlParameter param4 = new SqlParameter("g", book.Genre);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(param1);
                    cmd.Parameters.Add(param2);
                    cmd.Parameters.Add(param3);
                    cmd.Parameters.Add(param4);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows < 1)
                        Console.WriteLine("Transaction Failed due to an error");
                    else
                        Console.WriteLine("Book Updated Succesfully in library");
                }


                Console.WriteLine("Press any key to return to main menu");
                Console.ReadKey(false);
            }
        }





        public Book GetBookById(int id)
        {

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "select * from Books where id = @identity";

                SqlParameter param = new SqlParameter("identity", id);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(param);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Book newBook = new Book();
                            newBook.ID = reader.GetInt32(0);
                            newBook.Title = reader.GetString(1);
                            newBook.Author = reader.GetString(2);
                            newBook.Genre = reader.GetString(3);
                            newBook.IsAvaliable = reader.GetBoolean(4);
                            return newBook;
                        }
                    }
                }
                return null;
            }
        }



        //Returns all books that match query, and returns List if found or empty string
        public List<Book> searchBooks(string query)
        {
            List<Book> books = new List<Book>();


            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query1 = "select * from Books where Title = @qu or Author = @qu or Genre = @qu";
                SqlParameter param = new SqlParameter("qu", query);

                using (SqlCommand cmd = new SqlCommand(query1, con))
                {
                    cmd.Parameters.Add(param);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book newBook = new Book();
                            newBook.ID = reader.GetInt32(0);
                            newBook.Title = reader.GetString(1);
                            newBook.Author = reader.GetString(2);
                            newBook.Genre = reader.GetString(3);
                            newBook.IsAvaliable = reader.GetBoolean(4);
                            books.Add(newBook);
                        }
                    }
                }
            }
            return books;
        }


        //Returns all books
        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();


            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "select * from Books";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book newBook = new Book();
                            newBook.ID = reader.GetInt32(0);
                            newBook.Title = reader.GetString(1);
                            newBook.Author = reader.GetString(2);
                            newBook.Genre = reader.GetString(3);
                            newBook.IsAvaliable = reader.GetBoolean(4);
                            books.Add(newBook);
                        }
                    }
                }
            }
            return books;
        }



        //Remove the book based on ID, the idea is to input all books in a string excluding the
        //book to be removed and then rewriting the string in the Book.txt
        public void removeBook(int _bookId)
        {
            if (GetBookById(_bookId) == null)
            {
                Console.WriteLine("No such book in the library");
                return;
            }

            else
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "Delete from Books where id = @id";
                    con.Open();
                    SqlParameter param1 = new SqlParameter("@id", _bookId);

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add(param1);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows < 1)
                            Console.WriteLine("Deletion Failed due to an error");
                        else
                            Console.WriteLine("Book Removed Succesfully from library");
                    }

                }

            }
        }
        #endregion




        #region Borrower Class Function


        /// <summary>
        /// Register a new Borrower, checks if already in there, or email already in there
        /// </summary>
        /// <param name = "borrower" > Borrower object</param>
        public void registerBorrower(Borrower borrower)
        {
            if (borrower == null || borrower.Name == "default" || borrower.Email == "default")
            {
                Console.WriteLine("Wrong or uncompelete info, borrower not registered ");
                return;
            }
            else
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {

                    string checkEmailQuery = "SELECT COUNT(*) FROM Borrowers WHERE Email = @Email";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(checkEmailQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", borrower.Email);

                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            Console.WriteLine("Email already in use by another borrower.");
                            return;
                        }

                    }

                    string query = "Insert into Borrowers(Name, Email) values(@n, @e)";
                    SqlParameter param1 = new SqlParameter("@n", borrower.Name);
                    SqlParameter param2 = new SqlParameter("@e", borrower.Email);

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows < 1)
                            Console.WriteLine("Transaction Failed due to an error");
                        else
                            Console.WriteLine("Borrower Added Succesfully to library");
                    }
                }
            }
        }



        /// <summary>
        /// Updates borrower, checks all possible edge cases
        /// </summary>
        /// <param name="borrower">Borrower object</param>
        public void updateBorrower(Borrower borrower)
        {

            if (borrower == null || borrower.Name == "default" || borrower.Email == "default")
            {
                Console.WriteLine("Uncompelete or wrong information, book was'nt updated in library");
                return;
            }


            using (SqlConnection con = new SqlConnection(conStr))
            {
                
                string checkEmailQuery = "SELECT COUNT(*) FROM Borrowers WHERE Email = @Email AND Id != @BorrowerId";
                con.Open();
                using (SqlCommand cmd = new SqlCommand(checkEmailQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Email", borrower.Email);
                    cmd.Parameters.AddWithValue("@BorrowerId", borrower.BorrowerId);

                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        Console.WriteLine("Email already in use by another borrower.");
                        return;
                    }

                }


                string query = "update borrowers set Name = @n, Email = @m where id = @identity";

                SqlParameter param1 = new SqlParameter("identity", borrower.BorrowerId);
                SqlParameter param2 = new SqlParameter("n", borrower.Name);
                SqlParameter param3 = new SqlParameter("m", borrower.Email);

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(param1);
                    cmd.Parameters.Add(param2);
                    cmd.Parameters.Add(param3);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows < 1)
                        Console.WriteLine("Transaction Failed, No Such Borrower found");
                    else
                        Console.WriteLine("Borrower Updated Succesfully in library");
                }

            }
        }



        /// <summary>
        /// Deletes a borrower, uses the same idea as were used in delete book
        /// </summary>
        /// <param name="_bookId"> int book id</param>
        public void DeleteBorrower(int _borrowerId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "Delete from Borrowers where id = @BId";
                con.Open();
                SqlParameter param1 = new SqlParameter("BId", _borrowerId);

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(param1);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows < 1)
                        Console.WriteLine("Deletion Failed, No such Borrower found");
                    else
                        Console.WriteLine("Borrower Removed Succesfully from library");
                }

            }
        }


        #endregion




        #region Transaction Functions

        /// <summary>
        /// Performs the transaxtion, writes to transaction file, also handles each edge case
        /// </summary>
        /// <param name="transaction">transaciton object</param>
        public void recordTransaction(Transaction transaction)
        {
            if (transaction == null || transaction.BookID == -1 || transaction.BorrowerId == -1)
            {
                Console.WriteLine("Wrong or uncompelete data, Transaction Failed!");
                return;
            }

            else
            {
                if (GetBookById(transaction.BookID) != null)
                {
                    if (transaction.IsBorrowed)
                    {
                        if (borrowBook(transaction))
                            Console.WriteLine("Book Borrowed Successfully");

                        else
                            Console.WriteLine("Book was already issued, Borrow failed!");
                    }

                    else
                    {
                        if (returnBook(transaction))
                        {
                            Console.WriteLine("Book Returend Successfully");

                            if (!WriteTransaction(transaction))
                                Console.WriteLine("There was an error recording transaction");
                            else
                                Console.WriteLine("Transaction recorded successfully");
                        }
                        else
                        {
                            Console.Write("Book was not borrowed by this borrower, Transaction failed!");
                        }
                    }
                }
                else
                    Console.WriteLine("No Such Book! Transaction Failed");

            }

        }




        /// <summary>
        /// returns bookID's of all books that are borrowed by borrower of provided borrowerId till now ID, else returns empty list
        /// </summary>
        /// <param name="borrowerId">borrowerid</param>
        /// <returns>List of transaciton</returns>
        public List<Transaction> getBorrowedBookByBorrower(int borrowerId)
        {
            List<Transaction> transactions = new List<Transaction>();

            using(SqlConnection con = new SqlConnection(conStr))
            {
                string query = "select * from [Transaction] where borrowerId = @borrower and isBorrowed = 1";
                SqlParameter param1 = new SqlParameter("borrower", borrowerId);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    cmd.Parameters.Add(param1);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction currTransact = new Transaction();
                            currTransact.TransactionId = reader.GetInt32(0);
                            currTransact.BorrowerId = reader.GetInt32(1);
                            currTransact.BookID = reader.GetInt32(2);
                            currTransact.Date = reader.GetDateTime(3);
                            currTransact.IsBorrowed = reader.GetBoolean(4);
                            transactions.Add(currTransact);
                        }
                    }
                }

            }
            return transactions;
        }

        #endregion




        #region Helper Functions

        private bool borrowBook(Transaction transaction)
        {
            bool retVal = false;
            Book newBook = GetBookById(transaction.BookID);

            if (newBook.IsAvaliable)
            {
                if(WriteTransaction(transaction))
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        string query = $"Update Books set isAvaliable = 0 " +
                                       $"where id = {newBook.ID}";
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            int rows = cmd.ExecuteNonQuery();
                            if (rows < 1)
                                retVal = false;
                            else
                                retVal = true;
                        }
                    }
                }
            }
            else
                retVal = false;

            return retVal;
        }

        private bool returnBook(Transaction transaction)
        {
            bool retVal = false;
            Book newBook = GetBookById(transaction.BookID);


            bool borrowed = false;
            List<LibraryDAL.Transaction> record = getBorrowedBookByBorrower(transaction.BorrowerId);
            foreach(var transact in  record)
            {
                if (transact.IsBorrowed && transact.BookID == transaction.BookID)
                    borrowed = true;
            }

            if (!newBook.IsAvaliable & borrowed)
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = $"Update Books set isAvaliable = 1 " +
                                   $"where id = {newBook.ID}";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        int rows = cmd.ExecuteNonQuery();
                        if (rows < 1)
                            retVal = false;
                        else
                            retVal = true;
                    }
                }

            }
            else
                retVal = false;

            return retVal;
        }

        private bool WriteTransaction(Transaction transaction)
        {
            bool retVal = false;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "INSERT INTO [Transaction] (BorrowerId, BookId, Date, isBorrowed) VALUES (@BorrowerId, @BookId, @Date, @IsBorrowed)";
                SqlParameter param1 = new SqlParameter("BorrowerId", transaction.BorrowerId);
                SqlParameter param2 = new SqlParameter("BookId", transaction.BookID);
                SqlParameter param3 = new SqlParameter("Date", DateTime.Now);
                SqlParameter param4 = new SqlParameter("IsBorrowed", transaction.IsBorrowed);

                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add(param1);
                    cmd.Parameters.Add(param2);
                    cmd.Parameters.Add(param3);
                    cmd.Parameters.Add(param4);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows < 1)
                        retVal = false;
                    else
                        retVal = true;
                }
            }
            return retVal;
        }

        public List<Borrower> GetAllBorrowers()
        {
            List<Borrower> borrowers = new List<Borrower>();


            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "select * from Borrowers";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Borrower newBorrower = new Borrower();
                            newBorrower.BorrowerId = reader.GetInt32(0);
                            newBorrower.Name = reader.GetString(1);
                            newBorrower.Email = reader.GetString(2);
                            borrowers.Add(newBorrower);
                        }
                    }
                }
            }
            return borrowers;
        }
        
        #endregion

    }
}


