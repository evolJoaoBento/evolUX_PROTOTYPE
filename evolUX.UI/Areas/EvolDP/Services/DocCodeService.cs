using evolUX.API.Models;
using evolUX.UI.Areas.evolDP.Repositories.Interfaces;
using evolUX.UI.Areas.evolDP.Services.Interfaces;
using Flurl.Http;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Areas.evolDP;
using Shared.Models.General;
using Shared.ViewModels.Areas.evolDP;
using Shared.ViewModels.General;
using System.Data;

namespace evolUX.UI.Areas.evolDP.Services
{
    public class DocCodeService : IDocCodeService
    {
        private readonly IDocCodeRepository _docCodeRepository;
        public DocCodeService(IDocCodeRepository docCodeRepository)
        {
            _docCodeRepository = docCodeRepository;
        }
        public async Task<DocCodeViewModel> GetDocCodeGroup()
        {
            var response = await _docCodeRepository.GetDocCodeGroup();
            return response;
        }
        public async Task<DocCodeViewModel> GetDocCode(string docLayout, string docType)
        {
            var response = await _docCodeRepository.GetDocCode(docLayout, docType);
            return response;
        }
        public async Task<DocCodeConfigViewModel> GetDocCodeConfig(int docCodeID)
        {
            var response = await _docCodeRepository.GetDocCodeConfig(docCodeID);
            return response;
        }
        public async Task<DocCodeConfigOptionsViewModel> GetDocCodeConfigOptions(DocCode? docCode)
        {
            var response = await _docCodeRepository.GetDocCodeConfigOptions(docCode);
            return response;
        }
        public async Task<DocCodeConfigViewModel> RegistDocCodeConfig(DocCode docCode)
        {
            var response = await _docCodeRepository.RegistDocCodeConfig(docCode);
            return response;
        }
        public async Task<ExceptionLevelViewModel> GetExceptionLevel(int level)
        {
            var response = await _docCodeRepository.GetExceptionLevel(level);
            return response;
        }
        public async Task<ExceptionLevelViewModel> SetExceptionLevel(int level, int exceptionID, string exceptionCode, string exceptionDescription)
        {
            var response = await _docCodeRepository.SetExceptionLevel(level, exceptionID, exceptionCode, exceptionDescription);
            return response;
        }
        public async Task<ExceptionLevelViewModel> DeleteExceptionLevel(int level, int exceptionID)
        {
            var response = await _docCodeRepository.DeleteExceptionLevel(level, exceptionID);
            return response;
        }

        public async Task<DocCodeConfigViewModel> ChangeDocCode(DocCode docCode)
        {
            var response = await _docCodeRepository.ChangeDocCode(docCode);
            return response;
        }
        public async Task<ResultsViewModel> DeleteDocCodeConfig(int docCodeID, int startDate)
        {
            var response = await _docCodeRepository.DeleteDocCodeConfig(docCodeID, startDate);
            return response;
        }
        public async Task<ResultsViewModel> DeleteDocCode(DocCode docCode)
        {
            var response = await _docCodeRepository.DeleteDocCode(docCode);
            return response;
        }
        public async Task<DocCodeCompatibilityViewModel> GetCompatibility(int docCodeID)
        {
            var response = await _docCodeRepository.GetCompatibility(docCodeID);
            return response;
        }
        public async Task<DocCodeCompatibilityViewModel> ChangeCompatibility(int docCodeID, List<string> docCodeList)
        {
            var response = await _docCodeRepository.ChangeCompatibility(docCodeID, docCodeList);
            return response;
        }

        public async Task<List<string>> DocCodeData4Script(int docCodeID, int startDate)
        {
            DocCodeData4ScriptViewModel response = await _docCodeRepository.DocCodeData4Script(docCodeID, startDate);
            List<string> result = new List<string>();
            string script = "";
            string filename = "";
            if (response.Doc != null && !string.IsNullOrEmpty(response.Doc.DocLayout))
            {
                string ex1 = string.IsNullOrEmpty(response.Doc.ExceptionLevel1Code) ? "NULL" : string.Format("'{0}'", response.Doc.ExceptionLevel1Code.Replace("'", "''"));
                string ex2 = string.IsNullOrEmpty(response.Doc.ExceptionLevel2Code) ? "NULL" : string.Format("'{0}'", response.Doc.ExceptionLevel2Code.Replace("'", "''"));
                string ex3 = string.IsNullOrEmpty(response.Doc.ExceptionLevel3Code) ? "NULL" : string.Format("'{0}'", response.Doc.ExceptionLevel3Code.Replace("'", "''"));
                string pmSheets = response.Doc.ProdMaxSheets != null ? response.Doc.ProdMaxSheets.ToString() : "NULL";

                filename = string.Format("evolDP_{0}_{1}_{2}_{3}_{4}_{5:yyyyMMddHHmmss}.sql", 
                    response.Doc.DocLayout, response.Doc.DocType, 
                    ex1, ex2, ex3, DateTime.Now);
                script = @"
                    DECLARE @StartDate int
                    SELECT @StartDate = CAST(CONVERT(varchar(8),CURRENT_TIMESTAMP,112) as int)
                    BEGIN TRANSACTION";

                //RDC_EXCEPTION_LEVEL's
                foreach (ExceptionLevelScript e in response.ExceptionLevelList)
                {
                    script += string.Format(@"
                    INSERT INTO RDC_EXCEPTION_LEVEL{0}(ExceptionLevelID, ExceptionCode, ExceptionDescription)
                    SELECT (SELECT ISNULL(MAX(ExceptionLevelID),0) + 1 FROM RDC_EXCEPTION_LEVEL{0}), '{1}', '{2}'
                    WHERE NOT EXISTS (SELECT TOP 1 1 FROM RDC_EXCEPTION_LEVEL{0} WHERE ExceptionCode = '{1}')", 
                        e.Level,
                        e.ExceptionCode.Replace("'","''"),
                        e.ExceptionDescription.Replace("'", "''"));
                }

                //RD_DOCCODE
                script += string.Format(@"
                    DECLARE @DocCodeID int
                    SELECT @DocCodeID = DocCodeID
                    FROM RD_DOCCODE WITH(NOLOCK)
                    WHERE DocLayout = '{0}' AND DocType = '{1}' 
                        AND ISNULL(ExceptionLevel1ID,0) = ISNULL((SELECT TOP 1 ExceptionLevelID FROM RDC_EXCEPTION_LEVEL1 WHERE ExceptionCode = {2}),0)
                        AND ISNULL(ExceptionLevel2ID,0) = ISNULL((SELECT TOP 1 ExceptionLevelID FROM RDC_EXCEPTION_LEVEL2 WHERE ExceptionCode = {3}),0)
                        AND ISNULL(ExceptionLevel3ID,0) = ISNULL((SELECT TOP 1 ExceptionLevelID FROM RDC_EXCEPTION_LEVEL3 WHERE ExceptionCode = {4}),0)",
                        response.Doc.DocLayout, response.Doc.DocType, ex1, ex2, ex3);

                script += string.Format(@"
                    IF (@DocCodeID is NULL)
                    BEGIN
                        INSERT INTO RD_DOCCODE(DocCodeID, DocLayout, DocType, ExceptionLevel1ID, ExceptionLevel2ID, ExceptionLevel3ID, [Description], PrintMatchCode)
                        SELECT (SELECT ISNULL(MAX(DocCodeID),0) + 1 FROM RD_DOCCODE), '{0}', '{1}', 
                            (SELECT ExceptionLevelID FROM RDC_EXCEPTION_LEVEL1 WHERE ExceptionCode = {2}),
                            (SELECT ExceptionLevelID FROM RDC_EXCEPTION_LEVEL2 WHERE ExceptionCode = {3}),
                            (SELECT ExceptionLevelID FROM RDC_EXCEPTION_LEVEL3 WHERE ExceptionCode = {4}),
                            '{5}',
                            '{6}'

                        SELECT @DocCodeID = DocCodeID
                        FROM RD_DOCCODE WITH(NOLOCK)
                        WHERE DocLayout = '{0}' AND DocType = '{1}' 
                            AND ISNULL(ExceptionLevel1ID,0) = ISNULL((SELECT TOP 1 ExceptionLevelID FROM RDC_EXCEPTION_LEVEL1 WHERE ExceptionCode = {2}),0) 
                            AND ISNULL(ExceptionLevel2ID,0) = ISNULL((SELECT TOP 1 ExceptionLevelID FROM RDC_EXCEPTION_LEVEL2 WHERE ExceptionCode = {3}),0) 
                            AND ISNULL(ExceptionLevel3ID,0) = ISNULL((SELECT TOP 1 ExceptionLevelID FROM RDC_EXCEPTION_LEVEL3 WHERE ExceptionCode = {4}),0)
                    END
                    ELSE
                    BEGIN
                        UPDATE RD_DOCCODE
                        SET [Description] = '{5}', PrintMatchCode = '{6}'
                        WHERE DocCodeID = @DocCodeID
                    END", 
                    response.Doc.DocLayout.Replace("'", "''"),
                    response.Doc.DocType.Replace("'", "''"),
                    ex1, ex2, ex3,
                    response.Doc.DocDescription.Replace("'", "''"),
                    response.Doc.PrintMatchCode.Replace("'", "''"));

                //RD_DOCCODE_CONFIG
                script += string.Format(@"
                    INSERT INTO RD_DOCCODE_CONFIG(DocCodeID, StartDate, AggrCompatibility, EnvMediaID, ExpeditionType, ExpCode, SuportType, Priority, AgingDays, CaducityDate, MaxProdDate, ProdMaxSheets, ArchCaducityDate)
                    SELECT @DocCodeID, @StartDate, {0}, emg.EnvMediaGroupID, {1}, '{2}', {3}, {4}, 9999, '{5}', '{6}', {7}, '{8}'
                    FROM RD_ENVELOPE_MEDIA_GROUP emg WITH(NOLOCK)
                    WHERE emg.[Description] = '{9}'", 
                    response.Doc.AggrCompatibility, response.Doc.ExpeditionType, response.Doc.ExpCode,
                    response.Doc.SuportType, response.Doc.Priority, response.Doc.CaducityDate,
                    response.Doc.MaxProdDate, pmSheets, response.Doc.ArchCaducityDate,
                    response.Doc.EnvMediaDesc);

                //RD_DOCCODE_AGGREGATION_COMPATIBILITY
                script += @"
                   DELETE RD_DOCCODE_AGGREGATION_COMPATIBILITY
                 WHERE RefDocCodeID = @DocCodeID OR AggDocCodeID = @DocCodeID";

                foreach(DocCodeScript doc in response.AggDocCodeList) 
                {
                    ex1 = string.IsNullOrEmpty(doc.ExceptionLevel1Code) ? "NULL" : string.Format("'{0}'", doc.ExceptionLevel1Code.Replace("'", "''"));
                    ex2 = string.IsNullOrEmpty(doc.ExceptionLevel2Code) ? "NULL" : string.Format("'{0}'", doc.ExceptionLevel2Code.Replace("'", "''"));
                    ex3 = string.IsNullOrEmpty(doc.ExceptionLevel3Code) ? "NULL" : string.Format("'{0}'", doc.ExceptionLevel3Code.Replace("'", "''"));

                    script += string.Format(@"
                    INSERT INTO RD_DOCCODE_AGGREGATION_COMPATIBILITY(RefDocCodeID, AggDocCodeID)
                    SELECT @DocCodeID, DocCodeID
                    FROM RD_DOCCODE WITH(NOLOCK)
                    WHERE DocLayout = '{0}' AND DocType = '{1}' 
                        AND ISNULL(ExceptionLevel1ID,0) = ISNULL((SELECT TOP 1 ExceptionLevelID FROM RDC_EXCEPTION_LEVEL1 WHERE ExceptionCode = {2}),0) 
                        AND ISNULL(ExceptionLevel2ID,0) = ISNULL((SELECT TOP 1 ExceptionLevelID FROM RDC_EXCEPTION_LEVEL2 WHERE ExceptionCode = {3}),0) 
                        AND ISNULL(ExceptionLevel3ID,0) = ISNULL((SELECT TOP 1 ExceptionLevelID FROM RDC_EXCEPTION_LEVEL3 WHERE ExceptionCode = {4}),0)",
                        doc.DocLayout, doc.DocType, ex1, ex2, ex3);
                }
                script += @"
                    INSERT INTO RD_DOCCODE_AGGREGATION_COMPATIBILITY(RefDocCodeID, AggDocCodeID)
                    SELECT AggDocCodeID, @DocCodeID
                    FROM RD_DOCCODE_AGGREGATION_COMPATIBILITY WITH(NOLOCK)
                    WHERE RefDocCodeID = @DocCodeID AND AggDocCodeID <> @DocCodeID";

                script += @"
                    COMMIT TRANSACTION";

                result.Add(script);
                result.Add(filename);
            }
            return result;
        }
    }
}
