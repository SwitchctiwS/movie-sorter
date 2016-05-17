namespace MovieSorter
{
    class Movie
    {
        private string title;

        internal Movie(string title)
        {
            this.title = title;
        }

        internal string Title // Auto-implemented property didn't work. Don't know why.
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }
    }
}