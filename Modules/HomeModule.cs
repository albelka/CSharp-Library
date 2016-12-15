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
        // Get["/clients"] = _ => {
        //   var AllClients = Client.GetAll();
        //   return View ["clients.cshtml", AllClients];
        // };
        Get["/books/new"] = _ => {
          return View["books_form.cshtml"];
        };
        Post["/books/new"] = _ => {
          // Dictionary<string, List<object>> model = new Dictionary<string, List<object>>();
          Book newBook = new Book(Request.Form["title"]);
          newBook.Save();
          Copy newCopy = new Copy(newBook.GetId(),Request.Form["number-of"])
          List<Copies> allCopies = Copy.GetAll();
          List<Books> allBooks = Book.GetAll();
          // model.Add("books", allBooks);
          // model.Add("copies", allCopies);
          return View["success.cshtml"];
        };
        // Get["/client/new"] = _ => {
        //   List<Stylist> AllStylists = Stylist.GetAll();
        //   return View["clients_form.cshtml", AllStylists];
        // };
        //
        // Post["/clients/new"] = _ => {
        //   Client newClient = new Client(Request.Form["client-name"],Request.Form["stylist"]);
        //   newClient.Save();
        //   return View["success.cshtml"];
        // };
        // Post["/clients/delete"] = _ => {
        //   Client.DeleteAll();
        //   return View["cleared.cshtml"];
        // };
        Get["/bools/{id}"] = parameters => {
          Dictionary<string, object> model = new Dictionary<string,object>();
          var selectedBook = Book.Find(parameters.id);
          var numberOfCopies = selectedBook.GetTitle();
          model.Add("stylist", selectedStylist);
          model.Add("clients", stylistClientsList);
          return View["stylist.cshtml", model];
        };
        // Get["/stylist/edit/{id}"] = parameters => {
        //   Stylist selectedStylist = Stylist.Find(parameters.id);
        //   return View["stylist_edit.cshtml", selectedStylist];
        // };
        // Patch["/stylist/edit/{id}"] = parameters => {
        //   Stylist selectedStylist = Stylist.Find(parameters.id);
        //   selectedStylist.UpdateName(Request.Form["stylist-name"]);
        //   return View["success.cshtml"];
        // };
        // Get["/client/edit/{id}"] = parameters => {
        //   Client selectedClient = Client.Find(parameters.id);
        //   return View["client_edit.cshtml", selectedClient];
        // };
        // Patch["/client/edit/{id}"] = parameters => {
        //   Client selectedClient = Client.Find(parameters.id);
        //   selectedClient.UpdateName(Request.Form["client-name"]);
        //   return View["success.cshtml"];
        // };
        // Get["/stylist/delete/{id}"] = parameters => {
        //   Stylist selectedStylist = Stylist.Find(parameters.id);
        //   return View["/stylist_delete.cshtml", selectedStylist];
        // };
        // Delete["stylist/delete/{id}"] = parameters => {
        //   Stylist selectedStylist = Stylist.Find(parameters.id);
        //   selectedStylist.Delete();
        //   return View["success.cshtml"];
        // };
        // Get["/client/delete/{id}"] = parameters => {
        //   Client selectedClient = Client.Find(parameters.id);
        //   return View["/client_delete.cshtml", selectedClient];
        // };
        // Delete["/client/delete/{id}"] = parameters => {
        //   Client selectedClient = Client.Find(parameters.id);
        //   selectedClient.Delete();
        //   return View["success.cshtml"];
        // };
      }
  }
}
