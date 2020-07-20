using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workers.Models;

namespace Workers.Service
{
    public interface BdBrain
    {
       
        WorkersModel GetWorkers(int id);
        IEnumerable<WorkersModel> GetWorkers();

        void Add(WorkersModel workers);

        void Find(WorkersModel workers);
  
        void Edit(WorkersModel workers);

        void Remove(WorkersModel workers);

        bool NewWorkers(WorkersModel workers);

    }
}
