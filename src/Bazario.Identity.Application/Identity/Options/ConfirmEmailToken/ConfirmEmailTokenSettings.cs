﻿using Bazario.AspNetCore.Shared.Options;
using System.ComponentModel.DataAnnotations;

namespace Bazario.Identity.Application.Identity.Options.ConfirmEmailToken
{
    public sealed class ConfirmEmailTokenSettings : IAppOptions
    {
        public const string SectionName = nameof(ConfirmEmailTokenSettings);

        [Required]
        [Range(1, 21)]
        public int ExpirationTimeInDays { get; set; }

        // Note: will be used in ResendConfirmationEmailCommand
        [Required]
        [Range(1, 20)]
        public int ActiveTokensLimit { get; set; }
    }
}
