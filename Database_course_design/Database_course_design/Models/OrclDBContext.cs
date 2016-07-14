using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Database_course_design.Models
{
    public class OrclDBContext : DbContext
    {
        public OrclDBContext()
            :base("OracleDbContext")
        {

        }
        public DbSet<KUXIANGDATAEntities> entities { get; set; }
    }
}