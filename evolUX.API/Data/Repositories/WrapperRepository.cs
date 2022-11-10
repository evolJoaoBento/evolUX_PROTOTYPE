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
        private IPrintRepository _print;
        private ISessionRepository _session;
        private IConcludedPrintRepository _concludedPrint;
        private IConcludedEnvelopeRepository _concludedEnvelope;
        private IRecuperationRepository _recuperation;


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
        
        public IPrintRepository Print
        {
            get
            {
                if(_print == null)
                {
                    _print = new PrintRepository(_context);
                }
                return _print;
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

        public IConcludedPrintRepository ConcludedPrint
        {
            get
            {
                if(_concludedPrint == null)
                {
                    _concludedPrint = new ConcludedPrintRepository(_context);
                }
                return _concludedPrint;
            }
        }
        public IConcludedEnvelopeRepository ConcludedEnvelope
        {
            get
            {
                if(_concludedEnvelope == null)
                {
                    _concludedEnvelope = new ConcludedEnvelopeRepository(_context);
                }
                return _concludedEnvelope;
            }
        }
        
        public IRecuperationRepository Recuperation
        {
            get
            {
                if(_recuperation == null)
                {
                    _recuperation = new RecuperationRepository(_context);
                }
                return _recuperation;
            }
        }



    }
}
