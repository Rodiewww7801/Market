﻿using FluentValidation;
using Market.Data.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Web.Validators
{
    public class OrderDetailsValidator: AbstractValidator<OrderDetails>
    {
        public OrderDetailsValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name string must not be empty");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email string must not be empty");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email entered incorrectly");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Adress string must not be empty");
            RuleFor(x => x.City).NotEmpty().WithMessage("City string must not be empty");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country string must not be empty");
        }
    }
}
