using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text.Json.Serialization;

namespace ProjectB
{
    internal class Program
    {
        static List<Person> Persons = new();
        static List<Book> Books = new();
        static bool MoveArrow(ref int arrow, int min, int max)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter) return true;
            else if (key.Key == ConsoleKey.UpArrow) arrow--;
            else if (key.Key == ConsoleKey.DownArrow) arrow++;

            if (arrow < min) arrow = max;
            if (arrow > max) arrow = min;

            return false;
        }

        static void PrintGreenMessage(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        static void DrawMenu(int arrow, string[] menu, bool[]? selected = null, string? addbefore = null)
        {
            Console.Clear();

            if(addbefore != null) Console.WriteLine(addbefore);

            for (int i = 0; i < menu.Length; i++)
            {
                if (i == arrow)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("->");                    
                }
                else if(selected != null && i < selected.Length && selected[i])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" +");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("  ");
                }

                Console.WriteLine(menu[i]);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static bool[] ChooseMenu(string[] menu, int minitems, int maxitems, bool addBack = false, string? addbefore = null)
        {
            int arrow = 0;
            bool clicked = false;
            bool[] selected = new bool[menu.Length];
            Array.Fill(selected, false);

            bool chooseOne = (minitems == maxitems && maxitems == 1);

            if (!chooseOne)
            {
                Array.Resize(ref menu, menu.Length + 1);
                menu[menu.Length - 1] = "Done!";
            }
            if(addBack)
            {
                Array.Resize(ref menu, menu.Length + 1);
                menu[menu.Length - 1] = "Back";
            }

            do
            {
                DrawMenu(arrow, menu, selected, addbefore);
                clicked = MoveArrow(ref arrow, 0, menu.Length - 1);
                if (clicked && arrow < selected.Length)
                {
                    selected[arrow] ^= true;
                }
            } while (!clicked || (arrow != menu.Length - 1 && !chooseOne));

            return selected;
        }
        static void AddBookMenu()
        {
            Console.Clear();

            string[] menu =
            {
                "-Adding A New Book-\n",
                "Book`s Title: ",
                "Genre:",
                "\nAuthor: "
            };

            for (int i = 0; i <= 1; i++) Console.Write(menu[i]);
            
            string title = Console.ReadLine();
            menu[1] += title + "\n";
            
            Console.Write(menu[2]);

            string[] allGenres = Enum.GetNames(typeof(Genres));
            bool[] selectedGenres = ChooseMenu(allGenres, 1, allGenres.Length, false, string.Join("", menu.SkipLast(1)));

            Genres[] genres = allGenres
                .Where((x, index) => selectedGenres[index])
                .Select(x => (Genres)Enum.Parse(typeof(Genres), x))
                .ToArray();

            menu[2] += "\n  " + string.Join("\n  ", allGenres
                .Where((x, index) => selectedGenres[index])
                .Select(x => (Genres)Enum.Parse(typeof(Genres), x))
                .ToArray()
                );

            string[] allAuthors = Persons
                .Where(x => x is Author)
                .Select(x => x.Name + " " + x.Surname + $" (ID: {x.ID})")
                .ToArray();

            bool[] selectedAuthors = ChooseMenu(allAuthors, 1, 1, true, string.Join("", menu));

            int authorIndex = allAuthors
                .Where((x, index) => selectedAuthors[index])
                .Select((x, index) => index)
                .FirstOrDefault(-1);
            //Console.WriteLine("authorIndex = ", authorIndex);


            int authorID = -1;
            
            if (authorIndex != -1)
                 authorID = Persons
                    .Where(x => x is Author)
                    .Select((x, index) => x.ID)
                    .Skip(authorIndex - 1)
                    .FirstOrDefault();

            //throw new Exception($"authorIndex = {authorIndex}");
 
            Author? author = Persons
                .FirstOrDefault(x => x is Author && x.ID == authorID, null) as Author;

            menu[3] += author == null ? "Unknown" : author.Name + " " + author.Surname;

            Console.Clear();
            for (int i = 0; i <= 3; i++) Console.Write(menu[i]);

            Book book = new Book(title, genres, author);
            Books.Add(book);

            PrintGreenMessage("\n\nThe book was successfully added");
            Console.WriteLine("Press any key to return back");

            Console.ReadKey();
        }

        static void ChooseBookMenu()
        {
            string[] books = Books.Select(b => b.Title).ToArray();
            bool[] selected = ChooseMenu(books, 1, 1);

            Book book = Books
                .Where((x, index) => selected[index])
                .First();

            Console.Clear();
            Console.WriteLine(book.Info());

            Console.WriteLine("\nPress any key to return back");
            Console.ReadKey();
        }       

        static void BooksMenu()
        {
            int arrow = 0;
            string[] menu =
            {
                "Add Book",
                "Choose Book",
                "Back"
            };

            do
            {
                DrawMenu(arrow, menu);
            } while (!MoveArrow(ref arrow, 0, menu.Length - 1));

            switch (arrow)
            {
                case 0:
                    AddBookMenu();
                    MainMenu();
                    break;
                case 1:
                    ChooseBookMenu();
                    MainMenu();
                    break;
                case 2:
                    MainMenu();
                    break;
                default:
                    throw new UnknownCommandException();
            }
        }
        static void MainMenu()
        {            
            int arrow = 0;
            string[] menu =
            {
                "Books",
                "Members",
                "Authors",
                "Workers",
                "Exit"
            };

            do
            {                
                DrawMenu(arrow, menu);
            } while (!MoveArrow(ref arrow, 0, menu.Length - 1));

            switch (arrow)
            {
                case 0:
                    BooksMenu();
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    throw new UnknownCommandException();
            }
        }

        static void ColorGrid()//Generated by ChatGPT. Must be removed
        {
            Console.WriteLine("Foreground vs Background color grid:\n");

            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));

            foreach (var bg in colors)
            {
                foreach (var fg in colors)
                {
                    Console.BackgroundColor = bg;
                    Console.ForegroundColor = fg;

                    Console.Write($" {fg.ToString().PadRight(12)} ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }
        static void Main(string[] args)
        {
            Books.Add(new Book("HarryPotter", new Genres[] { Genres.Adventure, Genres.Fantasy }));
            Books.Add(new Book("TresureIsland", new Genres[] { Genres.Adventure }));
            Books.Add(new Book("Holmes", new Genres[] { Genres.Adventure, Genres.Detective }));

            Persons.Add(new Author("Diana", "Smith"));
            try
            {
                MainMenu();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            //ColorGrid();

            /*
            
            people.Add(new Member("Ann", "Smith", "02/12/2025"));
            people.Add(new Worker("Jane", "Smith", "01/11/2025"));
            people.Add(new Author("Author", "Smith", "02/12/2025"));

            foreach (Person p in people)
            {
                Console.WriteLine(p.MakeInfoCard());
                Console.WriteLine(p.Info());
                Console.WriteLine();
            }
            */
        }
    }
}
