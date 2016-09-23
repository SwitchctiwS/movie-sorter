using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieSorter {
    class Program {
        public enum ErrorCode { Success, FileDoesNotExist, CouldNotDelete };

        static List<string> movieList = new List<string>(); // Global variable (good idea?) All methods alter this
        private const string fileDir = @"/media/jared_thibault/SHARED/Movies.txt"; // Make a way to change this (config file or something)

        // Acts as a menu
        static void Main(string[] args) {
            StartUp();

            while(true) {
                Console.Clear();

                //Menu
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("Type the number then press enter.");
                Console.WriteLine();
                Console.WriteLine("1. View movie list");
                Console.WriteLine("2. Add to movie list");
                Console.WriteLine("3. Delete from movie list");
                Console.WriteLine("0. Exit");

                string menuChoice = Console.ReadLine(); // User's input

                switch (menuChoice) {
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

        /// <summary>
        /// Adds movie to list and updates file.
        /// </summary>
        static void AddMovie() {
            bool addMovie = false;

            do {
                Console.Clear();

                ViewMovies();

                Console.WriteLine("Please enter the movie's title and press Enter.");
                Console.WriteLine("Note: No input returns to the menu.");
                string newMovie = Console.ReadLine();

                // Returns to menu
                if (newMovie.Length == 0) {
                    return;
                }

                // Adds and sorts movie if it doesn't already exist
                if (!movieList.Exists(x => x == newMovie)) {
                    movieList.Add(newMovie);
                    movieList.Sort();
                    System.IO.File.WriteAllLines(fileDir, movieList.ToArray()); // Updates file

                    ViewMovies();

                    Console.WriteLine($"Successfully added \"{newMovie}\"."); // Add error checking at some point

                    // Prompt whether user wants to add another movie
                    if (YesOrNo("Add another movie? (Y/N)")) {
                        continue;
                    } else {
                        addMovie = true;
                    }
                } else {
                    // Movie already exists, so reprompt user
                    Console.WriteLine($"\n\"{newMovie}\" already exists.");

                    if (YesOrNo("Add a different movie? (Y/N)")) {
                        continue;
                    } else {
                        addMovie = true;
                    }
                }
            } while (addMovie == false);
        }

        /// <summary>
        /// Deletes movie from list and updates file.
        /// </summary>
        static void DeleteMovie() {
            bool deleteMovie = false;

            do {
                Console.Clear();

                ViewMovies();

                Console.WriteLine("Please enter the movie's title and press Enter.");
                Console.WriteLine("Note: No input returns to the menu.");
                string oldMovie = Console.ReadLine();

                // Returns to menu
                if (oldMovie.Length == 0) {
                    return;
                }

                // Removie() also gives a False if it couldn't find the item, so this avoid that.
                if (movieList.Exists(x => x == oldMovie)) {
                    if (movieList.Remove(oldMovie)) {
                        // Deletes and sorts movie list if it exists
                        movieList.Sort();
                        System.IO.File.WriteAllLines(fileDir, movieList.ToArray());

                        ViewMovies();

                        Console.WriteLine($"Successfully removed \"{oldMovie}\"."); // Add error checking at some point

                        // Prompt user if they want to delete another movie
                        if (YesOrNo("Delete another movie? (Y/N)")) {
                            continue;
                        } else {
                            deleteMovie = true;
                        }
                    } else {
                        // Gives error if it could not delete
                        ErrorHandler(ErrorCode.CouldNotDelete);
                    }
                } else {
                    // Reprompts user if it couldn't find the movie.
                    Console.WriteLine($"\n\"{oldMovie}\" does not exist.");

                    if (YesOrNo("Delete a different movie? (Y/N)")) {
                        continue;
                    } else {
                        deleteMovie = true;
                    }
                }
            } while (deleteMovie == false);
        }

        /// <summary>
        /// Exits program.
        /// </summary>
        static void Exit() {
            Console.WriteLine("\nGoodbye"); // Politeness
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit((int)ErrorCode.Success);
        }

        /// <summary>
        /// Display all movies currently in list.
        /// </summary>
        static void ViewMovies() {
            Console.Clear();

            Console.WriteLine("Movie list:");
            Console.WriteLine();
            Console.WriteLine("========");

            if (movieList.Count() == 0) {
                Console.WriteLine("--Empty--"); // Displays this if there are no movies in the list.
            } else {
                foreach (string movieTitle in movieList) {
                    // Displays movie title
                    Console.WriteLine(movieTitle);
                }
            }

            Console.WriteLine("========");
            Console.WriteLine();
        }

        // Additional methods

        /// <summary>
        /// Creates a text file in a directory if it doesn't already exist.
        /// </summary>
        static void CreateFile() {
            bool createFile = false;

            do {
                Console.Clear();

                System.IO.File.Create(fileDir).Close(); // Create and close file to allow editing later

                Console.WriteLine("Creating file...");

                // Check if file was created
                if (System.IO.File.Exists(fileDir)) {
                    Console.WriteLine($"\nFile created at {fileDir}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    createFile = true; // Exit loop when file is created
                } else {
                    ErrorHandler(ErrorCode.FileDoesNotExist);
                }
            } while (createFile == false);
        }

        /// <summary>
        /// Displays and returns an error code, then exits program.
        /// </summary>
        /// <param name="errorName">The name of the error</param>
        static void ErrorHandler(ErrorCode errorName) {
            Console.Clear();

            Console.WriteLine("There was an error.");
            Console.WriteLine($"Errorcode: {errorName}");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit((int)errorName);
        }

        /// <summary>
        /// Reads from file and adds all movies to list. Also sorts file and removes duplicates.
        /// </summary>
        static void StartUp() {
            // Checks if file exists. If it doesn't, makes a call to CreateFile().
            if (System.IO.File.Exists(fileDir)) {
                foreach (string movieTitle in System.IO.File.ReadLines(fileDir)) {
                    // Gets rid of blanks and duplicates
                    if (movieTitle.Length != 0 && !movieList.Exists(x => x == movieTitle)) {
                        movieList.Add(movieTitle);
                    }
                }
                movieList.Sort(); // Sorts list
                System.IO.File.WriteAllLines(fileDir, movieList.ToArray()); // Updates file
            } else {
                Console.WriteLine($"{fileDir} does not exist!");

                if (YesOrNo("Create file? (Y/N)")) {
                    CreateFile();
                } else {
                    // Exit if user doesn't want to create the file.
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadKey();
                    Environment.Exit((int)ErrorCode.FileDoesNotExist);
                }
            }
        }

        /// <summary>
        /// Prompts the user with a yes-or-no question.
        /// </summary>
        /// <param name="prompt">The question to ask the user.</param>
        /// <returns>True if "yes", False if "no".</returns>
        static bool YesOrNo(string prompt) {
            bool exit = false;

            do {
                Console.WriteLine(prompt);

                string choice = Console.ReadLine();
                if (choice == "y" || choice == "Y") {
                    exit = true;
                } else if (choice == "n" || choice == "N") {
                    break;
                } else {
                    Console.WriteLine("\nThat was not a valid choice.");
                    continue;
                }
            } while (exit == false);

            return exit;
        }
    }
}
