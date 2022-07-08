using ConstructSN.DAL.Infrastructure;
using ConstructSN.Shared.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructSN.DAL
{
    public class ClaimsRepository : ARepositoryMRC<ClaimAgainstCompany>
    {
        public ClaimsRepository(string connectionString, string databaseName, string nameCollection = "ClaimAgainstCompany") : base(connectionString, databaseName, nameCollection)
        {

        }
    }
}
