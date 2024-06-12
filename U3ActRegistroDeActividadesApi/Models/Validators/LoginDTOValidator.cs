using FluentValidation;
using U3ActRegistroDeActividadesApi.Models.DTOs;

namespace U3ActRegistroDeActividadesApi.Models.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Username).NotEmpty().NotNull().WithMessage("Ingresa el nombre del departamento");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Ingresa una contraseña");
        }
    }
}
