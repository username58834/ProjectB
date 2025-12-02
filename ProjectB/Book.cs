using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    public class Book : IInfo, ICloneable
    {
        static public int MaxLoanDays = 7;

        public string Title;

        public List<Genres> Genre = new List<Genres>();
        public DateTime? PublicationDate { get; set; }

        double rating = 0;
        int countRating = 0;
        public double Rating
        {
            get
            {
                return Math.Round(rating / countRating, 2);
            }
        }        

        public bool IsCheckOut;

        public Author Author;

        public DateTime? CheckOutDate = null;

        public Book(string title, Genres[] genres, Author author, string? published = null)
        {
            Title = title;

            for (int i = 0; i < genres.Length; i++)
            {
                Genre.Add(genres[i]);
            }

            Author = author;

            DateTime date;
            if (PublicationDate != null && DateTime.TryParse(published, out date))
            {
                PublicationDate = date;
            }
            else
            {
                PublicationDate = null;
            }
        }

        public void ReceiveRating(int rating)
        {
            countRating++;
            this.rating += rating;
        }
        public bool CheckIfWasEpired()
        {
            if (IsCheckOut && CheckOutDate != null) {
                DateTime checkOutDate = CheckOutDate.Value;
                DateTime date = DateTime.Now;

                return ((checkOutDate - date).TotalDays > MaxLoanDays);
            }
            return false;
        }

        public string Info()
        {
            return Title;
        }

        public object Clone()
        {
            Book clone = new Book(Title, new List<Genres>(this.Genre).ToArray(), Author, this.PublicationDate?.ToString("yyyy-MM-dd"));
            clone.rating = this.rating;
            clone.countRating = this.countRating;

            return clone;
        }
    }
}
