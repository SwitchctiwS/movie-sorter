using System.Collections.Generic;

namespace MovieSorter
{
    class MovieList<T> : List<T>
        where T : Movie
    {
        // Implement IComparable for sorting

    }
}
