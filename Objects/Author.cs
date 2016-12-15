
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Author
  {

    private int _id;
    private string _name;


    public Author(string Name, int Id = 0)
    {
      _name = Name;
      _id = Id;
    }

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else
      {
        Author newAuthor = (Author) otherAuthor;
        bool nameEquality = (this.GetName() == newAuthor.GetName());
        return (nameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES (@AuthorName);", conn);

      SqlParameter authorNameParameter = new SqlParameter("@AuthorName", this.GetName());
      cmd.Parameters.Add(authorNameParameter);
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

    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int AuthorId = rdr.GetInt32(0);
        string AuthorName = rdr.GetString(1);
        Author newAuthor = new Author(AuthorName, AuthorId);
        allAuthors.Add(newAuthor);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allAuthors;
    }

    public static Author Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = @AuthorId;", conn);
      SqlParameter authorIdParameter = new SqlParameter("@AuthorId", id.ToString());
      cmd.Parameters.Add(authorIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundAuthorId = 0;
      string foundAuthorName = null;
      while(rdr.Read())
      {
        foundAuthorId = rdr.GetInt32(0);
        foundAuthorName = rdr.GetString(1);
      }
      Author foundAuthor = new Author(foundAuthorName, foundAuthorId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundAuthor;
    }





    public void AddBook(Book newBook)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);", conn);

      SqlParameter AuthorIdParameter = new SqlParameter("@AuthorId", this.GetId());
      cmd.Parameters.Add(AuthorIdParameter);

      SqlParameter bookIdParameter = new SqlParameter("@BookId", newBook.GetId());
      cmd.Parameters.Add(bookIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Book> GetBooks()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT books.* FROM authors JOIN authors_books ON (authors.id = authors_books.author_id) JOIN books ON (authors_books.book_id = books.id) WHERE authors.id=@AuthorId;",conn);
      SqlParameter AuthorIdParameter = new SqlParameter("@AuthorId", this.GetId());
      cmd.Parameters.Add(AuthorIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Book> books = new List<Book>{};
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookName = rdr.GetString(1);
        Book newBook = new Book(bookName, bookId);
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

    public void Update(string AuthorName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE authors SET name = @AuthorName WHERE id = @AuthorId;", conn);

      SqlParameter authorIdParameter = new SqlParameter("@AuthorId", this.GetId());
      SqlParameter authorNameParameter = new SqlParameter("@AuthorName", AuthorName);
       cmd.Parameters.Add(authorIdParameter);
       cmd.Parameters.Add(authorNameParameter);

       SqlDataReader rdr = cmd.ExecuteReader();

       while(rdr.Read())
       {
         _id = rdr.GetInt32(0);
         _name = rdr.GetString(1);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM authors WHERE id = @AuthorId; DELETE FROM authors_books WHERE author_id = @AuthorId", conn);
      SqlParameter authorIdParameter = new SqlParameter("@AuthorId", this.GetId());
      cmd.Parameters.Add(authorIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
