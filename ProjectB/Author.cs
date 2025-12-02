using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    public class Author : Person
    {
        public string? Country;

        public string? Biography;

        public Author(string name, string surname, string? dateOfBirth = null, string? country = null, string? biography = null) : base(name, surname, dateOfBirth)
        {
            Country = country;
            Biography = biography;
        }

        public override string MakeInfoCard()
        {
            string txt = base.MakeInfoCard();

            txt += $"Country: {Country}\n" +
                $"Biography: {Biography}\n";

            return txt;
        }
        public override string GetRole()
        {
            return "Author";
        }
    }
}
