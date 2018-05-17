using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Data
{
    public enum WriteOffReason
    {
        [Display(Name = "Slab lot")]
        BadLot,
        [Display(Name = "Izdelek je pretekel")]
        Expired,
        [Display(Name = "Izdelek je bil uporabljen")]
        AllUnitsUsed,
        [Display(Name = "Drug razlog")]
        Other
    }
}