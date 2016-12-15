using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class CopyTest : IDisposable
  {
    public CopyTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void ReplacesEqualObjects_True()
    {

      Copy copyOne = new Copy(1,5, DateTime.Today);
      Copy copyTwo = new Copy(1,5, DateTime.Today);

      Assert.Equal(copyOne, copyTwo);
    }

    [Fact]
    public void GetAll_true()
    {
      //Arrange
      Copy copyOne = new Copy(1,5, DateTime.Today);
      copyOne.Save();
      Copy copyTwo = new Copy(4,16, DateTime.Today);
      copyTwo.Save();
      // Act
      int result = Copy.GetAll().Count;

      //Assert
      Assert.Equal(2, result);
    }

    [Fact]
    public void Save_SavesToDatabase_true()
    {
      //Arrange
      Patron testPatron = new Patron("Penny Flowers");
      testPatron.Save();
      //Act

      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron};
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Find_FindsPatronInDatabase_true()
    {
      //Arrange
      Patron testPatron = new Patron("Penny Flowers");
      testPatron.Save();

      //Act
      Patron foundPatron = Patron.Find(testPatron.GetId());

      //Assert
      Assert.Equal(testPatron, foundPatron);
    }

    [Fact]
    public void AddPatron_AddsPatronToCopy_True()
    {
      Patron testPatron = new Patron("Penny Flowers");
      testPatron.Save();
      Copy newCopy = new Copy(1,5,DateTime.Today);
      newCopy.Save();
      newCopy.AddPatron(testPatron);
      List<Patron> expected = new List<Patron>{testPatron};
      List<Patron> result = newCopy.GetPatrons();

      Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Deletes_Author()
    {
      Author newAuthor = new Author("Virgina Woolf");

      newAuthor.Save();
      newAuthor.Delete();

      List<Author> expected = new List<Author>{};
      List<Author> result = Author.GetAll();

      Assert.Equal(expected, result);

    }
    [Fact]
    public void Delete_DeletesAssociation_True()
    {
      Book newBook = new Book("Mr Wiggles");
      newBook.Save();
      Author newAuthor = new Author("JRR Tolkien");
      newAuthor.Save();
      newAuthor.AddBook(newBook);
      newAuthor.Delete();

      List<Book> result = newAuthor.GetBooks();
      List<Book> expected = new List<Book>{};
      Assert.Equal(expected, result);

    }
    [Fact]
    public void Update_UpdatesDatabase_True()
    {
      Author newAuthor = new Author("Frank Herbert");
      newAuthor.Save();
      newAuthor.Update("Frankie Herbert");

      Author testAuthor = new Author("Frankie Herbert");
      Author result = Author.Find(newAuthor.GetId());

      Assert.Equal(testAuthor, result);
    }

    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
      Patron.DeleteAll();
      Copy.DeleteAll();
    }
  }
}
