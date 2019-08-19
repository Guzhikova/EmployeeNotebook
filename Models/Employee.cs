using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace EmployeeNotebook.Models
{
    public class Employee
    {
        [Required(ErrorMessage = "Пожалуйста, введите фамилию сотрудника")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите имя сотрудника")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите год рождения сотрудника")]

        [Range(1920, 2019, ErrorMessage = "Недопустимый год рождения")]
        public int YearOfBirth { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите телефон сотрудника")]
        [RegularExpression(@"\d{10}", ErrorMessage = "Вы ввели некорректный номер телефона")]
        public string Phone { get; set; }



        //Load information about employees from XML-file to collection 
        public List<Employee> LoadEmployeesFromXML()
        {
            List<Employee> employeesList = new List<Employee>();
            XDocument doc = new XDocument();

            try
            {
                doc = XDocument.Load(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Employees.xml"));

                foreach (XElement empElement in doc.Element("Employees").Elements("Employee"))
                {
                    Employee employee = new Employee();

                    try
                    {
                        employee.Surname = empElement.Element("Surname").Value;
                        employee.Name = empElement.Element("Name").Value;

                        try
                        {
                            employee.YearOfBirth = Int32.Parse(empElement.Element("YearOfBirth").Value);
                        }
                        catch (FormatException e)
                        {
                            //write message 
                        }

                        employee.Phone = empElement.Element("Phone").Value;

                        employeesList.Add(employee);
                    }
                    catch (NullReferenceException e)
                    {
                        //write message 
                    }


                }
            }
            catch (Exception )
            { throw; }


            return employeesList;

        }

        //Save actual information about employees to XML-file
        public void SaveEmployeesToXML(List<Employee> employeesData)
        {
            try
            {
                var listDataAsXML = from c in employeesData
                                    select
                                    new XElement("Employee",
                                        new XElement("Surname", c.Surname),
                                        new XElement("Name", c.Name),
                                        new XElement("YearOfBirth", c.YearOfBirth),
                                        new XElement("Phone", c.Phone));

                XElement employeeDoc = new XElement("Employees", listDataAsXML);


                employeeDoc.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Employees.xml"));
            }
            catch (Exception)
            {

                throw;
            }

        }

        //Sort collection of employees by employees surname in ascending or descending order 
        public List<Employee> SortBySurname(List<Employee> employees, bool ascending)
        {
            List<Employee> sortedEmployees = new List<Employee>();

            try
            {
                var sortEmp = ascending ?
                               employees.OrderBy(emp => emp.Surname).ThenBy(emp => emp.Name) :
                               employees.OrderByDescending(emp => emp.Surname).ThenBy(emp => emp.Name);

                foreach (var emp in sortEmp)
                {
                    sortedEmployees.Add(emp);
                }

            }
            catch (Exception)
            {

                throw;
            }

            return sortedEmployees;
        }

        //Sort collection of employees by year of birth employee in ascending or descending order 
        public List<Employee> SortByYearOfBirth(List<Employee> employees, bool ascending)
        {
            List<Employee> sortedEmployees = new List<Employee>();

            try
            {
                var sortEmp = ascending ?
                                employees.OrderBy(emp => emp.YearOfBirth).ThenBy(emp => emp.Surname).ThenBy(emp => emp.Name) :
                                employees.OrderByDescending(emp => emp.YearOfBirth).ThenBy(emp => emp.Surname).ThenBy(emp => emp.Name);

                foreach (var emp in sortEmp)
                {
                    sortedEmployees.Add(emp);
                }
            }
            catch (Exception)
            {

                throw;
            }


            return sortedEmployees;
        }

        //Creates a list of employees with the required surname from the general list
        public List<Employee> SearchBySurname(List<Employee> employeesData, string surnamePart)
        {

            List<Employee> foundEmployees = new List<Employee>();


            try
            {
                IEnumerable<Employee> sequenceEmp = from emp in employeesData
                                                    where (emp.Surname.ToUpper()).Contains(surnamePart.ToUpper())
                                                    select emp;
                foreach (var emp in sequenceEmp)
                {
                    foundEmployees.Add(emp);
                }
            }
            catch (Exception)
            {

                throw;
            }


            return foundEmployees;
        }

        //Creates a list of employees with the required name from the general list
        public List<Employee> SearchByName(List<Employee> employees, string namePart)
        {
            List<Employee> foundEmployees = new List<Employee>();
            try
            {
                IEnumerable<Employee> sequenceEmp = from emp in employees
                                                    where (emp.Name.ToUpper()).Contains(namePart.ToUpper())
                                                    select emp;
                foreach (var emp in sequenceEmp)
                {
                    foundEmployees.Add(emp);
                }
            }
            catch (Exception)
            {

                throw;
            }




            return foundEmployees;
        }

        //Creates a list of employees with the required phone from the general list
        public List<Employee> SearchByPhone(List<Employee> employeesData, string phonePart)
        {
            //Regex regex = new Regex(namePart, RegexOptions.IgnoreCase);
            //regex.
            List<Employee> foundEmployees = new List<Employee>();

            try
            {
                IEnumerable<Employee> sequenceEmp = from emp in employeesData
                                                    where (emp.Phone.ToUpper()).Contains(phonePart.ToUpper())
                                                    select emp;
                foreach (var emp in sequenceEmp)
                {
                    foundEmployees.Add(emp);
                }
            }
            catch (Exception)
            {

                throw;
            }


            return foundEmployees;
        }

    }
}