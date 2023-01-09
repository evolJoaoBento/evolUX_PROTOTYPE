using evolUX.API.Data.Context;
using evolUX.API.Data.Interfaces;

namespace evolUX.API.Data.Repositories
{
    public class WrapperRepository : IWrapperRepository
    {
        private readonly DapperContext _context;
        private IEnvelopeMediaRepository _envelopeMedia;
        private IExpeditionCompaniesRepository _expeditionCompanies;
        private IExpeditionTypeRepository _expeditionType;
        private IExpeditionZoneRepository _expeditionZone;
        private IFinishingRepository _finishing;
        private IUserRepository _user;
        private ISidebarRepository _sidebar;
        private IDocCodeRepository _docCode;
        private IProductionReportRepository _productionReport;
        private IPrintFilesRepository _printFiles;
        private ISessionRepository _session;
        private IPrintedFilesRepository _printedFiles;
        private IFullfilledFilesRepository _fullfilledFiles;
        private IRecoverRepository _recover;
        private IPendingRegistriesRepository  _pendingRegistriesRepository;


        public WrapperRepository(DapperContext context)
        {
            _context = context;
        }


        public IEnvelopeMediaRepository EnvelopeMedia
        {
            get
            {
                if (_envelopeMedia == null)
                {
                    _envelopeMedia = new EnvelopeMediaRepository(_context);
                }
                return _envelopeMedia;
            }
        }
        public IExpeditionCompaniesRepository ExpeditionCompanies
        {
            get
            {
                if (_expeditionCompanies == null)
                {
                    _expeditionCompanies = new ExpeditionCompaniesRepository(_context);
                }
                return _expeditionCompanies;
            }
        }

        public IExpeditionTypeRepository ExpeditionType
        {
            get
            {
                if (_expeditionType == null)
                {
                    _expeditionType = new ExpeditionTypeRepository(_context);
                }
                return _expeditionType;
            }
        }

        public IExpeditionZoneRepository ExpeditionZone
        {
            get
            {
                if (_expeditionZone == null)
                {
                    _expeditionZone = new ExpeditionZoneRepository(_context);
                }
                return _expeditionZone;
            }
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
                if(_docCode == null)
                {
                    _docCode = new DocCodeRepository(_context);
                }
                return _docCode;
            }
        }
        
        public IProductionReportRepository ProductionReport
        {
            get
            {
                if(_productionReport == null)
                {
                    _productionReport = new ProductionReportRepository(_context);
                }
                return _productionReport;
            }
        }
        
        public IPrintFilesRepository PrintFiles
        {
            get
            {
                if(_printFiles == null)
                {
                    _printFiles = new PrintFilesRepository(_context);
                }
                return _printFiles;
            }
        }

        public ISessionRepository Session
        {
            get
            {
                if(_session == null)
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
                if(_printedFiles == null)
                {
                    _printedFiles = new PrintedFilesRepository(_context);
                }
                return _printedFiles;
            }
        }
        public IFullfilledFilesRepository FullfilledFiles
        {
            get
            {
                if(_fullfilledFiles == null)
                {
                    _fullfilledFiles = new FullfilledFilesRepository(_context);
                }
                return _fullfilledFiles;
            }
        }
        
        public IRecoverRepository Recover
        {
            get
            {
                if(_recover == null)
                {
                    _recover = new RecoverRepository(_context);
                }
                return _recover;
            }
        }
        public IPendingRegistriesRepository PendingRegistries
        {
            get
            {
                if(_pendingRegistriesRepository == null)
                {
                    _pendingRegistriesRepository = new PendingRegistriesRepository(_context);
                }
                return _pendingRegistriesRepository;
            }
        }



    }
}
