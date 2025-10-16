using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NurseRecordingSystem.Model.DTO.NurseServicesDTOs.PatientRecordsDTOs
{
    // DTO for the comprehensive view of a single patient record (nsp_ViewPatientRecord)
    public class ViewPatientRecordResponseDTO
    {
        [Required]
        public int PatientRecordId { get; set; }

        [Required]
        [MaxLength(50)]
        public string NursingDiagnosis { get; set; } = string.Empty;

        [Required]
        public string NursingIntervention { get; set; } = string.Empty;

        [Required]
        public int NurseId { get; set; }

        // Audit Fields
        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedOn { get; set; }

        [MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
    
}