using CommunityCounts.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CommunityCounts
{

    public class MySqlInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            if (!context.Database.Exists())
            {
                // if database did not exist before - create it
                context.Database.Create();
                SetUpSystemAdmin(context); // set up system Admin role
              
            }
            else
            {
                // query to check if MigrationHistory table is present in the database
                var migrationHistoryTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
                string.Format(
                  "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '__MigrationHistory'",
                  "ccidentity"));

                // if MigrationHistory table is not there (which is the case first time we run) - create it
                if (migrationHistoryTableExists.FirstOrDefault() == 0)
                {
                    context.Database.Delete();
                    context.Database.Create();
                    SetUpSystemAdmin(context); // set up system Admin role
                 
                }
            }
        }
        bool SetUpSystemAdmin(CommunityCounts.Models.ApplicationDbContext context)
        {
            //
            // This seeds the master administrator account to the system
            // Note, as database initialisation is not checked until the database is needed (such as registering a new User),
            // This logon identity is not available until at least one other user is created.
            //
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("systemAdmin"));        // Manages whole system
            ir = rm.Create(new IdentityRole("superAdmin"));         // Manages whole system for a customer, inc Setup
            ir = rm.Create(new IdentityRole("canDeleteClient"));    // Can perform client deletes for a customer
            ir = rm.Create(new IdentityRole("canMarkAttendance"));  // Can mark attendance sheets (all can print)
            ir = rm.Create(new IdentityRole("canManageSurveys"));   // Can setup / manage surveys for a customer
            ir = rm.Create(new IdentityRole("canManageQuicks"));   // Can setup / manage Quick contacts for a customer
            ir = rm.Create(new IdentityRole("canManageCaseWork"));  // Allowed to use most caseworking functions (except Delete)
            ir = rm.Create(new IdentityRole("canManageNeeds"));     // Can fully access Client Needs
            ir = rm.Create(new IdentityRole("canDeleteCaseWork"));  // can delete any / all casework for a client
            ir = rm.Create(new IdentityRole("canManageSurveyResults"));   // Can setup / manage survey results for a customer
                                                                // All other (authorised) users can read, create, update for a customer.
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = "greg@nsfwd.co.uk",
                Email = "Greg@nsfwd.co.uk",
                PhoneNumber = "01926678637",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true
            };
            ir = um.Create(user, "Ssl3nab13d!");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "systemAdmin");
            return ir.Succeeded;
        }
    }
}