using IMDBMovieRater;
using System;
using System.Collections.Generic;
using System.IO;

namespace IMDBRater
{
    class Program
    {
        static void Main(string[] args)
        {
            var directorySearch = new DirectoryRepository();
            var modifyDirectoryName = new ModifyDirectoryName();
            List<DirectoryInfo> movieList = new List<DirectoryInfo>();

            try
            {
                movieList = GetMovieList(directorySearch);

                foreach (var movie in movieList)
                {
                    var rating = GetMovieInfo(movie.Name);

                    if (rating != null)
                        UpdateMovieDirectoryNameWithRating(movie, rating, modifyDirectoryName);
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static void UpdateMovieDirectoryNameWithRating(DirectoryInfo movie, string movieRating, ModifyDirectoryName modifyDirectoryName)
        {
            var rating = movieRating.Remove(0, 12);
            rating = rating.Remove(3, 1);

            if (rating.Equals("N/A"))
                rating = rating.Remove(1, 1);

            modifyDirectoryName.Rename(movie, rating);
        }

        private static List<DirectoryInfo> GetMovieList(DirectoryRepository directorySearch)
        {
            var movies = directorySearch.GetDirectoryList();

            if (movies == null || movies.Count == 0)
                Console.WriteLine("No movie exist at the specified path.");

            else
                Console.WriteLine("Movie list retrieved.");

            return movies;
        }

        private static string GetMovieInfo(string movieName)
        {
            string tempDirectoryName = "TempDirectory";

            if (movieName.Equals(tempDirectoryName))
                return null;

            Console.WriteLine("\n" + movieName);

            var movieInfoSearch = new MovieRepository();

            var movieImdbRating = movieInfoSearch.GetMovieImdbRating(movieName);

            if (movieImdbRating == null || movieImdbRating.Contains("Data not found. Error Details : "))
            {
                Console.WriteLine("No information available for movie '{0}'.", movieName);
                return null;
            }

            Console.WriteLine("For movie '{0}', {1}.", movieName, movieImdbRating);
            return movieImdbRating;
        }
    }
}
