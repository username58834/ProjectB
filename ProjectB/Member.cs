using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    public class Member : Person, IInfo
    {
        public DateTime MemebershipDate { get; set; }

        public Member(string name, string surname, string? dateOfBirth = null) : base(name, surname, dateOfBirth)
        {
            MemebershipDate = DateTime.Now;
        }
        public bool BorrowBook(Book book, Worker worker)
        {
            return worker.CheckOutBook(book);
        }

        public bool ReturnBook(Book book, Worker worker)
        {
            return worker.CheckInBook(book);
        }

        public bool LeaveReview(Book book, int rating)
        {
            if (book != null)
            {
                book.ReceiveRating(rating);
            }
            return false;
        }

        public override string MakeInfoCard()
        {
            string txt = base.MakeInfoCard();

            txt += $"Membership date: {MemebershipDate.ToString()}\n";

            return txt;
        }
        public override string GetRole()
        {
            return "Member";
        }

        public override string Info()
        {
            return "Member " + Name + " " + Surname;
        }
    }
}
