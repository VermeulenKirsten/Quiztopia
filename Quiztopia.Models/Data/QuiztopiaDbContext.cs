using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Quiztopia.Models.Data
{
    public class QuiztopiaDbContext : IdentityDbContext<IdentityUser>
    {
        // Constructor

        public QuiztopiaDbContext(DbContextOptions<QuiztopiaDbContext> options)
            : base(options)
        {
        }

        // Properties


        // Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* modelBuilder.Entity<PersonsEducations>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.EducationId });
            }); */

        }
    }
}
