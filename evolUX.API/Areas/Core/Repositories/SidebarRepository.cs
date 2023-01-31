using Dapper;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Data.Context;

namespace evolUX.API.Areas.Core.Repositories
{
    public class SidebarRepository : ISidebarRepository
    {
        private readonly DapperContext _context;
        public SidebarRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> GetSidebar()
        {
            string sql = @"SELECT * FROM ng_sidebar SB LEFT JOIN ng_innerSidebar ISB ON SB.id = ISB.sidebarId WHERE SB.active = 1";
            var sidebarItems = new List<dynamic>();
            using (var connection = _context.CreateConnectionEvolFlow())
            {
                var sidebarDictionary = new Dictionary<int, dynamic>();
                sidebarItems = (List<dynamic>)await connection.QueryAsync<dynamic, dynamic, dynamic>(sql,
                    map: (SB, ISB) =>
                    {
                        if (!sidebarDictionary.ContainsKey(SB.id))
                        {
                            sidebarDictionary[SB.id] = new
                            {
                                Id = SB.id,
                                Title = SB.title,
                                TranslateTitle = SB.translateTitle,
                                RouterPrefix = SB.routerPrefix,
                                Theme = SB.theme,
                                SelectedTheme = SB.selectedTheme,
                                LinkTheme = SB.linkTheme,
                                Links = new List<dynamic>()
                            };
                        }
                        if (ISB is not null)
                        {
                            sidebarDictionary[SB.id].Links.Add(new
                            {
                                Id = ISB.id,
                                SidebarId = ISB.sidebarId,
                                Text = ISB.text,
                                TranslateText = ISB.translateText,
                                RouterLink = ISB.routerLink,
                                Active = ISB.active
                            });
                        }
                        return sidebarDictionary[SB.id];
                    }, splitOn: "id");
                var results = sidebarItems.Distinct().ToList();
                return results;
            }
        }

    }
}
