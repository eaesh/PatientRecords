using Backend.Models;
using Backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.PatientsController.Tests
{
    public class PatientsControllerTests
    {
        private readonly PatientRecordsContext _context;

        public PatientsControllerTests()
        {
            _context = BuildDB();
        }

        private PatientRecordsContext BuildDB()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PatientRecordsContext>();
            optionsBuilder.UseInMemoryDatabase("PatientRecords");

            return new PatientRecordsContext(optionsBuilder.Options);
        }

        private Backend.Controllers.PatientsController BuildController(PatientRecordsContext context) {
            return new Backend.Controllers.PatientsController(context);
        }

        #region PatientsController.GetPatients

        /// <summary>
        /// PatientsController.GetPatients() returns single record
        /// </summary>
        [Fact]
        public async Task GetPatient_ReturnSinglePatient()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var testData = new Patient {
                FirstName = "Foo",
                LastName = "Bar",
                Birthday = DateTime.Parse("2000-01-01"),
                Gender = Gender.Male
            };

            _context.Add(testData);
            _context.SaveChanges();

            using (var context = BuildDB())
            {
                // Act
                var controller = BuildController(context);
                var result = await controller.GetPatients();

                // Assert
                Assert.Single(result.Value);
                Assert.Equal("{ id = 1, firstName = Foo, lastName = Bar, birthday = 2000-01-01, gender = Male }",
                    result.Value.Single().ToString());
            }
        }

        #endregion

        #region PatientsController.PutPatient

        /// <summary>
        /// PatientsController.PutPatient() updates Gender at ID=1 
        /// </summary>
        [Fact]
        public async Task PutPatient_UpdatesSinglePatient()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.Patients.Add(new Patient
            {
                FirstName = "Foo",
                LastName = "Bar",
                Birthday = DateTime.Parse("2000-01-01"),
                Gender = Gender.Male
            });
            _context.SaveChanges();


            using (var context = BuildDB())
            {
                // Act
                var controller = BuildController(context);
                var result = await controller.PutPatient(1, "Foo,Bar,2000-01-01,F");

                // Assert
                Assert.IsType<OkResult>(result);
                Assert.Equal(Gender.Female, context.Patients.Single().Gender);
            }
        }

        /// <summary>
        /// PatientsController.PutPatient() returns ID not found
        /// </summary>
        [Fact]
        public async Task PutPatient_IDNotFound()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            using (var context = BuildDB())
            {
                // Act
                var controller = BuildController(context);
                var result = await controller.PutPatient(1, "Foo,Bar,2000-01-01,F");

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        /// <summary>
        /// PatientsController.PutPatient() fails due to incomplete record
        /// </summary>
        [Fact]
        public async Task PutPatient_IncompleteRecord()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.Patients.Add(new Patient
            {
                FirstName = "Foo",
                LastName = "Bar",
                Birthday = DateTime.Parse("2000-01-01"),
                Gender = Gender.Male
            });
            _context.SaveChanges();


            using (var context = BuildDB())
            {
                // Act
                var controller = BuildController(context);
                var result = await controller.PutPatient(1, "Foo,Bar,2000-20-10");

                // Assert
                ObjectResult objectResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.StartsWith("Incomplete record", objectResult.Value.ToString());
            }
        }


        /// <summary>
        /// PatientsController.PutPatient() fails at invalid birthday 
        /// </summary>
        [Fact]
        public async Task PutPatient_InvalidBirthday()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.Patients.Add(new Patient
            {
                FirstName = "Foo",
                LastName = "Bar",
                Birthday = DateTime.Parse("2000-01-01"),
                Gender = Gender.Male
            });
            _context.SaveChanges();


            using (var context = BuildDB())
            {
                // Act
                var controller = BuildController(context);
                var result = await controller.PutPatient(1, "Foo,Bar,2000-20-10,M");

                // Assert
                ObjectResult objectResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.StartsWith("Invalid birthday", objectResult.Value.ToString());
            }
        }

        /// <summary>
        /// PatientsController.PutPatient() fails at invalid gender 
        /// </summary>
        [Fact]
        public async Task PutPatient_InvalidGender()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.Patients.Add(new Patient
            {
                FirstName = "Foo",
                LastName = "Bar",
                Birthday = DateTime.Parse("2000-01-01"),
                Gender = Gender.Male
            });
            _context.SaveChanges();


            using (var context = BuildDB())
            {
                // Act
                var controller = BuildController(context);
                var result = await controller.PutPatient(1, "Foo,Bar,2000-01-01,X");

                // Assert
                ObjectResult objectResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.StartsWith("Invalid gender", objectResult.Value.ToString());
            }
        }

        #endregion

        #region PatientsController.UploadPatientsFile

        /// <summary>
        /// PatientsController.UploadPatientsFile() adds multiple patients
        /// </summary>
        [Fact]
        public async Task UploadPatientsFile_AddMultiplePatients()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();


            using (var context = BuildDB())
            {
                // Act
                var controller = BuildController(context);
                string file = "Clark,Kent,1984-02-29,M\nDiana, Prince, 1976 - 03 - 22, F\nTony, Stark, 1970 - 05 - 29, M\nCarol, Denvers, 1968 - 04 - 24, F";
                var result = await controller.UploadPatientsFile(file);

                // Assert
                Assert.IsType<OkResult>(result);
                Assert.True(context.Patients.Count() == 4);
            }
        }

        #endregion

    }
}
