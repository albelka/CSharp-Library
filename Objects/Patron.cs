using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Patron
  {

    private int _id;
    private string _name;


    public Patron(string Name, int Id = 0)
    {
      _name = Name;
      _id = Id;
    }

    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool nameEquality = (this.GetName() == newPatron.GetName());
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

      SqlCommand cmd = new SqlCommand("INSERT INTO patrons (name) OUTPUT INSERTED.id VALUES (@PatronName);", conn);

      SqlParameter patronNameParameter = new SqlParameter("@PatronName", this.GetName());
      cmd.Parameters.Add(patronNameParameter);
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

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int PatronId = rdr.GetInt32(0);
        string PatronName = rdr.GetString(1);
        Patron newPatron = new Patron(PatronName, PatronId);
        allPatrons.Add(newPatron);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allPatrons;
    }

    public static Patron Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = @PatronId;", conn);
      SqlParameter patronIdParameter = new SqlParameter("@PatronId", id.ToString());
      cmd.Parameters.Add(patronIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundPatronId = 0;
      string foundPatronName = null;
      while(rdr.Read())
      {
        foundPatronId = rdr.GetInt32(0);
        foundPatronName = rdr.GetString(1);
      }
      Patron foundPatron = new Patron(foundPatronName, foundPatronId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundPatron;
    }

    public void AddCopy(Copy newCopy)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (patron_id, copy_id) VALUES (@PatronId, @CopyId);", conn);

      SqlParameter PatronIdParameter = new SqlParameter("@PatronId", this.GetId());
      cmd.Parameters.Add(PatronIdParameter);

      SqlParameter copyIdParameter = new SqlParameter("@CopyId", newCopy.GetId());
      cmd.Parameters.Add(copyIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Book> GetCheckouts(List<Copy> copies)
    {
      List<Book> checkouts = new List<Book>{};

      foreach(Copy Copy in copies)
      {
        Book newBook = Book.Find(Copy.GetBookId());
        checkouts.Add(newBook);
      }
      return checkouts;
    }

    public List<Copy> GetCopies()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT copies.* FROM patrons JOIN checkouts ON (patrons.id = checkouts.patron_id) JOIN copies ON (checkouts.copy_id = copies.id) WHERE patrons.id=@PatronId;",conn);
      SqlParameter PatronIdParameter = new SqlParameter("@PatronId", this.GetId());
      cmd.Parameters.Add(PatronIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Copy> copies = new List<Copy>{};
      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        int bookId = rdr.GetInt32(1);
        int numberOf = rdr.GetInt32(2);
        DateTime dueDate = rdr.GetDateTime(3);
        Copy newCopy = new Copy(bookId, numberOf, dueDate, copyId);
        copies.Add(newCopy);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return copies;
    }

    public void Update(string PatronName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE patrons SET name = @PatronName WHERE id = @PatronId;", conn);

      SqlParameter patronIdParameter = new SqlParameter("@PatronId", this.GetId());
      SqlParameter patronNameParameter = new SqlParameter("@PatronName", PatronName);
       cmd.Parameters.Add(patronIdParameter);
       cmd.Parameters.Add(patronNameParameter);

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

    // // public static List<TEMPLATE> Sort()
    // // {
    // //   List<Task> allTEMPLATE = new List<Task>{};
    // //
    // //   SqlConnection conn = DB.Connection();
    // //   conn.Open();
    // //
    // //   SqlCommand cmd = new SqlCommand("SELECT * FROM template ORDER BY TEMPLATEdate;", conn);
    // //   SqlDataReader rdr = cmd.ExecuteReader();
    // //
    // //   while(rdr.Read())
    // //   {
    // //     int TEMPLATEId = rdr.GetInt32(0);
    // //     string TEMPLATEDescription = rdr.GetString(1);
    // //     TEMPLATE newTEMPLATE = new TEMPLATE(TEMPLATEDescription, TEMPLATEId);
    // //     allTEMPLATE.Add(newTEMPLATE);
    // //   }
    // //
    // //   if (rdr != null)
    // //   {
    // //     rdr.Close();
    // //   }
    // //   if (conn != null)
    // //   {
    // //     conn.Close();
    // //   }
    // //
    // //   return allTEMPLATE;
    // // }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM patrons WHERE id = @PatronId; DELETE FROM checkouts WHERE patron_id = @PatronId", conn);
      SqlParameter patronIdParameter = new SqlParameter("@PatronId", this.GetId());
      cmd.Parameters.Add(patronIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
