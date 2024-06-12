using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U3ActRegistroDeActividadesMaui.Models.DTOs;

namespace U3ActRegistroDeActividadesMaui.Models.Validators
{
    public class ActividadDTOValidator : AbstractValidator<ActividadDTO>
    {
        public ActividadDTOValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().NotNull().WithMessage("El título no debe ser núlo o vacío");
            RuleFor(x => x.Descripcion).NotEmpty().NotNull().WithMessage("La descripción no debe ser núlo o vacío");
            RuleFor(x => x.Imagen).NotEmpty().NotNull().WithMessage("La imagen no debe ser nula o vacía");
        }
    }
}
