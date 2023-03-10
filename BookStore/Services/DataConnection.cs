using BookStore.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class DataConnection: IDataConnection
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

    }
}
