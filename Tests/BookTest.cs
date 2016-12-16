using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Replace_ReplacesEqualObjects()
    {

      Book bookOne = new Book("Bobby Goes Home");
      Book bookTwo = new Book("Bobby Goes Home");

      Assert.Equal(bookOne, bookTwo);
    }

    [Fact]
    public void GetAll_GetsAllBooksFromDatabase()
    {
      //Arrange
      Book bookOne = new Book("Bobby Goes Home");
      bookOne.Save();
      Book bookTwo = new Book("Bobby Didnt Come Home");
      bookTwo.Save();
      // Act
      int result = Book.GetAll().Count;

      //Assert
      Assert.Equal(2, result);
    }

    [Fact]
    public void Save_SavesToDatabase()
    {
      //Arrange
      Book testBook = new Book("Bobby Goes Home");
      testBook.Save();
      //Act

      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook};
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void AddAuthor_AddsAuthorToBook()
    {
      Book newBook = new Book("Mr Wiggles");
      newBook.Save();
      Author newAuthor = new Author("Ryan");
      newAuthor.Save();
      newBook.AddAuthor(newAuthor);
      List<Author> expected = new List<Author>{newAuthor};
      List<Author> result = newBook.GetAuthors();

      Assert.Equal(expected, result);
    }

    [Fact]
    public void GetAuthors_GetsAuthorFromDatabase()
    {
      Book newBook = new Book("Mr Wiggles");
      newBook.Save();
      Author newAuthor = new Author("Ryan");
      newAuthor.Save();
      newBook.AddAuthor(newAuthor);

      List<Author> expected = new List<Author>{newAuthor};
      List<Author> result = newBook.GetAuthors();

      Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCopies_GetsCopiesFromDatabase()
    {
      Book newBook = new Book("Mr Wiggles");
      newBook.Save();
      Copy newCopy = new  Copy(newBook.GetId(),5, DateTime.Today);
      newCopy.Save();

      int result =newBook.GetCopies();

      Assert.Equal(5, result);
    }

    [Fact]
    public void Find_FindsBookInDatabase_true()
    {
      //Arrange
      Book testBook = new Book("Mr Wiggles");
      testBook.Save();

      //Act
      Book foundBook = Book.Find(testBook.GetId());

      //Assert
      Assert.Equal(testBook, foundBook);
    }

    [Fact]
    public void Test_Deletes_Book()
    {
      Book newBook = new Book("Mr Wiggles");

      newBook.Save();
      newBook.Delete();

      List<Book> expected = new List<Book>{};
      List<Book> result = Book.GetAll();

      Assert.Equal(expected, result);

    }
    [Fact]
    public void Delete_DeletesAssociation_True()
    {
      Book newBook = new Book("Mr Wiggles");
      newBook.Save();
      Author newAuthor = new Author("Shel Silverstein");
      newBook.Save();
      newBook.AddAuthor(newAuthor);
      newBook.Delete();

      List<Author> result = newBook.GetAuthors();
      List<Author> expected = new List<Author>{};
      Assert.Equal(expected, result);

    }

    [Fact]
    public void Update_UpdatesDatabase_True()
    {
      Book newBook = new Book("Mr Wiggles");
      newBook.Save();
      newBook.Update("Mr Jiggles");

      Book testBook = new Book("Mr Jiggles");
      Book result = Book.Find(newBook.GetId());

      Assert.Equal(testBook, result);
    }

    [Fact]
    public void Search_SearchBookByAuthor()
    {
      Author newAuthor = new Author("PD Woodhouse");
      newAuthor.Save();

      Book bookOne = new Book("Mr Wiggles");
      bookOne.Save();
      bookOne.AddAuthor(newAuthor);

      Book bookTwo = new Book("Mr Muffin");
      bookTwo.Save();
      bookTwo.AddAuthor(newAuthor);

      Book bookThree = new Book("Mr Floof");
      bookThree.Save();

      List<Book> booksByAuthor = Book.Search("PD Woodhouse");
      List<Book> expectedBooks = new List<Book> {bookOne, bookTwo};

      Assert.Equal(expectedBooks, booksByAuthor);
    }


    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
    }
  }
}
