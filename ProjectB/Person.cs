using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    public abstract class Person: IInfo
    {
        static int counter  = 0;
        
        public int ID;
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Person(string name, string surname, string? dateOfBirth = null)
        {
            counter++;
            ID = counter;
            Name = name;
            Surname = surname;

            DateTime date;
            if(dateOfBirth != null && DateTime.TryParse(dateOfBirth, out date))
            {
                DateOfBirth = date;
            }
            else
            {
                DateOfBirth = null;
            }
        }

        public abstract string GetRole(); 

        public virtual string MakeInfoCard()
        {
            string txt = "";

            txt = $"ID: {ID}\n" +
                $"Name: {Name}\n" +
                $"Surname: {Surname}\n" +
                $"Date of birth: {DateOfBirth.ToString()}\n";

            return txt;
        }

        public virtual string Info()
        {
            return "Person " + Name + " " + Surname;
        }
    }
}
