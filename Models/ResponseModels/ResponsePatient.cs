using MedicalLaboratoryNumber20WebAPI.Models.Entities;
using System;

namespace MedicalLaboratoryNumber20WebAPI.Models.ResponseModels
{
    public class ResponsePatient
    {
        public string FullName;
        public DateTime BirthDate;
        public string PassportNumber;
        public string PassportSeries;
        public string SecurityNumber;
        public string Phone;
        public string Email;

        public ResponsePatient(Patient patient)
        {
            FullName = patient.PatientFullName;
            BirthDate = patient.BirthDate;
            PassportNumber = patient.PassportNumber;
            PassportSeries = patient.PassportSeries;
            SecurityNumber = patient.SecurityNumber;
            Phone = patient.PatientPhone;
            Email = patient.PatientEmail;
        }
    }
}