using LinkOnce.Models;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkOnce.Hubs
{
    public class LinkStatusHub : Hub
    {
        IDatabaseContext context;
        public LinkStatusHub()
            : this(new DatabaseContext())
        {
        }
        public LinkStatusHub(IDatabaseContext context)
        {
            this.context = context;
        }

        public void GetLinkStatus(int LinkId)
        {
            var link = context.Links.FirstOrDefault(l => l.LinkId == LinkId);
            if (link != null)
            {
                if (link.IsUsed)
                {
                    Caller.updateStatus(new { Status = true, DateRetrieved = link.DateUsed, 
                        Message = "Retrieved " + link.DateUsed.ToLongDateString() + " " + link.DateUsed.ToLongTimeString() });
                }
                else
                {
                    Caller.updateStatus(new { Status = true, Message = "Not retrieved" });
                    Caller.scheduleRecheck();
                }
            }
            else
            {
                Caller.updateStatus(new { Status = false, Message = "Link not found" });
                Caller.scheduleRecheck();
            }
        }
    }
}