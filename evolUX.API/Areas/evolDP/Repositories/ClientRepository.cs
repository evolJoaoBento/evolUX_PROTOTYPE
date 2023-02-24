using Dapper;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Data.Context;
using Shared.Models.Areas.evolDP;
using System.Data;

namespace evolUX.API.Areas.evolDP.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DapperContext _context;
        public ClientRepository(DapperContext context)
        {
            _context = context;
        }

        //FOR REFERENCE https://stackoverflow.com/questions/33087629/dapper-dynamic-parameters-with-table-valued-parameters
        public async Task<IEnumerable<Business>> GetCompanyBusiness(DataTable CompanyBusinessList)
        {
            string sql = @"RD_UX_GET_BUSINESS_INFO";
            var parameters = new DynamicParameters();
            parameters.Add("CompanyBusinessList", CompanyBusinessList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<Business> companyBusiness = await connection.QueryAsync<Business>(sql, parameters, commandType: CommandType.StoredProcedure);
                return companyBusiness;
            }

        }

        public async Task<IEnumerable<ProjectElement>> GetProjects(DataTable CompanyBusinessList)
        {
            string sql = @"RDC_UX_GET_PROJECT_VERSIONS";
            var parameters = new DynamicParameters();
            parameters.Add("CompanyBusinessList", CompanyBusinessList.AsTableValuedParameter("IDlist"));

            using (var connection = _context.CreateConnectionEvolDP())
            {
                List<ProjectElement> projectsList = new List<ProjectElement>();

                connection.Open();
                var obs = await connection.QueryAsync(sql, parameters,
                                    commandType: CommandType.StoredProcedure);
                var dt = _context.ToDataTable(obs);
                if (dt != null)
                {
                    int lastBusinessID = -1;
                    string lastType = "";
                    string lastProjectCode = "";
                    ProjectElement projects = new ProjectElement();
                    Project project = new Project();
   
                    foreach (DataRow r in dt.Rows)
                    {
                        int businessID = (int)r["BusinessID"];
                        string type = (string)r["Type"];
                        if (lastBusinessID != businessID || lastType != type)
                        {
                            projects = new ProjectElement();
                            projectsList.Add(projects);

                            projects.BusinessID = businessID;
                            projects.BusinessCode = (string)r["BusinessCode"];
                            projects.BusinessDescription = (string)r["BusinessDescription"];
                            projects.Type = (string)r["Type"];
                            lastBusinessID = businessID;
                            lastType = type;
                        }
                        string projectCode = (string)r["ProjectCode"];
                        if (lastProjectCode != projectCode)
                        {
                            project = new Project();
                            projects.ProjectList.Add(project);

                            project.ProjectCode = projectCode;
                            lastProjectCode = projectCode;
                        }
                        
                        VersionElement version = new VersionElement();
                        project.VersionList.Add(version);
                        version.VersionID = (int)r["VersionID"];
                        version.StartDate = (int)r["StartDate"];
                        version.StartTime = (int)r["StartTime"];
                        version.Major = (int)r["Major"];
                        version.Minor = (int)r["Minor"];
                        version.Revision = (int)r["Revision"];
                        version.Patch = (int)r["Patch"];
                        version.Description = (string)r["Description"];
                    }
                }
                return projectsList;
            }
        }

        public async Task<IEnumerable<ConstantParameter>> GetParameters()
        {
            string sql = string.Format(@"SELECT	ParameterID,
									ParameterRef,
									ParameterValue,
									ParameterDescription
							FROM	RD_CONSTANT_PARAMETERS
							ORDER BY ParameterRef");

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ConstantParameter> constants = await connection.QueryAsync<ConstantParameter>(sql);
                return constants;
            }
        }

        public async Task<IEnumerable<ConstantParameter>> SetParameter(int parameterID, string parameterRef, int parameterValue, string parameterDescription)
        {
            string sql = "";
            var parameters = new DynamicParameters();
            parameters.Add("ParameterRef", parameterRef, DbType.String);
            parameters.Add("ParameterValue", parameterValue, DbType.Int64);
            parameters.Add("ParameterDescription", parameterDescription, DbType.String);

            if (parameterID == 0)
            {
                sql += string.Format(@"SET NOCOUNT ON
                            INSERT INTO RD_CONSTANT_PARAMETERS(ParameterID, ParameterRef, ParameterValue, ParameterDescription)
                            SELECT (SELECT ISNULL(MAX(ParameterID),0) + 1 FROM RD_CONSTANT_PARAMETERS), @ParameterValue, @ParameterDescription
                            WHERE NOT EXISTS (SELECT TOP 1 1 FROM RD_CONSTANT_PARAMETERS WITH(NOLOCK) WHERE ParameterRef = @ParameterRef)");
            }
            else
            {
                parameters.Add("ParameterID", parameterID, DbType.Int64);
                sql += string.Format(@"SET NOCOUNT ON
                            UPDATE RD_CONSTANT_PARAMETERS
                            SET ParameterRef = @ParameterRef, ParameterValue = @ParameterValue, ParameterDescription = @ParameterDescription
                            WHERE ParameterID = @ParameterID");
            }
            sql += string.Format(@"
                            SELECT	ParameterID,
									ParameterRef,
									ParameterValue,
									ParameterDescription
							FROM	RD_CONSTANT_PARAMETERS
							ORDER BY ParameterRef");

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ConstantParameter> constants = await connection.QueryAsync<ConstantParameter>(sql, parameters);
                return constants;
            }
        }

        public async Task<IEnumerable<ConstantParameter>> DeleteParameter(int parameterID)
        {
            string sql = "";
            var parameters = new DynamicParameters();
            parameters.Add("ParameterID", parameterID, DbType.Int64);

            sql += string.Format(@"SET NOCOUNT ON
                            DELETE RD_CONSTANT_PARAMETERS
                            WHERE ParameterID = @ParameterID");

            sql += string.Format(@"
                            SELECT	ParameterID,
									ParameterRef,
									ParameterValue,
								    ParameterDescription
							FROM	RD_CONSTANT_PARAMETERS
							ORDER BY ParameterRef");

            using (var connection = _context.CreateConnectionEvolDP())
            {
                IEnumerable<ConstantParameter> constants = await connection.QueryAsync<ConstantParameter>(sql, parameters);
                return constants;
            }

        }
    }
}
