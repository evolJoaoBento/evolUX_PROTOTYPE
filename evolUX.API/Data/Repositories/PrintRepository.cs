using Dapper;
using SharedModels.Models.Areas.Finishing;
using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;
using System.Data;
using System.Data.SqlClient;
using evolUX.API.Extensions;

namespace evolUX.API.Data.Repositories
{
    public class PrintRepository : IPrintRepository
    {
        private readonly DapperContext _context;
        public PrintRepository(DapperContext context)
        {
            _context = context;
        }


        //SUM TOTAL AND DYNAMICS
        //FOR REFERENCE https://www.faqcode4u.com/faq/530844/dapper-mapping-dynamic-pivot-columns-from-stored-procedure
        //              https://stackoverflow.com/questions/8229927/looping-through-each-element-in-a-datarow
        public async Task<IEnumerable<ResourceInfo>> GetPrinters(IEnumerable<int> profileList, string filesSpecs, bool ignoreProfiles)
        {
            string sql = @"evolUX_RESOURCE_BY_FILTER";
            /*
             *                 -- 'RECOVER' - Recuperações
                               -- 'RDRECOVER' - RegistDetail recuperações
                               -- 'EXPEDITION' - Guias de Expedição
             */
            var parameters = new DynamicParameters();
            parameters.Add("ProfileList", profileList.toDataTable().AsTableValuedParameter("IDlist"));
            parameters.Add("ResName", "PRINTER", DbType.String);
            parameters.Add("ResValueFilter", filesSpecs, DbType.String);
            parameters.Add("IgnoreProfiles", ignoreProfiles, DbType.Boolean);

            using (var connection = _context.CreateConnectionEvolFlow())
            {
                //pass all servicecompany runid

                IEnumerable<ResourceInfo> printers = await connection.QueryAsync<ResourceInfo>(sql, parameters, commandType: CommandType.StoredProcedure);
                return printers.Where(printers => printers.MatchFilter == true);
            }
        }

        public async Task<FlowInfo> GetFlowID(string serviceCompanyCode)
        {
            string sql = @" SELECT f.FlowID, f.DefaultPriority Priority
                            FROM FLOWS f WITH(NOLOCK)
                            INNER JOIN
                                FLOWS_CRITERIA fType WITH(NOLOCK)
                            ON fType.FlowID = f.FlowID AND fType.CriteriaName = 'TYPE'
                            INNER JOIN
                                FLOWS_CRITERIA fServiceCompany WITH(NOLOCK)
                            ON fServiceCompany.FlowID = f.FlowID AND fServiceCompany.CriteriaName = 'SERVICECOMPANYCODE'
                            WHERE f.[Enable] = 1 -- Fluxo Ativo
                               AND fType.CriteriaValue = 'PRINT'
                            AND fServiceCompany.CriteriaValue = '@ServiceCompanyCode'
                            ORDER BY f.FlowID ASC";
            /*
             *                 -- 'RECOVER' - Recuperações
                               -- 'RDRECOVER' - RegistDetail recuperações
                               -- 'EXPEDITION' - Guias de Expedição
             */
            var parameters = new DynamicParameters();
            parameters.Add("ServiceCompanyCode", serviceCompanyCode, DbType.String);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                //pass all servicecompany runid

                FlowInfo flowInfo = await connection.QueryFirstOrDefaultAsync<FlowInfo>(sql, parameters);
                return flowInfo;
            }
        } 
        public async Task<IEnumerable<FlowParameters>> GetFlowParameters(int flowID)
        {
            string sql = @" SELECT  FlowID, ParameterName, ParameterValue
                            FROM    FLOWS_DATA
                            WHERE   FlowID = @FlowID
                            ";
            var parameters = new DynamicParameters();
            parameters.Add("FlowID", flowID, DbType.Int32);

            using (var connection = _context.CreateConnectionEvolDP())
            {
                //pass all servicecompany runid

                IEnumerable<FlowParameters> flowParameters = await connection.QueryAsync<FlowParameters>(sql, parameters);
                return flowParameters;
            }
        }
    }
}
