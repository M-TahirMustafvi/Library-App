#pragma warning disable CS8602, CS8604
using System;


namespace LibraryDAL
{
    public class Book
    {
        private int _bookId;
        private string _title;
        private string _author;
        private string _genre;
        private bool _isAvaliable;

        public int BookID
        {
            get { return _bookId; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Book ID must be a positive integer.");
                _bookId = value;
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Title cannot be null or empty.");
                _title = value;
            }
        }

        public string Author
        {
            get { return _author; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Author cannot be null or empty");
                _author = value;
            }
        }

        public string Genre
        {
            get { return _genre; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Genre cannot be null or empty");
                _genre = value;
            }
        }

        public bool IsAvaliable
        {
            get { return _isAvaliable; }
            set { _isAvaliable = value; }
        }


        public Book()
        {
            _bookId = -1;
            _title = "default";
            _author = "default";
            _genre = "default";
            _isAvaliable = true;
        }

    }
}
