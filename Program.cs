using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSorter
{
    class Program
    {
        public enum ErrorCode { Success, Folder, File, Unknown };

        static void Main(string[] args)
        {
            string fileDir = @"C:\Users\LT_Ja\Documents\Movies.txt";
            List<Movie> movieList = StartUp(fileDir);

            while (true)
            {
                Console.Clear();

                Console.WriteLine("What would you like to do?");
                Console.WriteLine("Type the number then press return.");
                Console.WriteLine();
                Console.WriteLine("1. View movie list");
                Console.WriteLine("2. Add to movie list");
                Console.WriteLine("3. Delete from movie list");
                Console.WriteLine("4. Edit movie list");
                Console.WriteLine("9. Create movie list");
                Console.WriteLine("0. Exit");

                // User's input
                string menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case ("1"): // View
                        ViewMovie(movieList);
                        break;
                    case ("2"): // Add
                        AddMovie(movieList, fileDir);
                        break;
                    case ("3"): // Delete
                        DeleteMovie(movieList);
                        break;
                    case ("4"): // Edit
                        EditMovie(movieList);
                        break;
                    case ("9"): // Create list
                        CreateMovieList();
                        break;
                    case ("0"): // Exit
                        Exit();
                        break; // Need syntatically(?)
                    default:
                        Console.WriteLine("\nThat was not a valid input.");
                        Console.WriteLine("Press any key to restart...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static List<Movie> StartUp(string fileDir)
        {
            List<Movie> movieList = new List<Movie>(1);

            foreach (string movieTitle in System.IO.File.ReadLines(fileDir))
                movieList.Add(new Movie(movieTitle));

            return movieList;
        }

        static void ViewMovie(List<Movie> movieList)
        {
            
        }
        
        static void AddMovie(List<Movie> movieList, string fileDir)
        {
            bool addMovie = false;

            do
            {
                Console.Clear();

                bool dupe = false;

                Console.WriteLine("Please enter the movie's title.");
                Movie newMovie = new Movie(Console.ReadLine());

                foreach (Movie movie in movieList) // Checks if there's a duplicate
                    if (movie.Title == newMovie.Title)
                    {
                        dupe = true;
                        break;
                    }

                if (dupe != true) // Adds movie if no dupe
                {
                    movieList.Add(newMovie);

                    string[] movies = new string[movieList.Count]; // Creates new array and sorts it. This method sucks.
                    for (int i = 0; i < movieList.Count; i++)
                        movies[i] = movieList[i].Title;

                    movieList.Clear();

                    Array.Sort(movies);

                    foreach (string movie in movies) // Put contents of array back into a list
                        movieList.Add(new Movie(movie));

                    for (int i = 0; i < movieList.Count(); i++) // Removes empty spaces
                        if (movieList[i].Title == null)
                            movieList.RemoveAt(i);

                    System.IO.File.Delete(fileDir); // Deletes file then writes to it. Figure out how to update file instead of just deleting it.
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileDir, true))
                    {
                        for (int i = 0; i < movieList.Count; i++)
                            file.WriteLine(movieList[i].Title);
                    }

                    Console.WriteLine($"\nSuccessfully added {newMovie.Title}.");
                    Console.WriteLine("Add another movie? (Y/N)");

                    if (YesOrNo())
                        continue;
                    else
                        addMovie = true;
                }
                else
                {
                    Console.WriteLine($"\n {newMovie.Title} already exists.");
                    Console.WriteLine("Add a different movie? (Y/N)");

                    if (YesOrNo())
                        continue;
                    else
                        addMovie = true;
                }
            } while (addMovie == false);

            return;
        }

        static void DeleteMovie(List<Movie> movieList)
        {

        }

        static void EditMovie(List<Movie> movieList)
        {

        }

        static void CreateMovieList() // Change this so it happens at startup and does NOT prompt for where to save!
        {
            string folderPath = CreateDir();
            CreateFile(folderPath);
        }

        static void Exit()
        {
            Console.Clear();

            Console.WriteLine("Goodbye");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit((int)ErrorCode.Success);
        }

        // Delete CreateDir and CreateFile
        static string CreateDir()
        {
            bool createDir = false;
            string folderPath = null;

            do
            {
                Console.Clear(); // Squeaky clean

                Console.WriteLine("It is recommeneded that the default values are used because I don't know how to create config files yet.");
                Console.WriteLine();
                Console.WriteLine("Where do you want to save the text file?"); // Prompt user for file's path
                Console.WriteLine("Note: Default (i.e. no path) is the Documents folder.");
                Console.WriteLine("Type 0 and press return to go back to the menu.");
                folderPath = Console.ReadLine(); // Name of path

                if (folderPath == "0")
                    Main(null);

                if (folderPath.Length == 0) // Set to default (Documents folder)
                {
                    folderPath = @"C:\Users\LT_Ja\Documents";
                    createDir = true;
                }
                else
                {
                    if (System.IO.Directory.Exists(folderPath)) // If directory already exists, ask user if they want to create a different directory
                    {
                        Console.WriteLine($"\nDirectory {folderPath} already exists.");
                        Console.WriteLine("Create a different directory? (Y/N)");

                        if (YesOrNo())
                            continue;
                        else
                            createDir = true;
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(folderPath); // Create Directory

                        if (!System.IO.Directory.Exists(folderPath)) // Check if directory was created
                        {
                            Console.WriteLine("\nThere was an error.");
                            Console.WriteLine("Press any key to exit...");
                            Console.ReadKey();
                            Environment.Exit((int)ErrorCode.Folder); // Terminates if false
                        }
                        else
                        {
                            Console.WriteLine($"\nFolder created at {folderPath}");
                            Console.WriteLine("Press any key to return to the menu...");
                            Console.ReadKey();
                            createDir = true; // Exit loop when folder is created
                        }
                    }
                }
            } while (createDir == false);

            return folderPath;
        }

        static void CreateFile(string folderPath)
        {
            bool createFile = false;
            string fileName = null;

            do
            {
                Console.Clear(); // Squeaky clean

                Console.WriteLine("What do you want the text file to be called?"); // Prompt user for name of file
                Console.WriteLine("Note: Default (i.e. no filename) is \"Movies.txt\".");
                Console.WriteLine("Type 0 and press return to go back to the menu.");
                fileName = Console.ReadLine(); // Name of text file

                if (fileName == "0")
                    Main(null);

                if (fileName.Length == 0)
                    fileName = "Movies"; // Default

                fileName += ".txt"; // Add file type

                string filePath = System.IO.Path.Combine(folderPath, fileName);

                if (!System.IO.File.Exists(filePath)) // Checks if the file exists
                {
                    System.IO.File.Create(filePath); // Create file

                    if (!System.IO.File.Exists(filePath)) // Check if file was created
                    {
                        Console.WriteLine("\nThere was an error.");
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        Environment.Exit((int)ErrorCode.File); // Terminates if false
                    }
                    else
                    {
                        Console.WriteLine($"\nFile created at {filePath}");
                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                        createFile = true; // Exit loop when file is created
                    }
                }
                else
                {
                    Console.WriteLine($"\nFile {fileName} already exists at {folderPath}."); // Prompts user to create a different file if file already exists
                    Console.WriteLine("Create a different file? (Y/N)");

                    if (YesOrNo())
                        continue;
                    else
                        Main(null);
                }
            } while (createFile == false);

            return;
        }

        static bool YesOrNo() // Returns true if yes, false if no
        {
            string choice = Console.ReadLine();
            bool exit = false;

            do
            {
                if (choice == "y" || choice == "Y")
                {
                    Console.WriteLine("\nPress any key to restart...");
                    Console.ReadKey();
                    exit = true;
                    return exit;
                }
                else if (choice == "n" || choice == "N")
                {
                    Console.WriteLine("\nPress any key to go back to the menu...");
                    Console.ReadKey();
                    return exit;
                }
                else
                {
                    Console.WriteLine("\nThat was not a valid choice.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }
            } while (exit == false);

            return exit;
        }
    }
}
