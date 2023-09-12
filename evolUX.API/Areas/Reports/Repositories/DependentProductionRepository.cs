using Dapper;
using Shared.Models.Areas.Reports;
using evolUX.API.Data.Context;
using System.Data;
using System.Data.SqlClient;
using evolUX.API.Extensions;
using evolUX.API.Areas.Reports.Repositories.Interfaces;
using evolUX.API.Models;
using System.Drawing;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using NLog.Targets;

namespace evolUX.API.Areas.Reports.Repositories
{
    public class DependentProductionRepository
    {
        private readonly DapperContext _context;
        public DependentProductionRepository(DapperContext context)
        {
            _context = context;
        }
    }
}
