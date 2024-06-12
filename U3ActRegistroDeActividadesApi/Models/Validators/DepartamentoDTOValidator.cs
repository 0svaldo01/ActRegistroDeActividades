using FluentValidation;
using U3ActRegistroDeActividadesApi.Models.DTOs;

namespace U3ActRegistroDeActividadesApi.Models.Validators
{
    public class DepartamentoDTOValidator : AbstractValidator<DepartamentoDTO>
    {
        public DepartamentoDTOValidator()
        {
            RuleFor(x => x.Nombre).NotNull().NotEmpty().WithMessage("El nombre del departamento no puede ser nulo o vacio");
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("El nombre de usuario no puede ser nulo o vacio");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("La contraseña no puede ser nula o vacia");
        }
    }

}
