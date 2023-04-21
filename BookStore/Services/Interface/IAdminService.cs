using BookStore.Models;
using BookStore.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Interface
{
    public interface IAdminService
    {
        public Admin validateCredential(Admin adminLoginModel);
        public void AddBook(BookModel book);
        public void DeleteBook(string id);
        public List<BookModel> GetAllBooks();
        public VolumeInfoModel GetBookByIsbn(string isbn);
    }
}
