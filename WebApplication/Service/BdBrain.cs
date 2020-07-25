using System.Collections.Generic;
using Workers.Models;

namespace Workers.Service
{
    public interface BdBrain
    {
       
        WorkersModel GetWorkers(int id);
        WorkersModel GetLogin(string log);
        IEnumerable<WorkersModel> GetWorkers_workers();

        void Create(WorkersModel workers);

        void Find(WorkersModel workers);
  
        void Edit(WorkersModel workers);

        void Remove(WorkersModel workers);

        bool NewWorkers(WorkersModel workers);

    }
}
