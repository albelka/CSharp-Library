using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library
{
  public class Copy
  {

    private int _id;
    private int _bookId;
    private int _numberOf;
    private DateTime _dueDate;

    public Copy(int BookId, int NumberOf, DateTime DueDate, int Id = 0)
    {
      _bookId = BookId;
      _numberOf = NumberOf;
      _dueDate = DueDate;
      _id = Id;
    }

    public override bool Equals(System.Object otherCopy)
    {
      if (!(otherCopy is Copy))
      {
        return false;
      }
      else
      {
        Copy newCopy = (Copy) otherCopy;
        bool bookIdEquality = (this.GetBookId() == newCopy.GetBookId());
        bool numberOfEquality = (this.GetNumberOf() == newCopy.GetNumberOf());
        bool dueDateEquality = (this.GetDueDate() == newCopy.GetDueDate());


        return (bookIdEquality && numberOfEquality && dueDateEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public int GetBookId()
    {
      return _bookId;
    }

    public int GetNumberOf()
    {
      return _numberOf;
    }

    public DateTime GetDueDate()
    {
      return _dueDate;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO copies (book_id, number_of, due_date) OUTPUT INSERTED.id VALUES (@BookId, @NumberOf, @DueDate);", conn);

      SqlParameter bookIdParameter = new SqlParameter("@BookId", this.GetBookId());
      cmd.Parameters.Add(bookIdParameter);
      SqlParameter numberOfParameter = new SqlParameter("@NumberOf", this.GetNumberOf());
      cmd.Parameters.Add(numberOfParameter);
      SqlParameter dueDateParameter = new SqlParameter("@DueDate", this.GetDueDate());
      cmd.Parameters.Add(dueDateParameter);
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

    public static List<Copy> GetAll()
    {
      List<Copy> allCopies = new List<Copy>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        int bookId = rdr.GetInt32(1);
        int numberOf = rdr.GetInt32(2);
        DateTime dueDate = rdr.GetDateTime(3);
        Copy newCopy = new Copy(bookId, numberOf, dueDate, copyId);
        allCopies.Add(newCopy);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCopies;
    }

    public static Copy Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE id = @CopyId;", conn);
      SqlParameter copyIdParameter = new SqlParameter("@CopyId", id.ToString());
      cmd.Parameters.Add(copyIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCopyId = 0;
      int foundBookId = 0;
      int foundNumberOf = 0;
      DateTime foundDueDate = DateTime.Today;
      while(rdr.Read())
      {
        foundCopyId = rdr.GetInt32(0);
        foundBookId = rdr.GetInt32(1);
        foundNumberOf = rdr.GetInt32(2);
        foundDueDate = rdr.GetDateTime(3);
      }
      Copy foundCopy = new Copy(foundBookId, foundNumberOf, foundDueDate, foundCopyId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundCopy;
    }

    public void AddPatron(Patron newPatron)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (copy_id, patron_id) VALUES (@CopyId, @PatronId);", conn);

      SqlParameter copyIdParameter = new SqlParameter("@CopyId", this.GetId());
      cmd.Parameters.Add(copyIdParameter);

      SqlParameter patronIdParameter = new SqlParameter("@PatronId", newPatron.GetId());
      cmd.Parameters.Add(patronIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Patron> GetPatrons()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT patrons.* FROM copies JOIN checkouts ON (copies.id = checkouts.copy_id) JOIN patrons ON (checkouts.patron_id = patrons.id) WHERE copies.id=@CopyId;",conn);
      SqlParameter CopyIdParameter = new SqlParameter("@CopyId", this.GetId());
      cmd.Parameters.Add(CopyIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Patron> patrons = new List<Patron>{};
      while(rdr.Read())
      {
        int PatronId = rdr.GetInt32(0);
        string PatronName = rdr.GetString(1);
        Patron newPatron = new Patron(PatronName, PatronId);
        patrons.Add(newPatron);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return patrons;
    }

    public void Update(int NumberOf)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE copies SET number_of = @NumberOf WHERE id = @CopyId;", conn);

      SqlParameter copyIdParameter = new SqlParameter("@CopyId", this.GetId());
      SqlParameter copyNumberOfParameter = new SqlParameter("@NumberOf", NumberOf);
       cmd.Parameters.Add(copyIdParameter);
       cmd.Parameters.Add(copyNumberOfParameter);

       SqlDataReader rdr = cmd.ExecuteReader();

       while(rdr.Read())
       {
         _id = rdr.GetInt32(0);
         _numberOf = rdr.GetInt32(1);
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

//     // public static List<TEMPLATE> Sort()
//     // {
//     //   List<Task> allTEMPLATE = new List<Task>{};
//     //
//     //   SqlConnection conn = DB.Connection();
//     //   conn.Open();
//     //
//     //   SqlCommand cmd = new SqlCommand("SELECT * FROM template ORDER BY TEMPLATEdate;", conn);
//     //   SqlDataReader rdr = cmd.ExecuteReader();
//     //
//     //   while(rdr.Read())
//     //   {
//     //     int TEMPLATEId = rdr.GetInt32(0);
//     //     string TEMPLATEDescription = rdr.GetString(1);
//     //     TEMPLATE newTEMPLATE = new TEMPLATE(TEMPLATEDescription, TEMPLATEId);
//     //     allTEMPLATE.Add(newTEMPLATE);
//     //   }
//     //
//     //   if (rdr != null)
//     //   {
//     //     rdr.Close();
//     //   }
//     //   if (conn != null)
//     //   {
//     //     conn.Close();
//     //   }
//     //
//     //   return allTEMPLATE;
//     // }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM copies WHERE id = @CopyId; DELETE FROM checkouts WHERE copy_id = @CopyId", conn);
      SqlParameter copyIdParameter = new SqlParameter("@CopyId", this.GetId());
      cmd.Parameters.Add(copyIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM copies;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
