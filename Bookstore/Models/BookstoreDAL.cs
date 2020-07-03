using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class BookstoreDAL
    {
        public HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"https://localhost:44318");
            return client;
        }

        public async Task<List<Books>> GetBooks()
        {
            HttpClient client = GetHttpClient();
           var response = await client.GetAsync($"api/bookapi/");
            //Install-package Microsoft.AspNet.WebAPI.Client
            List<Books> booklist = await response.Content.ReadAsAsync<List<Books>>();
            return booklist;

        }

        public async Task<Books> AddBook(Books newBook)
        {
            HttpClient client = GetHttpClient();
            var response = await client.PostAsJsonAsync($"api/bookapi", newBook);
            var bookResult = await response.Content.ReadAsAsync<Books>();
            return bookResult;
        }
        public async Task<bool> DeleteBook (int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.DeleteAsync($"api/bookapi/{id}");
            return response.IsSuccessStatusCode;

        }
        public async void UpdateBook (Books updatedBook)
        {
            HttpClient client = GetHttpClient();
            var response = await client.PutAsJsonAsync($"api/bookapi", updatedBook);
          

        }

        public async Task<Books> GetBook(int id)
        {
            HttpClient client = GetHttpClient();
            var response = await client.GetAsync($"api/bookapi/{id}");
            if (response.IsSuccessStatusCode)
            {
                var book = await response.Content.ReadAsAsync<Books>();
                return book;
            }
            else
            {
                return null;
            }
        }
    }
}
