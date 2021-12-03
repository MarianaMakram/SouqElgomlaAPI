using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SouqElgomlaAPI
{
    public class SouqElgomlaDbContextFactory : IDesignTimeDbContextFactory<SouqElgomlaContext>
    {
        public SouqElgomlaContext CreateDbContext(string[] args)
        {
            /**To read connection string to database and create context object*/

            IConfigurationRoot configurationRoot =
                new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            DbContextOptionsBuilder<SouqElgomlaContext> Builder = new DbContextOptionsBuilder<SouqElgomlaContext>();

            Builder.UseSqlServer(configurationRoot.GetConnectionString("SouqElGomla"));

            SouqElgomlaContext context = new SouqElgomlaContext(Builder.Options);

            return context;
        }
    }
}
