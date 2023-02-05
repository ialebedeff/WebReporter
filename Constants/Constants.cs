using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AppConstants
{
    public static class Constants
    {
        public static DatabaseCommands DatabaseCommands { get; set; } = new();
    }
}