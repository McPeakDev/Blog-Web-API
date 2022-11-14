using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models {
    /// <summary>
    /// Defines the needed parameters to login to the Web API.
    /// </summary>
    public class Login {

        /// <summary>
        /// The attempted user's username.
        /// </summary>
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        /// <summary>
        /// The attempted user's passwrd.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
