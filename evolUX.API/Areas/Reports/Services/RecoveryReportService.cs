using evolUX.API.Areas.Reports.Services.Interfaces;
using Shared.ViewModels.Areas.Reports;
using Shared.Models.Areas.Reports;
using System.Data;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using Shared.Models.Areas.Core;
using System.Xml;
using Shared.Models.General;
using evolUX.API.Models;
using System.Collections.Generic;
using Shared.ViewModels.Areas.Finishing;

namespace evolUX.API.Areas.Reports.Services
{
    public class RecoveryReportService
    {
        private readonly IWrapperRepository _repository;
        public RecoveryReportService(IWrapperRepository repository)
        {
            _repository = repository;
        }
    }
}
