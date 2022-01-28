using FluentValidation;
using UnluCo.Egitim.API.Ikinci.Hafta.Models;
using UnluCo.Egitim.API.Ikinci.Hafta.Models.Dto;

namespace UnluCo.Egitim.API.Ikinci.Hafta.Validators
{
    public class StudentValidations: AbstractValidator<StudentDto>
    {
        public StudentValidations(ProcessType processType)
        {
            switch (processType)
            {
                case ProcessType.Delete:
                    break;
                case ProcessType.Add:
                case ProcessType.Update:
                    RuleFor(x => x.Name).NotEmpty().NotEqual("string");
                    RuleFor(x => x.Surname).NotEmpty().NotEqual("string");
                    RuleFor(x => x.SchoolName).NotEmpty().NotEqual("string");
                    RuleFor(x => x.No).NotNull().NotEqual(default(int));
                    break;
            }            
        }
    }
}
