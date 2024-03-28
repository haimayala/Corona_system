using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Corona_system.Models;

namespace Corona_system.Data
{
    public class Corona_systemContext : DbContext
    {
        public Corona_systemContext (DbContextOptions<Corona_systemContext> options)
            : base(options)
        {
        }

        public DbSet<Corona_system.Models.client> client { get; set; } = default!;

        public DbSet<Corona_system.Models.vaccination>? vaccination { get; set; }
    }
}
