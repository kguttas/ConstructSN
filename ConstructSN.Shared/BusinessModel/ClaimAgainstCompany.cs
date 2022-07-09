using ConstructSN.Shared.InfrastructureModel;
using ConstructSN.Shared.Validation;
using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructSN.Shared.BusinessModel
{
    public class ClaimAgainstCompany : EntityBase
    {
        public ClaimAgainstCompany()
        {

        }

        public string? RUT { get; set; }

        [Required(ErrorMessage = "El nombre de la empresa o persona que se comprometió a realizar el trabajo, es requerido.")]
        public string? NameCompany { get; set; }

        [Required(ErrorMessage = "La fecha en la cual se realizó el acuerdo o contrato, es requerida.")]
        public DateTime? DateCommitment { get; set; }

        [Required(ErrorMessage = "La descripción del problema por el cual reclama, es requerido.")]
        public string? ProblemDescription { get; set; }

    }
}
