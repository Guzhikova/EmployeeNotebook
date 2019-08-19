using EmployeeNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;

namespace EmployeeNotebook
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static Cache theCache;

        Employee employee = new Employee();

        protected void Application_Start()
        {
            //Loads the list of employees from xml-file and  saves it to the cache
            theCache = Context.Cache;
            employee = new Employee();
            List<Employee> employeesData = employee.LoadEmployeesFromXML();
            theCache.Insert("EmployeesData", employeesData);


            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }      
        
    }
}
