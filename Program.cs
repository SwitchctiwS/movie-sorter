using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieSorter
{
    class Program
    {
        public enum ErrorCode { Success, FileDoesNotExist, CouldNotDelete };

        static List<string> movieList = new List<string>(); // Global variable (good idea?) All methods alter this
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
                Console.WriteLine("0. Exit");

                // User's input
                string menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case ("1"): // View
                        ViewMovies();
                        Console.WriteLine("Press any key to return...");
                        Console.ReadKey();
                        break;
                    case ("2"): // Add
                        AddMovie();
                        break;
                    case ("3"): // Delete
                        DeleteMovie();
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

                ViewMovies();

                Console.WriteLine("Please enter the movie's title and press Enter.");
                Console.WriteLine("Note: No input returns to the menu.");
                string newMovie = Console.ReadLine();

                if (newMovie.Length == 0) // Returns to menu
                {
                    return;
                }
                if (!movieList.Exists(x => x == newMovie)) // Adds and sorts movie if it doesn't already exist
                {
                    movieList.Add(newMovie);
                    movieList.Sort();

                    System.IO.File.WriteAllLines(fileDir, movieList.ToArray());

                    ViewMovies();

                    Console.WriteLine($"Successfully added \"{newMovie}\"."); // Add error checking at some point

                    if (YesOrNo("Add another movie? (Y/N)"))
                    {
                        continue;
                    }
                    else
                    {
                        addMovie = true;
                    }
                }
                else
                {
                    Console.WriteLine($"\n\"{newMovie}\" already exists.");

                    if (YesOrNo("Add a different movie? (Y/N)"))
                    {
                        continue;
                    }
                    else
                    {
                        addMovie = true;
                    }
                }
            } while (addMovie == false);

            return;
        }

        static void DeleteMovie() // Figure out
        {
            bool deleteMovie = false;

            do
            {
                Console.Clear();

                ViewMovies();

                Console.WriteLine("Please enter the movie's title and press Enter.");
                Console.WriteLine("Note: No input returns to the menu.");
                string oldMovie = Console.ReadLine();

                if (oldMovie.Length == 0) // Returns to menu
                {
                    return;
                }
                if (movieList.Exists(x => x == oldMovie)) // Deletes and sorts movie list if it exists
                {
                    if (movieList.Remove(oldMovie))
                    {
                        movieList.Sort();

                        System.IO.File.WriteAllLines(fileDir, movieList.ToArray());

                        ViewMovies();

                        Console.WriteLine($"\nSuccessfully removed \"{oldMovie}\"."); // Add error checking at some point

                        if (YesOrNo("Delete another movie? (Y/N)"))
                        {
                            continue;
                        }
                        else
                        {
                            deleteMovie = true;
                        }
                    }
                    else
                    {
                        ErrorHandler(ErrorCode.CouldNotDelete);
                    }
                }
                else
                {
                    Console.WriteLine($"\n\"{oldMovie}\" does not exist.");

                    if (YesOrNo("Delete a different movie? (Y/N)"))
                    {
                        continue;
                    }
                    else
                    {
                        deleteMovie = true;
                    }
                }
            } while (deleteMovie == false);

            return;
        }

        static void Exit()
        {
            Console.WriteLine("\nGoodbye");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit((int)ErrorCode.Success);
        }

        static void ViewMovies()
        {
            Console.Clear();

            Console.WriteLine("Movie list:");
            Console.WriteLine();
            Console.WriteLine("========");

            if (movieList.Count() == 0)
            {
                Console.WriteLine("--Empty--");
            }
            else
            {
                foreach (string movieTitle in movieList) // Reads directly from file and displays movie title
                {
                    Console.WriteLine(movieTitle);
                }
            }

            Console.WriteLine("========");
            Console.WriteLine();

            return;
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
                {
                    ErrorHandler(ErrorCode.FileDoesNotExist);
                }
            } while (createFile == false);

            return;
        }

        static void ErrorHandler(ErrorCode errorName)
        {
            Console.Clear();

            Console.WriteLine("There was an error.");
            Console.WriteLine($"Errorcode: {errorName}");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit((int)errorName);
        }

        static void StartUp()
        {
            if (System.IO.File.Exists(fileDir))
            {
                foreach (string movieTitle in System.IO.File.ReadLines(fileDir))
                {
                    if (movieTitle.Length != 0) // Gets rid of blanks
                    {
                        movieList.Add(movieTitle);
                    }
                }
                movieList.Sort();
            }
            else
            {
                Console.WriteLine("Movies.txt does not exist!");

                if (YesOrNo("Create file? (Y/N)"))
                {
                    CreateFile();
                }
                else
                {
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadKey();
                    Environment.Exit((int)ErrorCode.FileDoesNotExist);
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
                {
                    exit = true;
                }
                else if (choice == "n" || choice == "N")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nThat was not a valid choice.");
                    continue;
                }
            } while (exit == false);

            return exit;
        }
    }
}