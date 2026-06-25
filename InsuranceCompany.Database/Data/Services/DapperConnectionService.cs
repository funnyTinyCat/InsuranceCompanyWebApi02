using InsuranceCompany.Database.Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Database.Data.Services
{
    public class DapperConnectionService : IDapperConnectionService
    {
        private readonly IConfiguration _configuration;
        public DapperConnectionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DapperConnection"));
        }

    }
}
