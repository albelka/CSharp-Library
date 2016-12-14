using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class AuthorTest : IDisposable
  {
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void ReplacesEqualObjects_True()
    {

      Author authorOne = new Author("Bobby");
      Author authorTwo = new Author("Bobby");

      Assert.Equal(authorOne, authorTwo);
    }

    [Fact]
    public void GetAll_true()
    {
      //Arrange
      Author authorOne = new Author("Daniel");
      authorOne.Save();
      Author authorTwo = new Author("Ryan");
      authorTwo.Save();
      // Act
      int result = Author.GetAll().Count;

      //Assert
      Assert.Equal(2, result);
    }

    [Fact]
    public void Save_SavesToDatabase_true()
    {
      //Arrange
      Author testAuthor = new Author("Jimmy");
      testAuthor.Save();
      //Act

      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor};
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Find_FindsAuthorInDatabase_true()
    {
      //Arrange
      Author testAuthor = new Author("Ryan");
      testAuthor.Save();

      //Act
      Author foundAuthor = Author.Find(testAuthor.GetId());

      //Assert
      Assert.Equal(testAuthor, foundAuthor);
    }

    [Fact]
    public void AddBook_AddsBookToAuthor_True()
    {
      Book newBook = new Book("Mr Wiggles");
      newBook.Save();
      Author newAuthor = new Author("Ryan");
      newAuthor.Save();
      newAuthor.AddBook(newBook);
      List<Book> expected = new List<Book>{newBook};
      List<Book> result = newAuthor.GetBooks();

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
    }
  }
}
