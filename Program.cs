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
                Console.WriteLine("Type the number then press enter.");
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
                        ViewMovies();
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

        // Note: Methods sorted in alpha order
        // Core methods
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
                        if (movieList[i].Title != null) // Removes empty spaces
                            tempStringArr[i] = movieList[i].Title;

                    movieList.Clear();

                    Array.Sort(tempStringArr);

                    System.IO.File.WriteAllLines(fileDir, tempStringArr);

                    foreach (string movieTitle in tempStringArr) // Put contents of array back into a list
                        movieList.Add(new Movie(movieTitle));

                    Console.WriteLine($"\nSuccessfully added \"{newMovie.Title}\"."); // Add error checking at some point

                    if (YesOrNo("Add another movie? (Y/N)"))
                        continue;
                    else
                        addMovie = true;
                }
                else
                {
                    Console.WriteLine($"\n\"{newMovie.Title}\" already exists.");

                    if (YesOrNo("Add a different movie? (Y/N)"))
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

        static void ViewMovies()
        {
            Console.Clear();

            if (System.IO.File.Exists(fileDir))
            {
                Console.WriteLine("Movie list:");
                Console.WriteLine();
                Console.WriteLine("========");

                if (movieList.Count() == 0)
                    Console.WriteLine("--Empty--");
                else
                    foreach (string movieTitle in System.IO.File.ReadLines(fileDir)) // Reads from file and displays movie title
                        Console.WriteLine(movieTitle);

                Console.WriteLine("========");
                Console.WriteLine();
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
            }
            else
                ErrorHandler(ErrorCode.NoFile);
        }

        // Side methods
        static void CreateFile()
        {
            bool createFile = false;

            do
            {
                System.IO.File.Create(fileDir).Close(); // Create and close file to allow editing later

                if (System.IO.File.Exists(fileDir)) // Check if file was created
                {
                    Console.Clear();

                    Console.WriteLine($"File created at {fileDir}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    createFile = true; // Exit loop when file is created
                }
                else
                    ErrorHandler(ErrorCode.NoFile);
            } while (createFile == false);

            return;
        }

        static void ErrorHandler(ErrorCode errorName)
        {
            Console.Clear();

            Console.WriteLine("\nThere was an error.");
            Console.WriteLine($"Errorcode: {errorName}");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit((int)errorName);
        }

        static void StartUp()
        {
            if (System.IO.File.Exists(fileDir))
                foreach (string movieTitle in System.IO.File.ReadLines(fileDir))
                    movieList.Add(new Movie(movieTitle));
            else
            {
                Console.WriteLine("Movies.txt does not exist!");

                if (YesOrNo("Create file? (Y/N)"))
                    CreateFile();
                else
                {
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadKey();
                    Environment.Exit((int)ErrorCode.NoFile);
                }
            }

            return;
        }

        static bool YesOrNo(string prompt) // Returns true if yes, false if no
        {
            bool exit = false;

            do
            {
                Console.WriteLine(prompt);

                string choice = Console.ReadLine();

                if (choice == "y" || choice == "Y")
                    exit = true;
                else if (choice == "n" || choice == "N")
                    break;
                else
                {
                    Console.WriteLine("\nThat was not a valid choice.");
                    Console.WriteLine("Press any key to restart...");
                    Console.ReadKey();

                    Console.Clear();

                    continue;
                }
            } while (exit == false);

            return exit;
        }
    }
}