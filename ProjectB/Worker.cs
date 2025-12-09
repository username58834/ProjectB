using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    public class Worker : Person, IInfo
    {
        public string? Position { get; set; }

        double salary = 0;
        public double Salary
        {
            get { return salary; }
            set
            {
                if (value >= 0) salary = value;
                else { throw new NegativeSalaryException(); }
            }
        }

        public Worker(string name, string surname, string? dateOfBirth = null, double salary = 0) : base(name, surname, dateOfBirth)
        {
            Salary = salary;
        }
        public bool CheckOutBook(Book book)
        {
            if(book != null && !book.IsCheckOut)
            {
                book.CheckOutDate = DateTime.Now;
                book.IsCheckOut = true;
                return true;
            }
            return false;
        }

        public bool CheckInBook(Book book)
        {
            if (book != null && book.IsCheckOut)
            {
                book.CheckOutDate = null;
                book.IsCheckOut = false;
                return true;
            }
            return false;
        }

        public bool AddBook(Book book, Library library)
        {
            if (book != null)
            {
                library.Books.Add(book);
                return true;
            }
            return false;
        }

        public bool RemoveBook(Book book, Library library)
        {
            if (book != null && library.Books.Contains(book))
            {
                library.Books.Remove(book);
                return true;
            }
            return false;
        }

        public bool AddAuthor(Author author, Library library)
        {
            if (author != null)
            {
                library.Authors.Add(author);
                return true;
            }
            return false;
        }

        public bool RemoveAurhor(Author author, Library library)
        {
            if (author != null && library.Authors.Contains(author))
            {
                library.Authors.Remove(author);
                return true;
            }
            return false;
        }


        public override string MakeInfoCard()
        {
            string txt = base.MakeInfoCard();

            txt += $"Position: {Position}\n" +
                $"Salary: {Salary}$\n";

            return txt;
        }

        public override string GetRole()
        {
            return "Worker";
        }

        public override string Info()
        {
            return "Worker " + Name + " " + Surname;
        }
    }
}
