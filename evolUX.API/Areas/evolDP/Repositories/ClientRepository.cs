using Dapper;
using Shared.Models.Areas.Finishing;
using Shared.Models.Areas.evolDP;
using evolUX.API.Data.Context;
using System.Data;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;

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
    }
}
