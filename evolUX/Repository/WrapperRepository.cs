using evolUX.Context;
using evolUX.Interfaces;

namespace evolUX.Repository
{
    public class WrapperRepository : IWrapperRepository
    {
        private readonly DapperContext _context;
        private IEnvelopeMediaRepository _envelopeMedia;
        private IExpeditionTypeRepository _expeditionType;
        private IFinishingRepository _finishing;
        private IUserRepository _user;
        private ISidebarRepository _sidebar;


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

        public WrapperRepository(DapperContext context)
        {
            _context = context;
        }

    }
}
