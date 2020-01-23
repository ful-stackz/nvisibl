using System;

namespace Nvisibl.DataLibrary.Repositories
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class RepositoryAttribute : Attribute
    {
    }
}
