﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Interface
{
    public interface IDataConnection
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }

    }
}
