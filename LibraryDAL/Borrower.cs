#pragma warning disable CS8602, CS8604
using System;

namespace LibraryDAL
{
    public class Borrower
    {
        private int _borrowerId;
        private string _name;
        private string _email;

        public int BorrowerId
        {
            get { return _borrowerId; }
            set
            {
                if (value > 0)
                {
                    _borrowerId = value;
                }
                else
                {
                    throw new ArgumentException("Borrower ID must be a positive integer.");
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentException("Name cannot be empty.");
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Contains("@"))
                {
                    _email = value;
                }
                else
                {
                    throw new ArgumentException("Invalid email address.");
                }
            }
        }
       
        public Borrower()
        {
            _borrowerId = -1;
            _name = "default";
            _email = "default";
        }

    }
}
