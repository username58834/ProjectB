using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text.Json.Serialization;

namespace ProjectB
{
    internal class Program
    {
        static Library library = new Library("Library");

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

        static void PrintColorMessage(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        static int EnterIntBetween(int minValue, int maxValue)
        {
            int result = minValue - 1;
            do
            {
                Console.WriteLine($"\nEnter a number between {minValue} and {maxValue}:");
                if(!int.TryParse(Console.ReadLine(), out result))
                {
                    PrintColorMessage("Please, enter an integer number", ConsoleColor.DarkRed);
                }
            } while (result > maxValue || result < minValue);
            return result;
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
        static Book AddBookMenu()
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

            string[] allAuthors = library.Authors
                .Select(x => x.Name + " " + x.Surname + $" (ID: {x.ID})")
                .ToArray();

            bool[] selectedAuthors = ChooseMenu(allAuthors, 1, 1, true, string.Join("", menu));

            for(int i=0;i<selectedAuthors.Length; i++)
            {
                Console.WriteLine($"i={i}, {allAuthors[i]}, selected = {selectedAuthors[i]}");
            }

            int authorIndex = allAuthors
                .Select((x, i) => i)
                .FirstOrDefault(i => selectedAuthors[i], -1);


            int authorID = -1;
            
            if (authorIndex != -1)
                 authorID = library.Authors
                    .Select((x, index) => x.ID)
                    .Skip(authorIndex)
                    .FirstOrDefault();


            Author? author = library.Authors
                .FirstOrDefault(x => x.ID == authorID, null);

            menu[3] += author == null ? "Unknown" : author.Name + " " + author.Surname;

            Console.Clear();
            for (int i = 0; i <= 3; i++) Console.Write(menu[i]);

            Book book = new Book(title, genres, author);

            PrintColorMessage("\n\nThe book was successfully added", ConsoleColor.DarkGreen);
            Console.WriteLine("Press any key to return back");

            Console.ReadKey();

            return book;
        }

        static Book? SelectBook()
        {
            string[] books = library.Books.Select(b => b.Title).ToArray();
            bool[] selected = ChooseMenu(books, 1, 1, true);

            Book? book = library.Books
                .Where((x, index) => selected[index])
                .FirstOrDefault();

            return book;
        }
        static void ChooseBookMenu()
        {
            Book? book = SelectBook();

            if (book == null) return;

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
                    ChooseBookMenu();
                    break;
                case 1:
                    break;
                default:
                    throw new UnknownCommandException();
            }
        }


        static Author? SelectAuthor()
        {
            string[] authors = library.Authors.Select(x => x.Name + " " + x.Surname + $" (ID: {x.ID})").ToArray();
            bool[] selected = ChooseMenu(authors, 1, 1, true);

            Author? author = library.Authors
                .Where((x, index) => selected[index])
                .FirstOrDefault();

            return author;
        }

        static void ChooseAuthorMenu()
        {
            Author? author = SelectAuthor();

            if (author == null) return;

            Console.Clear();
            Console.WriteLine(author.MakeInfoCard());

            Console.WriteLine("\nPress any key to return back");
            Console.ReadKey();
        }

        static void AuthorsMenu()
        {
            int arrow = 0;
            string[] menu =
            {
                "Choose Author",
                "Back"
            };

            do
            {
                DrawMenu(arrow, menu);
            } while (!MoveArrow(ref arrow, 0, menu.Length - 1));

            switch (arrow)
            {
                case 0:
                    ChooseAuthorMenu();
                    break;
                case 1:
                    break;
                default:
                    throw new UnknownCommandException();
            }
        }
        static void WorkerMenu(Worker worker)
        {
            int arrow = 0;
            string[] menu =
            {
                "Add Book",
                "Remove Book",
                "Back"
            };

            do
            {
                DrawMenu(arrow, menu);
            } while (!MoveArrow(ref arrow, 0, menu.Length - 1));

            switch (arrow)
            {
                case 0:
                    worker.AddBook(AddBookMenu(), library);
                    break;
                case 1:
                    Book? book = SelectBook();
                    if(book != null) worker.RemoveBook(book, library);
                    break;
                case 2:
                    break;
                default:
                    throw new UnknownCommandException();
            }
        }

        static Worker? SelectWorker()
        {
            string[] allWorkers = library.Workers.Select(w => w.Name + " " + w.Surname).ToArray();
            bool[] selectedWorkers = ChooseMenu(allWorkers, 1, 1, true, "Select a worker");

            int workerIndex = allWorkers
                .Select((x, i) => i)
                .FirstOrDefault(i => selectedWorkers[i], -1);

            int workerID = -1;

            if (workerIndex != -1)
                workerID = library.Workers

                   .Select((x, index) => x.ID)
                   .Skip(workerIndex)
                   .FirstOrDefault();

            Worker? worker = library.Workers
                .FirstOrDefault(x => x.ID == workerID, null);

            return worker;
        }


        static void MemberMenu(Member member)
        {
            int arrow = 0;
            string[] menu =
            {
                "Borrow Book",
                "Return Book",
                "Leave Review",
                "Back"
            };
            Worker? worker;
            Book? book;

            do
            {
                DrawMenu(arrow, menu);
            } while (!MoveArrow(ref arrow, 0, menu.Length - 1));

            switch (arrow)
            {
                case 0:
                    worker = SelectWorker();
                    book = SelectBook();
                    if (worker != null && book != null)
                    {
                        if(member.BorrowBook(book, worker))
                        {
                            PrintColorMessage("\n\nThe book was successfully borrowed", ConsoleColor.DarkGreen);
                            Console.WriteLine("Press any key to return back");

                            Console.ReadKey();
                        }
                        else
                        {
                            PrintColorMessage("\n\nUnfortunetly, the book is already taken", ConsoleColor.DarkRed);
                            Console.WriteLine("Press any key to return back");

                            Console.ReadKey();
                        }
                    }
                    break;
                case 1:
                    worker = SelectWorker();
                    book = SelectBook();
                    if (worker != null && book != null)
                    {
                        if (member.ReturnBook(book, worker))
                        {
                            PrintColorMessage("\n\nThe book was successfully returned", ConsoleColor.DarkGreen);
                            Console.WriteLine("Press any key to return back");

                            Console.ReadKey();
                        }
                        else
                        {
                            PrintColorMessage("\n\nThe book has already been returned", ConsoleColor.DarkRed);
                            Console.WriteLine("Press any key to return back");

                            Console.ReadKey();
                        }
                    }
                    break;
                case 2:
                    book = SelectBook();
                    if(book != null)
                    {
                        int rating = EnterIntBetween(1, 5);
                        member.LeaveReview(book, rating);

                        PrintColorMessage("\n\nThe review was successfully added", ConsoleColor.DarkGreen);
                        Console.WriteLine("Press any key to return back");

                        Console.ReadKey();
                    }
                    break;
                default:
                    throw new UnknownCommandException();
            }
        }

        static Member? SelectMember()
        {
            string[] allMembers = library.Members.Select(w => w.Name + " " + w.Surname).ToArray();
            bool[] selectedMembers = ChooseMenu(allMembers, 1, 1, true, "Select a member");

            int membersIndex = allMembers
                .Select((x, i) => i)
                .FirstOrDefault(i => selectedMembers[i], -1);

            int membersID = -1;

            if (membersIndex != -1)
                membersID = library.Members

                   .Select((x, index) => x.ID)
                   .Skip(membersIndex)
                   .FirstOrDefault();

            Member? member = library.Members
                .FirstOrDefault(x => x.ID == membersID, null);

            return member;
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
                    MainMenu();
                    break;
                case 1:
                    Member? member = SelectMember();
                    if (member != null) MemberMenu(member);
                    MainMenu();
                    break;
                case 2:
                    AuthorsMenu();
                    MainMenu();
                    break;
                case 3:
                    Worker? worker = SelectWorker();
                    if (worker != null) WorkerMenu(worker);
                    MainMenu();
                    break;
                case 4:
                    break;
                default:
                    throw new UnknownCommandException();
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            library.Books.Add(new Book("Harry Potter", new Genres[] { Genres.Adventure, Genres.Fantasy }));
            library.Books.Add(new Book("Tresure Island", new Genres[] { Genres.Adventure }));
            library.Books.Add(new Book("Sherlock Holmes", new Genres[] { Genres.Adventure, Genres.Detective }));

            library.Authors.Add(new Author("Diana", "Smith", "12.12.1990", "England"));
            library.Authors.Add(new Author("James", "Smith", "10.06.1886", "Canada"));

            library.Workers.Add(new Worker("John", "Smith"));
            library.Workers.Add(new Worker("Carl", "Smith"));

            library.Members.Add(new Member("Kate", "Smith"));
            library.Members.Add(new Member("Charles", "Smith"));

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
        }
    }
}
