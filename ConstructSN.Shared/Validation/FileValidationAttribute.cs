using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructSN.Shared.Validation
{
    public class FileValidationAttribute : ValidationAttribute
    {
        public FileValidationAttribute(string[] allowedExtensions)
        {
            AllowedExtensions = allowedExtensions;
        }

        private string[] AllowedExtensions { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                foreach (IBrowserFile file in (IBrowserFile[])value)
                {
                    var extension = System.IO.Path.GetExtension(file.Name);

                    if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        return new ValidationResult($"File must have one of the following extensions: {string.Join(", ", AllowedExtensions)}.", new[] { validationContext.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
