using eRaceSystem.DAL;
using eRaceSystem.DataModels;
using eRaceSystem.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.BLL
{
    [DataObject]
    public class eRaceController
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<EmployeePositions> ListEmployeeAndPosition(string emailDomain)
        {
            using (var context = new eRaceContext())
            {
                var results = from employees in context.Employees.Include(nameof(Position)).ToList()
                              orderby employees.Position.Description
                              select new EmployeePositions
                              {
                                  UserID = employees.EmployeeID,
                                  UserName = $"{employees.FirstName}.{employees.LastName}",
                                  Title = employees.Position.Description,
                                  EmailAddress = $"{employees.FirstName}.{employees.LastName}@{emailDomain}"
                              };

                return results;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<SetupPositions> ListPositions()
        {
            using (var context = new eRaceContext())
            {

                    var results = from positions in context.Positions.ToList()
                                  select new SetupPositions
                                  {
                                      Position = positions.Description
                                  };
                return results;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public StoreEmployees GetStoreEmployees (int EmployeeId)
        {
            using (var context = new eRaceContext())
            {
                var result = from people in context.Employees
                             where people.EmployeeID.Equals(EmployeeId)
                             select new  StoreEmployees
                             {
                                 EmployeeID = people.EmployeeID,
                                 FullName = people.FirstName + " " + people.LastName
                             };
                return result.First();
            }
        }


        internal List<Employee> ListEmployees()
        {

            using (var context = new eRaceContext())
            {
                return context.Employees.ToList();
            }
        }
    }
}
