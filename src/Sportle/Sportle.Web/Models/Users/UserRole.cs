using System.ComponentModel.DataAnnotations;

namespace Sportle.Web.Models.Users
{
    /// <summary>
    /// The roles that can be assigned to a user.
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Can access all administration features.
        /// </summary>
        [Display(Name = "Administrator")]
        Administrator
    }
}
