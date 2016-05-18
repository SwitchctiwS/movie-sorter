using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSorter
{
    class Program
    {
        public enum ErrorCode { Success, NoFile };

        static List<Movie> movieList = new List<Movie>();
        private const string fileDir = @"C:\Users\LT_Ja\Documents\Movies.txt"; // Make a way to change this (config file or something)

        static void Main(string[] args)
        {
            StartUp();

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
                Console.WriteLine("0. Exit");

                // User's input
                string menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case ("1"): // View
                        ViewMovie();
                        break;
                    case ("2"): // Add
                        AddMovie();
                        break;
                    case ("3"): // Delete
                        DeleteMovie();
                        break;
                    case ("4"): // Edit
                        EditMovie();
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

        static void StartUp() // Change so that it creates directory and makes a movies list
        {
            if (!System.IO.File.Exists(fileDir))
            {
                Console.WriteLine("Movies.txt does not exist!");
                Console.WriteLine("Create? (Y/N)");
                if (YesOrNo("continue", "exit"))
                    CreateFile();
                else
                    Environment.Exit((int)ErrorCode.NoFile);
            }

            if (movieList.Count > 0)
                foreach (string movieTitle in System.IO.File.ReadLines(fileDir))
                    movieList.Add(new Movie(movieTitle));

            return;
        }

        static void CreateFile()
        {
            bool createFile = false;

            do
            {
                System.IO.File.Create(fileDir); // Create file

                if (!System.IO.File.Exists(fileDir)) // Check if file was created
                    ErrorHandler((int)ErrorCode.NoFile);
                else
                {
                    Console.WriteLine($"\nFile created at {fileDir}");
                    Console.WriteLine("Press any key to return to the menu...");
                    Console.ReadKey();
                    createFile = true; // Exit loop when file is created
                }
            } while (createFile == false);

            return;
        }

        static void ViewMovie()
        {
            Console.Clear();

            Console.WriteLine("Movie list:");
            Console.WriteLine();

            foreach (string movieTitle in System.IO.File.ReadLines(fileDir)) // Reads from file and displays
                Console.WriteLine(movieTitle);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        
        static void AddMovie()
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

                    string[] tempStringArr = new string[movieList.Count]; // Creates new array and sorts it. Changes this and make it efficient.
                    for (int i = 0; i < movieList.Count; i++)
                        tempStringArr[i] = movieList[i].Title;

                    movieList.Clear();

                    Array.Sort(tempStringArr);

                    foreach (string movieTitle in tempStringArr) // Put contents of array back into a list
                        movieList.Add(new Movie(movieTitle));

                    for (int i = 0; i < movieList.Count(); i++) // Removes empty spaces
                        if (movieList[i].Title == null)
                            movieList.RemoveAt(i);

                    System.IO.File.Delete(fileDir); // Deletes file then writes to it. Figure out how to update file instead of just deleting it.
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileDir, true))
                    {
                        for (int i = 0; i < movieList.Count; i++)
                            file.WriteLine(movieList[i].Title);
                    }

                    Console.WriteLine($"\nSuccessfully added {newMovie.Title}."); // Add error checking
                    Console.WriteLine("Add another movie? (Y/N)");

                    if (YesOrNo("continue", "exit"))
                        continue;
                    else
                        addMovie = true;
                }
                else
                {
                    Console.WriteLine($"\n {newMovie.Title} already exists.");
                    Console.WriteLine("Add a different movie? (Y/N)");

                    if (YesOrNo("continue", "exit"))
                        continue;
                    else
                        addMovie = true;
                }
            } while (addMovie == false);

            return;
        }

        static void DeleteMovie()
        {

        }

        static void EditMovie()
        {

        }

        static void Exit()
        {
            Console.Clear();

            Console.WriteLine("Goodbye");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit((int)ErrorCode.Success);
        }

        /*
         
        static void CreateMovieList() // Change this so it happens at startup and does NOT prompt for where to save!
        {
            string folderPath = CreateDir();
            CreateFile(folderPath);
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
        */

        static bool YesOrNo(string yes, string no) // Returns true if yes, false if no
        {
            string choice = Console.ReadLine();
            bool exit = false;

            do
            {
                if (choice == "y" || choice == "Y")
                {
                    Console.WriteLine($"\nPress any key to {yes}...");
                    Console.ReadKey();
                    exit = true;
                    return exit;
                }
                else if (choice == "n" || choice == "N")
                {
                    Console.WriteLine($"\nPress any key to {no}...");
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

        static void ErrorHandler(int errorName)
        {
            Console.WriteLine("\nThere was an error.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(errorName); // Terminates if false
        }
    }
}
