using MedicalLaboratoryNumber20WebAPI.Models.Entities;

namespace MedicalLaboratoryNumber20WebAPI.Models.ResponseModels
{
    public class ResponseService
    {
        public int Code;
        public string Title;
        public decimal PriceInRubles;

        public ResponseService(Service service)
        {
            Code = service.Code;
            Title = service.ServiceName;
            PriceInRubles = service.PriceInRubles;
        }
    }
}