using System;

namespace Crane.Core.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]  
    public class HiddenCommandAttribute : Attribute
    {
         
    }
}