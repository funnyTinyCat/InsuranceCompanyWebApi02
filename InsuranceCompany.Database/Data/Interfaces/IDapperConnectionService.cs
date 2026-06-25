using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Database.Data.Interfaces
{
    public interface IDapperConnectionService
    {
        SqlConnection GetConnection();
    }
}
