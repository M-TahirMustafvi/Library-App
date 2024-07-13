#pragma warning disable CS8602, CS8604
using System;

namespace LibraryDAL
{
    public class Transaction
    {
        private int _transactionId;
        private int _bookId;
        private int _borrowerId;
        private DateTime _date;
        private bool _isBorrowed;

        public int TransactionId
        {
            get { return _transactionId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Transaction ID must be a positive integer.");
                }
                _transactionId = value;
            }
        }

        public int BookID
        {
            get { return _bookId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Book ID must be a positive integer.");
                }
                _bookId = value;
            }
        }

        public int BorrowerId
        {
            get { return _borrowerId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Borrower ID must be a positive integer.");
                }
                _borrowerId = value;
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public bool IsBorrowed
        {
            get { return _isBorrowed; }
            set { _isBorrowed = value; }
        }


        public Transaction()
        {
            _transactionId = -1;
            _bookId = -1;
            _borrowerId = -1;
            _date = DateTime.MinValue;
            _isBorrowed = false;
        }


        /// <summary>
        /// If you can't understand this without documntaion, then....!
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return $"TransactionId: {TransactionId},  BorrowerId: {BorrowerId}, BookId: {BookID}, Date: {Date}, IsBorrowed: {IsBorrowed}";
        }


    }

}

