// See https://aka.ms/new-console-template for more information
internal class Book
{
    public string BookName { get; }
    public string Author { get; }

    public Book(string bookName, string author)
    {
        BookName = bookName;
        Author = author;
    }

    public override bool Equals(object? obj)
    {
        return obj is Book other &&
               BookName == other.BookName &&
               Author == other.Author;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BookName, Author);
    }

    public override string ToString()
    {
        return $"{this.BookName}-{this.Author}";
    }
}