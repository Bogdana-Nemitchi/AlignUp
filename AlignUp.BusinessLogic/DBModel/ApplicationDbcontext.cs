using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlignUp.BusinessLogic.DBModel
{

    public class URegisterData
    {
    }
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() :
            base("name=AlignUpConnection")
        {
            // Disable initializer to prevent automatic database creation
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public DbSet<URegisterData> URegisterData { get; set; }
    }
    }