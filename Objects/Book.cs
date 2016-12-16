
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Book
  {

    private int _id;
    private string _title;


    public Book(string Title, int Id = 0)
    {
      _title = Title;
      _id = Id;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool titleEquality = (this.GetTitle() == newBook.GetTitle());
        return (titleEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetTitle()
    {
      return _title;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (title) OUTPUT INSERTED.id VALUES (@BookTitle);", conn);

      SqlParameter bookTitleParameter = new SqlParameter("@BookTitle", this.GetTitle());
      cmd.Parameters.Add(bookTitleParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }



    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int BookId = rdr.GetInt32(0);
        string BookTitle = rdr.GetString(1);
        Book newBook = new Book(BookTitle, BookId);
        allBooks.Add(newBook);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allBooks;
    }

    public static Book Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = @BookId;", conn);
      SqlParameter bookIdParameter = new SqlParameter("@BookId", id.ToString());
      cmd.Parameters.Add(bookIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundBookId = 0;
      string foundBookTitle = null;
      while(rdr.Read())
      {
        foundBookId = rdr.GetInt32(0);
        foundBookTitle = rdr.GetString(1);
      }

      Book foundBook = new Book(foundBookTitle, foundBookId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundBook;
    }

    public void AddAuthor(Author newAuthor)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);", conn);

      SqlParameter bookIdParameter = new SqlParameter("@BookId", this.GetId());
      cmd.Parameters.Add(bookIdParameter);

      SqlParameter authorIdParameter = new SqlParameter("@AuthorId", newAuthor.GetId());
      cmd.Parameters.Add(authorIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Author> GetAuthors()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT authors.* FROM books JOIN authors_books ON (books.id = authors_books.book_id) JOIN authors ON (authors_books.author_id = authors.id) WHERE books.id=@BookId;",conn);

      SqlParameter BookIdParameter = new SqlParameter("@BookId", this.GetId());
      cmd.Parameters.Add(BookIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Author> authors = new List<Author>{};
      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author newAuthor = new Author(authorName, authorId);
        authors.Add(newAuthor);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return authors;
    }

    public int GetCopies()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE book_id = @BookId;",conn);

      SqlParameter BookIdParameter = new SqlParameter("@BookId", this.GetId());
      cmd.Parameters.Add(BookIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int numberOf = 0;
      List<Copy> copies = new List<Copy>{};
      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        int bookId = rdr.GetInt32(1);
        numberOf = rdr.GetInt32(2);
        DateTime dueDate = rdr.GetDateTime(3);
        Copy newCopy = new Copy(bookId, numberOf, dueDate, copyId);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return numberOf;
    }


    public void Update(string BookTitle)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE books SET title = @BookTitle WHERE id = @BookId;", conn);

      SqlParameter bookIdParameter = new SqlParameter("@BookId", this.GetId());
      SqlParameter bookTitleParameter = new SqlParameter("@BookTitle", BookTitle);
       cmd.Parameters.Add(bookIdParameter);
       cmd.Parameters.Add(bookTitleParameter);

       SqlDataReader rdr = cmd.ExecuteReader();

       while(rdr.Read())
       {
         _id = rdr.GetInt32(0);
         _title = rdr.GetString(1);
       }
       if (rdr != null)
       {
         rdr.Close();
       }
       if (conn != null)
       {
         conn.Close();
       }
     }
     public static List<Book> Search(string authorName)
     {
       SqlConnection conn = DB.Connection();
       conn.Open();
       SqlCommand cmd = new SqlCommand("SELECT books.* FROM authors JOIN authors_books ON (authors.id = authors_books.author_id) JOIN books ON (authors_books.book_id = books.id) WHERE authors.name=@AuthorName;",conn);

       SqlParameter authorNameParameter = new SqlParameter("@AuthorName", authorName);
       cmd.Parameters.Add(authorNameParameter);
       SqlDataReader rdr = cmd.ExecuteReader();

       List<Book> books = new List<Book>{};
       while(rdr.Read())
       {
         int bookId = rdr.GetInt32(0);
         string bookTitle = rdr.GetString(1);
         Book newBook = new Book(bookTitle, bookId);
         books.Add(newBook);
       }
       if (rdr != null)
       {
         rdr.Close();
       }
       if (conn != null)
       {
         conn.Close();
       }
       return books;
     }

    // public static List<TEMPLATE> Sort()
    // {
    //   List<Task> allTEMPLATE = new List<Task>{};
    //
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT * FROM template ORDER BY TEMPLATEdate;", conn);
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   while(rdr.Read())
    //   {
    //     int TEMPLATEId = rdr.GetInt32(0);
    //     string TEMPLATEDescription = rdr.GetString(1);
    //     TEMPLATE newTEMPLATE = new TEMPLATE(TEMPLATEDescription, TEMPLATEId);
    //     allTEMPLATE.Add(newTEMPLATE);
    //   }
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //
    //   return allTEMPLATE;
    // }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM books WHERE id = @BookId; DELETE FROM authors_books WHERE book_id = @BookId", conn);
      SqlParameter bookIdParameter = new SqlParameter("@BookId", this.GetId());
      cmd.Parameters.Add(bookIdParameter);
      cmd.ExecuteNonQuery();

      if(conn!=null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM books;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
