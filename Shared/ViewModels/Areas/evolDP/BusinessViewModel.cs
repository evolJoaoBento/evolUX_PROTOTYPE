﻿using Shared.Models.Areas.Core;
using Shared.Models.Areas.evolDP;

namespace Shared.ViewModels.Areas.evolDP
{
    public class BusinessViewModel: ItemPermissions
    {
        public IEnumerable<Business> CompanyBusiness { get; set; }
    }
}
