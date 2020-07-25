using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Workers.Models;
using Workers.Service;

namespace Workers
{
    public class SQLrequest:BdBrain
    {
        private readonly ILogger<SQLrequest> _logger;
        public WorkersContext WorkersContext { get;}

        public SQLrequest(WorkersContext workersContext,ILogger<SQLrequest> logger)
        {
            WorkersContext = workersContext;
            _logger = logger;
            _logger.LogDebug("Data base");
        }

        public WorkersModel GetWorkers(int id)
        {
            _logger.LogInformation("Id of a worker", id);
            return WorkersContext.WorkersTable.FirstOrDefault(p => p.Id == id);
        }

        public WorkersModel GetLogin(string login)
        {
            _logger.LogInformation("Login of a worker", login);

            return WorkersContext.WorkersTable.FirstOrDefault(p => p.Login == login);
        }


        public IEnumerable<WorkersModel> GetWorkers_workers()
        {
            _logger.LogInformation("List of workers");
           return WorkersContext.WorkersTable.ToList();
        }
        public void Find(WorkersModel workers)
        {

        }
     
        public void Create(WorkersModel workers)
        {
            _logger.LogInformation("Data of workers recieved", nameof(workers));
            if (NewWorkers(workers))
            {
                WorkersContext.WorkersTable.Add(workers);
                WorkersContext.SaveChanges();
                _logger.LogInformation("Worker has been included in data base", nameof(workers));
            }
            _logger.LogInformation("Worker has not been included in data base", nameof(workers));
        }

        public void Edit(WorkersModel workers)
        {
            _logger.LogInformation("Data recieved", nameof(workers));
            WorkersContext.WorkersTable.Update(workers);
            WorkersContext.SaveChanges();
        }

        public void Remove(WorkersModel workers)
        {
            _logger.LogInformation("Data recieved", nameof(workers));
            WorkersContext.WorkersTable.Remove(workers);
            WorkersContext.SaveChanges();
        }

        public bool NewWorkers(WorkersModel workers)
        {
            _logger.LogInformation("Data recieved", nameof(workers));
            var count = 0;

            foreach (var item in GetWorkers_workers())
                if ((item.Login == workers.Login) && (item.Password == workers.Password)) ++count;

            return count > 0 ? throw new ArgumentException("Exist", nameof(workers)) : true;
        }

    }
}
