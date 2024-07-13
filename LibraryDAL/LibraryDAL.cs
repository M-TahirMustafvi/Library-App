#pragma warning disable CS8602, CS8604
using System;
using System.Text;


namespace LibraryDAL
{
    public class DAL
    {

        #region BookClassFunction
        //Adds book to the library.txt file firstly checks every possible edge case
        public void addBook(Book book)
        {
            if (book == null || book.BookID <= 0 || book.Title == "default" || book.Author == "default" || book.Genre == "default")
            {
                Console.WriteLine("Uncompelete or wrong information, book was'nt added in library");
                return;
            }
            else if (GetBookById(book.BookID) != null)
            {
                Console.WriteLine("Book is already there");
                return;
            }
            else
            {
                FileStream fin = new FileStream("Book.txt", FileMode.Append);
                StreamWriter stream = new StreamWriter(fin);
                string data = $"{book.BookID},{book.Title},{book.Author},{book.Genre},{book.IsAvaliable}";
                stream.WriteLine(data);
                stream.Close();
                fin.Close();
            }
            Console.WriteLine("Book was successfully added to library");
        }

        //Updates the book, by first searching on bases of book id, aslo handles every edge case
        public void updateBook(Book book)
        {
            if (book == null)
            {
                Console.WriteLine("Book cannot be null, wasn't updated!");
                return;
            }

            if (book.BookID <= 0 || book.Title == "default" || book.Author == "default" || book.Genre == "default")
            {
                Console.WriteLine("Uncompelete or wrong information, book was'nt updated in library");
                return;
            }

            StringBuilder result = new StringBuilder();
            bool bookFlag = false;

            FileStream fout = new FileStream("Book.txt", FileMode.Open);
            StreamReader stream = new StreamReader(fout);


            string data = "";
            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                string[] arr = data.Split(',');
                if (int.Parse(arr[0]) == book.BookID)
                {
                    bookFlag = true;
                    data = $"{book.BookID},{book.Title},{book.Author},{book.Genre},{book.IsAvaliable}";
                }
                result.AppendLine(data);
            }

            stream.Close();
            fout.Close();

            if (!bookFlag)
            {
                Console.WriteLine("No such book");
                return;
            }


            FileStream fin = new FileStream("Book.txt", FileMode.Create);
            StreamWriter wStream = new StreamWriter(fin);
            wStream.WriteLine(result);
            wStream.Close();
            fin.Close();
        }

        //Searches for the book in the file, and returns true if id matches else otherwise
        public Book GetBookById(int id)
        {
            FileStream fout = new FileStream("Book.txt", FileMode.OpenOrCreate);
            StreamReader stream = new StreamReader(fout);

            string data = "";
            bool bookFlag = false;
            string[] arr = { };

            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                arr = data.Split(',');
                if (int.Parse(arr[0]) == id)
                {
                    bookFlag = true;
                    break;
                }
            }

            stream.Close();
            fout.Close();

            if (!bookFlag)
            {
                Console.WriteLine("No such book in library");
                return null;
            }

            Book found = new Book();
            found.BookID = int.Parse(arr[0]);
            found.Title = arr[1];
            found.Author = arr[2];
            found.Genre = arr[3];
            found.IsAvaliable = bool.Parse(arr[4]);
            return found;
        }

        //Returns all books that match query, and returns List if found or empty string
        public List<Book> searchBooks(string query)
        {
            List<Book> books = new List<Book>();

            FileStream fout = new FileStream("Book.txt", FileMode.Open);
            StreamReader stream = new StreamReader(fout);

            string data = "";
            string[] arr = { };
            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                arr = data.Split(',');
                if (arr[1] == query || arr[2] == query || arr[3] == query)
                {
                    Book found = new Book();
                    found.BookID = int.Parse(arr[0]);
                    found.Title = arr[1];
                    found.Author = arr[2];
                    found.Genre = arr[3];
                    found.IsAvaliable = bool.Parse(arr[4]);

                    books.Add(found);
                }
            }

            stream.Close();
            fout.Close();
            return books;
        }

        //Returns all books
        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            FileStream fout = new FileStream("Book.txt", FileMode.Open);
            StreamReader stream = new StreamReader(fout);

            string data = "";
            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                string[] arr = data.Split(',');
                Book found = new Book();
                found.BookID = int.Parse(arr[0]);
                found.Title = arr[1];
                found.Author = arr[2];
                found.Genre = arr[3];
                found.IsAvaliable = bool.Parse(arr[4]);

                books.Add(found);
            }
            stream.Close();
            fout.Close();

            return books;
        }

        //Remove the book based on ID, the idea is to input all books in a string excluding the
        //book to be removed and then rewriting the string in the Book.txt
        public void removeBook(int _bookId)
        {
            FileStream fout = new FileStream("Book.txt", FileMode.Open);
            StreamReader stream = new StreamReader(fout);

            string data = "";
            StringBuilder result = new StringBuilder();
            bool bookFlag = false;
            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                string[] arr = data.Split(',');
                if (int.Parse(arr[0]) == _bookId)
                {
                    //Skips the string to be removed here
                    bookFlag = true;
                    continue;
                }
                result.AppendLine(data);
            }

            stream.Close();
            fout.Close();


            if (!bookFlag) { Console.WriteLine("No such book"); return; }

            //Updating File
            FileStream fin = new FileStream("Book.txt", FileMode.Create);
            StreamWriter wStream = new StreamWriter(fin);
            wStream.WriteLine(result);
            wStream.Close();
            fout.Close();
        }
        #endregion




        #region Borrower Class Function
        /// <summary>
        /// Register a new Borrower, checks if already in there, or email already in there
        /// </summary>
        /// <param name="borrower">Borrower object</param>
        public void registerBorrower(Borrower borrower)
        {
            if (borrower == null || borrower.BorrowerId == -1 || borrower.Name == "default" || borrower.Email == "default")
            {
                Console.WriteLine("Wrong or uncompelete info, borrower not registered ");
                return;
            }

            if (findBorrower(borrower.BorrowerId, borrower.Email))
            {
                Console.WriteLine("Registeration failed!");
                return;
            }

            FileStream fin = new FileStream("Borrowers.txt", FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fin);
            string data = $"{borrower.BorrowerId},{borrower.Name},{borrower.Email}";
            streamWriter.WriteLine(data);
            streamWriter.Close();
            fin.Close();
            Console.WriteLine("Borrower successfully added!");
        }

        /// <summary>
        /// Updates borrower, checks all possible edge cases
        /// </summary>
        /// <param name="borrower">Borrower object</param>
        public void updateBorrower(Borrower borrower)
        {
            if (borrower == null || borrower.BorrowerId == -1 || borrower.Name == "default" || borrower.Email == "default")
            {
                Console.WriteLine("Wrong or uncompelete info, borrower not registered ");
                return;
            }

            FileStream fout = new FileStream("Borrowers.txt", FileMode.Open);
            StreamReader stream = new StreamReader(fout);

            string data = "";
            StringBuilder result = new StringBuilder();
            bool borrowerFlag = false;

            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                string[] arr = data.Split(',');
                if (int.Parse(arr[0]) == borrower.BorrowerId)
                {
                    borrowerFlag = true;
                    data = $"{borrower.BorrowerId},{borrower.Name},{borrower.Email}";
                }
                result.AppendLine(data);
            }

            stream.Close();
            fout.Close();
            if (!borrowerFlag)
            {
                Console.WriteLine("No such Borrower");
                return;
            }

            FileStream fin = new FileStream("Borrowers.txt", FileMode.Create);
            StreamWriter wStream = new StreamWriter(fin);
            wStream.WriteLine(result);
            wStream.Close();
            fout.Close();
        }

        /// <summary>
        /// Deletes a borrower, uses the same idea as were used in delete book
        /// </summary>
        /// <param name="_bookId"> int book id</param>
        public void DeleteBorrower(int _bookId)
        {
            FileStream fout = new FileStream("Borrowers.txt", FileMode.Open);
            StreamReader stream = new StreamReader(fout);

            string data = "";
            StringBuilder result = new StringBuilder();
            bool borrowerFlag = false;

            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                string[] arr = data.Split(',');
                if (int.Parse(arr[0]) == _bookId)
                {
                    borrowerFlag = true;
                    continue;
                }
                result.AppendLine(data);
            }

            stream.Close();
            fout.Close();

            if (!borrowerFlag) { Console.WriteLine("No such Borrower"); return; }



            FileStream fin = new FileStream("Borrowers.txt", FileMode.Create);
            StreamWriter wStream = new StreamWriter(fin);
            wStream.WriteLine(result);
            wStream.Close();
            fout.Close();
        }


        /// <summary>
        /// Performs the transaxtion, writes to transaction file, also handles each edge case
        /// </summary>
        /// <param name="transaction">transaciton object</param>
        public void recordTransaction(Transaction transaction)
        {
            if (transaction == null || transaction.TransactionId == -1 || transaction.BookID == -1 || transaction.BorrowerId == -1)
            {
                Console.WriteLine("Wrong or uncompelete data!");
                return;
            }

            if (!findBorrower(transaction.BorrowerId))
            {
                Console.WriteLine("Please first register Borrower");
                return;
            }

            if (findBookAndTransact(transaction.BorrowerId, transaction.BookID, transaction.IsBorrowed))
            {
                FileStream fin = new FileStream("Transaction.txt", FileMode.Append);
                StreamWriter stream = new StreamWriter(fin);
                string data = $"{transaction.TransactionId},{transaction.BorrowerId},{transaction.BookID},{transaction.Date},{transaction.IsBorrowed}";
                stream.WriteLine(data);
                Console.WriteLine("Transaction compeleted Successfully");
                stream.Close();
                fin.Close();
            }
            else
                Console.WriteLine("Transaction was not compeleted due to an error");

        }
        #endregion





        #region Transaction Functions
        /// <summary>
        /// returns bookID's of all books that are borrowed by borrower of provided borrowerId till now ID, else returns empty list
        /// </summary>
        /// <param name="borrowerId">borrowerid</param>
        /// <returns>List of transaciton</returns>
        public List<Transaction> getBorrowedBookByBorrower(int borrowerId)
        {
            List<Transaction> transactions = new List<Transaction>();

            FileStream fout = new FileStream("Transaction.txt", FileMode.OpenOrCreate);
            StreamReader stream = new StreamReader(fout);

            string data = "";
            string[] arr = { };

            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                arr = data.Split(',');
                if (int.Parse(arr[1]) == borrowerId)
                {
                    Transaction found = new Transaction();
                    found.TransactionId = int.Parse(arr[0]);
                    found.BookID = int.Parse(arr[1]);
                    found.BorrowerId = int.Parse(arr[2]);
                    found.Date = DateTime.Parse(arr[3]);
                    found.IsBorrowed = bool.Parse(arr[4]);

                    transactions.Add(found);
                }
            }
            stream.Close();
            fout.Close();
            return transactions;
        }

        #endregion





        #region Helper Functions
        /// <summary>
        /// Finds borrower on bases of id and email
        /// Can send -1 in one of the parameter if One has to find on base of a single parameter
        /// </summary>
        /// <param name="id">id of borrower</param>
        /// <param name="email">email of borrower</param>
        /// <returns>Returns true if found, false otherWise!</returns>
        public bool findBorrower(int id, string email)
        {
            FileStream fout = new FileStream("Borrowers.txt", FileMode.OpenOrCreate);
            StreamReader stream = new StreamReader(fout);

            string data = "";
            string[] arr = { };

            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                arr = data.Split(',');
                if (int.Parse(arr[0]) == id)
                {
                    stream.Close();
                    fout.Close();
                    Console.WriteLine("Redundant ids are not allowed");
                    return true;

                }

                if (arr[2] == email)
                {
                    stream.Close();
                    fout.Close();
                    Console.WriteLine("Redundant email is not allowed");
                    return true;
                }
            }

            stream.Close();
            fout.Close();
            return false;

        }



        /// <summary>
        ///Helper function to find borrower on base of id
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns></returns>
        public bool findBorrower(int id)
        {
            FileStream fout = new FileStream("Borrowers.txt", FileMode.Open);
            StreamReader stream = new StreamReader(fout);


            string data = "";
            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                string[] arr = data.Split(',');
                if (int.Parse(arr[0]) == id)
                {
                    stream.Close();
                    fout.Close();
                    return true;
                }

            }


            stream.Close();
            fout.Close();
            return false;
        }

        /// <summary>
        /// Helper Funciton, Finds the book, performs transaction on base of argument issue, if issue == True, 
        /// issues the book, else returns it, updates book status, and returs true if successfull, else fasle
        /// Also check if the returning person has even issued book earlier or not, only allows to return the
        /// book if same person has issued it earlier and yet not returend
        /// </summary>
        /// <param name="id"> id of the book to be performed transaction on</param>
        /// <param name="issue">if issue == True, issues the book, else returns it</param>
        /// <param name="borrower">borrower who wants to issue or return a book</param>
        /// <returns>returns true if successfull, else fasle</returns>
        public bool findBookAndTransact(int borrower, int id, bool issue)
        {
            FileStream fout = new FileStream("Book.txt", FileMode.Open);
            StreamReader stream = new StreamReader(fout);
            bool found = false;

            string data = "";
            StringBuilder result = new StringBuilder();
            while ((data = stream.ReadLine()) != null)
            {
                if (data.Length <= 0) continue;

                string[] arr = data.Split(',');
                if (int.Parse(arr[0]) == id)
                {
                    found = true;
                    if (issue)
                    {
                        if (bool.Parse(arr[4]) == true)
                        {
                            data = $"{arr[0]},{arr[1]},{arr[2]},{arr[3]},{false}";
                            Console.WriteLine($"\n{data} Book issued successfully");
                            issue = true;
                        }
                        else
                        {
                            Console.WriteLine("\nBook is not avaliabe, i.e, already issued!");
                            stream.Close();
                            fout.Close();
                            return false;
                        }
                    }

                    else
                    {
                        if (bool.Parse(arr[4]) == false)
                        {
                            List<Transaction> transactions = new List<Transaction>(); //for checkin same person is returning the book
                            transactions = getBorrowedBookByBorrower(borrower);
                            bool bookStatus = false;
                            foreach (var trans in transactions)
                            {
                                if (trans.BookID == id)
                                    Console.WriteLine(trans.IsBorrowed);
                                bookStatus = trans.IsBorrowed;
                            }

                            if (!bookStatus)
                            {
                                Console.WriteLine("You havent issued this book so you cant return it");
                                stream.Close();
                                fout.Close();
                                return false;
                            }

                            data = $"{arr[0]},{arr[1]},{arr[2]},{arr[3]},{true}";
                            Console.WriteLine($"\n{data} Book returned successfully");
                            issue = false;
                        }
                        else
                        {
                            Console.WriteLine("\nThis book was not issued, wrong input!");
                            stream.Close();
                            fout.Close();
                            return false;
                        }
                    }
                }
                result.AppendLine(data);

            }
            stream.Close();
            fout.Close();


            FileStream fin = new FileStream("Book.txt", FileMode.Create);
            StreamWriter wStream = new StreamWriter(fin);
            wStream.WriteLine(result);
            wStream.Close();
            fin.Close();

            return false;
        }

        #endregion

    }
}
