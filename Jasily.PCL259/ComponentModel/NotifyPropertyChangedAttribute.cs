using System;
using Jasily.Interfaces;

namespace Jasily.ComponentModel
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotifyPropertyChangedAttribute : Attribute, IOrderable
    {
        /// <summary>
        /// order by asc
        /// </summary>
        public int Order { get; set; }

        public int GetOrderCode() => this.Order;
    }
}