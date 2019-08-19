using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using EmployeeNotebook.Models;


namespace EmployeeNotebook.Controllers
{
    public class HomeController : Controller
    {


        // GET: Home
        public ViewResult Index()
        {

            ViewBag.Employees = GetEmployeesFromCache();
            ViewBag.Hidden = "";
            return View();
        }

        //FORM of new employee

        [HttpGet]
        public ViewResult NewEmployeeForm()
        {
            return View();
        }

        //Validation form data and saving information about new employee to cache and xml-file
        [HttpPost]
        public ViewResult NewEmployeeForm(Employee newEmployee)
        {

            if (ModelState.IsValid)
            {
                Employee emp = new Employee();
                List<Employee> employeesData = new List<Employee>();

                try
                {
                    employeesData = GetEmployeesFromCache();
                    employeesData.Add(newEmployee);


                    HttpContext.Cache.Insert("EmployeesData", employeesData);
                    emp.SaveEmployeesToXML(employeesData);

                }
                catch (Exception)
                {

                    throw;
                }
                ViewBag.Employees = employeesData;
                return View("Index");

            }
            else
                return View();
        }

        //Sort table of employees (on home page) by employees surname in ascending or descending order 
        public ViewResult SortBySurname(bool ascending)
        {
            Employee emp = new Employee();
            ViewBag.Employees = emp.SortBySurname(GetEmployeesFromCache(), ascending);

            return View("Index");
        }

        //Sort table of employees (on home page) by year of birth in ascending or descending order 
        public ViewResult SortByYear(bool ascending)
        {
            Employee emp = new Employee();
            ViewBag.Employees = emp.SortByYearOfBirth(GetEmployeesFromCache(), ascending);

            return View("Index");
        }

        //Delete information about selected employee from cache and xml-file
        public ViewResult DeleteEmployee(string surname, string name, int yearOfBirth, string phone)
        {
            Employee emp = new Employee();


            List<Employee> employeesData = GetEmployeesFromCache();

            var itemToDelete = employeesData.Where(empTemp => (empTemp.Surname == surname)
                                                            && (empTemp.Name == name)
                                                            && (empTemp.YearOfBirth == yearOfBirth)
                                                            && (empTemp.Phone == phone)).Select(empTemp => empTemp).First();

            employeesData.Remove(itemToDelete);

            emp.SaveEmployeesToXML(employeesData);
            HttpContext.Cache.Insert("EmployeesData", employeesData);
            ViewBag.Employees = employeesData;

            return View("Index");
        }

        //Display the table of employees with the required surname, name or phone
        [HttpPost]
        public ViewResult Search(string searchValue, string searchParameter)
        {
            Employee emp = new Employee();
            List<Employee> searchEmployees = new List<Employee>();


            switch (searchParameter)
            {
                case "surname":
                    searchEmployees = emp.SearchBySurname(GetEmployeesFromCache(), searchValue);
                    break;

                case "name":
                    searchEmployees = emp.SearchByName(GetEmployeesFromCache(), searchValue);
                    break;

                case "phone":
                    searchEmployees = emp.SearchByPhone(GetEmployeesFromCache(), searchValue);
                    break;
            }

            ViewBag.Hidden = "hidden";
            ViewBag.Employees = searchEmployees;

            return View("Index");
        }


        //Get collection of employees from cache
        private List<Employee> GetEmployeesFromCache()
        {
            List<Employee> empList = new List<Employee>();
            Cache theCache = HttpContext.Cache;

            if (theCache.Get("EmployeesData") != null)
            {
                empList = theCache.Get("EmployeesData") as List<Employee>;
            }

            return empList;
        }

    }
}