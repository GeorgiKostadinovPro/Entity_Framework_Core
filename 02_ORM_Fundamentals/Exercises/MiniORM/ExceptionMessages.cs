using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniORM;

internal static class ExceptionMessages
{
    public static string EntityNullException = "Entity cannot be null!";

    public static string InvalidEntitiesException = "{0} Invalid entities found in {1}!";
}
