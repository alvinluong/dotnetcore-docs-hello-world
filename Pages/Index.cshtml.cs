using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;
using System;
using System.DirectoryServices;
using System.Web.UI;

namespace dotnetcoresample.Pages;

public class IndexModel : PageModel
{

   public partial class Index : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if the user is already authenticated
                if (User.Identity.IsAuthenticated)
                {
                    // User is authenticated; you can redirect them to a secure page.
                    Response.Redirect("SecurePage.aspx");
                }
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string ldapServer = "ldap://10.8.3.15:389"; // LDAP server address
            string ldapBaseDN = "DC=ladbs,DC=ci,DC=la,DC=ca,DC=us"; // Base DN for your LDAP directory
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Text;

            using (DirectoryEntry entry = new DirectoryEntry(ldapServer + "/" + ldapBaseDN, username, password))
            {
                try
                {
                    // Attempt to bind to the LDAP server with the provided credentials
                    DirectorySearcher searcher = new DirectorySearcher(entry);
                    searcher.Filter = "(sAMAccountName=" + username + ")";
                    SearchResult result = searcher.FindOne();

                    if (result != null)
                    {
                        // Authentication successful
                        FormsAuthentication.RedirectFromLoginPage(username, false);
                    }
                    else
                    {
                        // Authentication failed
                        ErrorLabel.Text = "Authentication failed. Please check your username and password.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorLabel.Text = "An error occurred: " + ex.Message;
                }
            }
        }
    }
}
