using System.Collections.Generic;

namespace Infozdrav.Web.Models.Manage
{
    public class UserEditViewModel : UserViewModel
    {
        public List<RoleViewModel> Roles { get; set; }

    }
}