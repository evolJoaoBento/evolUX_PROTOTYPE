using evolUX.Context;
using evolUX.Interfaces;

namespace evolUX.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly DapperContext _context;
        private IEnvelopeMediaRepository _envelopeMedia;
        private IExpeditionTypeRepository _expeditionType;
        private IFinishingRepository _finishing;

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

        public RepositoryWrapper(DapperContext context)
        {
            _context = context;
        }

    }
}
