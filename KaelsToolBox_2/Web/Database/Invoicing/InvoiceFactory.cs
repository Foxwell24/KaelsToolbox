using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Web.Database.Invoicing;

public class InvoiceFactory(Client client, Provider provider, BillableItem[] items) 
{
    public Invoice Output { get; private set; } = new();

    private Client _Client = client;
    private Provider _Provider = provider;
    private BillableItem[] _Items = items;


}
