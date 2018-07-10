using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MySystem.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateGreaterThanAttribute : ValidationAttribute, IClientValidatable
    {

        private string DateToCompareFieldName { get; set; }

        public DateGreaterThanAttribute(string dateToCompareFieldName)
        {
            DateToCompareFieldName = dateToCompareFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime laterDate = (DateTime)value;

            DateTime earlierDate = (DateTime)validationContext.ObjectType.GetProperty(DateToCompareFieldName).GetValue(validationContext.ObjectInstance, null);

            if (laterDate > earlierDate)            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("A Data fim deve ser maior que a data de inicio.");
            }
        }

        //esse método retorna as validações que serão utilizadas no lado cliente
        IEnumerable<ModelClientValidationRule> IClientValidatable.GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule
            {
                ErrorMessage = "A Data fim deve ser maior que a data de inicio.",
                ValidationType = "dategreaterthan"
            };

            clientValidationRule.ValidationParameters.Add("datetocomparefieldname", DateToCompareFieldName);

            return new[] { clientValidationRule };
        }
    }
}
