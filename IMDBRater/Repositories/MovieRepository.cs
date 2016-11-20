using System;
using System.Configuration;
using System.Linq;
using System.Net;

namespace IMDBMovieRater
{
    public class MovieRepository
    {
        private string urlPath;

        public MovieRepository()
        {
            urlPath = ConfigurationManager.AppSettings["OMDBApiUrl"];
        }

        internal string GetMovieImdbRating(string movieName)
        {
            var url = GetUrl(movieName);

            var content = GetImdbContent(url);

            return GetImdbRating(content);
        }

        private string GetImdbContent(string url)
        {
            string result = null;

            try
            {
                var client = new WebClient();
                result = client.DownloadString(url);
            }
            catch 
            {
                return null;
            }

            return result;
        }

        private string GetImdbRating(string content)
        {
            string successResponse = "response=\"True\"";

            if (!content.Contains(successResponse))
                return "Data not found. Error Details : " + content;

            int strtIndex = content.IndexOf("imdbRating");
            int endIndex = content.IndexOf("imdbVotes");

            return content.Substring(strtIndex, endIndex - strtIndex - 1);
        }

        private string GetUrl(string movieName)
        {
            string name = movieName;
            string year = string.Empty;

            name = ReplaceYearBrackets(name);

            int yearStartIndex = name.IndexOf("[");

            if (yearStartIndex != -1)
            {
                name = name.Substring(0, yearStartIndex);
                year = movieName.Substring(yearStartIndex + 1, 4);
            }
            name = name.Replace(" ", "+");

            var url = urlPath.Replace("{1}", name);
            return url.Replace("{2}", year);
        }

        private string ReplaceYearBrackets(string name)
        {
            if (name.Contains('('))
                name = name.Replace("(", "[");
            
            if (name.Contains(')'))
                name = name.Replace(")", "]");
            
            if (name.Contains('{'))
                name = name.Replace("{", "[");
            
            if (name.Contains('}'))
                name = name.Replace("}", "]");
            
            return name;
        }
    }
}
