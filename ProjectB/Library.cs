using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    public class Library
    {
        public string Name;

        public List<Book> Books = new List<Book>();

        public List<Author> Authors = new List<Author>();

        public List<Member> Members = new List<Member>();

        public List<Worker> Workers = new List<Worker>();

        public Library(string name)
        {
            Name = name;
        }

        public bool HireWorker(Worker? worker)
        {
            if (worker != null)
            {
                Workers.Add(worker);
                return true;
            }
            return false;
        }

        public bool FireWorker(Worker? worker)
        {
            if (worker != null && Workers.Contains(worker))
            {
                Workers.Remove(worker);
                return true;
            }
            return false;
        }

        public bool AddMemeber(Member? member)
        {
            if (member != null)
            {
                Members.Add(member);
                return true;
            }
            return false;
        }

        public bool RemoveMemeber(Member? member)
        {
            if (member != null && Members.Contains(member))
            {
                Members.Remove(member);
                return true;
            }
            return false;
        }
    }
}
