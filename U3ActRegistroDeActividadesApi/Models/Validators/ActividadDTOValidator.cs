using FluentValidation;
using U3ActRegistroDeActividadesApi.Models.DTOs;

namespace U3ActRegistroDeActividadesApi.Models.Validators
{
    public class ActividadDTOValidator : AbstractValidator<ActividadDTO>
    {
        public ActividadDTOValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().NotNull().WithMessage("El título no debe ser núlo o vacío");
            RuleFor(x => x.Descripcion).NotEmpty().NotNull().WithMessage("La descripción no debe ser núlo o vacío");
            RuleFor(x => x.Imagen)
                .Must(IsValidExtensionImage).WithMessage("Seleccione una imagen en formato png,jpg o jpeg")
                .NotEmpty().NotNull().WithMessage("La imagen no debe ser nula o vacía");
        }

        private bool IsValidExtensionImage(IFormFile imagen)
        {
            var result = imagen.ContentType == "image/png" || imagen.ContentType == "image/jpg"
              || imagen.ContentType == "image/jpeg";
            return result;
        }
    }
}
