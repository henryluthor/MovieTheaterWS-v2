using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace MovieTheaterWS_v2.Classes
{
    public class UserRegistrationBaseDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // The exclamation mark after null is called null-forgiving operator
        // It is used to suppress compiler warnings about potential null values when Nullable Reference Types are enabled.
        // It says to the compiler "I know this value might look like it could be null, but trust me, it won't be null when it's actually used"
        // Think of it as a "shut up, compiler" or "trust me, bro" operator.
        //public string Email { get; set; } = null!;
        // Now updated to the folloing line
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
