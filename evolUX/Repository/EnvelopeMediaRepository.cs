using Dapper;
using evolUX.Context;
using evolUX.Interfaces;

namespace evolUX.Repository
{
    public class EnvelopeMediaRepository : IEnvelopeMediaRepository
    {
        private readonly DapperContext _context;
        public EnvelopeMediaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> GetEnvelopeMedia()
        {
            var envelopeMediaList = new List<dynamic>();
            string sql = @"select EnvMediaID as [id],
	                        EnvMediaName as [name],
	                        [Description] as [description]
                            from RD_ENVELOPE_MEDIA";
            using (var connection = _context.CreateConnection())
            {
                envelopeMediaList =(List<dynamic>) await connection.QueryAsync(sql);
                return envelopeMediaList;
            }
        }

        public async Task<List<dynamic>> GetEnvelopeMediaGroups()
        {
            var envelopeMediaGroupList = new List<dynamic>();
            string sql = @"SELECT  
                            CAST(emg.EnvMediaGroupID as varchar) as [id], 
                            emg.[Description] as [description],
                            CAST(emg.DefaultEnvMediaID as varchar) as [defaultEnvMediaId],     
                            em.[Description] as [omission]
		                    FROM  RD_ENVELOPE_MEDIA_GROUP emg,
			                    RD_ENVELOPE_MEDIA em
		                    WHERE emg.DefaultEnvMediaID = em.EnvMediaID";
            using (var connection = _context.CreateConnection())
            {
                envelopeMediaGroupList = (List<dynamic>)await connection.QueryAsync<dynamic>(sql);
                return envelopeMediaGroupList;
            }

        }
    }
}
