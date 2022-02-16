using MedicalLaboratoryNumber20WebAPI.Models.Entities;
using System;

namespace MedicalLaboratoryNumber20WebAPI.Models.ResponseModels
{
    public class ResponseResearchResult
    {
        public string ServiceTitle;
        public string Result;
        public DateTime DateOfResearch;

        public ResponseResearchResult(BloodServiceOfUser bloodServiceOfUser)
        {
            ServiceTitle = bloodServiceOfUser.Service.ServiceName;
            Result = bloodServiceOfUser.Result;
            DateOfResearch = bloodServiceOfUser.FinishedDateTime;
        }
    }
}