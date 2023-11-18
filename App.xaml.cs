using FoodOrder.Data;
using FoodOrder.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace FoodOrder;

public partial class App : Application
{

    private void InitializeDatabase()
    {
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated(); // Creates the database and tables

            //Ensurin that roles exist
            if (!db.Roles.Any())
            {
                db.Roles.Add(new Role { RoleName = "Admin" });
                db.Roles.Add(new Role { RoleName = "Customer" });
                db.SaveChanges();
            }

            var adminExists = db.Users.Any(u => u.Username == "admin");
            if (!adminExists)
            {
                // Create a default admin user
                var hashedPassword = BCryptNet.HashPassword("admin"); // Hash the default password
                var adminRole = db.Roles.FirstOrDefault(r => r.RoleName == "Admin");
                db.Users.Add(new User { Username = "admin", Password = hashedPassword, Role = adminRole });
                db.SaveChanges();
            }
        }
    }

    public App()
	{
		InitializeComponent();
        InitializeDatabase();

        MainPage = new AppShell();
	}
}
