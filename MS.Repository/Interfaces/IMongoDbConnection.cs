using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MS.Repository.Interfaces
{
    public interface IMongoDbConnection
    {
        IMongoDatabase GetDatabase();
    }
}