#pragma warning disable CS8602, CS8604
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Transactions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


namespace LibraryDAL
{
    public class Book
    {
        private int _id;
        private string _title;
        private string _author;
        private string _genre;
        private bool _isAvaliable;

        public int ID
        {
            get { return _id; }
            set
            {
                if (value >= 0)
                    _id = value;
                else
                    throw new ArgumentException("ID must be a positvie integer");
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
            _title = "default";
            _author = "default";
            _genre = "default";
            _isAvaliable = true;
        }


        public override string ToString()
        {
            return $"Title: {Title}, Author: {Author}, Genre: {Genre}, Avaliability: {IsAvaliable}";
        }

    }
}
