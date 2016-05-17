using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSorter
{
    class Movie
    {
        private string title;

        internal Movie(string title)
        {
            this.title = title;
        }

        internal string Title { get; set; }
    }
}
