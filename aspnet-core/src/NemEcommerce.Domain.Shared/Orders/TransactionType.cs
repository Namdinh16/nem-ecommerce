using System;
using System.Collections.Generic;
using System.Text;

namespace NemEcommerce.Orders
{
    public enum TransactionType
    {
        ConfirmOrder,
        StartProcessing,
        FinishOrder,
        CancelOrder
    }
}
