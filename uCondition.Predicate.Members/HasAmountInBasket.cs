using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;
using uCondition.Models;

namespace uCondition.Predicates.Members
{
    public class HasAmountInBasket : uCondition.Models.Predicate
    {
        public HasAmountInBasket()
        {
            Name = "More than amount in basket (£)";
            Alias = "HasXAmountInBasket";
            Icon = "icon-shopping-basket-alt-2";
            Category = "ECommerce";
            Fields = new List<EditableProperty>
            {
                new EditableProperty("Amount (£)", "amount", "Textstring")
            };
        }

        public override bool Validate(IFieldValues fieldValues)
        {
            var minBasketAmount = fieldValues.GetValue<decimal>("amount");
            //get the current basket and check the total amount
            var basketAmount = 49.99m;
            return basketAmount >= minBasketAmount;
        }
    }

    public class HasPreviousSpentOnStore : Predicate
    {
        public HasPreviousSpentOnStore()
        {
            Name = "Previously spent amount in store (£)";
            Alias = "HasSpentAmountInStore";
            Icon = "icon-bill-dollar";
            Category = "ECommerce";
            Fields = new List<EditableProperty>
            {
                new EditableProperty("Amount (£)", "amount", "Textstring")
            };
        }

        public override bool Validate(IFieldValues fieldValues)
        {
            return true;
        }
    }
}
