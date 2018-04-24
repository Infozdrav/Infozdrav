using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infozdrav.Web.Data
{
    public enum WriteOffReason
    {
        BadLot,
        Expired,
        AllUnitsUsed,
        Other
    }
}