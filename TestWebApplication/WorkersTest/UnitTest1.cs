using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Workers.Models;
using Microsoft.EntityFrameworkCore;
using Workers;
using Microsoft.Extensions.Logging;

namespace WorkersTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Create_Worker_Test()
        {
            // Arrange
            //Arrange
            DbContextOptions<WorkersContext> opt;
            var builder = new DbContextOptionsBuilder<WorkersContext>();
            builder.UseInMemoryDatabase(databaseName: "database_test");
            opt = builder.Options;

            var MoqContext = new WorkersContext(opt);
            var MoqRepo = new SQLrequest(MoqContext, new Logger<SQLrequest>(new LoggerFactory()));
            

            //Act
            MoqRepo.Create(new WorkersModel
            {
                Id = 1,
                Name = "Diana",
                Surname = "Vahomskaya",
                Email = "diana.vahomskaya@mail.ru",
                Login = "di_vahomik",
                Password = "123456789",
                Role = "Admin",
                Place = "לעס",
                Culture = "ru"
            });
            MoqRepo.Create(new WorkersModel
            {
                Id = 2,
                Name = "Mariya",
                Surname = "Polovinkina",
                Email = "polov@mail.ru",
                Login = "mapol",
                Password = "123456789",
                Role = "Worker",
                Place = "לודאפמם",
                Culture = "ru"
            });
            MoqRepo.Create(new WorkersModel
            {
                Id = 3,
                Name = "Sveta",
                Surname = "Svetlanovna",
                Email = "svet@mail.ru",
                Login = "svetsvet",
                Password = "123456789",
                Role = "Worker",
                Place = "לעס",
                Culture = "en"
            });

            var worker_delete = MoqRepo.GetWorkers(3);
            if (worker_delete != null) MoqRepo.Remove(worker_delete);

            if (MoqRepo.GetWorkers(2) != null)
            {
                var WorkerEdit = MoqRepo.GetWorkers(2);
                WorkerEdit.Name = "Dima";
                MoqRepo.Edit(WorkerEdit);
            }

            var worker = MoqRepo.GetWorkers_workers();
            var workerEdit = MoqRepo.GetWorkers(2);

            Assert.AreEqual(2, worker.Count());
            Assert.AreEqual("Dima", workerEdit.Name);
        }

        [Test]
        public void GetWorkers_Test()
        {
            //Arrange
            IQueryable<WorkersModel> worker = new List<WorkersModel>
            {
                new WorkersModel
                {
                Id=1,
                Name = "Diana",
                Surname = "Vahomskaya",
                Email = "diana.vahomskaya@mail.ru",
                Login="di_vahomik",
                Password ="123456789",
                Role = "Admin",
                Place="לעס",
                Culture="ru"
                }

            }.AsQueryable();

            var MoqSet = new Mock<DbSet<WorkersModel>>();
            MoqSet.As<IQueryable<WorkersModel>>().Setup(m => m.Provider).Returns(worker.Provider);
            MoqSet.As<IQueryable<WorkersModel>>().Setup(m => m.Expression).Returns(worker.Expression);
            MoqSet.As<IQueryable<WorkersModel>>().Setup(m => m.ElementType).Returns(worker.ElementType);
            MoqSet.As<IQueryable<WorkersModel>>().Setup(m => m.GetEnumerator()).Returns(worker.GetEnumerator());

            var MoqContext = new Mock<WorkersContext>();
            MoqContext.Setup(c => c.WorkersTable).Returns(MoqSet.Object);

            //Act
            var MoqRepo = new SQLrequest(MoqContext.Object, new Logger<SQLrequest>(new LoggerFactory()));
            var GetWorkers = MoqRepo.GetWorkers_workers();
            var GetWorkersId = MoqRepo.GetWorkers(1);
            var GetWorkersLogin = MoqRepo.GetLogin("di_vahomik");
            var result = MoqRepo.NewWorkers(new WorkersModel
            {
                Id=6,
                Name = "Diana",
                Surname = "Vahomskaya",
                Email = "diana.vahomskaya@mail.ru",
                Login = "diiiana",
                Password = "123f56789",
                Role = "Admin",
                Place = "לעס",
                Culture = "ru"
            });

            //Assert
            Assert.AreEqual(1, GetWorkers.Count());
            Assert.IsNotNull(GetWorkersId);
            Assert.IsNotNull(GetWorkersLogin);
            Assert.True(result);
        }

      
    }
}