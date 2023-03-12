using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.evolDP.Repositories;
using evolUX.API.Areas.evolDP.Repositories.Interfaces;
using evolUX.API.Areas.Finishing.Repositories;
using evolUX.API.Areas.Finishing.Repositories.Interfaces;
using evolUX.API.Data.Context;

namespace evolUX.API.Areas.Core.Repositories
{
    public class WrapperRepository : IWrapperRepository
    {
        private readonly DapperContext _context;
        private IFinishingRepository _finishing;
        private IUserRepository _user;
        private ISidebarRepository _sidebar;
        private IDocCodeRepository _docCode;
        private IGenericRepository _generic;
        private IExpeditionRepository _expeditionType;
        private IConsumablesRepository _consumables;
        private IServiceProvisionRepository _serviceProvision;

        private IProductionReportRepository _productionReport;
        private IRegistJobRepository _registJob;
        private ISessionRepository _session;
        private IPrintedFilesRepository _printedFiles;
        private IFulfiledFilesRepository _fullfilledFiles;
        private IRecoverRepository _recover;
        private IPendingRegistRepository _pendingRegistRepository;
        private IPostalObjectRepository _postalObjectRepository;
        private IPendingRecoverRepository _pendingRecoverRepository;
        private IExpeditionReportRepository _expeditionReportRepository;


        public WrapperRepository(DapperContext context)
        {
            _context = context;
        }

        public IFinishingRepository Finishing
        {
            get
            {
                if (_finishing == null)
                {
                    _finishing = new FinishingRepository(_context);
                }
                return _finishing;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return _user;
            }
        }

        public ISidebarRepository Sidebar
        {
            get
            {
                if (_sidebar == null)
                {
                    _sidebar = new SidebarRepository(_context);
                }
                return _sidebar;
            }
        }

        public IDocCodeRepository DocCode
        {
            get
            {
                if (_docCode == null)
                {
                    _docCode = new DocCodeRepository(_context);
                }
                return _docCode;
            }
        }
        public IGenericRepository Generic
        {
            get
            {
                if (_generic == null)
                {
                    _generic = new GenericRepository(_context);
                }
                return _generic;
            }
        }

        public IConsumablesRepository Consumables
        {
            get
            {
                if (_consumables == null)
                {
                    _consumables = new ConsumablesRepository(_context);
                }
                return _consumables;
            }
        }
        public IExpeditionRepository ExpeditionType
        {
            get
            {
                if (_expeditionType == null)
                {
                    _expeditionType = new ExpeditionRepository(_context);
                }
                return _expeditionType;
            }
        }
       
        public IServiceProvisionRepository ServiceProvision
        {
            get
            {
                if (_serviceProvision == null)
                {
                    _serviceProvision = new ServiceProvisionRepository(_context);
                }
                return _serviceProvision;
            }
        }
        public IProductionReportRepository ProductionReport
        {
            get
            {
                if (_productionReport == null)
                {
                    _productionReport = new ProductionReportRepository(_context);
                }
                return _productionReport;
            }
        }

        public IRegistJobRepository RegistJob
        {
            get
            {
                if (_registJob == null)
                {
                    _registJob = new RegistJobRepository(_context);
                }
                return _registJob;
            }
        }

        public ISessionRepository Session
        {
            get
            {
                if (_session == null)
                {
                    _session = new SessionRepository(_context);
                }
                return _session;
            }
        }

        public IPrintedFilesRepository PrintedFiles
        {
            get
            {
                if (_printedFiles == null)
                {
                    _printedFiles = new PrintedFilesRepository(_context);
                }
                return _printedFiles;
            }
        }
        public IFulfiledFilesRepository FullfilledFiles
        {
            get
            {
                if (_fullfilledFiles == null)
                {
                    _fullfilledFiles = new FulfiledFilesRepository(_context);
                }
                return _fullfilledFiles;
            }
        }

        public IRecoverRepository Recover
        {
            get
            {
                if (_recover == null)
                {
                    _recover = new RecoverRepository(_context);
                }
                return _recover;
            }
        }
        public IPendingRegistRepository PendingRegist
        {
            get
            {
                if (_pendingRegistRepository == null)
                {
                    _pendingRegistRepository = new PendingRegistRepository(_context);
                }
                return _pendingRegistRepository;
            }
        }
        public IPostalObjectRepository PostalObject
        {
            get
            {
                if (_postalObjectRepository == null)
                {
                    _postalObjectRepository = new PostalObjectRepository(_context);
                }
                return _postalObjectRepository;
            }
        }
        public IPendingRecoverRepository PendingRecover
        {
            get
            {
                if (_pendingRecoverRepository == null)
                {
                    _pendingRecoverRepository = new PendingRecoverRepository(_context);
                }
                return _pendingRecoverRepository;
            }
        }

        public IExpeditionReportRepository ExpeditionReport
        {
            get
            {
                if (_expeditionReportRepository == null)
                {
                    _expeditionReportRepository = new ExpeditionReportRepository(_context);
                }
                return _expeditionReportRepository;
            }
        }
    }
}
