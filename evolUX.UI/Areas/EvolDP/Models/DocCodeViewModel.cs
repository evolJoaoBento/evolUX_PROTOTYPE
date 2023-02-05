using Shared.Models.Areas.evolDP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace evolUX_dev.Areas.EvolDP.Models
{
    public class DocCodeViewModel
    {
        public int DocCodeId { get; set; }
        public string DocLayout { get; set; }
        public string DocSubtype { get; set; }
        public string DocDescription { get; set; }
        public ExceptionLevel1 DocExceptionLevel1 { get; set; }
        public ExceptionLevel2 DocExceptionLevel2 { get; set; }
        public ExceptionLevel3 DocExceptionLevel3 { get; set; }
        public EnvMediaGroupViewModel DocEnvMediaGroup { get; set; }

        [Required]
        public int DocStartDate { get; set; }
        public string DocEnvelopeGroup { get; set; }
        public string DocAggregation { get; set; }
        public int DocAggregationValue { get; set; }
        public string DocAggregationCompatibility { get; set; }
        public int DocPriority { get; set; }
        public int DocProdMaxSheets { get; set; }
        public Company DocExpeditionCompany { get; set; }
        public ExpeditionType DocExpeditionType { get; set; }
        public string DocServiceTask { get; set; }
        public string DocFinishing { get; set; }
        public string DocCaducityDate { get; set; }
        public string DocMaxProdDate { get; set; }
        public string DocArchCaducityDate { get; set; }
        public string DocArchive { get; set; }
        public string DocEmail { get; set; }
        public string DocEmailHide { get; set; }
        public string DocElectronicFormat { get; set; }
        public string DocDigitalSignPDF { get; set; }
        public string DocElectronicFormatHide { get; set; }
        public string DocSuportType { get; set; }
        public string DocAging { get; set; }

        public bool DocCheckStatus { get; set; }
        public List<DropListItemViewModel> DocExceptionLevel1List { get; set; }
        public List<DropListItemViewModel> DocExceptionLevel2List { get; set; }
        public List<DropListItemViewModel> DocExceptionLevel3List { get; set; }
        public List<DropListItemViewModel> DocEnvMediaGroupList { get; set; }
        public List<DropListItemViewModel> DocAggregationList { get; set; }
        public List<DropListItemViewModel> DocExpeditionCompanyList { get; set; }
        public List<DropListItemViewModel> DocExpeditionTypeList { get; set; }
        public List<DropListItemViewModel> DocServiceTaskList { get; set; }
        public List<DropListItemViewModel> DocFinishingList { get; set; } //Duvidas
        public List<DropListItemViewModel> DocArchiveList { get; set; } //Duvidas
        public List<DropListItemViewModel> DocEmailList { get; set; }
        public List<DropListItemViewModel> DocEmailHideList { get; set; }
        public List<DropListItemViewModel> DocElectronicFormatList { get; set; }
        public List<DropListItemViewModel> DocElectronicFormatHideList { get; set; }




    }
}
