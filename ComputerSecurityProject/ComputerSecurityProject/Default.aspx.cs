using ComputerSecurityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComputerSecurityProject
{
    public partial class _Default : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            using (var context = new Entities())
            {
                StringBuilder htmlTable = new StringBuilder();
                htmlTable.AppendLine("<table style=\"width: 100%\">");

                htmlTable.AppendLine("<tr>");
                htmlTable.AppendLine("<th>Customer Name</th>");
                htmlTable.AppendLine("<th>Customer Email</th>");
                htmlTable.AppendLine("<th>Customer Id</th>");
                htmlTable.AppendLine("</tr>");

                var customers = context.Customers.ToList();
                foreach (var customer in customers)
                {
                    htmlTable.AppendLine("<tr>");
                    htmlTable.AppendLine("<td>"+ customer.CustomerName + "</td>");
                    htmlTable.AppendLine("<td>" + customer.CustomerEmail + "</td>");
                    htmlTable.AppendLine("<td>" + customer.CustomerId + "</td>");
                    htmlTable.AppendLine("</tr>");
                }
                
                htmlTable.AppendLine("</table>");

                TableLiteral.Text = htmlTable.ToString();
            }
        }

        protected void AddCustomer(object sender, EventArgs e)
        {
            var email = Request.Form["inputEmail"];
            var id = int.Parse(Request.Form["inputCustomerId"]);
            var name = Request.Form["inputCustomerName"];

            using (var context = new Entities())
            {
                var customer = new Customer() {
                    CustomerId = id,
                    CustomerName = name,
                    CustomerEmail = email
                };
                context.Customers.Add(customer);
                context.SaveChanges();
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void OnTextChanged(object sender, EventArgs e)
        { //'; delete from customers where CustomerName='asdc
            var name = Request.Form["searchCustomerInput"];

            using (var context = new Entities())
            {
                // The next line if for sql injection purposes 
                //var customers = context.Customers.SqlQuery("select * FROM Customers where CustomerName = '" + name + "'").ToList<Customer>();
                var customers = context.Customers.Where(c => c.CustomerName.Contains(name)).ToList();
                StringBuilder htmlTable = new StringBuilder();
                htmlTable.AppendLine("<table style=\"width: 100%\">");

                htmlTable.AppendLine("<tr>");
                htmlTable.AppendLine("<th>Customer Name</th>");
                htmlTable.AppendLine("<th>Customer Email</th>");
                htmlTable.AppendLine("<th>Customer Id</th>");
                htmlTable.AppendLine("</tr>");

                foreach (var customer in customers)
                {
                    htmlTable.AppendLine("<tr>");
                    htmlTable.AppendLine("<td>" + customer.CustomerName + "</td>");
                    htmlTable.AppendLine("<td>" + customer.CustomerEmail + "</td>");
                    htmlTable.AppendLine("<td>" + customer.CustomerId + "</td>");
                    htmlTable.AppendLine("</tr>");
                }

                htmlTable.AppendLine("</table>");

                TableLiteral.Text = htmlTable.ToString();
            }
        }
    }
}