﻿using Flurl.Http;
using Shared.Models.Areas.evolDP;
using Shared.ViewModels.Areas.evolDP;

namespace evolUX.UI.Areas.evolDP.Repositories.Interfaces
{
    public interface IServiceProvisionRepository
    {
        public Task<IEnumerable<Company>> GetServiceCompanies(string serviceCompanyList);
        public Task<IEnumerable<Company>> GetServiceCompany(int serviceCompanyID);
        public Task<IEnumerable<ServiceCompanyRestriction>> GetServiceCompanyRestrictions(int serviceCompanyID);
        public Task<IEnumerable<ServiceCompanyServiceResume>> GetServiceCompanyConfigsResume(int serviceCompanyID);
        public Task<Company> SetServiceCompany(Company serviceCompany);
        public Task SetServiceCompanyRestriction(int serviceCompanyID, int materialTypeID, int materialPosition, int fileSheetsCutoffLevel, bool restrictionMode);
        public Task<IEnumerable<ServiceCompanyService>> GetServiceCompanyConfigs(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID);
        public Task SetServiceCompanyConfig(int serviceCompanyID, int costDate, int serviceTypeID, int serviceID, double serviceCost, string formula);
        public Task<IEnumerable<ServiceElement>> GetServices(int serviceTypeID);
        public Task<ServiceTypeViewModel> GetServiceTypes();
        public Task SetServiceType(int serviceTypeID, string serviceTypeCode, string serviceTypeDesc);
        public Task<IEnumerable<int>> GetServiceCompanyList(int serviceTypeID, int serviceID, int costDate);
        public Task<IEnumerable<ServiceTask>> GetServiceTasks(int? serviceTaskID);
    }
}
