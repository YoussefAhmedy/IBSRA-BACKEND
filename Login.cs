using System;
using System.Web.UI;

namespace UserLogin
{
    public partial class Login : Page
    {
        private DatabaseHelper db;

        protected void Page_Load(object sender, EventArgs e)
        {
            db = new DatabaseHelper();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            
            if (string.IsNullOrEmpty(phone))
            {
                ShowMessage("Please enter phone number", "error");
                return;
            }

            try
            {
                User user = db.LoginUser(phone);
                
                if (user != null)
                {
                    Session["CurrentUser"] = user;
                    ShowUserInfo(user);
                    ShowMessage("Login successful", "success");
                }
                else
                {
                    ShowMessage("User not found", "error");
                    HideUserInfo();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "error");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string ageText = txtAge.Text.Trim();
            string phone = txtPhone.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(name))
            {
                ShowMessage("Please enter name", "error");
                return;
            }

            if (string.IsNullOrEmpty(ageText) || !int.TryParse(ageText, out int age))
            {
                ShowMessage("Please enter valid age", "error");
                return;
            }

            if (string.IsNullOrEmpty(phone))
            {
                ShowMessage("Please enter phone number", "error");
                return;
            }

            try
            {
                User newUser = db.RegisterUser(name, age, phone);
                
                if (newUser != null)
                {
                    Session["CurrentUser"] = newUser;
                    ShowUserInfo(newUser);
                    ShowMessage("Registration successful", "success");
                    ClearForm();
                }
                else
                {
                    ShowMessage("User already exists", "error");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "error");
            }
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.CssClass = $"message {type}";
        }

        private void ShowUserInfo(User user)
        {
            pnlUserInfo.Visible = true;
            lblUserInfo.Text = $@"
                <strong>ID:</strong> {user.ID}<br/>
                <strong>Name:</strong> {user.Name}<br/>
                <strong>Age:</strong> {user.Age}<br/>
                <strong>Phone:</strong> {user.PhoneNumber}<br/>
                <strong>Created:</strong> {user.CreatedAt:yyyy-MM-dd HH:mm:ss}
            ";
        }

        private void HideUserInfo()
        {
            pnlUserInfo.Visible = false;
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtPhone.Text = "";
        }
    }
}
