using Nancy;
using System;
using System.Collections.Generic;
using Nancy.ViewEngines.Razor;


namespace Library
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
      {
        Get["/"] = _ => {
          // List<Stylist> AllLists = Stylist.GetAll();
          return View["index.cshtml"];
        };
        Get["/books"] = _ => {
          var AllBooks = Book.GetAll();
          return View["books.cshtml", AllBooks];
        };
        Get["/patrons"] = _ => {
          List<Patron> allPatrons = Patron.GetAll();
          return View ["patrons.cshtml", allPatrons];
        };
        Get["/books/new"] = _ => {
          return View["books_form.cshtml"];
        };
        Post["/books/new"] = _ => {
          Book newBook = new Book(Request.Form["title"]);
          newBook.Save();
          Copy newCopy = new Copy(newBook.GetId(),Request.Form["number-of"], DateTime.Today);
          newCopy.Save();
          Author newAuthor = new Author(Request.Form["author"]);
          newAuthor.Save();
          newBook.AddAuthor(newAuthor);
          List<Author> allAuthors = Author.GetAll();
          List<Copy> allCopies = Copy.GetAll();
          List<Book> allBooks = Book.GetAll();
          return View["success.cshtml"];
        };
        Get["/books/search"] = _ => {
          return View["books_search.cshtml"];
        };
        Get["/books/found"] = _ => {
          List<Author> selectAuthors = new List<Author>{};
          List<Book> foundBooks = new List<Book>{};
          string authorName = Request.Form["name"];
          List<Author> allAuthors = Author.GetAll();
          foreach(Author listAuthor in allAuthors)
          {
            if(listAuthor.GetName() == authorName)
            {
              selectAuthors.Add(listAuthor);
            }
          }
          foreach(Author newAuthor in selectAuthors)
          {
            foundBooks=newAuthor.GetBooks();
          }
          return View["/books_found.cshtml", foundBooks];
        };
        Get["/patrons/new"] = _ => {
          List<Patron> AllPatrons = Patron.GetAll();
          return View["patrons_form.cshtml", AllPatrons];
        };

        Post["/patrons/new"] = _ => {
          Patron newPatron = new Patron(Request.Form["name"]);
          newPatron.Save();
          return View["success.cshtml"];
        };

        Get["/books/{id}"] = parameters => {
          Dictionary<string, object> model = new Dictionary<string,object>();
          var selectedBook = Book.Find(parameters.id);
          List<Author> author = selectedBook.GetAuthors();
          selectedBook.AddAuthor(author[0]);
          var copies = selectedBook.GetCopies();
          model.Add("book", selectedBook);
          model.Add("author", author);
          model.Add("copies", copies);
          return View["book.cshtml", model];
        };

        Get["/patron/{id}"] = parameters => {
          Patron selectedPatron = Patron.Find(parameters.id);
          List<object> model = new List<object>{};
          List<Book> bookList = Book.GetAll();
          model.Add(selectedPatron);
          model.Add(bookList);
          return View["patron.cshtml", model];
        };
        Get["patron/checkout/{id}"] = parameters => {
          List<Book> bookList = new List<Book> {};
          Patron selectedPatron = Patron.Find(parameters.id);
          Book newBook = Book.Find(int.Parse(Request.Form("book")));
          Console.WriteLine(newBook);
          bookList.Add(newBook);
          return View["patron_checkout.cshtml", bookList];
        };
        // Patch["patron/checkout/{id}"] = parameters => {
        //   Patron selectedPatron = Patron.Find(parameters.id);
        //   Book newBook = Book.Find(Request.Form("book"));
        //   return View["success.cshtml"];
        // };

        Get["/book/edit/{id}"] = parameters => {
          Book selectedBook = Book.Find(parameters.id);
          return View["book_edit.cshtml", selectedBook];
        };
        Patch["/book/edit/{id}"] = parameters => {
          Book selectedBook = Book.Find(parameters.id);
          selectedBook.Update(Request.Form["book-title"]);
          return View["success.cshtml"];
        };
        Get["/patron/edit/{id}"] = parameters => {
          Patron selectedPatron = Patron.Find(parameters.id);
          return View["patron_edit.cshtml", selectedPatron];
        };
        Patch["/patron/edit/{id}"] = parameters => {
          Patron selectedPatron =Patron.Find(parameters.id);
          selectedPatron.Update(Request.Form["name"]);
          return View["success.cshtml"];
        };
        Get["/book/delete/{id}"] = parameters => {
          Book selectedBook = Book.Find(parameters.id);
          return View["/book_delete.cshtml", selectedBook];
        };
        Delete["book/delete/{id}"] = parameters => {
          Book selectedBook = Book.Find(parameters.id);
          selectedBook.Delete();
          return View["success.cshtml"];
        };
        Get["/patron/delete/{id}"] = parameters => {
          Patron selectedPatron =Patron.Find(parameters.id);
          return View["/patron_delete.cshtml", selectedPatron];
        };
        Delete["/patron/delete/{id}"] = parameters => {
          Patron selectedPatron =Patron.Find(parameters.id);
          selectedPatron.Delete();
          return View["success.cshtml"];
        };
      }
  }
}
